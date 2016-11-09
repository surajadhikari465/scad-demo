using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceManagement.Models
{
    public class PriceModel
    {
        public decimal Price { get; set; }
        public int ItemId { get; set; }
        public string Region { get; set; }
        public int BusinessUnit { get; set; }
    }
}
