using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Dashboard.CommonDatabaseAccess
{
    public interface IAppLog
    {
        int AppLogID { get; set; }
        int AppID { get; set; }
        string UserName { get; set; }
        DateTime InsertDate { get; set; }
        DateTime LoggingTimestamp { get; }
        string Level { get; set; }
        string Logger { get; set; }
        string Message { get; set; }
        string MachineName { get; set; }
        string AppName { get; }

    }
}
