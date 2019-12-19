namespace Icon.Web.Tests.Integration.TestModels
{
    public class TestHierarchyClassModel
    {
        public string HierarchyName { get; set; }
        public int HierarchyClassId { get; set; }
        public string HierarchyClassName { get; set; }
        public int HierarchyLevel { get; set; }
        public int? HierarchyParentClassId { get; set; }
        public string HierarchyLineage { get; set; }
    }
}
