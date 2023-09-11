using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanikaUpdate.Models
{
    public class Setting
    {
        public string server { get; set; }
        public string username { get; set; }
        public string database { get; set; }
        public string password { get; set; }

        public string version { get; set; }

        public string startingVersion { get; set; }

        public double PbPercentage { get; set; }

    }


    public class SettingsReader
    {
        public Setting ReadSettings()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(System.Reflection.Assembly.GetExecutingAssembly().Location);

          

            return new Setting()
            {
                server = config.AppSettings.Settings["server"].Value,
                username = config.AppSettings.Settings["username"].Value,
                database = config.AppSettings.Settings["database"].Value,
                password = config.AppSettings.Settings["password"].Value,
                version = config.AppSettings.Settings["version"].Value,
                startingVersion = config.AppSettings.Settings["startingVersion"].Value,
                PbPercentage = Convert.ToInt64(config.AppSettings.Settings["PbPercentage"].Value)

            };



        }
    }

    public class SettingsWriter
    {
        public void WriteSettings(Setting userSetting)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(System.Reflection.Assembly.GetExecutingAssembly().Location);
            config.AppSettings.Settings.Clear();

            config.AppSettings.Settings.Add("server", userSetting.server);
            config.AppSettings.Settings.Add("username", userSetting.username);
            config.AppSettings.Settings.Add("database", userSetting.database);
            config.AppSettings.Settings.Add("password", userSetting.password);
            config.AppSettings.Settings.Add("version", userSetting.version);
            config.AppSettings.Settings.Add("startingVersion", userSetting.startingVersion);
            config.AppSettings.Settings.Add("PbPercentage",Convert.ToString(userSetting.PbPercentage));

            config.Save(ConfigurationSaveMode.Minimal);
        }
    }
}
