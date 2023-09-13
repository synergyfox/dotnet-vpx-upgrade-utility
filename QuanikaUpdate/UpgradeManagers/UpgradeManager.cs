using QuanikaUpdate.Helpers;
using QuanikaUpdate.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using VPSetup.Database;
using VPSetup.Helpers;

namespace QuanikaUpdate.UpgradeManagers
{
    internal abstract class UpgradeManager
    {
        protected DAL Database { get; private set; }
        protected Window Window { get; private set; }
        protected MainWindow MainWindow { get; private set; }
        protected string Version { get; private set; }
        protected UpgradeManager()
        {
            Database = new DAL();
        }
        internal async Task Manage(string version, Window gui)
        {
            try
            {
                Version = version;
                MainWindow = (MainWindow)gui;
                Window = gui;

                var directories = GetDirectories(version);
                foreach (var patchDirectory in directories)
                {
                    await ManageSinglePatch(patchDirectory);
                }
            }
            catch (Exception ex)
            {
                UpdateErrorLabel(ex.Message);
            }
        }

        protected IEnumerable<string> GetDirectories(string version)
        {
            try
            {

                string currentPatchLocation = Path.GetDirectoryName(GetCurrentDirectoryPath());

                var directories = Directory.GetDirectories(Path.Combine(currentPatchLocation, "Updates"));

                var directoryFiles = directories.Select(Path.GetFullPath);

                var directoriesToOperate = new List<string>();

                foreach (var item in directoryFiles)
                {
                    if (Helper.isSmallVersion(version, item))
                    {
                        directoriesToOperate.Add(item);
                    }
                }
                return directories;
            }
            catch (Exception ex)
            {
                UpdateErrorLabel($"Error: {ex.Message}");
                throw;
            }

        }
        protected IEnumerable<FileDetails> GetDirectoryDetails(string directory)
        {
            try
            {
                return Directory.GetDirectories(directory)
                .Select(x =>
                new FileDetails()
                {
                    Path = Path.GetFullPath(x),
                    Name = Path.GetFileName(x)
                });
            }
            catch (Exception ex)
            {
                UpdateErrorLabel($"Error: {ex.Message}");
                throw;
            }

        }
        private protected string GetCurrentDirectoryPath()
        {
            try
            {
                return Assembly.GetExecutingAssembly().Location;
            }
            catch (Exception ex)
            {
                UpdateErrorLabel($"Error: {ex.Message}");
                throw;
            }

        }
        protected void CopyWebFiles(string sourceDir, string webName)
        {
            try
            {
                var site = PathsHelper.GetWebInfoAndPath(webName);

                if (string.IsNullOrEmpty(site.Item1) || site.Item2 is null)
                {
                    return;
                }
                if (site.Item2.State is Microsoft.Web.Administration.ObjectState.Started)
                {
                    //var result = MessageBox.Show($"{webName} is running we are gonna to stop it. Are you sure to stop it", "Warning", MessageBoxButton.YesNoCancel);
                    //if (result is MessageBoxResult.Yes)
                    //{
                    site.Item2.Stop();
                    CopyDirectory(sourceDir, site.Item1);
                    site.Item2.Start();
                    //}
                }
            }
            catch (Exception ex)
            {
                UpdateErrorLabel($"Error: {ex.Message}");
            }

        }
        protected bool CopyDirectory(string sourceDir, string destDir)
        {
            try
            {
                var sourceDirectoryInfo = new DirectoryInfo(sourceDir);

                if (!Directory.Exists(destDir))
                {
                    Directory.CreateDirectory(destDir);
                    UpdateLabel($"Created directory: {destDir}");
                }

                foreach (FileInfo file in sourceDirectoryInfo.GetFiles())
                {
                    string destFileName = Path.Combine(destDir, file.Name);
                    file.CopyTo(destFileName, true);
                    UpdateLabel($"Copied file: {file.Name}");
                }

                // Copy subdirectories recursively
                foreach (var subDir in sourceDirectoryInfo.GetDirectories())
                {
                    string destSubDir = Path.Combine(destDir, subDir.Name);
                    CopyDirectory(subDir.FullName, destSubDir);
                }
                return true;
            }
            catch (Exception ex)
            {
                UpdateErrorLabel($"Error: {ex.Message}");
                return false;
            }
        }
        protected async Task ExecuteSqlFiles(FileDetails fileDetails)
        {
            await ExecuteSqlScriptsInDirectory(fileDetails.Path);
        }
        private async Task ExecuteSqlScriptsInDirectory(string directoryPath)
        {
            try
            {
                string[] sqlFiles = Directory.GetFiles(directoryPath, "*.sql");

                UpdateLabel("Started executing sql files");

                if (sqlFiles.Length == 0)
                {
                    UpdateLabel("No SQL files found in the directory.");
                    return;
                }

                var tasks = new List<Task>();
                Database.Connection.Open();
                foreach (string sqlFilePath in sqlFiles)
                {
                    tasks.Add(Task.Run(() =>
                    {
                        try
                        {
                            UpdateLabel($"Executing sql file {sqlFilePath}");

                            string sqlScript = File.ReadAllText(sqlFilePath);

                            _ = RunSqlScript(sqlScript);

                            UpdateLabel($"SQL script '{Path.GetFileName(sqlFilePath)}' executed successfully.");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error executing SQL script in '{sqlFilePath}': {ex.Message}");
                            UpdateLabel($"Error executing SQL script in '{Path.GetFileName(sqlFilePath)}': {ex.Message}");
                        }
                    }));
                    await Task.WhenAll(tasks);
                }
            }
            catch (Exception ex)
            {
                UpdateLabel("Error: " + ex.Message);
            }
            finally
            {
                Database.Connection.Close();
            }
        }
        public bool RunSqlScript(string script)
        {
            try
            {
                if (string.IsNullOrEmpty(script))
                {
                    return false;
                }

                // split script on GO command
                IEnumerable<string> commandStrings = Regex.Split(script, @"^\s*GO\s*$",
                                           RegexOptions.Multiline | RegexOptions.IgnoreCase);


                foreach (string commandString in commandStrings)
                {
                    if (commandString.Trim() != "")
                    {
                        using (var command = new SqlCommand(commandString, Database.Connection))
                        {
                            try
                            {
                                command.ExecuteNonQuery();
                            }
                            catch (SqlException ex)
                            {
                                Helper.writeLog(ex, " 9024S ");
                                string spError = commandString.Length > 100 ? commandString.Substring(0, 100) + " ...\n..." : commandString;

                                return false;
                            }
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Helper.writeLog(ex);
                throw;
            }
        }
        protected void UpdateLabel(string message)
        {
            MainWindow.UpdatePbLabel(message);
        }
        protected void UpdateErrorLabel(string message)
        {
            MainWindow.UpdatePbLabel(message);
        }
        protected abstract Task ManageSinglePatch(string patchDirectory);
    }
}
