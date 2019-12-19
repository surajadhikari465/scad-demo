using System;

namespace OutOfStock.Messages
{
    [Serializable]
    public class KnownUploadItem
    {
        public string Upc;
        public string Vin;
        public string ReasonCode;
        public DateTime StartDate;
        public string ProductStatus;
        public DateTime? ExpirationDate;
    }
}
