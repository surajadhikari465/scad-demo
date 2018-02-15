using Dapper;
using Icon.Monitoring.DataAccess.Model;

namespace Icon.Monitoring.DataAccess.Queries
{
    public class GetMammothJobScheduleQuery : IQueryHandlerMammoth<GetMammothJobScheduleParameters, JobSchedule>
    {
        private IDbProvider db;

        public GetMammothJobScheduleQuery(IDbProvider db)
        {
            this.db = db;
        }

        public JobSchedule Search(GetMammothJobScheduleParameters parameters)
        {
            return db.Connection.QueryFirstOrDefault<JobSchedule>(
                @"  SELECT 
                        [JobScheduleId]
                        ,[JobName]
                        ,[Region]
                        ,[DestinationQueueName]
                        ,[StartDateTimeUtc]
                        ,[LastScheduledDateTimeUtc]
                        ,[LastRunEndDateTimeUtc]
                        ,[NextScheduledDateTimeUtc]
                        ,[IntervalInSeconds]
                        ,[Enabled]
                        ,[Status]
                        ,[XmlObject]
                    FROM app.JobSchedule 
                    WHERE JobName = @JobName 
                    AND Region = @Region",
                parameters);
        }
    }
}
