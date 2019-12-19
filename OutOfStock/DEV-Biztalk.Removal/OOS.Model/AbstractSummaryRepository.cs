using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOS.Model
{
    public abstract class AbstractSummaryRepository : ISummaryRepository
    {
        public OOSSummary SelectSummarySatisfying(ISummarySpecification spec)
        {
            return spec.SatisfyingSummaryFrom(this);
        }



        public OOSSummary OOSSummaryFor(DateTime startDate, DateTime endDate)
        {
            var countSummary = OOSCountSummaryFor(startDate, endDate);
            var overlaySummary = Overlay(countSummary);
            var skuSummary = SKUSummaryBy();
            var scanSummary = NumberOfScansBy(startDate, endDate);
            return new OOSSummary(overlaySummary, skuSummary, scanSummary);
        }

        private OOSCountSummary Overlay(OOSCountSummary countSummary)
        {
            var regionSummary = OOSCountSummaryForOverlay();
            OOSCountSummary overlaySummary = regionSummary.Overlay(countSummary);
            return overlaySummary;
        }

        internal abstract OOSCountSummary OOSCountSummaryFor(DateTime startDate, DateTime endDate);
        internal abstract OOSCountSummary OOSCountSummaryForOverlay();
        internal abstract SKUSummary SKUSummaryBy();
        internal abstract ScanSummary NumberOfScansBy(DateTime startDate, DateTime endDate);
    }
}
