using Dapper;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Common.DataAccess.DbProviders;
using System;

namespace MammothWebApi.DataAccess.Commands
{
    public class DeleteStagingCommandHandler : ICommandHandler<DeleteStagingCommand>
    {
        private IDbProvider db;

        public DeleteStagingCommandHandler(IDbProvider db)
        {
            this.db = db;
        }

        public void Execute(DeleteStagingCommand data)
        {
            string sql = String.Format(@"DELETE FROM stage.{0} WHERE TransactionId = @TransactionId", data.StagingTableName);
            int affectedRows = this.db.Connection.Execute(sql, new { TransactionId = data.TransactionId }, transaction: this.db.Transaction);
        }
    }
}
