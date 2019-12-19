using Icon.Framework;

namespace Icon.Web.DataAccess.Infrastructure
{
    public class MessageHierarchyData
    {
        public HierarchyClass HierarchyClass { get; set; }
        public bool ClassNameChange { get; set; }
        public bool DeleteMessage { get; set; }
    }
}
