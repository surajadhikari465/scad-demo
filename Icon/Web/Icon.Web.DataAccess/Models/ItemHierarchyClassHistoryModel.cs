using System;

namespace Icon.Web.DataAccess.Models
{
    public class ItemHierarchyClassHistoryModel
    {
        public int ItemId { get; set; }
        public int HierarchyClassId { get; set; }
        public int LocaleId { get; set; }
        public string HierarchyLineage { get; set; }
        public int HierarchyId { get; set; }
        public string HierarchyName { get; set; }
        public DateTime SysStartTimeUtc { get; set; }
        public DateTime SysEndTimeUtc { get; set; }
    }
}
