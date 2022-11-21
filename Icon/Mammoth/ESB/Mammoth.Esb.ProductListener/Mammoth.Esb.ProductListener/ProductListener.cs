using Icon.Common.Email;
using Icon.Dvs;
using Icon.Dvs.ListenerApplication;
using Icon.Dvs.MessageParser;
using Icon.Dvs.Subscriber;
using Icon.Dvs.Model;
using Icon.Logging;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Esb.ProductListener.Commands;
using Mammoth.Esb.ProductListener.Mappers;
using Mammoth.Esb.ProductListener.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Newtonsoft.Json;

namespace Mammoth.Esb.ProductListener
{
    public class ProductListener : ListenerApplication<ProductListener>
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

        private const string ToBeReceivedByProperty = "toBeReceivedBy";
        private const string IconMessageIdProperty = "IconMessageID";
        public ProductListener(DvsListenerSettings listenerSettings,
            IDvsSubscriber subscriber,
            IEmailClient emailClient,
            ILogger<ProductListener> logger,
            IMessageParser<List<ItemModel>> messageParser,
            IHierarchyClassIdMapper hierarchyClassIdMapper,
            ICommandHandler<AddOrUpdateProductsCommand> addOrUpdateProductsCommandHandler,
			ICommandHandler<DeleteProductsExtendedAttributesCommand> deleteProductsExtendedAttrCommandHandler,
            ICommandHandler<MessageArchiveCommand> messageArchiveCommandHandler)
            : base(listenerSettings, subscriber, emailClient, logger)
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

        public override void HandleMessage(DvsMessage message)
        {
            bool isSuccess = true;
            List<ItemModel> items = null;

            try
            {
                processCounter = 0;
                items = messageParser.ParseMessage(message);
                isSuccess = ProcessItems(items, message);

                if (!isSuccess)
                {
                    throw new Exception("There was an issue in processing the message");
                }
            }
            catch (Exception e)
            {
                isSuccess = false;
                ArchiveMessage(message);
                // Throwing for logging & retry
                throw e;
            }
            finally
            {
                string iconMessageId;
                message.SqsMessage.MessageAttributes.TryGetValue(IconMessageIdProperty, out iconMessageId);
                logger.Info($"{(isSuccess ? "Successfully" : "Error(s) in")} processed Message ID '{iconMessageId}'. Process times: {processCounter}.");
            }
        }

        bool ProcessItems(List<ItemModel> items, DvsMessage message)
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
                    LogAndNotifyErrorWithMessage(e, message);             
                    
                    if(items.Count > 1)
                    { 
                        foreach(var item in items)
                        {
                            ProcessItems(new List<ItemModel>(){ item }, message);
                        }
                    }
                }
            }

            return isOK;
        }

        private void LogAndNotifyErrorWithMessage(Exception ex, DvsMessage message)
        {
            string messageId;
            message.SqsMessage.MessageAttributes.TryGetValue(IconMessageIdProperty, out messageId);
            string errorMessage = $@"
                An error occurred while processing message. <br/> MessageID : { messageId }
                <br /> SQS MessageID : { message.SqsMessage.MessageId }
                <br /> S3 Bucket Name : { message.SqsMessage.S3BucketName }
                <br /> S3 Key : { message.SqsMessage.S3Key }
                <br/> Message: { message.SqsMessage.MessageAttributes }
            ";
            LogAndNotifyError(errorMessage, ex);
        }

        private void ArchiveMessage(DvsMessage message)
        {
            try
            {
                string toBeReceivedBy;
                message.SqsMessage.MessageAttributes.TryGetValue(ToBeReceivedByProperty, out toBeReceivedBy);
                messageArchiveCommand.MessageId = Guid.NewGuid().ToString();
                var header = BuildMessageHeader(toBeReceivedBy, messageArchiveCommand.MessageId.ToString());
                messageArchiveCommand.MessageHeadersJson = JsonConvert.SerializeObject(header);
                messageArchiveCommand.MessageBody = message.MessageContent;
                messageArchiveCommand.InsertDateUtc = DateTime.UtcNow;
                messageArchiveCommandHandler.Execute(messageArchiveCommand);
            }
            catch (Exception ex)
            {
                LogAndNotifyErrorWithMessage(ex, message);
            }
        }

        public Dictionary<string, string> BuildMessageHeader(string toBeReceivedBy, string messageId)
        {
            var messageProperties = new Dictionary<string, string>
            {
                { "IconMessageID", messageId},
                { "Source", "Icon" },
                { "TransactionType", "Global Item" },
                { ToBeReceivedByProperty, toBeReceivedBy }
            };

            return messageProperties;
        }
    }
}