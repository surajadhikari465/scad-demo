using Icon.Common.Email;
using Icon.Esb;
using Icon.Esb.ListenerApplication;
using Icon.Esb.MessageParsers;
using Icon.Esb.Subscriber;
using Icon.Logging;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Esb.ProductListener.Commands;
using Mammoth.Esb.ProductListener.Mappers;
using Mammoth.Esb.ProductListener.Models;
using System;
using System.Collections.Generic;

namespace Mammoth.Esb.ProductListener
{
    public class ProductListener : ListenerApplication<ProductListener, ListenerApplicationSettings>
    {
        private IMessageParser<List<ProductModel>> messageParser;
        private IHierarchyClassIdMapper hierarchyClassIdMapper;
        private ICommandHandler<AddOrUpdateProductsCommand> addOrUpdateProductsCommandHandler;
        private AddOrUpdateProductsCommand addOrUpdateProductsCommand;

        public ProductListener(ListenerApplicationSettings listenerApplicationSettings,
            EsbConnectionSettings esbConnectionSettings,
            IEsbSubscriber subscriber,
            IEmailClient emailClient,
            ILogger<ProductListener> logger,
            IMessageParser<List<ProductModel>> messageParser,
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
            List<ProductModel> products = null;
            try
            {
                products = messageParser.ParseMessage(args.Message);
            }
            catch (Exception e)
            {
                LogAndNotifyErrorWithMessage(e, args);
            }

            if (products != null)
            {
                try
                {
                    hierarchyClassIdMapper.PopulateHierarchyClassDatabaseIds(products);
                    addOrUpdateProductsCommand.Products = products;
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
