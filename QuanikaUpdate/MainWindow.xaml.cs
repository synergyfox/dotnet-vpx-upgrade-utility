using AxisInstallerPackage.Wins;
using Microsoft.Win32;
using QuanikaUpdate.Helpers;
using QuanikaUpdate.Models;
using QuanikaUpdate.UpgradeManagers;
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
using Application = System.Windows.Application;

namespace QuanikaUpdate
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DAL db;
        FTPHelper ftp;
        public List<InstalledVersionModel> installed_apps { get; set; }
        public List<Logs> installed_logs { get; set; }
        public List<VersionInformation> VersionInfoList;
        public MainWindow()
        {
            InitializeComponent();
            db = new DAL();
            ftp = new FTPHelper();
            VersionInfoList = new List<VersionInformation>();
            txtVersion.Text = Helper.getAppSetting("version");


        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtVersion.Text = ConfigurationManager.AppSettings["version"].ToString();
            NextButtonWizardFunctionality(1);
            // checkApplicationStatuses();
        }

        #region btnNext
        private void btnNext1_Click(object sender, RoutedEventArgs e)
        {
            UpdatePbLabel("Test Log");

            UpgradeManager upgradeManager = new VisitorPointUpgradeManager();
            upgradeManager.Manage("4.0.28", this);
            this.loader.Visibility = Visibility.Visible;
            NextButtonWizardFunctionality(3);


            //btnNext1.IsEnabled = false;
            //#region Check if Application/Dx/Service Installed  and running
            //if (CConfig.IsClientApplicationInstalled || CConfig.IsComServiceInstalled || CConfig.IsDataUploadBoatInstalled || CConfig.IsMeetingCreatorBotInstalled)
            //{
            //    // Check If Application Running
            //    if (CConfig.IsClientApplicationInstalled)
            //    {
            //        Helper.CheckIfApplicationRunning(_Const.Client_Application_PROCESS_NAME);
            //    }
            //    // Check If Data Exchange Running
            //    if (CConfig.IsComServiceInstalled)
            //    {
            //        Helper.CheckIfApplicationRunning(_Const.Com_Service_PROCESS_NAME);
            //    }
            //    // Check If Service Running
            //    if (CConfig.IsDataUploadBoatInstalled)
            //    {
            //        Helper.CheckIfApplicationRunning(_Const.Data_Upload_Bot_PROCESS_NAME);

            //    }
            //    // Check If Quanika Active Directory Service Running
            //    if (CConfig.IsMeetingCreatorBotInstalled)
            //    {
            //        Helper.CheckIfApplicationRunning(_Const.Meeting_Creator_Bot_PROCESS_NAME);
            //    }

            //    //// Check If Quanika Offline Task Service Running
            //    //if (CConfig.isOfflineTaskServiceInstalled)
            //    //{
            //    //    Helper.CheckIfApplicationRunning(_Const.OfflineTask_App_Name);
            //    //}

            //    //// Check If Quanika LPN Service Running
            //    //if (CConfig.isLPNServiceInstalled)
            //    //{
            //    //    Helper.CheckIfApplicationRunning(_Const.LPN_App_Name);
            //    //}

            //    // Check If DX Monitoring Service Running
            //    if (CConfig.IsVPKioskInstalled)
            //    {
            //        Helper.CheckIfApplicationRunning(_Const.VisitorPoint_Kiosk_PROCESS_NAME);
            //    }
            //    if (CConfig.Setting.database == "" || CConfig.Setting.server == "" || CConfig.Setting.username == "" || CConfig.Setting.password == "")
            //    {
            //        NextButtonWizardFunctionality(2);

            //    }
            //    else
            //    {
            //        this.loader.Visibility = Visibility.Visible;
            // await ExecuteTask();

            //    }


            //}
            //#endregion
            //else
            //{


            //    MessageBoxResult result = DisplayMessageBox.Show(MsgBoxTitle.WarningTitle, _Const.QCS_Not_Installed, MessageBoxButton.OK, Wins.MessageBoxImage.Warning);

            //    if (result == MessageBoxResult.OK)
            //    {
            //        Application.Current.Shutdown();
            //    }


            //}

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
                    await ExecuteTask();
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
                    Helper.Run(_Const.DOT_NET_45_X64);
                }

                if (!Helper.IsApplictionInstalled("Microsoft System CLR Types for SQL Server 2014", out appguid))
                {
                    Helper.RunMsiSetup(_Const.GET_CLR_PATH);
                }

                if (!Helper.IsApplictionInstalled("Microsoft Report Viewer 2015 Runtime", out appguid))
                {
                    Helper.RunMsiSetup(_Const.GET_REPORT_VIEWER_PATH);
                }

                Helper.Run(_Const.GET_APPLICATION_PATH);
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

        void bindDbValues()
        {
            CConfig._db_server = Helper.getAppSetting("server");
            CConfig._database_name = Helper.getAppSetting("database");
            CConfig._db_username = Helper.getAppSetting("username");
            CConfig._db_pwd = Helper.Decrypt(Helper.getAppSetting("password"));
            txtServer.Text = CConfig._db_server;
            txtUsername.Text = CConfig._db_username;
            txtPassword.Password = CConfig._db_pwd;
        }

        async Task ExecuteTask()
        {

            this.LogLabel.Text = _Const.Verify_Version;

            decimal lowest_version = VersionInfoList.Min(entry => entry.Version);
            var VersionInfo = VersionInfoList.Where(entry => entry.Version == lowest_version);
            CConfig.Setting.version = lowest_version.ToString();


            this.LogLabel.Text = _Const.Checking_Updates;
            await Task.Run(() => Helper.taskDealy());

            #region FTP
            // Check if Ftp Enabled 

            //if (Helper.getAppSetting("FtpEnabled") == "1")
            //{
            //    Response res = await ftp.CheckUpdates(this, CConfig.Setting.version);
            //    if (res.status == true)
            //    {
            //        this.LogLabel.Text = res.message;
            //        await ExecuteUpdateLogs(_Const.execute_Update_Logs);

            //    }

            //    else if (res.status == false && res.message == _Const.No_Updates)
            //    {
            //        MessageBoxResult result = DisplayMessageBox.Show("Info", res.message, MessageBoxButton.OK, Wins.MessageBoxImage.Information);
            //        if (result == MessageBoxResult.OK)
            //        {
            //            Application.Current.Shutdown();
            //        }
            //    }
            //    else if (res.status == false && res.message == "Version exist")
            //    {
            //        await ExecuteUpdateLogs(_Const.execute_Pending_Logs);
            //    }

            //    else if (res.status == false && res.message == _Const.Con_Error_Ftp)
            //    {
            //        this.LogLabel.Text = _Const.Checking_LocalStorage;

            //        await CheckLocalStorage(CConfig.Setting.version);

            //    }
            //}
            //// Check For Local Storage
            //else
            #endregion

            this.LogLabel.Text = _Const.Checking_LocalStorage;
            await CheckLocalStorage(CConfig.Setting.version);
        }

        async Task CheckLocalStorage(string maxversion)
        {
            var sourceDirecotory = Assembly.GetExecutingAssembly().Location;

            string loc = Path.GetDirectoryName(sourceDirecotory);

            var directories = Directory.GetDirectories(loc + "\\Updates");

            var directoryFiles = directories.Select(Path.GetFullPath);

            List<string> directoriesToOperate = new List<string>();
            foreach (var item in directoryFiles)
            {
                var subDirecotories = Directory.GetDirectories(item).Select(x => new { FullPath = Path.GetFullPath(x), FileName = Path.GetFileName(x) });
                var botDirectory = subDirecotories.FirstOrDefault(x => x.FileName is "bot-data");
                if (botDirectory != null)
                {
                    var files = Directory.GetFiles(botDirectory.FullPath);
                    foreach (var file in files)
                    {

                    }
                }
                if (Helper.isSmallVersion(maxversion, item))
                {
                    directoriesToOperate.Add(item);
                }
            }

            await Task.Run(() => Helper.taskDealy());
            Response res2 = await Helper.CheckUpdates(this, maxversion);
            if (res2.status == true)
            {
                this.LogLabel.Text = res2.message;
                await ExecuteUpdateLogs(_Const.execute_Update_Logs);

            }
            else if (res2.status == false && res2.message == _Const.No_Updates)
            {
                MessageBoxResult result = DisplayMessageBox.Show("Info", res2.message, MessageBoxButton.OK, Wins.MessageBoxImage.Information);
                if (result == MessageBoxResult.OK)
                {
                    Application.Current.Shutdown();
                }
            }
            else if (res2.status == false && res2.message == "Version exist")
            {
                await ExecuteUpdateLogs(_Const.execute_Pending_Logs);
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

        async Task ExecuteUpdateLogs(string logsType)
        {
            try
            {

                string loc = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                List<UpdateLogs> logs = new List<UpdateLogs>();
                List<UpdateLogs> sqllogs = new List<UpdateLogs>();
                int Version = Convert.ToInt32(CConfig.Setting.version.Replace(".", ""));

                // Get Logs from database
                this.LogLabel.Text = _Const.Get_DB_logs;
                if (logsType == _Const.execute_Update_Logs)
                {
                    logs = await Task.Run(() => db.getUpdateLogs(Version, CConfig.Hostname));
                }
                else if (logsType == _Const.execute_Pending_Logs)
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
                    MessageBoxResult result = DisplayMessageBox.Show(MsgBoxTitle.WarningTitle, _Const.Db_backup, MessageBoxButton.YesNo, Wins.MessageBoxImage.Warning);
                    if (result == MessageBoxResult.Yes)
                    {
                        await ExecuteLogs(logs, sqllogs);
                    }
                    else
                    {
                        Application.Current.Shutdown();
                    }
                }
                else
                {
                    await ExecuteLogs(logs, sqllogs);
                }

            }
            catch (Exception ex)
            {
                Helper.writeLog(ex);

            }

        }

        async Task ExecuteLogs(List<UpdateLogs> logs, List<UpdateLogs> sqllogs)
        {
            if (logs.Any() || sqllogs.Any())
            {
                NextButtonWizardFunctionality(3);
                await Task.Run(() => Helper.taskDealy());
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
                        this.pbLabel.Text = _Const.Execute_SQl_Logs;

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
                                MessageBoxResult logsresult = DisplayMessageBox.Show("error", _Const.update_logs_error, MessageBoxButton.OK, Wins.MessageBoxImage.Error);
                                if (logsresult == MessageBoxResult.OK)
                                {
                                    Application.Current.Shutdown();
                                }
                            }
                        }

                    }

                    #endregion

                    #region Execute Dll Logs
                    var dllLogs = logs.Where(f => f.version == ver && f.command.TrimEnd().EndsWith(".dll") && f.status == false).ToList();
                    if (dllLogs.Any())
                    {
                        this.pbLabel.Text = _Const.Execute_DLL_Logs;
                        Response resDll = await Task.Run(() => Helper.ExecuteLogs(this, dllLogs, _Const.Dll_Extension, _Const.updates_dll_Folder_Name, _Const.backup_dll_Folder_Name));
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
                    var exeLogs = logs.Where(f => f.version == ver && f.command.TrimEnd().EndsWith(".exe") && f.status == false).ToList();
                    if (exeLogs.Any())
                    {
                        this.LogLabel.Text = _Const.Execute_Exe_Logs;
                        Response resExe = await Task.Run(() => Helper.ExecuteLogs(this, exeLogs, _Const.Exe_Extension, _Const.updates_exe_Folder_Name, _Const.backup_exe_Folder_Name));
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
                    var anyfileLogs = logs.Where(i => i.version == ver && !i.command.TrimEnd().EndsWith(".exe") && !i.command.TrimEnd().EndsWith(".dll") && !i.command.TrimEnd().EndsWith(".sql") && i.status == false).ToList();
                    if (anyfileLogs.Any())
                    {
                        this.LogLabel.Text = _Const.Execute_Exe_Logs;
                        Response resanyfile = await Task.Run(() => Helper.ExecuteLogs(this, anyfileLogs, null, _Const.updates_files_Folder_Name, null));
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
                MessageBoxResult result = DisplayMessageBox.Show("error", _Const.No_Logs, MessageBoxButton.OK, Wins.MessageBoxImage.Error);
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
                Helper.writeLog(ex);
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
                Helper.writeLog(ex);
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
                Helper.writeLog(ex);
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
                        // getAppStatuses();
                        grdContentInstalled.Visibility = Visibility.Visible;

                        break;
                    case 5:
                        //Right Side
                        hideAllBodyGrid();
                        // getUpdateLogs();
                        grdFinish.Visibility = Visibility.Visible;

                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Helper.writeLog(ex);
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


        private void getUpdateLogs()
        {
            this.installed_logs = new List<Models.Logs>();
            string loc = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var serializer = new XmlSerializer(typeof(Updates));
            using (var stream = new StreamReader(loc + "\\Updates\\" + CConfig.Setting.version + "\\version.xml"))
            {
                var deserializeData = (Updates)serializer.Deserialize(stream);
                foreach (var data in deserializeData.Files.Log)
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
                Helper.writeLog(ex);
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
            catch (Exception ex)
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
            catch (Exception ex)
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

        private void checkApplicationStatuses()
        {
            CConfig.IsClientApplicationInstalled = Helper.checkInstalled(_Const.Client_Application_Name);
            CConfig.IsComServiceInstalled = Helper.checkInstalled(_Const.Com_Service_Name);
            CConfig.IsDataUploadBoatInstalled = Helper.checkInstalled(_Const.Data_Upload_Bot_Name);
            CConfig.IsMeetingCreatorBotInstalled = Helper.checkInstalled(_Const.Meeting_Creator_Bot_Name);
            CConfig.IsVPKioskInstalled = Helper.checkInstalled(_Const.VisitorPoint_Kiosk_Name);
            //CConfig.isLPNServiceInstalled = Helper.checkInstalled(_Const.LPN_Service_Name);
            //CConfig.isOfflineTaskServiceInstalled = Helper.checkInstalled(_Const.OfflineTask_Service_Name);
            decimal application_version, dx_version, service_version,
                ad_version = 0, dxMonitoring_version = 0, offlinetask_version = 0, lpn_version = 0;

            #region Check Version 
            if (CConfig.IsClientApplicationInstalled)
            {
                CConfig.ClientApplicationVersion = Helper.GetApplicationVersion(_Const.Client_Application_INSTALLED_CONFIG_PATH_x64);
                Decimal.TryParse(Helper.cleanVersion(CConfig.ClientApplicationVersion), out application_version);
                VersionInfoList.Add(
                    new VersionInformation
                    {
                        App = _Const.Client_Application_Name,
                        IsInstalled = CConfig.IsClientApplicationInstalled,
                        Version = application_version
                    });
            }
            if (CConfig.IsComServiceInstalled)
            {
                CConfig.ComServiceVersion = Helper.GetApplicationVersion(_Const.Com_Service_INSTALLED_CONFIG_PATH_x64);
                Decimal.TryParse(Helper.cleanVersion(CConfig.ComServiceVersion), out dx_version);
                VersionInfoList.Add(
                  new VersionInformation
                  {
                      App = _Const.Com_Service_Name,
                      IsInstalled = CConfig.IsComServiceInstalled,
                      Version = dx_version
                  });
            }
            if (CConfig.IsDataUploadBoatInstalled)
            {
                CConfig.DataUploadBotVersion = Helper.GetApplicationVersion(_Const.Data_Upload_Bot_INSTALLED_CONFIG_PATH_x64);
                Decimal.TryParse(Helper.cleanVersion(CConfig.DataUploadBotVersion), out service_version);
                VersionInfoList.Add(
              new VersionInformation
              {
                  App = _Const.Data_Upload_Bot_Name,
                  IsInstalled = CConfig.IsDataUploadBoatInstalled,
                  Version = service_version
              });
            }
            if (CConfig.IsMeetingCreatorBotInstalled)
            {
                CConfig.MeetingCreatorBotVersion = Helper.GetApplicationVersion(_Const.Meeting_Creator_Bot_INSTALLED_CONFIG_PATH_x64);
                Decimal.TryParse(Helper.cleanVersion(CConfig.MeetingCreatorBotVersion), out ad_version);
                VersionInfoList.Add(
              new VersionInformation
              {
                  App = _Const.Meeting_Creator_Bot_Name,
                  IsInstalled = CConfig.IsMeetingCreatorBotInstalled,
                  Version = ad_version
              });
            }
            if (CConfig.IsVPKioskInstalled)
            {
                CConfig.VPKioskVersion = Helper.GetApplicationVersion(_Const.VisitorPoint_Kiosk_INSTALLED_CONFIG_PATH_x64);
                Decimal.TryParse(Helper.cleanVersion(CConfig.VPKioskVersion), out dxMonitoring_version);
                VersionInfoList.Add(
              new VersionInformation
              {
                  App = _Const.VisitorPoint_Kiosk_Name,
                  IsInstalled = CConfig.IsVPKioskInstalled,
                  Version = dxMonitoring_version
              });
            }
            //if (CConfig.isOfflineTaskServiceInstalled)
            //{
            //    CConfig.OfflineTask_Service_version = Helper.GetApplicationVersion(_Const.OFFLINE_TASK_SERVICE_INSTALLED_CONFIG_PATH_X64);
            //    Decimal.TryParse(Helper.cleanVersion(CConfig.OfflineTask_Service_version), out offlinetask_version);
            //    VersionInfoList.Add(
            //  new VersionInformation
            //  {
            //      App = _Const.OfflineTask_Service_Name,
            //      IsInstalled = CConfig.isOfflineTaskServiceInstalled,
            //      Version = offlinetask_version
            //  });
            //}
            //if (CConfig.isLPNServiceInstalled)
            //{
            //    CConfig.LPN_Service_version = Helper.GetApplicationVersion(_Const.LPN_INSTALLED_CONFIG_PATH_X64);
            //    Decimal.TryParse(Helper.cleanVersion(CConfig.LPN_Service_version), out lpn_version);
            //    VersionInfoList.Add(
            //  new VersionInformation
            //  {
            //      App = _Const.LPN_Service_Name,
            //      IsInstalled = CConfig.isLPNServiceInstalled,
            //      Version = lpn_version
            //  });

            //}
            #endregion


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
                    this.LogLabel.Text = _Const.Extract_Updates;
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
                                labelText = _Const.Extract_Finish;
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
                            labelText = _Const.Extract_Finish;
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