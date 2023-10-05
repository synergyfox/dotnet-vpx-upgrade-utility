using System.IO;
using System.Reflection;

namespace VPSetup.Helpers
{
    public static class ApplicationConstants
    {

        public static string Client_Application_Name = "Client-Application";
        public static string Com_Service_Name = "VisitorPoint-COMMServiceSetup";
        public static string Data_Upload_Bot_Name = "VisitorPoint-DataUploadBot-Application";
        public static string Meeting_Creator_Bot_Name = "VisitorPoint-MeetingCreatorBot";
        public static string VisitorPoint_Kiosk_Name = "VisitorPoint-Kiosk";
        public static string VisitorPoint_Oulook_Name = "Outlook";
        public static string VisitorPoint_Web_Name = "Vp Web";
        public static string VisitorPoint_Web_Reg_Name = "Reg Link";
        public static string VisitorPoint_Qr_Web_Name = "Qr Web";

        //for service name
        
        public static string Com_Service_Service_Name = "VisitorPoint-COMM Service Setup Service";
        public static string Data_Upload_Bot_Service_Name = "VisitorPoint DataUploadBot Service";
        public static string Meeting_Creator_Bot_Service_Name = "VisitorPoint-MeetingCreatorBot Service";
     

        public static bool IncludeOutlook = false;



        public static string Client_Application_PROCESS_NAME = @"VisitorPointDesktopApp";

        public static string Com_Service_Process_Name = @"EQServiceSetup";

        public static string Com_Service_PROCESS_NAME = @"EQService";
        public static string Data_Upload_Bot_PROCESS_NAME = @"BotMonitoring";
        public static string Data_Upload_Bot_Service_PROCESS_NAME = @"DataUploadBotService";
        public static string Meeting_Creator_Bot_PROCESS_NAME = @"BotMonitoring";
        public static string Meeting_Creator_Bot_Service_PROCESS_NAME = @"BotService";
        public static string Kiosk_PROCESS_NAME = @"EQService"; //EQServiceSetup
        public static string Oulook_PROCESS_NAME = "Outlook";
        public static string Web_PROCESS_NAME = "Vp Web";
        public static string Web_Reg_PROCESS_NAME = "Reg Link";
        public static string Qr_Web_PROCESS_NAME = "Qr Web";

        public static string Client_Application_INSTALLED_PATH_x64 = @"C:\Program Files (x86)\VisitorPoint\Client Application";
        public static string Client_Application_INSTALLED_PATH_X86 = @"C:\Program Files\VisitorPoint\Client Application";

        public static string Com_Service_INSTALLED_PATH_x64 = @"C:\Program Files (x86)\VisitorPoint\COMService";
        public static string Com_Service_INSTALLED_PATH_X86 = @"C:\Program Files\VisitorPoint\COMService";

        public static string Data_Upload_Bot_INSTALLED_PATH_x64 = @"C:\Program Files (x86)\VisitorPoint\DataUploadBot";
        public static string Data_Upload_Bot_INSTALLED_PATH_X86 = @"C:\Program Files\VisitorPoint\DataUploadBot";

        public static string Meeting_Creator_Bot_INSTALLED_PATH_x64 = @"C:\Program Files (x86)\VisitorPoint\MeetingCreatorBot";
        public static string Meeting_Creator_Bot_INSTALLED_PATH_X86 = @"C:\Program Files\VisitorPoint\MeetingCreatorBot";

        public static string VisitorPoint_Kiosk_INSTALLED_PATH_x64 = @"C:\Program Files (x86)\VisitorPoint\VisitorPoint-Kiosk";
        public static string VisitorPoint_Kiosk_INSTALLED_PATH_X86 = @"C:\Program Files\VisitorPoint\VisitorPoint-Kiosk";



        public static string Client_Application_INSTALLED_CONFIG_PATH_x64 = @"C:\Program Files (x86)\VisitorPoint\Client Application\VisitorPointDesktopApp.exe";
        public static string Client_Application_INSTALLED_CONFIG_PATH_X86 = @"C:\Program Files\VisitorPoint\Client Application\VisitorPointDesktopApp.exe";

