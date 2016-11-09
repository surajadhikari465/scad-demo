using RegionalEventController.DataAccess.Models;
using System.Collections.Generic;

namespace RegionalEventController.DataAccess.Commands
{
    public class DeleteNewItemsFromIrmaQueueCommand
    {
        public List<IrmaNewItem> NewIrmaItems { get; set; }
    }
}
