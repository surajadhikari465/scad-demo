using Icon.Framework;
using System.Collections.Generic;

namespace PushController.DataAccess.Commands
{
    public class AddOrUpdateItemPriceBulkCommand
    {
        public List<ItemPrice> ItemPrices { get; set; }
    }
}
