using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSupport.DataAccess.TransferObjects
{
    public class GpmPriceTransferObject
    {
        public Tuple<long,long> IDs { get; set; }
        public string Region { get; set; }
        public Guid GpmID { get; set; }
        public int ItemID { get; set; }
        public int BusinessUnit { get; set; }
        public string CurrencyCode { get; set; }
        public DateTime? TagExpirationDate { get; set; }
        PriceTransferObject RegPrice { get; set; }
        PriceTransferObject SalePrice { get; set; }
    }
}
