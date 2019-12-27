using System.Collections.Generic;

namespace Icon.Web.DataAccess.Managers
{
    public class AddItemManager
    {
        public int ItemId { get; set; }
        public int MerchandiseHierarchyClassId { get; set; }
        public int BrandsHierarchyClassId { get; set; }
        public int TaxHierarchyClassId { get; set; }
        public int FinancialHierarchyClassId { get; set; }
        public int NationalHierarchyClassId { get; set; }
        public int ManufacturerHierarchyClassId { get; set; }
        public Dictionary<string, string> ItemAttributes { get; set; }
        public int ItemTypeId { get; set; }
        public int? BarCodeTypeId { get; set; }
        public string ScanCode { get; set; }

        public AddItemManager()
        {
            ItemAttributes = new Dictionary<string, string>();
        }
    }
}