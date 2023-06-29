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
        public string InvoiceNumber { get; set; }
        public string ExternalSource { get; set; }
        public string PurchaseType { get; set; }
        public string SupplierNumber { get; set; }
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
        public string PurchaseOrderNotes { get; set; }
        public int? PurchaseOrderDetailNumber { get; set; }
        public int? SourceItemKey { get; set; }
        public string ItemName { get; set; }
        public string ItemBrand { get; set; }
        public string DefaultScanCode { get; set; }
        public int? HostSubTeamNumber { get; set; }
        public string HostSubTeamName { get; set; }
        public decimal? QuantityOrdered { get; set; }
        public string OrderedUnitCode { get; set; }
        public string OrderedUnit { get; set; }
        public int? PackSize1 { get; set; }
        public int? PackSize2 { get; set; }
        public string RetailUnit { get; set; }
        public bool CostedByWeight { get; set; }
        public bool CatchweightRequired { get; set; }
        public decimal? ItemCost { get; set; }
        public DateTime? EarliestArrivalDate { get; set; }
        public DateTime? ExpectedArrivalDate { get; set; }
        public decimal? EInvoiceQuantity { get; set; }
        public decimal? EInvoiceWeight { get; set; }
        public int? OtherOrderExternalSourceOrderID { get; set; }
        public string OtherExternalSourceDescription { get; set; }
        public string VendorItemNumber { get; set; }
    }
}
