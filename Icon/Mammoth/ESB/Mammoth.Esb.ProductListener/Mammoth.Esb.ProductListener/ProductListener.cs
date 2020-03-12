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
        private ICommandHandler<MessageArchiveCommand> messageArchiveCommandHandler;
        private MessageArchiveCommand messageArchiveCommand;

        private const string NonReceivingSystemsProperty = "nonReceivingSysName";
        public ProductListener(ListenerApplicationSettings listenerApplicationSettings,
            EsbConnectionSettings esbConnectionSettings,
            IEsbSubscriber subscriber,
            IEmailClient emailClient,
            ILogger<ProductListener> logger,
            IMessageParser<List<ItemModel>> messageParser,
            IHierarchyClassIdMapper hierarchyClassIdMapper,
            ICommandHandler<AddOrUpdateProductsCommand> addOrUpdateProductsCommandHandler,
			ICommandHandler<DeleteProductsExtendedAttributesCommand> deleteProductsExtendedAttrCommandHandler,
            ICommandHandler<MessageArchiveCommand> messageArchiveCommandHandler)
            : base(listenerApplicationSettings, esbConnectionSettings, subscriber, emailClient, logger)
        {
            this.messageParser = messageParser;
            this.hierarchyClassIdMapper = hierarchyClassIdMapper;
            this.addOrUpdateProductsCommandHandler = addOrUpdateProductsCommandHandler;
            this.addOrUpdateProductsCommand = new AddOrUpdateProductsCommand();
			this.deleteProductsExtendedAttrCommandHandler = deleteProductsExtendedAttrCommandHandler;
			this.deleteProductsExtendedAttrCommand = new DeleteProductsExtendedAttributesCommand();
            this.messageArchiveCommandHandler = messageArchiveCommandHandler;
            this.messageArchiveCommand = new MessageArchiveCommand();
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
            if (!isSuccess)
            {
                try
                {
                    messageArchiveCommand.MessageId = new Guid().ToString();
                    List<string> lstnonReceivingSystemsProduct = new List<string>() { args.Message.GetProperty("nonReceivingSysName") };
                    var header = BuildMessageHeader(lstnonReceivingSystemsProduct, messageArchiveCommand.MessageId.ToString());
                    messageArchiveCommand.MessageHeadersJson = header.ToString();
                    messageArchiveCommand.MessageBody = args.Message.MessageText;
                    messageArchiveCommand.InsertDateUtc = DateTime.UtcNow;
                    messageArchiveCommandHandler.Execute(messageArchiveCommand);
                }
                catch (Exception e)
                {
                    LogAndNotifyErrorWithMessage(e, args);
                }
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
        public Dictionary<string, string> BuildMessageHeader(List<string> nonReceivingSystemsProduct, string messageId)
        {
            var messageProperties = new Dictionary<string, string>
            {
               { "IconMessageID", messageId},
               { "Source", "Icon" },
               { "TransactionType", "Global Item" }               
            };

           messageProperties.Add(NonReceivingSystemsProperty, String.Join(",", nonReceivingSystemsProduct));

            return messageProperties;
        }
    }
}