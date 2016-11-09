using Icon.Dashboard.CommonDatabaseAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Dashboard.IconDatabaseAccess
{
    public class ApiJobSummaryReport : IApiJobSummary
    {
        public ApiJobSummaryReport() { }

        public ApiJobSummaryReport(string messageType, DateTime startTime, DateTime endTime)
            : this()
        {
            this.MessageType = messageType;
            this.StartTime = startTime;
            this.EndTime = endTime;
        }

        public string MessageType { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public long CountProcessedMessages { get; set; }
        public long CountFailedMessages { get; set; }
    }
}
