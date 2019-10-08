using Dapper;
using Mammoth.Common.DataAccess.CommandQuery;
using PrimeAffinityController.Models;
using System.Data;
using System.Linq;

namespace PrimeAffinityController.Queries
{
    public class GetJobScheduleQuery : IQueryHandler<GetJobScheduleParameters, JobScheduleModel>
    {
        private IDbConnection dbConnection;
        private PrimeAffinityControllerSettings settings;

        public GetJobScheduleQuery(IDbConnection dbConnection, PrimeAffinityControllerSettings settings)
        {
            this.dbConnection = dbConnection;
            this.settings = settings;
        }

        public JobScheduleModel Search(GetJobScheduleParameters parameters)
        {
            return dbConnection.Query<JobScheduleModel>(
                @"  UPDATE app.JobSchedule
                    SET InstanceId = @InstanceId
                    WHERE JobName = @JobName
                        AND Region = @Region
                        AND InstanceId IS NULL
                       
                    SELECT 
                        JobScheduleId
                        ,JobName
                        ,Region
                        ,DestinationQueueName
                        ,StartDateTimeUtc
                        ,LastScheduledDateTimeUtc
                        ,LastRunEndDateTimeUtc
                        ,NextScheduledDateTimeUtc
                        ,IntervalInSeconds
                        ,Enabled
                        ,Status
                        ,XmlObject
                        ,RunAdHoc
                        ,InstanceId
                    FROM app.JobSchedule 
                    WHERE JobName = @JobName 
                        AND Region = @Region",
                new
                {
                    JobName = parameters.JobName,
                    Region = parameters.Region,
                    InstanceId = settings.InstanceId
                })
                .FirstOrDefault();
        }
    }
}
