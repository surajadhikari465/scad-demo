using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Icon.Dashboard.Mvc.ViewModels
{
    public class RecentLogEntriesReportCollectionViewModel
    {
        public const int DefaultHoursForRecent = 24;
        public const int DefaulPollingIntervalSeconds = 60;

        public RecentLogEntriesReportCollectionViewModel()
            : this (DefaultHoursForRecent, DefaulPollingIntervalSeconds) { }

        public RecentLogEntriesReportCollectionViewModel(int hoursForRecent, int pollingInterval)
        {

            HoursConsideredRecent = hoursForRecent;
            PollingRefreshIntervalSeconds = pollingInterval;
            Reports = new List<RecentLogEntriesReportViewModel>();
        }

        public IEnumerable<RecentLogEntriesReportViewModel> Reports { get; set; }

        public int HoursConsideredRecent { get; set; }

        public int PollingRefreshIntervalSeconds { get; set; }
    }
}