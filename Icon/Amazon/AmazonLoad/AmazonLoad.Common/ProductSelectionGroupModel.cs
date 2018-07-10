namespace AmazonLoad.Common
{
    public class ProductSelectionGroupModel
    {
        public int ProductSelectionGroupId { get; set; }
        public string ProductSelectionGroupName { get; set; }
        public int ProductSelectionGroupTypeId { get; set; }
        public int? TraitId { get; set; }
        public string TraitValue { get; set; }
        public int? MerchandiseHierarchyClassId { get; set; }
        public string ProductSelectionGroupTypeName { get; set; }
    }
}
