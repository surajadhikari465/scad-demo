namespace AmazonLoad.Hierarchy
{
    public class HierarchyClassModel
    {
        public int HierarchyId { get; set; }
        public string HierarchyName { get; set; }
        public string HierarchyLevelName { get; set; }
        public bool ItemsAttached { get; set; }
        public string HierarchyClassId { get; set; }
        public string HierarchyClassName { get; set; }
        public int HierarchyLevel { get; set; }
        public int? HierarchyParentClassId { get; set; }
    }
}
