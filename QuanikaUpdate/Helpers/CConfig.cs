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

        public static bool IsClientApplicationInstalled = false;
        public static bool IsComServiceInstalled = false;
        public static bool IsDataUploadBoatInstalled = false;
        public static bool IsMeetingCreatorBotInstalled = false;
        public static bool IsVPKioskInstalled = false;
        //public static bool isLPNServiceInstalled = false;
        //public static bool isOfflineTaskServiceInstalled = false;
        public static string ClientApplicationVersion = "";
        public static string ComServiceVersion = "";
        public static string DataUploadBotVersion = "";
        public static string MeetingCreatorBotVersion = "";
        public static string VPKioskVersion = "";
        //public static string LPN_Service_version = "";
        //public static string OfflineTask_Service_version = "";
        public static string Hostname = System.Net.Dns.GetHostName();
    }
}
