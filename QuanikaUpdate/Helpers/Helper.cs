
using Microsoft.Win32;
using QuanikaUpdate;
using QuanikaUpdate.BLL;
using QuanikaUpdate.Models;
using QuanikaUpdate.Wins;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Sql;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using VPSetup.Database;
using VPSetup.Extensions;

namespace VPSetup.Helpers
{
    public static class Helper
    {

        private static string LOCAL_MACHINE = "SOFTWARE\\WOW6432Node\\Quanika", CURRENT_USER = "SOFTWARE\\Quanika";
        public static string Encrypt(string input, string key = "lkaju-x3h4-f5f45")
        {
            try
            {
                byte[] inputArray = UTF8Encoding.UTF8.GetBytes(input);
                TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
                tripleDES.Key = UTF8Encoding.UTF8.GetBytes(key);
                tripleDES.Mode = CipherMode.ECB;
                tripleDES.Padding = PaddingMode.PKCS7;
                ICryptoTransform cTransform = tripleDES.CreateEncryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
                tripleDES.Clear();
                return Convert.ToBase64String(resultArray, 0, resultArray.Length);
            }
            catch (Exception ex)
            {
                Helper.writeLog(ex);
                return string.Empty;
            }
        }
        public static string Decrypt(string input, string key = "lkaju-x3h4-f5f45")
        {
            try
            {
                byte[] inputArray = Convert.FromBase64String(input);
                TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
                tripleDES.Key = UTF8Encoding.UTF8.GetBytes(key);
                tripleDES.Mode = CipherMode.ECB;
                tripleDES.Padding = PaddingMode.PKCS7;
                ICryptoTransform cTransform = tripleDES.CreateDecryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
                tripleDES.Clear();
                return UTF8Encoding.UTF8.GetString(resultArray);
            }
            catch (Exception ex)
            {
                Helper.writeLog(ex);
                return string.Empty;
            }

        }
        public static void showBox(string errorType, string msg)
        {

            switch (errorType)
            {
                case "error": MessageBox.Show(msg, "Error", MessageBoxButton.OK, System.Windows.MessageBoxImage.Error); break;
                case "ok": MessageBox.Show(msg, "Success", MessageBoxButton.OK, System.Windows.MessageBoxImage.Information); break;
                case "info": MessageBox.Show(msg, "Info", MessageBoxButton.OK, System.Windows.MessageBoxImage.Information); break;
                default: MessageBox.Show(msg); break;
            }
        }

