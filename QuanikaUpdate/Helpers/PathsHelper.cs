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
        internal static string GetVisitorPointPaths(VisitorPointDestinations vp)
        {
            var is64bitOS = OSHelper.Is64BitOperatingSystem();
            switch (vp)
            {
                case VisitorPointDestinations.ClientApplication:
                    if (is64bitOS)
                    {
                        return _Const.Client_Application_INSTALLED_PATH_x64;
                    }
                    return _Const.Client_Application_INSTALLED_PATH_X86;
                case VisitorPointDestinations.ComService:
                    if (is64bitOS)
                    {
                        return _Const.Com_Service_INSTALLED_PATH_x64;
                    }
                    return _Const.Com_Service_INSTALLED_PATH_X86;
                case VisitorPointDestinations.DataUploadBot:
                    if (is64bitOS)
                    {
                        return _Const.Data_Upload_Bot_INSTALLED_PATH_x64;
                    }
                    return _Const.Data_Upload_Bot_INSTALLED_PATH_X86;
                case VisitorPointDestinations.MeetingCreatorBot:
                    if (is64bitOS)
                    {
                        return _Const.Meeting_Creator_Bot_INSTALLED_PATH_x64;
                    }
                    return _Const.Meeting_Creator_Bot_INSTALLED_PATH_X86;
                case VisitorPointDestinations.WebReg:
                    return GetWebPath("Vp Web");
                case VisitorPointDestinations.Kiosk:
                    if (is64bitOS)
                    {
                        return _Const.VisitorPoint_Kiosk_INSTALLED_PATH_x64;
                    }
                    return _Const.VisitorPoint_Kiosk_INSTALLED_PATH_X86;
                case VisitorPointDestinations.Outlook:
                    if (is64bitOS)
                    {
                        return "";
                    }
                    return "";
                case VisitorPointDestinations.Web:
                    return GetWebPath("Vp Web");
                default:
                    return string.Empty;
            }

        }
        internal static string GetVPPatchFolderName(VisitorPointDestinations vp)
        {
            switch (vp)
            {
                case VisitorPointDestinations.ClientApplication:
                    return VpPatchFolders.ClientApplication;
                case VisitorPointDestinations.ComService:
                    return VpPatchFolders.ClientApplication;
                case VisitorPointDestinations.DataUploadBot:
                    return VpPatchFolders.ClientApplication;
                case VisitorPointDestinations.MeetingCreatorBot:
                    return VpPatchFolders.MeetingCreatorBot;
                case VisitorPointDestinations.WebReg:
                    return VpPatchFolders.WebReg;
                case VisitorPointDestinations.Kiosk:
                    return VpPatchFolders.VisitorPointKiosk;
                case VisitorPointDestinations.Outlook:
                    return VpPatchFolders.Outlook;
                case VisitorPointDestinations.Web:
                    return VpPatchFolders.Web;
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
    }
}
