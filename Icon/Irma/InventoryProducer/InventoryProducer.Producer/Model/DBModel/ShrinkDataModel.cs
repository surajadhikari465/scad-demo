using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryProducer.Producer.Model.DBModel
{
    public class ShrinkDataModel
    {
        public int AdjustmentNumber { get; set; }
        public int LocationNumber { get; set; }
        public string LocationName { get; set; }
        public int? HostSubTeamNumber { get; set; }
        public string HostSubTeamName { get; set; }
        public int? SubTeamNumber { get; set; }
        public string SubTeamName { get; set; }
        public string DefaultScanCode { get; set; }
        public string ReasonCode { get; set; }
        public decimal AdjustmentQuantity { get; set; }
        public string AdjustmentQuantityUOMCode { get; set; }
        public int SourceItemKey { get; set; }
        public DateTime CurrentDateTime { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
    }
}
