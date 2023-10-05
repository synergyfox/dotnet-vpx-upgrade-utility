using AxisInstallerPackage.Wins;
using Microsoft.Win32;
using QuanikaUpdate.Constants;
using QuanikaUpdate.Helpers;
using QuanikaUpdate.Models;
using QuanikaUpdate.Wins;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Xml.Serialization;
using VPSetup.Database;
using VPSetup.Helpers;
using static QuanikaUpdate.Models.XmlSerialze;
using static VPSetup.Helpers.CConfig;
using Application = System.Windows.Application;

namespace QuanikaUpdate
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        internal UpgradeUtility UpgradeUtility { get; private set; } = UpgradeUtility.VisitorPoint;
        DAL db;
        FTPHelper ftp;
        public List<InstalledVersionModel> installed_apps { get; set; }
        public List<Logs> installed_logs { get; set; }
        public List<VersionInformation> VersionInfoList;
        public MainWindow()
        {
            InitializeComponent();


            ftp = new FTPHelper();
            VersionInfoList = new List<VersionInformation>();
            txtVersion.Text = Helper.getAppSetting("version");


        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtVersion.Text = ConfigurationManager.AppSettings["version"].ToString();

            NextButtonWizardFunctionality(1);

            CheckApplicationStatuses();

            db = new DAL();
        }

        #region btnNext
        private async void btnNext1_Click(object sender, RoutedEventArgs e)
        {
            if (UpgradeUtility is UpgradeUtility.Quanika)
            {
                await ProcessWithQuanikaModules();
            }
            else if (UpgradeUtility is UpgradeUtility.VisitorPoint)
            {
                await ProcessWithVisitorPointModules();
            }
        }

        private async Task ProcessWithQuanikaModules()
        {
            btnNext1.IsEnabled = false;
            #region Check if Application/Dx/Service Installed  and running
            if (CConfig.isApplicationInstalled || CConfig.isDXInstalled || CConfig.isServiceInstalled || CConfig.isADServiceInstalled || CConfig.isOfflineTaskServiceInstalled)
            {

                CheckQuanikaModulesRunning();

                if (CConfig.Setting.database == "" || CConfig.Setting.server == "" || CConfig.Setting.username == "" || CConfig.Setting.password == "")
                {
                    NextButtonWizardFunctionality(2);

                }
                else
                {
                    this.loader.Visibility = Visibility.Visible;
                    await ExecuteQuanikaTasks();

                }
            }
            #endregion
            else
            {


                MessageBoxResult result = DisplayMessageBox.Show(MsgBoxTitle.WarningTitle, ApplicationConstants.QCS_Not_Installed, MessageBoxButton.OK, Wins.MessageBoxImage.Warning);

                if (result == MessageBoxResult.OK)
                {
                    Application.Current.Shutdown();
                }


            }
        }
        private async Task ProcessWithVisitorPointModules()
        {
            btnNext1.IsEnabled = false;


            CheckVPModulesRunning();

            if (Helper.IsAnyDbSettingEmpty())
            {
                NextButtonWizardFunctionality(2);
            }
            else
            {
                loader.Visibility = Visibility.Visible;
                await ExecuteVisitorPointTask();
            }
        }
        private static void CheckQuanikaModulesRunning()
        {
            if (CConfig.isApplicationInstalled)
            {
                Helper.CheckIfApplicationRunning(ApplicationConstants.Application_Process_Name);
            }
            // Check If Data Exchange Running
            if (CConfig.isDXInstalled)
            {
                Helper.CheckIfApplicationRunning(ApplicationConstants.Dx_Process_Name);
            }
            // Check If Service Running
            if (CConfig.isServiceInstalled)
            {
                Helper.CheckIfApplicationRunning(ApplicationConstants.Service_Process_Name);

            }
            // Check If Quanika Active Directory Service Running
            if (CConfig.isADServiceInstalled)
            {
                Helper.CheckIfApplicationRunning(ApplicationConstants.ADService_Process_Name);
            }

            // Check If Quanika Offline Task Service Running
            if (CConfig.isOfflineTaskServiceInstalled)
            {
                Helper.CheckIfApplicationRunning(ApplicationConstants.OfflineTask_App_Name);
            }

            // Check If Quanika LPN Service Running
            if (CConfig.isLPNServiceInstalled)
            {
                Helper.CheckIfApplicationRunning(ApplicationConstants.LPN_App_Name);
            }

            // Check If DX Monitoring Service Running
            if (CConfig.isDXMONITORINGServiceInstalled)
            {
                Helper.CheckIfApplicationRunning(ApplicationConstants.DXMonitoring_Process_Name);
            }
        }

        private void CheckVPModulesRunning()
        {
            if (CConfig.IsClientApplicationInstalled)
            {
                Helper.CheckIfApplicationRunning(ApplicationConstants.Client_Application_PROCESS_NAME, "VisitorPoint-Client Application");

            }
            // Check If Data Exchange Running
            if (CConfig.IsComServiceInstalled)
            {
                Helper.CheckIfApplicationRunning(ApplicationConstants.Com_Service_Process_Name, "VisitorPoint COM Service Settings Application");
                Helper.CheckIfApplicationRunning(ApplicationConstants.Com_Service_PROCESS_NAME, "VisitorPoint-COMService");
            }
            // Check If Service Running
            if (CConfig.IsMeetingCreatorBotInstalled)
            {
                Helper.CheckIfApplicationRunning(ApplicationConstants.Meeting_Creator_Bot_PROCESS_NAME, ApplicationConstants.Meeting_Creator_Bot_Name+" Application" +" or "+ "VisitorPoint DataUpload-Bot Application");
                Helper.CheckIfApplicationRunning(ApplicationConstants.Meeting_Creator_Bot_Service_PROCESS_NAME, ApplicationConstants.Meeting_Creator_Bot_Service_Name);

            }
            // Check If Quanika Active Directory Service Running
            if (CConfig.IsDataUploadBoatInstalled)
            {
                Helper.CheckIfApplicationRunning(ApplicationConstants.Data_Upload_Bot_PROCESS_NAME, "VisitorPoint DataUpload-Bot Application");
                Helper.CheckIfApplicationRunning(ApplicationConstants.Data_Upload_Bot_Service_PROCESS_NAME, ApplicationConstants.Data_Upload_Bot_Service_Name);
            }

            // Check If Quanika Offline Task Service Running
            if (CConfig.IsOutlookInstalled)
            {
                Helper.CheckIfApplicationRunning(ApplicationConstants.Oulook_PROCESS_NAME, ApplicationConstants.VisitorPoint_Oulook_Name);
            }

            // Check If Quanika LPN Service Running
            if (CConfig.IsKioskInstalled)
            {
                Helper.CheckIfApplicationRunning(ApplicationConstants.Kiosk_PROCESS_NAME, ApplicationConstants.VisitorPoint_Kiosk_Name);
            }

            // Check If DX Monitoring Service Running
            if (CConfig.IsWebInstalled)
            {
                CheckIsWebRunning(ApplicationConstants.VisitorPoint_Web_Name);
            }
            if (CConfig.IsWebRegInstalled)
            {
                CheckIsWebRunning(ApplicationConstants.VisitorPoint_Web_Reg_Name);
            }
            if (CConfig.IsQrWebInstalled)
            {
                CheckIsWebRunning(ApplicationConstants.VisitorPoint_Qr_Web_Name);
            }
        }

        private void CheckIsWebRunning(string webname)
        {
            var pathsHelper = new PathsHelper();
            if (pathsHelper.IsWebRunning(webname))
            {
                MessageBoxResult result = DisplayMessageBox.Show(MsgBoxTitle.ErrorTitle, webname + " is running. Please stop first and then retry again", MessageBoxButton.YesNoCancel, QuanikaUpdate.Wins.MessageBoxImage.Error);
                while (result == MessageBoxResult.OK)
                {
                    if (pathsHelper.IsWebRunning(webname))
                    {
                        MessageBoxResult Retryresult = DisplayMessageBox.Show(MsgBoxTitle.ErrorTitle, webname + " is running. Please stop first and then retry again", MessageBoxButton.YesNoCancel, QuanikaUpdate.Wins.MessageBoxImage.Error);
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



        //Database settings
        private async void btnNext2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ValidateFields())
                    return;
                string msg = "";

                CConfig._db_server = txtServer.Text;
                CConfig._db_username = txtUsername.Text;
                CConfig._db_pwd = txtPassword.Password;

                if (new DAL().IsConnected(out msg))
                {
                    saveDbSettings();
                    saveRegistryValues();
                    await ExecuteQuanikaTasks();
                    NextButtonWizardFunctionality(5);
                }
                else
                {
                    DisplayMessageBox.Show(msg, "Info", MessageBoxButton.OK, Wins.MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                //show error
            }
        }

        #region Network Sharing
        private bool ValidateFields()
        {
            bool result = false;
            StringBuilder _string = new StringBuilder();
            if (string.IsNullOrEmpty(txtServer.Text) || txtServer.Text.Length == 0)
            {
                _string.AppendLine("Server name is required.");
            }
            if (string.IsNullOrEmpty(txtUsername.Text) || txtUsername.Text.Length == 0)
            {
                _string.AppendLine("User name is required.");
            }
            if (string.IsNullOrEmpty(txtPassword.Password.ToString()) || txtPassword.Password.Length == 0)
            {
                _string.AppendLine("Password is required.");
            }
            if (!string.IsNullOrEmpty(_string.ToString()))
            {

                Helper.showBox("info", _string.ToString());
                result = true;
            }
            return result;
        }


        private void btnNext3_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string appguid = "";
                if (!Helper.CheckDotNet45OrPlus())
                {
                    Helper.Run(ApplicationConstants.DOT_NET_45_X64);
                }

                if (!Helper.IsApplictionInstalled("Microsoft System CLR Types for SQL Server 2014", out appguid))
                {
                    Helper.RunMsiSetup(ApplicationConstants.GET_CLR_PATH);
                }

                if (!Helper.IsApplictionInstalled("Microsoft Report Viewer 2015 Runtime", out appguid))
                {
                    Helper.RunMsiSetup(ApplicationConstants.GET_REPORT_VIEWER_PATH);
                }

                Helper.Run(ApplicationConstants.GET_APPLICATION_PATH);
                //Helper.updateConfigFile("APP");
                NextButtonWizardFunctionality(4);
            }
            catch (Exception ex)
            {
                //show error
            }

        }

        #endregion

        #endregion

        #region btnPrevious
        private void btnPrevious2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PreviousButtonWizardFunctionality(1);
            }
            catch (Exception ex)
            {
                //show error
            }
        }

        private void btnPrevious3_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PreviousButtonWizardFunctionality(2);
            }
            catch (Exception ex)
            {
                //show error
            }

        }
        #endregion

        #region btnCancel
        private void btnCancel1_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnCancel2_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void btnCancel3_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        #endregion

        private void saveDbSettings()
        {

            CConfig.Setting.server = txtServer.Text;
            CConfig.Setting.database = CConfig._database_name;
            CConfig.Setting.username = txtUsername.Text;
            CConfig.Setting.password = Helper.Encrypt(txtPassword.Password.ToString());
        }

        private void saveRegistryValues()
        {

            try
            {
                RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\WOW6432Node\Quanika");

                if (key != null)
                {
                    key.SetValue("database", CConfig._database_name);
                    key.SetValue("password", Helper.Encrypt(txtPassword.Password.ToString()));
                    key.SetValue("username", txtUsername.Text);
                    key.SetValue("server", txtServer.Text);

                }
                key.Close();
            }
            catch (Exception ex)
            {

            }
        }

        //void bindDbValues()
        //{
        //    CConfig._db_server = Helper.getAppSetting("server");
        //    CConfig._database_name = Helper.getAppSetting("database");
        //    CConfig._db_username = Helper.getAppSetting("username");
        //    CConfig._db_pwd = Helper.Decrypt(Helper.getAppSetting("password"));
        //    txtServer.Text = CConfig._db_server;
        //    txtUsername.Text = CConfig._db_username;
        //    txtPassword.Password = CConfig._db_pwd;
        //}

        private async Task ExecuteVisitorPointTask()
        {

            this.LogLabel.Text = ApplicationConstants.Verify_Version;

            decimal lowest_version = VersionInfoList.Min(entry => entry.version);
            var VersionInfo = VersionInfoList.Where(entry => entry.version == lowest_version);
            CConfig.Setting.version = lowest_version.ToString();


            this.LogLabel.Text = ApplicationConstants.Checking_Updates;
            await Task.Run(() => Helper.TaskDealy());

            #region Commented FTP
            // Check if Ftp Enabled 

            //if (Helper.getAppSetting("FtpEnabled") == "1")
            //{
            //    Response res = await ftp.CheckUpdates(this, CConfig.Setting.version);
            //    if (res.status == true)
            //    {
            //        this.LogLabel.Text = res.message;
            //        await ExecuteUpdateLogs(ApplicationConstantss.execute_Update_Logs);

            //    }

            //    else if (res.status == false && res.message == ApplicationConstantss.No_Updates)
            //    {
            //        MessageBoxResult result = DisplayMessageBox.Show("Info", res.message, MessageBoxButton.OK, Wins.MessageBoxImage.Information);
            //        if (result == MessageBoxResult.OK)
            //        {
            //            Application.Current.Shutdown();
            //        }
            //    }
            //    else if (res.status == false && res.message == "Version exist")
            //    {
            //        await ExecuteUpdateLogs(ApplicationConstantss.execute_Pending_Logs);
            //    }

            //    else if (res.status == false && res.message == ApplicationConstantss.Con_Error_Ftp)
            //    {
            //        this.LogLabel.Text = ApplicationConstantss.Checking_LocalStorage;

            //        await CheckLocalStorage(CConfig.Setting.version);

            //    }
            //}
            //// Check For Local Storage
            //else
            //{

            //}
            #endregion

            this.LogLabel.Text = ApplicationConstants.Checking_LocalStorage;
            await CheckVPLocalStorage(CConfig.Setting.version);

        }

        private async Task ExecuteQuanikaTasks()
        {

            this.LogLabel.Text = ApplicationConstants.Verify_Version;

            decimal lowest_version = VersionInfoList.Min(entry => entry.version);
            var VersionInfo = VersionInfoList.Where(entry => entry.version == lowest_version);
            CConfig.Setting.version = lowest_version.ToString();


            this.LogLabel.Text = ApplicationConstants.Checking_Updates;
            await Task.Run(() => Helper.TaskDealy());

            #region Check if Ftp Enabled  ___Commented

            //if (Helper.getAppSetting("FtpEnabled") == "1")
            //{
            //    Response res = await ftp.CheckUpdates(this, CConfig.Setting.version);
            //    if (res.status == true)
            //    {
            //        this.LogLabel.Text = res.message;
            //        await ExecuteUpdateLogs(ApplicationConstantss.execute_Update_Logs);

            //    }

            //    else if (res.status == false && res.message == ApplicationConstantss.No_Updates)
            //    {
            //        MessageBoxResult result = DisplayMessageBox.Show("Info", res.message, MessageBoxButton.OK, Wins.MessageBoxImage.Information);
            //        if (result == MessageBoxResult.OK)
            //        {
            //            Application.Current.Shutdown();
            //        }
            //    }
            //    else if (res.status == false && res.message == "Version exist")
            //    {
            //        await ExecuteUpdateLogs(ApplicationConstantss.execute_Pending_Logs);
            //    }

            //    else if (res.status == false && res.message == ApplicationConstantss.Con_Error_Ftp)
            //    {
            //        this.LogLabel.Text = ApplicationConstantss.Checking_LocalStorage;

            //        await CheckQuanikaLocalStorage(CConfig.Setting.version);

            //    }
            //}
            //// Check For Local Storage
            //else
            //{

            //}
            #endregion

            this.LogLabel.Text = ApplicationConstants.Checking_LocalStorage;
            await CheckQuanikaLocalStorage(CConfig.Setting.version);
        }
        private async Task CheckVPLocalStorage(string maxversion)
        {
            await Task.Run(() => Helper.TaskDealy());
            var res2 = await Helper.InsertVisitorPointUpdates(this, maxversion);
            if (res2.status == true)
            {
                this.LogLabel.Text = res2.message;
                await ExecuteVPUpdateLogs(ApplicationConstants.execute_Update_Logs);

            }
            else if (res2.status == false && res2.message == ApplicationConstants.No_Updates)
            {
                MessageBoxResult result = DisplayMessageBox.Show("Info", res2.message, MessageBoxButton.OK, Wins.MessageBoxImage.Information);
                if (result == MessageBoxResult.OK)
                {
                    Application.Current.Shutdown();
                }
            }
            else if (res2.status == false && res2.message == "Version exist")
            {
                await ExecuteVPUpdateLogs(ApplicationConstants.execute_Pending_Logs);
            }
            else
            {

                MessageBoxResult result = DisplayMessageBox.Show("Info", res2.message, MessageBoxButton.OK, Wins.MessageBoxImage.Information);
                if (result == MessageBoxResult.OK)
                {
                    Application.Current.Shutdown();
                }
            }
        }
        private async Task CheckQuanikaLocalStorage(string maxversion)
        {
            await Task.Run(() => Helper.TaskDealy());
            Response res2 = await Helper.InsertQunaikaUpdates(this, maxversion);
            if (res2.status == true)
            {
                this.LogLabel.Text = res2.message;
                await ExecuteQuanikaUpdateLogs(ApplicationConstants.execute_Update_Logs);

            }
            else if (res2.status == false && res2.message == ApplicationConstants.No_Updates)
            {
                MessageBoxResult result = DisplayMessageBox.Show("Info", res2.message, MessageBoxButton.OK, Wins.MessageBoxImage.Information);
                if (result == MessageBoxResult.OK)
                {
                    Application.Current.Shutdown();
                }
            }
            else if (res2.status == false && res2.message == "Version exist")
            {
                await ExecuteQuanikaUpdateLogs(ApplicationConstants.execute_Pending_Logs);
            }
            else
            {

                MessageBoxResult result = DisplayMessageBox.Show("Info", res2.message, MessageBoxButton.OK, Wins.MessageBoxImage.Information);
                if (result == MessageBoxResult.OK)
                {
                    Application.Current.Shutdown();
                }
            }
        }
        private async Task ExecuteQuanikaUpdateLogs(string logsType)
        {
            try
            {

                string loc = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                List<UpdateLogs> logs = new List<UpdateLogs>();
                List<UpdateLogs> sqllogs = new List<UpdateLogs>();
                int Version = Convert.ToInt32(CConfig.Setting.version.Replace(".", ""));

                // Get Logs from database
                this.LogLabel.Text = ApplicationConstants.Get_DB_logs;
                if (logsType == ApplicationConstants.execute_Update_Logs)
                {
                    logs = await Task.Run(() => db.getUpdateLogs(Version, CConfig.Hostname));
                }
                else if (logsType == ApplicationConstants.execute_Pending_Logs)
                {
                    logs = await Task.Run(() => db.getPendingLogs(CConfig.Hostname));
                }
                CConfig.PbPercentage = 100;
                if (logs.Count > 0)
                {
                    CConfig.PbPercentage = ((double)logs.Count / 100);
                }
                sqllogs = await Task.Run(() => db.getDbLogs(Version));
                if (sqllogs.Any())
                {
                    CConfig.PbPercentage = ((double)(logs.Count + sqllogs.Count) / 100);
                    MessageBoxResult result = DisplayMessageBox.Show(MsgBoxTitle.WarningTitle, ApplicationConstants.Db_backup, MessageBoxButton.YesNo, Wins.MessageBoxImage.Warning);
                    if (result == MessageBoxResult.Yes)
                    {
                        await ExecuteQuanikaLogs(logs, sqllogs);
                    }
                    else
                    {
                        Application.Current.Shutdown();
                    }
                }
                else
                {
                    await ExecuteQuanikaLogs(logs, sqllogs);
                }

            }
            catch (Exception ex)
            {
                Helper.WriteLog(ex);

            }

        }
        private async Task ExecuteVPUpdateLogs(string logsType)
        {
            try
            {

                string loc = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                List<UpdateLogs> logs = new List<UpdateLogs>();
                List<UpdateLogs> sqllogs = new List<UpdateLogs>();
                int Version = Convert.ToInt32(CConfig.Setting.version.Replace(".", ""));

                // Get Logs from database
                this.LogLabel.Text = ApplicationConstants.Get_DB_logs;
                if (logsType == ApplicationConstants.execute_Update_Logs)
                {
                    logs = await Task.Run(() => db.getUpdateLogs(Version, Hostname));
                }
                else if (logsType == ApplicationConstants.execute_Pending_Logs)
                {
                    logs = await Task.Run(() => db.getPendingLogs(Hostname));
                }
                CConfig.PbPercentage = 100;
                if (logs.Count > 0)
                {
                    CConfig.PbPercentage = ((double)logs.Count / 100);
                }
                sqllogs = await Task.Run(() => db.GetVpDbLogs(Version));
                if (sqllogs.Any())
                {
                    CConfig.PbPercentage = ((double)(logs.Count + sqllogs.Count) / 100);
                    MessageBoxResult result = DisplayMessageBox.Show(MsgBoxTitle.WarningTitle, ApplicationConstants.Db_backup, MessageBoxButton.YesNo, Wins.MessageBoxImage.Warning);
                    if (result == MessageBoxResult.Yes)
                    {
                        await ExecuteVisiitorPointLogs(logs, sqllogs);
                    }
                    else
                    {
                        Application.Current.Shutdown();
                    }
                }
                else
                {
                    await ExecuteVisiitorPointLogs(logs, sqllogs);
                }

            }
            catch (Exception ex)
            {
                Helper.WriteLog(ex);

            }

        }
        private async Task ExecuteQuanikaLogs(List<UpdateLogs> logs, List<UpdateLogs> sqllogs)
        {
            if (logs.Any() || sqllogs.Any())
            {
                NextButtonWizardFunctionality(3);
                await Task.Run(() => Helper.TaskDealy());
                List<string> distinctVersion = new List<string>();

                distinctVersion = logs.Select(x => x.version).Distinct().ToList();
                if (sqllogs.Any())
                {
                    distinctVersion.AddRange(sqllogs.Select(x => x.version).Distinct().ToList());
                    distinctVersion = distinctVersion.Select(x => x).Distinct().OrderBy(x => x).ToList();
                }

                foreach (var ver in distinctVersion)
                {
                    #region Execute Sql Logs
                    var exist = sqllogs.Where(f => f.version == ver && f.status == false).ToList();
                    if (exist.Any())
                    {
                        // Execute sql logs
                        this.pbLabel.Text = ApplicationConstants.Execute_SQl_Logs;

                        Response resSql = await Task.Run(() => Helper.ExecuteSqlLogs(this, exist));
                        if (resSql.status == false)
                        {
                            if (db.UpdateLogStatus(ver, CConfig.Hostname))
                            {
                                MessageBoxResult logsresult = DisplayMessageBox.Show("error", resSql.message, MessageBoxButton.OK, Wins.MessageBoxImage.Error);
                                if (logsresult == MessageBoxResult.OK)
                                {
                                    Application.Current.Shutdown();
                                }
                            }
                            else
                            {
                                MessageBoxResult logsresult = DisplayMessageBox.Show("error", ApplicationConstants.update_logs_error, MessageBoxButton.OK, Wins.MessageBoxImage.Error);
                                if (logsresult == MessageBoxResult.OK)
                                {
                                    Application.Current.Shutdown();
                                }
                            }
                        }

                    }

                    #endregion

                    #region Execute Dll Logs
                    var dllLogs = logs.Where(f => f.version == ver && f.Command.TrimEnd().EndsWith(".dll") && f.status == false).ToList();
                    if (dllLogs.Any())
                    {
                        this.pbLabel.Text = ApplicationConstants.Execute_DLL_Logs;
                        Response resDll = await Task.Run(() => Helper.ExecuteQuanikaLogs(this, dllLogs, ApplicationConstants.Dll_Extension, ApplicationConstants.updates_dll_Folder_Name, ApplicationConstants.backup_dll_Folder_Name));
                        if (resDll.status == false)
                        {
                            MessageBoxResult logsresult = DisplayMessageBox.Show("error", resDll.message, MessageBoxButton.OK, Wins.MessageBoxImage.Error);
                            if (logsresult == MessageBoxResult.OK)
                            {
                                Application.Current.Shutdown();
                            }
                        }
                    }
                    #endregion

                    #region Execute Exe Logs
                    var exeLogs = logs.Where(f => f.version == ver && f.Command.TrimEnd().EndsWith(".exe") && f.status == false).ToList();
                    if (exeLogs.Any())
                    {
                        this.LogLabel.Text = ApplicationConstants.Execute_Exe_Logs;
                        Response resExe = await Task.Run(() => Helper.ExecuteQuanikaLogs(this, exeLogs, ApplicationConstants.Exe_Extension, ApplicationConstants.updates_exe_Folder_Name, ApplicationConstants.backup_exe_Folder_Name));
                        if (resExe.status == false)
                        {
                            MessageBoxResult logsresult = DisplayMessageBox.Show("error", resExe.message, MessageBoxButton.OK, Wins.MessageBoxImage.Error);
                            if (logsresult == MessageBoxResult.OK)
                            {
                                Application.Current.Shutdown();
                            }
                        }
                    }
                    #endregion

                    #region Execute File Logs
                    var anyfileLogs = logs.Where(i => i.version == ver && !i.Command.TrimEnd().EndsWith(".exe") && !i.Command.TrimEnd().EndsWith(".dll") && !i.Command.TrimEnd().EndsWith(".sql") && i.status == false).ToList();
                    if (anyfileLogs.Any())
                    {
                        this.LogLabel.Text = ApplicationConstants.Execute_Exe_Logs;
                        Response resanyfile = await Task.Run(() => Helper.ExecuteQuanikaLogs(this, anyfileLogs, null, ApplicationConstants.updates_files_Folder_Name, null));
                        if (resanyfile.status == false)
                        {
                            MessageBoxResult logsresult = DisplayMessageBox.Show("error", resanyfile.message, MessageBoxButton.OK, Wins.MessageBoxImage.Error);
                            if (logsresult == MessageBoxResult.OK)
                            {
                                Application.Current.Shutdown();
                            }

                        }
                    }
                    #endregion

                    if (!db.CheckIfUpdateVersion(ver, CConfig.Hostname))
                    {
                        this.pbLabel.Text = "Updating version to " + ver + " in config";
                        CConfig.Setting.version = ver;
                        Helper.updateVersionInConfig(ver);
                    }
                }

                if (db.CheckIfUpdated(distinctVersion))
                {
                    NextButtonWizardFunctionality(4);
                }
                else
                {

                    NextButtonWizardFunctionality(5);
                }

            }
            else
            {
                MessageBoxResult result = DisplayMessageBox.Show("error", ApplicationConstants.No_Logs, MessageBoxButton.OK, Wins.MessageBoxImage.Error);
                if (result == MessageBoxResult.OK)
                {
                    Application.Current.Shutdown();
                }


            }

        }
        private async Task ExecuteVisiitorPointLogs(List<UpdateLogs> logs, List<UpdateLogs> sqllogs)
        {
            if (logs.Count > 0 || sqllogs.Count > 0)
            {
                NextButtonWizardFunctionality(3);
                await Task.Run(() => Helper.TaskDealy());
                List<string> distinctVersion = new List<string>();

                distinctVersion = logs.Select(x => x.version).Distinct().ToList();
                if (sqllogs.Any())
                {
                    distinctVersion.AddRange(sqllogs.Select(x => x.version).Distinct().ToList());
                    distinctVersion = distinctVersion.Select(x => x).Distinct().OrderBy(x => x).ToList();
                }

                foreach (var ver in distinctVersion)
                {
                    #region Execute Sql Logs
                    var exist = sqllogs.Where(f => f.version == ver && f.status == false).ToList();
                    if (exist.Any())
                    {
                        // Execute sql logs
                        this.pbLabel.Text = ApplicationConstants.Execute_SQl_Logs;

                        Response resSql = await Task.Run(() => Helper.ExecuteSqlLogs(this, exist));
                        if (resSql.status == false)
                        {
                            if (db.UpdateLogStatus(ver, CConfig.Hostname))
                            {
                                MessageBoxResult logsresult = DisplayMessageBox.Show("error", resSql.message, MessageBoxButton.OK, Wins.MessageBoxImage.Error);
                                if (logsresult == MessageBoxResult.OK)
                                {
                                    Application.Current.Shutdown();
                                }
                            }
                            else
                            {
                                MessageBoxResult logsresult = DisplayMessageBox.Show("error", ApplicationConstants.update_logs_error, MessageBoxButton.OK, Wins.MessageBoxImage.Error);
                                if (logsresult == MessageBoxResult.OK)
                                {
                                    Application.Current.Shutdown();
                                }
                            }
                        }

                    }

                    #endregion


                    var clientApplicationLogs = logs.Where(f => f.version == ver && f.Command.Contains(VpXmlTags.ClientApplication) && f.status == false).ToList() ?? new List<UpdateLogs>();
                    if (clientApplicationLogs.Count > 0)
                    {
                        this.pbLabel.Text = ApplicationConstants.Execute_DLL_Logs;
                        Response resDll = await Task.Run(() => Helper.ExecuteVPLogs(this, clientApplicationLogs, VpPatchFolders.ClientApplication, VpPatchBackupFolders.ClientApplication));
                        if (resDll.status == false)
                        {
                            MessageBoxResult logsresult = DisplayMessageBox.Show("error", resDll.message, MessageBoxButton.OK, Wins.MessageBoxImage.Error);
                            if (logsresult == MessageBoxResult.OK)
                            {
                                Application.Current.Shutdown();
                            }
                        }
                    }
                    var comServiceLogs = logs.Where(f => f.version == ver && f.Command.Contains(VpXmlTags.ComService) && f.status == false).ToList() ?? new List<UpdateLogs>();
                    if (comServiceLogs.Count > 0)
                    {
                        this.pbLabel.Text = ApplicationConstants.Execute_DLL_Logs;
                        Response resDll = await Task.Run(() => Helper.ExecuteVPLogs(this, comServiceLogs, VpPatchFolders.ComService, VpPatchBackupFolders.ComService));
                        if (resDll.status == false)
                        {
                            MessageBoxResult logsresult = DisplayMessageBox.Show("error", resDll.message, MessageBoxButton.OK, Wins.MessageBoxImage.Error);
                            if (logsresult == MessageBoxResult.OK)
                            {
                                Application.Current.Shutdown();
                            }
                        }
                    }
                    var dataUploadBoatLogs = logs.Where(f => f.version == ver && f.Command.Contains(VpXmlTags.DataUploadBot) && f.status == false).ToList() ?? new List<UpdateLogs>();
                    if (dataUploadBoatLogs.Count > 0)
                    {
                        this.pbLabel.Text = ApplicationConstants.Execute_DLL_Logs;
                        Response resDll = await Task.Run(() => Helper.ExecuteVPLogs(this, dataUploadBoatLogs, VpPatchFolders.DataUploadBot, VpPatchBackupFolders.DataUploadBot));
                        if (resDll.status == false)
                        {
                            MessageBoxResult logsresult = DisplayMessageBox.Show("error", resDll.message, MessageBoxButton.OK, Wins.MessageBoxImage.Error);
                            if (logsresult == MessageBoxResult.OK)
                            {
                                Application.Current.Shutdown();
                            }
                        }
                    }
                    var meetingCreatorBot = logs.Where(f => f.version == ver && f.Command.Contains(VpXmlTags.MeetingCreatorBot) && f.status == false).ToList() ?? new List<UpdateLogs>();
                    if (meetingCreatorBot.Count > 0)
                    {
                        this.pbLabel.Text = ApplicationConstants.Execute_DLL_Logs;
                        Response resDll = await Task.Run(() => Helper.ExecuteVPLogs(this, meetingCreatorBot, VpPatchFolders.MeetingCreatorBot, VpPatchBackupFolders.MeetingCreatorBot));
                        if (resDll.status == false)
                        {
                            MessageBoxResult logsresult = DisplayMessageBox.Show("error", resDll.message, MessageBoxButton.OK, Wins.MessageBoxImage.Error);
                            if (logsresult == MessageBoxResult.OK)
                            {
                                Application.Current.Shutdown();
                            }
                        }
                    }
                    var kioskLogs = logs.Where(f => f.version == ver && f.Command.Contains(VpXmlTags.VisitorPointKiosk) && f.status == false).ToList() ?? new List<UpdateLogs>();
                    if (kioskLogs.Count > 0)
                    {
                        this.pbLabel.Text = ApplicationConstants.Execute_DLL_Logs;
                        Response resDll = await Task.Run(() => Helper.ExecuteVPLogs(this, kioskLogs, VpPatchFolders.VisitorPointKiosk, VpPatchBackupFolders.VisitorPointKiosk));
                        if (resDll.status == false)
                        {
                            MessageBoxResult logsresult = DisplayMessageBox.Show("error", resDll.message, MessageBoxButton.OK, Wins.MessageBoxImage.Error);
                            if (logsresult == MessageBoxResult.OK)
                            {
                                Application.Current.Shutdown();
                            }
                        }
                    }
                    var outlookLogs = logs.Where(f => f.version == ver && f.Command.Contains(VpXmlTags.Outlook) && f.status == false).ToList() ?? new List<UpdateLogs>();
                    if (outlookLogs.Count > 0 && ApplicationConstants.IncludeOutlook)
                    {
                        this.pbLabel.Text = ApplicationConstants.Execute_DLL_Logs;
                        Response resDll = await Task.Run(() => Helper.ExecuteVPLogs(this, outlookLogs, VpPatchFolders.Outlook, VpPatchBackupFolders.Outlook));
                        if (resDll.status == false)
                        {
                            MessageBoxResult logsresult = DisplayMessageBox.Show("error", resDll.message, MessageBoxButton.OK, Wins.MessageBoxImage.Error);
                            if (logsresult == MessageBoxResult.OK)
                            {
                                Application.Current.Shutdown();
                            }
                        }
                    }
                    var webLogs = logs.Where(f => f.version == ver && f.Command.Contains(VpXmlTags.Web) && f.status == false).ToList() ?? new List<UpdateLogs>();
                    if (webLogs.Count > 0)
                    {
                        this.pbLabel.Text = ApplicationConstants.Execute_DLL_Logs;
                        Response resDll = await Task.Run(() => Helper.ExecuteVPLogs(this, webLogs, VpPatchFolders.Web, VpPatchBackupFolders.Web));
                        if (resDll.status == false)
                        {
                            MessageBoxResult logsresult = DisplayMessageBox.Show("error", resDll.message, MessageBoxButton.OK, Wins.MessageBoxImage.Error);
                            if (logsresult == MessageBoxResult.OK)
                            {
                                Application.Current.Shutdown();
                            }
                        }
                    }
                    var webRegLogs = logs.Where(f => f.version == ver && f.Command.Contains(VpXmlTags.WebReg) && f.status == false).ToList() ?? new List<UpdateLogs>();
                    if (webRegLogs.Count > 0)
                    {
                        this.pbLabel.Text = ApplicationConstants.Execute_DLL_Logs;
                        Response resDll = await Task.Run(() => Helper.ExecuteVPLogs(this, webRegLogs, VpPatchFolders.WebReg, VpPatchBackupFolders.WebReg));
                        if (resDll.status == false)
                        {
                            MessageBoxResult logsresult = DisplayMessageBox.Show("error", resDll.message, MessageBoxButton.OK, Wins.MessageBoxImage.Error);
                            if (logsresult == MessageBoxResult.OK)
                            {
                                Application.Current.Shutdown();
                            }
                        }
                    }
                    var qrwWebLogs = logs.Where(f => f.version == ver && f.Command.Contains(VpXmlTags.QrWeb) && f.status == false).ToList() ?? new List<UpdateLogs>();
                    if (qrwWebLogs.Count > 0)
                    {
                        this.pbLabel.Text = ApplicationConstants.Execute_DLL_Logs;
                        Response resDll = await Task.Run(() => Helper.ExecuteVPLogs(this, qrwWebLogs, VpPatchFolders.QrWeb, VpPatchBackupFolders.QrWeb));
                        if (resDll.status == false)
                        {
                            MessageBoxResult logsresult = DisplayMessageBox.Show("error", resDll.message, MessageBoxButton.OK, Wins.MessageBoxImage.Error);
                            if (logsresult == MessageBoxResult.OK)
                            {
                                Application.Current.Shutdown();
                            }
                        }
                    }


                    if (!db.CheckIfUpdateVersion(ver, CConfig.Hostname))
                    {
                        this.pbLabel.Text = "Updating version to " + ver + " in config";
                        CConfig.Setting.version = ver;
                        Helper.UpdateVpVersionInConfig(ver);
                    }
                }

                if (db.CheckIfUpdated(distinctVersion))
                {
                    NextButtonWizardFunctionality(4);
                }
                else
                {

                    NextButtonWizardFunctionality(5);
                }

            }
            else
            {
                MessageBoxResult result = DisplayMessageBox.Show("error", ApplicationConstants.No_Logs, MessageBoxButton.OK, Wins.MessageBoxImage.Error);
                if (result == MessageBoxResult.OK)
                {
                    Application.Current.Shutdown();
                }


            }

        }
        public void worker_ProgressChanged(double Percentage)
        {
            try
            {
                //safe call
                Dispatcher.Invoke(() =>
                {
                    pbStatus.Value = pbStatus.Value + Percentage;
                });
            }
            catch (Exception ex)
            {
                Helper.WriteLog(ex);
            }

        }
        public void UpdateWindow(string text)
        {
            try
            {
                //safe call
                Dispatcher.Invoke(() =>
                {
                    LogLabel.Text = text;
                });
            }
            catch (Exception ex)
            {
                Helper.WriteLog(ex);
            }
        }

        public void UpdatePbLabel(string text)
        {
            try
            {
                //safe call
                Dispatcher.Invoke(() =>
                {
                    pbLabel.Text = text;
                });
            }
            catch (Exception ex)
            {
                Helper.WriteLog(ex);
            }
        }
        private void btnCloseApp_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        #region NextButtonWizardFunctionality
        private void NextButtonWizardFunctionality(int nextGrid)
        {
            try
            {
                switch (nextGrid)
                {
                    case 1:
                        //Right Side4
                        hideAllBodyGrid();
                        grdContentBody1.Visibility = Visibility.Visible;
                        break;
                    case 2:
                        //Right Side
                        hideAllBodyGrid();
                        grdContentBody2.Visibility = Visibility.Visible;
                        break;
                    case 3:
                        //Right Side
                        hideAllBodyGrid();
                        grdContentBody3.Visibility = Visibility.Visible;
                        break;
                    case 4:
                        //Right Side
                        hideAllBodyGrid();
                        getAppStatuses();
                        grdContentInstalled.Visibility = Visibility.Visible;

                        break;
                    case 5:
                        //Right Side
                        hideAllBodyGrid();
                        if (UpgradeUtility is UpgradeUtility.VisitorPoint)
                        {
                            GetVPUpdateLogs();
                        }
                        else
                        {
                            GetUpdateLogs();
                        }
                        grdFinish.Visibility = Visibility.Visible;

                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Helper.WriteLog(ex);
            }
        }



        #endregion

        #region PreviousButtonWizardFunctionality
        private void PreviousButtonWizardFunctionality(int previousGrid)
        {
            switch (previousGrid)
            {
                case 1:
                    //Right Side
                    hideAllBodyGrid();
                    grdContentBody1.Visibility = Visibility.Visible;
                    break;
                case 2:
                    //Right Side
                    hideAllBodyGrid();
                    grdContentBody2.Visibility = Visibility.Visible;
                    break;
                case 3:
                    //Right Side
                    hideAllBodyGrid();
                    grdContentBody3.Visibility = Visibility.Visible;
                    break;
                default:
                    break;
            }

        }

        #endregion

        private void hideAllBodyGrid()
        {
            grdContentBody1.Visibility = Visibility.Collapsed;
            grdContentBody2.Visibility = Visibility.Collapsed;
            grdContentBody3.Visibility = Visibility.Collapsed;
            grdContentInstalled.Visibility = Visibility.Collapsed;
            grdFinish.Visibility = Visibility.Collapsed;
        }
        private async void getAppStatuses()
        {
            this.installed_apps = await Task.Run(() => db.getAppStatuses());
            this.DataContext = this.installed_apps;
        }


        private void GetUpdateLogs()
        {
            this.installed_logs = new List<Models.Logs>();
            string loc = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var serializer = new XmlSerializer(typeof(Updates));
            using (var stream = new StreamReader(loc + "\\Updates\\" + CConfig.Setting.version + "\\version.xml"))
            {
                var deserializeData = (Updates)serializer.Deserialize(stream);

                var logs = deserializeData?.Files?.Log;
                if (logs is null)
                {
                    return;
                }
                foreach (var data in logs)
                {
                    Logs log = new Logs();
                    log.command = data;
                    this.installed_logs.Add(log);
                }
            }
            this.DataContext = this.installed_logs;


        }

        private void GetVPUpdateLogs()
        {
            this.installed_logs = new List<Models.Logs>();
            string loc = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var serializer = new XmlSerializer(typeof(VpVersion));
            using (var stream = new StreamReader(loc + "\\Updates\\" + CConfig.Setting.version + "\\version.xml"))
            {
                var deserializeData = (Updates)serializer.Deserialize(stream);

                var logs = deserializeData?.Files?.Log;
                if (logs is null)
                {
                    return;
                }
                foreach (var data in logs)
                {
                    Logs log = new Logs();
                    log.command = data;
                    this.installed_logs.Add(log);
                }
            }
            this.DataContext = this.installed_logs;


        }
        private void imgSqlInstance_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                this.Cursor = System.Windows.Input.Cursors.Wait;
                var servers = Helper.GetSqlServerInsances();
                this.Cursor = System.Windows.Input.Cursors.Arrow;
                SqlServerInstancesWin frm = new SqlServerInstancesWin(servers);
                frm.ShowDialog();
                txtServer.Text = frm.serverName;
            }
            catch (Exception ex)
            {
                Helper.WriteLog(ex);
            }
        }

        private void btnCancel4_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch (Exception)
            {

            }
        }
        private void btnFinish_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void imgSqlInstance_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                var servers = Helper.GetSqlServerInsances();
                Mouse.OverrideCursor = null;
                if (servers != null && servers.Count > 0)
                {
                    Window yourParentWindow = Window.GetWindow(MainWin);
                    //Center the main screen
                    Helper.CenterWindowOnScreen(yourParentWindow);
                    SqlServerInstancesWin frm = new SqlServerInstancesWin(servers);
                    frm.Owner = yourParentWindow;
                    frm.ShowDialog();
                    if (!string.IsNullOrEmpty(frm.serverName))
                    {
                        txtServer.Text = frm.serverName;
                        txtUsername.Text = "";
                        txtPassword.Password = "";
                    }
                }
            }
            catch (Exception)
            {
                //show error 
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }
        }

        private void btnConfigureDatabase_Click(object sender, RoutedEventArgs e)
        {

        }

        #region ShowDBServerPasswrodFunctionality
        private void imgPwd_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            txtDBPwdSql.Text = txtPassword.Password.ToString();
            txtPassword.Visibility = Visibility.Collapsed;
            txtDBPwdSql.Visibility = Visibility.Visible;
        }

        private void imgPwd_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            txtPassword.Visibility = Visibility.Visible;
            txtPassword.Focus();
            txtDBPwdSql.Visibility = Visibility.Collapsed;
        }
        #endregion



        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void CheckApplicationStatuses()
        {
            if (UpgradeUtility is UpgradeUtility.Quanika)
            {
                CheckIsQuanikaModulesInstalled();
                AddQuanikaVersionsInVersionList();
            }
            else if (UpgradeUtility is UpgradeUtility.VisitorPoint)
            {
                CheckIsVpModulesInstalled();
                AddVPVersionsInVersionList();
            }

        }

        private static void CheckIsVpModulesInstalled()
        {
            var pathHelper = new PathsHelper();

            CConfig.IsClientApplicationInstalled = Helper.CheckInstalled(ApplicationConstants.Client_Application_Name);
            CConfig.IsDataUploadBoatInstalled = Helper.CheckInstalled(ApplicationConstants.Data_Upload_Bot_Name);
            CConfig.IsMeetingCreatorBotInstalled = Helper.CheckInstalled(ApplicationConstants.Meeting_Creator_Bot_Name);
            CConfig.IsComServiceInstalled = Helper.CheckInstalled(ApplicationConstants.Com_Service_Name);
            CConfig.IsOutlookInstalled = Helper.CheckInstalled(ApplicationConstants.VisitorPoint_Oulook_Name);

            CConfig.IsWebInstalled = pathHelper.IsWebInstalled(ApplicationConstants.VisitorPoint_Web_Name);
            CConfig.IsWebRegInstalled = pathHelper.IsWebInstalled(ApplicationConstants.VisitorPoint_Web_Reg_Name);
            CConfig.IsQrWebInstalled = pathHelper.IsWebInstalled(ApplicationConstants.VisitorPoint_Qr_Web_Name);
            CConfig.IsKioskInstalled = Helper.CheckInstalled(ApplicationConstants.VisitorPoint_Kiosk_Name);
        }

        private static void CheckIsQuanikaModulesInstalled()
        {
            CConfig.isApplicationInstalled = Helper.CheckInstalled(ApplicationConstants.Application_Name);
            CConfig.isDXInstalled = Helper.CheckInstalled(ApplicationConstants.Dx_Name);
            CConfig.isServiceInstalled = Helper.CheckInstalled(ApplicationConstants.Service_Name);
            CConfig.isADServiceInstalled = Helper.CheckInstalled(ApplicationConstants.ADService_Name);
            CConfig.isDXMONITORINGServiceInstalled = Helper.CheckInstalled(ApplicationConstants.DXMonitoring_Service_Name);
            CConfig.isLPNServiceInstalled = Helper.CheckInstalled(ApplicationConstants.LPN_Service_Name);
            CConfig.isOfflineTaskServiceInstalled = Helper.CheckInstalled(ApplicationConstants.OfflineTask_Service_Name);
        }

        private void AddQuanikaVersionsInVersionList()
        {
            decimal application_version, dx_version, service_version, ad_version = 0, dxMonitoring_version = 0, offlinetask_version = 0, lpn_version = 0;

            #region Check Version 
            if (CConfig.isApplicationInstalled)
            {
                CConfig.Application_version = Helper.GetApplicationVersion(ApplicationConstants.APP_INSTALLED_EXE_PATH_X64);
                Decimal.TryParse(Helper.CleanVersion(CConfig.Application_version), out application_version);
                VersionInfoList.Add(
                    new VersionInformation
                    {
                        App = ApplicationConstants.Application_Name,
                        isInstalled = CConfig.isApplicationInstalled,
                        version = application_version
                    });
            }
            if (CConfig.isDXInstalled)
            {
                CConfig.Dx_version = Helper.GetApplicationVersion(ApplicationConstants.DX_INSTALLED_CONFIG_PATH_x64);
                Decimal.TryParse(Helper.CleanVersion(CConfig.Dx_version), out dx_version);
                VersionInfoList.Add(
                  new VersionInformation
                  {
                      App = ApplicationConstants.Dx_Name,
                      isInstalled = CConfig.isDXInstalled,
                      version = dx_version
                  });
            }
            if (CConfig.isServiceInstalled)
            {
                CConfig.Service_version = Helper.GetApplicationVersion(ApplicationConstants.Service_INSTALLED_CONFIG_PATH_X64);
                Decimal.TryParse(Helper.CleanVersion(CConfig.Service_version), out service_version);
                VersionInfoList.Add(
              new VersionInformation
              {
                  App = ApplicationConstants.Service_Name,
                  isInstalled = CConfig.isServiceInstalled,
                  version = service_version
              });
            }
            if (CConfig.isADServiceInstalled)
            {
                CConfig.ADService_version = Helper.GetApplicationVersion(ApplicationConstants.ADService_INSTALLED_CONFIG_PATH_X64);
                Decimal.TryParse(Helper.CleanVersion(CConfig.ADService_version), out ad_version);
                VersionInfoList.Add(
              new VersionInformation
              {
                  App = ApplicationConstants.Dx_Name,
                  isInstalled = CConfig.isADServiceInstalled,
                  version = ad_version
              });
            }
            if (CConfig.isDXMONITORINGServiceInstalled)
            {
                CConfig.DXMONITORING_Service_version = Helper.GetApplicationVersion(ApplicationConstants.DXMonitoring_INSTALLED_CONFIG_PATH_X64);
                Decimal.TryParse(Helper.CleanVersion(CConfig.DXMONITORING_Service_version), out dxMonitoring_version);
                VersionInfoList.Add(
              new VersionInformation
              {
                  App = ApplicationConstants.DXMonitoring_Service_Name,
                  isInstalled = CConfig.isDXMONITORINGServiceInstalled,
                  version = dxMonitoring_version
              });
            }
            if (CConfig.isOfflineTaskServiceInstalled)
            {
                CConfig.OfflineTask_Service_version = Helper.GetApplicationVersion(ApplicationConstants.OFFLINE_TASK_SERVICE_INSTALLED_CONFIG_PATH_X64);
                Decimal.TryParse(Helper.CleanVersion(CConfig.OfflineTask_Service_version), out offlinetask_version);
                VersionInfoList.Add(
              new VersionInformation
              {
                  App = ApplicationConstants.OfflineTask_Service_Name,
                  isInstalled = CConfig.isOfflineTaskServiceInstalled,
                  version = offlinetask_version
              });
            }
            if (CConfig.isLPNServiceInstalled)
            {
                CConfig.LPN_Service_version = Helper.GetApplicationVersion(ApplicationConstants.LPN_INSTALLED_CONFIG_PATH_X64);
                Decimal.TryParse(Helper.CleanVersion(CConfig.LPN_Service_version), out lpn_version);
                VersionInfoList.Add(
              new VersionInformation
              {
                  App = ApplicationConstants.LPN_Service_Name,
                  isInstalled = CConfig.isLPNServiceInstalled,
                  version = lpn_version
              });

            }
            #endregion
        }

        private void AddVPVersionsInVersionList()
        {
            if (CConfig.IsClientApplicationInstalled)
            {
                string path = PathsHelper.GetVisitorPointInstallsedConfigPaths(VisitorPointDestination.ClientApplication);

                Helper.SetDbConfig(VisitorPointDestination.ClientApplication, path);

                CConfig.ClientApplicationVersion = Helper.GetApplicationVersion(path);
                if (!string.IsNullOrEmpty(CConfig.ClientApplicationVersion))
                {
                    decimal.TryParse(Helper.CleanVersion(CConfig.ClientApplicationVersion), out var s);

                    VersionInfoList.Add(
                        new VersionInformation
                        {
                            App = ApplicationConstants.Client_Application_Name,
                            isInstalled = CConfig.IsClientApplicationInstalled,
                            version = s
                        });
                }
            }
            if (CConfig.IsDataUploadBoatInstalled)
            {
                string path = PathsHelper.GetVisitorPointInstallsedConfigPaths(VisitorPointDestination.DataUploadBot);

                Helper.SetDbConfig(VisitorPointDestination.DataUploadBot, path);

                CConfig.DataUploadBotVersion = Helper.GetApplicationVersion(path);

                if (!string.IsNullOrEmpty(CConfig.DataUploadBotVersion))
                {
                    decimal.TryParse(Helper.CleanVersion(CConfig.DataUploadBotVersion), out var s);

                    VersionInfoList.Add(
                        new VersionInformation
                        {
                            App = ApplicationConstants.Data_Upload_Bot_Name,
                            isInstalled = CConfig.IsDataUploadBoatInstalled,
                            version = s
                        });
                }
            }
            if (CConfig.IsComServiceInstalled)
            {
                string path = PathsHelper.GetVisitorPointInstallsedConfigPaths(VisitorPointDestination.ComService);

                Helper.SetDbConfig(VisitorPointDestination.ComService, path);

                CConfig.ComServiceVersion = Helper.GetApplicationVersion(path);

                if (!string.IsNullOrEmpty(CConfig.ComServiceVersion))
                {
                    decimal.TryParse(Helper.CleanVersion(CConfig.ComServiceVersion), out var s);

                    VersionInfoList.Add(
                        new VersionInformation
                        {
                            App = ApplicationConstants.Com_Service_Name,
                            isInstalled = CConfig.IsComServiceInstalled,
                            version = s
                        });
                }

            }
            if (CConfig.IsKioskInstalled)
            {
                string path = PathsHelper.GetVisitorPointInstallsedConfigPaths(VisitorPointDestination.ClientApplication);

                Helper.SetDbConfig(VisitorPointDestination.Kiosk, path);

                CConfig.VPKioskVersion = Helper.GetApplicationVersion(path);

                if (!string.IsNullOrEmpty(CConfig.VPKioskVersion))
                {
                    decimal.TryParse(Helper.CleanVersion(CConfig.VPKioskVersion), out var s);

                    VersionInfoList.Add(
                        new VersionInformation
                        {
                            App = ApplicationConstants.VisitorPoint_Kiosk_Name,
                            isInstalled = CConfig.IsKioskInstalled,
                            version = s
                        });
                }
            }

            if (CConfig.IsOutlookInstalled && false)
            {
                string path = PathsHelper.GetVisitorPointInstallsedConfigPaths(VisitorPointDestination.Outlook);

                Helper.SetDbConfig(VisitorPointDestination.Outlook, path);

                CConfig.VPOutlookVersion = Helper.GetApplicationVersion(path);

                if (!string.IsNullOrEmpty(CConfig.VPOutlookVersion))
                {
                    decimal.TryParse(Helper.CleanVersion(CConfig.VPOutlookVersion), out var s);

                    VersionInfoList.Add(
                                     new VersionInformation
                                     {
                                         App = ApplicationConstants.VisitorPoint_Oulook_Name,
                                         isInstalled = CConfig.isApplicationInstalled,
                                         version = s
                                     });
                }

            }
            if (CConfig.IsMeetingCreatorBotInstalled)
            {
                string path = PathsHelper.GetVisitorPointInstallsedConfigPaths(VisitorPointDestination.MeetingCreatorBot);

                Helper.SetDbConfig(VisitorPointDestination.MeetingCreatorBot, path);

                CConfig.MeetingCreatorBotVersion = Helper.GetApplicationVersion(path);

                if (!string.IsNullOrEmpty(CConfig.MeetingCreatorBotVersion))
                {
                    decimal.TryParse(Helper.CleanVersion(CConfig.MeetingCreatorBotVersion), out var s);

                    VersionInfoList.Add(
                        new VersionInformation
                        {
                            App = ApplicationConstants.Meeting_Creator_Bot_Name,
                            isInstalled = CConfig.IsMeetingCreatorBotInstalled,
                            version = s
                        });
                }

            }
            if (CConfig.IsWebInstalled)
            {
                var path = PathsHelper.GetVisitorPointPaths(VisitorPointDestination.Web);

                CConfig.VPWebVersion = Helper.GetWebVersion(path);

                Helper.SetDbConfig(VisitorPointDestination.Web, path);
                if (!string.IsNullOrEmpty(CConfig.VPWebVersion))
                {
                    decimal.TryParse(Helper.CleanVersion(CConfig.VPWebVersion), out var s);

                    VersionInfoList.Add(
                        new VersionInformation
                        {
                            App = ApplicationConstants.VisitorPoint_Web_Name,
                            isInstalled = CConfig.IsWebInstalled,
                            version = s
                        });
                }
            }


            if (CConfig.IsWebRegInstalled)
            {
                var path = PathsHelper.GetVisitorPointPaths(VisitorPointDestination.WebReg);

                Helper.SetDbConfig(VisitorPointDestination.WebReg, path);

                CConfig.VPWebRegVersion = Helper.GetWebVersion(path);

                if (!string.IsNullOrEmpty(CConfig.VPWebRegVersion))
                {
                    decimal.TryParse(Helper.CleanVersion(CConfig.VPWebRegVersion), out var s);

                    VersionInfoList.Add(
                        new VersionInformation
                        {
                            App = ApplicationConstants.VisitorPoint_Web_Reg_Name,
                            isInstalled = CConfig.IsWebRegInstalled,
                            version = s
                        });
                }

            }
            if (CConfig.IsQrWebInstalled)
            {
                var path = PathsHelper.GetVisitorPointPaths(VisitorPointDestination.QrWeb);

                Helper.SetDbConfig(VisitorPointDestination.QrWeb, path);

                CConfig.VPQrWeb = Helper.GetWebVersion(path);

                if (!string.IsNullOrEmpty(CConfig.VPQrWeb))
                {
                    decimal.TryParse(Helper.CleanVersion(CConfig.VPQrWeb), out var s);

                    VersionInfoList.Add(
                        new VersionInformation
                        {
                            App = ApplicationConstants.VisitorPoint_Qr_Web_Name,
                            isInstalled = CConfig.IsQrWebInstalled,
                            version = s
                        });
                }

            }
        }

        private async void btnBrowseUpdates_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
                ofd.Multiselect = true;
                ofd.Filter = "ZIP files (.zip)|*.zip";
                FolderBrowserDialog fbd = new FolderBrowserDialog();

                if (ofd.ShowDialog() == true)
                {
                    this.LogLabel.Text = ApplicationConstants.Extract_Updates;
                    this.loader.Visibility = Visibility.Visible;
                    string labelText = string.Empty;
                    string loc = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                    foreach (var file in ofd.FileNames)
                    {
                        this.btnNext1.IsEnabled = false;
                        this.btnBrowse.IsEnabled = false;
                        string folderName = Path.GetFileNameWithoutExtension(file);
                        if (Directory.Exists(loc + "\\Updates\\" + folderName))
                        {
                            MessageBoxResult result = DisplayMessageBox.Show(MsgBoxTitle.ErrorTitle, folderName + " already exists. Do you want to replace this?", MessageBoxButton.YesNo, QuanikaUpdate.Wins.MessageBoxImage.Error);
                            if (result == MessageBoxResult.Yes)
                            {
                                labelText = ApplicationConstants.Extract_Finish;
                                await Task.Run(() => Helper.ExtractToDirectory(file, loc + "\\Updates", true));
                            }
                            else
                            {
                                labelText = string.Empty;
                                continue;
                            }
                        }
                        else
                        {
                            labelText = ApplicationConstants.Extract_Finish;
                            await Task.Run(() => Helper.ExtractToDirectory(file, loc + "\\Updates", true));
                        }
                    }
                    this.loader.Visibility = Visibility.Collapsed;
                    this.LogLabel.Text = labelText;
                    this.btnBrowse.IsEnabled = true;
                    this.btnNext1.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                Helper.writeLog(ex.Message);

            }
        }
    }
}