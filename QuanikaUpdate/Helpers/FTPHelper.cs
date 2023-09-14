using FluentFTP;
using QuanikaUpdate.Models;
using QuanikaUpdate.Wins;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;
using VPSetup.Database;
using VPSetup.Helpers;
using static QuanikaUpdate.Models.XmlSerialze;

namespace QuanikaUpdate.Helpers
{
    public class FTPHelper
    {
        DAL db;

        public FTPHelper()
        {
            db = new DAL();
        }


        public async Task<Response> CheckUpdates(MainWindow gui, string Version)
        {
            try
            {
                FtpClient client = new FtpClient();
                client.Host = Helper.Decrypt(CConfig.host);
                client.Credentials = new NetworkCredential(Helper.Decrypt(CConfig.ftp_username), Helper.Decrypt(CConfig.ftp_password));
                try
                {
                    await Task.Run(() => client.Connect());
                }

                catch (Exception ex)
                {
                    Response res = new Response();
                    res.status = false;
                    res.message = _Const.Con_Error_Ftp;
                    res.delveloper_message = ex.StackTrace;
                    return res;

                }

                List<string> directories = new List<string>();
                foreach (FtpListItem item in client.GetListing("/Uploads"))
                {
                    if (item.Type == FtpFileSystemObjectType.Directory)
                    {
                        if (Helper.isSmallVersion(Version, item.Name))
                        {
                            directories.Add(item.Name);
                        }
                    }

                }
                if (directories.Count > 0)
                {
                    var ascDirec = directories.OrderBy(f => f.Replace(".", string.Empty));
                    MessageBoxResult result = DisplayMessageBox.Show("Info", _Const.Updates_Found, MessageBoxButton.YesNo, Wins.MessageBoxImage.Information);
                    if (result == MessageBoxResult.Yes)
                    {

                        gui.UpdateWindow(_Const.Downloading_Update);
                        await Task.Run(() => Helper.taskDealy());


                        foreach (FtpListItem item in client.GetListing("/Uploads"))
                        {
                            foreach (var name in ascDirec)
                            {

                                if (item.Name == name)
                                {

                                    string loc = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                                    Directory.CreateDirectory(loc + "\\Updates\\" + item.Name);
                                    await Task.Run(() => client.DownloadDirectory(loc + "\\Updates\\" + item.Name, item.FullName, FtpFolderSyncMode.Update));

                                    var serializer = new XmlSerializer(typeof(Updates));
                                    using (var stream = new StreamReader(loc + "\\Updates\\" + item.Name + "\\version.xml"))
                                    {
                                        var deserializeData = (Updates)serializer.Deserialize(stream);
                                        string version = deserializeData.version;
                                        // Insert db logs in database from xml
                                        foreach (var data in deserializeData.Files.Database)
                                        {
                                            data.Value = Regex.Replace(data.Value, @"^\s+|\t|\n|\r", "");
                                            if (!db.CheckIfDbLogsExists(version))
                                            {
                                                var logsresponse = await Helper.InsertQuanikaUpdateLogs("Sql Server", data.Value, null, "sql", gui, version);
                                                if (logsresponse.status == false)
                                                {
                                                    return logsresponse;
                                                }
                                            }
                                        }
                                        // Insert Dll logs into database from xml
                                        foreach (var data in deserializeData.Files.DLL)
                                        {
                                            data.Value = Regex.Replace(data.Value, @"^\s+|\t|\n|\r", "");
                                            var logsresponse = await Helper.InsertQuanikaUpdateLogs(data.module, data.Value, data.folder, "dll", gui, version);
                                            if (logsresponse.status == false)
                                            {
                                                return logsresponse;
                                            }
                                        }

                                        // Insert exe logs into database from xml
                                        foreach (var data in deserializeData.Files.EXE)
                                        {
                                            data.Value = Regex.Replace(data.Value, @"^\s+|\t|\n|\r", "");
                                            var logsresponse = await Helper.InsertQuanikaUpdateLogs(data.module, data.Value, data.folder, "exe", gui, version);
                                            if (logsresponse.status == false)
                                            {
                                                return logsresponse;
                                            }
                                        }

                                        // Insert file logs into database from xml
                                        foreach (var data in deserializeData.Files.AnyFile)
                                        {
                                            data.Value = Regex.Replace(data.Value, @"^\s+|\t|\n|\r", "");
                                            var logsresponse = await Helper.InsertQuanikaUpdateLogs(data.module, data.Value, data.folder, "exe", gui, version);
                                            if (logsresponse.status == false)
                                            {
                                                return logsresponse;
                                            }
                                        }

                                        // Insert keys logs into database from xml
                                        foreach (var data in deserializeData.Files.Keys)
                                        {
                                            data.Value = Regex.Replace(data.Value, @"^\s+|\t|\n|\r", "");
                                            var logsresponse = await Helper.InsertQuanikaUpdateLogs(data.module, data.Value, data.key, "keys", gui, version);
                                            if (logsresponse.status == false)
                                            {
                                                return logsresponse;
                                            }
                                        }

                                    }

                                }
                            }

                        }

                    }
                    else
                    {

                        Application.Current.Shutdown();
                    }

                    Response res = new Response();
                    res.status = true;
                    res.message = _Const.Success_Download_Updates;
                    res.delveloper_message = _Const.Success_Download_Updates;
                    return res;

                }
                else
                {
                    Response res = new Response();
                    res.status = false;
                    res.message = _Const.No_Updates;
                    res.delveloper_message = _Const.No_Updates;
                    return res;

                }


            }
            catch (Exception ex)
            {
                Helper.writeLog(ex);
                Response res = new Response();
                res.status = false;
                res.message = _Const.Error_Download_Updates;
                res.delveloper_message = ex.StackTrace;
                return res;



            }

        }

    }
}
