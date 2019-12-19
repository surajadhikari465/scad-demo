using System.Collections.Generic;

namespace BulkItemUploadProcessor.Common.Models
{
    public class ItemDbModel
    {
        public int ItemId { get; set; }
        public int ItemTypeId { get; set; }
        public string ItemTypeCode { get; set; }
        public string ItemTypeDescription { get; set; }
        public string ScanCode { get; set; }
        public int ScanCodeTypeId { get; set; }
        public string ScanCodeTypeDescription { get; set; }
        public int BarcodeTypeId { get; set; }
        public string BarcodeType { get; set; }
        public int MerchandiseHierarchyClassId { get; set; }
        public int BrandsHierarchyClassId { get; set; }
        public int TaxHierarchyClassId { get; set; }
        public int FinancialHierarchyClassId { get; set; }
        public int NationalHierarchyClassId { get; set; }
        public string ItemAttributesJson { get; set; }
        public string Brands { get; set; }
        public string Merchandise { get; set; }
        public string Tax { get; set; }
        public string National { get; set; }
        public string Financial { get; set; }
        public string Manufacturer { get; set; }
        public Dictionary<string, string> Nutritions { get; set; } = new Dictionary<string, string>();
        public int ManufacturerHierarchyClassId { get; set; }
    }
}