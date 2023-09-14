using ApplicationSetup.Extensions;
using QuanikaUpdate.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using VPSetup.Helpers;

namespace VPSetup.Database
{
    public class DAL
    {
        private SqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;


        public DAL()
        {
            Initialize();
        }

        //Initialize values
        private void Initialize()
        {
            try
            {
                Helper.GetDbCredentials();
                server = CConfig.Setting.server;
                database = CConfig.Setting.database;
                uid = CConfig.Setting.username;
                password = Helper.Decrypt(CConfig.Setting.password);
                string connectionString;
                connectionString = "SERVER=" + server + ";" + "DATABASE=" + database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";MultipleActiveResultSets=true;";
                connection = new SqlConnection(connectionString);
            }
            catch (Exception ex)
            {
                Helper.writeLog(ex, "Initialize");
            }
        }

        internal bool IsConnected(out string msg)
        {
            bool result = false;
            msg = "";
            try
            {
                using (SqlConnection conn = new SqlConnection(string.Format("server={0};uid={1};pwd={2};database=QuanikaDb", CConfig._db_server, CConfig._db_username, CConfig._db_pwd)))
                {
                    if (conn.State != ConnectionState.Open)
                    {
                        conn.Close();
                        conn.Open();
                    }

                    if (conn.State == ConnectionState.Open)
                    {
                        result = true;
                        conn.Close();
                    }
                }
            }
            catch (SqlException ex)
            {
                if (ex.Message.ToString().Contains("login failed for user", StringComparison.OrdinalIgnoreCase))
                {
                    msg = "Login failed. Invalid username or password.";
                }
                else if (ex.Message.ToString().Contains("a network-related or instance-specific error occurred", StringComparison.OrdinalIgnoreCase))
                {
                    msg = "Unable to connect to Database Server. Please check your database settings";
                }
                else
                {
                    msg = ex.Message.ToString();
                }
                Helper.writeLog(ex);
            }
            return result;
        }


        private bool OpenConnection()
        {
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Close();
                    connection.Open();
                }

