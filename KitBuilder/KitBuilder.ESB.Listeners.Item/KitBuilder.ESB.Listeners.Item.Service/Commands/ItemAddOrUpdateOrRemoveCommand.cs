using KitBuilder.ESB.Listeners.Item.Service.Models;
using System.Collections.Generic;

namespace KitBuilder.ESB.Listeners.Item.Service.Commands
{
    public class ItemAddOrUpdateOrRemoveCommand
    {
        public IEnumerable<ItemModel> Items { get; set; }
    }
}