        public static void AddValue(string key, string value)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(
                                       System.Reflection.Assembly.GetExecutingAssembly().Location);
            config.AppSettings.Settings.Remove(key);
            config.AppSettings.Settings.Add(key, value);
            config.Save(ConfigurationSaveMode.Minimal);
        }
        public static string getAppSetting(string key)
        {
            try
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(
                                        System.Reflection.Assembly.GetExecutingAssembly().Location);
                return config.AppSettings.Settings[key].Value;
            }
            catch (Exception ex)
            {
                Helper.writeLog(ex);
                return "";
            }
        }
        public static Version GetVersion()
        {
            try
            {
                Thread.Sleep(1000);

                using (RegistryKey key = Registry.LocalMachine.OpenSubKey("Software\\Wow6432Node\\Quanika"))
                {
                    if (key != null)
                    {
                        Object o = key.GetValue("version");
                        if (o != null)
                        {
                            Version version = new Version(o as String);

                            CConfig.Setting.version = version.ToString();
                            return version;
                        }
                    }


                }

            }
            catch (Exception ex)
            {
                writeLog(ex);
            }
            return null;
        }
        public static void GetDbCredentials()
        {
            try
            {
                using (RegistryKey local_key = Registry.LocalMachine.OpenSubKey(LOCAL_MACHINE))
                {
                    if (local_key != null)
                    {

                        CConfig.Setting.server = local_key.GetValueNames().Contains("server") ? local_key.GetValue("server").ToString() : "";
                        CConfig.Setting.database = local_key.GetValueNames().Contains("database") ? local_key.GetValue("database").ToString() : "";
                        CConfig.Setting.username = local_key.GetValueNames().Contains("username") ? local_key.GetValue("username").ToString() : "";
                        CConfig.Setting.password = local_key.GetValueNames().Contains("password") ? local_key.GetValue("password").ToString() : "";
                    }
                    else
                    { // If only application is installed Then local machine wont exist
                        using (RegistryKey current_user_key = Registry.CurrentUser.OpenSubKey(CURRENT_USER))
                        {
                            if (current_user_key != null)
                            {
                                CConfig.Setting.server = current_user_key.GetValueNames().Contains("server") ? current_user_key.GetValue("server").ToString() : "";
                                CConfig.Setting.database = current_user_key.GetValueNames().Contains("database") ? current_user_key.GetValue("database").ToString() : "";
                                CConfig.Setting.username = current_user_key.GetValueNames().Contains("username") ? current_user_key.GetValue("username").ToString() : "";
                                CConfig.Setting.password = current_user_key.GetValueNames().Contains("password") ? current_user_key.GetValue("password").ToString() : "";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                writeLog(ex);
            }

        }
        public static string GetLocalIPAddress()
        {
            try
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        return ip.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                writeLog(ex);
            }
            return string.Empty;
        }
        public static bool CheckDotNet45OrPlus()
        {
            bool result = false;
            try
            {
                using (RegistryKey ndpKey = RegistryExtensions.OpenBaseKey(RegistryHive.LocalMachine, RegistryExtensions.RegistryHiveType.X86).OpenSubKey("SOFTWARE\\Microsoft\\NET Framework Setup\\NDP\\v4\\Full\\"))
                {
                    int releaseKey = Convert.ToInt32(ndpKey.GetValue("Release"));
                    if (true)
                    {
                        string version = CheckFor45DotVersion(releaseKey);
                        if (!string.IsNullOrEmpty(version))
                            result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.writeLog(ex);
            }
            return result;
        }
        private static string CheckFor45DotVersion(int releaseKey)
        {
            if (releaseKey >= 461808)
            {
                return "4.7.2 or later";
            }
            if (releaseKey >= 461308)
            {
                return "4.7.1 or later";
            }
            if (releaseKey >= 460798)
            {
                return "4.7 or later";
            }
            if (releaseKey >= 394802)
            {
                return "4.6.2 or later";
            }
            if (releaseKey >= 394254)
            {
                return "4.6.1 or later";
            }
            if (releaseKey >= 393295)
            {
                return "4.6 or later";
            }
            if (releaseKey >= 393273)
            {
                return "4.6 RC or later";
            }
            if ((releaseKey >= 379893))
            {
                return "4.5.2 or later";
            }
            if ((releaseKey >= 378675))
            {
                return "4.5.1 or later";
            }
            if ((releaseKey >= 378389))
            {
                return "4.5 or later";
            }
            return "";
        }
        public static bool CheckDotNet35()
        {
            RegistryKey installed_versions = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\v3.5");
            if (installed_versions != null)
            {
                string version = installed_versions.GetValue("Version").ToString();
                if (!string.IsNullOrEmpty(version))
                    return true;
            }

            return false;
        }
        public static void Run(string path, string args = "")
        {
            try
            {
                Process p = new Process();
                p.StartInfo.FileName = path;
                p.StartInfo.UseShellExecute = false;
                if (!string.IsNullOrEmpty(args))
                    p.StartInfo.Arguments = args;
                p.StartInfo.RedirectStandardOutput = true;
                p.Start();
                string output = p.StandardOutput.ReadToEnd();
                p.WaitForExit();
                p.Close();
            }
            catch (Exception ex)
            {
                Helper.showBox("error", ex.Message.ToString());
            }
        }
        static void OutputHandler(object sendingProcess, DataReceivedEventArgs outLine)
        {
            //* Do your stuff with the output (write to console/log/StringBuilder)
            Console.WriteLine(outLine.Data);
        }
        public static string getQueryFile(string name)
        {
            try
            {
                return File.ReadAllText(name);
            }
            catch (Exception ex)
            {

                Helper.writeLog(ex);
                return null;
            }
        }
        public static bool isSQLServerInstalled()
        {
            string regPath = @"SOFTWARE\MICROSOFT\Microsoft SQL Server";
            if (OSHelper.Is64BitOperatingSystem())
                regPath = @"SOFTWARE\Wow6432Node\Microsoft\Microsoft SQL Server";

            RegistryKey RK = Registry.LocalMachine.OpenSubKey(regPath, false);

            return RK != null;
        }
        public static void writeLog(Exception ex, string code = "")
        {
            try
            {
                string loc = Assembly.GetEntryAssembly().Location;
                string filePath = String.Concat(loc, "_log.txt");
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine("Code: " + code + Environment.NewLine + " Message :" + ex.Message + Environment.NewLine + "StackTrace :" + ex.StackTrace +
                       "" + Environment.NewLine + "Date :" + DateTime.Now.ToString());
                    writer.WriteLine(Environment.NewLine + "-----------------------------------------------------------------------------" + Environment.NewLine);
                }

            }
            catch (Exception)
            {
                return;
            }
        }
        public static void writeLog(string msg)
        {
            try
            {
                string loc = Assembly.GetEntryAssembly().Location;
                string filePath = String.Concat(loc, "temp_log.txt");
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine(Environment.NewLine + "-----------------------------------------------------------------------------" + Environment.NewLine);
                    writer.WriteLine(msg);
                    writer.WriteLine(Environment.NewLine + "-----------------------------------------------------------------------------" + Environment.NewLine);
                }

            }
            catch (Exception)
            {
                return;
            }
        }
        //public static string Get_DX_Config_File()
        //{
        //    if (OSHelper.Is64BitOperatingSystem())
        //        return _Const.DX_INSTALLED_CONFIG_PATH_x64;

        //    return _Const.DX_INSTALLED_CONFIG_PATH_X86;
        //}
        //public static string Get_App_Config_File()
        //{
        //    if (OSHelper.Is64BitOperatingSystem())
        //        return _Const.APP_INSTALLED_EXE_PATH_X64;

        //    return _Const.APP_INSTALLED_EXE_PATH_X86;
        //}
        public static List<string> GetSqlServerInsances()
        {
            List<string> servers = new List<string>();
            try
            {
                DataTable dt = SqlDataSourceEnumerator.Instance.GetDataSources();
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if ((dt.Rows[i]["InstanceName"] as string) != null)
                            servers.Add(dt.Rows[i]["ServerName"] + "\\" + dt.Rows[i]["InstanceName"]);
                        else
                            servers.Add(dt.Rows[i]["ServerName"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.writeLog(ex);
            }
            return servers;
        }
        public static void CenterWindowOnScreen(Window yourParentWindow)
        {
            double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            double windowWidth = yourParentWindow.Width;
            double windowHeight = yourParentWindow.Height;
            yourParentWindow.Left = (screenWidth / 2) - (windowWidth / 2);
            yourParentWindow.Top = (screenHeight / 2) - (windowHeight / 2);
        }
        public static void RunMsiSetup(string path)
        {
            try
            {
                Process p = new Process();
                p.StartInfo.FileName = path;
                p.StartInfo.UseShellExecute = true;
                p.Start();
                p.WaitForExit();
            }
            catch (Exception ex)
            {
                Helper.showBox("error", ex.Message.ToString());
            }
        }
        public static bool IsApplictionInstalled(string p_name, out string guid)
        {
            string displayName; guid = "";
            RegistryKey key;

            // search in: CurrentUser
            key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall");
            foreach (String keyName in key.GetSubKeyNames())
            {
                RegistryKey subkey = key.OpenSubKey(keyName);
                displayName = subkey.GetValue("DisplayName") as string;
                if (p_name.Equals(displayName, StringComparison.OrdinalIgnoreCase) == true)
                {
                    string uninstallCommand = subkey.GetValue("UninstallString") as string;
                    guid = uninstallCommand.Split('{', '}').ElementAtOrDefault(1);
                    return true;
                }
            }

            // search in: LocalMachine_32
            key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall");
            foreach (String keyName in key.GetSubKeyNames())
            {
                RegistryKey subkey = key.OpenSubKey(keyName);
                displayName = subkey.GetValue("DisplayName") as string;
                if (p_name.Equals(displayName, StringComparison.OrdinalIgnoreCase) == true)
                {
                    string uninstallCommand = subkey.GetValue("UninstallString") as string;
                    guid = uninstallCommand.Split('{', '}').ElementAtOrDefault(1);
                    return true;
                }
            }

            // search in: LocalMachine_64
            key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall");
            foreach (String keyName in key.GetSubKeyNames())
            {
                RegistryKey subkey = key.OpenSubKey(keyName);
                displayName = subkey.GetValue("DisplayName") as string;
                if (p_name.Equals(displayName, StringComparison.OrdinalIgnoreCase) == true)
                {
                    string uninstallCommand = subkey.GetValue("UninstallString") as string;
                    guid = uninstallCommand.Split('{', '}').ElementAtOrDefault(1);
                    return true;
                }
            }

            // NOT FOUND
            return false;
        }
        public static bool CopyFiles(string source, string destination)
        {
            try
            {
                if (!Directory.Exists(destination))
                {
                    Directory.CreateDirectory(destination);
                }
                if (File.Exists(Path.Combine(destination, Path.GetFileName(source))))
                {


                    File.Delete(Path.Combine(destination, Path.GetFileName(source)));
                    System.IO.File.Copy(source, Path.Combine(destination, Path.GetFileName(source)), false);
                }
                else
                {
                    System.IO.File.Copy(source, Path.Combine(destination, Path.GetFileName(source)), false);
                }
                return true;
            }
            catch (Exception ex)
            {
                Helper.writeLog(ex);
                return false;
            }
        }
        public static bool checkInstalled(string applicationName)
        {
            try
            {
                string displayName;
                string registryKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
                if (OSHelper.Is64BitOperatingSystem())
                    registryKey = @"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall";
                RegistryKey key = Registry.LocalMachine.OpenSubKey(registryKey);
                if (key != null)
                {
                    foreach (RegistryKey subkey in key.GetSubKeyNames().Select(keyName => key.OpenSubKey(keyName)))
                    {
                        displayName = subkey.GetValue("DisplayName") as string;
                        if (displayName != null && displayName == applicationName)
                        {
                            return true;
                        }
                    }
                    key.Close();
                }
            }
            catch (Exception ex)
            {
                Helper.writeLog(ex);
            }

            return false;
        }
        #region Extract Updates from XML File    
        public static async Task<Response> CheckUpdates(MainWindow gui, string Version)
        {
            try
            {
                DAL db = new DAL();
                string loc = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                var dir = Directory.GetDirectories(loc + "\\Updates").Select(Path.GetFileName)
                            .ToList();
                List<string> directories = new List<string>();
                foreach (var item in dir)
                {
                    if (Helper.isSmallVersion(Version, item))
                    {
                        directories.Add(item);
                    }
                }
                if (directories.Count > 0)
                {
                    //    var ascDirec = directories.OrderBy(f => f.Replace(".", string.Empty));
                    MessageBoxResult result = DisplayMessageBox.Show("Info", _Const.Download_Update, MessageBoxButton.YesNo, QuanikaUpdate.Wins.MessageBoxImage.Information);
                    //    if (result == MessageBoxResult.Yes)
                    //    {
                    //        foreach (var direc in ascDirec)
                    //        {

                    //            var serializer = new XmlSerializer(typeof(Updates));
                    //            using (var stream = new StreamReader(loc + "\\Updates\\" + direc + "\\version.xml"))
                    //            {
                    //                var deserializeData = (Updates)serializer.Deserialize(stream);
                    //                string version = deserializeData.version;
                    //                // Insert db logs in database from xml
                    //                foreach (var data in deserializeData.Files.Database)
                    //                {
                    //                    data.Value = Regex.Replace(data.Value, @"^\s+|\t|\n|\r", "");
                    //                    if (!db.CheckIfDbLogsExists(version))
                    //                    {
                    //                       var logsresponse = await insertUpdateLogs(Modules.SqlServer, data.Value, null, "sql", gui, version);
                    //                        if (logsresponse.status == false)
                    //                        {
                    //                            return logsresponse;
                    //                        }
                    //                    }
                    //                }

                    //                // Insert Dll logs into database from xml
                    //                foreach (var data in deserializeData.Files.DLL)
                    //                {
                    //                    data.Value = Regex.Replace(data.Value, @"^\s+|\t|\n|\r", "");
                    //                    var logsresponse = await insertUpdateLogs(data.module, data.Value, data.folder, "dll", gui, version);
                    //                    if (logsresponse.status == false)
                    //                    {
                    //                        return logsresponse;
                    //                    }
                    //                }

                    //                // Insert exe logs into database from xml
                    //                foreach (var data in deserializeData.Files.EXE)
                    //                {
                    //                    data.Value = Regex.Replace(data.Value, @"^\s+|\t|\n|\r", "");
                    //                    var logsresponse = await insertUpdateLogs(data.module, data.Value, data.folder, "exe", gui, version);
                    //                    if (logsresponse.status == false)
                    //                    {
                    //                        return logsresponse;
                    //                    }
                    //                }
                    //                // Insert Keys logs into database from xml
                    //                foreach (var data in deserializeData.Files.Keys)
                    //                {
                    //                    data.Value = Regex.Replace(data.Value, @"^\s+|\t|\n|\r", "");
                    //                    var logsresponse = await insertUpdateLogs(data.module, data.Value, data.key, "keys", gui, version);
                    //                    if (logsresponse.status == false)
                    //                    {
                    //                        return logsresponse;
                    //                    }
                    //                }

                    //                // Insert file logs into database from xml
                    //                foreach (var data in deserializeData.Files.AnyFile)
                    //                {
                    //                    data.Value = Regex.Replace(data.Value, @"^\s+|\t|\n|\r", "");
                    //                    var logsresponse = await insertUpdateLogs(data.module, data.Value, data.folder, "exe", gui, version);
                    //                    if (logsresponse.status == false)
                    //                    {
                    //                        return logsresponse;
                    //                    }
                    //                }
                    //            }
                    //        }

                    //    }
                    //    else
                    //    {

                    //        Application.Current.Shutdown();
                    //    }
                }
                else
                {
                    Response res3 = new Response();
                    res3.status = false;
                    res3.message = _Const.No_Updates;
                    res3.delveloper_message = _Const.No_Updates;
                    return res3;

                }

                Response res = new Response();
                res.status = true;
                res.message = _Const.Success_Download_Updates;
                res.delveloper_message = _Const.Success_Download_Updates;
                return res;

            }
            catch (Exception ex)
            {
                Helper.writeLog(ex);
                Response res = new Response();
                res.status = false;
                res.message = _Const.localStorage_Error;
                res.delveloper_message = ex.StackTrace;
                return res;

            }

        }
        #endregion
        public static async Task<Response> ExecuteSqlLogs(MainWindow gui, List<UpdateLogs> logs)
        {

            try
            {
                Thread.Sleep(500);
                DAL db = new DAL();
                Response res = new Response();
                string loc = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var sqlLogs = logs.Where(f => f.command.Contains("run sql")).ToList();
                foreach (var log in sqlLogs)
                {
                    string[] splitCommds = log.command.Split(' ');
                    logCommand logSplit = new logCommand();

                    logSplit.Title = splitCommds[0];
                    logSplit.Module = splitCommds[1];
                    logSplit.Filename = splitCommds[2];
                    if (log.command.Contains("run sql"))
                    {
                        gui.UpdatePbLabel("Executing script " + logSplit.Filename.Replace("@", " "));
                        taskDealy();
                        if (!db.runSqlScriptFile(Helper.getQueryFile(loc + "\\Updates\\" + log.version + "\\sql\\" + logSplit.Filename.Replace("@", " "))))
                        {
                            gui.UpdatePbLabel("Unable to execute script " + logSplit.Filename.Replace("@", " "));

                            res.status = false;
                            res.message = ("Unable to execute script " + logSplit.Filename.Replace("@", " ") + " Please restore your database");
                            res.delveloper_message = "Unable to execute script " + logSplit.Filename.Replace("@", " ");
                            return res;
                        }

                        else
                        {
                            gui.UpdatePbLabel("Updating status of " + logSplit.Filename.Replace("@", " ") + " in database");
                            if (await Task.Run(() => (!db.UpdateLogStatus(log.Id))))
                            {
                                res.status = false;
                                res.message = "Unable to update status of " + logSplit.Filename.Replace("@", " ") + " in database";
                                res.delveloper_message = "Unable to update status of " + logSplit.Filename.Replace("@", " ") + " in database";
                                return res;
                            }
                            else
                            {
                                res.status = true;
                                res.message = "Successfully update status of " + logSplit.Filename.Replace("@", " ") + " in database";
                                res.delveloper_message = "Successfully update status of " + logSplit.Filename.Replace("@", " ") + " in database";

                            }


                        }

                    }

                    gui.worker_ProgressChanged(CConfig.PbPercentage);
                    Helper.taskDealy();
                }
                return res;
            }
            catch (Exception ex)
            {
                Helper.writeLog(ex);
                Response res = new Response();
                res.status = false;
                res.message = _Const.Execute_SQl_Logs_Error;
                res.delveloper_message = ex.StackTrace;
                return res;

            }


        }
        public static void taskDealy()
        {
            Thread.Sleep(1000);



        }

        public static bool isSmallVersion(string _installedVersion, string _serverVersion)
        {
            if (!string.IsNullOrEmpty(_installedVersion))
            {
                _installedVersion = cleanVersion(_installedVersion);
            }

            if (!string.IsNullOrEmpty(_serverVersion))
            {
                _serverVersion = cleanVersion(_serverVersion);
            }
            NumberStyles style = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands;
            decimal DecServerVersion = 0;
            Decimal.TryParse(_serverVersion, style, CultureInfo.InvariantCulture, out DecServerVersion);
            decimal DecInstalledVersion = 0;
            Decimal.TryParse(_installedVersion, style, CultureInfo.InvariantCulture, out DecInstalledVersion);
            if (DecInstalledVersion < DecServerVersion)
                return true;
            else
                return false;
        }

        public static string cleanVersion(string version)
        {
            var versionArr = version.Split('.');
            string returnVal = "";
            if (versionArr.Count() > 3)
                returnVal = string.Format("{0}.{1}{2}{3}", versionArr[0], versionArr[1], versionArr[2], versionArr[3]);
            else if (versionArr.Count() == 3)
                returnVal = string.Format("{0}.{1}{2}", versionArr[0], versionArr[1], versionArr[2]);
            return returnVal;
        }
        public static void addKeyInConfig(string key, string value, string path)
        {
            try
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(path);
                // config.AppSettings.Settings[key].Value = value;
                config.AppSettings.Settings.Remove(key);
                config.AppSettings.Settings.Add(key, value);
                config.Save(ConfigurationSaveMode.Minimal);

            }
            catch (Exception ex)
            {
                writeLog(ex, string.Format("Key:{0} Value:{1} Path:{2}", key, value, path));
            }

        }
        public static void updateVersionInConfig(string version)
        {
            try
            {
                string path = @"C:/Program Files (x86)/Quanika/Quanika-DX/DataExchange.exe";
                if (Helper.checkInstalled(_Const.Client_Application_Name))
                {
                    if (OSHelper.Is64BitOperatingSystem())
                    {
                        path = _Const.Client_Application_INSTALLED_CONFIG_PATH_x64;
                    }
                    else
                    {
                        path = _Const.Client_Application_INSTALLED_CONFIG_PATH_X86;
                    }
                    Configuration config = ConfigurationManager.OpenExeConfiguration(path);
                    config.AppSettings.Settings["version"].Value = version;
                    config.Save(ConfigurationSaveMode.Minimal);

                }
                if (Helper.checkInstalled(_Const.Com_Service_Name))
                {
                    if (OSHelper.Is64BitOperatingSystem())
                    {
                        path = _Const.Com_Service_INSTALLED_CONFIG_PATH_x64;
                    }
                    else
                    {
                        path = _Const.Com_Service_INSTALLED_CONFIG_PATH_X86;
                    }
                    Configuration config = ConfigurationManager.OpenExeConfiguration(path);
                    config.AppSettings.Settings["version"].Value = version;
                    config.Save(ConfigurationSaveMode.Minimal);

                }
                if (Helper.checkInstalled(_Const.Data_Upload_Bot_Name))
                {
                    if (OSHelper.Is64BitOperatingSystem())
                    {
                        path = _Const.Data_Upload_Bot_INSTALLED_CONFIG_PATH_x64;
                    }
                    else
                    {
                        path = _Const.Data_Upload_Bot_INSTALLED_CONFIG_PATH_X86;
                    }
                    Configuration config = ConfigurationManager.OpenExeConfiguration(path);
                    config.AppSettings.Settings["version"].Value = version;
                    config.Save(ConfigurationSaveMode.Minimal);

                }
                if (Helper.checkInstalled(_Const.Meeting_Creator_Bot_Name))
                {
                    if (OSHelper.Is64BitOperatingSystem())
                    {
                        path = _Const.Meeting_Creator_Bot_INSTALLED_CONFIG_PATH_x64;
                    }
                    else
                    {
                        path = _Const.Meeting_Creator_Bot_INSTALLED_CONFIG_PATH_X86;
                    }
                    Configuration config = ConfigurationManager.OpenExeConfiguration(path);
                    config.AppSettings.Settings["version"].Value = version;

                    config.Save(ConfigurationSaveMode.Minimal);

                }
                if (Helper.checkInstalled(_Const.VisitorPoint_Kiosk_Name))
                {
                    if (OSHelper.Is64BitOperatingSystem())
                    {
                        path = _Const.VisitorPoint_Kiosk_INSTALLED_CONFIG_PATH_x64;
                    }
                    else
                    {
                        path = _Const.VisitorPoint_Kiosk_INSTALLED_CONFIG_PATH_X86;
                    }
                    Configuration config = ConfigurationManager.OpenExeConfiguration(path);
                    config.AppSettings.Settings["version"].Value = version;

                    config.Save(ConfigurationSaveMode.Minimal);

                }

                //if (Helper.checkInstalled(_Const.OfflineTask_Service_Name))
                //{
                //    if (OSHelper.Is64BitOperatingSystem())
                //    {
                //        path = _Const.OFFLINE_TASK_SERVICE_INSTALLED_CONFIG_PATH_X64;
                //    }
                //    else
                //    {
                //        path = _Const.OFFLINE_TASK_SERVICE_CONFIG_PATH_X86;
                //    }
                //    Configuration config = ConfigurationManager.OpenExeConfiguration(path);
                //    config.AppSettings.Settings["version"].Value = version;

                //    config.Save(ConfigurationSaveMode.Minimal);

                //}

                //if (Helper.checkInstalled(_Const.LPN_Service_Name))
                //{
                //    if (OSHelper.Is64BitOperatingSystem())
                //    {
                //        path = _Const.LPN_INSTALLED_CONFIG_PATH_X64;
                //    }
                //    else
                //    {
                //        path = _Const.LPN_INSTALLED_INSTALL_PATH_X86;
                //    }
                //    Configuration config = ConfigurationManager.OpenExeConfiguration(path);
                //    config.AppSettings.Settings["version"].Value = version;

                //    config.Save(ConfigurationSaveMode.Minimal);

                //}
            }
            catch (Exception ex)
            {
                writeLog(ex);
            }

        }

        public static void CheckIfApplicationRunning(string Name)
        {
            try
            {
                if (Process.GetProcessesByName(Name).Length > 0)
                {
                    MessageBoxResult result = DisplayMessageBox.Show(MsgBoxTitle.ErrorTitle, Name + " is running. Please stop first and then retry again", MessageBoxButton.YesNoCancel, QuanikaUpdate.Wins.MessageBoxImage.Error);
                    while (result == MessageBoxResult.OK)
                    {
                        if (Process.GetProcessesByName(Name).Length > 0)
                        {
                            MessageBoxResult Retryresult = DisplayMessageBox.Show(MsgBoxTitle.ErrorTitle, Name + " is running. Please stop first and then retry again", MessageBoxButton.YesNoCancel, QuanikaUpdate.Wins.MessageBoxImage.Error);
                            if (Retryresult == MessageBoxResult.No || result == MessageBoxResult.Cancel)
                            {

                                result = Retryresult;
                            }
                        }
                        else
                        {

                            break;
                        }

                    }
                    if (result == MessageBoxResult.No || result == MessageBoxResult.Cancel)
                    {

                        Application.Current.Shutdown();
                    }

                }
            }
            catch (Exception ex)
            {
                writeLog(ex);
            }

        }

        #region Registery Related Functions
        public static string GetApplicationVersion(string path)
        {
            string version = "";
            try
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(path);
                version = config.AppSettings.Settings["version"]?.Value;
                return version;

            }
            catch (Exception ex)
            {

                writeLog(ex);
                return version;
            }


        }
        #endregion


        public static async Task<Response> insertUpdateLogs(string module, string value, string folder, string type, MainWindow gui, string version)
        {
            DAL db = new DAL();
            string command = "";
            string commandType = _Const.UpdateCommand + " ";
            string Hostname = CConfig.Hostname;
            if (type == "keys") commandType = _Const.ConfigCommand + " ";
            //if (module == "app" && CConfig.isApplicationInstalled)
            //{
            //    gui.UpdateWindow("Inserting Quanika-Application " + type + " logs" + " " + version + " in database");
            //    command = commandType + module + " " + value.Replace(" ", "@") + " " + folder;
            //}
            else if (module is Modules.ClientApplication && CConfig.IsComServiceInstalled)
            {
                gui.UpdateWindow("Inserting Client-Application " + type + " logs in database");
                command = commandType + module + " " + value.Replace(" ", "@") + " " + folder;
            }
            else if (module is Modules.ComService && CConfig.IsDataUploadBoatInstalled)
            {
                gui.UpdateWindow("Inserting Com-Service " + type + " logs " + " " + version + " in database");
                command = commandType + module + " " + value.Replace(" ", "@") + " " + folder;
            }
            else if (module is Modules.MeetingCreateBot && CConfig.IsMeetingCreatorBotInstalled)
            {
                gui.UpdateWindow("Inserting Meeting-Bot " + type + " logs  version" + " " + version + " in database");
                command = commandType + module + " " + value.Replace(" ", "@") + " " + folder;
            }
            else if (module is Modules.DataBot && CConfig.IsVPKioskInstalled)
            {
                gui.UpdateWindow("Inserting Data-Bot " + type + " logs  version" + " " + version + " in database");
                command = commandType + module + " " + value.Replace(" ", "@") + " " + folder;
            }
            else if (module is Modules.Kiosk && CConfig.IsVPKioskInstalled)
            {
                gui.UpdateWindow("Inserting Kiosk" + type + " logs  version" + " " + version + " in database");
                command = commandType + module + " " + value.Replace(" ", "@") + " " + folder;
            }
            else if (module is Modules.Web)  //&& Check is web installed
            {
                gui.UpdateWindow("Inserting Web " + type + " logs  version" + " " + version + " in database");
                command = commandType + module + " " + value.Replace(" ", "@") + " " + folder;
            }

            if (module is Modules.SqlServer)
            {
                Hostname = "Sql Server";
                command = "run sql " + value;
                gui.UpdateWindow("Inserting update logs version" + " " + version + " in database");
            }
            if (command != "" && !db.CheckIfLogAlreadyExist(version, CConfig.Hostname, command) ? await Task.Run(() => (!db.InsertUpdateLogs(version, command, Hostname))) : false)
            {

                Response res2 = new Response();
                res2.status = false;
                res2.message = _Const.Insert_db_Error;
                res2.delveloper_message = "Unable to insert logs in db. Please check logs.";

                return res2;

            }
            else
            {
                Response res2 = new Response();
                res2.status = true;
                return res2;
            }
        }
        public static async Task<Response> ExecuteLogs(MainWindow gui, List<UpdateLogs> logs, string extension, string update_Path, string backup_Path)
        {
            try
            {
                Thread.Sleep(1000);
                DAL db = new DAL();
                Response res = new Response();
                foreach (var log in logs)
                {
                    string[] splitCommds = log.command.Split(' ');
                    logCommand logSplit = new logCommand();

                    logSplit.Title = splitCommds[0];
                    logSplit.Module = splitCommds[1];
                    logSplit.Filename = splitCommds[2];
                    #region Configure Command Processing    
                    if (splitCommds.Length > 3 && splitCommds[0] == _Const.ConfigCommand)
                    {
                        ConfigureKeys.ProcessConfigure(splitCommds[3], splitCommds[2], splitCommds[1]);
                        res = await ExecuteConfigureLogs(logSplit, log.version, log.Id, gui);
                    }
                    #endregion
                    #region Update Command Processing                    
                    else
                    {
                        if (splitCommds.Length > 3 && splitCommds[3] != string.Empty)
                        {
                            logSplit.Folder = splitCommds[3];

                        }
                        // get extension from logs in case of anyfile
                        if (update_Path == "anyfile")
                        {
                            FileInfo fi = new FileInfo(logSplit.Filename.Replace("@", " "));
                            extension = fi.Extension;
                        }
                        string destinationPath = "";

                        #region Client Application
                        if (CConfig.IsComServiceInstalled && log.command.Contains(Modules.ClientApplication) && log.command.Contains(extension))
                        {
                            if (logSplit.Folder == null)
                            {
                                if (OSHelper.Is64BitOperatingSystem())
                                {
                                    destinationPath = _Const.Client_Application_INSTALLED_PATH_x64;
                                }
                                else
                                {
                                    destinationPath = _Const.Client_Application_INSTALLED_PATH_X86;
                                }

                            }
                            else
                            {
                                if (OSHelper.Is64BitOperatingSystem())
                                {
                                    destinationPath = _Const.Client_Application_INSTALLED_PATH_x64 + "/" + logSplit.Folder;
                                }
                                else
                                {
                                    destinationPath = _Const.Client_Application_INSTALLED_PATH_X86 + "/" + logSplit.Folder;
                                }
                            }
                            res = await ExecuteApplicationLogs(logSplit, extension, update_Path, log.version, destinationPath, _Const.Client_Application_Name, log.Id, backup_Path, gui);

                        }
                        #endregion

                        #region Com Service
                        else if (CConfig.IsClientApplicationInstalled && log.command.Contains(Modules.ComService) && log.command.Contains(extension))
                        {

                            if (logSplit.Folder == null)
                            {
                                if (OSHelper.Is64BitOperatingSystem())
                                {
                                    destinationPath = _Const.Com_Service_INSTALLED_PATH_x64;
                                }
                                else
                                {
                                    destinationPath = _Const.Com_Service_INSTALLED_PATH_X86;
                                }

                            }
                            else
                            {
                                if (OSHelper.Is64BitOperatingSystem())
                                {
                                    destinationPath = _Const.Com_Service_INSTALLED_PATH_x64 + "/" + logSplit.Folder;
                                }
                                else
                                {
                                    destinationPath = _Const.Com_Service_INSTALLED_PATH_X86 + "/" + logSplit.Folder;
                                }
                            }
                            res = await ExecuteApplicationLogs(logSplit, extension, update_Path, log.version, destinationPath, _Const.Com_Service_Name, log.Id, backup_Path, gui);

                        }
                        #endregion

                        #region Meeting Create Bot
                        else if (CConfig.IsDataUploadBoatInstalled && log.command.Contains(Modules.MeetingCreateBot) && log.command.Contains(extension))
                        {
                            if (logSplit.Folder == null)
                            {
                                if (OSHelper.Is64BitOperatingSystem())
                                {
                                    destinationPath = _Const.Meeting_Creator_Bot_INSTALLED_PATH_x64;
                                }
                                else
                                {
                                    destinationPath = _Const.Meeting_Creator_Bot_INSTALLED_PATH_X86;
                                }

                            }
                            else
                            {
                                if (OSHelper.Is64BitOperatingSystem())
                                {
                                    destinationPath = _Const.Meeting_Creator_Bot_INSTALLED_PATH_x64 + "/" + logSplit.Folder;
                                }
                                else
                                {
                                    destinationPath = _Const.Meeting_Creator_Bot_INSTALLED_PATH_X86 + "/" + logSplit.Folder;
                                }
                            }
                            res = await ExecuteApplicationLogs(logSplit, extension, update_Path, log.version, destinationPath, _Const.Meeting_Creator_Bot_Name, log.Id, backup_Path, gui);

                        }
                        #endregion

                        #region Data Bot
                        else if (CConfig.IsMeetingCreatorBotInstalled && log.command.Contains(Modules.DataBot) && log.command.Contains(extension))
                        {
                            if (logSplit.Folder == null)
                            {
                                if (OSHelper.Is64BitOperatingSystem())
                                {
                                    destinationPath = _Const.Data_Upload_Bot_INSTALLED_PATH_x64;
                                }
                                else
                                {
                                    destinationPath = _Const.Data_Upload_Bot_INSTALLED_PATH_X86;
                                }
                            }
                            else
                            {
                                if (OSHelper.Is64BitOperatingSystem())
                                {
                                    destinationPath = _Const.Data_Upload_Bot_INSTALLED_PATH_x64 + "/" + logSplit.Folder;
                                }
                                else
                                {
                                    destinationPath = _Const.Data_Upload_Bot_INSTALLED_PATH_X86 + "/" + logSplit.Folder;
                                }
                            }
                            res = await ExecuteApplicationLogs(logSplit, extension, update_Path, log.version, destinationPath, _Const.Data_Upload_Bot_Name, log.Id, backup_Path, gui);
                        }


                        #endregion

                        #region KIOSK
                        else if (CConfig.IsVPKioskInstalled && log.command.Contains(Modules.Kiosk) && log.command.Contains(extension))
                        {
                            if (logSplit.Folder == null)
                            {
                                if (OSHelper.Is64BitOperatingSystem())
                                {
                                    destinationPath = _Const.VisitorPoint_Kiosk_INSTALLED_CONFIG_PATH_x64;
                                }
                                else
                                {
                                    destinationPath = _Const.VisitorPoint_Kiosk_INSTALLED_CONFIG_PATH_X86;
                                }
                            }
                            else
                            {
                                if (OSHelper.Is64BitOperatingSystem())
                                {
                                    destinationPath = _Const.VisitorPoint_Kiosk_INSTALLED_CONFIG_PATH_x64 + "/" + logSplit.Folder;
                                }
                                else
                                {
                                    destinationPath = _Const.VisitorPoint_Kiosk_INSTALLED_CONFIG_PATH_X86 + "/" + logSplit.Folder;
                                }
                            }
                            res = await ExecuteApplicationLogs(logSplit, extension, update_Path, log.version, destinationPath, _Const.Data_Upload_Bot_Name, log.Id, backup_Path, gui);
                        }
                        #endregion


                        #region If Application Type is DxMonitoring
                        //else if (CConfig.isDXMONITORINGServiceInstalled && log.command.Contains(Modules.) && log.command.Contains(extension))
                        //{
                        //    if (logSplit.Folder == null)
                        //    {
                        //        if (OSHelper.Is64BitOperatingSystem())
                        //        {
                        //            destinationPath = _Const.DXMonitoring_Service_INSTALLED_PATH_X64;
                        //        }
                        //        else
                        //        {
                        //            destinationPath = _Const.DXMonitoring_Service_INSTALLED_PATH_X86;
                        //        }
                        //    }
                        //    else
                        //    {
                        //        if (OSHelper.Is64BitOperatingSystem())
                        //        {
                        //            destinationPath = _Const.DXMonitoring_Service_INSTALLED_PATH_X64 + "/" + logSplit.Folder;
                        //        }
                        //        else
                        //        {
                        //            destinationPath = _Const.DXMonitoring_Service_INSTALLED_PATH_X86 + "/" + logSplit.Folder;
                        //        }
                        //    }
                        //    res = await ExecuteApplicationLogs(logSplit, extension, update_Path, log.version, destinationPath, _Const.Dx_Name, log.Id, backup_Path, gui);
                        //}
                        #endregion
                        #region If Application Type is LPN Service
                        //else if (CConfig.isLPNServiceInstalled && log.command.Contains("lpn") && log.command.Contains(extension))
                        //{
                        //    if (logSplit.Folder == null)
                        //    {
                        //        if (OSHelper.Is64BitOperatingSystem())
                        //        {
                        //            destinationPath = _Const.LPN_INSTALLED_INSTALL_PATH_X64;
                        //        }
                        //        else
                        //        {
                        //            destinationPath = _Const.LPN_INSTALLED_INSTALL_PATH_X86;
                        //        }
                        //    }
                        //    else
                        //    {
                        //        if (OSHelper.Is64BitOperatingSystem())
                        //        {
                        //            destinationPath = _Const.LPN_INSTALLED_INSTALL_PATH_X64 + "/" + logSplit.Folder;
                        //        }
                        //        else
                        //        {
                        //            destinationPath = _Const.LPN_INSTALLED_INSTALL_PATH_X86 + "/" + logSplit.Folder;
                        //        }
                        //    }
                        //    res = await ExecuteApplicationLogs(logSplit, extension, update_Path, log.version, destinationPath, _Const.Dx_Name, log.Id, backup_Path, gui);
                        //}
                        #endregion
                        #region If Application Type is Offline Task Service
                        //else if (CConfig.isOfflineTaskServiceInstalled && log.command.Contains("offline") && log.command.Contains(extension))
                        //{
                        //    if (logSplit.Folder == null)
                        //    {
                        //        if (OSHelper.Is64BitOperatingSystem())
                        //        {
                        //            destinationPath = _Const.OFFLINE_TASK_INSTALL_INSTALLED_INSTALL_PATH_X64;
                        //        }
                        //        else
                        //        {
                        //            destinationPath = _Const.OFFLINE_TASK_SERVICE_INSTALLED_INSTALL_PATH_X86;
                        //        }
                        //    }
                        //    else
                        //    {
                        //        if (OSHelper.Is64BitOperatingSystem())
                        //        {
                        //            destinationPath = _Const.OFFLINE_TASK_INSTALL_INSTALLED_INSTALL_PATH_X64 + "/" + logSplit.Folder;
                        //        }
                        //        else
                        //        {
                        //            destinationPath = _Const.OFFLINE_TASK_SERVICE_INSTALLED_INSTALL_PATH_X86 + "/" + logSplit.Folder;
                        //        }
                        //    }
                        //    res = await ExecuteApplicationLogs(logSplit, extension, update_Path, log.version, destinationPath, _Const.Dx_Name, log.Id, backup_Path, gui);
                        //}
                        #endregion

                        else
                        {
                            res.status = true;
                            res.message = "No file found with extension " + extension + " to execute in db";
                            res.delveloper_message = "No file found with extension " + extension + " to execute in db";
                        }
                    }
                    #endregion
                }
                return res;
            }
            catch (Exception ex)
            {
                Helper.writeLog(ex);
                Response res = new Response();
                res.status = false;
                res.message = _Const.Execute_DLL_Logs_Error;
                res.delveloper_message = ex.StackTrace;
                return res;
            }
        }
        public static async Task<Response> ExecuteApplicationLogs(logCommand logSplit, string extension, string update_Path, string version, string destinationPath, string ApplicationNames, long logId, string Backup_Path, MainWindow gui)
        {
            string loc = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            FileInfo fi = new FileInfo(logSplit.Filename.Replace("@", " "));
            Response res = new Response();
            DAL db = new DAL();
            if (update_Path != _Const.updates_files_Folder_Name ? fi.Extension == extension : true)
            {
                string sourcePath = loc + "\\Updates\\" + version + "\\" + update_Path + "\\" + logSplit.Filename.Replace("@", " ");
                gui.UpdatePbLabel("Updating " + ApplicationNames + " with " + logSplit.Filename.Replace("@", " "));

                if (await Task.Run(() => (update_Path != _Const.updates_files_Folder_Name ? !Helper.CopyFiles(sourcePath, destinationPath, CConfig.Setting.version, logSplit.Filename.Replace("@", " "), ApplicationNames, Backup_Path) : (!Helper.CopyFiles(sourcePath, destinationPath)))))
                {

                    gui.UpdatePbLabel("Unable to update " + ApplicationNames + " with " + logSplit.Filename.Replace("@", " "));
                    res.status = false;
                    res.message = "Unable to update " + ApplicationNames + " with " + logSplit.Filename.Replace("@", " ");
                    res.delveloper_message = "Unable to update " + ApplicationNames + " with " + logSplit.Filename.Replace("@", " ");
                    return res;
                }
                else
                {
                    gui.UpdatePbLabel("Updating status of " + logSplit.Filename.Replace("@", " ") + " in database");
                    if (await Task.Run(() => (!db.UpdateLogStatus(logId))))
                    {
                        gui.UpdatePbLabel("Unable to update status of " + logSplit.Filename.Replace("@", " ") + " in database");
                        res.status = false;
                        res.message = "Unable to update status of " + logSplit.Filename.Replace("@", " ") + " in database";
                        res.delveloper_message = "Unable to update status of " + logSplit.Filename.Replace("@", " ") + " in database";
                        return res;
                    }
                    else
                    {

                        gui.worker_ProgressChanged(CConfig.PbPercentage);
                        res.status = true;
                        res.message = "Successfully update status of " + logSplit.Filename.Replace("@", " ") + " in database";
                        res.delveloper_message = "Successfully update status of " + logSplit.Filename.Replace("@", " ") + " in database";
                        return res;
                    }
                }

            }
            else
            {
                res.status = true;
                return res;
            }
        }

        public static async Task<Response> ExecuteConfigureLogs(logCommand logSplit, string version, long logId, MainWindow gui)
        {
            Response res = new Response();
            DAL db = new DAL();
            gui.UpdatePbLabel("Updating status of " + logSplit.Filename.Replace("@", " ") + " in database");
            if (await Task.Run(() => (!db.UpdateLogStatus(logId))))
            {
                gui.UpdatePbLabel("Unable to update status of " + logSplit.Filename.Replace("@", " ") + " in database");
                res.status = false;
                res.message = "Unable to update status of " + logSplit.Filename.Replace("@", " ") + " in database";
                res.delveloper_message = "Unable to update status of " + logSplit.Filename.Replace("@", " ") + " in database";
                return res;
            }
            else
            {
                gui.worker_ProgressChanged(CConfig.PbPercentage);
                res.status = true;
                res.message = "Successfully update status of " + logSplit.Filename.Replace("@", " ") + " in database";
                res.delveloper_message = "Successfully update status of " + logSplit.Filename.Replace("@", " ") + " in database";
                return res;
            }

        }
        public static bool CopyFiles(string source, string destination, string version, string command, string Type, string Backup_Path)
        {
            try
            {
                if (!Directory.Exists(destination))
                {
                    Directory.CreateDirectory(destination);
                }

                if (File.Exists(Path.Combine(destination, Path.GetFileName(source))))
                {
                    string loc = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                    if (!Directory.Exists(loc + "\\" + Backup_Path + "\\" + version + "\\" + Type))
                    {
                        Directory.CreateDirectory(loc + "\\" + Backup_Path + "\\" + version + "\\" + Type);
                    }
                    string destinationPath = loc + "\\" + Backup_Path + "\\" + version + "\\" + Type;
                    if (File.Exists(Path.Combine(destinationPath, command)))
                    {
                        File.Delete(Path.Combine(destinationPath, command));
                        System.IO.File.Copy(Path.Combine(destination, command), Path.Combine(destinationPath, command), false);

                    }
                    else
                    {
                        System.IO.File.Copy(Path.Combine(destination, command), Path.Combine(destinationPath, command), false);

                    }

                    File.Delete(Path.Combine(destination, Path.GetFileName(source)));
                    System.IO.File.Copy(source, Path.Combine(destination, Path.GetFileName(source)), false);
                }
                else
                {
                    System.IO.File.Copy(source, Path.Combine(destination, Path.GetFileName(source)), false);
                }
                return true;
            }
            catch (Exception ex)
            {
                writeLog(ex, "CopyFiles");
                return false;
            }
        }
        public static void ExtractToDirectory(string sourceArchiveFileName, string destinationDirectoryName, bool overwrite)
        {
            try
            {
                string folderName = Path.GetFileNameWithoutExtension(sourceArchiveFileName);
                using (ZipArchive archive = ZipFile.OpenRead(sourceArchiveFileName))
                {
                    if (!overwrite)
                    {
                        archive.ExtractToDirectory(destinationDirectoryName);
                        return;
                    }
                    if (folderName != archive.Entries[0].FullName.TrimEnd('/'))
                    {
                        string completeFileName = Path.Combine(destinationDirectoryName, folderName + '/');
                        string directory = Path.GetDirectoryName(completeFileName);
                        if (!Directory.Exists(directory))
                            Directory.CreateDirectory(directory);

                        foreach (ZipArchiveEntry file in archive.Entries)
                        {
                            completeFileName = Path.Combine(destinationDirectoryName, folderName + '/' + file.FullName);
                            directory = Path.GetDirectoryName(completeFileName);

                            if (!Directory.Exists(directory))
                                Directory.CreateDirectory(directory);

                            if (file.Name != "")
                                file.ExtractToFile(completeFileName, true);
                        }
                    }
                    else
                    {
                        foreach (ZipArchiveEntry file in archive.Entries)
                        {
                            string completeFileName = Path.Combine(destinationDirectoryName, file.FullName);
                            string directory = Path.GetDirectoryName(completeFileName);

                            if (!Directory.Exists(directory))
                                Directory.CreateDirectory(directory);

                            if (file.Name != "")
                                file.ExtractToFile(completeFileName, true);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Helper.writeLog(ex.Message);
            }
        }
    }
}




