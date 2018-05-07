using Dapper;
using Icon.Common.DataAccess;
using System.Data;

namespace WebSupport.DataAccess.Commands
{
    public class StartJobAddHocCommandHandler : ICommandHandler<StartJobAddHocCommand>
    {
        private IDbConnection connection;

        public StartJobAddHocCommandHandler(IDbConnection connection)
        {
            this.connection = connection;
        }

        public void Execute(StartJobAddHocCommand data)
        {
            connection.Execute(@"
                UPDATE app.JobSchedule
                   SET RunAdHoc = 1,
                       Status = 'ready'
                WHERE JobScheduleId = @JobScheduleId
                    AND Status <> 'running'",
                data.JobSchedule);
        }
    }
}