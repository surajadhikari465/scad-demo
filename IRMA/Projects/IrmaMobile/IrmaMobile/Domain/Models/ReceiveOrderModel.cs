using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IrmaMobile.Domain.Models
{
    public class ReceiveOrderModel
    {
        public int Quantity { get; set; }
        public decimal Weight { get; set; }
        public DateTime Date { get; set; }
        public bool Correction { get; set; }
        public int OrderItemId { get; set; }
        public int ReasonCodeId { get; set; }
        public decimal PackSize { get; set; }
        public int UserId { get; set; }
    }
}