        //public static string Com_Service_INSTALLED_CONFIG_PATH_x64 = @"C:\Program Files (x86)\VisitorPoint\COMService\EQServiceSetup.exe";
        //public static string Com_Service_INSTALLED_CONFIG_PATH_X86 = @"C:\Program Files\VisitorPoint\COMService\EQServiceSetup.exe";
        public static string Com_Service_INSTALLED_CONFIG_PATH_x64 = @"C:\Program Files (x86)\VisitorPoint\COMService\EQService.exe";
        public static string Com_Service_INSTALLED_CONFIG_PATH_X86 = @"C:\Program Files\VisitorPoint\COMService\EQService.exe";
        //public static string Data_Upload_Bot_INSTALLED_CONFIG_PATH_x64 = @"C:\Program Files (x86)\VisitorPoint\DataUploadBot\BotMonitoring.exe";
        //public static string Data_Upload_Bot_INSTALLED_CONFIG_PATH_X86 = @"C:\Program Files\VisitorPoint\DataUploadBot\BotMonitoring.exe";

        //public static string Meeting_Creator_Bot_INSTALLED_CONFIG_PATH_x64 = @"C:\Program Files (x86)\VisitorPoint\MeetingCreatorBot\BotMonitoring.exe";
        //public static string Meeting_Creator_Bot_INSTALLED_CONFIG_PATH_X86 = @"C:\Program Files\VisitorPoint\MeetingCreatorBot\BotMonitoring.exe";

        public static string Data_Upload_Bot_INSTALLED_CONFIG_PATH_x64 = @"C:\Program Files (x86)\VisitorPoint\DataUploadBot\DataUploadBotService.exe";
        public static string Data_Upload_Bot_INSTALLED_CONFIG_PATH_X86 = @"C:\Program Files\VisitorPoint\DataUploadBot\DataUploadBotService.exe";

        public static string Meeting_Creator_Bot_INSTALLED_CONFIG_PATH_x64 = @"C:\Program Files (x86)\VisitorPoint\MeetingCreatorBot\BotService.exe";
        public static string Meeting_Creator_Bot_INSTALLED_CONFIG_PATH_X86 = @"C:\Program Files\VisitorPoint\MeetingCreatorBot\BotService.exe";

        public static string VisitorPoint_Kiosk_INSTALLED_CONFIG_PATH_x64 = @"C:\Program Files (x86)\VisitorPoint\VisitorPoint-Kiosk\VPKiosk.exe";
        public static string VisitorPoint_Kiosk_INSTALLED_CONFIG_PATH_X86 = @"C:\Program Files\VisitorPoint\VisitorPoint-Kiosk\VPKiosk.exe";












        #region Application Names
        public static string Application_Name = "Quanika-Application";
        public static string Dx_Name = "Quanika-DX";
        public static string Service_Name = "Quanika-Service";
        public static string ADService_Name = "Quanika-ActiveDirectory-Service";
        public static string DXMonitoring_Service_Name = "DX Monitoring Service";
        public static string OfflineTask_Service_Name = "Quanika Offline Task Service";
        public static string LPN_Service_Name = "Quanika LPN Service";
        public static string OfflineTask_App_Name = "OfflineTaskService";
        public static string LPN_App_Name = "LPNService";

        #endregion

        #region Application Process Names
        public static string Application_Process_Name = "Quanika Application";
        public static string Dx_Process_Name = "DataExchange";
        public static string Service_Process_Name = "QuanikaService";
        public static string ADService_Process_Name = "ActiveDirectoryBackgroundService";
        public static string DXMonitoring_Process_Name = "DXMonitoringService";
        #endregion

        #region Command Types
        public static string UpdateCommand = "update", ConfigCommand = "configure";

        #endregion
        public static string executableLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        public static string DOT_NET_45_X64 = @"prerequisites\.netframework4.6.exe";

