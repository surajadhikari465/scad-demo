using System;

namespace Icon.Web.Mvc.Models
{
    public class FailedPriceMessageViewModel
    {
        public int Id { get; set; }
        public string RegionCode { get; set; }
        public int BusinesUnit_ID { get; set; }
        public string LocaleName { get; set; }
        public string ScanCode { get; set; }
        public string ChangeType { get; set; }
        public decimal? Price { get; set; }
        public decimal? SalePrice { get; set; }
        public DateTime InsertDate { get; set; }
        public DateTime? ProcessedDate { get; set; }
    }
}