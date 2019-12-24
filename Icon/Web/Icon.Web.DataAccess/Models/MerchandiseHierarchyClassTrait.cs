
namespace Icon.Web.DataAccess.Models
{
    public class MerchandiseHierarchyClassTrait 
    {
        public int HierarchyClassId { get; set; }
        public bool? ProhibitDiscount { get; set; }
        public int? FinancialHierarchyClassId { get; set; }
        public string ItemType { get; set; }
    }
}
