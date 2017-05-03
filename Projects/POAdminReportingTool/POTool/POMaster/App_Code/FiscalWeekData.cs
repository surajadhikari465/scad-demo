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
    public partial class FiscalWeekData : FiscalWeek
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

        new private void ResetData()
        {
            TotalPO = 0;
            SuspendedPO = 0;
            Region = "";
            base.ResetData();
        }

        public FiscalWeekData()
        {
            ResetData();
        }

        public FiscalWeekData(FiscalWeek _data, string _region)
        {
            ResetData();
            Period = _data.Period;
            Year = _data.Year;
            Week = _data.Week;
            StartDate = _data.StartDate;
            EndDate = _data.EndDate;
            Description = _data.Description;
            Region = _region;

            init();
        }

        new public void init()
        {
            base.init();
            loadTotals();
        }

        public void loadTotals()
        {
            if (Period == 0 || Year == 0 || Week == 0)
            {
                return;
            }
            TotalPO = 0;
            SuspendedPO = 0;
            string query = "SELECT TOP 1 TotalPO, SuspendedPO FROM POTotals WHERE FP=@FP AND FY=@FY AND FW=@FW AND Region=@Region";
            using (SqlConnection cn = new SqlConnection(Config.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@FP", Period);
                cmd.Parameters.AddWithValue("@FY", Year);
                cmd.Parameters.AddWithValue("@FW", Week);
                cmd.Parameters.AddWithValue("@Region", Region);
                cn.Open();
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        TotalPO += Convert.ToInt32(rdr[0].ToString());
                        SuspendedPO += Convert.ToInt32(rdr[1].ToString());

                    }
                    if (TotalPO == 0)
                    {
                        // Totals POs are empty - try to re-calculate them
                        //calculateTotals();
                    }
                }
                else
                {
                    //calculateTotals();
                }
            }
        }

        private bool ShouldUpdateTotal()
        {
            string query = "SELECT LastUpdated FROM POTotals WHERE FP=@FP AND FY=@FY AND FW=@FW AND Region=@Region";
            using (SqlConnection cn = new SqlConnection(Config.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@FP", Period);
                cmd.Parameters.AddWithValue("@FY", Year);
                cmd.Parameters.AddWithValue("@FW", Week);
                cmd.Parameters.AddWithValue("@Region", Region);
                cn.Open();
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (rdr.HasRows)
                {
                    rdr.Read();
                    DateTime LastUpdated = Convert.ToDateTime(rdr[0].ToString());
                    // Check if last updated was more than a day ago.
                    if (LastUpdated.AddDays(1) < DateTime.Now)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    // No row found, go ahead and generate it!
                    return true;
                }
            }
        }

        public void calculateTotals(bool force)
        {
            if (Period == 0 || Year == 0 || Week == 0)
            {
                return;
            }

            if (ShouldUpdateTotal() || force)
            {

                using (SqlConnection cn = new SqlConnection(Config.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("UpdatePOTotals", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FP", Period);
                    cmd.Parameters.AddWithValue("@FY", Year);
                    cmd.Parameters.AddWithValue("@FW", Week);
                    cmd.Parameters.AddWithValue("@StartDate", StartDate.ToShortDateString() + " 00:00:00");
                    cmd.Parameters.AddWithValue("@EndDate", EndDate.ToShortDateString() + " 23:59:59");
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
}