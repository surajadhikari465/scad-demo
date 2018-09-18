using System;

namespace AmazonLoad.MammothPrice
{
    public class PriceModelGpm
    {
        public string Region { get; set; }
        public Guid? GpmId { get; set; }
        public int ItemId { get; set; }
        public string ItemTypeCode { get; set; }
        public string ItemTypeDesc { get; set; }
        public string ScanCode { get; set; }
        public int BusinessUnitId { get; set; }
        public string LocaleName { get; set; }
        public decimal Price { get; set; }
        public string PriceType { get; set; }
        public string PriceTypeId { get; set; }
        public string PriceTypeDesc { get; set; }
        public string SubPriceTypeCode { get; set; }
        public string SubPriceTypeId { get; set; }
        public string SubPriceTypeDesc { get; set; }
        public int Multiple { get; set; }
        public string CurrencyCode { get; set; }
        public string SellableUOM { get; set; }
        public string UomName { get; set; }
        public decimal? PercentOff { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public DateTime? TagExpirationDate { get; set; }
        public DateTime InertDateUtc { get; set; }
        public DateTime? ModifiedDateUtc { get; set; }
    }
}
