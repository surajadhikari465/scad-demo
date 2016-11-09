using Icon.Framework;
using Icon.Web.DataAccess.Models;

namespace Icon.Web.DataAccess.Commands
{
    public class AddHierarchyClassMessageCommand
    {
        public HierarchyClass HierarchyClass { get; set; }
        public bool ClassNameChange { get; set; }
        public bool DeleteMessage { get; set; }
    }
}
