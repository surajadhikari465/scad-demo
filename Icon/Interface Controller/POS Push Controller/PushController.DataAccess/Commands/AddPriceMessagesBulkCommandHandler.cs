using Icon.RenewableContext;
using Icon.Framework;
using Icon.Logging;
using MoreLinq;
using PushController.DataAccess.Interfaces;
using System;
using System.Data;
using System.Data.SqlClient;

namespace PushController.DataAccess.Commands
{
    public class AddPriceMessagesBulkCommandHandler : ICommandHandler<AddPriceMessagesBulkCommand>
    {
        private ILogger<AddPriceMessagesBulkCommandHandler> logger;
        private IRenewableContext<IconContext> context;

        public AddPriceMessagesBulkCommandHandler(ILogger<AddPriceMessagesBulkCommandHandler> logger, IRenewableContext<IconContext> context)
        {
            this.logger = logger;
            this.context = context;
        }

        public void Execute(AddPriceMessagesBulkCommand command)
        {
            if (command.Messages == null || command.Messages.Count == 0)
            {
                logger.Warn("AddPriceMessagesBulkCommandHandler was called with a null or empty list.  Check execution logic in the calling method.");
                return;
            }

            SqlParameter messagesParameter = new SqlParameter("PriceMessages", SqlDbType.Structured);
            messagesParameter.TypeName = "app.PriceMessageType";

            messagesParameter.Value = command.Messages.ConvertAll(m => new
                {
                    MessageTypeId = m.MessageTypeId,
                    MessageStatusId = m.MessageStatusId,
                    MessageHistoryId = m.MessageHistoryId,
                    IRMAPushID = m.IRMAPushID,
                    InsertDate = m.InsertDate,
                    RegionCode = m.RegionCode,
                    BusinessUnit_ID = m.BusinessUnit_ID,
                    ItemId = m.ItemId,
                    ItemTypeCode = m.ItemTypeCode,
                    ItemTypeDesc = m.ItemTypeDesc,
                    LocaleId = m.LocaleId,
                    LocaleName = m.LocaleName,
                    ScanCodeId = m.ScanCodeId,
                    ScanCode = m.ScanCode,
                    ScanCodeTypeId = m.ScanCodeTypeId,
                    ScanCodeTypeDesc = m.ScanCodeTypeDesc,
                    ChangeType = m.ChangeType,
                    UomCode = m.UomCode,
                    UomName = m.UomName,
                    CurrencyCode = m.CurrencyCode,
                    Price = m.Price,
                    Multiple = m.Multiple,
                    SalePrice = m.SalePrice,
                    SaleMultiple = m.SaleMultiple,
                    SaleStartDate = m.SaleStartDate,
                    SaleEndDate = m.SaleEndDate,
                    PreviousSalePrice = m.PreviousSalePrice,
                    PreviousSaleMultiple = m.PreviousSaleMultiple,
                    PreviousSaleStartDate = m.PreviousSaleStartDate,
                    PreviousSaleEndDate = m.PreviousSaleEndDate,
                    InProcessBy = m.InProcessBy,
                    ProcessedDate = m.ProcessedDate
                }).ToDataTable();

            string sql = "EXEC app.GeneratePriceMessages @PriceMessages";

            context.Context.Database.ExecuteSqlCommand(sql, messagesParameter);

            logger.Info(String.Format("Successfully bulk inserted {0} Price message(s) beginning with IRMAPushID {1}.",
                command.Messages.Count, command.Messages[0].IRMAPushID));
        }
    }
}
