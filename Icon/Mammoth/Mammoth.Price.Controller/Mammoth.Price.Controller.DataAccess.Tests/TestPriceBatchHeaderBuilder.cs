using System;

namespace Mammoth.Price.Controller.DataAccess.Tests
{
    internal class TestPriceBatchHeaderBuilder
    {
        public int PriceBatchHeaderID;
        public int PriceBatchStatusID;
        public int? ItemChgTypeID;
        public int? PriceChgTypeID;
        public DateTime StartDate;
        public DateTime? PrintedDate;
        public DateTime? SentDate;
        public DateTime? ProcessedDate;
        public int? POSBatchID;
        public string BatchDescription;
        public bool AutoApplyFlag;
        public DateTime? ApplyDate;

        public TestPriceBatchHeaderBuilder()
        {
            this.PriceBatchHeaderID = 1;
            this.PriceBatchStatusID = 1;
            this.StartDate = DateTime.Today.AddDays(2);
            this.AutoApplyFlag = false;
            this.BatchDescription = "UnitTest Batch";
        }

        public TestPriceBatchHeaderBuilder WithStartDate(DateTime startDate)
        {
            this.StartDate = startDate;
            return this;
        }

        public PriceBatchHeader Build()
        {
            PriceBatchHeader header = new PriceBatchHeader();
            header.PriceBatchHeaderID = this.PriceBatchHeaderID;
            header.PriceBatchStatusID = this.PriceBatchStatusID;
            header.StartDate = this.StartDate;
            header.AutoApplyFlag = this.AutoApplyFlag;
            header.BatchDescription = this.BatchDescription;
            return header;
        }

        public static implicit operator PriceBatchHeader(TestPriceBatchHeaderBuilder builder)
        {
            return builder.Build();
        }
    }
}
