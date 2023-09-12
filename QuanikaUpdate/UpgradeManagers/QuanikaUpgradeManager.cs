using QuanikaUpdate.Constants;
using QuanikaUpdate.Helpers;
using QuanikaUpdate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanikaUpdate.UpgradeManagers
{
    internal class QuanikaUpgradeManager : UpgradeManager
    {
        private IEnumerable<FileDetails> _subDirecotories = Enumerable.Empty<FileDetails>();
        protected async override Task ManageSinglePatch(string patchDirectory)
        {
            _subDirecotories = GetDirectoryDetails(patchDirectory);
            await CopyVpFiles(patchDirectory);
        }

        private async Task CopyVpFiles(string patchDirectory)
        {
            //CopyFiles(patchDirectory, VpPatchFolders.ClientApplication, VisitorPointDestinations.ClientApplication);
            //CopyFiles(patchDirectory, VpPatchFolders.ComService, VisitorPointDestinations.ComService);
            //CopyFiles(patchDirectory, VpPatchFolders.DataUploadBot, VisitorPointDestinations.DataUploadBot);
            //CopyFiles(patchDirectory, VpPatchFolders.MeetingCreatorBot, VisitorPointDestinations.MeetingCreatorBot);
            //CopyFiles(patchDirectory, VpPatchFolders.WebReg, VisitorPointDestinations.WebReg);
            //CopyFiles(patchDirectory, VpPatchFolders.Web, VisitorPointDestinations.Web);
            //CopyFiles(patchDirectory, VpPatchFolders.Outlook, VisitorPointDestinations.Outlook);
            //CopyFiles(patchDirectory, VpPatchFolders.VisitorPointKiosk, VisitorPointDestinations.Kiosk);

            if (_subDirecotories.FirstOrDefault(x => x.Name.Equals("Sql", StringComparison.OrdinalIgnoreCase)) is FileDetails Sqldir)
            {
                await ExecuteSqlFiles(Sqldir);
            }
        }

        private void CopyFiles(string directory, string folderName, QuanikaDestinations quanikaDestinations)
        {
            if (_subDirecotories.FirstOrDefault(x => x.Name == folderName) is FileDetails dir)
            {
                var path = PathsHelper.GetQuanikaPaths(quanikaDestinations);
                if (string.IsNullOrEmpty(path))
                {
                    MainWindow.UpdatePbLabel("Executing script ");
                    return;
                }

                CopyDirectory(dir.Path, path);
            }
        }
    }
}
