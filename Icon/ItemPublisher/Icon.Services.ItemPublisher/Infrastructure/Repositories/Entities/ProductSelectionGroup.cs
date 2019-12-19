namespace Icon.Services.ItemPublisher.Repositories.Entities
{
    public class ProductSelectionGroup
    {
        public int? AttributeId { get; set; }
        public string AttributeName { get; set; }
        public string AttributeValue { get; set; }
        public int ProductSelectionGroupId { get; set; }
        public string ProductSelectionGroupName { get; set; }
        public int ProductSelectionGroupTypeId { get; set; }
        public string ProductSelectionGroupTypeName { get; set; }
        public int? TraitId { get; set; }
        public string TraitValue { get; set; }
        public int? MerchandiseHierarchyClassId { get; set; }
    }
}