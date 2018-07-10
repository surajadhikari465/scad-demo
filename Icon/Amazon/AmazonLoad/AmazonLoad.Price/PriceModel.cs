using System;

namespace AmazonLoad.Price
{
    public class PriceModel
    {
        public DateTime? EndDate { get; set; }
        public DateTime StartDate { get; set; }
        public int Multiple { get; set; }
        public decimal Price { get; set; }
        public string SubPriceTypeCode { get; set; }
        public string PriceTypeCode { get; set; }
        public string CurrencyCode { get; set; }
        public string UomCode { get; set; }
        public string LocaleName { get; set; }
        public int BusinessUnitId { get; set; }
        public string ItemTypeDesc { get; set; }
        public string ItemTypeCode { get; set; }
        public int ItemId { get; set; }
        public string ScanCode { get; set; }
    }
}
