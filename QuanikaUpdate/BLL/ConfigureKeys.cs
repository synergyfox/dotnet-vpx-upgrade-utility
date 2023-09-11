using System;
using VPSetup.Helpers;

namespace QuanikaUpdate.BLL
{
    static class ConfigureKeys
    {
        public static void  ProcessConfigure(string key , string value , string module )
        {
            try
            {
                value = value.Replace('@', ' ');
            switch (module)
            {
                case "lpn":
                    if (CConfig.isLPNServiceInstalled) {
                        Helper.addKeyInConfig(key, value, _Const.LPN_INSTALLED_CONFIG_PATH_X64);
                    }
                        break;
                case "service":
                        if (CConfig.isServiceInstalled)
                        {
                            Helper.addKeyInConfig(key, value, _Const.Service_INSTALLED_CONFIG_PATH_X64);
                        }
                        break;
                case "app":
                        if (CConfig.isApplicationInstalled)
                        {
                            Helper.addKeyInConfig(key, value, _Const.APP_INSTALLED_EXE_PATH_X64);
                        }
                        break;

                    case "dx":
                        if (CConfig.isDXInstalled)
                        {
                            Helper.addKeyInConfig(key, value, _Const.DX_INSTALLED_CONFIG_PATH_x64);
                        }
                        break;
                    case "offline":
                        if (CConfig.isOfflineTaskServiceInstalled)
                        {
                            Helper.addKeyInConfig(key, value, _Const.OFFLINE_TASK_SERVICE_INSTALLED_CONFIG_PATH_X64);
                        }
                        break;
                    case "rd":
                        if (CConfig.isDXMONITORINGServiceInstalled)
                        {
                            Helper.addKeyInConfig(key, value, _Const.DXMonitoring_INSTALLED_CONFIG_PATH_X64);
                        }
                        break;
                    case "ad":
                        if (CConfig.isADServiceInstalled)
                        {
                            Helper.addKeyInConfig(key, value, _Const.ADService_INSTALLED_CONFIG_PATH_X64);
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
