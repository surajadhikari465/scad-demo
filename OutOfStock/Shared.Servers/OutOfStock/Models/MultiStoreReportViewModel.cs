using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using OOS.Model;
using OOSCommon.DataContext;
using OutOfStock.Classes;

namespace OutOfStock.Models
{
    public class MultiStoreReportViewModel : ICustomReportViewModel
    {

        private readonly List<ColumnDataModel> _columns = new List<ColumnDataModel>();
       // private IOOSEntitiesFactory entityFactory;


        public MultiStoreReportViewModel()
        {
            //ColumnDataModel.DataName is case sensitive and must match the PROPERTY Name of the column its mapped too.
            _columns.Add(new ColumnDataModel("Stores", "StoresList"));
            _columns.Add(new ColumnDataModel("Subteam", "PS_SUBTEAM"));
            _columns.Add(new ColumnDataModel("Upc", "UPC"));
            _columns.Add(new ColumnDataModel("Brand Name", "BRAND_NAME"));
            _columns.Add(new ColumnDataModel("Description", "LONG_DESCRIPTION"));
            _columns.Add(new ColumnDataModel("Size", "ITEM_SIZE"));
            _columns.Add(new ColumnDataModel("UOM", "ITEM_UOM"));
            _columns.Add(new ColumnDataModel("Order Code", "VIN"));
            _columns.Add(new ColumnDataModel("Vendor Key", "VENDOR_KEY"));
            _columns.Add(new ColumnDataModel("Cumulative Sales Opportunity", "sales"));
            //--_columns.Add(new ColumnDataModel("Last Date Sold", "LAST_DATE_SOLD"));
            _columns.Add(new ColumnDataModel("Times Scanned", "times_scanned"));
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
            Dictionary<int, string> storeInfo,
            Dictionary<string, string> teamInfo,
            Dictionary<string, string> subTeamInfo,
            DateTime todaysDate,
            ref IOOSEntitiesFactory dbfactory)
        {

            IEnumerable<ScanWithNoVimData> result;
            //var dtStart = DateTime.Now;
            var startDate = start.Value;
            var endDate = end.Value;
            //var minDate = DateTime.MinValue;
            var storeIds = storeInfo?.Keys.ToList() ?? new List<int>();
           // var teamIds = (teamInfo != null) ? teamInfo.Keys.ToList() : new List<string>();
           // var subTeamIds = (subTeamInfo != null) ? subTeamInfo.Keys.ToList() : new List<string>();
            
            //endDate = new InclusiveTimeZoneEndDateSpecification(todaysDate, endDate).InclusiveEndDate;

            using (var dbContext = dbfactory.New())
            {
                var headers = (from rh in dbContext.REPORT_HEADER
                               join st in dbContext.STORE on rh.STORE_ID equals st.ID
                               //join stores in storeIds on st.ID equals stores
                               join r in dbContext.REGION on st.REGION_ID equals r.ID
                               // left join product status
                               where rh.OffsetCorrectedCreateDate >= startDate &&
                                     rh.OffsetCorrectedCreateDate <= endDate &&
                                     rh.EXCLUDE_FLAG == null &&
                                     storeIds.Contains(rh.STORE_ID)
                               select new { rh.ID,  StoreId = rh.STORE_ID });

                var stores = from s in dbContext.STORE select new {s.ID ,s.PS_BU, s.STORE_NAME, s.STORE_ABBREVIATION};



                var headersWithStore = (from s in stores
                    join h in headers on s.ID equals h.StoreId
                    select new
                    {
                        h.ID,
                        s.PS_BU,
                        s.STORE_ABBREVIATION,
                        s.STORE_NAME, 
                        StoreId = s.ID
                    });


                var scansMissingVimData = (from h in headersWithStore
                    join scans in dbContext.ScansMissingVimDatas on h.ID equals
                        scans.Report_Header_Id
                    select
                        new ScanWithNoVimData()
                        {
                            ReportHeaderId = h.ID,
                            StoreId = h.StoreId,
                            BusinessUnit = h.PS_BU,
                            StoreAbbreviation = h.STORE_ABBREVIATION,
                            StoreName = h.STORE_NAME,
                            UPC = scans.UPC,
                            scanCount = scans.ScanCount
                        });

                var group = from s in scansMissingVimData
                            group s by new { s.StoreAbbreviation, s.UPC }
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

        

        public IEnumerable<ICustomReportExcelModel> RunQuery(RunQueryParameters parameters, ref IOOSEntitiesFactory dbfactory )
        {
            DateTime startDate = parameters.Start.Value;
            DateTime endDate = parameters.End.Value;
            var storeIds = parameters.StoreInfo?.Keys.ToList() ?? new List<int>();
            var teamIds = parameters.TeamInfo?.Keys.ToList() ?? new List<string>();
            var subTeamIds = parameters.SubTeamInfo?.Keys.ToList() ?? new List<string>();
            var groupByUpc = parameters.GroupByUpc;


            using (var dbContext = dbfactory.New())
            {

                var subTeams = subTeamIds.Any() ? $"'{string.Join<string>(",", subTeamIds)}'" : "NULL";
                var teams = teamIds.Any() ? $"'{string.Join<string>(",", teamIds)}'" : "NULL";
                var storearray = storeIds.Select(i => i.ToString(CultureInfo.InvariantCulture)).ToArray();
                var stores = storeIds.Any() ? $"'{string.Join<string>(",", storearray)}'" : "NULL";

                IEnumerable<ICustomReportExcelModel> data;
                using (var datacontext = new OOSEntities(MvcApplication.oosEFConnectionString))
                {

                    // find the region we are working with.
                    var regionabbr = (from s in storeIds
                        join st in dbContext.STORE on s equals st.ID
                        join r in dbContext.REGION on st.REGION_ID equals r.ID
                        select r.REGION_ABBR).FirstOrDefault();

                    const string sql = @"EXEC dbo.MultiStoreReport @Region = '{0}', -- varchar(5)
                                @startdate = '{1}', -- datetime
                                @enddate = '{2}', -- datetime
                                @StoreIds = {3}, -- varchar(max)
                                @TeamIds = {4}, -- varchar(max)
                                @SubTeamIds = {5}, -- varchar(max)
                                @Debug = 0, -- int,
	                            @testUPC = null,
                                @GroupByUpc = {6}";

                    

                    var query = string.Format(sql, regionabbr, startDate, endDate, stores, teams, subTeams,groupByUpc);
                    data = datacontext.ExecuteStoreQuery<MultiStoreReportExcelModel>(query).ToList();


                }

                return data;

            }
        }
    }
}