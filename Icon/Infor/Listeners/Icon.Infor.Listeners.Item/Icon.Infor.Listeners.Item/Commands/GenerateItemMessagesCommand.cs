namespace Icon.Infor.Listeners.Item.Commands
{
    using System.Collections.Generic;

    using Icon.Infor.Listeners.Item.Models;

    public class GenerateItemMessagesCommand
    {
        public IEnumerable<ItemModel> Items { get; set; }
    }
}
