using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Icon.Infor.Listeners.HierarchyClass.Models;

namespace Icon.Infor.Listeners.HierarchyClass.Commands
{
    public class DeleteHierarchyClassesCommand
    {
        public IEnumerable<HierarchyClassModel> HierarchyClasses { get; set; }
    }
}
