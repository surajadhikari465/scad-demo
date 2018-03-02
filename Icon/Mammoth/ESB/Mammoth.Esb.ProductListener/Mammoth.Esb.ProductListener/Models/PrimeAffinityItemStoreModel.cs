using System;

namespace Mammoth.Esb.ProductListener.Models
{
    public class PrimeAffinityItemStoreModel
    {
        public string Region { get; set; }
        public int ItemId { get; set; }
        public int BusinessUnitId { get; set; }
        public string ScanCode { get; set; }
        public string ItemTypeCode { get; set; }
        public string StoreName { get; set; }
        public string PriceType { get; set; }
        public decimal? Price { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
