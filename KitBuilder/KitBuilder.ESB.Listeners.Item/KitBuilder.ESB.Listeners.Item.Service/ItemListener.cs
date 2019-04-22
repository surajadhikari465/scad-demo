using Icon.Common.Email;
using Icon.Esb;
using Icon.Esb.ListenerApplication;
using Icon.Esb.MessageParsers;
using Icon.Esb.Subscriber;
using Icon.Logging;
using KitBuilder.ESB.Listeners.Item.Service.Constants;
using KitBuilder.ESB.Listeners.Item.Service.Models;
using KitBuilder.ESB.Listeners.Item.Service.Notifiers;
using KitBuilder.ESB.Listeners.Item.Service.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KitBuilder.ESB.Listeners.Item.Service
{
    public class ItemListener : ListenerApplication<ItemListener, ListenerApplicationSettings>
    {
        private IMessageParser<IEnumerable<ItemModel>> messageParser;
        private IItemService service;
        private IItemListenerNotifier notifier;
        private List<ItemModel> models;

        public ItemListener(
            IMessageParser<IEnumerable<ItemModel>> messageParser,
            IItemService service,
            ListenerApplicationSettings listenerApplicationSettings,
            EsbConnectionSettings esbConnectionSettings,
            IEsbSubscriber subscriber,
            IEmailClient emailClient,
            IItemListenerNotifier notifier,
            ILogger<ItemListener> logger) : base(listenerApplicationSettings, esbConnectionSettings, subscriber, emailClient, logger)
        {
            this.messageParser = messageParser;
            this.service = service;
            this.notifier = notifier;
            this.models = new List<ItemModel>();
        }

        public override void HandleMessage(object sender, EsbMessageEventArgs args)
        {
            bool schemaErrorOccurred = false;
            try
            {
                models = messageParser.ParseMessage(args.Message).ToList();
                if (models.Any())
                {
                    service.AddOrUpdateOrRemoveItems(models);
                    ArchiveMessage(args.Message);
                }
            }
            catch (Exception ex)
            {
                LogAndNotifyErrorWithMessage(ex, args);
                schemaErrorOccurred = ex.Message == ApplicationErrors.Messages.UnableToParseMessage;
            }
            finally
            {
                models.Clear();
            }
        }

        private void ArchiveMessage(IEsbMessage message)
        {
            try
            {
                service.ArchiveMessage(message);
            }
            catch (Exception ex)
            {
                logger.Error(JsonConvert.SerializeObject(
                        new
                        {
                            ErrorCode = ApplicationErrors.Codes.UnableToArchiveMessage,
                            ErrorDetails = ApplicationErrors.Messages.UnableToArchiveMessage,
                            Message = message,
                            Exception = ex
                        }));
            }
        }

        private void NotifyItemErrors(IEsbMessage message, bool schemaErrorOccurred, List<ItemModel> models)
        {
            notifier.NotifyOfItemError(message, schemaErrorOccurred, models.Where(m => m.ErrorCode != null).ToList());
        }
    }
}
