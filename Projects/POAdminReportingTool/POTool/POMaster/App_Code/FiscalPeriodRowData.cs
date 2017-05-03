using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;

/// <summary>
/// Summary description for FiscalPeriodRowData
/// </summary>
namespace POReports
{
    public partial class FiscalPeriodRowData
    {
        public int Year { get; set; }
        public int Period { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<VendorFiscalData> VendorRows { get; set; }
        public List<StoreFiscalData> StoreRows { get; set; }
        public List<FiscalResolutionData> ResolutionRows { get; set; }
        private string loadType { get; set; }
        public string Region { get; set; }
        public FiscalPeriodData FiscalPeriod
        {
            get
            {
                if (_fiscalPeriodData == null && (Year > 0 && Period > 0))
                {
                    _fiscalPeriodData = new FiscalPeriodData(Factory.GetFiscalPeriod(Year, Period), Region);
                    _fiscalPeriodData.init();
                }
                return _fiscalPeriodData;
            }
        }
        private FiscalPeriodData _fiscalPeriodData { get; set; }

        private void ResetData()
        {
            Year = 0;
            Period = 0;
            StartDate = DateTime.MinValue;
            EndDate = DateTime.MinValue;
            VendorRows = new List<VendorFiscalData>();
            StoreRows = new List<StoreFiscalData>();
            ResolutionRows = new List<FiscalResolutionData>();
            loadType = "all";
        }

        public FiscalPeriodRowData()
        {
            ResetData();
        }

        public FiscalPeriodRowData(int FY, int FP, string type, string _region)
        {
            ResetData();
            Year = FY;
            Period = FP;
            loadType = type;
            Region = _region;
        }

        public void init()
        {
            // Attempt to load start and end dates if we do not have them.
            if((StartDate == DateTime.MinValue || EndDate == DateTime.MinValue) && (Year > 0 && Period > 0)){
                StartDate = FiscalPeriod.StartDate;
                EndDate = FiscalPeriod.EndDate;
            }
            load();
        }

        private void load()
        {
            if (StartDate == null || EndDate == null || StartDate == DateTime.MinValue || EndDate == DateTime.MinValue)
            {
                return;
            }

            string query = "";

            VendorRows = new List<VendorFiscalData>();
            StoreRows = new List<StoreFiscalData>();
            ResolutionRows = new List<FiscalResolutionData>();

            if (loadType == "all" || loadType == "regional_vendor" || loadType == "vendor")
            {
                // Load Vendor Data
                query = "SELECT [Vendor] = ISNULL(Vendor, 'Unknown'), [TotalPO] = CAST(COUNT(*) AS INT), [SuspendedPO] = CAST(SUM(CASE WHEN (Suspended = 'Y' AND ResolutionCode <> 'Closed as Other/None') THEN 1 ELSE 0 END) AS INT) FROM POData WHERE CloseDate BETWEEN @StartDate AND @EndDate AND Region = @Region GROUP BY Vendor ORDER BY Vendor ASC";
                using (SqlConnection cn = new SqlConnection(Config.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, cn);
                    cmd.Parameters.AddWithValue("@StartDate", string.Format("{0} 00:00:00", StartDate.ToShortDateString()));
                    cmd.Parameters.AddWithValue("@EndDate", string.Format("{0} 23:59:59", EndDate.ToShortDateString()));
                    cmd.Parameters.AddWithValue("@Region", Region);
                    cn.Open();
                    SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                    if (rdr.HasRows)
                    {
                        while (rdr.Read())
                        {
                            VendorFiscalData d = Factory.GetAs<VendorFiscalData>(rdr);
                            d.FiscalPeriod = FiscalPeriod;
                            d.init();
                            VendorRows.Add(d);
                        }
                    }
                }
            }

            if (loadType == "all" || loadType == "regional_store" || loadType == "store")
            {
                // Load Store Data
                query = "SELECT [Store] = ISNULL(Store, '???'), [TotalPO] = CAST(COUNT(*) AS INT), [SuspendedPO] = CAST(SUM(CASE WHEN (Suspended = 'Y' AND ResolutionCode <> 'Closed as Other/None') THEN 1 ELSE 0 END) AS INT) FROM POData WHERE Region=@Region AND CloseDate BETWEEN @StartDate AND @EndDate GROUP BY Store ORDER BY Store ASC";
                using (SqlConnection cn = new SqlConnection(Config.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, cn);
                    cmd.Parameters.AddWithValue("@StartDate", string.Format("{0} 00:00:00", StartDate.ToShortDateString()));
                    cmd.Parameters.AddWithValue("@EndDate", string.Format("{0} 23:59:59", EndDate.ToShortDateString()));
                    cmd.Parameters.AddWithValue("@Region", Region);
                    cn.Open();
                    SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                    if (rdr.HasRows)
                    {
                        while (rdr.Read())
                        {
                            StoreFiscalData d = Factory.GetAs<StoreFiscalData>(rdr);
                            d.FiscalPeriod = FiscalPeriod;
                            d.init();
                            StoreRows.Add(d);
                        }
                    }
                }
            }

            if (loadType == "all" || loadType == "regional_rc" || loadType == "rc")
            {
                // Load Resolution Code Data
                query = "SELECT [ResolutionCode] = ISNULL(ResolutionCode, 'Not Specified'), [TotalPO] = CAST(COUNT(*) AS INT), [SuspendedPO] = CAST(SUM(CASE WHEN (Suspended = 'Y' AND ResolutionCode <> 'Closed as Other/None') THEN 1 ELSE 0 END) AS INT) FROM POData WHERE Region = @Region AND CloseDate BETWEEN @StartDate AND @EndDate AND ResolutionCode <> '' GROUP BY ResolutionCode ORDER BY ResolutionCode ASC";
                using (SqlConnection cn = new SqlConnection(Config.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, cn);
                    cmd.Parameters.AddWithValue("@StartDate", string.Format("{0} 00:00:00", StartDate.ToShortDateString()));
                    cmd.Parameters.AddWithValue("@EndDate", string.Format("{0} 23:59:59", EndDate.ToShortDateString()));
                    cmd.Parameters.AddWithValue("@Region", Region);
                    cn.Open();
                    SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                    if (rdr.HasRows)
                    {
                        while (rdr.Read())
                        {
                            FiscalResolutionData d = Factory.GetAs<FiscalResolutionData>(rdr);
                            d.FiscalPeriod = FiscalPeriod;
                            d.init();
                            ResolutionRows.Add(d);
                        }
                    }
                }
            }
        }
    }
}