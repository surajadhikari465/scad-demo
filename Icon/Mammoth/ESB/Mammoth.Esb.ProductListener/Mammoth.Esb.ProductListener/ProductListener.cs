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
using System.Linq;

namespace Mammoth.Esb.ProductListener
{
    public class ProductListener : ListenerApplication<ProductListener, ListenerApplicationSettings>
    {
        private int processCounter;
        private IMessageParser<List<ItemModel>> messageParser;
        private IHierarchyClassIdMapper hierarchyClassIdMapper;
        private ICommandHandler<AddOrUpdateProductsCommand> addOrUpdateProductsCommandHandler;
        private AddOrUpdateProductsCommand addOrUpdateProductsCommand;
		private ICommandHandler<DeleteProductsExtendedAttributesCommand> deleteProductsExtendedAttrCommandHandler;
		private DeleteProductsExtendedAttributesCommand deleteProductsExtendedAttrCommand;

        public ProductListener(ListenerApplicationSettings listenerApplicationSettings,
            EsbConnectionSettings esbConnectionSettings,
            IEsbSubscriber subscriber,
            IEmailClient emailClient,
            ILogger<ProductListener> logger,
            IMessageParser<List<ItemModel>> messageParser,
            IHierarchyClassIdMapper hierarchyClassIdMapper,
            ICommandHandler<AddOrUpdateProductsCommand> addOrUpdateProductsCommandHandler,
			ICommandHandler<DeleteProductsExtendedAttributesCommand> deleteProductsExtendedAttrCommandHandler)
            : base(listenerApplicationSettings, esbConnectionSettings, subscriber, emailClient, logger)
        {
            this.messageParser = messageParser;
            this.hierarchyClassIdMapper = hierarchyClassIdMapper;
            this.addOrUpdateProductsCommandHandler = addOrUpdateProductsCommandHandler;
            this.addOrUpdateProductsCommand = new AddOrUpdateProductsCommand();
			this.deleteProductsExtendedAttrCommandHandler = deleteProductsExtendedAttrCommandHandler;
			this.deleteProductsExtendedAttrCommand = new DeleteProductsExtendedAttributesCommand();
        }

        public override void HandleMessage(object sender, EsbMessageEventArgs args)
        {
            bool isSuccess = true;
            List<ItemModel> items = null;

            try
            {
                processCounter = 0;
                items = messageParser.ParseMessage(args.Message);
                isSuccess = ProcessItems(items, args);
            }
            catch (Exception e)
            {
                isSuccess = false;
                LogAndNotifyErrorWithMessage(e, args);
            }

            AcknowledgeMessage(args);
            logger.Info($"{(isSuccess ? "Successfully" : "Error(s) in")} processed Message ID '{args.Message.GetProperty("IconMessageID")}'. Process times: {processCounter}.");
        }

        bool ProcessItems(List<ItemModel> items, EsbMessageEventArgs args)
        {
            bool isOK = true;

            if(items != null && items.Any())
            {
                try
                {
                    processCounter++;
                    hierarchyClassIdMapper.PopulateHierarchyClassDatabaseIds(items.Select(i => i.GlobalAttributes));
                    addOrUpdateProductsCommand.Items = items;
                    addOrUpdateProductsCommandHandler.Execute(addOrUpdateProductsCommand);

					deleteProductsExtendedAttrCommand.Items = items;
					deleteProductsExtendedAttrCommandHandler.Execute(deleteProductsExtendedAttrCommand);

					if(!String.IsNullOrWhiteSpace(addOrUpdateProductsCommand.TraitCodeWarning))
					{
						logger.Warn($"Invalid extended attribute code(s): {addOrUpdateProductsCommand.TraitCodeWarning}");
					}
                }
                catch (Exception e)
                {
                    isOK = false;
                    LogAndNotifyErrorWithMessage(e, args);
                    
                    if(items.Count > 1)
                    { 
                        foreach(var item in items)
                        {
                            ProcessItems(new List<ItemModel>(){ item }, args);
                        }
                    }
                }
            }

            return isOK;
        }
    }
}
