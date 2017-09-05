using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSupport.DataAccess.TransferObjects
{
    public class PriceTransferObject
    {
        public long ID { get; set; }
        public decimal Price { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string PriceType { get; set; }
        public short Multiple { get; set; }
        public string SellableUOM { get; set; }
    }
}
