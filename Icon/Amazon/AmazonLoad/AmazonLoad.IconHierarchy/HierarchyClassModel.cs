using System.Collections;
using System.Collections.Generic;

namespace AmazonLoad.IconHierarchy
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
        public string NationalClassCode { get; set; }
        public Dictionary<string,string> HierarchyTraits { get; set; }
    }
}
