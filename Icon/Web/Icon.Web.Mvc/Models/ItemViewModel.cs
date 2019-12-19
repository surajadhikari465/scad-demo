using Icon.Framework;
using Icon.Web.Common;
using Icon.Web.DataAccess.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;

namespace Icon.Web.Mvc.Models
{
    public class ItemViewModel
    {
        [DisplayName("Item ID")]
        public int ItemId { get; set; }
        public int ItemTypeId { get; set; }
        [DisplayName("Item Type")]
        public string ItemTypeDescription { get; set; }
        [DisplayName("Scan Code")]
        public string ScanCode { get; set; }
        public int BarcodeTypeId { get; set; }
        [DisplayName("Barcode Type")]
        public string BarcodeType { get; set; }
        [DisplayName("Merchandise")]
        public int MerchandiseHierarchyClassId { get; set; }
        [DisplayName("Merchandise")]
        public string MerchandiseHierarchyLineage { get; set; }
        [DisplayName("Brand")]
        public int BrandsHierarchyClassId { get; set; }
        [DisplayName("Brand")]
        public string BrandsHierarchyLineage { get; set; }
        [DisplayName("Tax")]
        public int TaxHierarchyClassId { get; set; }
        [DisplayName("Tax")]
        public string TaxHierarchyLineage { get; set; }
        [DisplayName("Financial")]
        public int FinancialHierarchyClassId { get; set; }
        [DisplayName("Financial")]
        public string FinancialHierarchyLineage { get; set; }
        [DisplayName("National")]
        public int NationalHierarchyClassId { get; set; }
        [DisplayName("National")]
        public string NationalHierarchyLineage { get; set; }
        [DisplayName("Manufacturer")]
        public int? ManufacturerHierarchyClassId { get; set; }
        [DisplayName("Manufacturer")]
        public string ManufacturerHierarchyLineage { get; set; }

        public Dictionary<string, string> ItemAttributes { get; set; }
        public Dictionary<string, string> Nutritions { get; set; }

        public Enums.WriteAccess UserWriteAccess { get; set; }

        public ItemViewModel(ItemDbModel item)
        {
            ItemId = item.ItemId;
            ItemTypeId = item.ItemTypeId;
            ItemTypeDescription = ItemTypes.Descriptions.AsDictionary[item.ItemTypeId];
            ScanCode = item.ScanCode;
            BarcodeTypeId = item.BarcodeTypeId;
            BarcodeType = item.BarcodeType;
            MerchandiseHierarchyClassId = item.MerchandiseHierarchyClassId;
            BrandsHierarchyClassId = item.BrandsHierarchyClassId;
            TaxHierarchyClassId = item.TaxHierarchyClassId;
            FinancialHierarchyClassId = item.FinancialHierarchyClassId;
            NationalHierarchyClassId = item.NationalHierarchyClassId;
            ManufacturerHierarchyClassId = item.ManufacturerHierarchyClassId;
            ItemAttributes = JsonConvert.DeserializeObject<Dictionary<string, string>>(item.ItemAttributesJson);
            Nutritions = item.Nutritions;
        }

        public ItemViewModel()
        {
        }
    }
}