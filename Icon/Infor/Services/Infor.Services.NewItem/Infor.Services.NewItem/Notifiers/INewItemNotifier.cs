using Infor.Services.NewItem.Models;
using System.Collections.Generic;

namespace Infor.Services.NewItem.Notifiers
{
    public interface INewItemNotifier
    {
        void NotifyOfNewItemError(IEnumerable<NewItemModel> newItemModels);
    }
}