        public static string SQL_X86_EXE = @"sql\SQL2008-R2\SQLEXPR_x86_ENU.exe";
        public static string SQL_X64_EXE = @"sql\SQL2016-SP1\Install.cmd";

        public static string OPEN_FIREWALL_X86_008 = @"sql\SQL2008-R2\Open firewall - x86.cmd";
        public static string OPEN_FIREWALL_X64_008 = @"sql\SQL2008-R2\Open firewall - x64.cmd";

        public static string OPEN_FIREWALL_X64_16 = @"sql\SQL2016-SP1\Open firewall - x64.cmd";

        public static string SQL_2008_X86 = string.Format("{0}{1}", executableLocation, @"\sql\SQL2008-R2\SQLEXPR_x86_ENU.exe");
        public static string SQL_2016_x64 = string.Format("{0}{1}", executableLocation, @"\sql\SQL2016-SP1\SQLEXPR_x64_ENU.exe");
        public static string SQL_X64_INSTALL_COMMAND = @"/QS /ACTION=Install /IACCEPTSQLSERVERLICENSETERMS=1 
/IACCEPTROPENLICENSETERMS=1 /ENU=1 /UpdateEnabled=1 
/UpdateSource=""MU"" 
/FEATURES=SQL,Tools 
/INSTANCENAME=SynergyAxis 
/AGTSVCACCOUNT=""NT AUTHORITY\Network Service"" 
/AGTSVCSTARTUPTYPE=Automatic 
/BROWSERSVCSTARTUPTYPE=Automatic 
/SAPWD={0} 
/SECURITYMODE=SQL 
/ADDCURRENTUSERASSQLADMIN=True 
/SQLSVCACCOUNT=""NT AUTHORITY\Network Service"" 
/SQLSVCSTARTUPTYPE=Automatic 
/TCPENABLED=1";
        public static string SQL_X86_INSTALL_COMMAND = @"/QS /ACTION=Install /FEATURES=SQL,Tools /INSTANCENAME=SynergyAxis /IACCEPTSQLSERVERLICENSETERMS /SQLSVCACCOUNT=""NT AUTHORITY\Network Service"" /SQLSVCSTARTUPTYPE=Automatic /AGTSVCACCOUNT=""NT AUTHORITY\Network Service"" /AGTSVCSTARTUPTYPE=Automatic /BROWSERSVCSTARTUPTYPE=Automatic /SECURITYMODE=SQL /SAPWD={0} /ADDCURRENTUSERASSQLADMIN=True /TCPENABLED=1";


        #region InstalledApplicationPath

        public static string DX_INSTALLED_CONFIG_PATH_x64 = @"C:\Program Files (x86)\Quanika\Quanika-DX\DataExchange.exe";
        public static string DX_INSTALLED_CONFIG_PATH_X86 = @"C:\Program Files\Quanika\Quanika-DX\DataExchange.exe";

        public static string APP_INSTALLED_EXE_PATH_X64 = @"C:\Program Files (x86)\Quanika\Quanika-Application\Quanika Application.exe";
        public static string APP_INSTALLED_EXE_PATH_X86 = @"C:\Program Files\Quanika\Quanika-Application\Quanika Application.exe";

        public static string Service_INSTALLED_CONFIG_PATH_X64 = @"C:\Program Files (x86)\Quanika\Quanika-Service\QuanikaService.exe";
        public static string Service_INSTALLED_CONFIG_PATH_X86 = @"C:\Program Files\Quanika\Quanika-Service\QuanikaService.exe";

        public static string ADService_INSTALLED_CONFIG_PATH_X64 = @"C:\Program Files (x86)\Quanika\QuanikaADService\ActiveDirectoryBackgroundService.exe";
        public static string ADService_INSTALLED_CONFIG_PATH_X86 = @"C:\Program Files\Quanika\QuanikaADService\ActiveDirectoryBackgroundService.exe";

