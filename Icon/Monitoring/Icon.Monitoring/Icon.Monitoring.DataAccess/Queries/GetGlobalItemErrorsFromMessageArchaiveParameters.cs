using Icon.Common.DataAccess;
using System;

namespace Icon.Monitoring.DataAccess.Queries
{
    public class GetGlobalItemErrorsFromMessageArchaiveParameters : IQuery<int>
    {
        public DateTime LastMonitorDate { get; set; }
    }
}