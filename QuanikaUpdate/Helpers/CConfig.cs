using QuanikaUpdate.Models;

namespace VPSetup.Helpers
{
    public static class CConfig
    {

        public static Setting Setting = new Setting();
        public static string encKey = "lkaju-x3h4-f5f45";

        public static string _version = "Version 1.0";

        public static string host = "LDvoWOcxJJ8Y+PulGP0urg==";
        public static string ftp_username = "cm94Na8eytwsO+hY5zEknxj4+6UY/S6u";
        public static string ftp_password = "DNzxlm1G2cmqCPHIZWvMXA==";


        public static string version = "";

        public static string startingVersion = "";

        public static double PbPercentage = 0;

        public static bool error = false;

        public static bool RunSQl = true;

        //DB settings
        public static string _db_server = "", _database_name = "QuanikaDB", _db_username = "", _db_pwd = "";
        public static string databaseBackupPath = "";
        //DotNet Framework Versions
        public static bool dotnet35 = false;
        public static bool dotnet45 = false;

        public static bool isApplicationInstalled = false;
        public static bool isDXInstalled = false;
        public static bool isServiceInstalled = false;
        public static bool isADServiceInstalled = false;
        public static bool isDXMONITORINGServiceInstalled = false;
        public static bool isLPNServiceInstalled = false;
        public static bool isOfflineTaskServiceInstalled = false;
        public static string Application_version = "";
        public static string Dx_version = "";
        public static string Service_version = "";
        public static string ADService_version = "";
        public static string DXMONITORING_Service_version = "";
        public static string LPN_Service_version = "";
        public static string OfflineTask_Service_version = "";
        public static string Hostname = System.Net.Dns.GetHostName();

        public static bool IsClientApplicationInstalled = false;
        public static bool IsComServiceInstalled = false;
        public static bool IsDataUploadBoatInstalled = false;
        public static bool IsMeetingCreatorBotInstalled = false;
        public static bool IsKioskInstalled = false;
        public static bool IsOutlookInstalled = false;
        public static bool IsWebRegInstalled = false;
        public static bool IsWebInstalled = false;
        public static bool IsQrWebInstalled = false;

        public static bool IsVisitorPointSettingsInstalled = false;

        //public static bool isLPNServiceInstalled = false;
        //public static bool isOfflineTaskServiceInstalled = false;
        public static string ClientApplicationVersion = "";
        public static string ComServiceVersion = "";
        public static string DataUploadBotVersion = "";
        public static string MeetingCreatorBotVersion = "";
        public static string VPKioskVersion = "";
        public static string VPOutlookVersion = "";
        public static string VPWebRegVersion = "";
        public static string VPWebVersion = "";
        public static string VPQrWeb = "";
        public static string VPSettingsVersion = "";
        internal class VpPatchFolders
        {
            public const string
                ClientApplication = "client-app",
                ComService = "com-service",
                DataUploadBot = "bot-data",
                MeetingCreatorBot = "bot-meeting",
                VisitorPointKiosk = "kiosk",
                VisitorPointSettings = "vp-setting",
                Outlook = "outlook",
                WebReg = "web-reg",
                Web = "web",
                QrWeb = "qr-web";

        }

        internal class VpPatchBackupFolders
        {
            public const string
                ClientApplication = "client-app-backup",
                ComService = "com-service-backup",
                DataUploadBot = "bot-data-backup",
                MeetingCreatorBot = "bot-meeting-backup",
                VisitorPointKiosk = "kiosk-backup",
                VisitorPointSettings = "vp-setting-backup",
                Outlook = "outlook-backup",
                WebReg = "web-reg-backup",
                Web = "web-backup",
            QrWeb = "qr-web-backup";
        }
        internal class VpXmlTags
        {
            public const string
                ClientApplication = "$client-app",
                ComService = "$com-service",
                DataUploadBot = "$bot-data",
                MeetingCreatorBot = "$bot-meeting",
                VisitorPointKiosk = "$kiosk",

                VisitorPointSettings = "$vp-setting",
                Outlook = "$outlook",
                WebReg = "$web-reg",
                Web = "$vp-web",
                QrWeb = "$qr-web";
        }
        internal class QuanikaPatchFolders
        {
            public const string DataBot = "",
                ClientApplication = "VisitorPointDesktopApp",
                ComService = "EQServiceSetup",
                DataUploadBot = "BotMonitoring",
                MeetingCreatorBot = "BotMonitoring",
                VisitorPointKiosk = "VPKiosk",
            VisitorPointSetting = "VisitorPointSettings";

        }
    }
}
