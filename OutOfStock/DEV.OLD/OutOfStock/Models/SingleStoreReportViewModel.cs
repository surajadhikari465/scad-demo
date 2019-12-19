using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime;
using System.Security.Cryptography;
using System.Web.UI.WebControls.Expressions;
using OOS.Model;
using OOSCommon.DataContext;
using OOSCommon.Movement;
using OutOfStock.Classes;

namespace OutOfStock.Models
{

    public class StoresListDetails
    {
        public string UPC { get; set; }
        public string Stores { get; set; }

        public StoresListDetails() {}
        public StoresListDetails(string upc, string stores)
        {
            this.UPC = upc;
            this.Stores = stores;
        }
    }

    public class SingleStoreReportViewModel : ICustomReportViewModel
    {

//·        Subteam
//·        UPC
//·        Brand name
//·        Description
//·        Size
//·        UOM
//·        Vendor Key


//·        Order Code
//·        Avg. Daily Units 
//·        Cumulative Sales opportunity (should show high to Low $)
//·        Last Date of Sales
//·        # of days with sales (in last 12 weeks)

//·        Times Scanned (# of times a UPC was scanned during date range)
//·        Product Status
//·        Cost
//·        Margin
//·        Price
//·        Eff Price Type
//·        Category name
//·        Class Name


        private readonly List<ColumnDataModel> _columns = new List<ColumnDataModel>(); 
        //private IOOSEntitiesFactory entityFactory;
        


        public SingleStoreReportViewModel()
        {
            //ColumnDataModel.DataName is case sensitive and must match the PROPERTY Name of the column its mapped too.
            _columns.Add(new ColumnDataModel("Subteam", "PS_SUBTEAM"));
            _columns.Add(new ColumnDataModel("Upc", "UPC"));
            _columns.Add(new ColumnDataModel("Brand Name", "BRAND_NAME"));
            _columns.Add(new ColumnDataModel("Description", "LONG_DESCRIPTION"));
            _columns.Add(new ColumnDataModel("Size", "ITEM_SIZE"));
            _columns.Add(new ColumnDataModel("UOM", "ITEM_UOM"));
            _columns.Add(new ColumnDataModel("Vendor Key", "VENDOR_KEY"));
            _columns.Add(new ColumnDataModel("Order Code", "VIN"));
            _columns.Add(new ColumnDataModel("Avg Daily Units", "Avg_daily_Units"));
            _columns.Add(new ColumnDataModel("Cumulative Sales Opportunity", "sales"));
            _columns.Add(new ColumnDataModel("Last Date Sold", "LAST_DATE_SOLD"));
            _columns.Add(new ColumnDataModel("Days With Sales", "DAYS_WITH_SALES"));
            _columns.Add(new ColumnDataModel("Times Scanned", "times_scanned"));
            //_columns.Add(new ColumnDataModel("Stores", "StoresList"));
            _columns.Add(new ColumnDataModel("Product Status", "Product_Status"));
            _columns.Add(new ColumnDataModel("Cost", "cost"));
            
            _columns.Add(new ColumnDataModel("Margin", "margin"));
            _columns.Add(new ColumnDataModel("Price", "EFF_PRICE"));
            _columns.Add(new ColumnDataModel("Eff Price Type", "EFF_PRICETYPE"));
            _columns.Add(new ColumnDataModel("Category Name", "CATEGORY_NAME"));
            _columns.Add(new ColumnDataModel("Class Name", "CLASS_NAME"));

            
        }


        public List<ColumnDataModel> Columns
        {
            get { return _columns; }
        }


