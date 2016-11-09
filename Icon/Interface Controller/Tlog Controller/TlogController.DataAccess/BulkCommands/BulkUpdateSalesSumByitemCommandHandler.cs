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
    public class BulkUpdateSalesSumByitemCommandHandler : IBulkCommandHandler<BulkUpdateSalesSumByitemCommand>
    {
        private ILogger<BulkUpdateSalesSumByitemCommandHandler> logger;
        private IrmaContext context;

        public BulkUpdateSalesSumByitemCommandHandler(ILogger<BulkUpdateSalesSumByitemCommandHandler> logger, IrmaContext context)
        {
            this.logger = logger;
            this.context = context;
        }

        public int Execute(BulkUpdateSalesSumByitemCommand command)
        {
            int returnVal = 0;
            if (command.ItemMovementsToIrma == null || command.ItemMovementsToIrma.Count == 0)
            {
                logger.Warn("BulkUpdateSalesSumByitemCommandHandler was called with a null or empty list.  Check execution logic in the calling method.");
                return returnVal;
            }

            SqlParameter messagesParameter = new SqlParameter("ItemMovement", SqlDbType.Structured);
            messagesParameter.TypeName = "dbo.ItemMovementType";

            messagesParameter.Value = command.ItemMovementsToIrma.ConvertAll(e => new
            {
                BusinessUnitId = e.BusinessUnitId,
                Identifier = e.Identifier,
                TransDate = e.TransDate.Date,
                Quantity = e.Quantity,
                ItemVoid = e.ItemVoid,
                ItemType = e.ItemType,
                BasePrice = e.BasePrice,
                Weight = e.Weight,
                MarkDownAmount = e.MarkdownAmount
            }).ToDataTable();

            string sql = "EXEC dbo.IconUpdateSalesSumByItem @ItemMovement";

            returnVal = context.Database.ExecuteSqlCommand(sql, messagesParameter);

            logger.Info(String.Format("Successfully updated {0} Sale_SumByItems entries from ItemMovement {1} to ItemMovement {2}. ",
                returnVal.ToString(), command.ItemMovementsToIrma[0].ItemMovementID, command.ItemMovementsToIrma[command.ItemMovementsToIrma.Count - 1].ItemMovementID));

            return returnVal;
        }
    }
}
