using Infor.Services.NewItem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infor.Services.NewItem.Commands
{
    public class AddNewItemsToIconCommand
    {
        public IEnumerable<NewItemModel> NewItems { get; set; }
    }
}
