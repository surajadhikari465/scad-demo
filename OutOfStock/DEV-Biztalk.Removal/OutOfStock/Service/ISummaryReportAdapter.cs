using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OOS.Model;
using OutOfStock.Models;

namespace OutOfStock.Service
{
    public interface ISummaryReportAdapter
    {
        List<SummaryReportViewModel> Adapt(OOSSummary oosSummary);
    }
}
