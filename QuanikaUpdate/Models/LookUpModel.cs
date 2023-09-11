using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace AxisInstallerPackage.Models
{
    public class LookUpModel
    {
        public string valueMember { get; set; }
        public string displayMember { get; set; }
        public LookUpModel()
        {
            valueMember = "UnKnown";
            displayMember = "UnKnown";
        }
    }
}
