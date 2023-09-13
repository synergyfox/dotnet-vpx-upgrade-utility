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
        private string _version = string.Empty;


        protected async override Task ManageSinglePatch(string patchDirectory)
        {
            try
            {
                _subDirecotories = GetDirectoryDetails(patchDirectory);

                /*await Task.Run(async () =>*/
                await CopyVpFiles()/*)*/;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task CopyVpFiles()
        {
            try
            {
                CopyVPApplicationsFiles();
                MainWindow.worker_ProgressChanged(33);

                await ExecuteVPSqlFiles();
                MainWindow.worker_ProgressChanged(33);

                CopyVpWebFiles();
                MainWindow.worker_ProgressChanged(100);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void CopyVpWebFiles()
        {
            try
            {
                CopyWeb(VpPatchFolders.Web, "Vp Web");

                CopyWeb(VpPatchFolders.WebReg, "Reg Link");
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task ExecuteVPSqlFiles()
        {
            try
            {
                if (_subDirecotories.FirstOrDefault(x => x.Name.Equals("Sql", StringComparison.OrdinalIgnoreCase)) is FileDetails Sqldir)
                {
                    await ExecuteSqlFiles(Sqldir);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void CopyVPApplicationsFiles()
        {
            try
            {
                CopyFiles(VpPatchFolders.ClientApplication, VisitorPointDestinations.ClientApplication);
                CopyFiles(VpPatchFolders.ComService, VisitorPointDestinations.ComService);
                CopyFiles(VpPatchFolders.DataUploadBot, VisitorPointDestinations.DataUploadBot);
                CopyFiles(VpPatchFolders.MeetingCreatorBot, VisitorPointDestinations.MeetingCreatorBot);

                CopyFiles(VpPatchFolders.Outlook, VisitorPointDestinations.Outlook);
                CopyFiles(VpPatchFolders.VisitorPointKiosk, VisitorPointDestinations.Kiosk);
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void CopyWeb(string patchDirectoryName, string webName)
        {
            try
            {
                if (_subDirecotories.FirstOrDefault(x => x.Name.Equals(patchDirectoryName, StringComparison.OrdinalIgnoreCase)) is FileDetails web)
                {
                    CopyWebFiles(web.Path, webName);
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        private void CopyFiles(string folderName, VisitorPointDestinations visitorPointDestinations)
        {
            try
            {
                if (_subDirecotories.FirstOrDefault(x => x.Name == folderName) is FileDetails dir)
                {
                    var path = PathsHelper.GetVisitorPointPaths(visitorPointDestinations);
                    if (string.IsNullOrEmpty(path))
                    {
                        MainWindow.UpdatePbLabel("Executing script ");
                        return;
                    }

                    var res = CopyDirectory(dir.Path, path);
                    if (res)
                    {
                        Database.InsertUpdateLogs(_version, $"{dir.Name} Copied", HostName ?? string.Empty);
                    }
                }
            }
            catch (Exception)
            {
                throw;
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
