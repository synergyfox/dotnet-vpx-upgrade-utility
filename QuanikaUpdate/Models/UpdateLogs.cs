using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanikaUpdate.Models
{
   public class UpdateLogs
    {
        public long Id { get; set; }
        public string version { get; set; }
        public string Command { get; set; }
        public bool status { get; set; }

        public int installedVersion { get; set; }

        public string Hostname { get; set; }
    }


    public class Logs
    {
       
        public string command { get; set; }
       
    }
}
