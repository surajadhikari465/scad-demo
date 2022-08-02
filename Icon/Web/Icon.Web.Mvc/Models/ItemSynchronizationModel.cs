using System;
using System.Collections.Generic;
using Icon.Web.DataAccess.Models;


namespace Icon.Web.Mvc.Models
{
	public class ItemSynchronizationModel
	{

        public IEnumerable<AttributeViewModel> Attributes { get; set; }

        public int ItemId { get; set; }

        //[Display(Name = "Brand")]
        public int BrandHierarchyClassId { get; set; }

        //[Display(Name = "Tax")]
        public int TaxHierarchyClassId { get; set; }

        //[Display(Name = "Merchandise")]
        public int MerchandiseHierarchyClassId { get; set; }

        //[Display(Name = "Barcode Type")]
        public int BarcodeTypeId { get; set; }

        //[Display(Name = "Scan Code Type")]
        public string ScanCodeType { get; set; }

        //[Display(Name = "National")]
        public int NationalHierarchyClassId { get; set; }

        //[Display(Name = "Manufacturer")]
        public int? ManufacturerHierarchyClassId { get; set; }

        //[Display(Name = "Scan Code")]
        public string ScanCode { get; set; }

        public List<string> Errors { get; set; }

        public Dictionary<string, string> ItemAttributes { get; set; }

        public IEnumerable<BarcodeTypeModel> BarcodeTypes { get; set; }
        public List<ItemColumnOrderModel> ItemColumnOrderModelList { get; set; }
    
    }
}
