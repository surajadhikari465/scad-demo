using Icon.Common.DataAccess;
using Icon.Infor.Listeners.Item.Commands;
using Icon.Infor.Listeners.Item.Models;
using System.Collections.Generic;
using Icon.Esb.Subscriber;
using System;

namespace Icon.Infor.Listeners.Item.Services
{
    public class ItemService : IItemService
    {
        private ICommandHandler<ItemAddOrUpdateCommand> addOrUpdateItemsCommandHandler;
        private ICommandHandler<GenerateItemMessagesCommand> generateItemMessagesCommandHandler;
        private ICommandHandler<ArchiveItemsCommand> archiveItemsCommandHandler;
        private ICommandHandler<ArchiveMessageCommand> archiveMessageCommandHandler;

        public ItemService(
            ICommandHandler<ItemAddOrUpdateCommand> addOrUpdateItemsCommandHandler,
            ICommandHandler<GenerateItemMessagesCommand> generateItemMessagesCommandHandler,
            ICommandHandler<ArchiveItemsCommand> archiveItemsCommandHandler,
            ICommandHandler<ArchiveMessageCommand> archiveMessageCommandHandler)
        {
            this.addOrUpdateItemsCommandHandler = addOrUpdateItemsCommandHandler;
            this.generateItemMessagesCommandHandler = generateItemMessagesCommandHandler;
            this.archiveItemsCommandHandler = archiveItemsCommandHandler;
            this.archiveMessageCommandHandler = archiveMessageCommandHandler;
        }

        public void AddOrUpdateItems(IEnumerable<ItemModel> items)
        {
            addOrUpdateItemsCommandHandler.Execute(new ItemAddOrUpdateCommand { Items = items });
        }

        public void GenerateItemMessages(IEnumerable<ItemModel> items)
        {
            generateItemMessagesCommandHandler.Execute(new GenerateItemMessagesCommand { Items = items });
        }

        public void ArchiveItems(IEnumerable<ItemModel> models)
        {
            archiveItemsCommandHandler.Execute(new ArchiveItemsCommand { Models = models });
        }

        public void ArchiveMessage(IEsbMessage message)
        {
            archiveMessageCommandHandler.Execute(new ArchiveMessageCommand { Message = message });
        }
    }
}