                if (connection.State == ConnectionState.Open)
                    return true;
                else
                    return false;

            }
            catch (SqlException ex)
            {
                Helper.writeLog(ex);

                return false;
            }

        }

        //Close connection
        private bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (SqlException ex)
            {
                Helper.writeLog(ex);
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        internal bool checkConnection()
        {
            return this.OpenConnection();
        }


        internal bool InsertUpdateLogs(string version, string command, string Hostname)
        {

            try
            {

                Thread.Sleep(1000);

                if (this.OpenConnection() == true)
                {
                    SqlCommand cmd = connection.CreateCommand();
                    string query = "INSERT into UpdateLogs (Version, Command, Status, TimeStamp , Hostname) ";
                    query += " VALUES(@Version, @Command, @Status, @TimeStamp , @Hostname) ";
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@Version", version);
                    cmd.Parameters.AddWithValue("@Command", command);
                    cmd.Parameters.AddWithValue("@Status", false);
                    cmd.Parameters.AddWithValue("@TimeStamp", DateTime.Now);
                    cmd.Parameters.AddWithValue("@Hostname", Hostname);
                    cmd.ExecuteNonQuery();
                    this.CloseConnection();
                    return true;
                }

                return false;
            }
            catch (Exception e)
            {
                Helper.writeLog(e);
                return false;
            }

        }

        internal bool CheckIfVersion(string Version)
        {
            try
            {
                bool retOutput = false;
                string query = "Select count(*) as total from [UpdateLogs] where version = '" + Version + "'";
                if (this.OpenConnection() == true)
                {
                    SqlCommand cmd = new SqlCommand(query, connection);
                    var result = cmd.ExecuteScalar();

                    if (result == null)
                    {
                        retOutput = false;
                    }
                    else
                    {
                        Int32 count = (Int32)result;
                        if (count > 0)
                            retOutput = true;
                        else
                            retOutput = false;
                    }
                }
                return retOutput;
            }
            catch (Exception ex)
            {
                Helper.writeLog(ex, "CheckIfVersion");
                return false;
            }
        }


        internal bool CheckIfUpdateVersion(string Version, string hostName)
        {
            try
            {
                bool retOutput = false;
                string query = "Select count(*) as total from [UpdateLogs]  where version = '" + Version + "'and Hostname = '" + hostName + "'  and status = 'false'";
                if (this.OpenConnection() == true)
                {
                    SqlCommand cmd = new SqlCommand(query, connection);
                    var result = cmd.ExecuteScalar();

                    if (result == null)
                    {
                        retOutput = false;
                    }
                    else
                    {
                        Int32 count = (Int32)result;
                        if (count > 0)
                            retOutput = true;
                        else
                            retOutput = false;
                    }
                }
                return retOutput;
            }
            catch (Exception ex)
            {
                Helper.writeLog(ex, "CheckIfUpdateVersion");
                return false;
            }
        }



        internal bool CheckIfUpdated(IEnumerable<string> versions)
        {
            try
            {
                bool retOutput = false;
                SqlCommand cmd = connection.CreateCommand();
                string query = "Select count(*) as total from [UpdateLogs] where version in ({0}) and status = 'false'";
                if (this.OpenConnection() == true)
                {
                    var idParameterList = new List<string>();
                    var index = 0;
                    foreach (var ver in versions)
                    {
                        var paramName = "@idParam" + index;
                        cmd.Parameters.AddWithValue(paramName, ver);
                        idParameterList.Add(paramName);
                        index++;
                    }
                    cmd.CommandText = String.Format(query, string.Join(",", idParameterList));

                    var result = cmd.ExecuteScalar();

                    if (result == null)
                    {
                        retOutput = false;
                    }
                    else
                    {
                        Int32 count = (Int32)result;
                        if (count > 0)
                            retOutput = true;
                        else
                            retOutput = false;
                    }
                }
                return retOutput;
            }
            catch (Exception ex)
            {
                Helper.writeLog(ex, "CheckIfUpdated");
                return false;
            }
        }
        internal List<UpdateLogs> getUpdateLogs(int version, string hostName)
        {
            try
            {
                Thread.Sleep(1000);

                List<UpdateLogs> logsList = new List<UpdateLogs>();
                string query = "select Id, Command , Status , Version ,  Hostname   from UpdateLogs  where REPLACE(version,'.','') > '" + version + "' and Hostname = '" + hostName + "'  order by REPLACE(version,'.','') asc";
                OpenConnection();
                SqlCommand cmd = new SqlCommand(query, connection);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        UpdateLogs logs = new UpdateLogs();
                        logs.Id = Convert.ToInt64(reader["Id"]);
                        logs.version = Convert.ToString(reader["Version"]);
                        logs.Command = Convert.ToString(reader["Command"]);
                        logs.status = Convert.ToBoolean(reader["Status"]);
                        logs.Hostname = Convert.ToString(reader["Hostname"]);
                        logsList.Add(logs);

                    }
                }
                return logsList;
            }
            catch (Exception ex)
            {
                Helper.writeLog(ex);
                return null;
            }
        }

        internal List<UpdateLogs> getDbLogs(int version)
        {
            try
            {
                Thread.Sleep(1000);

                List<UpdateLogs> logsList = new List<UpdateLogs>();
                string query = "select Id, Version , Command , Status ,  Hostname   from UpdateLogs  where REPLACE(version,'.','') > '" + version + "' and Command like '%run%' and Status = 'false' order by REPLACE(version,'.','') asc";
                OpenConnection();
                SqlCommand cmd = new SqlCommand(query, connection);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        UpdateLogs logs = new UpdateLogs();
                        logs.Id = Convert.ToInt64(reader["Id"]);
                        logs.version = Convert.ToString(reader["Version"]);
                        logs.Command = Convert.ToString(reader["Command"]);
                        logs.status = Convert.ToBoolean(reader["Status"]);
                        logs.Hostname = Convert.ToString(reader["Hostname"]);
                        logsList.Add(logs);

                    }
                }
                return logsList;
            }
            catch (Exception ex)
            {
                Helper.writeLog(ex);
                return null;
            }
        }

        internal List<UpdateLogs> GetVpDbLogs(int version)
        {
            try
            {
                Thread.Sleep(1000);

                List<UpdateLogs> logsList = new List<UpdateLogs>();
                string query = "select Id, Version , Command , Status ,  Hostname   from UpdateLogs  where REPLACE(version,'.','') > '" + version + "' and Command like '%run sql%' and Status = 'false' order by REPLACE(version,'.','') asc";
                OpenConnection();
                SqlCommand cmd = new SqlCommand(query, connection);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        UpdateLogs logs = new UpdateLogs();
                        logs.Id = Convert.ToInt64(reader["Id"]);
                        logs.version = Convert.ToString(reader["Version"]);
                        logs.Command = Convert.ToString(reader["Command"]);
                        logs.status = Convert.ToBoolean(reader["Status"]);
                        logs.Hostname = Convert.ToString(reader["Hostname"]);
                        logsList.Add(logs);

                    }
                }
                return logsList;
            }
            catch (Exception ex)
            {
                Helper.writeLog(ex);
                return null;
            }
        }
        internal List<UpdateLogs> getPendingLogs(string hostName)
        {
            try
            {
                Thread.Sleep(1000);

                List<UpdateLogs> logsList = new List<UpdateLogs>();
                string query = " select Id, Command , Status , Version , Hostname   from UpdateLogs  where Status = 'false' and Hostname = '" + hostName + "'  order by REPLACE(version,'.','') asc";
                OpenConnection();
                SqlCommand cmd = new SqlCommand(query, connection);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        UpdateLogs logs = new UpdateLogs();
                        logs.Id = Convert.ToInt64(reader["Id"]);
                        logs.version = Convert.ToString(reader["Version"]);
                        logs.Command = Convert.ToString(reader["Command"]);
                        logs.status = Convert.ToBoolean(reader["Status"]);
                        logs.Hostname = Convert.ToString(reader["Hostname"]);
                        logsList.Add(logs);

                    }
                }
                return logsList;
            }
            catch (Exception ex)
            {
                Helper.writeLog(ex);
                return null;
            }
        }
        public bool runSqlScriptFile(string script)
        {
            try
            {
                if (string.IsNullOrEmpty(script)) return false;

                // split script on GO command
                System.Collections.Generic.IEnumerable<string> commandStrings = Regex.Split(script, @"^\s*GO\s*$",
                                         RegexOptions.Multiline | RegexOptions.IgnoreCase);

                connection.Open();
                foreach (string commandString in commandStrings)
                {
                    if (commandString.Trim() != "")
                    {
                        using (var command = new SqlCommand(commandString, connection))
                        {
                            try
                            {
                                command.ExecuteNonQuery();
                            }
                            catch (SqlException ex)
                            {
                                Helper.writeLog(ex, " 9024S ");
                                string spError = commandString.Length > 100 ? commandString.Substring(0, 100) + " ...\n..." : commandString;
                                //(string.Format("Please check the SqlServer script.\nFile: {0} \nLine: {1} \nError: {2} \nSQL Command: \n{3}", pathStoreProceduresFile, ex.LineNumber, ex.Message, spError));
                                return false;
                            }
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Helper.writeLog(ex);
                return false;
            }
        }
        public long ExecuteScalar(string procedureName, params SqlParameter[] param)
        {
            try
            {
                var cmd = CreateCommand(procedureName, param);
                OpenConnection();
                return Convert.ToInt64(cmd.ExecuteScalar());
            }
            finally
            {
                CloseConnection();
            }
        }

        private SqlCommand CreateCommand(string procedureName, params SqlParameter[] param)
        {
            OpenConnection();
            var cmd = new SqlCommand(procedureName, connection)
            {
                CommandType = CommandType.StoredProcedure,
                CommandTimeout = 500
            };
            // add proc parameters
            cmd.Parameters.Clear();
            if (param != null)
            {
                cmd.Parameters.AddRange(param);
            }
            // return param
            cmd.Parameters.Add(new SqlParameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.ReturnValue, false,
                0, 0, string.Empty, DataRowVersion.Default, null));

            return cmd;
        }
        internal bool UpdateLogStatus(long Id)
        {
            bool result = false;
            try
            {
                Thread.Sleep(1000);
                if (this.OpenConnection() == true)
                {
                    SqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "UPDATE  [UpdateLogs] SET Status = 'true' Where Id = @Id ";
                    cmd.Parameters.AddWithValue("@Id", Id);
                    cmd.ExecuteNonQuery();
                    this.CloseConnection();
                    result = true;
                }
                return result;
            }
            catch (Exception ex)
            {
                Helper.writeLog(ex, "UpdateLogStatus");
                return result;
            }

        }

        internal bool UpdateLogStatus(string version, string hostName)
        {
            bool result = false;
            try
            {
                Thread.Sleep(1000);
                if (this.OpenConnection() == true)
                {

                    SqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = " update UpdateLogs set status = 'False'  where version = @version and Hostname = @hostName ";
                    cmd.Parameters.AddWithValue("@version", version);
                    cmd.Parameters.AddWithValue("@hostName", hostName);
                    cmd.ExecuteNonQuery();
                    this.CloseConnection();
                    result = true;
                }
                return result;
            }
            catch (Exception ex)
            {
                Helper.writeLog(ex, "UpdateLogStatus");
                return result;
            }

        }
        internal List<InstalledVersionModel> getAppStatuses()
        {
            try
            {
                Thread.Sleep(1000);
                List<InstalledVersionModel> verList = new List<InstalledVersionModel>();
                string query = @" select ver.Hostname , ver.Type ,ver.Version, CASE WHEN
    EXISTS(
      SELECT *
        FROM UpdateLogs   logs
       WHERE logs.status = 'false' and logs.InstalledVersion = ver.Id
    )
  THEN
    0
  ELSE
    1 end as status from Installed_Versions ver  order by REPLACE(ver.Version,'.','')  desc

  ;";
                OpenConnection();
                SqlCommand cmd = new SqlCommand(query, connection);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        InstalledVersionModel ver = new InstalledVersionModel();

                        ver.Hostname = Convert.ToString(reader["Hostname"]);
                        ver.Type = Convert.ToString(reader["type"]);
                        ver.version = Convert.ToString(reader["Version"]);
                        ver.status = Convert.ToBoolean(reader["status"]);
                        verList.Add(ver);

                    }
                }
                return verList;
            }
            catch (Exception ex)
            {
                Helper.writeLog(ex);
                return null;
            }
        }

        internal bool CheckIfDbLogsExists(string Version)
        {
            try
            {
                bool retOutput = false;
                string query = "Select count(*) as total from [UpdateLogs] where version = '" + Version + "' and Command like '%run%'  and status = 'true'";
                if (this.OpenConnection() == true)
                {
                    SqlCommand cmd = new SqlCommand(query, connection);
                    var result = cmd.ExecuteScalar();

                    if (result == null)
                    {
                        retOutput = false;
                    }
                    else
                    {
                        Int32 count = (Int32)result;
                        if (count > 0)
                            retOutput = true;
                        else
                            retOutput = false;
                    }
                }
                return retOutput;
            }
            catch (Exception ex)
            {
                Helper.writeLog(ex, "CheckIfDbLogsExists");
                return false;
            }
        }

        internal bool CheckIfLogAlreadyExist(string Version, string hostName, string type)
        {
            try
            {
                bool retOutput = false;
                string query = "Select count(*) as total from [UpdateLogs]  where version = '" + Version + "'and Hostname = '" + hostName + "'  and command = '" + type + "'";
                if (this.OpenConnection() == true)
                {
                    SqlCommand cmd = new SqlCommand(query, connection);
                    var result = cmd.ExecuteScalar();

                    if (result == null)
                    {
                        retOutput = false;
                    }
                    else
                    {
                        Int32 count = (Int32)result;
                        if (count > 0)
                            retOutput = true;
                        else
                            retOutput = false;
                    }
                }
                return retOutput;
            }
            catch (Exception ex)
            {
                Helper.writeLog(ex, "CheckIfLogAlreadyExist");
                return false;
            }
        }

    }
}
