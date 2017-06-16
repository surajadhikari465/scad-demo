using Icon.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalEventController.DataAccess.Commands
{
    public class AddOrUpdateNationalHierarchyCommand
    {
        public HierarchyClass HierarchyClass { get; set; }
        public HierarchyClass ParentHierarchyClass { get; set; }
        public int? IrmaId { get; set; }
    }
}
