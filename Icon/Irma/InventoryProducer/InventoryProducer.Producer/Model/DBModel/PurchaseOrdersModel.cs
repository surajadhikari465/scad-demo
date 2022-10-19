using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryProducer.Producer.Model.DBModel
{
    public class PurchaseOrdersModel
    {
        public int OrderHeaderId { get; set; }
        public int? ExternalOrderId { get; set; }
        public string ExternalSource { get; set; }
        public string PurchaseType { get; set; }
        public string SupplierName { get; set; }
        public int LocationNumber { get; set; }
        public string LocationName { get; set; }
        public int? OrderSubTeamNo { get; set; }
        public string OrderSubTeamName { get; set; }
        public int? OrderTeamNo { get; set; }
        public string OrderTeamName { get; set; }
        public string Status { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public int? ApproverId { get; set; }
        public string ApproverName { get; set; }
        public DateTime? CreateDateTime { get; set; }
        public DateTime? ApproveDateTime { get; set; }
        public DateTime? CloseDateTime { get; set; }
        public string PurchaseOrderComments { get; set; }
        public int? PurchaseOrderDetailNumber { get; set; }
        public int? SourceItemKey { get; set; }
        public string DefaultScanCode { get; set; }
        public int? HostSubTeamNumber { get; set; }
        public string HostSubTeamName { get; set; }
    }
}
