using Icon.Common.DataAccess;
using System;

namespace Icon.Monitoring.DataAccess.Queries
{
    public class GetMessageArchiveParameters : IQuery<int>
    {
        public DateTime LastMonitorDate { get; set; }
    }
}