        public IEnumerable<ScanWithNoVimData> GetScanswithNoVimData(DateTime? start,
            DateTime? end,
            Dictionary<int, string> StoreInfo,
            Dictionary<string, string> TeamInfo,
            Dictionary<string, string> SubTeamInfo,
            DateTime todaysDate, ref IOOSEntitiesFactory dbfactory)
        {

            IEnumerable<ScanWithNoVimData> result;
            var dtStart = DateTime.Now;
            DateTime startDate = start.Value;
            DateTime endDate = end.Value;
            DateTime minDate = DateTime.MinValue;
            var storeIds = (StoreInfo != null) ? StoreInfo.Keys.ToList() : new List<int>();
            var TeamIds = (TeamInfo != null) ? TeamInfo.Keys.ToList() : new List<string>();
            var SubTeamIds = (SubTeamInfo != null) ? SubTeamInfo.Keys.ToList() : new List<string>();
            // make sure the date range is inclusive.
            endDate = new InclusiveTimeZoneEndDateSpecification(todaysDate, endDate).InclusiveEndDate;

            using (var dbContext = dbfactory.New())
            {
                var Headers = (from rh in dbContext.REPORT_HEADER
                    join st in dbContext.STORE on rh.STORE_ID equals st.ID
                    join stores in storeIds on st.ID equals stores
                    join r in dbContext.REGION on st.REGION_ID equals r.ID
                    // left join product status
                    where rh.OffsetCorrectedCreateDate >= startDate &&
                          rh.OffsetCorrectedCreateDate <= endDate &&
                          rh.EXCLUDE_FLAG == null
                    select new {rh.ID,st.PS_BU, st.STORE_NAME, st.STORE_ABBREVIATION, StoreId = st.ID});

                var ScansMissingVimData = (from h in Headers
                    join scans in dbContext.ScansMissingVimDatas on h.ID equals scans.Report_Header_Id                    
                    select
                        new ScanWithNoVimData()
                        {
                           ReportHeaderId = h.ID,
                            StoreId = h.StoreId,
                            BusinessUnit = h.PS_BU,
                            StoreAbbreviation = h.STORE_ABBREVIATION,
                            StoreName= h.STORE_NAME,
                            UPC = scans.UPC,
                            scanCount = scans.ScanCount
                        });

                var group = from s in ScansMissingVimData
                    group s by new {s.StoreAbbreviation, s.UPC}
                    into g
                    select new ScanWithNoVimData()
                    {
                        ReportHeaderId = g.Max(x => x.ReportHeaderId),
                        StoreId = g.Max(x => x.StoreId),
                        BusinessUnit = g.Max(x => x.BusinessUnit),
                        StoreAbbreviation = g.Key.StoreAbbreviation,
                        StoreName = g.Max(x => x.StoreName),
                        UPC = g.Key.UPC,
                        scanCount = g.Sum(x => x.scanCount)
                    };

                result = group.ToList();
            }

            return result;


        }



