using Microsoft.Web.Administration;
using QuanikaUpdate.Constants;
using System;
using System.Linq;
using VPSetup.Extensions;
using VPSetup.Helpers;
using static VPSetup.Helpers.CConfig;

namespace QuanikaUpdate.Helpers
{
    internal class PathsHelper
    {
        internal static string GetVisitorPointPaths(VisitorPointDestination vp)
        {
            var is64bitOS = OSHelper.Is64BitOperatingSystem();
            switch (vp)
            {
                case VisitorPointDestination.ClientApplication:
                    if (is64bitOS)
                    {
                        return ApplicationConstants.Client_Application_INSTALLED_PATH_x64;
                    }
                    return ApplicationConstants.Client_Application_INSTALLED_PATH_X86;
                case VisitorPointDestination.ComService:
                    if (is64bitOS)
                    {
                        return ApplicationConstants.Com_Service_INSTALLED_PATH_x64;
                    }
                    return ApplicationConstants.Com_Service_INSTALLED_PATH_X86;
                case VisitorPointDestination.DataUploadBot:
                    if (is64bitOS)
                    {
                        return ApplicationConstants.Data_Upload_Bot_INSTALLED_PATH_x64;
                    }
                    return ApplicationConstants.Data_Upload_Bot_INSTALLED_PATH_X86;
                case VisitorPointDestination.MeetingCreatorBot:
                    if (is64bitOS)
                    {
                        return ApplicationConstants.Meeting_Creator_Bot_INSTALLED_PATH_x64;
                    }
                    return ApplicationConstants.Meeting_Creator_Bot_INSTALLED_PATH_X86;
                case VisitorPointDestination.WebReg:
                    return GetWebPath("Reg Link");
                case VisitorPointDestination.Kiosk:
                    if (is64bitOS)
                    {
                        return ApplicationConstants.VisitorPoint_Kiosk_INSTALLED_PATH_x64;
                    }
                    return ApplicationConstants.VisitorPoint_Kiosk_INSTALLED_PATH_X86;
                case VisitorPointDestination.Outlook:
                    if (is64bitOS)
                    {
                        return "";
                    }
                    return "";
                case VisitorPointDestination.Web:
                    return GetWebPath("Vp Web");
                case VisitorPointDestination.QrWeb:
                    return GetWebPath("QR Web");
                default:
                    return string.Empty;
            }

        }
        internal static string GetVisitorPointInstallsedConfigPaths(VisitorPointDestination vp)
        {
            var is64bitOS = OSHelper.Is64BitOperatingSystem();
            switch (vp)
            {
                case VisitorPointDestination.ClientApplication:
                    if (is64bitOS)
                    {
                        return ApplicationConstants.Client_Application_INSTALLED_CONFIG_PATH_x64;
                    }
                    return ApplicationConstants.Client_Application_INSTALLED_CONFIG_PATH_X86;
                case VisitorPointDestination.ComService:
                    if (is64bitOS)
                    {
                        return ApplicationConstants.Com_Service_INSTALLED_CONFIG_PATH_x64;
                    }
                    return ApplicationConstants.Com_Service_INSTALLED_CONFIG_PATH_X86;
                case VisitorPointDestination.DataUploadBot:
                    if (is64bitOS)
                    {
                        return ApplicationConstants.Data_Upload_Bot_INSTALLED_CONFIG_PATH_x64;
                    }
                    return ApplicationConstants.Data_Upload_Bot_INSTALLED_CONFIG_PATH_X86;
                case VisitorPointDestination.MeetingCreatorBot:
                    if (is64bitOS)
                    {
                        return ApplicationConstants.Meeting_Creator_Bot_INSTALLED_CONFIG_PATH_x64;
                    }
                    return ApplicationConstants.Meeting_Creator_Bot_INSTALLED_CONFIG_PATH_X86;
                case VisitorPointDestination.WebReg:
                    return GetWebPath("Reg Link");
                case VisitorPointDestination.Kiosk:
                    if (is64bitOS)
                    {
                        return ApplicationConstants.VisitorPoint_Kiosk_INSTALLED_CONFIG_PATH_x64;
                    }
                    return ApplicationConstants.VisitorPoint_Kiosk_INSTALLED_CONFIG_PATH_X86;
                case VisitorPointDestination.Outlook:
                    if (is64bitOS)
                    {
                        return "";
                    }
                    return "";
                case VisitorPointDestination.Web:
                    return GetWebPath("Vp Web");
                case VisitorPointDestination.QrWeb:
                    return GetWebPath("Qr Web");
                default:
                    return string.Empty;
            }

        }
        internal static string GetVPPatchFolderName(VisitorPointDestination vp)
        {
            switch (vp)
            {
                case VisitorPointDestination.ClientApplication:
                    return VpPatchFolders.ClientApplication;
                case VisitorPointDestination.ComService:
                    return VpPatchFolders.ClientApplication;
                case VisitorPointDestination.DataUploadBot:
                    return VpPatchFolders.ClientApplication;
                case VisitorPointDestination.MeetingCreatorBot:
                    return VpPatchFolders.MeetingCreatorBot;
                case VisitorPointDestination.WebReg:
                    return VpPatchFolders.WebReg;
                case VisitorPointDestination.Kiosk:
                    return VpPatchFolders.VisitorPointKiosk;
                case VisitorPointDestination.Outlook:
                    return VpPatchFolders.Outlook;
                case VisitorPointDestination.Web:
                    return VpPatchFolders.Web;
                case VisitorPointDestination.QrWeb:
                    return VpPatchFolders.QrWeb;
                default:
                    return string.Empty;
            }
        }
        internal static string GetQuanikaPaths(QuanikaDestinations vp)
        {
            var is64bitOS = OSHelper.Is64BitOperatingSystem();
            switch (vp)
            {
                case QuanikaDestinations.Application:
                    if (is64bitOS)
                    {

                    }
                    return "";
                case QuanikaDestinations.DX:
                    if (is64bitOS)
                    {

                    }
                    return "";
                case QuanikaDestinations.OfflineService:
                    if (is64bitOS)
                    {

                    }
                    return "";
                case QuanikaDestinations.QuanikaService:
                    if (is64bitOS)
                    {

                    }
                    return "";
                case QuanikaDestinations.LPN:
                    if (is64bitOS)
                    {

                    }
                    return "";
                default:
                    return "";
            }
        }

