using GlobalEventController.Common;
using System.Collections.Generic;

namespace GlobalEventController.DataAccess.BulkCommands
{
    public class BulkAddMammothPriceEventsCommand
    {
        public List<ValidatedItemModel> ValidatedItems { get; set; }
    }
}
