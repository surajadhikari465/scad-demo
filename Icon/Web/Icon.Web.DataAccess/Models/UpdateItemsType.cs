namespace Icon.Web.DataAccess.Models
{
    public class UpdateItemsType
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
        public int ItemTypeId { get; set; }
    }
}