        internal static string GetWebPath(string webName)
        {
            ServerManager serverManager = new ServerManager();
            string sitePath = serverManager.Sites.FirstOrDefault(x => x.Name.Equals(webName, StringComparison.OrdinalIgnoreCase)).Applications["/"].VirtualDirectories["/"].PhysicalPath;
            return sitePath;
        }

        internal static (string, Site) GetWebInfoAndPath(string webName)
        {
            ServerManager serverManager = new ServerManager();

            var site = serverManager.Sites.FirstOrDefault(x => x.Name.Equals(webName, StringComparison.OrdinalIgnoreCase));
            string sitePath = site.Applications["/"].VirtualDirectories["/"].PhysicalPath;
            return (sitePath, site);
        }
        internal static Site GetWebInfo(string webName)
        {
            ServerManager serverManager = new ServerManager();
            return serverManager.Sites.FirstOrDefault(x => x.Name.Equals(webName, StringComparison.OrdinalIgnoreCase));
        }
        internal bool IsWebRunning(string webName)
        {
            var siteInfo = GetWebInfo(webName);
            if (siteInfo is null)
            {
                return false;
            }
            if (siteInfo.State is ObjectState.Started || siteInfo.State is ObjectState.Starting)
            {
                return true;
            }
            return false;
        }
        internal bool IsWebInstalled(string webName)
        {
            var siteInfo = GetWebInfo(webName);
            if (siteInfo is null)
            {
                return false;
            }
            return true;
        }

        internal static bool IsVpWeb(VisitorPointDestination @enum)
        {
            switch (@enum)
            {
                case VisitorPointDestination.Web:
                    return true;
                case VisitorPointDestination.WebReg:
                    return true;
                case VisitorPointDestination.QrWeb:
                    return true;
                default:
                    return false;
            }
        }
    }
}
