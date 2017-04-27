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
    public partial class RegionFiscalPeriodData : FiscalPeriod
    {
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
        public string RegionName { get; set; }

        new private void ResetData()
        {
            TotalPO = 0;
            SuspendedPO = 0;
            Region = "";
            RegionName = "";
        }

        public RegionFiscalPeriodData()
        {
            ResetData();
        }

        public RegionFiscalPeriodData(GlobalFiscalPeriodData _data, string _region, string _regionName)
        {
            ResetData();
            Period = _data.Period;
            Year = _data.Year;
            StartDate = _data.StartDate;
            EndDate = _data.EndDate;
            Region = _region;
            RegionName = _regionName;

            init();
        }

        new public void init()
        {
            base.init();
            loadTotals();
        }

        public void loadTotals()
        {
            if (Period == 0 || Year == 0)
            {
                return;
            }
            TotalPO = 0;
            SuspendedPO = 0;
            string query = "SELECT [TotalPOTotal] = ISNULL(SUM(TotalPO), 0), [SuspendedPOTotal] = ISNULL(SUM(SuspendedPO), 0) FROM POTotals WHERE FP=@FP AND FY=@FY AND Region=@Region";
            using (SqlConnection cn = new SqlConnection(Config.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@FP", Period);
                cmd.Parameters.AddWithValue("@FY", Year);
                cmd.Parameters.AddWithValue("@Region", Region);
                cn.Open();
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (rdr.HasRows)
                {
                    rdr.Read();                    
                    TotalPO += Convert.ToInt32(rdr[0].ToString());
                    SuspendedPO += Convert.ToInt32(rdr[1].ToString());            
                }
            }
        }
    }
}