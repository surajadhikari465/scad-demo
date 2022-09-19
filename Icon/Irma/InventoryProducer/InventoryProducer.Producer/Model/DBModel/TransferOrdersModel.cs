using System;

namespace InventoryProducer.Producer.Model.DBModel
{
    // DB model for TransferOrders
    public class TransferOrdersModel
    {
        public int OrderHeaderId { get; set; }
        public int? FromLocationNumber { get; set; }
        public string FromLocationName { get; set; }
        public int? ToLocationNumber { get; set; }
        public string ToLocationName { get; set; }
        public int? FromSubTeamNumber { get; set; }
        public string FromSubTeamName { get; set; }
        public int? ToSubTeamNumber { get; set; }
        public string ToSubTeamName { get; set; }
        public string Status { get; set; }
        public int? UserId { get; set; }
        public string UserName { get; set; }
        public int? ApproverId { get; set; }
        public string ApproverName { get; set; }
        public DateTime? CreateDateTime { get; set; }
        public DateTime? ApproveDateTime { get; set; }
        public int? TransferOrderDetailNumber { get; set; }
        public int? SourceItemKey { get; set; }
        public string DefaultScanCode { get; set; }
        public int? HostSubTeamNumber { get; set; }
        public string HostSubTeamName { get; set; }
        public decimal? QuantityOrdered { get; set; }
        public string OrderedUnit { get; set; }
        public string OrderedUnitCode { get; set; }
        public DateTime? ExpectedArrivalDate { get; set; }
        public decimal? PackageDesc1 { get; set; }
        public decimal? PackageDesc2 { get; set; }
    }
}
