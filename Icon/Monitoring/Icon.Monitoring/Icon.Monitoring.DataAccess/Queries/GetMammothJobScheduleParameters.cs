using Icon.Common.DataAccess;
using Icon.Monitoring.DataAccess.Model;

namespace Icon.Monitoring.DataAccess.Queries
{
    public class GetMammothJobScheduleParameters : IQuery<JobSchedule>
    {
        public string JobName { get; set; }
        public string Region { get; set; }
    }
}
