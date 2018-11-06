using System.Collections.Generic;
using Icon.Esb.Subscriber;
using KitBuilder.ESB.Listeners.Item.Service.Models;

namespace KitBuilder.ESB.Listeners.Item.Service.Notifiers
{
    public interface IItemListenerNotifier
    {
        void NotifyOfItemError(IEsbMessage message, bool schemaErrorOccurred, List<ItemModel> itemModelsWithErrors);
    }
}
