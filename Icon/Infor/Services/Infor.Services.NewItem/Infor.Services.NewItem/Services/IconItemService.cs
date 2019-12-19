using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.NewItem.Models;
using Icon.Common.DataAccess;
using Services.NewItem.Commands;

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
