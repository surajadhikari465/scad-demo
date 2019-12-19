namespace BulkItemUploadProcessor.Common.Models
{
    public class MerchDependentItemPropertiesModel
    {
        public int FinancialHierarcyClassId { get; set; }
        public bool ProhibitDiscount { get; set; }
        public string NonMerchandiseTraitValue { get; set; }
    }

    public class MerchPropertiesModel : MerchDependentItemPropertiesModel
    {
        public int MerchandiseHierarchyClassId { get; set; }
        public string ItemTypeCode { get; set; }
    }

}