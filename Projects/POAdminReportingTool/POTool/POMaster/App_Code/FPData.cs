using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

/// <summary>
/// Summary description for FPData
/// </summary>
/// 
namespace POReports
{
    public class FiscalData
    {
        public FiscalPeriod CurrentPeriod { get; set; }
        public FiscalPeriod NextPeriod { get; set; }
        public FiscalPeriod PreviousPeriod { get; set; }
        public FiscalWeek CurrentWeek { get; set; }
        public FiscalWeek NextWeek { get; set; }
        public FiscalWeek PreviousWeek { get; set; }
        //public FiscalYear CurrentYear { get; set; }
        //public FiscalYear NextYear { get; set; }
        //public FiscalYear PreviousYear { get; set; }

        public FiscalData()
        {

        }

        public void init()
        {
            // Load all data based on the current date.
            LoadCurrentFiscalWeekData();
            LoadCurrentFiscalPeriodData();
        }

        #region Fiscal Week
        
        private void LoadCurrentFiscalWeekData()
        {
            using (SqlConnection cn = new SqlConnection(Config.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("GetFiscalWeekPCN", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (rdr.HasRows)
                {
                    int row = 0;
                    while (rdr.Read())
                    {
                        switch (row)
                        {
                            case 0:
                                PreviousWeek = Factory.GetAs<FiscalWeek>(rdr);
                                PreviousWeek.init();
                                break;
                            case 1:
                                CurrentWeek = Factory.GetAs<FiscalWeek>(rdr);
                                CurrentWeek.init();
                                break;
                            case 2:
                                NextWeek = Factory.GetAs<FiscalWeek>(rdr);
                                NextWeek.init();
                                break;
                            default: break;
                        }
                        row++;
                    }
                }
            }
        }

        #endregion

        #region Fiscal Period
        private void LoadCurrentFiscalPeriodData()
        {
            using (SqlConnection cn = new SqlConnection(Config.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("GetFiscalPeriodPCN", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (rdr.HasRows)
                {
                    int row = 0;
                    while (rdr.Read())
                    {
                        switch (row)
                        {
                            case 0:
                                PreviousPeriod = Factory.GetAs<FiscalPeriod>(rdr);
                                PreviousPeriod.init();
                                break;
                            case 1:
                                CurrentPeriod = Factory.GetAs<FiscalPeriod>(rdr);
                                CurrentPeriod.init();
                                break;
                            case 2:
                                NextPeriod = Factory.GetAs<FiscalPeriod>(rdr);
                                NextPeriod.init();
                                break;
                            default: break;
                        }
                        row++;
                    }
                }
            }
        }
        #endregion
    }
}