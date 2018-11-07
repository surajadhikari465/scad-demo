using System;
using System.Data;
using System.IO;
using System.Web;
using System.Web.Script.Serialization;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Drawing.Chart;
using System.Drawing.Imaging;
using OfficeOpenXml.Style;
using OfficeOpenXml.Style.XmlAccess;
using System.Web.UI;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.Protocols;
using System.Reflection;
using System.Threading;

/// <summary>
/// The Factory class contains all of the methods to retrive application data from the database
/// </summary>

namespace POReports
{
    public class Factory
    {

        #region User Methods
        //! Returns a populated UserRecord object based on the supplied Active Directory GUID string
        public static UserRecord GetUser(string ID)
        {
            return GetUser(ID, true);
        }

        public static List<PORegion> GetRegions()
        {
            List<PORegion> list = new List<PORegion>();

            string query = "SELECT * FROM Regions WHERE RegionID != 'CEN' ORDER BY Name ASC";
            using (SqlConnection cn = new SqlConnection(Config.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(query, cn);
                cn.Open();
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        PORegion d = GetAs<PORegion>(rdr);
                        list.Add(d);
                    }
                }
            }

            return list;
        }

        public static PORegion GetRegion(string RegionID)
        {
            PORegion result = new PORegion();

            string query = "SELECT * FROM Regions WHERE RegionID=@RegionID";
            using (SqlConnection cn = new SqlConnection(Config.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@RegionID", RegionID);
                cn.Open();
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        result = GetAs<PORegion>(rdr);
                    }
                }
            }

