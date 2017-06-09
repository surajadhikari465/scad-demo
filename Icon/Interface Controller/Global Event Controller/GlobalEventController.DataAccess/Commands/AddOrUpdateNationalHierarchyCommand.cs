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
        public int IconId { get; set; }
        public string Name { get; set; }
        public int? Level { get; set; }
        public int? ParentId { get; set; }
        public int? IrmaId { get; set; }
        public HierarchyClass HierarchyClass { get; set; }
    }
}
