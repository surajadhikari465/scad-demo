using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOS.Model
{
    public class SummarySpecification : ISummarySpecification
    {
        private DateTime startDate;
        private DateTime endDate;

        public SummarySpecification(DateTime startDate, DateTime endDate)
        {
            this.startDate = startDate;
            this.endDate = endDate;
        }

        public OOSSummary SatisfyingSummaryFrom(ISummaryRepository repository)
        {
            return repository.OOSSummaryFor(startDate, endDate);
        }
    }
}
