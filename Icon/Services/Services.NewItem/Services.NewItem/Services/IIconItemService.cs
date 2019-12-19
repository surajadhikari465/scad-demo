using Services.NewItem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.NewItem.Services
{
    public interface IIconItemService
    {
        void AddItemEventsToIconEventQueue(IEnumerable<NewItemModel> newItems);
    }
}
