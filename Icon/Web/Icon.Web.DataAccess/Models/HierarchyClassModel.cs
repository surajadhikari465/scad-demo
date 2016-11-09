
namespace Icon.Web.DataAccess.Models
{
    public class HierarchyClassModel
    {
        public int HierarchyId { get; set; }
        public int HierarchyClassId { get; set; }
        public string HierarchyClassName { get; set; }
        public int? HierarchyLevel { get; set; }
        public string HierarchyLineage { get; set; }
        public int? HierarchyParentClassId { get; set; }       
    }
}
