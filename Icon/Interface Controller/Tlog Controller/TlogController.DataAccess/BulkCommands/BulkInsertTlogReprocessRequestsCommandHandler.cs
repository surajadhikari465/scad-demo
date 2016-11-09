using Icon.Logging;
using Irma.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using TlogController.DataAccess.Interfaces;
using TlogController.Common;

namespace TlogController.DataAccess.BulkCommands
{
    public class BulkInsertTlogReprocessRequestsCommandHandler : IBulkCommandHandler<BulkInsertTlogReprocessRequestsCommand>
    {
        private ILogger<BulkInsertTlogReprocessRequestsCommandHandler> logger;
        private IrmaContext context;

        public BulkInsertTlogReprocessRequestsCommandHandler(ILogger<BulkInsertTlogReprocessRequestsCommandHandler> logger, IrmaContext context)
        {
            this.logger = logger;
            this.context = context;
        }

        public int Execute(BulkInsertTlogReprocessRequestsCommand command)
        {
            int returnVal = 0;
            if (command.TlogReprocessRequests == null || command.TlogReprocessRequests.Count == 0)
            {
                logger.Warn("BulkInsertTlogReprocessRequestsCommandHandler was called with a null or empty list.  Check execution logic in the calling method.");
                return returnVal;
            }

            SqlParameter messagesParameter = new SqlParameter("ReprocessRequest", SqlDbType.Structured);
            messagesParameter.TypeName = "dbo.TlogReprocessRequestType";

            messagesParameter.Value = command.TlogReprocessRequests.ConvertAll(e => new
            {
                BusinessUnit_ID = e.BusinessUnit_ID,
                TransDate = e.Date_Key
            }).ToDataTable();

            string sql = "EXEC dbo.IconInsertTlogReprocessRequest @ReprocessRequest";

            returnVal = context.Database.ExecuteSqlCommand(sql, messagesParameter);

            logger.Info(String.Format("Successfully insert {0} TlogReprocessRequest entries.",
                returnVal.ToString()));

            return returnVal;
        }
    }
}
