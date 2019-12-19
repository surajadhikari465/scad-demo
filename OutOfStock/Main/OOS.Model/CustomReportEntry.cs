using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOS.Model
{
    public class CustomReportEntry
    {
        public string UPC { get; private set; }
        public string PS_TEAM { get; private set; }
        public string PS_SUBTEAM { get; private set; }
        public decimal CASE_SIZE { get; private set; }
        public decimal EFF_COST { get; private set; }
        public decimal EFF_PRICE { get; private set; }
        public string EFF_PRICETYPE { get; private set; }
        public decimal MOVEMENT { get; private set; }
        public string NOTES { get; private set; }
        public string VENDOR_KEY { get; private set; }
        public string VIN { get; private set; }
        public string BRAND { get; private set; }
        public string BRAND_NAME { get; private set; }
        public string LONG_DESCRIPTION { get; private set; }
        public string ITEM_SIZE { get; private set; }
        public string ITEM_UOM { get; private set; }
        public string CATEGORY_NAME { get; private set; }
        public string CLASS_NAME { get; private set; }
        public string ProductStatus { get; private set; }
        public DateTime? StartDate { get; private set; }
        public int timesScanned { get; private set; }
        public string StoresList { get; private set; }

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
            string StoresList
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

        }
    }
}
