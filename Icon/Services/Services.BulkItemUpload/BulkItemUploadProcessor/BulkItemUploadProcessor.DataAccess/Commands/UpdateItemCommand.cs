using System.Collections.Generic;

namespace BulkItemUploadProcessor.DataAccess.Commands
{
    public class UpdateItemCommand
    {
        public int ItemId { get; set; }
        public int MerchandiseHierarchyClassId { get; set; }
        public int BrandsHierarchyClassId { get; set; }
        public int TaxHierarchyClassId { get; set; }
        public int FinancialHierarchyClassId { get; set; }
        public int NationalHierarchyClassId { get; set; }
        public int ManufacturerHierarchyClassId { get; set; }


        public Dictionary<string, string> ItemAttributes { get; set; } = new Dictionary<string, string>();
        public string ItemTypeCode { get; set; }
    }
}