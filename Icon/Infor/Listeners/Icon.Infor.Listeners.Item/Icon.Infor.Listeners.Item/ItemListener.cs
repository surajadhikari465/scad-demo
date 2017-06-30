﻿using Icon.Esb.ListenerApplication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Icon.Esb.Subscriber;
using Icon.Esb.MessageParsers;
using Icon.Common.Email;
using Icon.Esb;
using Icon.Logging;
using Icon.Infor.Listeners.Item.Models;
using Icon.Infor.Listeners.Item.Services;
using Icon.Infor.Listeners.Item.Validators;
using Icon.Framework;
using Icon.Common.Context;
using Newtonsoft.Json;
using Icon.Infor.Listeners.Item.Constants;
using Icon.Infor.Listeners.Item.Notifiers;

namespace Icon.Infor.Listeners.Item
{
    public class ItemListener : ListenerApplication<ItemListener, ListenerApplicationSettings>
    {
        private IMessageParser<IEnumerable<ItemModel>> messageParser;
        private ICollectionValidator<ItemModel> itemValidator;
        private IItemService service;
        private IItemListenerNotifier notifier;
        private List<ItemModel> models;

        public ItemListener(
            IMessageParser<IEnumerable<ItemModel>> messageParser,
            ICollectionValidator<ItemModel> itemValidator,
            IItemService service,
            ListenerApplicationSettings listenerApplicationSettings,
            EsbConnectionSettings esbConnectionSettings,
            IEsbSubscriber subscriber,
            IEmailClient emailClient,
            IItemListenerNotifier notifier,
            ILogger<ItemListener> logger) : base(listenerApplicationSettings, esbConnectionSettings, subscriber, emailClient, logger)
        {
            this.messageParser = messageParser;
            this.itemValidator = itemValidator;
            this.service = service;
            this.notifier = notifier;
            this.models = new List<ItemModel>();
        }

        public override void HandleMessage(object sender, EsbMessageEventArgs args)
        {
            try
            {

                models = messageParser.ParseMessage(args.Message).ToList();
                if (models.Any())
                {
                    itemValidator.ValidateCollection(models);
                    service.AddOrUpdateItems(models);
                    service.GenerateItemMessages(models);
                }
            }
            catch (Exception ex)
            {
                LogAndNotifyErrorWithMessage(ex, args);
            }
            finally
            {
                ArchiveMessage(args.Message);
                ArchiveItems(models);
                NotifyItemErrors(args.Message, models);

                AcknowledgeMessage(args);
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

        private void ArchiveItems(List<ItemModel> models)
        {
            try
            {
                if (models.Any())
                {
                    service.ArchiveItems(models);
                }
            }
            catch(Exception ex)
            {
                logger.Error(JsonConvert.SerializeObject(
                        new
                        {
                            ErrorCode = ApplicationErrors.Codes.UnableToArchiveItems,
                            ErrorDetails = ApplicationErrors.Messages.UnableToArchiveItems,
                            Items = models,
                            Exception = ex
                        }));
            }
        }

        private void NotifyItemErrors(IEsbMessage message, List<ItemModel> models)
        {
            notifier.NotifyOfItemError(message, models.Where(m => m.ErrorCode != null).ToList());
        }
    }
}
