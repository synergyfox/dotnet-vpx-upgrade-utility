using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanikaUpdate.Models
{
  public  class VersionModel
    {
        public string MinVersion { get; set; }
        public string MaxVersion { get; set; }
    }

    public class VersionsModel
    {
        public string DBVersion { get; set; }
        public decimal CleanVersion { get; set; }
    }


    public class InstalledVersionModel
    {
        public int Id { get; set; }
        public string Hostname { get; set; }
        public string IpAddress { get; set; }
        public string Type { get; set; }
        public string version { get; set; }
        public bool status { get; set; }
        public bool IsCancelled { get; set; }

    }

    public class logCommand
    {
        public string Title { get; set; }
        public string Module { get; set; }
        public string Filename { get; set; }

        public string Folder { get; set; }
        
    }

    public class CurrentVersionModel
    {
        public string appHost { get; set; }
        public string appVersion { get; set; }
        public string dxHost { get; set; }
        public string dxVersion { get; set; }

    }

    public class VersionInformation
    {   
        public string App { get; set; }
        public decimal Version { get; set; }
        public bool IsInstalled { get; set; }

    }

}
