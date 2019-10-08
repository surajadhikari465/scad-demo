using System;

namespace PrimeAffinityController.Models
{
    public class PrimeAffinityPsgPriceModel
    {
        public string MessageAction { get; set; }
        public string Region { get; set; }
        public int PriceID { get; set; }
        public int ItemID { get; set; }
        public int BusinessUnitID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal Price { get; set; }
        public string PriceType { get; set; }
        public string PriceUOM { get; set; }
        public int CurrencyID { get; set; }
        public int Multiple { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ScanCode { get; set; }
        public string ItemTypeCode { get; set; }
        public string StoreName { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorDetails { get; set; }
    }
}
