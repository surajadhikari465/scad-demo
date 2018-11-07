using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for FiscalPeriod
/// </summary>
/// 
namespace POReports
{
    public class FiscalPeriod
    {
        public int Period { get; set; }
        public int Quarter { get; set; }
        public int Year { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        private void ResetData()
        {
            Period = 0;
            Quarter = 0;
            Year = 0;
            StartDate = DateTime.MinValue;
            EndDate = DateTime.MinValue;
        }

        public FiscalPeriod()
        {
            ResetData();
        }

        public void init()
        {

        }        
    }
}