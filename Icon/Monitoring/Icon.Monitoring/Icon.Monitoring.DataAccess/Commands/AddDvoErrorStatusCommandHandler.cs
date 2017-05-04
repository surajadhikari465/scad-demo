using Dapper;
using Icon.Common.DataAccess;

namespace Icon.Monitoring.DataAccess.Commands
{
    public class AddDvoErrorStatusCommandHandler : ICommandHandler<AddDvoErrorStatusCommand>
    {
        private const string FailedStatus = "FAILED";
        private IDbProvider db;

        public AddDvoErrorStatusCommandHandler(IDbProvider db)
        {
            this.db = db;
        }

        public void Execute(AddDvoErrorStatusCommand data)
        {
            var sql = @"insert into monitor.JobStatus(JobName, Status, Region) 
                        select @JobName, 
                               @Status,
                               @Region
                        where not exists 
                            (
                                select * from monitor.JobStatus where JobName = @JobName and Region = @Region
                            )";

            db.Connection.Execute(
                sql: sql,
                param: new { Region = data.Region, Status = FailedStatus, JobName = Common.Constants.CustomJobNames.DvoJobName },
                transaction: db.Transaction);
        }
    }
}
