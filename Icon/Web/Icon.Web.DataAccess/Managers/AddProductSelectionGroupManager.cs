namespace Icon.Web.DataAccess.Managers
{
    public class AddProductSelectionGroupManager
    {
        public string ProductSelectionGroupName { get; set; }
        public int ProductSelectionGroupTypeId { get; set; }
        public int? TraitId { get; set; }
        public string TraitValue { get; set; }
        public int? MerchandiseHierarchyClassId { get; set; }
        public int? AttributeId { get; set; }
        public string AttributeValue { get; set; }
    }
}
