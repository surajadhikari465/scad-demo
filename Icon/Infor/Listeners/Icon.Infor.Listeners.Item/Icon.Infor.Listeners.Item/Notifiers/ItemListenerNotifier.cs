using Esb.Core.EsbServices;
using Icon.Common.Email;
using Icon.Esb.Services.ConfirmationBod;
using Icon.Esb.Subscriber;
using Icon.Infor.Listeners.Item.Constants;
using Icon.Infor.Listeners.Item.Models;
using System.Collections.Generic;
using System.Text;

namespace Icon.Infor.Listeners.Item.Notifiers
{
    public class ItemListenerNotifier : IItemListenerNotifier
    {
        private const string WfmTenantId = "WFM";
        private IEmailClient emailClient;
        private ItemListenerSettings settings;
        private IEsbService<ConfirmationBodEsbRequest> confirmationBodeEsbService;

        public ItemListenerNotifier(
            IEmailClient emailClient,
            ItemListenerSettings settings,
            IEsbService<ConfirmationBodEsbRequest> confirmationBodeEsbService)
        {
            this.emailClient = emailClient;
            this.settings = settings;
            this.confirmationBodeEsbService = confirmationBodeEsbService;
        }

        public void NotifyOfItemError(IEsbMessage message, bool schemaErrorOccurred, List<ItemModel> itemModelsWithErrors)
        {
            if (settings.EnableConfirmBods)
            {
                SendConfirmationBod(message, schemaErrorOccurred, itemModelsWithErrors);
            }
            if (itemModelsWithErrors.Count > 0)
            {
                StringBuilder builder = new StringBuilder();
                builder.AppendLine("The following items from Infor had errors when adding or updating the items in Icon.")
                    .Append("<br /><br />");

                builder.AppendFormat("<b>Infor Message ID:</b> {0}", message.GetProperty("IconMessageID"))
                    .Append("<br /><br />");

                foreach (var item in itemModelsWithErrors)
                {
                    builder
                        .AppendFormat("    <b>Item ID:</b> {0}", item.ItemId)
                        .Append("<br />")
                        .AppendFormat("    <b>Scan Code:</b> {0}", item.ScanCode)
                        .Append("<br />")
                        .AppendFormat("    <b>Error Code:</b> {0}", item.ErrorCode)
                        .Append("<br />")
                        .AppendFormat("    <b>Error Details:</b> {0}", item.ErrorDetails)
                        .Append("<br /><br />");
                }
                emailClient.Send(builder.ToString(), "Infor Item Listener: Item Errors");
            }
        }

        private void SendConfirmationBod(IEsbMessage message, bool schemaErrorOccurred, List<ItemModel> itemModelsWithErrors)
        {
            var messageId = message.GetProperty("IconMessageID");
            if (schemaErrorOccurred)
            {
                ConfirmationBodEsbRequest request = new ConfirmationBodEsbRequest
                {
                    BodId = messageId,
                    ErrorDescription = ApplicationErrors.Messages.UnableToParseMessage,
                    ErrorReasonCode = ApplicationErrors.Codes.UnableToParseMessage,
                    ErrorType = ConfirmationBodEsbErrorTypes.Schema,
                    EsbMessageProperties = new Dictionary<string, string>
                    {
                        { "IconMessageID", messageId }
                    },
                    OriginalMessage = message.MessageText,
                    TenantId = WfmTenantId
                };
                confirmationBodeEsbService.Send(request);
            }
            else
            {
                foreach (var item in itemModelsWithErrors)
                {
                    ConfirmationBodEsbRequest request = new ConfirmationBodEsbRequest
                    {
                        BodId = messageId,
                        ErrorDescription = item.ErrorDetails,
                        ErrorReasonCode = item.ErrorCode,
                        ErrorType = GetErrorType(item),
                        EsbMessageProperties = new Dictionary<string, string>
                        {
                            { "IconMessageID", messageId }
                        },
                        OriginalMessage = message.MessageText,
                        TenantId = WfmTenantId
                    };
                    confirmationBodeEsbService.Send(request);
                }
            }
        }

        private ConfirmationBodEsbErrorTypes GetErrorType(ItemModel item)
        {
            if(item.ErrorCode == ApplicationErrors.Codes.ItemAddOrUpdateError 
                || item.ErrorCode == ApplicationErrors.Codes.GenerateItemMessagesError)
            {
                return ConfirmationBodEsbErrorTypes.DatabaseConstraint;
            }
            else
            {
                return ConfirmationBodEsbErrorTypes.Data;
            }
        }
    }
}
