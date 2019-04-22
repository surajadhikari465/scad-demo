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
        private ICommandHandler<ItemAddOrUpdateOrRemoveCommand> addOrUpdateItemsCommandHandler;
        private ICommandHandler<ArchiveMessageCommand> archiveMessageCommandHandler;
        public ItemService(ICommandHandler<ItemAddOrUpdateOrRemoveCommand> addOrUpdateItemsCommandHandler, ICommandHandler<ArchiveMessageCommand> archiveMessageCommandHandler)
        {
            this.addOrUpdateItemsCommandHandler = addOrUpdateItemsCommandHandler;
            this.archiveMessageCommandHandler = archiveMessageCommandHandler;
        }

        public void AddOrUpdateOrRemoveItems(IEnumerable<ItemModel> items)
        {
            addOrUpdateItemsCommandHandler.Execute(new ItemAddOrUpdateOrRemoveCommand { Items = items });
        }

        public void ArchiveMessage(IEsbMessage message)
        {
            archiveMessageCommandHandler.Execute(new ArchiveMessageCommand { Message = message });
        }
    }
}
