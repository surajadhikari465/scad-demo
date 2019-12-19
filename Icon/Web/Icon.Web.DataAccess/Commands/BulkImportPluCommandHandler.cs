using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Logging;
using Icon.Web.Common;
using Icon.Web.DataAccess.Extensions;
using Icon.Web.DataAccess.Models;
using System;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;

namespace Icon.Web.DataAccess.Commands
{
    public class BulkImportPluCommandHandler : ICommandHandler<BulkImportCommand<BulkImportPluModel>>
    {
        private readonly IconContext context;
        private ILogger logger;

        public BulkImportPluCommandHandler(ILogger logger, IconContext context)
        {
            this.context = context;
            this.logger = logger;
        }

        public void Execute(BulkImportCommand<BulkImportPluModel> data)
        {
            SqlParameter itemList = new SqlParameter("ItemList", SqlDbType.Structured);
            SqlParameter userName = new SqlParameter("userName", SqlDbType.NVarChar);
            itemList.TypeName = "app.PLUMapImportType";
            itemList.Value = data.BulkImportData.ToDataTable();
            userName.Value = data.UserName;

            // Stored Procedure to be Executed for Bulk Import
            string sql = "EXEC app.PLUMapImport @ItemList, @userName";

            using (DbContextTransaction transaction = context.Database.BeginTransaction())
            {
                try
                {
                    context.Database.ExecuteSqlCommand(sql, itemList, userName);
                    transaction.Commit();
                    logger.Info(String.Format("User {0} mapped {1} national PLUs through PLU bulk mapping.", data.UserName, data.BulkImportData.Count));
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new CommandException("The CommandHandler threw an exception.", ex);
                }
            }
        }
    }
}
