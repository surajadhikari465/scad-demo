namespace Icon.Web.Mvc.Excel
{
    public static class NewItemExcelHelper
    {
        public static class NewExcelExportColumnNames
        {
            public const string BarCodeType = "BarcodeType";
            public const string ScanCode = "ScanCode";
            public const string Brand = "BrandsHierarchyClassId";
            public const string Merchandise = "MerchandiseHierarchyClassId";
            public const string NationalClass = "NationalHierarchyClassId";
            public const string Tax = "TaxHierarchyClassId";
            public const string Manufacturer = "ManufacturerHierarchyClassId";
            public const string ItemId = "ItemId";
            public const string ItemType = "ItemTypeDescription";
            public const string ScanCodeType = "ScanCodeTypeDescription";
            public const string Financial = "FinancialHierarchyClassId";
            public const string ProhibitDiscount = "ProhibitDiscount";
        }

        public static class NewExcelExportColumnWidths
        {
            public const int BarCodeType = 6000;
            public const int ScanCode = 4000;
            public const int HierarchyClass = 4000;
            public const int AttributeNames = 4000;
            public const int Tax = 4000;
            public const int ItemId = 4000;
            public const int ItemType = 4000;
            public const int ScanCodeType = 4000;
            public const int Financial = 4000;
            public const int ProhibitDiscount = 4000;
            public const int Manufacturer = 4000;
        }

        public static class ConsolidatedNewItemColumnIndexes
        {
            public const int BarCodeTypeColumnIndex = 0;
            public const int ScanCodeColumnIndex = 1;
            public const int ScanCodeTypeIndex = 2;
            public const int BrandColumnIndex = 3;
            public const int MerchandiseColumnIndex = 4;
            public const int TaxColumnIndex = 5;
            public const int NationalColumnIndex = 6;
            public const int FinancialIndex = 7;
            public const int ManufacturerIndex = 8;
            public const int ProhibitDiscountndex = 9;
            public const int ItemIdIndex = 10;
            public const int ItemTypeIndex = 11;


        }
    }
}