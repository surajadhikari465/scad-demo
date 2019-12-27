using System.Collections.Generic;

namespace Icon.Web.DataAccess.Commands
{
    public class AddItemCommand
    {
        public int ItemId { get; set; }
        public int MerchandiseHierarchyClassId { get; set; }
        public int BrandsHierarchyClassId { get; set; }
        public int TaxHierarchyClassId { get; set; }
        public int FinancialHierarchyClassId { get; set; }
        public int NationalHierarchyClassId { get; set; }
        public Dictionary<string, string> ItemAttributes { get; set; }
        public int ItemTypeId { get; set; }
        public int? SelectedBarCodeTypeId { get; set; }
        public string ScanCode { get; set; }
        public int ManufacturerHierarchyClassId { get; set; }

        public AddItemCommand()
        {
            ItemAttributes = new Dictionary<string, string>();
        }
    }
}