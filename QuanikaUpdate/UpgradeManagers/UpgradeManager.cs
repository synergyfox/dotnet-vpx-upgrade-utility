using QuanikaUpdate.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
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
        protected UpgradeManager()
        {
            Database = new DAL();
        }
        protected IEnumerable<string> GetDirectories(string version)
        {
            try
            {
                var sourceDirecotory = GetCurrentDirectoryPath();

                string loc = Path.GetDirectoryName(sourceDirecotory);

                var directories = Directory.GetDirectories(Path.Combine(loc, "Updates"));

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
        internal async Task Manage(string version, Window gui)
        {
            try
            {
                MainWindow = (MainWindow)gui;
                Window = gui;

                var directories = GetDirectories(version);
                foreach (var patchDirectory in directories)
                {
                    await Task.Run(() => ManageSinglePatch(patchDirectory));
                }
            }
            catch (Exception ex)
            {
                UpdateErrorLabel(ex.Message);
            }
        }
        protected void CopyDirectory(string sourceDir, string destDir)
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
                    UpdateLabel($"Copied file: {file.FullName} to {destFileName}");
                }

                // Copy subdirectories recursively
                foreach (var subDir in sourceDirectoryInfo.GetDirectories())
                {
                    string destSubDir = Path.Combine(destDir, subDir.Name);
                    CopyDirectory(subDir.FullName, destSubDir);
                }
            }
            catch (Exception ex)
            {
                UpdateErrorLabel($"Error: {ex.Message}");
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

                using (SqlConnection connection = Database.Connection)
                {
                    connection.Open();
                    var tasks = new List<Task>();

                    foreach (string sqlFilePath in sqlFiles)
                    {
                        tasks.Add(Task.Run(() =>
                        {
                            try
                            {
                                UpdateLabel($"Executing sql file {sqlFilePath}");
                                string sqlScript = File.ReadAllText(sqlFilePath);
                                sqlScript = sqlScript.Replace("GO", " ");
                                using (SqlCommand command = new SqlCommand(sqlScript, connection))
                                {
                                    command.CommandType = CommandType.Text;
                                    command.ExecuteNonQuery();
                                }

                                UpdateLabel($"SQL script in '{Path.GetFileName(sqlFilePath)}' executed successfully.");
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Error executing SQL script in '{sqlFilePath}': {ex.Message}");
                                UpdateLabel($"Error executing SQL script in '{Path.GetFileName(sqlFilePath)}': {ex.Message}");
                            }
                        }));
                    }
                    await Task.WhenAll(tasks);
                }
            }
            catch (Exception ex)
            {
                UpdateLabel("Error: " + ex.Message);
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
