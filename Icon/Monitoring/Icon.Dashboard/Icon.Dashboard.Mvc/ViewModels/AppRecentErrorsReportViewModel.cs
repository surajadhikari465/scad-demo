using Icon.Dashboard.CommonDatabaseAccess;
using Icon.Dashboard.Mvc.Enums;
using Icon.Dashboard.Mvc.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Icon.Dashboard.Mvc.ViewModels
{
    public class AppRecentErrorsReportViewModel
    {
        public const int DefaultHoursForRecent = 24;

        public AppRecentErrorsReportViewModel()
            : this(LoggingLevel.Error, DefaultHoursForRecent)
        {
        }

        public AppRecentErrorsReportViewModel(LoggingLevel logLevel = LoggingLevel.Error, int hoursConsideredRecent = DefaultHoursForRecent)
        {
            Level = LoggingLevel.Error;
            ReportHours = DefaultHoursForRecent;
        }

        public AppRecentErrorsReportViewModel(IconOrMammothEnum system, IAppLogSummary logEntryReport)
            : this ()
        {
            this.System = system;
            this.AppID = logEntryReport.AppID;
            this.AppName = logEntryReport.AppName;
            if (string.IsNullOrWhiteSpace(logEntryReport.AppName))
            {
                this.ReportName = logEntryReport.AppName;
            }
            else
            {
                if (logEntryReport.AppName.Length < 20)
                {
                    // if the app name is short enough, append "Log" to the end of it
                    this.ReportName = $"{logEntryReport.AppName} Log";
                }
                else if (logEntryReport.AppName.Length > 22)
                {
                    // if the app name is too long, truncate it
                    this.ReportName = logEntryReport.AppName.Substring(0, 23);
                }
            }
            this.Level = logEntryReport.LogLevel;
            this.ReportHours = logEntryReport.DefinitionOfRecent.Hours;
            this.LogEntryCount = logEntryReport.LogEntryCount;
        }

        public AppRecentErrorsReportViewModel(LoggedAppViewModel iconApp) : this()
        {
            this.AppID = iconApp.AppID;
            this.ReportName = iconApp.AppName;
        }

        public IconOrMammothEnum System { get; set; }

        public int AppID { get; set; }

        public string AppName { get; set; }

        public string ReportName { get; set; }

        public LoggingLevel Level { get; set; }

        public int ReportHours { get; set; }

        public int? LogEntryCount { get; set; }

        //public string AppLoggerLinkController { get; set; }
        //public string AppLoggerLinkAction { get; set; }
        //public object AppLoggerLinkRouteValues { get; set; }

        public string LinkForIndividualAppLogger { get; set; }

        public string GetBootstrapClassForLevel()
        {
            return Utils.GetBootstrapClassForLevel(this.Level.ToString());
        }
    }
}