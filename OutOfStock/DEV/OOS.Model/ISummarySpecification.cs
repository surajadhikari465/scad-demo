using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OOS.Model.Repository;

namespace OOS.Model
{
    public interface ISummarySpecification
    {
        OOSSummary SatisfyingSummaryFrom(ISummaryRepository repository);
    }
}
