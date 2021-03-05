using Icon.Framework;
using System.Collections.Generic;

namespace BulkItemUploadProcessor.Common
{
    public static class Constants
    {
        public const string ItemWorksheetName = "Items";
        public static class Attributes
        {
            public const string ScanCode = "ScanCode";
            public const string CreatedBy = "CreatedBy";
            public const string CreatedDateTimeUtc = "CreatedDateTimeUtc";
            public const string ModifiedBy = "ModifiedBy";
            public const string ModifiedDateTimeUtc = "ModifiedDateTimeUtc";
            public const string ItemTypeCode = "ItemTypeCode";
            public const string FoodStampEligible = "FoodStampEligible";
            public const string Inactive = "Inactive";
            public const string ProhibitDiscount = "ProhibitDiscount";
        }

        public const string ScanCodeColumnHeader = "Scan Code";
        public const string BarcodeTypeColumnHeader = "Barcode Type";
        public const string RemoveExcelValue = "REMOVE";
        public const string FoodStampEligibleAttributeName = "FoodStampEligible";
        public const string InactiveAttributeName = "Inactive";
        public const string ProhibitDiscountAttributeName = "ProhibitDiscount";
        public const string JsonTrue = "true";
        public const string JsonFalse = "false";
        public const string ManufacturerHierarchyName = "Manufacturer";

        public static SortedSet<string> HierarchyColumnNames = new SortedSet<string>
        {
            HierarchyNames.Merchandise,
            HierarchyNames.Brands,
            HierarchyNames.Tax,
            HierarchyNames.National,
            ManufacturerHierarchyName
        };
    }
}