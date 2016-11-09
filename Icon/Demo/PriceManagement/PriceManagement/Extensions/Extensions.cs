using PriceManagement.Models;
using PriceManagement.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceManagement.Extensions
{
    public static class Extensions
    {
        public static List<ItemViewModel> ToViewModels(this IEnumerable<ItemModel> items)
        {
            return items.Select(i => new ItemViewModel { ItemId = i.ItemId, ScanCode = i.ScanCode }).ToList();
        }
    }
}
