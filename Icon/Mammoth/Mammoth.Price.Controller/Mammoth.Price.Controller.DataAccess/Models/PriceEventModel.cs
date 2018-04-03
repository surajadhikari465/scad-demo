using System;

namespace Mammoth.Price.Controller.DataAccess.Models
{
    public class PriceEventModel
    {
        public int QueueId { get; set; }
        public int EventTypeId { get; set; }
        public string Region { get; set; }
        public string ScanCode { get; set; }
        public int BusinessUnitId { get; set; }
        public int NewRegularMultiple { get; set; }
        public decimal NewRegularPrice { get; set; }
        public DateTime NewStartDate { get; set; }
        public DateTime? NewSaleEndDate { get; set; }
        public decimal? NewSalePrice { get; set; }
        public int? NewSaleMultiple { get; set; }
        public string NewPriceType { get; set; }
        public string PriceUom { get; set; }
        public string CurrencyCode { get; set; }
        public decimal CurrentRegularPrice { get; set; }
        public int CurrentRegularMultiple { get; set; }
        public decimal? CurrentSalePrice { get; set; }
        public int? CurrentSaleMultiple { get; set; }
        public DateTime? CurrentSaleStartDate { get; set; }
        public DateTime? CurrentSaleEndDate { get; set; }
        public string CurrentPriceType { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorDetails { get; set; }
        public string ErrorSource { get; set; }
    }
}
