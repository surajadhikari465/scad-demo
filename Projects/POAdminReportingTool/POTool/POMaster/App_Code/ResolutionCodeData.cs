using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ResolutionCodeData
/// </summary>
/// 
namespace POReports
{
    public class ResolutionCodeData
    {
        public string Name { get; set; }
        public int Total { get; set; }

        public ResolutionCodeData()
        {
            Name = "Not specified";
            Total = 0;
        }
    }
}