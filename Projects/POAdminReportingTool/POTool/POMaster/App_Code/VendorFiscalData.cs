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
    public class VendorFiscalData
    {
        public int TotalPO { get; set; }
        public int SuspendedPO { get; set; }
        public string Vendor { get; set; }
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
        public FiscalPeriodData FiscalPeriod { get; set; }
        public FiscalWeekData FiscalWeek { get; set; }
        public decimal SuspendedContribution
        {
            get
            {
                decimal value = 0;
                if (FiscalWeek != null && FiscalWeek.SuspendedPO > 0 && SuspendedPO > 0)
                {
                    // If week is loaded use it
                    value = Math.Round(((decimal)SuspendedPO / (decimal)FiscalWeek.SuspendedPO) * 100, 2);
                }
                else if (FiscalPeriod != null && FiscalPeriod.SuspendedPO > 0 && SuspendedPO > 0)
                {
                    // If period is loaded use it instead
                    value = Math.Round(((decimal)SuspendedPO / (decimal)FiscalPeriod.SuspendedPO) * 100, 2);
                }
                return value;
            }
        }

        private void ResetData()
        {
            TotalPO = 0;
            SuspendedPO = 0;
            Vendor = "";
            FiscalWeek = null;
            FiscalPeriod = null;
        }

        public VendorFiscalData()
        {
            ResetData();
        }

        public void SetFiscalPeriod(FiscalPeriod _data)
        {
            if (_data == null) return;
            FiscalPeriod = new FiscalPeriodData();
            FiscalPeriod.Period = _data.Period;
            FiscalPeriod.Year = _data.Year;
            FiscalPeriod.StartDate = _data.StartDate;
            FiscalPeriod.EndDate = _data.EndDate;
            FiscalPeriod.init();
        }

        public void SetFiscalWeek(FiscalWeek _data)
        {
            if(_data == null) return;
            FiscalWeek = new FiscalWeekData();
            FiscalWeek.Period = _data.Period;
            FiscalWeek.Year = _data.Year;
            FiscalWeek.Week = _data.Week;
            FiscalWeek.StartDate = _data.StartDate;
            FiscalWeek.EndDate = _data.EndDate;
            FiscalWeek.Description = _data.Description;
            FiscalWeek.init();
            FiscalWeek.loadTotals();
        }

        public void init()
        {
            
        }

        public VendorFiscalData(FiscalWeek _data)
        {
            ResetData();
            SetFiscalWeek(_data);
            LoadVendorWeekTotals();
        }

        public VendorFiscalData(FiscalPeriod _data)
        {
            ResetData();
            SetFiscalPeriod(_data);
            LoadVendorPeriodTotals();
        }

        public VendorFiscalData(FiscalWeek _data, string _vendor)
        {
            ResetData();
            SetFiscalWeek(_data);
            Vendor = _vendor;
            LoadVendorWeekTotals();
        }

        public VendorFiscalData(FiscalPeriod _data, string _vendor)
        {
            ResetData();
            SetFiscalPeriod(_data);
            Vendor = _vendor;
            LoadVendorPeriodTotals();
        }

        public void LoadVendorPeriodTotals()
        {
            if (FiscalPeriod == null || FiscalPeriod.Period == 0 || FiscalPeriod.Year == 0 || String.IsNullOrEmpty(Vendor))
            {
                return;
            }

            string query = string.Format("EXEC [dbo].[GetVendorPOTotals] @StartDate='{0} 00:00:00', @EndDate='{1} 23:59:59', @Vendor='{2}'", FiscalPeriod.StartDate.ToShortDateString(), FiscalPeriod.EndDate.ToShortDateString(), Vendor);
            using (SqlConnection cn = new SqlConnection(Config.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(query, cn);
                cn.Open();
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (rdr.HasRows)
                {
                    rdr.Read();
                    TotalPO = Convert.ToInt32(rdr[0].ToString());
                    SuspendedPO = Convert.ToInt32(rdr[1].ToString());
                }
            }
        }

        public void LoadVendorWeekTotals()
        {
            if (FiscalWeek == null || FiscalWeek.Period == 0 || FiscalWeek.Year == 0 || FiscalWeek.Week == 0 || String.IsNullOrEmpty(Vendor))
            {
                return;
            }

            string query = string.Format("EXEC [dbo].[GetVendorPOTotals] @StartDate='{0} 00:00:00', @EndDate='{1} 23:59:59', @Vendor='{2}'", FiscalWeek.StartDate.ToShortDateString(), FiscalWeek.EndDate.ToShortDateString(), Vendor);
            using (SqlConnection cn = new SqlConnection(Config.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(query, cn);
                cn.Open();
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (rdr.HasRows)
                {
                    rdr.Read();
                    TotalPO = Convert.ToInt32(rdr[0].ToString());
                    SuspendedPO = Convert.ToInt32(rdr[1].ToString());
                }
            }
        }
    }
}