using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.NewItem.Models;

namespace Services.NewItem.Commands
{
    public class ArchiveNewItemsCommand
    {
        public IEnumerable<NewItemModel> NewItems { get; set; }
    }
}
