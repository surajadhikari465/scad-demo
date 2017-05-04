using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infor.Services.NewItem.Models;
using Icon.Logging;
using Infor.Services.NewItem.Infrastructure;
using Infor.Services.NewItem.Constants;
using Newtonsoft.Json;

namespace Infor.Services.NewItem.Notifiers
{
    public class NewItemNotifier : INewItemNotifier
    {
        private HashSet<string> notificationErrorCodes = new HashSet<string>
        {
            ApplicationErrors.Codes.InvalidBrand,
            ApplicationErrors.Codes.InvalidTaxClassCode,
            ApplicationErrors.Codes.InvalidNationalClassCode,
            ApplicationErrors.Codes.InvalidProductDescription,
            ApplicationErrors.Codes.InvalidPosDescription,
            ApplicationErrors.Codes.InvalidRetailUom
        };
        private NewItemNotifierSettings settings;
        private IRegionalEmailClientFactory regionalEmailClientFactory;
        private ILogger<NewItemNotifier> logger;

        public NewItemNotifier(
            NewItemNotifierSettings settings,
            IRegionalEmailClientFactory regionalEmailClientFactory,
            ILogger<NewItemNotifier> logger)
        {
            this.settings = settings;
            this.regionalEmailClientFactory = regionalEmailClientFactory;
            this.logger = logger;
        }

        public void NotifyOfNewItemError(IEnumerable<NewItemModel> newItemModels)
        {
            if (newItemModels != null && newItemModels.Any())
            {
                var errorItemGroups = newItemModels
                    .Where(i => i.ErrorCode != null && notificationErrorCodes.Contains(i.ErrorCode))
                    .GroupBy(i => i.Region);

                foreach (var itemGroup in errorItemGroups)
                {
                    try
                    {
                        if (settings.RegionalNotificationEnabled[itemGroup.Key])
                        {
                            var emailClient = regionalEmailClientFactory.CreateEmailClient(itemGroup.Key);

                            string builder = BuildMessage(itemGroup);

                            emailClient.Send(builder.ToString(), "Infor New Item Service: New Item Errors");

                            logger.Info(JsonConvert.SerializeObject(new
                            {
                                Message = "Sent error notification for items with errors.",
                                Region = itemGroup.Key,
                                Items = itemGroup.ToList(),
                            }));
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Error(JsonConvert.SerializeObject(new
                        {
                            Message = "Error occurred when attempting to notify regions of item errors.",
                            Region = itemGroup.Key,
                            Items = itemGroup.ToList(),
                            Error = ex
                        }));
                    }
                }
            }
        }

        private static string BuildMessage(IGrouping<string, NewItemModel> group)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("The following items from IRMA had errors when attempting to send them to Infor. These items will not flow to Infor for Global Data Team validation until the error is fixed.")
                .Append("<br /><br />");

            foreach (var item in group)
            {
                builder.AppendFormat("    <b>Region:</b> {0}", item.Region)
                    .Append("<br />")
                    .AppendFormat("    <b>Scan Code:</b> {0}", item.ScanCode)
                    .Append("<br />")
                    .AppendFormat("    <b>Sent Timestamp:</b> {0}", item.QueueInsertDate)
                    .Append("<br />")
                    .AppendFormat("    <b>Error Code:</b> {0}", item.ErrorCode)
                    .Append("<br />")
                    .AppendFormat("    <b>Error Details:</b> {0}", item.ErrorDetails)
                    .Append("<br /><br />");
            }

            return builder.ToString();
        }
    }
}
