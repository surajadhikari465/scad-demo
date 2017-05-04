using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace POReports
{

    public class FiscalWeek
    {
        public int Period { get; set; }
        public int Week { get; set; }
        public int Year { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public void ResetData()
        {
            Period = 0;
            Week = 0;
            Year = 0;
            Description = "";
            StartDate = DateTime.MinValue;
            EndDate = DateTime.MinValue;
        }

        public FiscalWeek()
        {
            ResetData();
            // Load current fiscal week as default
            //load(DateTime.Now);
        }

        public void init()
        {

        }
    }
}