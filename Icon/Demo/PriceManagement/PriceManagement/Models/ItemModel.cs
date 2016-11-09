using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceManagement.Models
{
    public class ItemModel
    {
        public ItemModel()
        {
            Prices = new List<PriceModel>();
        }

        public int ItemId { get; set; }
        public string ScanCode { get; set; }
        public List<PriceModel> Prices { get; set; }
    }
}
