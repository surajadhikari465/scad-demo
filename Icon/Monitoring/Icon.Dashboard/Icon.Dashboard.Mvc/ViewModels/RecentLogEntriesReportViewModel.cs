using Icon.Dashboard.CommonDatabaseAccess;
using Icon.Dashboard.Mvc.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Icon.Dashboard.Mvc.ViewModels
{
    public class RecentLogEntriesReportViewModel
    {
        public const int DefaultHoursForRecent = 24;

        public RecentLogEntriesReportViewModel() 
            : this(LoggingLevel.Error, DefaultHoursForRecent)
        {
        }

        public RecentLogEntriesReportViewModel(LoggingLevel logLevel = LoggingLevel.Error, int hoursConsideredRecent = DefaultHoursForRecent)
        {
            Level = LoggingLevel.Error;
            DefinitionOfRecent = new TimeSpan(DefaultHoursForRecent, 0, 0);
        }

        public RecentLogEntriesReportViewModel(IAppLogSummary logEntryReport)
        {
            this.AppID = logEntryReport.AppID;
            this.Name = logEntryReport.AppName;
            this.Level = logEntryReport.LogLevel;
            this.DefinitionOfRecent = logEntryReport.DefinitionOfRecent;
            this.LogEntryCount = logEntryReport.LogEntryCount;
        }

        public RecentLogEntriesReportViewModel(IconAppViewModel iconApp) : this()
        {
            this.AppID = iconApp.AppID;
            this.Name = iconApp.AppName;
        }

        //public IconApplicationRecentLogEntriesReportViewModel(IconApplicationViewModel iconApplicationDefinition)
        //{
        //    if (iconApplicationDefinition != null)
        //    {
        //        if (iconApplicationDefinition.DatabaseId.HasValue)
        //        {
        //            this.AppID = iconApplicationDefinition.DatabaseId.Value;
        //        }
        //        this.Name = iconApplicationDefinition.Name;
        //    }
        //}

        public int AppID { get; set; }
        
        public string Name { get; set; }

        public LoggingLevel Level { get; set; }

        public TimeSpan DefinitionOfRecent { get; set; }
        public int? LogEntryCount { get; set; }

        public string GetBootstrapClassForLevel()
        {
            return Utils.GetBootstrapClassForLevel(this.Level.ToString());
        }
    }
}