using Dapper;
using Icon.Common.DataAccess;
using System.Data;

namespace Services.Extract.DataAccess.Commands
{
    public class UpdateJobLastRunEndCommandHandler : ICommandHandler<UpdateJobLastRunEndCommand>
    {
        private IDbConnection connection;

        public UpdateJobLastRunEndCommandHandler(IDbConnection connection)
        {
            this.connection = connection;
        }

        public void Execute(UpdateJobLastRunEndCommand data)
        {
            connection.Execute(@"
                UPDATE app.JobSchedule
                SET LastRunEndDateTimeUtc = @LastRunEndDateTimeUtc
                WHERE JobScheduleId = @JobScheduleId",
                new
                {
                    LastRunEndDateTimeUtc = data.LastRunEndDateTime,
                    data.JobScheduleId
                });
        }
    }
}
