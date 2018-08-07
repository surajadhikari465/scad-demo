using System;

namespace MammothWebApi.DataAccess.Models
{
    public partial class PricesGpm
    {
        public string Region { get; set; }
        public int ItemID { get; set; }
        public int BusinessUnitID { get; set; }
        public Guid GpmID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal Price { get; set; }
        public decimal? PercentOff { get; set; }
        public string PriceType { get; set; }
        public string PriceTypeAttribute { get; set; }
        public string SellableUOM { get; set; }
        public string CurrencyCode { get; set; }
        public int Multiple { get; set; }
        public DateTime? TagExpirationDate { get; set; }
        public DateTime InsertDateUtc { get; set; }
        public DateTime? ModifiedDateUtc { get; set; }
    }
}
