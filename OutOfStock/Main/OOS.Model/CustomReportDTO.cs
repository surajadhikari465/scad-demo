using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOS.Model
{
    public class CustomReportDTO
    {
        public string STORE_NAME { get; set; }
        public string STORE_ABBREVIATION { get; set; }
        public string UPC { get; set; }
        public string PS_TEAM { get; set; }
        public string PS_SUBTEAM { get; set; }
        public decimal? CASE_SIZE { get; set; }
        public decimal? EFF_COST { get; set; }
        public decimal? EFF_PRICE { get; set; }
        public string EFF_PRICETYPE { get; set; }
        public decimal? MOVEMENT { get; set; }
        public string NOTES { get; set; }
        public string VENDOR_KEY { get; set; }
        public string VIN { get; set; }
        public string BRAND { get; set; }
        public string BRAND_NAME { get; set; }
        public string LONG_DESCRIPTION { get; set; }
        public string ITEM_SIZE { get; set; }
        public string ITEM_UOM { get; set; }
        public string CATEGORY_NAME { get; set; }
        public string CLASS_NAME { get; set; }
        public string ProductStatus { get; set; }
        public DateTime? StartDate { get; set; }
        public int? timesScanned { get; set; }
    }
}
