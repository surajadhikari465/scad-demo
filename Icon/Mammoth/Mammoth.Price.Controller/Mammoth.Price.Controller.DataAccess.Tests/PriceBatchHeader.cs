using System;

namespace Mammoth.Price.Controller.DataAccess.Tests
{
    public class PriceBatchHeader
    {
        public int PriceBatchHeaderID { get; set; }
        public int PriceBatchStatusID { get; set; }
        public int? ItemChgTypeID { get; set; }
        public int? PriceChgTypeID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? PrintedDate { get; set; }
        public DateTime? SentDate { get; set; }
        public DateTime? ProcessedDate { get; set; }
        public int? POSBatchID { get; set; }
        public string BatchDescription { get; set; }
        public bool AutoApplyFlag { get; set; }
        public DateTime? ApplyDate { get; set; }
    }
}
