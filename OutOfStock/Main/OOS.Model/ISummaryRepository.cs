using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOS.Model
{
    public interface ISummaryRepository
    {
        OOSSummary SelectSummarySatisfying(ISummarySpecification spec);
        OOSSummary OOSSummaryFor(DateTime startDate, DateTime endDate);
    }
}
