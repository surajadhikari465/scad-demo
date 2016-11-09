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
    public class AddItemLocaleMessagesBulkCommandHandler : ICommandHandler<AddItemLocaleMessagesBulkCommand>
    {
        private ILogger<AddItemLocaleMessagesBulkCommandHandler> logger;
        private IRenewableContext<IconContext> context;

        public AddItemLocaleMessagesBulkCommandHandler(ILogger<AddItemLocaleMessagesBulkCommandHandler> logger, IRenewableContext<IconContext> context)
        {
            this.logger = logger;
            this.context = context;
        }

        public void Execute(AddItemLocaleMessagesBulkCommand command)
        {
            if (command.Messages == null || command.Messages.Count == 0)
            {
                logger.Warn("AddItemLocaleMessagesBulkCommandHandler was called with a null or empty list.  Check execution logic in the calling method.");
                return;
            }

            SqlParameter messagesParameter = new SqlParameter("ItemLocaleMessages", SqlDbType.Structured);
            messagesParameter.TypeName = "app.ItemLocaleMessageType";

            messagesParameter.Value = command.Messages.ConvertAll(m => new
                {
                    MessageTypeId = m.MessageTypeId,
                    MessageStatusId = m.MessageStatusId,
                    MessageHistoryId = m.MessageHistoryId,
                    MessageActionId = m.MessageActionId,
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
                    LockedForSale = m.LockedForSale,
                    Recall = m.Recall,                    
                    TMDiscountEligible = m.TMDiscountEligible,
                    Case_Discount = m.Case_Discount,
                    AgeCode = m.AgeCode,
                    Restricted_Hours = m.Restricted_Hours,
                    Sold_By_Weight = m.Sold_By_Weight,
                    ScaleForcedTare = m.ScaleForcedTare,
                    Quantity_Required = m.Quantity_Required,
                    Price_Required = m.Price_Required,
                    QtyProhibit = m.QtyProhibit,
                    VisualVerify = m.VisualVerify,
                    LinkedItemScanCode = m.LinkedItemScanCode,
                    PreviousLinkedItemScanCode = m.PreviousLinkedItemScanCode,
                    PosScaleTare = m.PosScaleTare,
                    InProcessBy = m.InProcessBy,
                    ProcessedDate = m.ProcessedDate
                }).ToDataTable();

            string sql = "EXEC app.GenerateItemLocaleMessages @ItemLocaleMessages";

            context.Context.Database.ExecuteSqlCommand(sql, messagesParameter);

            logger.Info(String.Format("Successfully bulk inserted {0} ItemLocale message(s) beginning with IRMAPushID {1}.",
                command.Messages.Count, command.Messages[0].IRMAPushID));
        }
    }
}
