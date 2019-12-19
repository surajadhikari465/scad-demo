using System;

namespace Icon.Web.Mvc.Models
{
    public class FailedItemLocaleMessageViewModel
    {
        public int Id { get; set; }
        public string RegionCode { get; set; }
        public int BusinessUnit_ID { get; set; }
        public string ScanCode { get; set; }
        public DateTime InsertDate { get; set; }
        public DateTime? ProcessedDate { get; set; }
    }
}