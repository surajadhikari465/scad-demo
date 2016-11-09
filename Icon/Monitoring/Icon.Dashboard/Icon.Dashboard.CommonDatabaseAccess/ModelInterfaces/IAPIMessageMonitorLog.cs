using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Dashboard.CommonDatabaseAccess
{
    public interface IAPIMessageMonitorLog
    {
        int APIMessageMonitorLogID { get; set; }
        int MessageTypeID { get; set; }
        DateTime? StartTime { get; set; }
        DateTime? EndTime { get; set; }
        int? CountProcessedMessages { get; set; }
        int? CountFailedMessages { get; set; }
        string MessageTypeName { get; }
    }
}
