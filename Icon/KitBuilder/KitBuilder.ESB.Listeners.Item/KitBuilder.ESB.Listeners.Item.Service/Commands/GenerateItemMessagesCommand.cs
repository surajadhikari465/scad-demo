using System.Collections.Generic;
using KitBuilder.ESB.Listeners.Item.Service.Models;

namespace KitBuilder.ESB.Listeners.Item.Service.Commands
{
    public class GenerateItemMessagesCommand
    {
        public IEnumerable<ItemModel> Items { get; set; }
    }
}
