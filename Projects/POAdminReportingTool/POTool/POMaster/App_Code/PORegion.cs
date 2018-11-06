using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for PORegion
/// </summary>
/// 
namespace POReports
{
    public class PORegion
    {
        public string RegionID { get; set; }
        public string Name { get; set; }

        public PORegion()
        {
            RegionID = "";
            Name = "";
        }
    }
}