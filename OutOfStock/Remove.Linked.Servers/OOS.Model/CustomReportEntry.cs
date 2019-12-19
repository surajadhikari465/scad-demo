using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOS.Model
{
    public class CustomReportEntry
    {
        public string UPC { get;  set; }
        public string PS_TEAM { get;  set; }
        public string PS_SUBTEAM { get;  set; }
        public decimal CASE_SIZE { get;  set; }
        public decimal EFF_COST { get;  set; }
        public decimal EFF_PRICE { get;  set; }
        public string EFF_PRICETYPE { get;  set; }
        public decimal MOVEMENT { get;  set; }
        public string NOTES { get;  set; }
        public string VENDOR_KEY { get;  set; }
        public string VIN { get;  set; }
        public string BRAND { get;  set; }
        public string BRAND_NAME { get;  set; }
        public string LONG_DESCRIPTION { get;  set; }
        public string ITEM_SIZE { get;  set; }
        public string ITEM_UOM { get;  set; }
        public string CATEGORY_NAME { get;  set; }
        public string CLASS_NAME { get;  set; }
        public string ProductStatus { get;  set; }
        public DateTime? StartDate { get;  set; }
        public int timesScanned { get;  set; }
        public string StoresList { get;  set; }
        public DateTime? LAST_DATE_SOLD { get; set; }
        public int? DAYS_WITH_SALES { get; set; }
        public decimal? CumulativeSales { get; set; }
        public decimal? ActualSales { get; set; }


        public CustomReportEntry()
        {
        }

        public CustomReportEntry(
            string upc,
            string psTeam,
            string psSubteam,
            decimal caseSize,
            decimal effCost,
            decimal effPrice,
            string effPriceType,
            decimal movement,
            string notes,
            string vendorKey,
            string vin,
            string brand,
            string brandName,
            string description,
            string itemSize,
            string unitOfMeasure,
            string category,
            string @class,
            string productStatus,
            DateTime? startDate,
            int timesScanned,
            string StoresList,
            DateTime? LastDateSold, 
            int? DaysWithSales,
            decimal? CumulativeSales,
            decimal? ActualSales
            )
        {
            UPC = upc;
            PS_TEAM = psTeam;
            PS_SUBTEAM = psSubteam;
            CASE_SIZE = caseSize;
            EFF_COST = effCost;
            EFF_PRICE = effPrice;
            EFF_PRICETYPE = effPriceType;
            MOVEMENT = movement;
            NOTES = notes;
            VENDOR_KEY = vendorKey;
            VIN = vin;
            BRAND = brand;
            BRAND_NAME = brandName;
            LONG_DESCRIPTION = description;
            ITEM_SIZE = itemSize;
            ITEM_UOM = unitOfMeasure;
            CATEGORY_NAME = category;
            CLASS_NAME = @class;
            ProductStatus = productStatus;
            StartDate = startDate;
            this.timesScanned = timesScanned;
            this.StoresList = StoresList;
            this.DAYS_WITH_SALES = DaysWithSales;
            this.LAST_DATE_SOLD = LastDateSold;
            this.CumulativeSales = CumulativeSales;
            this.ActualSales = ActualSales;
        }
    }
}