        public static string DXMonitoring_INSTALLED_CONFIG_PATH_X64 = @"C:\Program Files (x86)\Quanika\DX Monitoring Service\DXMonitoringService.exe";
        public static string DXMonitoring_INSTALLED_CONFIG_PATH_X86 = @"C:\Program Files\Quanika\DX Monitoring Service\DXMonitoringService.exe";

        public static string LPN_INSTALLED_CONFIG_PATH_X64 = @"C:\Program Files (x86)\Quanika\Quanika LPN Service\LPNService.exe";
        public static string LPN_INSTALLED_CONFIG_PATH_X86 = @"C:\Program Files\Quanika\Quanika LPN Service\LPNService.exe";

        public static string OFFLINE_TASK_SERVICE_INSTALLED_CONFIG_PATH_X64 = @"C:\Program Files (x86)\Quanika\Quanika Offline Task Service\OfflineTaskService.exe";
        public static string OFFLINE_TASK_SERVICE_CONFIG_PATH_X86 = @"C:\Program Files\Quanika\Quanika Offline Task Service\OfflineTaskService.exe";


        public static string Client_Installer_CONFIG_PATH = @"ClientInstaller\ApplicationSetup.exe";

        public static string Web_UI_INSTALLED_PATH = @"C:\www\QuanikaEnterprise\Web";
        public static string Web_API_INSTALLED_PATH = @"C:\www\QuanikaEnterprise\API";

        public static string DX_INSTALLED_PATH_x64 = @"C:\Program Files (x86)\Quanika\Quanika-DX";
        public static string DX_INSTALLED_PATH_X86 = @"C:\Program Files\Quanika\Quanika-DX";

        public static string APP_INSTALLED_PATH_X64 = @"C:\Program Files (x86)\Quanika\Quanika-Application";
        public static string APP_INSTALLED_PATH_X86 = @"C:\Program Files\Quanika\Quanika-Application";

        public static string Service_INSTALLED_PATH_X64 = @"C:\Program Files (x86)\Quanika\Quanika-Service";
        public static string Service_INSTALLED_PATH_X86 = @"C:\Program Files\Quanika\Quanika-Service";

        public static string ADService_INSTALLED_PATH_X64 = @"C:\Program Files (x86)\Quanika\QuanikaADService";
        public static string ADService_INSTALLED_PATH_X86 = @"C:\Program Files\Quanika\QuanikaADService";

        public static string DXMonitoring_Service_INSTALLED_PATH_X64 = @"C:\Program Files (x86)\Quanika\DX Monitoring Service";
        public static string DXMonitoring_Service_INSTALLED_PATH_X86 = @"C:\Program Files\Quanika\DX Monitoring Service";

        public static string LPN_INSTALLED_INSTALL_PATH_X64 = @"C:\Program Files (x86)\Quanika\Quanika LPN Service";
        public static string LPN_INSTALLED_INSTALL_PATH_X86 = @"C:\Program Files\Quanika\Quanika LPN Service";

        public static string OFFLINE_TASK_INSTALL_INSTALLED_INSTALL_PATH_X64 = @"C:\Program Files (x86)\Quanika\Quanika Offline Task Service";
        public static string OFFLINE_TASK_SERVICE_INSTALLED_INSTALL_PATH_X86 = @"C:\Program Files\Quanika\Quanika Offline Task Service";



        public static string updates_dll_Folder_Name = "dlls";
        public static string backup_dll_Folder_Name = "DllBackups";
        public static string updates_exe_Folder_Name = "exe";
        public static string backup_exe_Folder_Name = "ExeBackups";
        public static string updates_files_Folder_Name = "anyfile";
        public static string Dll_Extension = ".dll";
        public static string Exe_Extension = ".exe";
        #endregion

        #region Apps Path
        public static string GET_APPLICATION_PATH = @"applications\quanika_application.exe";

        public static string GET_CLR_PATH = @"ClientInstaller\prerequisites\SQLSysClrTypes.msi";
        public static string GET_REPORT_VIEWER_PATH = @"ClientInstaller\prerequisites\ReportViewer.msi";

