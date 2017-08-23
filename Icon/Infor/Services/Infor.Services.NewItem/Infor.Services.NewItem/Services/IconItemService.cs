using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infor.Services.NewItem.Models;
using Icon.Common.DataAccess;
using Infor.Services.NewItem.Commands;

namespace Infor.Services.NewItem.Services
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
