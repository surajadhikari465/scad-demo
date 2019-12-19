using Icon.Common.DataAccess;
using Services.NewItem.Commands;
using Services.NewItem.Models;
using System.Collections.Generic;
using System.Linq;

namespace Services.NewItem.Services
{
    public class IconItemService : IIconItemService
    {
        private ICommandHandler<AddItemEventsToIconEventQueueCommand> addItemEventsToIconEventQueueCommandHandler;

        public IconItemService(ICommandHandler<AddItemEventsToIconEventQueueCommand> addItemEventsToIconEventQueueCommandHandler)
        {
            this.addItemEventsToIconEventQueueCommandHandler = addItemEventsToIconEventQueueCommandHandler;
        }

        public void AddItemEventsToIconEventQueue(IEnumerable<NewItemModel> newItems)
        {
            if (newItems.Any())
            {
                addItemEventsToIconEventQueueCommandHandler.Execute(new AddItemEventsToIconEventQueueCommand
                {
                    NewItems = newItems
                });
            }
        }
    }
}
