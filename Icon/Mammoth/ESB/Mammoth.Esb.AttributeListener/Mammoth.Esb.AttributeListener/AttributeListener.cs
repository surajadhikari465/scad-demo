using Esb.Core.Serializer;
using Icon.Common.Email;
using Icon.Esb.Schemas.Attributes.ContractTypes;
using Icon.Esb.Schemas.Mammoth.ContractTypes;
using Icon.Dvs;
using Icon.Dvs.ListenerApplication;
using Icon.Dvs.MessageParser;
using Icon.Dvs.Model;
using Icon.Dvs.Subscriber;
using Icon.Logging;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Esb.AttributeListener.Commands;
using Newtonsoft.Json;
using System;

namespace Mammoth.Esb.AttributeListener
{
    public class AttributeListener : ListenerApplication<AttributeListener>
    {
        private IMessageParser<AttributesType> messageParser;
        private ICommandHandler<AddOrUpdateAttributesCommand> addOrUpdateAttributesCommandHandler;
        private ErrorMessageHandler errorMessageHandler; 
        private AddOrUpdateAttributesCommand addOrUpdateAttributesCommand;
        private ISerializer<ErrorMessage> serializer;

        public AttributeListener(
            DvsListenerSettings listenerSettings,
            IDvsSubscriber subscriber,
            IEmailClient emailClient,
            ILogger<AttributeListener> logger,
            IMessageParser<AttributesType> messageParser,
            ICommandHandler<AddOrUpdateAttributesCommand> addOrUpdateAttributesCommandHandler,
            ErrorMessageHandler errorMessageHandler,
            ISerializer<ErrorMessage> serializer
            )
            : base(listenerSettings, subscriber, emailClient, logger)
        {
            this.messageParser = messageParser;
            this.addOrUpdateAttributesCommandHandler = addOrUpdateAttributesCommandHandler;
            this.errorMessageHandler = errorMessageHandler;
            this.serializer = serializer;
            addOrUpdateAttributesCommand = new AddOrUpdateAttributesCommand();
        }

        public override void HandleMessage(DvsMessage message)
        {
            try
            {
                AttributesType attributes = messageParser.ParseMessage(message);
                addOrUpdateAttributesCommand.Attributes = attributes;
                addOrUpdateAttributesCommandHandler.Execute(addOrUpdateAttributesCommand);

                message.SqsMessage.MessageAttributes.TryGetValue("MessageID", out var messageId);
                logger.Info(String.Format("Successfully processed Message ID '{0}'.", messageId));
            }
            catch (Exception e)
            {
                WriteToErrorMessageProducer(e, message);
                // Logging && Notifying will be handled internally
                throw e;
            }
        }

        private void WriteToErrorMessageProducer(Exception e, DvsMessage message)
        {
            message.SqsMessage.MessageAttributes.TryGetValue("MessageID", out string messageId);
            message.SqsMessage.MessageAttributes.TryGetValue("toBeReceivedBy", out string toBeReceivedBy);
            message.SqsMessage.MessageAttributes.TryGetValue("TransactionType", out string transactionType);
            message.SqsMessage.MessageAttributes.TryGetValue("Source", out string source);

            ErrorMessage errorMessage = new ErrorMessage
            {
                MessageProperties = new NameValuePair[]
                {
                    new NameValuePair
                    {
                        name = "toBeReceivedBy",
                        value = toBeReceivedBy
                    },
                    new NameValuePair
                    {
                        name = "TransactionType",
                        value = transactionType
                    },
                    new NameValuePair
                    {
                        name = "Source",
                        value = source
                    }
                }
            };

            string errorMessageProperties = JsonConvert.SerializeObject(errorMessage);

            errorMessageHandler.HandleErrorMessage(
                this.settings.ListenerApplicationName,
                messageId,
                errorMessageProperties,
                message.MessageContent,
                "Data Issue",
                e.StackTrace,
                "Fatal"
            );
        }
    }
}