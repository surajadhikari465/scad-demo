using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infor.Services.NewItem.Models;

namespace Infor.Services.NewItem.Services
{
    public class AddNewItemsToInforRequest
    {
        public IEnumerable<NewItemModel> NewItems { get; set; }
        public string Region { get; set; }
    }
}
