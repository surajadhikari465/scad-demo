using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;

/// <summary>
/// Summary description for FiscalWeekData
/// </summary>
namespace POReports
{
    public partial class FiscalPeriodData : FiscalPeriod
    {
        public List<FiscalWeekData> FiscalWeeks { get; set; }
        public int TotalPO {get; set;}
        public int SuspendedPO { get; set; }
        public decimal PercentOfTotal
        {
            get
            {
                if (TotalPO > 0 && SuspendedPO > 0)
                {
                    return Convert.ToDecimal((SuspendedPO / TotalPO) * 100);
                }
                else if (TotalPO > 0 && (TotalPO == SuspendedPO))
                {
                    return 100;
                }
                else
                {
                    return 0;
                }
            }
        }
        public string Region { get; set; }

        private void ResetData()
        {
            TotalPO = 0;
            SuspendedPO = 0;
            FiscalWeeks = null;
            Region = "";
        }

        public FiscalPeriodData()
        {
            ResetData();
        }

        public FiscalPeriodData(FiscalPeriod _data, string _region)
        {
            ResetData();
            Period = _data.Period;
            Year = _data.Year;
            StartDate = _data.StartDate;
            EndDate = _data.EndDate;
            Region = _region;

            init();
        }

        new public void init()
        {
            base.init();
            LoadFiscalWeeks();
        }

        private void LoadFiscalWeeks()
        {
            if (Year == 0 || Period == 0 || StartDate == DateTime.MinValue || EndDate == DateTime.MinValue) return;

            FiscalWeeks = Factory.GetFiscalWeekData(Year, Period, Region);
            TotalPO = 0;
            SuspendedPO = 0;
            foreach (FiscalWeekData d in FiscalWeeks)
            {
                TotalPO += d.TotalPO;
                SuspendedPO += d.SuspendedPO;
            }

        }
    }
}