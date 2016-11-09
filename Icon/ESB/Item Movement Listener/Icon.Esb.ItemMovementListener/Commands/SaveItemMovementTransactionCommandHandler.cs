using Icon.Common.DataAccess;
using Icon.Framework;
using System.Data;
using System.Data.SqlClient;

namespace Icon.Esb.ItemMovementListener.Commands
{
    public class SaveItemMovementTransactionCommandHandler : ICommandHandler<SaveItemMovementTransactionCommand>
    {
        public void Execute(SaveItemMovementTransactionCommand data)
        {
            #region SQL to process item movement mesage
            string insertItemMovementSql = "EXEC app.InsertItemMovement @ItemMovementTransactions";

            SqlParameter itemMovementTransactionsSql = new SqlParameter("ItemMovementTransactions", SqlDbType.Structured)
            {
                TypeName = "app.ItemMovementType",
                Value = Utility.ToDataTable(data.ItemMovementTransactions)
            };
            #endregion

            using (var context = new IconContext())
            {
                context.Database.ExecuteSqlCommand(insertItemMovementSql, itemMovementTransactionsSql);
            }
        }
    }
}