        public IEnumerable<ICustomReportExcelModel> RunQuery(DateTime? start,
                                                            DateTime? end,
                                                            Dictionary<int, string> StoreInfo,
                                                            Dictionary<string, string> TeamInfo,
                                                            Dictionary<string, string> SubTeamInfo,
                                                            DateTime todaysDate, ref IOOSEntitiesFactory dbfactory)
        {
            var dtStart = DateTime.Now;
            DateTime startDate = start.Value;
            DateTime endDate = end.Value;
            DateTime minDate = DateTime.MinValue;
            var storeIds = (StoreInfo != null) ? StoreInfo.Keys.ToList() : new List<int>();
            var TeamIds = (TeamInfo != null) ? TeamInfo.Keys.ToList() : new List<string>();
            var SubTeamIds = (SubTeamInfo != null) ? SubTeamInfo.Keys.ToList() : new List<string>();


            // make sure the date range is inclusive.
            endDate = new InclusiveTimeZoneEndDateSpecification(todaysDate, endDate).InclusiveEndDate;

//            var report = new OOSCustomReport(startDate, endDate);
            using (var dbContext = dbfactory.New())
            {


//                var itemdata = (from rh in dbContext.REPORT_HEADER
//                               join rd in dbContext.REPORT_DETAIL on rh.ID equals rd.REPORT_HEADER_ID
//                               join st in dbContext.STORE on rh.STORE_ID equals st.ID
//                               join stores in storeIds on st.ID equals stores
//                               join r in dbContext.REGION on st.REGION_ID equals r.ID
//                               // left join product status
//                               join ps in dbContext.ProductStatus on new {UPC = rd.UPC, REGION = r.REGION_ABBR} equals
//                                   new {UPC = ps.UPC, REGION = ps.Region} into joined
//                               from j in joined.DefaultIfEmpty()
//                               where rh.OffsetCorrectedCreateDate >= startDate &&
//                                     rh.OffsetCorrectedCreateDate <= endDate &&
//                                     (j.ExpirationDate == null || j.ExpirationDate > DateTime.Now) &&
//                                     (TeamIds.Count <= 0 || TeamIds.Contains(rd.PS_TEAM)) &&
//                                     (SubTeamIds.Count <= 0 || SubTeamIds.Contains(rd.PS_SUBTEAM)) 
//                                     && (rh.EXCLUDE_FLAG == null)

//                               orderby rd.ID

//                               select new CustomReportDTO()
//                                   {
//                                       ID = rd.ID,
//                                       STORE_NAME = st.STORE_NAME,
//                                       STORE_ABBREVIATION = st.STORE_ABBREVIATION,
//                                       UPC = rd.UPC,
//                                       PS_TEAM = rd.PS_TEAM,
//                                       PS_SUBTEAM = rd.PS_SUBTEAM,
//                                       CASE_SIZE = rd.CASE_SIZE,
//                                       EFF_COST = rd.EFF_COST,
//                                       EFF_PRICE = rd.EFF_PRICE,
//                                       EFF_PRICETYPE = rd.EFF_PRICETYPE,
//                                       MOVEMENT = rd.MOVEMENT,
//                                       NOTES = rd.NOTES,
//                                       VENDOR_KEY = rd.VENDOR_KEY,
//                                       VIN = rd.VIN,
//                                       BRAND = rd.BRAND,
//                                       BRAND_NAME = rd.BRAND_NAME,
//                                       LONG_DESCRIPTION = rd.LONG_DESCRIPTION,
//                                       ITEM_SIZE = rd.ITEM_SIZE,
//                                       ITEM_UOM = rd.ITEM_UOM,
//                                       CATEGORY_NAME = rd.CATEGORY_NAME,
//                                       CLASS_NAME = rd.CLASS_NAME,
//                                       ProductStatus = j.ProductStatus,
//                                       StartDate = j.StartDate,
//                                       timesScanned = 0,
//                                       LAST_DATE_SOLD =  rd.LASTDATEOFSALES,
//                                       DAYS_WITH_SALES = rd.DAYSOFMOVEMENT
//                                   }).ToList();

//  //              var UPCList = (from i in itemdata select i.UPC).Distinct().ToList();


////                var PSBUList = (from s in dbContext.STORE where storeIds.Contains(s.ID) select s.PS_BU).ToList();


//                // get comma separated list of stores for each UPC.
//                var StoresByUPClist = (from i in itemdata
//                                       select new {UPC = i.UPC, Store = i.STORE_ABBREVIATION}).ToList();


//                var StoresByUPCgrouped = (from a in StoresByUPClist
//                                        group a by a.UPC
//                                        into g
//                                        select new StoresListDetails()
//                                            {
//                                                UPC = g.FirstOrDefault().UPC,
//                                                Stores = string.Join(",", (g.Select(x => x.Store).Distinct()).ToArray())
//                                            }).ToList();
                
//                // get scan counts  for each UPC
//                var CountsByUPC = (from i in itemdata
//                                  group i by i.UPC
//                                      into g
//                                      select new { UPC = g.Key, ScanCount = g.Count() }).ToList();

//                // for each distinct UPC get the most recent record by ReportDetail.ID

//                var MostRecentIdByUPC = (from i in itemdata
//                                        group i by i.UPC
//                                            into g
//                                            select new
//                                                {
//                                                    UPC = g.Key,
//                                                    MaxId = g.Max(mi => mi.ID)
//                                                }).ToList();

//                // get avg values for each UPC
//                var AveragesByUPC = (from i in itemdata
//                                    group i by i.UPC
//                                        into g
//                                        select new
//                                        {
//                                            UPC = g.Key,
//                                            AvgEffCost = g.Where(p => p.EFF_COST.HasValue).Average(p => p.EFF_COST),
//                                            AvgEffPrice = g.Where(p => p.EFF_PRICE.HasValue).Average(p => p.EFF_PRICE),
//                                            AvgMovement = g.Where(p => p.MOVEMENT.HasValue).Average(p => p.MOVEMENT),
//                                        }).ToList();

//                // combine all the previous datasets together and create report data.
//                var entries = (from r in MostRecentIdByUPC
//                              join i in itemdata on r.MaxId equals i.ID                              
//                              join cnt in CountsByUPC on r.UPC equals cnt.UPC
//                              join a in AveragesByUPC on r.UPC equals a.UPC
//                              join s in StoresByUPCgrouped on r.UPC equals s.UPC
//                              select new CustomReportEntry() {
//                                      UPC=r.UPC,
//                                      PS_TEAM = i.PS_TEAM,
//                                      PS_SUBTEAM = i.PS_SUBTEAM,
//                                      CASE_SIZE = (i.CASE_SIZE.HasValue ? i.CASE_SIZE.Value : 0),
//                                      EFF_COST = (a.AvgEffCost.HasValue ? a.AvgEffCost.Value : 0),
//                                      EFF_PRICE = (a.AvgEffPrice.HasValue ? a.AvgEffPrice.Value : 0),
//                                      EFF_PRICETYPE = i.EFF_PRICETYPE,
//                                      MOVEMENT = (a.AvgMovement.HasValue ? a.AvgMovement.Value : 0),
//                                      NOTES = i.NOTES,
//                                      VENDOR_KEY = i.VENDOR_KEY,
//                                      VIN=i.VIN,
//                                      BRAND =i.BRAND,
//                                      BRAND_NAME = i.BRAND_NAME,
//                                      LONG_DESCRIPTION = i.LONG_DESCRIPTION,
//                                      ITEM_SIZE = i.ITEM_SIZE,
//                                      ITEM_UOM = i.ITEM_UOM,
//                                      CATEGORY_NAME = i.CATEGORY_NAME,
//                                      CLASS_NAME = i.CLASS_NAME,
//                                      ProductStatus = i.ProductStatus,
//                                      StartDate = i.StartDate,
//                                      timesScanned = cnt.ScanCount,
//                                      StoresList = s.Stores,
//                                      LAST_DATE_SOLD = i.LAST_DATE_SOLD,
//                                      DAYS_WITH_SALES = i.DAYS_WITH_SALES,
//                              }).ToList();

//                entries.ToList().ForEach(report.Add);

             //var days  = Math.Abs(endDate.Subtract(startDate).Days);
             //   if (days == 0) days = 1;

                // turn List<String> into comma separated strings or DB NULL if empty.
                string SubTeams = SubTeamIds.Any() ? string.Format("'{0}'", string.Join<string>(",", SubTeamIds)) : "NULL";
                string Teams = TeamIds.Any() ? string.Format("'{0}'", string.Join<string>(",", TeamIds)) : "NULL";

                IEnumerable<ICustomReportExcelModel> data = null;
                using (var datacontext = new OOSEntities(OutOfStock.MvcApplication.oosEFConnectionString))
                {

                    // find the region we are working with.
                    var regionabbr = (from s in storeIds
                                      join st in dbContext.STORE on s equals st.ID
                                      join r in dbContext.REGION on st.REGION_ID equals r.ID
                                      select r.REGION_ABBR).FirstOrDefault();

                    var sql = @"EXEC dbo.SingleStoreReport @Region = '{0}', -- varchar(5)
                                @startdate = '{1}', -- datetime
                                @enddate = '{2}', -- datetime
                                @StoreId = {3}, -- int
                                @TeamIds = {4}, -- varchar(max)
                                @SubTeamIds = {5}, -- varchar(max)
                                @Debug = 0, -- int,
	                            @testUPC = null";

                    var query = string.Format(sql, regionabbr, startDate, endDate, storeIds.FirstOrDefault(), Teams, SubTeams);
                    
                     data = datacontext.ExecuteStoreQuery<SingleStoreReportExcelModel>(query).ToList();

                 
                }
                return data;


            //    return (from item in report.Entries()
            //            let timesScanned = item.timesScanned
            //            let cost = decimal.Round((item.CASE_SIZE == 0M) ? 0 : item.MOVEMENT * (item.EFF_COST / item.CASE_SIZE), 2)
            //            let margin = (item.EFF_PRICE == 0M || item.CASE_SIZE == 0M) ? 0 : Math.Round(((item.EFF_PRICE - (item.EFF_COST / item.CASE_SIZE)) / item.EFF_PRICE) * 100)
            //            let avgUnitOpportunity = decimal.Round(item.MOVEMENT, 2)
            //            let avgSalesOpportunity = decimal.Round(item.MOVEMENT * item.EFF_PRICE, 2)
            //            let sales = decimal.Round((item.MOVEMENT * item.EFF_PRICE * days), 2)
            //            let productStatus = item.ProductStatus
            //            select new SingleStoreReportExcelModel(item.PS_SUBTEAM, item.UPC, item.BRAND, item.BRAND_NAME, item.LONG_DESCRIPTION, item.ITEM_SIZE, item.ITEM_UOM, item.VENDOR_KEY, item.VIN, sales, timesScanned, item.NOTES, cost, margin, item.EFF_PRICE, item.EFF_PRICETYPE, item.CATEGORY_NAME, item.CLASS_NAME, avgUnitOpportunity, avgSalesOpportunity, productStatus, item.StoresList, item.LAST_DATE_SOLD, item.DAYS_WITH_SALES)).ToList().OrderByDescending(x => x.sales);

            }


               

        }
    }
}