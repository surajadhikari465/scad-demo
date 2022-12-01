using System;

namespace GPMService.Producer.Model.DBModel
{
    internal class GetExpiringTprsQueryModel
    {
        public long PriceID { get; set; }
        public string Region { get; set; }
        public string GpmID { get; set; }
        public int ItemID { get; set; }
        public int BusinessUnitID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal Price { get; set; }
        public string PriceType { get; set; }
        public string PriceTypeAttribute { get; set; }
        public string SellableUOM { get; set; }
        public string CurrencyCode { get; set; }
        public byte Multiple { get; set; }
        public DateTime? TagExpirationDate { get; set; }
        public DateTime InsertDateUtc { get; set; }
        public DateTime? ModifiedDateUtc { get; set; }
        public string ItemTypeCode { get; set; }
        public string ScanCode { get; set; }
        public string StoreName { get; set; }
        public int? SubTeamNumber { get; set; }
        public decimal? PercentOff { get; set; }
    }
}
