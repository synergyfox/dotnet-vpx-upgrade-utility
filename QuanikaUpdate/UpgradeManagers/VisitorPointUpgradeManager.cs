using QuanikaUpdate.Constants;
using QuanikaUpdate.Helpers;
using QuanikaUpdate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static VPSetup.Helpers.CConfig;

namespace QuanikaUpdate.UpgradeManagers
{
    internal class VisitorPointUpgradeManager : UpgradeManager
    {
        private IEnumerable<FileDetails> _subDirecotories = Enumerable.Empty<FileDetails>();
        protected async override Task ManageSinglePatch(string patchDirectory)
        {
            _subDirecotories = GetDirectoryDetails(patchDirectory);
            await CopyVpFiles();
        }

        private async Task CopyVpFiles()
        {
            CopyFiles(VpPatchFolders.ClientApplication, VisitorPointDestinations.ClientApplication);
            CopyFiles(VpPatchFolders.ComService, VisitorPointDestinations.ComService);
            CopyFiles(VpPatchFolders.DataUploadBot, VisitorPointDestinations.DataUploadBot);
            CopyFiles(VpPatchFolders.MeetingCreatorBot, VisitorPointDestinations.MeetingCreatorBot);
            CopyFiles(VpPatchFolders.WebReg, VisitorPointDestinations.WebReg);
            CopyFiles(VpPatchFolders.Web, VisitorPointDestinations.Web);
            CopyFiles(VpPatchFolders.Outlook, VisitorPointDestinations.Outlook);
            CopyFiles(VpPatchFolders.VisitorPointKiosk, VisitorPointDestinations.Kiosk);

            if (_subDirecotories.FirstOrDefault(x => x.Name.Equals("Sql", StringComparison.OrdinalIgnoreCase)) is FileDetails Sqldir)
            {
                await ExecuteSqlFiles(Sqldir);
            }
        }

        private void CopyFiles(string folderName, VisitorPointDestinations visitorPointDestinations)
        {

            if (_subDirecotories.FirstOrDefault(x => x.Name == folderName) is FileDetails dir)
            {
                var path = PathsHelper.GetVisitorPointPaths(visitorPointDestinations);
                if (string.IsNullOrEmpty(path))
                {
                    MainWindow.UpdatePbLabel("Executing script ");
                    return;
                }

                CopyDirectory(dir.Path, path);
            }
            #region Old Commented Code
            //var files = Directory.GetFiles(dir.Path);
            //foreach (var file in files)
            //{
            //    var destPath = PathsHelper.GetVisitorPointPaths(visitorPointDestinations);
            //    var fileName = Path.GetFileName(file);
            //    var finalDestPath = Path.Combine(destPath, fileName);
            //    File.Copy(file, finalDestPath, true);
            //}
            #endregion
        }


    }
}
