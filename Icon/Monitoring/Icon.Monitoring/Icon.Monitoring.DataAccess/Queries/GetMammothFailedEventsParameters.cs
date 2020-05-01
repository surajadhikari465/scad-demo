using Icon.Common.DataAccess;
using Icon.Monitoring.DataAccess.Model;
using System;
using System.Collections.Generic;

namespace Icon.Monitoring.DataAccess.Queries
{
    public class GetMammothFailedEventsParameters : IQuery<List<MammothFailedEvent>>
    {
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
