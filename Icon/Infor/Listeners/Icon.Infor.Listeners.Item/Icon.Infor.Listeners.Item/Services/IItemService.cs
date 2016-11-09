using System.Collections.Generic;
using Icon.Esb.Subscriber;
using Icon.Infor.Listeners.Item.Models;

namespace Icon.Infor.Listeners.Item.Services
{
    public interface IItemService
    {
        void AddOrUpdateItems(IEnumerable<ItemModel> items);
        void GenerateItemMessages(IEnumerable<ItemModel> items);
        void ArchiveItems(IEnumerable<ItemModel> models);
        void ArchiveMessage(IEsbMessage message);
    }
}