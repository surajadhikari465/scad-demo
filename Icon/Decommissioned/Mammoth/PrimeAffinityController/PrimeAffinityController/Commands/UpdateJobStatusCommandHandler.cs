using Dapper;
using Icon.Logging;
using Mammoth.Common.DataAccess.CommandQuery;
using System;
using System.Data;

namespace PrimeAffinityController.Commands
{
    public class UpdateJobStatusCommandHandler : ICommandHandler<UpdateJobStatusCommand>
    {
        private IDbConnection dbConnection;
        private ILogger<UpdateJobStatusCommandHandler> logger;

        public UpdateJobStatusCommandHandler(IDbConnection dbConnection, ILogger<UpdateJobStatusCommandHandler> logger)
        {
            this.dbConnection = dbConnection;
            this.logger = logger;
        }

        public void Execute(UpdateJobStatusCommand data)
        {
            try
            {
                dbConnection.Execute(
                    @"  UPDATE app.JobSchedule
                        SET Status = @Status
                        WHERE JobScheduleId = @JobScheduleId",
                    new
                    {
                        JobScheduleId = data.JobSchedule.JobScheduleId,
                        Status = data.Status
                    });
            }
            catch(Exception ex)
            {
                logger.Error(
                    new
                    {
                        Message = "Unable to update job schedule status",
                        Status = data.Status,
                        JobSchedule = data.JobSchedule,
                        Error = ex
                    }.ToJson());
                throw;
            }
        }
    }
}
