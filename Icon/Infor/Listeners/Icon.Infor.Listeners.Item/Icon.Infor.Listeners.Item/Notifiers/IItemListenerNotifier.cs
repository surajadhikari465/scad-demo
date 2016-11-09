using Icon.Esb.Subscriber;
using Icon.Infor.Listeners.Item.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Infor.Listeners.Item.Notifiers
{
    public interface IItemListenerNotifier
    {
        void NotifyOfItemError(IEsbMessage message, List<ItemModel> itemModelsWithErrors);
    }
}
