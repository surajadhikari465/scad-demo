using Icon.Dashboard.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Icon.Dashboard.Mvc.ViewModels
{
    public class IconLogEntryCollectionViewModel
    {
        public string AppName { get; set; }
        public string Title { get; set; }

        public LogErrorLevelEnum ErrorLevel { get; set; }

        public IEnumerable<IconLogEntryViewModel> LogEntries { get; set; }

        public PaginationPageSetViewModel PaginationModel { get; set; }

        public RecentLogEntriesReportCollectionViewModel RecentLogEntriesReport { get; set; }
    }
}