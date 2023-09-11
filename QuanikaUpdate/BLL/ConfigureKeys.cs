using System;
using VPSetup.Helpers;

namespace QuanikaUpdate.BLL
{
    static class ConfigureKeys
    {
        public static void ProcessConfigure(string key, string value, string module)
        {
            try
            {
                value = value.Replace('@', ' ');
                switch (module)
                {
                    //case "lpn":
                    //    if (CConfig.isLPNServiceInstalled)
                    //    {
                    //        Helper.addKeyInConfig(key, value, _Const.LPN_INSTALLED_CONFIG_PATH_X64);
                    //    }
                    //    break;
                    case "service":
                        if (CConfig.IsDataUploadBoatInstalled)
                        {
                            Helper.addKeyInConfig(key, value, _Const.Data_Upload_Bot_INSTALLED_CONFIG_PATH_x64);
                        }
                        break;
                    case "app":
                        if (CConfig.IsClientApplicationInstalled)
                        {
                            Helper.addKeyInConfig(key, value, _Const.Client_Application_INSTALLED_CONFIG_PATH_x64);
                        }
                        break;

                    case "dx":
                        if (CConfig.IsComServiceInstalled)
                        {
                            Helper.addKeyInConfig(key, value, _Const.Com_Service_INSTALLED_CONFIG_PATH_x64);
                        }
                        break;
                    //case "offline":
                    //    if (CConfig.isOfflineTaskServiceInstalled)
                    //    {
                    //        Helper.addKeyInConfig(key, value, _Const.OFFLINE_TASK_SERVICE_INSTALLED_CONFIG_PATH_X64);
                    //    }
                    //    break;
                    case "rd":
                        if (CConfig.IsVPKioskInstalled)
                        {
                            Helper.addKeyInConfig(key, value, _Const.VisitorPoint_Kiosk_INSTALLED_CONFIG_PATH_x64);
                        }
                        break;
                    case "ad":
                        if (CConfig.IsMeetingCreatorBotInstalled)
                        {
                            Helper.addKeyInConfig(key, value, _Const.Meeting_Creator_Bot_INSTALLED_CONFIG_PATH_x64);
                        }
                        break;
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
