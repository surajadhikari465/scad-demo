using Icon.Common.Email;
using Icon.Esb.Subscriber;
using KitBuilder.ESB.Listeners.Item.Service.Models;
using System.Collections.Generic;
using System.Text;

namespace KitBuilder.ESB.Listeners.Item.Service.Notifiers
{
    public class ItemListenerNotifier : IItemListenerNotifier
    {
        private const string WfmTenantId = "WFM";
        private IEmailClient emailClient;
        private ItemListenerSettings settings;

        public ItemListenerNotifier(IEmailClient emailClient,ItemListenerSettings settings)
        {
            this.emailClient = emailClient;
            this.settings = settings;
        }

        public void NotifyOfItemError(IEsbMessage message, bool schemaErrorOccurred, List<ItemModel> itemModelsWithErrors)
        {
         
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

       
    }
}
