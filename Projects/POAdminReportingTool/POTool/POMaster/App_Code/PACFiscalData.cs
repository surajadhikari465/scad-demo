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
    public class PACFiscalData
    {
        public int SuspendedLineItems { get; set; }
        public int SuspendedPO { get; set; }
        public string Vendor { get; set; }        
        public FiscalWeekData FiscalWeek { get; set; }
        public decimal SuspendedItemsPerPO
        {
            get
            {
                decimal value = 0;
                try { value = SuspendedLineItems / SuspendedPO; }
                catch { }
                return value;
            }
        }

        private void ResetData()
        {
            SuspendedLineItems = 0;
            SuspendedPO = 0;
            Vendor = "";
            FiscalWeek = null;

        }

        public PACFiscalData()
        {
            ResetData();
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

        public PACFiscalData(FiscalWeek _data)
        {
            ResetData();
            SetFiscalWeek(_data);
            LoadPACData();
        }

        public PACFiscalData(FiscalWeek _data, string _vendor)
        {
            ResetData();
            SetFiscalWeek(_data);
            Vendor = _vendor;
            LoadPACData();
        }

        public void LoadPACData()
        {
            // TODO: Need to write query for Pay by Agreed cost vendors

            return;

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
                    SuspendedLineItems = Convert.ToInt32(rdr[0].ToString());
                    SuspendedPO = Convert.ToInt32(rdr[1].ToString());
                }
            }
        }
    }
}