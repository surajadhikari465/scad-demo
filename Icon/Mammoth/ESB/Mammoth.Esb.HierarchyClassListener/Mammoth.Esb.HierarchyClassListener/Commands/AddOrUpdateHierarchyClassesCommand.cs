using Mammoth.Esb.HierarchyClassListener.Models;
using System.Collections.Generic;

namespace Mammoth.Esb.HierarchyClassListener.Commands
{
    public class AddOrUpdateHierarchyClassesCommand
    {
        public List<HierarchyClassModel> HierarchyClasses { get; set; }
    }
}