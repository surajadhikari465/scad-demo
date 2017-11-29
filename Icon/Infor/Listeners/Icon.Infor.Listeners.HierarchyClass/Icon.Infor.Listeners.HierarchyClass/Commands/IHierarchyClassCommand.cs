using Icon.Infor.Listeners.HierarchyClass.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Infor.Listeners.HierarchyClass.Commands
{
    public interface IHierarchyClassCommand
    {
        IEnumerable<InforHierarchyClassModel> HierarchyClasses { get; set; }
    }
}
