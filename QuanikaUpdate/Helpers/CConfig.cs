using QuanikaUpdate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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


        public static string version = "" ;

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
    }
}
