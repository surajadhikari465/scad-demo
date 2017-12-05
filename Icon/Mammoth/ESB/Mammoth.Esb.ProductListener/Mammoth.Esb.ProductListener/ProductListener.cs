using Icon.Esb;
using Icon.Esb.ListenerApplication;
using Icon.Esb.MessageParsers;
using Icon.Esb.Subscriber;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Esb.ProductListener.Commands;
using Mammoth.Esb.ProductListener.Mappers;
using Mammoth.Esb.ProductListener.Models;
using System;
using System.Collections.Generic;
using Icon.Logging;
using Icon.Common.Email;
using System.Linq;

namespace Mammoth.Esb.ProductListener
{
    public class ProductListener : ListenerApplication<ProductListener, ListenerApplicationSettings>
    {
        private IMessageParser<List<ItemModel>> messageParser;
        private IHierarchyClassIdMapper hierarchyClassIdMapper;
        private ICommandHandler<AddOrUpdateProductsCommand> addOrUpdateProductsCommandHandler;
        private AddOrUpdateProductsCommand addOrUpdateProductsCommand;

        public ProductListener(ListenerApplicationSettings listenerApplicationSettings,
            EsbConnectionSettings esbConnectionSettings,
            IEsbSubscriber subscriber,
            IEmailClient emailClient,
            ILogger<ProductListener> logger,
            IMessageParser<List<ItemModel>> messageParser,
            IHierarchyClassIdMapper hierarchyClassIdMapper,
            ICommandHandler<AddOrUpdateProductsCommand> addOrUpdateProductsCommandHandler)
            : base(listenerApplicationSettings, esbConnectionSettings, subscriber, emailClient, logger)
        {
            this.messageParser = messageParser;
            this.hierarchyClassIdMapper = hierarchyClassIdMapper;
            this.addOrUpdateProductsCommandHandler = addOrUpdateProductsCommandHandler;
            addOrUpdateProductsCommand = new AddOrUpdateProductsCommand();
        }

        public override void HandleMessage(object sender, EsbMessageEventArgs args)
        {
            List<ItemModel> items = null;
            try
            {
                items = messageParser.ParseMessage(args.Message);
            }
            catch (Exception e)
            {
                LogAndNotifyErrorWithMessage(e, args);
            }

            if (items != null)
            {
                try
                {
                    hierarchyClassIdMapper.PopulateHierarchyClassDatabaseIds(items.Select(i => i.GlobalAttributes));
                    addOrUpdateProductsCommand.Items = items;
                    addOrUpdateProductsCommandHandler.Execute(addOrUpdateProductsCommand);
                }
                catch (Exception e)
                {
                    LogAndNotifyErrorWithMessage(e, args);
                }
            }

            AcknowledgeMessage(args);

            logger.Info(String.Format("Successfully processed Message ID '{0}'.", args.Message.GetProperty("IconMessageID")));
        }
    }
}
