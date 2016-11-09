
namespace Icon.Web.DataAccess.Commands
{
    public class HierarchyClassCommand
    {
        public int HierarchyClassId { get; set; }
        public int HierarchyId { get; set; }
        public int HierarchyLevel { get; set; }
        public int? HierarchyParentClassId { get; set; }
    }
}
