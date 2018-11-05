using System.Collections.Generic;
using Icon.Esb.Subscriber;
using KitBuilder.ESB.Listeners.Item.Service.Models;

namespace KitBuilder.ESB.Listeners.Item.Service.Services
{
    public interface IItemService
    {
        void AddOrUpdateItems(IEnumerable<ItemModel> items);
    }
}