        public static string GET_DX_PATH = @"applications\quanika_dx.exe";
        public static string GET_SERVICE_PATH = @"applications\quanika_service.exe";
        public static string GET_DXMONITORING_SERVICE_PATH = @"applications\quanika_monitoring_service.exe";

        #endregion


        #region App Messages
        public static string QCS_Not_Installed = "Quanika Appllication  not installed";
        public static string App_Running = "Quanika Application  is running. Please stop first and then retry again.";
        public static string DX_Running = "Quanika DX  is running. Please stop first and then retry again.";
        public static string Service_Running = "Quanika Service  is running. Please stop first and then retry again.";
        public static string AdService_Running = "Quanika ActiveDirtectory Service  is running. Please stop first and then retry again.";

        public static string Verify_Version = "Verifying Application Version";
        public static string Checking_Updates = "Checking for updates";
        public static string Con_Error_Ftp = "Unable to connect to ftp server";
        public static string Updates_Found = "Updates found. Do you want to update?";
        public static string Download_Update = "Updates found. Do you want to update?";
        public static string Insert_db_Error = "Unable to insert logs in db..Click ok to close";
        public static string Success_Download_Updates = "Successfully download Updates from ftp server.";
        public static string No_Updates = "No Updates Available";
        public static string Error_Download_Updates = "Unable to download updates from ftp server.";
        public static string Checking_LocalStorage = "Checking for updates in localstorage";
        public static string Find_Version_Error = "Unable to find version in database";
        public static string localStorage_Error = "Unable to get updates from local storage. Click Ok to close";
        public static string Get_DB_logs = "Getting update logs from database";
        public static string Get_Pending_logs = "Getting pending logs from database";
        public static string Db_Backup_Error = "Database has not been backup on the specified path";
        public static string Execute_SQl_Logs = "Executing Sql Log";
        public static string Db_Restore_Error = "Unable to execute sql scripts. Database can not restored because it is being used with any other session.";
        public static string Db_Restored = "Unable to execute sql scripts. Database restored successfully. Press Ok to close";
        public static string Execute_SQl_Logs_Error = "Unable to execute sql scripts. Press Ok to close";
        public static string Execute_DLL_Logs = "Executing dll Logs";
        public static string Execute_DLL_Logs_Error = "Unable to execute dlls. Click ok to close";
        public static string Execute_Exe_Logs = "Executing exe Logs";
        public static string Execute_Exe_Logs_Error = "Unable to execute exe. Click ok to close";
        public static string Update_Version_Error = "Unable to update version in database. Click ok to close.";
        public static string Updatedb_Version_Error = "Unable to update database version in database. Click ok to close.";
        public static string Get_DB_logs_Error = "Unable to finds logs to execute in db";
        public static string Downloading_Update = "Downloading updates from ftp server";
        public static string Remove_Logs_Error = "Unable to delete logs from database. Click ok to close";
        public static string Dismiss_Updates = "Are you sure you want to dismiss updates?";
        public static string installed_versions_error = "Unable to find installed version in database. Click ok to close";
        public static string db_versions_error = "Unable to find database installed version in database. Click ok to close";
        public static string update_logs_error = "Unable to update logs in database. Click ok to close";
        public static string Replacing_dlls = "Getting previous dlls";
        public static string Backup_dll = "Unable to get backup dlls. Click ok to close";
        public static string Backup_exe = "Unable to get backup exes. Click ok to close";
        public static string Revert_Version_Error = "Unable to revert version in database. Click ok to close.";
        public static string Revert_Permission = "Do you want to revert?.";
        public static string Db_backup = "Please make sure you have backup your database before continuing. Are you sure you want to continue?";
        public static string No_Logs = "No logs found in db to execute.";
        public static string Restore_Db = "Please restore you db to the last version";
        public static string Extract_Updates = "Extracting latest updates";
        public static string Extract_Finish = "Files extraction completed";
        #endregion

        public static string execute_Update_Logs = "Execute Update Logs";
        public static string execute_Pending_Logs = "Execute Pending Logs";

    }
}
