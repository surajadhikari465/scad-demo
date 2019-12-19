using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OOS.Model;
using OOSCommon;
using OutOfStock.Models;

namespace OutOfStock.Service
{
    public class SummaryReportService
    {
        private ISummaryRepositoryFactory repositoryFactory;
        private ISummaryReportAdapter summaryAdapter;

        public SummaryReportService(ISummaryRepositoryFactory repositoryFactory, ISummaryReportAdapter summaryAdapter)
        {
            this.repositoryFactory = repositoryFactory;
            this.summaryAdapter = summaryAdapter;
        }

        public IEnumerable<SummaryReportViewModel> SummaryReportFor(DateTime startDate, DateTime endDate, string regionAbbrev)
        {
            var repository = repositoryFactory.New(regionAbbrev);

            if (repository == null)
                return new List<SummaryReportViewModel>();

            var spec = new SummarySpecification(startDate, endDate);
            var oosSummary = repository.SelectSummarySatisfying(spec);

            return summaryAdapter.Adapt(oosSummary);
        }
    }
}