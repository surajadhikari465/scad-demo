using Icon.Framework;

namespace Icon.Web.DataAccess.Commands
{
    public class AddHierarchyClassMessageCommand
    {
        public HierarchyClass HierarchyClass { get; set; }
        public bool ClassNameChange { get; set; }
        public bool DeleteMessage { get; set; }
        public string NationalClassCode { get; set; }
    }
}
