using Icon.Dashboard.CommonDatabaseAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Dashboard.CommonDatabaseAccess
{
    public interface IAppLogSummary
    {
        string AppName { get; set; }
        int AppID { get; set; }
        int? LogEntryCount { get; set; }
        LoggingLevel LogLevel { get; set; }
        TimeSpan DefinitionOfRecent { get; set; }
    }
}
