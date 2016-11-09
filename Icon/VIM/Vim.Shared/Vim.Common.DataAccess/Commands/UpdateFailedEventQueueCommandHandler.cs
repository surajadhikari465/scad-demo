using Dapper;

namespace Vim.Common.DataAccess.Commands
{
    public class UpdateFailedEventQueueCommandHandler : ICommandHandler<UpdateFailedEventQueueCommand>
    {
        private IDbProvider db;

        public UpdateFailedEventQueueCommandHandler(IDbProvider db)
        {
            this.db = db;
        }

        public void Execute(UpdateFailedEventQueueCommand data)
        {
            string sql = @"UPDATE vim.EventQueue
                            SET 
	                            ProcessedFailedDate = SYSDATETIME(),
                                InProcessBy = NULL,
	                            NumberOfRetry = ISNULL(NumberOfRetry,-1) + 1
                            WHERE 
                                QueueId IN @QueueIds;";

            int affectedRows = this.db.Connection.Execute(sql,
                new
                {
                    QueueIds = data.QueueIds
                },
                transaction: this.db.Transaction);
        }
    }
}
