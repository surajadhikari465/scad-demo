using Mammoth.Esb.HierarchyClassListener.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mammoth.Esb.HierarchyClassListener.Commands
{
    public class AddOrUpdateMerchandiseHierarchyLineageCommand
    {
        public List<HierarchyClassModel> HierarchyClasses { get; set; }
    }
}
