using Dapper;
using Mammoth.Common.DataAccess.CommandQuery;
using System.Data;

namespace PrimeAffinityController.Commands
{
    public class ClearInstanceIdFromJobScheduleCommandHandler : ICommandHandler<ClearInstanceIdFromJobScheduleCommand>
    {
        private IDbConnection dbConnection;
        private PrimeAffinityControllerSettings settings;

        public ClearInstanceIdFromJobScheduleCommandHandler(IDbConnection dbConnection, PrimeAffinityControllerSettings settings)
        {
            this.dbConnection = dbConnection;
            this.settings = settings;
        }

        public void Execute(ClearInstanceIdFromJobScheduleCommand data)
        {
            dbConnection.Execute(
                @"  UPDATE app.JobSchedule
                    SET InstanceId = NULL
                    WHERE JobScheduleId = @JobScheduleId
                        AND InstanceId = @InstanceId",
                new
                {
                    JobScheduleId = data.JobSchedule.JobScheduleId,
                    InstanceId = settings.InstanceId
                });
        }
    }
}