
namespace Icon.Web.Mvc.Models
{
    public class ProductSelectionGroupViewModel
    {
        public int ProductSelectionGroupId { get; set; }
        public string ProductSelectionGroupName { get; set; }
        public int ProductSelectionGroupTypeId { get; set; }
        public int? TraitId { get; set; }
        public string TraitValue { get; set; }
        public int? MerchandiseHierarchyClassId { get; set; }
        public int? AttributeId { get; set; }
        public string AttributeValue { get; set; }
    }
}