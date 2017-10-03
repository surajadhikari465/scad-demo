using Dapper;
using Icon.Common.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSupport.DataAccess.Commands
{
    public class DeleteJobScheduleCommandHandler : ICommandHandler<DeleteJobScheduleCommand>
    {
        private IDbConnection connection;

        public DeleteJobScheduleCommandHandler(IDbConnection connection)
        {
            this.connection = connection;
        }

        public void Execute(DeleteJobScheduleCommand data)
        {
            connection.Execute(
                @"DELETE app.JobSchedule WHERE JobScheduleId = @JobScheduleId", 
                data);
        }
    }
}
