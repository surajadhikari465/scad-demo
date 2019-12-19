using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOS.Model
{
    public class CustomReportDTO
    {
        public int ID { get; set; }
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
        public DateTime? LAST_DATE_SOLD { get; set; }
        public int? DAYS_WITH_SALES { get; set; }

        public CustomReportDTO() {}

        public CustomReportDTO(int Id, string StoreName, string StoreAbbreviation, string UPC, string PSTeam, string PSSubTeam, decimal? CaseSize,
            decimal? EFFCost, decimal? EFFPrice, string EFFPriceType, decimal? Movement, string Notes, string VendorKey, string VIN, string Brand,
            string BrandName, string LongDescription, string ItemSize, string ItemUOM, string CategoryName, string ClassName, 
            DateTime? LastDateSold, int? DaysWithSales ,string ProductStatus, DateTime? StartDate, int? TimesScanned)
        {
            this.ID = Id;
            this.STORE_NAME = StoreName;
            this.STORE_ABBREVIATION = StoreAbbreviation;
            this.UPC = UPC;
            this.PS_TEAM = PSTeam;
            this.PS_SUBTEAM = PSSubTeam;
            this.CASE_SIZE = CaseSize;
            this.EFF_COST = EFFCost;
            this.EFF_PRICE = EFFPrice;
            this.EFF_PRICETYPE = EFFPriceType;
            this.MOVEMENT = Movement;
            this.NOTES = Notes;
            this.VENDOR_KEY = VendorKey;
            this.VIN = VIN;
            this.BRAND = Brand;
            this.BRAND_NAME = BrandName;
            this.LONG_DESCRIPTION = LongDescription;
            this.ITEM_SIZE = ItemSize;
            this.ITEM_UOM = ItemUOM;
            this.CATEGORY_NAME = CategoryName;
            this.CLASS_NAME = ClassName;
            this.ProductStatus = ProductStatus;
            this.StartDate = StartDate;
            this.timesScanned = TimesScanned;
            this.DAYS_WITH_SALES = DaysWithSales;
            this.LAST_DATE_SOLD = LastDateSold;

        }

    }
}
