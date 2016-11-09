using Icon.Infor.Listeners.Item.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Infor.Listeners.Item.Commands
{
    public class ItemAddOrUpdateCommand
    {
        public IEnumerable<ItemModel> Items { get; set; }
    }
}
