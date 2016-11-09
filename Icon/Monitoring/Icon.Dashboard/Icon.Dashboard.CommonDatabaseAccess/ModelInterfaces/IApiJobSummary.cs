using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Dashboard.CommonDatabaseAccess
{
    public interface IApiJobSummary
    {
        string MessageType { get; set; }
        DateTime StartTime { get; set; }
        DateTime EndTime { get; set; }
        long CountProcessedMessages { get; set; }
        long CountFailedMessages { get; set; }
    }
}
