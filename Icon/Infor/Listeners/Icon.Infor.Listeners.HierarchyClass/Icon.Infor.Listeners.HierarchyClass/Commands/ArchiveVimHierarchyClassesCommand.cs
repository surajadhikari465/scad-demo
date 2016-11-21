using System.Collections.Generic;
using Icon.Infor.Listeners.HierarchyClass.Models;

namespace Icon.Infor.Listeners.HierarchyClass.Commands
{
    public class ArchiveVimHierarchyClassesCommand
    {
        public IEnumerable<VimHierarchyClassModel> HierarchyClasses { get; set; }
    }
}