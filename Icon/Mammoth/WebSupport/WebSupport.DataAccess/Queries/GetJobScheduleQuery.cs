using Dapper;
using Icon.Common.DataAccess;
using System.Data;
using System.Linq;
using WebSupport.DataAccess.Models;

namespace WebSupport.DataAccess.Queries
{
    public class GetJobScheduleQuery : IQueryHandler<GetJobScheduleParameters, JobSchedule>
    {
        private IDbConnection connection;

        public GetJobScheduleQuery(IDbConnection connection)
        {
            this.connection = connection;
        }

        public JobSchedule Search(GetJobScheduleParameters parameters)
        {
            return connection.Query<JobSchedule>(@"
                    SELECT [JobScheduleId]
                          ,[JobName]
                          ,[Region]
                          ,[DestinationQueueName]
                          ,[StartDateTimeUtc]
                          ,[LastRunDateTimeUtc]
                          ,[IntervalInSeconds]
                          ,[Enabled]
                          ,[XmlObject]
                      FROM [app].[JobSchedule]
                      WHERE JobScheduleId = @JobScheduleId",
                      new { JobScheduleId = parameters.JobScheduleId }).First();
        }
    }
}
