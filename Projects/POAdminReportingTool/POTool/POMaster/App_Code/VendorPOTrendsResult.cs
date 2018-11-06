using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for VendorTrend
/// </summary>
namespace POReports
{
    public class VendorPOTrendsResult
    {
        public List<POData> POData { get; set; }
        public List<VendorTrendRow> Trends { get; set; }
        public List<ResolutionCodeData> ResolutionTotals { get; set; }
        public string Region { get; set; }

        public VendorPOTrendsResult(){
            Region = "";
            POData = new List<POData>();
            Trends = new List<VendorTrendRow>();
            ResolutionTotals  = new List<ResolutionCodeData>();
        }
    }
}