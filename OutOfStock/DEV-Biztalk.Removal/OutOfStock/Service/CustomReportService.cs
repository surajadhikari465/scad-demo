using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OOS.Model;
using OutOfStock.Models;

namespace OutOfStock.Service
{
    public class CustomReportService
    {
        private CustomReportRepository repository;
        private CustomReportAdapter adapter;

        public CustomReportService(CustomReportRepository repository, CustomReportAdapter adapter)
        {
            this.repository = repository;
            this.adapter = adapter;
        }

        public IEnumerable<CustomReportViewModel> CustomReportFor(DateTime startDate, DateTime endDate, List<int> storeIds, List<string> teams, List<string> subteams)
        {
            var report = repository.For(startDate, endDate, storeIds, teams, subteams);
            return adapter.Adapt(report);
        }
    }
}