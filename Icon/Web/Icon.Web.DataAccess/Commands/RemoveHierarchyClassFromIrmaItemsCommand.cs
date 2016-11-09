using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Web.DataAccess.Commands
{
    public class RemoveHierarchyClassFromIrmaItemsCommand
    {
        public int HierarchyId { get; set; }
        public int HierarchyClassId { get; set; }
    }
}
