using System;

namespace OutOfStock.Messages
{
    [Serializable]
    public class ProductStatusEvent : Event
    {
        public string Region;
        public string VendorKey;
        public string Vin;
        public string Upc;
        public string Reason;
        public DateTime StartDate;
        public string ProductStatus;
        public DateTime ExpirationDate;
    }
}
