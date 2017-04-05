using Icon.Common.Email;
using Icon.Esb;
using Icon.Esb.ListenerApplication;
using Icon.Esb.MessageParsers;
using Icon.Esb.Subscriber;
using Icon.Infor.Listeners.Price.Constants;
using Icon.Infor.Listeners.Price.Models;
using Icon.Infor.Listeners.Price.Services;
using Icon.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Infor.Listeners.Price
{
    public class PriceListener : ListenerApplication<PriceListener, ListenerApplicationSettings>
    {
        private IMessageParser<IEnumerable<PriceModel>> messageParser;
        private IService<PriceModel> service;

        public PriceListener(
            IMessageParser<IEnumerable<PriceModel>> messageParser,
            IService<PriceModel> service,
            ListenerApplicationSettings listenerApplicationSettings, 
            EsbConnectionSettings esbConnectionSettings, 
            IEsbSubscriber subscriber, 
            IEmailClient emailClient, 
            ILogger<PriceListener> logger) : base(listenerApplicationSettings, esbConnectionSettings, subscriber, emailClient, logger)
        {
            this.messageParser = messageParser;
            this.service = service;
        }

        public override void HandleMessage(object sender, EsbMessageEventArgs args)
        {
            IEnumerable<PriceModel> prices = null;

            try
            {
                prices = messageParser.ParseMessage(args.Message);

                if(prices.Any())
                {
                    service.Process(prices, args.Message);
                }
            }
            catch(Exception ex)
            {
                logger.Error(JsonConvert.SerializeObject(
                    new
                    {
                        ErrorCode = Errors.Codes.UnexpectedError,
                        MessageId = args.Message.GetProperty("MessageID"),
                        Message = args.Message.MessageText,
                        Error = ex
                    }));
            }
            finally
            {
                AcknowledgeMessage(args);
            }
        }
    }
}
