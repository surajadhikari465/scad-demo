using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IrmaMobile.Domain.Models
{
    public class TransferOrderItemModel
    {
        public int QuantityOrdered { get; set; }
        public int ItemKey { get; set; }
        public int QuantityUnit { get; set; }
        public int AdjustedCost { get; set; }
        public int ReasonCodeDetailId { get; set; }
    }
}
