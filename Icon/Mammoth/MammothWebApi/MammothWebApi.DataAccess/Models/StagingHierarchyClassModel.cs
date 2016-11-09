using System;

namespace MammothWebApi.DataAccess.Models
{
    public class StagingHierarchyClassModel
    {
        public int HierarchyClassID { get; set; }
        public int? HierarchyID { get; set; }
        public string HierarchyClassName { get; set; }
        public int? HierarchyClassParentID { get; set; }
        public DateTime? Timestamp { get; set; }
    }
}
