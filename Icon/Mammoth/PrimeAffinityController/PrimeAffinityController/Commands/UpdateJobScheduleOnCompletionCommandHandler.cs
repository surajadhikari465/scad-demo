using Dapper;
using Mammoth.Common.DataAccess.CommandQuery;
using PrimeAffinityController.Constants;
using System;
using System.Data;

namespace PrimeAffinityController.Commands
{
    public class UpdateJobScheduleOnCompletionCommandHandler : ICommandHandler<UpdateJobScheduleOnCompletionCommand>
    {
        private IDbConnection dbConnection;

        public UpdateJobScheduleOnCompletionCommandHandler(IDbConnection dbConnection)
        {
            this.dbConnection = dbConnection;
        }

        public void Execute(UpdateJobScheduleOnCompletionCommand data)
        {
            if (data.JobSchedule.RunAdHoc.HasValue && data.JobSchedule.RunAdHoc.Value)
            {
                dbConnection.Execute(
                    @"  UPDATE app.JobSchedule
                        SET LastRunEndDateTimeUtc = @LastRunEndDateTimeUtc,
                            Status = @Status,
                            RunAdHoc = 0,
                            InstanceId = NULL
                        WHERE JobScheduleId = @JobScheduleId",
                    new
                    {
                        LastRunEndDateTimeUtc = DateTime.UtcNow,
                        Status = data.Status,
                        JobScheduleId = data.JobSchedule.JobScheduleId,
                    });
            }
            else
            {
                dbConnection.Execute(
                    @"  UPDATE app.JobSchedule
                        SET LastScheduledDateTimeUtc = NextScheduledDateTimeUtc,
                            NextScheduledDateTimeUtc = @NextScheduledDateTimeUtc,
                            LastRunEndDateTimeUtc = @LastRunEndDateTimeUtc,
                            Status = @Status,
                            RunAdHoc = 0,
                            InstanceId = NULL
                        WHERE JobScheduleId = @JobScheduleId",
                    new
                    {
                        NextScheduledDateTimeUtc = CalculateNextScheduledDateTimeUtc(data),
                        LastRunEndDateTimeUtc = DateTime.UtcNow,
                        Status = data.Status,
                        JobScheduleId = data.JobSchedule.JobScheduleId,
                    });
            }
        }

        private static DateTime CalculateNextScheduledDateTimeUtc(UpdateJobScheduleOnCompletionCommand data)
        {
            if (data.Status == ApplicationConstants.JobScheduleStatuses.Ready)
                return data.JobSchedule.NextScheduledDateTimeUtc.AddSeconds(data.JobSchedule.IntervalInSeconds);
            else
                return data.JobSchedule.NextScheduledDateTimeUtc;
        }
    }
}