            return result;
        }

        public static UserRecord GetUser(string UserID, bool LoadFromAD)
        {
            if (string.IsNullOrEmpty(UserID)) return null;
            UserRecord result = null;
            string query = "SELECT TOP 1 * FROM POUsers WHERE UserID=@UserID";
            using (SqlConnection cn = new SqlConnection(Config.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cn.Open();
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (rdr.HasRows)
                {
                    rdr.Read();
                    result = GetAs<UserRecord>(rdr);
                    result.init();
                }
                else if (LoadFromAD)
                {
                    // User not found in cache.
                    // Attempt to get them from Active Directory
                    result = new UserRecord();
                    result.init();
                    Guid UserGuid = new Guid(UserID);
                    result.GetUserFromActiveDirectory(UserGuid);
                    result.Save();
                }
            }
            return result;
        }


        public static bool isAuthorized(string userID)
        {
            UserRecord user = GetUser(userID);
            return user.isAllowed("LOGIN");
        }

        public static List<TeamMember> FindTeamMembers(string Criteria)
        {
            List<TeamMember> tms = new List<TeamMember>();

            string filter = String.Format("(&(objectCategory=person)(objectClass=user)(displayName=*{0}*))", Criteria);
            string query = "OU=REGIONS,DC=wfm,DC=pvt";
            string[] properties = { "givenname", "mail", "displayName", "objectGUID", "sn" };
            //NetworkCredential credentials = System.Net.CredentialCache.DefaultCredentials;
            //LdapDirectoryIdentifier directoryIdentifier = new LdapDirectoryIdentifier("wfm.pvt");
            //using (LdapConnection connection = new LdapConnection(directoryIdentifier, credentials, AuthType.Basic))
            using (LdapConnection connection = new LdapConnection("wfm.pvt"))
            {
                connection.Timeout = new TimeSpan(0, 0, 30);
                connection.SessionOptions.ProtocolVersion = 3;
                SearchRequest search = new SearchRequest(query, filter, System.DirectoryServices.Protocols.SearchScope.Subtree, properties);
                SearchResponse response = (SearchResponse)connection.SendRequest(search);
                foreach (SearchResultEntry entry in response.Entries)
                {
                    //Console.WriteLine(entry.Attributes["mail"][0]);
                    try
                    {
                        TeamMember tm = new TeamMember();
                        //DirectoryEntry entry = result.GetDirectoryEntry();
                        tm.Firstname = (string)entry.Attributes["givenname"][0];
                        tm.Lastname = (string)entry.Attributes["sn"][0];
                        tm.DisplayName = (string)entry.Attributes["displayName"][0];
                        tm.Email = (string)entry.Attributes["mail"][0];
                        Guid userID = new System.Guid((System.Byte[])entry.Attributes["objectguid"][0]);
                        tm.UserID = userID.ToString();
                        //tm.UserID = entry.Guid.ToString();
                        tm.Region = parseRegionFromDisplayName(tm.DisplayName);
                        tm.StoreTLC = parseStoreFromDisplayName(tm.DisplayName);
                        tms.Add(tm);
                    }
                    catch { }
                }
            }

            return tms;
        }

        public static string parseRegionFromDisplayName(string str)
        {
            if (str.IndexOf("(") != -1)
            {
                return str.Substring(str.IndexOf("(") + 1, 2);
            }
            else
            {
                return "";
            }
        }

        public static string parseStoreFromDisplayName(string str)
        {
            if (str.IndexOf("(") != -1)
            {
                return str.Substring(str.IndexOf("(") + 4, 3);
            }
            else
            {
                return "";
            }
        }

        #endregion

        #region Trend Report Methods
        public static VendorPOTrendsResult GetVendorPOsWithRCTotals(DateTime StartDate, DateTime EndDate, string Vendor, string region)
        {
            VendorPOTrendsResult result = new VendorPOTrendsResult();
            result.POData = GetVendorPOs(StartDate, EndDate, Vendor, region);
            result.ResolutionTotals = GetResolutionCodeTotals(StartDate, EndDate, Vendor, null, region);            
            return result;
        }

        public static VendorPOTrendsResult GetVendorPOTrends(int fiscalYear, string Vendor, string region)
        {
            VendorPOTrendsResult result = new VendorPOTrendsResult();
            //result.POData = GetVendorPOs(StartDate, EndDate, Vendor, region);
            //result.ResolutionTotals = GetResolutionCodeTotals(StartDate, EndDate, Vendor, null, region);
            List<VendorFiscalData> data = GetVendorFiscalWeekData(fiscalYear.ToString(), Vendor);
            VendorTrendRow trendRow = null;
            foreach (VendorFiscalData d in data)
            {
                if (trendRow == null || d.FiscalWeek.Period != trendRow.Period)
                {
                    if (trendRow != null)
                    {
                        result.Trends.Add(trendRow);
                    }
                    trendRow = new VendorTrendRow(d.FiscalWeek.Period, d.TotalPO, d.SuspendedPO, region);
                }
                else
                {
                    // Update totals
                    trendRow.TotalPOs += d.TotalPO;
                    trendRow.SuspendedPOs += d.SuspendedPO;
                    trendRow.calculate();
                }
            }

            return result;
        }


        public static VendorPOTrendsResult GetStorePOsWithRCTotals(DateTime StartDate, DateTime EndDate, string Store, string Region)
        {
            VendorPOTrendsResult result = new VendorPOTrendsResult();
            result.POData = GetStorePOs(StartDate, EndDate, Store, Region);
            result.ResolutionTotals = GetResolutionCodeTotals(StartDate, EndDate, null, Store, Region);
            result.Region = Region;            

            return result;
        }

        public static VendorPOTrendsResult GetStorePOTrends(int fiscalYear, string Store, string Region)
        {
            VendorPOTrendsResult result = new VendorPOTrendsResult();
            result.Region = Region;
            List<StoreFiscalData> data = GetStoreFiscalWeekData(fiscalYear.ToString(), Store, Region);
            VendorTrendRow trendRow = null;
            foreach (StoreFiscalData d in data)
            {
                if (trendRow == null || d.FiscalWeek.Period != trendRow.Period)
                {
                    if (trendRow != null)
                    {
                        result.Trends.Add(trendRow);
                    }
                    trendRow = new VendorTrendRow(d.FiscalWeek.Period, d.TotalPO, d.SuspendedPO, Region);
                }
                else
                {
                    // Update totals
                    trendRow.TotalPOs += d.TotalPO;
                    trendRow.SuspendedPOs += d.SuspendedPO;
                    trendRow.calculate();
                }
            }

            return result;
        }

        public static VendorPOTrendsResult GetRCPOsWithoutTrends(DateTime StartDate, DateTime EndDate, string resolutionCode, string Region)
        {
            VendorPOTrendsResult result = new VendorPOTrendsResult();
            result.POData = GetRCPOs(StartDate, EndDate, resolutionCode, Region);            

            return result;
        }

        public static VendorPOTrendsResult GetRCPOTrends(int fiscalYear, string resolutionCode, string Region)
        {
            VendorPOTrendsResult result = new VendorPOTrendsResult();
            List<FiscalResolutionData> data = GetRCFiscalWeekData(fiscalYear.ToString(), resolutionCode, Region);
            VendorTrendRow trendRow = null;
            foreach (FiscalResolutionData d in data)
            {
                if (trendRow == null || d.FiscalWeek.Period != trendRow.Period)
                {
                    if (trendRow != null)
                    {
                        result.Trends.Add(trendRow);
                    }
                    trendRow = new VendorTrendRow(d.FiscalWeek.Period, d.TotalPO, d.SuspendedPO, Region);
                }
                else
                {
                    // Update totals
                    trendRow.TotalPOs += d.TotalPO;
                    trendRow.SuspendedPOs += d.SuspendedPO;
                    trendRow.calculate();
                }
            }

            return result;
        }

        #endregion

        #region Get PO Methods
        public static List<POData> GetRCPOs(DateTime StartDate, DateTime EndDate, string resolutionCode, string Region)
        {
            List<POData> list = new List<POData>();

            string query = "SELECT * FROM POData WHERE CloseDate BETWEEN @StartDate AND @EndDate AND ResolutionCode=@ResolutionCode AND Region=@Region";
            using (SqlConnection cn = new SqlConnection(Config.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@StartDate", string.Format("{0} 00:00:00", StartDate.ToShortDateString()));
                cmd.Parameters.AddWithValue("@EndDate", string.Format("{0} 23:59:59", EndDate.ToShortDateString()));
                cmd.Parameters.AddWithValue("@ResolutionCode", resolutionCode);
                cmd.Parameters.AddWithValue("@Region", Region);
                cn.Open();
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        POData d = GetAs<POData>(rdr);
                        d.init();
                        list.Add(d);
                    }
                }
            }

            return list;
        }

        public static List<POData> GetStorePOs(DateTime StartDate, DateTime EndDate, string Store, string Region)
        {
            List<POData> list = new List<POData>();

            string query = "SELECT * FROM POData WHERE CloseDate BETWEEN @StartDate AND @EndDate AND Store=@Store AND Region=@Region";
            using (SqlConnection cn = new SqlConnection(Config.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@StartDate", string.Format("{0} 00:00:00", StartDate.ToShortDateString()));
                cmd.Parameters.AddWithValue("@EndDate", string.Format("{0} 23:59:59", EndDate.ToShortDateString()));
                cmd.Parameters.AddWithValue("@Store", Store);
                cmd.Parameters.AddWithValue("@Region", Region);
                cn.Open();
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        //POData d = new POData(rdr);
                        POData d = Factory.GetAs<POData>(rdr);
                        d.init();
                        list.Add(d);
                    }
                }
            }

            return list;
        }

        public static List<POData> GetVendorPOs(DateTime StartDate, DateTime EndDate, string Vendor, string Region)
        {
            List<POData> list = new List<POData>();

            string query = "SELECT * FROM POData WHERE CloseDate BETWEEN @StartDate AND @EndDate AND Vendor=@Vendor AND Region = @Region";
            using (SqlConnection cn = new SqlConnection(Config.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@StartDate", string.Format("{0} 00:00:00", StartDate.ToShortDateString()));
                cmd.Parameters.AddWithValue("@EndDate", string.Format("{0} 23:59:59", EndDate.ToShortDateString()));
                cmd.Parameters.AddWithValue("@Vendor", Vendor);
                cmd.Parameters.AddWithValue("@Region", Region);
                cn.Open();
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        //POData d = new POData(rdr);
                        POData d = GetAs<POData>(rdr);
                        d.init();
                        list.Add(d);
                    }
                }
            }

            return list;
        }

        public static List<FiscalWeekData> GetFiscalWeekData(string year, string Region)
        {
            int _year = 0;
            try { _year = Convert.ToInt32(year); }
            catch { return null; }
            List<FiscalWeek> fiscalWeeks = Factory.GetFiscalWeeks(_year, false);
            List<FiscalWeekData> convertedFiscalData = new List<FiscalWeekData>();
            FiscalData fd = new FiscalData();
            fd.init();
            foreach (FiscalWeek w in fiscalWeeks)
            {
                if (w.Period == fd.CurrentWeek.Period && w.Week == fd.CurrentWeek.Week && w.Year == fd.CurrentWeek.Year)
                {
                    // Skip current FP since we don't have data loaded in.
                }
                else
                {
                    FiscalWeekData cw = new FiscalWeekData(w, Region);
                    convertedFiscalData.Add(cw);
                }
            }
            return convertedFiscalData;
        }

        public static List<FiscalWeekData> GetFiscalWeekData(int year, int period, string Region)
        {
            List<FiscalWeek> fiscalWeeks = Factory.GetFiscalWeeks(year, period, false);
            List<FiscalWeekData> convertedFiscalData = new List<FiscalWeekData>();
            FiscalData fd = new FiscalData();
            fd.init();
            foreach (FiscalWeek w in fiscalWeeks)
            {
                if (w.Period == fd.CurrentWeek.Period && w.Week == fd.CurrentWeek.Week && w.Year == fd.CurrentWeek.Year)
                {
                    // Skip current FW since we don't have data loaded in.
                }
                else
                {
                    if (Region == "ALL")
                    {
                        List<PORegion> Regions = GetRegions();
                        foreach (PORegion r in Regions)
                        {
                            FiscalWeekData cw = new FiscalWeekData(w, r.RegionID);
                            convertedFiscalData.Add(cw);
                        }
                    }
                    else
                    {
                        FiscalWeekData cw = new FiscalWeekData(w, Region);
                        convertedFiscalData.Add(cw);
                    }
                }
            }
            return convertedFiscalData;
        }

        public static List<GlobalFiscalPeriodData> GetGlobalData(string year)
        {
            int _year = 0;
            try { _year = Convert.ToInt32(year); }
            catch { return null; }
            List<FiscalPeriod> fiscalPeriods = Factory.GetFiscalPeriods(_year, false);
            List<GlobalFiscalPeriodData> convertedFiscalData = new List<GlobalFiscalPeriodData>();
            foreach (FiscalPeriod w in fiscalPeriods)
            {
                GlobalFiscalPeriodData cw = new GlobalFiscalPeriodData(w);
                convertedFiscalData.Add(cw);
            }
            return convertedFiscalData;
        }

        public static List<RegionFiscalPeriodData> GetRegionalFiscalPeriodData(GlobalFiscalPeriodData p)
        {
            List<RegionFiscalPeriodData> convertedFiscalData = new List<RegionFiscalPeriodData>();
            FiscalData fd = new FiscalData();
            fd.init();

            List<PORegion> Regions = GetRegions();

            // Loop through all regions and build region consolidate fiscal period data
            foreach (PORegion r in Regions)
            {
                RegionFiscalPeriodData cw = new RegionFiscalPeriodData(p, r.RegionID, r.Name);
                convertedFiscalData.Add(cw);                
            }
            return convertedFiscalData;
        }

        public static List<FiscalPeriodData> GetAllFiscalPeriodData(string year, string region)
        {
            int _year = 0;
            try { _year = Convert.ToInt32(year); }
            catch { return null; }
            List<FiscalPeriod> fiscalPeriods = Factory.GetFiscalPeriods(_year, true);
            List<FiscalPeriodData> convertedFiscalData = new List<FiscalPeriodData>();
            foreach (FiscalPeriod w in fiscalPeriods)
            {
                FiscalPeriodData cw = new FiscalPeriodData(w, region);
                convertedFiscalData.Add(cw);

            }
            return convertedFiscalData;
        }

        public static List<VendorFiscalData> GetVendorFiscalWeekData(string year, string vendor)
        {
            int _year = 0;
            try { _year = Convert.ToInt32(year); }
            catch { return null; }
            List<FiscalWeek> fiscalWeeks = Factory.GetFiscalWeeks(_year, true);
            List<VendorFiscalData> convertedFiscalData = new List<VendorFiscalData>();
            foreach (FiscalWeek w in fiscalWeeks)
            {
                VendorFiscalData cw = new VendorFiscalData(w, vendor);
                convertedFiscalData.Add(cw);
            }
            return convertedFiscalData;
        }

        public static List<StoreFiscalData> GetStoreFiscalWeekData(string year, string store, string region)
        {
            int _year = 0;
            try { _year = Convert.ToInt32(year); }
            catch { return null; }
            List<FiscalWeek> fiscalWeeks = Factory.GetFiscalWeeks(_year, true);
            List<StoreFiscalData> convertedFiscalData = new List<StoreFiscalData>();
            foreach (FiscalWeek w in fiscalWeeks)
            {
                StoreFiscalData cw = new StoreFiscalData(w, store, region);
                convertedFiscalData.Add(cw);
            }
            return convertedFiscalData;
        }

        public static POData GetPODataByID(int PONumber)
        {
            POData result = new POData();
            string query = "SELECT TOP 1 * FROM POData WHERE PONumber = @PONumber";
            using (SqlConnection cn = new SqlConnection(Config.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@PONumber", PONumber);
                cn.Open();
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                
                if (rdr.HasRows)
                {
                    rdr.Read();
                    result = GetAs<POData>(rdr);
                    result.init();
                }
            }
            return result;
        }

        public static List<FiscalResolutionData> GetRCFiscalWeekData(string year, string resolutionCode, string Region)
        {
            int _year = 0;
            try { _year = Convert.ToInt32(year); }
            catch { return null; }
            List<FiscalWeek> fiscalWeeks = Factory.GetFiscalWeeks(_year, true);
            List<FiscalResolutionData> convertedFiscalData = new List<FiscalResolutionData>();
            foreach (FiscalWeek w in fiscalWeeks)
            {
                FiscalResolutionData cw = new FiscalResolutionData(w, resolutionCode, Region);
                convertedFiscalData.Add(cw);
            }
            return convertedFiscalData;
        }

        #endregion

        #region Get Total Methods
        public static int[] GetYearDataRange()
        {
            int[] result = new int[2];
            result[0] = DateTime.Now.Year;
            result[1] = DateTime.Now.Year;

            string query = "SELECT [MIN] = MIN(FY), [MAX] = MAX(FY) FROM POTotals";
            using (SqlConnection cn = new SqlConnection(Config.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(query, cn);
                cn.Open();
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                if (rdr.HasRows)
                {
                    rdr.Read();

                    try
                    {
                        result[0] = Convert.ToInt32(rdr[0].ToString());
                        result[1] = Convert.ToInt32(rdr[1].ToString());
                    }
                    catch (Exception)
                    {
                        // Do Nothing
                    }                    
                }
            }

            return result;
        }

        public static List<ResolutionCodeData> GetResolutionCodeTotals(DateTime StartDate, DateTime EndDate, string Vendor, string Store, string Region)
        {
            List<ResolutionCodeData> list = new List<ResolutionCodeData>();           
            using (SqlConnection cn = new SqlConnection(Config.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("GetResolutionCodeTotals", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartDate", StartDate.ToShortDateString() + " 00:00:00");
                cmd.Parameters.AddWithValue("@EndDate", EndDate.ToShortDateString() + " 23:59:59");
                cmd.Parameters.AddWithValue("@Region", Region);

                if (!String.IsNullOrEmpty(Vendor)) cmd.Parameters.AddWithValue("@Vendor", Vendor);
                if (!String.IsNullOrEmpty(Store)) cmd.Parameters.AddWithValue("@Store", Store);

                cn.Open();
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        ResolutionCodeData d = Factory.GetAs<ResolutionCodeData>(rdr);                       
                        list.Add(d);
                    }
                }
            }

            return list;
        }
        #endregion

        #region Export Methods
        public static string ExportGlobalOverview(string year)
        {
            List<GlobalFiscalPeriodData> data = GetGlobalData(year);

            // Generate Excel
            // Check if file already exists
            string fileName = @"POReport_Global_FY" + year + "_Overview.xlsx";
            string folder = Config.DataFolder + @"Excel\";
            string URL = Config.DataURL.ToString() + @"Excel/" + fileName;

            if (File.Exists(folder + fileName))
            {
                File.Delete(folder + fileName);
            }
            else if (!Directory.Exists(folder))
            {
                try { Directory.CreateDirectory(folder); }
                catch { return ""; }
            }

            FileInfo newFile = new FileInfo(folder + fileName);
            ExcelPackage pck = new ExcelPackage(newFile);
            //Add the Content sheet
            var ws = pck.Workbook.Worksheets.Add("Global FP" + year + " Overview");

            // Set Title Style
            var titleStyle = pck.Workbook.Styles.CreateNamedStyle("titleStyle");   //This one is language dependent
            titleStyle.Style.Font.Bold = true;
            titleStyle.Style.Font.Size = 16;

            // Set Header Style
            var headerStyle = pck.Workbook.Styles.CreateNamedStyle("HeaderStyle");   //This one is language dependent
            headerStyle.Style.Font.Bold = true;
            headerStyle.Style.Font.Color.SetColor(Color.White);
            headerStyle.Style.Fill.PatternType = ExcelFillStyle.Solid;
            headerStyle.Style.Fill.BackgroundColor.SetColor(Color.DarkGreen);

            // Set SubHeader Style
            var subHeaderStyle = pck.Workbook.Styles.CreateNamedStyle("SubHeaderStyle");   //This one is language dependent
            subHeaderStyle.Style.Font.Bold = true;
            subHeaderStyle.Style.Font.Color.SetColor(Color.White);
            subHeaderStyle.Style.Fill.PatternType = ExcelFillStyle.Solid;
            subHeaderStyle.Style.Fill.BackgroundColor.SetColor(Color.DarkGray);

            // Set cell styles
            var cellStyle = pck.Workbook.Styles.CreateNamedStyle("CellStyle");   //This one is language dependent
            cellStyle.Style.Font.Bold = false;
            cellStyle.Style.Font.Color.SetColor(Color.Black);
            cellStyle.Style.Fill.PatternType = ExcelFillStyle.Solid;
            cellStyle.Style.Fill.BackgroundColor.SetColor(Color.LightYellow);

            ws.Cells["A1:J1"].Merge = true;
            ws.Cells["A1:J1"].StyleName = "titleStyle";

            ws.Cells["A1"].Value = "Global PO Report - FY" + year + " Overview";
            ws.Cells["A2"].Value = "Date Exported: " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
            // A3 == spacer

            int startRow = 4;
            int currentRow = startRow;        

            foreach (GlobalFiscalPeriodData fp in data)
            {
                // Header
                ws.Cells[currentRow, 1].Value = string.Format("FP{0} FY{1}", fp.Period, fp.Year);
                ws.Cells[currentRow, 1, currentRow, 4].StyleName = "HeaderStyle";
                currentRow++;

                ws.Cells[currentRow, 1].Value = "";
                ws.Cells[currentRow, 2].Value = "Total PO Count";
                ws.Cells[currentRow, 3].Value = "Suspended PO Count";
                ws.Cells[currentRow, 4].Value = "% of Total";
                ws.Cells[currentRow, 1, currentRow, 4].StyleName = "SubHeaderStyle";
                currentRow++;

                foreach (RegionFiscalPeriodData d in fp.Regions)
                {
                    ws.Cells[currentRow, 1].Value = d.RegionName;
                    ws.Cells[currentRow, 2].Value = d.TotalPO;
                    ws.Cells[currentRow, 3].Value = d.SuspendedPO;
                    ws.Cells[currentRow, 4].Formula = "C" + currentRow.ToString() + "/B" + currentRow.ToString() + "";
                    // Style newly added rows
                    ws.Cells[currentRow, 1, currentRow, 4].StyleName = "CellStyle";
                    ws.Cells[currentRow, 2, currentRow, 3].Style.Numberformat.Format = "#,##0";
                    ws.Cells[currentRow, 4].Style.Numberformat.Format = "#%";
                    currentRow++;
                }

                // Spacer
                currentRow++;
            }
            
            // Auto Fit Columns
            int max_cols = 10;
            for (int i = 1; i < max_cols; i++)
            {
                ws.Column(i).AutoFit();
            }

            // Write to disk
            pck.Save();

            if (File.Exists(folder + fileName))
            {
                return URL;
            }
            else
            {
                return "";
            }
        }

        public static string ExportFiscalOverview(string year, string region)
        {
            List<POReports.FiscalWeekData> data = GetFiscalWeekData(year, region);

            // Generate Excel
            // Check if file already exists
            string fileName = @"POReport_"+region+"_FY"+year+"_Overview.xlsx";
            string folder = Config.DataFolder + @"Excel\";
            string URL = Config.DataURL.ToString() + @"Excel/" + fileName;

            if (File.Exists(folder + fileName))
            {
                File.Delete(folder + fileName);
            }
            else if (!Directory.Exists(folder))
            {
                try { Directory.CreateDirectory(folder); }
                catch { return ""; }
            }

            FileInfo newFile = new FileInfo(folder + fileName);
            ExcelPackage pck = new ExcelPackage(newFile);
            //Add the Content sheet
            var ws = pck.Workbook.Worksheets.Add(region + " FP" + year + " Overview");

            // Set Title Style
            var titleStyle = pck.Workbook.Styles.CreateNamedStyle("titleStyle");   //This one is language dependent
            titleStyle.Style.Font.Bold = true;
            titleStyle.Style.Font.Size = 16;

            // Set Header Style
            var headerStyle = pck.Workbook.Styles.CreateNamedStyle("HeaderStyle");   //This one is language dependent
            headerStyle.Style.Font.Bold = true;
            headerStyle.Style.Font.Color.SetColor(Color.White);
            headerStyle.Style.Fill.PatternType = ExcelFillStyle.Solid;
            headerStyle.Style.Fill.BackgroundColor.SetColor(Color.DarkGreen);

            // Set cell styles
            var cellStyle = pck.Workbook.Styles.CreateNamedStyle("CellStyle");   //This one is language dependent
            cellStyle.Style.Font.Bold = false;
            cellStyle.Style.Font.Color.SetColor(Color.Black);
            cellStyle.Style.Fill.PatternType = ExcelFillStyle.Solid;
            cellStyle.Style.Fill.BackgroundColor.SetColor(Color.LightYellow);

            ws.Cells["A1:J1"].Merge = true;
            ws.Cells["A1:J1"].StyleName = "titleStyle";

            ws.Cells["A1"].Value = region + " PO Reports - FY"+year+" Overview";
            ws.Cells["A2"].Value = "Date Exported: " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
            // A3 == spacer

            // Header
            ws.Cells["A4"].Value = "Week";
            ws.Cells["B4"].Value = "Total PO Count";
            ws.Cells["C4"].Value = "Suspended PO Count";
            ws.Cells["D4"].Value = "% of Total";

            ws.Cells["A4:D4"].StyleName = "HeaderStyle";

            int startRow = 5;
            int currentRow = startRow;

            foreach (FiscalWeekData d in data)
            {
                ws.Cells["A" + currentRow.ToString()].Value = string.Format("FP{0}FW{1}", d.Period, d.Week);
                ws.Cells["B" + currentRow.ToString()].Value = d.TotalPO;
                ws.Cells["C" + currentRow.ToString()].Value = d.SuspendedPO;
                ws.Cells["D" + currentRow.ToString()].Formula = "C" + currentRow.ToString() + "/B" + currentRow.ToString()+"";

                currentRow++;
            }

            // Style newly added rows
            ws.Cells["A" + startRow.ToString() + ":D" + (currentRow - 1).ToString()].StyleName = "CellStyle";
            ws.Cells["D" + startRow.ToString() + ":D" + (currentRow - 1).ToString()].Style.Numberformat.Format = "#%";
            // Auto Fit Columns
            int max_cols = 10;
            for (int i = 1; i < max_cols; i++)
            {
                ws.Column(i).AutoFit();
            }

            // Write to disk
            pck.Save();

            if (File.Exists(folder + fileName))
            {
                return URL;
            }
            else
            {
                return "";
            }
        }

        public static string ExportFiscalWeekDetail(string dataSets, string type, string region)
        {
            List<FiscalWeekRowData> data = new List<FiscalWeekRowData>();

            // Populate Data
            string[] weeks = dataSets.Split(',');
            foreach (string p in weeks)
            {
                int fp = 0;
                int fy = 0;
                int fw = 0;
                string[] arr = p.Split('-');
                try { fy = Convert.ToInt32(arr[0]); }
                catch { }
                try { fp = Convert.ToInt32(arr[1]); }
                catch { }
                try { fw = Convert.ToInt32(arr[2]); }
                catch { }

                if (fy > 0 && fp > 0 && fw > 0)
                {
                    FiscalWeekRowData fd = new POReports.FiscalWeekRowData(fy, fp, fw, type, region);
                    fd.init();
                    data.Add(fd);
                }
            }

            string fileName = string.Format("POReport_{0}_{1}_Detail.xlsx", region, type);
            string title = string.Format("PO Reports {0} {1} Detail", region, type);
            if (data.Count == 0) return "";
            else if (data.Count == 1)
            {
                fileName = string.Format("POReport_{0}_FY{1}_FP{2}_Week_{4}_{3}_Detail.xlsx", region, data[0].Year, data[0].Period, type, data[0].Week);
                title = string.Format("PO Reports {0} FY{1} FP{2} Week {4} {3} Detail", region, data[0].Year, data[0].Period, type, data[0].Week);
            }
            else
            {
                fileName = string.Format("POReport_{0}_Week_Compare_Detail.xlsx", region, data[0].Year, data[0].Period, type);
                title = string.Format("PO Reports {0} Week Compare Detail", region, data[0].Year, data[0].Period, type);
            }

            // Generate Excel
            // Check if file already exists
            string folder = Config.DataFolder + @"Excel\";
            string URL = Config.DataURL.ToString() + @"Excel/" + fileName;

            if (File.Exists(folder + fileName))
            {
                File.Delete(folder + fileName);
            }
            else if (!Directory.Exists(folder))
            {
                try { Directory.CreateDirectory(folder); }
                catch { return ""; }
            }

            FileInfo newFile = new FileInfo(folder + fileName);
            ExcelPackage pck = new ExcelPackage(newFile);
            //Add the Content sheet
            var ws = pck.Workbook.Worksheets.Add(region + " Week Detail");

            // Set Title Style
            var titleStyle = pck.Workbook.Styles.CreateNamedStyle("titleStyle");   //This one is language dependent
            titleStyle.Style.Font.Bold = true;
            titleStyle.Style.Font.Size = 16;

            // Set Header Style
            var headerStyle = pck.Workbook.Styles.CreateNamedStyle("HeaderStyle");   //This one is language dependent
            headerStyle.Style.Font.Bold = true;
            headerStyle.Style.Font.Color.SetColor(Color.White);
            headerStyle.Style.Fill.PatternType = ExcelFillStyle.Solid;
            headerStyle.Style.Fill.BackgroundColor.SetColor(Color.DarkGreen);

            // Set cell styles
            var cellStyle = pck.Workbook.Styles.CreateNamedStyle("CellStyle");   //This one is language dependent
            cellStyle.Style.Font.Bold = false;
            cellStyle.Style.Font.Color.SetColor(Color.Black);
            cellStyle.Style.Fill.PatternType = ExcelFillStyle.Solid;
            cellStyle.Style.Fill.BackgroundColor.SetColor(Color.LightYellow);

            // Set Sub header styles
            var subHeaderStyle = pck.Workbook.Styles.CreateNamedStyle("SubHeaderStyle");   //This one is language dependent
            subHeaderStyle.Style.Font.Bold = false;
            subHeaderStyle.Style.Font.Color.SetColor(Color.Black);
            subHeaderStyle.Style.Font.Bold = true;
            subHeaderStyle.Style.Fill.PatternType = ExcelFillStyle.Solid;
            subHeaderStyle.Style.Fill.BackgroundColor.SetColor(Color.DarkKhaki);

            ws.Cells["A1:J1"].Merge = true;
            ws.Cells["A1:J1"].StyleName = "titleStyle";

            ws.Cells["A1"].Value = title;
            ws.Cells["A2"].Value = "Date Exported: " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
            // A3 == spacer

            // Header
            int startRow = 5;
            int currentRow = startRow;

            switch (type)
            {
                case "vendor":
                    ws.Cells["A4"].Value = "Vendor";
                    ws.Cells["B4"].Value = "Total PO Count";
                    ws.Cells["C4"].Value = "Suspended PO Count";
                    ws.Cells["D4"].Value = "% of Total";
                    ws.Cells["E4"].Value = "Suspended Contribution";
                    ws.Cells["A4:E4"].StyleName = "HeaderStyle";

                    foreach (FiscalWeekRowData fw in data)
                    {
                        ws.Cells[currentRow, 1].Value = string.Format("FW{2} FP{0} FY{1}", fw.Period, fw.Year, fw.Week);
                        ws.Cells[currentRow, 1, currentRow, 5].Merge = true;
                        ws.Cells[currentRow, 1, currentRow, 5].StyleName = "SubHeaderStyle";
                        currentRow++;

                        foreach (VendorFiscalData d in fw.VendorRows)
                        {
                            ws.Cells["A" + currentRow.ToString()].Value = d.Vendor;
                            ws.Cells["B" + currentRow.ToString()].Value = d.TotalPO;
                            ws.Cells["C" + currentRow.ToString()].Value = d.SuspendedPO;
                            ws.Cells["D" + currentRow.ToString()].Formula = "C" + currentRow.ToString() + "/B" + currentRow.ToString() + "";
                            ws.Cells["E" + currentRow.ToString()].Value = d.SuspendedContribution;

                            ws.Cells[currentRow, 1, currentRow, 5].StyleName = "CellStyle";
                            currentRow++;
                        }
                    }

                    // Style newly added rows
                    ws.Cells["D" + startRow.ToString() + ":D" + (currentRow - 1).ToString()].Style.Numberformat.Format = "#%";
                    ws.Cells["E" + startRow.ToString() + ":E" + (currentRow - 1).ToString()].Style.Numberformat.Format = "#%";
                    break;
                case "store":
                    ws.Cells["A4"].Value = "Store";
                    ws.Cells["B4"].Value = "Total PO Count";
                    ws.Cells["C4"].Value = "Suspended PO Count";
                    ws.Cells["D4"].Value = "% of Total";
                    ws.Cells["E4"].Value = "Suspended Contribution";
                    ws.Cells["A4:E4"].StyleName = "HeaderStyle";

                    foreach (FiscalWeekRowData fw in data)
                    {
                        ws.Cells[currentRow, 1].Value = string.Format("FW{2} FP{0} FY{1}", fw.Period, fw.Year, fw.Week);
                        ws.Cells[currentRow, 1, currentRow, 5].Merge = true;
                        ws.Cells[currentRow, 1, currentRow, 5].StyleName = "SubHeaderStyle";
                        currentRow++;
                        foreach (StoreFiscalData d in fw.StoreRows)
                        {
                            ws.Cells["A" + currentRow.ToString()].Value = d.Store;
                            ws.Cells["B" + currentRow.ToString()].Value = d.TotalPO;
                            ws.Cells["C" + currentRow.ToString()].Value = d.SuspendedPO;
                            ws.Cells["D" + currentRow.ToString()].Formula = "C" + currentRow.ToString() + "/B" + currentRow.ToString() + "";
                            ws.Cells["E" + currentRow.ToString()].Value = d.SuspendedContribution;

                            ws.Cells[currentRow, 1, currentRow, 5].StyleName = "CellStyle";
                            currentRow++;
                        }
                    }

                    // Style newly added rows
                    ws.Cells["D" + startRow.ToString() + ":D" + (currentRow - 1).ToString()].Style.Numberformat.Format = "#%";
                    ws.Cells["E" + startRow.ToString() + ":E" + (currentRow - 1).ToString()].Style.Numberformat.Format = "#%";
                    break;
                case "rc":
                    ws.Cells["A4"].Value = "Resolution Code";
                    ws.Cells["B4"].Value = "Suspended PO Count";
                    ws.Cells["C4"].Value = "Suspended Contribution";
                    ws.Cells["A4:C4"].StyleName = "HeaderStyle";

                    foreach (FiscalWeekRowData fw in data)
                    {
                        ws.Cells[currentRow, 1].Value = string.Format("FW{2} FP{0} FY{1}", fw.Period, fw.Year, fw.Week);
                        ws.Cells[currentRow, 1, currentRow, 5].Merge = true;
                        ws.Cells[currentRow, 1, currentRow, 5].StyleName = "SubHeaderStyle";
                        currentRow++;
                        foreach (FiscalResolutionData d in fw.ResolutionRows)
                        {
                            ws.Cells["A" + currentRow.ToString()].Value = d.ResolutionCode;
                            ws.Cells["B" + currentRow.ToString()].Value = d.SuspendedPO;
                            ws.Cells["C" + currentRow.ToString()].Value = d.SuspendedContribution;

                            ws.Cells[currentRow, 1, currentRow, 3].StyleName = "CellStyle";
                            currentRow++;
                        }
                    }

                    // Style newly added rows
                    ws.Cells["C" + startRow.ToString() + ":C" + (currentRow - 1).ToString()].Style.Numberformat.Format = "#%";
                    break;
                default: return "";
            }
            
            // Auto Fit Columns
            int max_cols = 10;
            for (int i = 1; i < max_cols; i++)
            {
                ws.Column(i).AutoFit();
            }

            // Write to disk
            pck.Save();

            if (File.Exists(folder + fileName))
            {
                return URL;
            }
            else
            {
                return "";
            }
        }

        public static string ExportFiscalPeriodDetail(string dataSets, string type, string region)
        {
            List<FiscalPeriodRowData> data = new List<FiscalPeriodRowData>();

            // Populate Data
            string[] periods = dataSets.Split(',');
            foreach (string p in periods)
            {
                int fp = 0;
                int fy = 0;
                string[] arr = p.Split('-');
                try { fy = Convert.ToInt32(arr[0]); }
                catch { }
                try { fp = Convert.ToInt32(arr[1]); }
                catch { }

                if (fy > 0 && fp > 0)
                {
                    FiscalPeriodRowData fd = new POReports.FiscalPeriodRowData(fy, fp, type, region);
                    fd.init();
                    data.Add(fd);
                }
            }

            string fileName = string.Format("POReport_{0}_{1}_Detail.xlsx", region, type);
            string title = string.Format("PO Reports {0} {1} Detail", region, type);
            if (data.Count == 0) return "";
            else if (data.Count == 1)
            {
                fileName = string.Format("POReport_{0}_FY{1}_FP{2}_{3}_Detail.xlsx", region, data[0].Year, data[0].Period, type);
                title = string.Format("PO Reports {0} FY{1} FP{2} {3} Detail", region, data[0].Year, data[0].Period, type);
            }
            else
            {
                fileName = string.Format("POReport_{0}_FP_Compare_Detail.xlsx", region, data[0].Year, data[0].Period, type);
                title = string.Format("PO Reports {0} FP Compare Detail", region, data[0].Year, data[0].Period, type);
            }

            // Generate Excel
            // Check if file already exists
            string folder = Config.DataFolder + @"Excel\";
            string URL = Config.DataURL.ToString() + @"Excel/" + fileName;

            if (File.Exists(folder + fileName))
            {
                File.Delete(folder + fileName);
            }
            else if (!Directory.Exists(folder))
            {
                try { Directory.CreateDirectory(folder); }
                catch { return ""; }
            }

            FileInfo newFile = new FileInfo(folder + fileName);
            ExcelPackage pck = new ExcelPackage(newFile);
            //Add the Content sheet
            var ws = pck.Workbook.Worksheets.Add(region + " FP Detail");

            // Set Title Style
            var titleStyle = pck.Workbook.Styles.CreateNamedStyle("titleStyle");   //This one is language dependent
            titleStyle.Style.Font.Bold = true;
            titleStyle.Style.Font.Size = 16;

            // Set Header Style
            var headerStyle = pck.Workbook.Styles.CreateNamedStyle("HeaderStyle");   //This one is language dependent
            headerStyle.Style.Font.Bold = true;
            headerStyle.Style.Font.Color.SetColor(Color.White);
            headerStyle.Style.Fill.PatternType = ExcelFillStyle.Solid;
            headerStyle.Style.Fill.BackgroundColor.SetColor(Color.DarkGreen);

            // Set cell styles
            var cellStyle = pck.Workbook.Styles.CreateNamedStyle("CellStyle");   //This one is language dependent
            cellStyle.Style.Font.Bold = false;
            cellStyle.Style.Font.Color.SetColor(Color.Black);
            cellStyle.Style.Fill.PatternType = ExcelFillStyle.Solid;
            cellStyle.Style.Fill.BackgroundColor.SetColor(Color.LightYellow);

            // Set Sub header styles
            var subHeaderStyle = pck.Workbook.Styles.CreateNamedStyle("SubHeaderStyle");   //This one is language dependent
            subHeaderStyle.Style.Font.Bold = false;
            subHeaderStyle.Style.Font.Color.SetColor(Color.Black);
            subHeaderStyle.Style.Font.Bold = true;
            subHeaderStyle.Style.Fill.PatternType = ExcelFillStyle.Solid;
            subHeaderStyle.Style.Fill.BackgroundColor.SetColor(Color.DarkKhaki);

            ws.Cells["A1:J1"].Merge = true;
            ws.Cells["A1:J1"].StyleName = "titleStyle";

            ws.Cells["A1"].Value = title;
            ws.Cells["A2"].Value = "Date Exported: " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
            // A3 == spacer

            // Header
            int startRow = 5;
            int currentRow = startRow;

            switch (type)
            {
                case "vendor":
                    ws.Cells["A4"].Value = "Vendor";
                    ws.Cells["B4"].Value = "Total PO Count";
                    ws.Cells["C4"].Value = "Suspended PO Count";
                    ws.Cells["D4"].Value = "% of Total";
                    ws.Cells["E4"].Value = "Suspended Contribution";
                    ws.Cells["A4:E4"].StyleName = "HeaderStyle";

                    foreach (FiscalPeriodRowData fp in data)
                    {
                        ws.Cells[currentRow, 1].Value = string.Format("FP{0} FY{1}", fp.Period, fp.Year);                       
                        ws.Cells[currentRow, 1, currentRow, 5].Merge = true;
                        ws.Cells[currentRow, 1, currentRow, 5].StyleName = "SubHeaderStyle";
                        currentRow++;
                        foreach (VendorFiscalData d in fp.VendorRows)
                        {
                            ws.Cells[currentRow, 1].Value = d.Vendor;
                            ws.Cells[currentRow, 2].Value = d.TotalPO;  //B
                            ws.Cells[currentRow, 3].Value = d.SuspendedPO;  //C
                            ws.Cells[currentRow, 4].Formula = "C" + currentRow.ToString() + "/B" + currentRow.ToString() + "";
                            ws.Cells[currentRow, 5].Value = d.SuspendedContribution;

                            ws.Cells[currentRow, 1, currentRow, 5].StyleName = "CellStyle";
                            currentRow++;
                        }
                    }

                    // Style newly added rows
                    ws.Cells["D" + startRow.ToString() + ":D" + (currentRow - 1).ToString()].Style.Numberformat.Format = "#%";
                    ws.Cells["E" + startRow.ToString() + ":E" + (currentRow - 1).ToString()].Style.Numberformat.Format = "#%";
                    break;
                case "store":
                    ws.Cells["A4"].Value = "Store";
                    ws.Cells["B4"].Value = "Total PO Count";
                    ws.Cells["C4"].Value = "Suspended PO Count";
                    ws.Cells["D4"].Value = "% of Total";
                    ws.Cells["E4"].Value = "Suspended Contribution";
                    ws.Cells["A4:E4"].StyleName = "HeaderStyle";
                    foreach (FiscalPeriodRowData fp in data)
                    {
                        ws.Cells[currentRow, 1].Value = string.Format("FP{0} FY{1}", fp.Period, fp.Year);
                        ws.Cells[currentRow, 1, currentRow, 5].Merge = true;
                        ws.Cells[currentRow, 1, currentRow, 5].StyleName = "SubHeaderStyle";
                        currentRow++;
                        foreach (StoreFiscalData d in fp.StoreRows)
                        {
                            ws.Cells["A" + currentRow.ToString()].Value = d.Store;
                            ws.Cells["B" + currentRow.ToString()].Value = d.TotalPO;
                            ws.Cells["C" + currentRow.ToString()].Value = d.SuspendedPO;
                            ws.Cells["D" + currentRow.ToString()].Formula = "C" + currentRow.ToString() + "/B" + currentRow.ToString() + "";
                            ws.Cells["E" + currentRow.ToString()].Value = d.SuspendedContribution;
                            ws.Cells[currentRow, 1, currentRow, 5].StyleName = "CellStyle";
                            currentRow++;
                        }
                    }

                    // Style newly added rows
                    ws.Cells["D" + startRow.ToString() + ":D" + (currentRow - 1).ToString()].Style.Numberformat.Format = "#%";
                    ws.Cells["E" + startRow.ToString() + ":E" + (currentRow - 1).ToString()].Style.Numberformat.Format = "#%";
                    break;
                case "rc":
                    ws.Cells["A4"].Value = "Resolution Code";
                    ws.Cells["B4"].Value = "Suspended PO Count";
                    ws.Cells["C4"].Value = "Suspended Contribution";
                    ws.Cells["A4:C4"].StyleName = "HeaderStyle";

                    foreach (FiscalPeriodRowData fp in data)
                    {
                        ws.Cells[currentRow, 1].Value = string.Format("FP{0} FY{1}", fp.Period, fp.Year);
                        ws.Cells[currentRow, 1, currentRow, 5].Merge = true;
                        ws.Cells[currentRow, 1, currentRow, 5].StyleName = "SubHeaderStyle";
                        currentRow++;
                        foreach (FiscalResolutionData d in fp.ResolutionRows)
                        {
                            ws.Cells["A" + currentRow.ToString()].Value = d.ResolutionCode;
                            ws.Cells["B" + currentRow.ToString()].Value = d.SuspendedPO;
                            ws.Cells["C" + currentRow.ToString()].Value = d.SuspendedContribution;
                            ws.Cells[currentRow, 1, currentRow, 3].StyleName = "CellStyle";
                            currentRow++;
                        }
                    }

                    // Style newly added rows
                    ws.Cells["C" + startRow.ToString() + ":C" + (currentRow - 1).ToString()].Style.Numberformat.Format = "#%";
                    break;
                default: return "";
            }

            // Auto Fit Columns
            int max_cols = 10;
            for (int i = 1; i < max_cols; i++)
            {
                ws.Column(i).AutoFit();
            }

            // Write to disk
            pck.Save();

            if (File.Exists(folder + fileName))
            {
                return URL;
            }
            else
            {
                return "";
            }
        }

        public static string ExportVendorDetail(string year, string period, string week, string vendor, string region)
        {
            List<POData> data = new List<POData>();
            int fy, fp, fw;
            try
            {
                fy = Convert.ToInt32(year);
                fp = Convert.ToInt32(period);
                fw = Convert.ToInt32(week);
            }
            catch { return ""; }

            if (fp > 0 && fw > 0 && fy > 0)
            {
                FiscalWeek _tmpWk = Factory.GetFiscalWeek(fy, fp, fw);
                data = GetVendorPOs(_tmpWk.StartDate, _tmpWk.EndDate, vendor, region);
            }
            else if (fp > 0 && fy > 0)
            {
                FiscalPeriod _tmpFp = Factory.GetFiscalPeriod(fy, fp);
                data = GetStorePOs(_tmpFp.StartDate, _tmpFp.EndDate, vendor, region);
            }
            else
            {
                return "";
            }

            // Generate Excel
            // Check if file already exists
            string cleanVendorName = vendor.Replace(" ", "_").Replace("'", "").Replace("/", "");
            string fileName = @"POReport_"+region+"_FY" + year + "FP" + period + "FW" + week + "_"+cleanVendorName+".xlsx";
            string folder = Config.DataFolder + @"Excel\";
            string URL = Config.DataURL.ToString() + @"Excel/" + fileName;

            if (File.Exists(folder + fileName))
            {
                File.Delete(folder + fileName);
            }
            else if (!Directory.Exists(folder))
            {
                try { Directory.CreateDirectory(folder); }
                catch { return ""; }
            }

            FileInfo newFile = new FileInfo(folder + fileName);
            ExcelPackage pck = new ExcelPackage(newFile);
            //Add the Content sheet
            var ws = pck.Workbook.Worksheets.Add(region + " FY" + year + "FP" + period + "FW" + week + " Detail");

            // Set Title Style
            var titleStyle = pck.Workbook.Styles.CreateNamedStyle("titleStyle");   //This one is language dependent
            titleStyle.Style.Font.Bold = true;
            titleStyle.Style.Font.Size = 16;

            // Set Header Style
            var headerStyle = pck.Workbook.Styles.CreateNamedStyle("HeaderStyle");   //This one is language dependent
            headerStyle.Style.Font.Bold = true;
            headerStyle.Style.Font.Color.SetColor(Color.White);
            headerStyle.Style.Fill.PatternType = ExcelFillStyle.Solid;
            headerStyle.Style.Fill.BackgroundColor.SetColor(Color.DarkGreen);

            // Set cell styles
            var cellStyle = pck.Workbook.Styles.CreateNamedStyle("CellStyle");   //This one is language dependent
            cellStyle.Style.Font.Bold = false;
            cellStyle.Style.Font.Color.SetColor(Color.Black);
            cellStyle.Style.Fill.PatternType = ExcelFillStyle.Solid;
            cellStyle.Style.Fill.BackgroundColor.SetColor(Color.LightYellow);

            ws.Cells["A1:J1"].Merge = true;
            ws.Cells["A1:J1"].StyleName = "titleStyle";

            ws.Cells["A1"].Value = region + " PO Reports - FY" + year + "FP" + period + "FW" + week + " " + vendor;
            ws.Cells["A2"].Value = "Date Exported: " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
            // A3 == spacer

            // Header
            ws.Cells["A4"].Value = "PO #";
            ws.Cells["B4"].Value = "Suspended";
            ws.Cells["C4"].Value = "Close Date";
            ws.Cells["D4"].Value = "Resolution Code";
            ws.Cells["E4"].Value = "PO Admin Notes";
            ws.Cells["F4"].Value = "Vendor Name";
            ws.Cells["G4"].Value = "Subteam";
            ws.Cells["H4"].Value = "Store";
            ws.Cells["I4"].Value = "Adjusted Cost";
            ws.Cells["J4"].Value = "Credit PO";
            ws.Cells["K4"].Value = "Vendor Type";
            ws.Cells["L4"].Value = "EInvoice Matched to PO";
            ws.Cells["M4"].Value = "PO Notes";
            ws.Cells["N4"].Value = "Closer";

            ws.Cells["A4:N4"].StyleName = "HeaderStyle";

            int startRow = 5;
            int currentRow = startRow;

            foreach (POData d in data)
            {
                ws.Cells["A" + currentRow.ToString()].Value = d.PONumber;
                ws.Cells["B" + currentRow.ToString()].Value = d.Suspended;
                ws.Cells["C" + currentRow.ToString()].Value = d.CloseDate;
                ws.Cells["D" + currentRow.ToString()].Value = d.ResolutionCode;
                ws.Cells["E" + currentRow.ToString()].Value = d.AdminNotes;
                ws.Cells["F" + currentRow.ToString()].Value = d.Vendor;
                ws.Cells["G" + currentRow.ToString()].Value = d.Subteam;
                ws.Cells["H" + currentRow.ToString()].Value = d.Store;
                ws.Cells["I" + currentRow.ToString()].Value = d.AdjustedCost;
                ws.Cells["J" + currentRow.ToString()].Value = d.CreditPO;
                ws.Cells["K" + currentRow.ToString()].Value = d.VendorType;
                ws.Cells["L" + currentRow.ToString()].Value = d.EInvoiceMatchedToPO;
                ws.Cells["M" + currentRow.ToString()].Value = d.PONotes;
                ws.Cells["N" + currentRow.ToString()].Value = d.ClosedBy;
                
                currentRow++;
            }

            // Style newly added rows
            ws.Cells["A" + startRow.ToString() + ":N" + (currentRow - 1).ToString()].StyleName = "CellStyle";
            // Auto Fit Columns
            int max_cols = 15;
            for (int i = 2; i < max_cols; i++)
            {
                ws.Column(i).AutoFit();
            }

            // Write to disk
            pck.Save();

            if (File.Exists(folder + fileName))
            {
                return URL;
            }
            else
            {
                return "";
            }
        }

        public static string ExportStoreDetail(string year, string period, string week, string store, string region)
        {
            List<POData> data = new List<POData>();
            int fy, fp, fw;
            try
            {
                fy = Convert.ToInt32(year);
                fp = Convert.ToInt32(period);
                fw = Convert.ToInt32(week);
            }
            catch { return ""; }

            if (fp > 0 && fw > 0 && fy > 0)
            {
                FiscalWeek _tmpWk = Factory.GetFiscalWeek(fy, fp, fw);
                data = GetStorePOs(_tmpWk.StartDate, _tmpWk.EndDate, store, region);
            }
            else if (fp > 0 && fy > 0)
            {
                FiscalPeriod _tmpFp = Factory.GetFiscalPeriod(fy, fp);
                data = GetStorePOs(_tmpFp.StartDate, _tmpFp.EndDate, store, region);                
            }
            else
            {
                return "";
            }

            // Generate Excel
            // Check if file already exists
            string cleanName = store.Replace(" ", "_").Replace("'", "").Replace("/", "");
            string fileName = @"POReport_"+region+"_FY" + year + "FP" + period + "FW" + week + "_" + cleanName + ".xlsx";
            string folder = Config.DataFolder + @"Excel\";
            string URL = Config.DataURL.ToString() + @"Excel/" + fileName;

            if (File.Exists(folder + fileName))
            {
                File.Delete(folder + fileName);
            }
            else if (!Directory.Exists(folder))
            {
                try { Directory.CreateDirectory(folder); }
                catch { return ""; }
            }

            FileInfo newFile = new FileInfo(folder + fileName);
            ExcelPackage pck = new ExcelPackage(newFile);
            //Add the Content sheet
            var ws = pck.Workbook.Worksheets.Add(region + " FY" + year + "FP" + period + "FW" + week + " Detail");

            // Set Title Style
            var titleStyle = pck.Workbook.Styles.CreateNamedStyle("titleStyle");   //This one is language dependent
            titleStyle.Style.Font.Bold = true;
            titleStyle.Style.Font.Size = 16;

            // Set Header Style
            var headerStyle = pck.Workbook.Styles.CreateNamedStyle("HeaderStyle");   //This one is language dependent
            headerStyle.Style.Font.Bold = true;
            headerStyle.Style.Font.Color.SetColor(Color.White);
            headerStyle.Style.Fill.PatternType = ExcelFillStyle.Solid;
            headerStyle.Style.Fill.BackgroundColor.SetColor(Color.DarkGreen);

            // Set cell styles
            var cellStyle = pck.Workbook.Styles.CreateNamedStyle("CellStyle");   //This one is language dependent
            cellStyle.Style.Font.Bold = false;
            cellStyle.Style.Font.Color.SetColor(Color.Black);
            cellStyle.Style.Fill.PatternType = ExcelFillStyle.Solid;
            cellStyle.Style.Fill.BackgroundColor.SetColor(Color.LightYellow);

            ws.Cells["A1:J1"].Merge = true;
            ws.Cells["A1:J1"].StyleName = "titleStyle";

            ws.Cells["A1"].Value = region + " PO Reports - FY" + year + "FP" + period + "FW" + week + " " + store;
            ws.Cells["A2"].Value = "Date Exported: " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
            // A3 == spacer

            // Header
            ws.Cells["A4"].Value = "PO #";
            ws.Cells["B4"].Value = "Suspended";
            ws.Cells["C4"].Value = "Close Date";
            ws.Cells["D4"].Value = "Resolution Code";
            ws.Cells["E4"].Value = "PO Admin Notes";
            ws.Cells["F4"].Value = "Vendor Name";
            ws.Cells["G4"].Value = "Subteam";
            ws.Cells["H4"].Value = "Store";
            ws.Cells["I4"].Value = "Adjusted Cost";
            ws.Cells["J4"].Value = "Credit PO";
            ws.Cells["K4"].Value = "Vendor Type";
            ws.Cells["L4"].Value = "EInvoice Matched to PO";
            ws.Cells["M4"].Value = "PO Notes";
            ws.Cells["N4"].Value = "Closer";
            ws.Cells["O4"].Value = "Region";

            ws.Cells["A4:O4"].StyleName = "HeaderStyle";

            int startRow = 5;
            int currentRow = startRow;

            foreach (POData d in data)
            {
                ws.Cells["A" + currentRow.ToString()].Value = d.PONumber;
                ws.Cells["B" + currentRow.ToString()].Value = d.Suspended;
                ws.Cells["C" + currentRow.ToString()].Value = d.CloseDate;
                ws.Cells["D" + currentRow.ToString()].Value = d.ResolutionCode;
                ws.Cells["E" + currentRow.ToString()].Value = d.AdminNotes;
                ws.Cells["F" + currentRow.ToString()].Value = d.Vendor;
                ws.Cells["G" + currentRow.ToString()].Value = d.Subteam;
                ws.Cells["H" + currentRow.ToString()].Value = d.Store;
                ws.Cells["I" + currentRow.ToString()].Value = d.AdjustedCost;
                ws.Cells["J" + currentRow.ToString()].Value = d.CreditPO;
                ws.Cells["K" + currentRow.ToString()].Value = d.VendorType;
                ws.Cells["L" + currentRow.ToString()].Value = d.EInvoiceMatchedToPO;
                ws.Cells["M" + currentRow.ToString()].Value = d.PONotes;
                ws.Cells["N" + currentRow.ToString()].Value = d.ClosedBy;
                ws.Cells["O" + currentRow.ToString()].Value = d.Region;

                currentRow++;
            }

            // Style newly added rows
            ws.Cells["A" + startRow.ToString() + ":O" + (currentRow - 1).ToString()].StyleName = "CellStyle";
            // Auto Fit Columns
            int max_cols = 15;
            for (int i = 2; i < max_cols; i++)
            {
                ws.Column(i).AutoFit();
            }

            // Write to disk
            pck.Save();

            if (File.Exists(folder + fileName))
            {
                return URL;
            }
            else
            {
                return "";
            }
        }

        public static string ExportRCDetail(string year, string period, string week, string rc, string region)
        {
            List<POData> data = new List<POData>();
            int fy, fp, fw;
            try
            {
                fy = Convert.ToInt32(year);
                fp = Convert.ToInt32(period);
                fw = Convert.ToInt32(week);
            }
            catch { return ""; }

            if (fp > 0 && fw > 0 && fy > 0)
            {
                FiscalWeek _tmpWk = Factory.GetFiscalWeek(fy, fp, fw);
                data = GetRCPOs(_tmpWk.StartDate, _tmpWk.EndDate, rc, region);
            }
            else if (fp > 0 && fy > 0)
            {
                FiscalPeriod _tmpFp = Factory.GetFiscalPeriod(fy, fp);
                data = GetStorePOs(_tmpFp.StartDate, _tmpFp.EndDate, rc, region);                
            }
            else
            {
                return "";
            }

            // Generate Excel
            // Check if file already exists
            string cleanName = rc.Replace(" ", "_").Replace("'", "").Replace("/", "");
            string fileName = @"POReport_"+region+"_FY" + year + "FP" + period + "FW" + week + "_" + cleanName + ".xlsx";
            string folder = Config.DataFolder + @"Excel\";
            string URL = Config.DataURL.ToString() + @"Excel/" + fileName;

            if (File.Exists(folder + fileName))
            {
                File.Delete(folder + fileName);
            }
            else if (!Directory.Exists(folder))
            {
                try { Directory.CreateDirectory(folder); }
                catch { return ""; }
            }

            FileInfo newFile = new FileInfo(folder + fileName);
            ExcelPackage pck = new ExcelPackage(newFile);
            //Add the Content sheet
            var ws = pck.Workbook.Worksheets.Add(region+" FY" + year + "FP" + period + "FW" + week + " Detail");

            // Set Title Style
            var titleStyle = pck.Workbook.Styles.CreateNamedStyle("titleStyle");   //This one is language dependent
            titleStyle.Style.Font.Bold = true;
            titleStyle.Style.Font.Size = 16;

            // Set Header Style
            var headerStyle = pck.Workbook.Styles.CreateNamedStyle("HeaderStyle");   //This one is language dependent
            headerStyle.Style.Font.Bold = true;
            headerStyle.Style.Font.Color.SetColor(Color.White);
            headerStyle.Style.Fill.PatternType = ExcelFillStyle.Solid;
            headerStyle.Style.Fill.BackgroundColor.SetColor(Color.DarkGreen);

            // Set cell styles
            var cellStyle = pck.Workbook.Styles.CreateNamedStyle("CellStyle");   //This one is language dependent
            cellStyle.Style.Font.Bold = false;
            cellStyle.Style.Font.Color.SetColor(Color.Black);
            cellStyle.Style.Fill.PatternType = ExcelFillStyle.Solid;
            cellStyle.Style.Fill.BackgroundColor.SetColor(Color.LightYellow);

            ws.Cells["A1:J1"].Merge = true;
            ws.Cells["A1:J1"].StyleName = "titleStyle";

            ws.Cells["A1"].Value = region + " PO Reports - FY" + year + "FP" + period + "FW" + week + " " + rc;
            ws.Cells["A2"].Value = "Date Exported: " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
            // A3 == spacer

            // Header
            ws.Cells["A4"].Value = "PO #";
            ws.Cells["B4"].Value = "Suspended";
            ws.Cells["C4"].Value = "Close Date";
            ws.Cells["D4"].Value = "Resolution Code";
            ws.Cells["E4"].Value = "PO Admin Notes";
            ws.Cells["F4"].Value = "Vendor Name";
            ws.Cells["G4"].Value = "Subteam";
            ws.Cells["H4"].Value = "Store";
            ws.Cells["I4"].Value = "Adjusted Cost";
            ws.Cells["J4"].Value = "Credit PO";
            ws.Cells["K4"].Value = "Vendor Type";
            ws.Cells["L4"].Value = "EInvoice Matched to PO";
            ws.Cells["M4"].Value = "PO Notes";
            ws.Cells["N4"].Value = "Closer";
            ws.Cells["O4"].Value = "Region";

            ws.Cells["A4:O4"].StyleName = "HeaderStyle";

            int startRow = 5;
            int currentRow = startRow;

            foreach (POData d in data)
            {
                ws.Cells["A" + currentRow.ToString()].Value = d.PONumber;
                ws.Cells["B" + currentRow.ToString()].Value = d.Suspended;
                ws.Cells["C" + currentRow.ToString()].Value = d.CloseDate;
                ws.Cells["D" + currentRow.ToString()].Value = d.ResolutionCode;
                ws.Cells["E" + currentRow.ToString()].Value = d.AdminNotes;
                ws.Cells["F" + currentRow.ToString()].Value = d.Vendor;
                ws.Cells["G" + currentRow.ToString()].Value = d.Subteam;
                ws.Cells["H" + currentRow.ToString()].Value = d.Store;
                ws.Cells["I" + currentRow.ToString()].Value = d.AdjustedCost;
                ws.Cells["J" + currentRow.ToString()].Value = d.CreditPO;
                ws.Cells["K" + currentRow.ToString()].Value = d.VendorType;
                ws.Cells["L" + currentRow.ToString()].Value = d.EInvoiceMatchedToPO;
                ws.Cells["M" + currentRow.ToString()].Value = d.PONotes;
                ws.Cells["N" + currentRow.ToString()].Value = d.ClosedBy;
                ws.Cells["O" + currentRow.ToString()].Value = d.Region;

                currentRow++;
            }

            // Style newly added rows
            ws.Cells["A" + startRow.ToString() + ":O" + (currentRow - 1).ToString()].StyleName = "CellStyle";
            // Auto Fit Columns
            int max_cols = 15;
            for (int i = 2; i < max_cols; i++)
            {
                ws.Column(i).AutoFit();
            }

            // Write to disk
            pck.Save();

            if (File.Exists(folder + fileName))
            {
                return URL;
            }
            else
            {
                return "";
            }
        }

        #endregion

        #region Class Reflection Methods
        // Dictionary to store cached properties
        private static IDictionary<string, PropertyInfo[]> propertiesCache = new Dictionary<string, PropertyInfo[]>();

        // Help with locking
        private static ReaderWriterLockSlim propertiesCacheLock = new ReaderWriterLockSlim();

        // Get an array of PropertyInfo for this type
        public static PropertyInfo[] GetCachedProperties<T>()
        {
            PropertyInfo[] props = new PropertyInfo[0];
            if (propertiesCacheLock.TryEnterUpgradeableReadLock(100))
            {
                try
                {
                    if (!propertiesCache.TryGetValue(typeof(T).FullName, out props))
                    {
                        props = typeof(T).GetProperties();
                        if (propertiesCacheLock.TryEnterWriteLock(100))
                        {
                            try
                            {
                                propertiesCache.Add(typeof(T).FullName, props);
                            }
                            finally
                            {
                                propertiesCacheLock.ExitWriteLock();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    string message = ex.Message.ToString();
                }
                finally
                {
                    propertiesCacheLock.ExitUpgradeableReadLock();
                }
                return props;
            }
            else
            {
                return typeof(T).GetProperties();
            }
        }

        public static List<string> GetColumnList(SqlDataReader reader)
        {
            List<string> columnList = new List<string>();
            System.Data.DataTable readerSchema = reader.GetSchemaTable();
            for (int i = 0; i < readerSchema.Rows.Count; i++)
            {
                columnList.Add(readerSchema.Rows[i]["ColumnName"].ToString());
            }
            return columnList;
        }

        public static T GetAs<T>(SqlDataReader reader)
        {
            // Create a new object
            T newObjectToReturn = Activator.CreateInstance<T>();

            // Get all the properties in our Object
            PropertyInfo[] props = GetCachedProperties<T>();

            List<string> columnList = GetColumnList(reader);

            // For each property get the data from the reader to the object
            for (int i = 0; i < props.Length; i++)
            {
                if (columnList.Contains(props[i].Name) && reader[props[i].Name] != DBNull.Value)
                {
                    typeof(T).InvokeMember(props[i].Name, BindingFlags.SetProperty, null, newObjectToReturn, new Object[] { reader[props[i].Name] });
                }
            }
            return newObjectToReturn;
        }

        public static bool ColumnExists(SqlDataReader reader, string columnName)
        {
            reader.GetSchemaTable().DefaultView.RowFilter = "ColumnName= '" + columnName + "'";
            return (reader.GetSchemaTable().DefaultView.Count > 0);
        }
        #endregion

        #region Fiscal Period/Week Functions

        public static FiscalPeriod GetFiscalPeriod(DateTime Date)
        {
            FiscalPeriod result = null;
            string query = "SELECT TOP 1 [Period] = FiscalPeriodNumber, [Year] = FiscalYear, Quarter, [StartDate] = FPStartDate, [EndDate] = FPEndDate FROM [FiscalPeriods] WHERE FPStartDate <= @Date AND FPEndDate >= @Date";
            using (SqlConnection cn = new SqlConnection(Config.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@Date", Date);
                cn.Open();
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (rdr.HasRows)
                {
                    rdr.Read();
                    result = GetAs<FiscalPeriod>(rdr);
                    result.init();
                }
            }
            return result;
        }

        public static List<FiscalPeriod> GetFiscalPeriods(int Year, bool includeFuture)
        {
            List<FiscalPeriod> result = new List<FiscalPeriod>();
            string query = "";

            if (includeFuture)
            {
                query = "SELECT [Period] = FiscalPeriodNumber, [Year] = FiscalYear, Quarter, [StartDate] = FPStartDate, [EndDate] = FPEndDate FROM [FiscalPeriods] WHERE FiscalYear = @Year ORDER BY FiscalYear, Quarter, FiscalPeriodNumber ASC";
            }
            else
            {
                query = "SELECT [Period] = FiscalPeriodNumber, [Year] = FiscalYear, Quarter, [StartDate] = FPStartDate, [EndDate] = FPEndDate FROM [FiscalPeriods] WHERE FiscalYear=@Year AND FPStartDate <= GETDATE() ORDER BY FiscalYear, Quarter, FiscalPeriodNumber ASC";
            }
            using (SqlConnection cn = new SqlConnection(Config.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@Year", Year);
                cn.Open();
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        FiscalPeriod d = GetAs<FiscalPeriod>(rdr);
                        d.init();
                        result.Add(d);
                    }
                }
            }
            return result;
        }

        public static FiscalPeriod GetFiscalPeriod(int Year, int Period)
        {
            FiscalPeriod result = null;
            string query = "SELECT TOP 1 [Period] = FiscalPeriodNumber, [Year] = FiscalYear, Quarter, [StartDate] = FPStartDate, [EndDate] = FPEndDate FROM [FiscalPeriods] WHERE FiscalPeriodNumber = @Period AND FiscalYear = @Year";
            using (SqlConnection cn = new SqlConnection(Config.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@Period", Period);
                cmd.Parameters.AddWithValue("@Year", Year);
                cn.Open();
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (rdr.HasRows)
                {
                    rdr.Read();
                    result = GetAs<FiscalPeriod>(rdr);
                    result.init();
                }
            }
            return result;
        }

        public static FiscalWeek GetFiscalWeek(int Year, int Period, int Week)
        {
            FiscalWeek result = null;
            string query = "SELECT TOP 1 Week, Period, Year, Description, StartDate, EndDate FROM FiscalWeek WHERE Year = @Year AND Period = @Period AND Week=@Week";
            using (SqlConnection cn = new SqlConnection(Config.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@Year", Year);
                cmd.Parameters.AddWithValue("@Period", Period);
                cmd.Parameters.AddWithValue("@Week", Week);
                cn.Open();
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (rdr.HasRows)
                {
                    rdr.Read();
                    result = GetAs<FiscalWeek>(rdr);
                }
            }
            return result;
        }

        public static FiscalWeek GetFiscalWeek(DateTime Date)
        {
            FiscalWeek result = null;
            string query = "SELECT TOP 1 Week, Period, Year, Description, StartDate, EndDate FROM FiscalWeek WHERE StartDate <= @Date AND EndDate >= @Date";
            using (SqlConnection cn = new SqlConnection(Config.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@Date", Date);
                cn.Open();
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (rdr.HasRows)
                {
                    rdr.Read();
                    result = GetAs<FiscalWeek>(rdr);
                }
            }
            return result;
        }


        public static List<FiscalWeek> GetFiscalWeeks(int year, bool includeFuture)
        {
            List<FiscalWeek> list = new List<FiscalWeek>();
            string query = "";
            if (includeFuture)
            {
                query = "SELECT Week, Period, Year, Description, StartDate, EndDate FROM FiscalWeek WHERE Year=@Year ORDER BY Year, Period, Week ASC";
            }
            else
            {
                query = "SELECT Week, Period, Year, Description, StartDate, EndDate FROM FiscalWeek WHERE Year=@Year AND StartDate <= GETDATE() ORDER BY Year, Period, Week ASC";
            }
            using (SqlConnection cn = new SqlConnection(Config.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@Year", year);
                cn.Open();
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        FiscalWeek u = GetAs<FiscalWeek>(rdr);
                        u.init();
                        list.Add(u);
                    }
                }
            }

            return list;
        }

        public static List<FiscalWeek> GetFiscalWeeks(int year, int period, bool includeFuture)
        {
            List<FiscalWeek> list = new List<FiscalWeek>();
            string query = "";
            if (includeFuture)
            {
                query = "SELECT * FROM FiscalWeek WHERE Year=@Year AND Period=@Period ORDER BY Year, Period, Week ASC";
            }
            else
            {
                query = "SELECT * FROM FiscalWeek WHERE Year=@Year AND Period=@Period AND StartDate <= GETDATE() ORDER BY Year, Period, Week ASC";
            }
            using (SqlConnection cn = new SqlConnection(Config.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@Year", year);
                cmd.Parameters.AddWithValue("@Period", period);
                cn.Open();
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        FiscalWeek u = GetAs<FiscalWeek>(rdr);
                        u.init();
                        list.Add(u);
                    }
                }
            }

            return list;
        }

        #endregion
    }

    public class TeamMember
    {
        public string SAMAccountName { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string StoreTLC { get; set; }
        public string Region { get; set; }
        public string UserID { get; set; }
        public string DisplayName { get; set; }
    }
}