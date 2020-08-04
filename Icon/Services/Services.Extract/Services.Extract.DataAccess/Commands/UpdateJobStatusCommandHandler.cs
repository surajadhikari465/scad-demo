using Dapper;
using Icon.Common.DataAccess;
using System.Data;

namespace Services.Extract.DataAccess.Commands
{
    public class UpdateJobStatusCommandHandler : ICommandHandler<UpdateJobStatusCommand>
    {
        private IDbConnection connection;

        public UpdateJobStatusCommandHandler(IDbConnection connection)
        {
            this.connection = connection;
        }

        public void Execute(UpdateJobStatusCommand data)
        {
            connection.Execute(@"
                UPDATE app.JobSchedule
                SET Status = @Status
                WHERE JobScheduleId = @JobScheduleId",
                data);
        }
    }
}
