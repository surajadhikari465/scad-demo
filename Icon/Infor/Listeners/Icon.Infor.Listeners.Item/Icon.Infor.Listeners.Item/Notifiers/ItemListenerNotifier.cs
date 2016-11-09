using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Icon.Esb.Subscriber;
using Icon.Infor.Listeners.Item.Models;
using Icon.Common.Email;

namespace Icon.Infor.Listeners.Item.Notifiers
{
    public class ItemListenerNotifier : IItemListenerNotifier
    {
        private IEmailClient emailClient;

        public ItemListenerNotifier(IEmailClient emailClient)
        {
            this.emailClient = emailClient;
        }

        public void NotifyOfItemError(IEsbMessage message, List<ItemModel> itemModelsWithErrors)
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
