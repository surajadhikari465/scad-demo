using System.Collections.Generic;

namespace Icon.Web.Mvc.Utility
{
    public static class ExportItemColumnNameMapper
    {
        public static readonly Dictionary<string, string> keyToDisplayNameDictionay = new Dictionary<string, string>()
        {
            {"ItemId", "ItemId" },
            {"ItemTypeId", "Item Type Id" },
            {"ItemTypeCode", "Item Type Code" },
            {"ItemTypeDescription", "Item Type Description" },
            {"ScanCode", "Scan Code" },
            {"ScanCodeTypeId", "Scan Code Type Id" },
            {"ScanCodeTypeDescription", "Scan Code Type Description" },
            {"BrandsHierarchyClassId", "Brands" },
            {"MerchandiseHierarchyClassId", "Merchandise" },
            {"TaxHierarchyClassId", "Tax" },
            {"NationalHierarchyClassId", "National" },
            {"FinancialHierarchyClassId", "Financial" },
            {"ManufacturerHierarchyClassId", "Manufacturer" },
            {"ProhibitDiscount","Prohibit Discount" },
            {"BarcodeType","Barcode Type" }
        };
    }
}