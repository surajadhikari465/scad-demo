using System;

namespace IrmaMobile.Domain.Models
{
    public class UpdateOrderBeforeClosingModel
    {
        public int OrderId { get; set; }    
        public string InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal InvoiceCost { get; set; }
        public string VendorDocId { get; set; }
        public DateTime VendorDocDate { get; set; }
        public int SubteamNo { get; set; }
        public bool PartialShipment { get; set; }
    }
}