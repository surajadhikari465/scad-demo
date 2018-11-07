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
    public class StoreFiscalData
    {
        public int TotalPO { get; set; }
        public int SuspendedPO { get; set; }
        public string Store { get; set; }
        public string Region { get; set; }
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
        public FiscalWeekData FiscalWeek { get; set; }
        public FiscalPeriodData FiscalPeriod { get; set; }
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
            Store = "";
            FiscalWeek = null;
            FiscalPeriod = null;
            Region = "";
        }

        public StoreFiscalData()
        {
            ResetData();
        }

        public void init()
        {

        }

        public void SetFiscalPeriod(FiscalPeriod _data, string _region)
        {
            if (_data == null) return;
            Region = _region;
            FiscalPeriod = new FiscalPeriodData();
            FiscalPeriod.Period = _data.Period;
            FiscalPeriod.Year = _data.Year;
            FiscalPeriod.StartDate = _data.StartDate;
            FiscalPeriod.EndDate = _data.EndDate;
            FiscalPeriod.init();
        }

        public void SetFiscalWeek(FiscalWeek _data, string _region)
        {
            if(_data == null) return;
            Region = _region;
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

        public StoreFiscalData(FiscalWeek _data, string _region)
        {
            ResetData();
            SetFiscalWeek(_data, _region);

            FiscalWeek.loadTotals();
        }

        public StoreFiscalData(FiscalWeek _data, string _store, string _region)
        {
            ResetData();
            SetFiscalWeek(_data, _region);
            Store = _store;

            FiscalWeek.loadTotals();
            LoadVendorFWTotals();
        }

        public StoreFiscalData(FiscalPeriod _data, string _region)
        {
            ResetData();
            SetFiscalPeriod(_data, _region);
        }

        public StoreFiscalData(FiscalPeriod _data, string _store, string _region)
        {
            ResetData();
            SetFiscalPeriod(_data, _region);
            Store = _store;

            LoadVendorFPTotals();
        }

        public void LoadVendorFPTotals()
        {
            if (FiscalPeriod.Period == 0 || FiscalPeriod.Year == 0 || String.IsNullOrEmpty(Store))
            {
                return;
            }

            using (SqlConnection cn = new SqlConnection(Config.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("GetStorePOTotals", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartDate", FiscalPeriod.StartDate.ToShortDateString() + " 00:00:00");
                cmd.Parameters.AddWithValue("@EndDate", FiscalPeriod.EndDate.ToShortDateString() + " 23:59:59");
                cmd.Parameters.AddWithValue("@Store", Store);
                cmd.Parameters.AddWithValue("@Region", Region);
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

        public void LoadVendorFWTotals()
        {
            if (FiscalWeek.Period == 0 || FiscalWeek.Year == 0 || FiscalWeek.Week == 0 || String.IsNullOrEmpty(Store))
            {
                return;
            }

            using (SqlConnection cn = new SqlConnection(Config.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("GetStorePOTotals", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartDate", FiscalWeek.StartDate.ToShortDateString() + " 00:00:00");
                cmd.Parameters.AddWithValue("@EndDate", FiscalWeek.EndDate.ToShortDateString() + " 23:59:59");
                cmd.Parameters.AddWithValue("@Store", Store);
                cmd.Parameters.AddWithValue("@Region", Region);
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