namespace Icon.Services.ItemPublisher.Repositories.Entities
{
    public class Hierarchy
    {
        public int ItemId { get; set; }
        public int HierarchyId { get; set; }
        public string HierarchyName { get; set; }
        public int HierarchyClassId { get; set; }
        public string HierarchyClassName { get; set; }
        public int? HierarchyClassParentId { get; set; }
        public string HierarchyClassParentName { get; set; }
        public int? HierarchyLevel { get; set; }
    }
}