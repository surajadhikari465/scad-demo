using Dapper;
using Icon.Common.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Monitoring.DataAccess.Commands
{
    public class DeleteDvoErrorStatusCommandHandler : ICommandHandler<DeleteDvoErrorStatusCommand>
    {
        private IDbProvider db;

        public DeleteDvoErrorStatusCommandHandler(IDbProvider db)
        {
            this.db = db;
        }

        public void Execute(DeleteDvoErrorStatusCommand data)
        {
            var sql = "delete from monitor.JobStatus where Region = @Region and JobName = @JobName";

            db.Connection.Execute(
                sql: sql,
                param: new { Region = data.Region, JobName = Common.Constants.CustomJobNames.DvoJobName },
                transaction: db.Transaction);
        }
    }
}
