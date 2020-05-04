using System;
using System.Collections.Generic;

namespace IrmaMobile.Domain.Models
{
    public class OrderModel
    {
        public DateTime AccountingUploadDate { get; set; }

        public DateTime Accounting_In_DateStamp { get; set; }
        
        public decimal AdjustedReceivedCost { get; set; }
        
        public decimal AllowanceDiscountAmount { get; set; }
        
        public string ApprovedByUserName { get; set; }

        public DateTime ApprovedDate { get; set; }

        public string BuyerEmail { get; set; }

        public string BuyerName { get; set; }

        public DateTime CloseDate { get; set; }

        public string ClosedByUserName { get; set; }

        public string CompanyName { get; set; }

        public int CreatedBy { get; set; }

        public string CreatedByName { get; set; }

        public DateTime CurrSysTime { get; set; }

        public int CurrencyID { get; set; }

        public bool DSDOrder { get; set; }

        public bool DeletedOrder { get; set; }

        public decimal DiscountAmount { get; set; }

        public int DiscountType { get; set; }

        public bool Distribution_Center { get; set; }

        public int EXEWarehouse { get; set; }

        public int EinvoiceID { get; set; }

        public bool EinvoiceRequired { get; set; }

        public bool Electronic_Order { get; set; }

        public bool Email_Order { get; set; }

        public DateTime Expected_Date { get; set; }

        public bool Fax_Order { get; set; }

        public decimal Freight3Party_OrderCost { get; set; }

        public bool FromQueue { get; set; }

        public int From_SubTeam_Unrestricted { get; set; }

        public bool HFM_Store { get; set; }

        public decimal InvoiceAmount { get; set; }

        public decimal InvoiceCost { get; set; }

        public DateTime InvoiceDate { get; set; }

        public decimal InvoiceFreight { get; set; }

        public string InvoiceNumber { get; set; }

        public bool IsEXEDistributed { get; set; }

        public bool IsVendorExternal { get; set; }

        public int ItemsReceived { get; set; }

        public bool Manufacturer { get; set; }

        public string Notes { get; set; }

        public DateTime OrderDate { get; set; }

        public string OrderEnd { get; set; }

        public decimal OrderFreight { get; set; }

        public int OrderHeader_ID { get; set; }

        public List<IrmaMobile.Legacy.OrderItem> OrderItems { get; set; }

        public int OrderType_Id { get; set; }

        public decimal OrderedCost { get; set; }

        public DateTime OriginalCloseDate { get; set; }

        public int OriginalOrder_ID { get; set; }

        public bool OverrideTransmissionMethod { get; set; }

        public DateTime POCostDate { get; set; }

        public int POTransmissionTypeID { get; set; }

        public string PSAddressSequence { get; set; }

        public string PSLocationCode { get; set; }

        public string PSVendorID { get; set; }

        public bool PartialShipment { get; set; }

        public bool PayByAgreedCost { get; set; }

        public int ProductType_ID { get; set; }

        public int PurchaseLocation_ID { get; set; }

        public decimal QuantityDiscount { get; set; }

        public int ReceiveLocation_ID { get; set; }

        public bool ReceivingStore_Distribution_Center { get; set; }

        public int RecvLog_No { get; set; }

        public int RefuseReceivingReasonID { get; set; }

        public IrmaMobile.Legacy.Result ResultObject { get; set; }

        public int ReturnOrder_ID { get; set; }

        public bool Return_Order { get; set; }

        public bool Sent { get; set; }

        public DateTime SentDate { get; set; }

        public string ShipToStoreCompanyName { get; set; }

        public int ShipToStoreNo { get; set; }

        public string StoreCompanyName { get; set; }

        public int Store_No { get; set; }

        public string Store_Phone { get; set; }

        public bool Store_Vend { get; set; }

        public string SubTeam_Name { get; set; }

        public int SubteamInvoiceNumber { get; set; }

        public int SupplyTransferToSubTeam { get; set; }

        public string SupplyType_SubTeamName { get; set; }

        public int Temperature { get; set; }

        public int To_SubTeam_Unrestricted { get; set; }

        public decimal TotalHandlingCharge { get; set; }

        public int Transfer_SubTeam { get; set; }

        public int Transfer_To_SubTeam { get; set; }

        public string Transfer_To_SubTeamName { get; set; }

        public decimal UploadedCost { get; set; }

        public DateTime UploadedDate { get; set; }

        public int User_ID { get; set; }

        public DateTime VendorDocDate { get; set; }

        public string VendorDocID { get; set; }

        public int Vendor_ID { get; set; }

        public int Vendor_Store_No { get; set; }

        public bool WFM { get; set; }

        public bool WFM_Store { get; set; }

        public DateTime WarehouseCancelled { get; set; }

        public bool WarehouseSent { get; set; }

        public DateTime WarehouseSentDate { get; set; }

        public bool isDropShipment { get; set; }
    }
}
