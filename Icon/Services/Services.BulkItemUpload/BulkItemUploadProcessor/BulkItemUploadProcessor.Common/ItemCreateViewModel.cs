using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using BulkItemUploadProcessor.Common.Models;
using Icon.Framework;
using Newtonsoft.Json;
using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;

namespace BulkItemUploadProcessor.Common
{
    public class NewItemViewModel
    {
        public NewItemViewModel()
        {
            ItemAttributes = new Dictionary<string, string>();
            Errors = new List<string>();
        }
        public int RowId { get; set; }
        public int ItemId { get; set; }


        public string BrandHierarchyClassId { get; set; }
        public string BrandHierarchyName { get; set; }
        public string TaxHierarchyClassId { get; set; }
        public string TaxHierarchyName { get; set; }
        public string MerchandiseHierarchyClassId { get; set; }
        public string MerchandiseHierarchyName { get; set; }
        public string FinancialHierarchyClassId { get; set; }
        public string FinancialHierarchyName { get; set; }
        public string ManufacturerHierarchyClassId { get; set; }
        public string ManufacturerHierarchyName { get; set; }
        public string BarcodeTypeId { get; set; }
        public string BarcodeTypeName { get; set; }
        public string BarcodeType { get; set; }
        public string NationalHierarchyClassId { get; set; }
        public string NationalHierarchyName { get; set; }
        public string ScanCode { get; set; }
        public string  Inactive { get; set; }
        public List<string> Errors { get; set; }
        public Dictionary<string, string> ItemAttributes { get; set; }
        public Dictionary<string, string> Nutritions { get; set; }

        public NewItemViewModel(ItemDbModel item)
        {
            ItemId = item.ItemId;
            ScanCode = item.ScanCode;
            BarcodeTypeId = item.BarcodeTypeId.ToString();
            BarcodeType = item.BarcodeType;
            MerchandiseHierarchyClassId = item.MerchandiseHierarchyClassId.ToString();
            BrandHierarchyClassId = item.BrandsHierarchyClassId.ToString();
            TaxHierarchyClassId = item.TaxHierarchyClassId.ToString();
            FinancialHierarchyClassId = item.FinancialHierarchyClassId.ToString();
            NationalHierarchyClassId = item.NationalHierarchyClassId.ToString();
            ManufacturerHierarchyClassId = item.ManufacturerHierarchyClassId.ToString();
            ItemAttributes = JsonConvert.DeserializeObject<Dictionary<string, string>>(item.ItemAttributesJson);
            Errors = new List<string>();
            Nutritions = item.Nutritions;
        }

    }


    public class EditItemViewModel 
    {
        public int RowId { get; set; }
        public string ScanCode { get; set; }
        public string BrandHierarchyClassId { get; set; }
        public string NationalHierarchyClassId { get; set; }
        public string MerchandiseHierarchyClassId { get; set; }
        public string TaxHierarchyClassId { get; set; }
        public string ManufacturerHierarchyClassId { get; set; }
        public bool BrandHierarchyClassIdIncluded { get; set; }
        public bool ScanCodeIncluded { get; set; }
        public bool NationalHierarchyClassIdIncluded { get; set; }
        public bool MerchandiseHierarchyClassIdIncluded { get; set; }
        public bool TaxHierarchyClassIdIncluded { get; set; }
        public bool ManufacturerHierarchyClassIdIncludede { get; set; }
        public int ItemId { get; set; }

        public Dictionary<string, string> ValuesToUpdate;
        public Dictionary<string, string> MergedItemAttributes;
        public List<string> Errors;

        public EditItemViewModel(int rowId, Dictionary<string, string> valuesToUpdate)
        {
            RowId = rowId;
            ValuesToUpdate = valuesToUpdate;
            Errors = new List<string>();
        }

        public EditItemViewModel()
        {
            ValuesToUpdate = new Dictionary<string, string>();
            Errors = new List<string>();
        }
    }
}