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
            
            //endDate = new InclusiveTimeZoneEndDateSpecification(todaysDate, endDate).InclusiveEndDate;

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



        public IEnumerable<ICustomReportExcelModel> RunQuery(RunQueryParameters parameters, ref IOOSEntitiesFactory dbfactory)
        {
            var startDate = parameters.Start.Value;
            var endDate = parameters.End.Value;
            var storeIds = parameters.StoreInfo?.Keys.ToList() ?? new List<int>();
            var teamIds = parameters.TeamInfo?.Keys.ToList() ?? new List<string>();
            var subTeamIds = parameters.SubTeamInfo?.Keys.ToList() ?? new List<string>();


            
            //endDate = new InclusiveTimeZoneEndDateSpecification(todaysDate, endDate).InclusiveEndDate;

//            var report = new OOSCustomReport(startDate, endDate);
            using (var dbContext = dbfactory.New())
            {




                // turn List<String> into comma separated strings or DB NULL if empty.
                var subTeams = subTeamIds.Any() ? $"'{string.Join<string>(",", subTeamIds)}'" : "NULL";
                var teams = teamIds.Any() ? $"'{string.Join<string>(",", teamIds)}'" : "NULL";

                IEnumerable<ICustomReportExcelModel> data;
                using (var datacontext = new OOSEntities(MvcApplication.oosEFConnectionString))
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

                    var query = string.Format(sql, regionabbr, startDate, endDate, storeIds.FirstOrDefault(), teams, subTeams);
                    
                     data = datacontext.ExecuteStoreQuery<SingleStoreReportExcelModel>(query).ToList();

                 
                }
                return data;
          

            }


               

        }
    }
}