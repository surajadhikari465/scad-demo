using Services.NewItem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.NewItem.Commands
{
    public class AddItemEventsToIconEventQueueCommand
    {
        public IEnumerable<NewItemModel> NewItems { get; set; }
    }
}
