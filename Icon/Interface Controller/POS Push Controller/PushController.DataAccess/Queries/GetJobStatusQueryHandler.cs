using Irma.Framework;
using PushController.DataAccess.Interfaces;
using System.Linq;

namespace PushController.DataAccess.Queries
{
    public class GetJobStatusQueryHandler : IQueryHandler<GetJobStatusQuery, JobStatus>
    {
        public JobStatus Execute(GetJobStatusQuery parameters)
        {
            var jobStatus = (from job in parameters.Context.JobStatus
                             where job.Classname == parameters.JobName
                             select job).Single();

            return jobStatus;
        }
    }
}
