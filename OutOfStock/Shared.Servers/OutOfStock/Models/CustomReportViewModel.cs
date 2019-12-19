using System;
using System.Collections.Generic;
using System.Linq;
using OOS.Model;
using OOSCommon;
using OutOfStock.Classes;
using OutOfStock.Service;

namespace OutOfStock.Models
{
    public class CustomReportViewModel : ICustomReportViewModel
    {
        private IOOSLog logger;
        private CustomReportService customReportService;

        public string PS_SUBTEAM { get; set; }
        public string UPC { get; set; }
        public string BRAND { get; set; }
        public string BRAND_NAME { get; set; }
        public string LONG_DESCRIPTION { get; set; }
        public string ITEM_SIZE { get; set; }
        public string ITEM_UOM { get; set; }
        public string VENDOR_KEY { get; set; }
        public string VIN { get; set; }
        public decimal sales { get; set; }
        public int times_scanned { get; set; }
        public string StoresList { get; set; }
        public string NOTES { get; set; }
        public decimal cost { get; set; }
        public decimal margin { get; set; }
        public decimal? EFF_PRICE { get; set; }
        public string EFF_PRICETYPE { get; set; }
        public string CATEGORY_NAME { get; set; }
        public string CLASS_NAME { get; set; }
        public decimal Avg_daily_Units { get; set; }
        public decimal Avg_Mov_Sales { get; set; }
        public string Product_Status { get; set; }
        

        public CustomReportViewModel(
            string PS_SUBTEAM,
            string UPC,
            string BRAND,
            string BRAND_NAME,
            string LONG_DESCRIPTION,
            string ITEM_SIZE,
            string ITEM_UOM,
            string VENDOR_KEY,
            string VIN,
            decimal sales,
            int times_scanned,
            string NOTES,
            decimal cost,
            decimal margin,
            decimal? EFF_PRICE,
            string EFF_PRICETYPE,
            string CATEGORY_NAME,
            string CLASS_NAME,
            decimal Avg_daily_Units,
            decimal Avg_Mov_Sales,
            string Product_Status,
            string storesList
        )
        {
            this.PS_SUBTEAM = PS_SUBTEAM;
            this.UPC = UPC;
            this.BRAND = BRAND;
            this.BRAND_NAME = BRAND_NAME;
            this.LONG_DESCRIPTION = LONG_DESCRIPTION;
            this.ITEM_SIZE = ITEM_SIZE;
            this.ITEM_UOM = ITEM_UOM;
            this.VENDOR_KEY = VENDOR_KEY;
            this.VIN = VIN;
            this.sales = sales;
            this.times_scanned = times_scanned;
            this.NOTES = NOTES;
            this.cost = cost;
            this.margin = margin;
            this.EFF_PRICE = EFF_PRICE;
            this.EFF_PRICETYPE = EFF_PRICETYPE;
            this.CATEGORY_NAME = CATEGORY_NAME;
            this.CLASS_NAME = CLASS_NAME;
            this.Avg_daily_Units = Avg_daily_Units;
            this.Avg_Mov_Sales = Avg_Mov_Sales;
            this.Product_Status = Product_Status;
            this.StoresList = storesList;
        }

        public CustomReportViewModel(ILogService logService, CustomReportService customReportService)
        {
            logger = logService.GetLogger();
            this.customReportService = customReportService;
        }

        public List<ColumnDataModel> Columns { get; private set; }

        public IEnumerable<ScanWithNoVimData> GetScanswithNoVimData(DateTime? start, DateTime? end, Dictionary<int, string> StoreInfo, Dictionary<string, string> TeamInfo,
            Dictionary<string, string> SubTeamInfo, DateTime todaysDate, ref IOOSEntitiesFactory dbfactory)
        {
            return null;
        }

        public IEnumerable<ICustomReportExcelModel> RunQuery(RunQueryParameters parameters, ref IOOSEntitiesFactory dbfactory)
        {
            //logger.Trace("RunQuery() Enter");
            //var dtStart = DateTime.Now;

            //DateTime startDate = start.Value;
            //DateTime endDate = end.Value;
            //List<int> storeIds = (dicStore != null) ? dicStore.Keys.ToList() : new List<int>();
            //List<string> teams = (dicTeam != null) ? dicTeam.Keys.ToList() : new List<string>();
            //List<string> subteams = (dicSubTeam != null) ? dicSubTeam.Keys.ToList() : new List<string>();

            //endDate = new InclusiveTimeZoneEndDateSpecification(todaysDate, endDate).InclusiveEndDate;
            //var results = customReportService.CustomReportFor(startDate, endDate, storeIds, teams, subteams);

            //TimeSpan ts = DateTime.Now.Subtract(dtStart);
            //logger.Trace("RunQuery() Exit, Elapsed=" + ts);
            //return results;
            return null;
        }

    }
}