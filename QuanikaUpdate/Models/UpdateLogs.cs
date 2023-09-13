namespace QuanikaUpdate.Models
{
    public class UpdateLogs
    {
        public long Id { get; set; }
        public string Version { get; set; }
        public string Command { get; set; }
        public bool Status { get; set; }

        public int InstalledVersion { get; set; }

        public string Hostname { get; set; }
    }


    public class Logs
    {

        public string command { get; set; }

    }
}
