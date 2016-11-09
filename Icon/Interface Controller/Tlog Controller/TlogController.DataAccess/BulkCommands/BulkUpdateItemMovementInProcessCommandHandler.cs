using Icon.Framework;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using TlogController.DataAccess.Interfaces;

namespace TlogController.DataAccess.BulkCommands
{
    public class BulkUpdateItemMovementInProcessCommandHandler : IQueryHandler<BulkUpdateItemMovementInProcessCommand, List<ItemMovement>>
    {
        private readonly IconContext context;

        public BulkUpdateItemMovementInProcessCommandHandler(IconContext context)
        {
            this.context = context;
            context.Database.CommandTimeout = 300;
        }

        public List<ItemMovement> Execute(BulkUpdateItemMovementInProcessCommand parameters)
        {
            SqlParameter maxTransaction = new SqlParameter("MaxTransaction", SqlDbType.Int);
            maxTransaction.Value = parameters.MaxTransaction;

            SqlParameter instance = new SqlParameter("Instance", SqlDbType.NVarChar);
            instance.Value = parameters.Instance;

            string sql = @"EXEC app.UpdateItemMovementInProcess @MaxTransaction, @Instance";

            DbRawSqlQuery<ItemMovement> tlog = this.context.Database.SqlQuery<ItemMovement>(sql, maxTransaction, instance);
            List<ItemMovement> queuedTlogs = new List<ItemMovement>(tlog);
            return queuedTlogs;
        }
    }
}
