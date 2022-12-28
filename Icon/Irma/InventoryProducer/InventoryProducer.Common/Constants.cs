
namespace InventoryProducer.Common
{
    public static class Constants
    {
        public struct XmlNamespaces
        {
            public const string EnterpriseUnitOfMeasureMgmtUnitOfMeasure = "http://schemas.wfm.com/Enterprise/UnitOfMeasureMgmt/UnitOfMeasure/V2";
            public const string EnterpriseInventoryMgmtCommonRefTypes = "http://schemas.wfm.com/Enterprise/InventoryMgmt/CommonRefTypes/V1";
            public const string EnterpriseTransactionMgmtCommonRefTypes = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTypes/V1";
            public const string IrmaHoneyCrispEvents = "http://schemas.wfm.com/IrmaHoneycrisp/Events.xsd";
            public const string MammothErrorMessage = "http://schemas.wfm.com/Mammoth/ErrorMessage.xsd";
            public const string EnterpriseInventoryMgmtOrderReceipt = "http://schemas.wfm.com/Enterprise/InventoryMgmt/OrderReceipt/V1";
        }

        public struct EventType
        {
            public const string TSF_DEL = "TSF_DEL";
            public const string PO_DEL = "PO_DEL";
            public static string TSF_LINE_DEL = "TSF_LINE_DEL";
            public const string PO_LINE_DEL = "PO_LINE_DEL";
            public const string DEL = "DEL";
        }

        public struct MessageProperty
        {
            public const string TransactionID = "TransactionID";
            public const string MessageType = "MessageType";
            public const string NonReceivingSystems = "nonReceivingSysName";
            public const string MessageNumber = "MessageNumber";
            public const string TransactionType = "TransactionType";
            public const string Source = "Source";
            public const string RegionCode = "RegionCode";
        }

        public struct ProducerType
        {
            public const string Spoilage = "spoilage";
            public const string Transfer = "transfer";
            public const string PurchaseOrder = "purchaseorder";
            public const string Receive = "receive";
            public const string RepublishInventory = "republishinventory";
        }

        public struct TransactionType
        {
            public const string PurchaseOrders = "PurchaseOrders";
            public const string ReceiptOrders = "ReceiptOrders";
            public const string TransferOrders = "TransferOrders";
            public const string InventorySpoilage = "InventorySpoilage";
        }
    }
}
