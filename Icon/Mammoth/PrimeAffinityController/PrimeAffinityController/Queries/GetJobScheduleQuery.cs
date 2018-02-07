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

        public GetJobScheduleQuery(IDbConnection dbConnection)
        {
            this.dbConnection = dbConnection;
        }

        public JobScheduleModel Search(GetJobScheduleParameters parameters)
        {
            return dbConnection.Query<JobScheduleModel>(
                @"  SELECT * FROM app.JobSchedule 
                    WHERE JobName = @JobName 
                    AND Region = @Region",
                parameters)
                .FirstOrDefault();
        }
    }
}
