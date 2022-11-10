using System;

namespace InventoryProducer.Producer.Model.DBModel
{
    // DB model for Receive
    public class ReceiveModel
    {
        public int OrderHeaderId { get; set; }
        public int OrderItemID { get; set; }
        public string Identifier { get; set; }
        public int ItemKey { get; set; }
        public string HostSubTeam { get; set; }
        public int? HostSubTeamNumber { get; set; }
        public string SubTeam { get; set; }
        public int? SubTeamNumber { get; set; }
        public DateTime? DateReceived { get; set; }
        public int CreditPO { get; set; }              // Stored as bit (boolean) 0 or 1 in DB, hence used int
        public int? QuantityReceived { get; set; }
        public int? QuantityOrdered { get; set; }
        public decimal PackageDesc1 { get; set; }
        public decimal PackageDesc2 { get; set; }
        public int? RecvLogUserID { get; set; }
        public string RecvUserName { get; set; }
        public string OrderUom { get; set; }
        public string ReceiptUom { get; set; }
        public string ReceiptStatus { get; set; }
        public string VIN { get; set; }
        public int? StoreNumber { get; set; }
        public string StoreName { get; set; }
        public DateTime? PastReceiptDate { get; set; }
        public string SupplierNumber { get; set; }
        public DateTime CreateDateTime { get; set; }
    }
}