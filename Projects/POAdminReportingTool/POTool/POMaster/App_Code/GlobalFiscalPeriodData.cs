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
    public partial class GlobalFiscalPeriodData : FiscalPeriod
    {

        public List<RegionFiscalPeriodData> Regions { get; set; }
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

        private void ResetData()
        {
            TotalPO = 0;
            SuspendedPO = 0;
            Regions = null;
        }

        public GlobalFiscalPeriodData()
        {
            ResetData();
        }

        public GlobalFiscalPeriodData(FiscalPeriod _data)
        {
            ResetData();
            Period = _data.Period;
            Year = _data.Year;
            StartDate = _data.StartDate;
            EndDate = _data.EndDate;

            init();
        }

        new public void init()
        {
            base.init();
            LoadRegionalData();
        }

        private void LoadRegionalData()
        {
            if (Year == 0 || Period == 0 || StartDate == DateTime.MinValue || EndDate == DateTime.MinValue) return;

            Regions = Factory.GetRegionalFiscalPeriodData(this);
            TotalPO = 0;
            SuspendedPO = 0;
            foreach (RegionFiscalPeriodData d in Regions)
            {
                TotalPO += d.TotalPO;
                SuspendedPO += d.SuspendedPO;
            }

        }
        
    }
}