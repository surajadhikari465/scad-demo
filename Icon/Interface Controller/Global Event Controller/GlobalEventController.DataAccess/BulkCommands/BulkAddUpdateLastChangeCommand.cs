using GlobalEventController.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalEventController.DataAccess.BulkCommands
{
    public class BulkAddUpdateLastChangeCommand
    {
        public List<IconItemLastChangeModel> IconLastChangedItems { get; set; }
    }
}
