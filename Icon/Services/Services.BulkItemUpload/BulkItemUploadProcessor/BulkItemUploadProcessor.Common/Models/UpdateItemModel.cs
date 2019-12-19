using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkItemUploadProcessor.Common.Models
{
    public class UpdateItemModel
    {
        public int ItemId { get; set; }
        public string ScanCode { get; set; }
        public int? BrandsHierarchyClassId { get; set; }
        public int? FinancialHierarchyClassId { get; set; }
        public int? MerchandiseHierarchyClassId { get; set; }
        public int? NationalHierarchyClassId { get; set; }
        public int? TaxHierarchyClassId { get; set; }
        public int? ManufacturerHierarchyClassId { get; set; }
        public string ItemAttributesJson { get; set; }
        public int? ItemTypeId { get; set; }
    }
}
