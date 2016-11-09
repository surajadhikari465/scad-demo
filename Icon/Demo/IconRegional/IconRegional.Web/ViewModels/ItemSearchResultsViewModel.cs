using IconRegional.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IconRegional.Web.ViewModels
{
    public class ItemSearchResultsViewModel
    {
        public List<ItemModel> Items { get; set; }
        public int TotalNumberOfItems { get; set; }
    }
}
