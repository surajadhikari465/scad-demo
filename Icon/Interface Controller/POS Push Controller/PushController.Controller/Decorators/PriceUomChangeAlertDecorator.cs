using Icon.Common.Email;
using Icon.Framework;
using Icon.Logging;
using InterfaceController.Common;
using PushController.Common;
using PushController.Common.Models;
using PushController.Controller.CacheHelpers;
using PushController.Controller.MessageBuilders;
using PushController.DataAccess.Interfaces;
using PushController.DataAccess.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PushController.Controller.Decorators
{
    public class PriceUomChangeAlertDecorator : IMessageBuilder<MessageQueuePrice>
    {
        private IMessageBuilder<MessageQueuePrice> messageBuilder;
        private IEmailClient emailClient;
        private IQueryHandler<GetItemPricesByPushDataQuery, List<ItemPriceModel>> getItemPricesQueryHandler;
        private IPriceUomChangeConfiguration configuration;
        private ICacheHelper<int, Locale> localeCacheHelper;
        private ICacheHelper<string, ScanCodeModel> scanCodeCacheHelper;
        private ILogger<PriceUomChangeAlertDecorator> logger;
        private bool sendUomEmails;

        public PriceUomChangeAlertDecorator(
            IEmailClient emailClient,
            IQueryHandler<GetItemPricesByPushDataQuery, List<ItemPriceModel>> getItemPricesQueryHandler,
            IPriceUomChangeConfiguration configuration,
            ICacheHelper<int, Locale> localeCacheHelper,
            ICacheHelper<string, ScanCodeModel> scanCodeCacheHelper,
            ILogger<PriceUomChangeAlertDecorator> logger,
            IMessageBuilder<MessageQueuePrice> messageBuilder)
        {
            this.messageBuilder = messageBuilder;
            this.emailClient = emailClient;
            this.getItemPricesQueryHandler = getItemPricesQueryHandler;
            this.configuration = configuration;
            this.localeCacheHelper = localeCacheHelper;
            this.scanCodeCacheHelper = scanCodeCacheHelper;
            this.logger = logger;

            // Determines if sending of these UOM Change Emails is turned on or off.
            this.sendUomEmails = this.configuration.SendEmails;
        }

        public List<MessageQueuePrice> BuildMessages(List<IRMAPush> posDataReadyForEsb)
        {
            List<MessageQueuePrice> messages = this.messageBuilder.BuildMessages(posDataReadyForEsb);

            if (!sendUomEmails)
            {
                return messages;
            }

            try
            {
                var queryData = new GetItemPricesByPushDataQuery { IrmaPushList = posDataReadyForEsb };
                List<ItemPriceModel> itemPrices = getItemPricesQueryHandler.Execute(queryData);

                var uomChangedItems = new List<PriceUomChangeModel>();
                foreach (IRMAPush posDataRow in posDataReadyForEsb)
                {
                    Locale locale = localeCacheHelper.Retrieve(posDataRow.BusinessUnit_ID);
                    ScanCodeModel scanCodeModel = scanCodeCacheHelper.Retrieve(posDataRow.Identifier);

                    ItemPriceModel itemPriceModel = itemPrices.SingleOrDefault(ip => 
                        ip.LocaleId == locale.localeID 
                        && ip.ItemId == scanCodeModel.ItemId
                        && ip.ItemPriceTypeId == ItemPriceTypes.Reg);

                    if (itemPriceModel != null)
                    {
                        if ((posDataRow.Sold_By_Weight && itemPriceModel.UomId != UOMs.Pound) || (!posDataRow.Sold_By_Weight && itemPriceModel.UomId == UOMs.Pound))
                        {
                            var uomChange = new PriceUomChangeModel(posDataRow);
                            uomChange.CurrentPosUom = itemPriceModel.UomId == UOMs.Pound ? "LB" : "EA";
                            uomChangedItems.Add(uomChange);
                        }
                    }
                }

                if (uomChangedItems.Count > 0)
                {
                    emailClient.SetRecipients(this.configuration.PriceUomChangeRecipients.Split(','));

                    string emailSubject = this.configuration.PriceUomChangeSubject;
                    string emailBody = EmailHelper.BuildMessageBodyForPriceUomChanges(uomChangedItems);

                    emailClient.Send(emailBody, emailSubject);    
                }
            }
            catch (Exception ex)
            {
                var exceptionHandler = new ExceptionHandler<PriceUomChangeAlertDecorator>(this.logger);
                string message = "A failure occurred while attempting to send the alert email for Price UOM Changes.  Price Messages will still be created.";
                exceptionHandler.HandleException(message, ex, this.GetType(), MethodBase.GetCurrentMethod());
            }

            return messages;
        }
    }
}
