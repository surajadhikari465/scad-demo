using Icon.Dashboard.CommonDatabaseAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Dashboard.MammothDatabaseAccess
{
    public class AppLogSummaryQueryResult : IAppLogSummary
    {
        public AppLogSummaryQueryResult() { }

        public AppLogSummaryQueryResult(string appName, int appID, TimeSpan timePeriod, LoggingLevel logLevel = LoggingLevel.Error)
            : this()
        {
            AppName = appName;
            AppID = appID;
            DefinitionOfRecent = timePeriod;
            LogLevel = logLevel;
        }

        public string AppName { get; set; }
        public int AppID { get; set; }
        public int? LogEntryCount { get; set; }
        public LoggingLevel LogLevel { get; set; }
        public TimeSpan DefinitionOfRecent { get; set; }
    }
}
