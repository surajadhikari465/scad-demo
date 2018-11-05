using Icon.Common.DataAccess;
using System.Collections.Generic;
using Icon.Esb.Subscriber;
using System;
using KitBuilder.ESB.Listeners.Item.Service.Commands;
using KitBuilder.ESB.Listeners.Item.Service.Models;

namespace KitBuilder.ESB.Listeners.Item.Service.Services
{
    public class ItemService : IItemService
    {
        private ICommandHandler<ItemAddOrUpdateCommand> addOrUpdateItemsCommandHandler;
        public ItemService(ICommandHandler<ItemAddOrUpdateCommand> addOrUpdateItemsCommandHandler)
        {
            this.addOrUpdateItemsCommandHandler = addOrUpdateItemsCommandHandler;
        }

        public void AddOrUpdateItems(IEnumerable<ItemModel> items)
        {
            addOrUpdateItemsCommandHandler.Execute(new ItemAddOrUpdateCommand { Items = items });
        }
    }
}
