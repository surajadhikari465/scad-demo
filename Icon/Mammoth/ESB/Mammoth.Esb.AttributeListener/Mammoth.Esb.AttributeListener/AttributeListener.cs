using Esb.Core.Serializer;
using Icon.Common.Email;
using Icon.Esb;
using Icon.Esb.ListenerApplication;
using Icon.Esb.MessageParsers;
using Icon.Esb.Producer;
using Icon.Esb.Schemas.Attributes.ContractTypes;
using Icon.Esb.Schemas.Mammoth.ContractTypes;
using Icon.Esb.Subscriber;
using Icon.Logging;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Esb.AttributeListener.Commands;
using Newtonsoft.Json;
using System;

namespace Mammoth.Esb.AttributeListener
{
    public class AttributeListener : ListenerApplication<AttributeListener, ListenerApplicationSettings>
    {
        private IMessageParser<AttributesType> messageParser;
        private ICommandHandler<AddOrUpdateAttributesCommand> addOrUpdateAttributesCommandHandler;
        private readonly IEsbProducer errorProducer;
        private AddOrUpdateAttributesCommand addOrUpdateAttributesCommand;
        private ISerializer<ErrorMessage> serializer;

        public AttributeListener(ListenerApplicationSettings listenerApplicationSettings,
            EsbConnectionSettings esbConnectionSettings,
            IEsbSubscriber subscriber,
            IEmailClient emailClient,
            ILogger<AttributeListener> logger,
            IMessageParser<AttributesType> messageParser,
            ICommandHandler<AddOrUpdateAttributesCommand> addOrUpdateAttributesCommandHandler,
            IEsbProducer errorProducer,
            ISerializer<ErrorMessage> serializer
            )
            : base(listenerApplicationSettings, esbConnectionSettings, subscriber, emailClient, logger)
        {
            this.messageParser = messageParser;
            this.addOrUpdateAttributesCommandHandler = addOrUpdateAttributesCommandHandler;
            this.errorProducer = errorProducer;
            this.serializer = serializer;
            addOrUpdateAttributesCommand = new AddOrUpdateAttributesCommand();
        }

        public override void HandleMessage(object sender, EsbMessageEventArgs args)
        {
            try
            {
                AttributesType attributes = messageParser.ParseMessage(args.Message);
                addOrUpdateAttributesCommand.Attributes = attributes;
                addOrUpdateAttributesCommandHandler.Execute(addOrUpdateAttributesCommand);

                var messageId = args.Message.GetProperty("IconMessageID");
                logger.Info(String.Format("Successfully processed Message ID '{0}'.", String.IsNullOrEmpty(messageId) ? args.Message.GetProperty("MessageID") : messageId));
            }
            catch (Exception e)
            {
                LogAndNotifyErrorWithMessage(e, args);
                WriteToErrorMessageProducer(e, args);
            }
            finally
            {
                AcknowledgeMessage(args);
            }
        }

        private void WriteToErrorMessageProducer(Exception e, EsbMessageEventArgs args)
        {
            ErrorMessage errorMessage = new ErrorMessage
            {
                Message = args.Message.MessageText,
                ErrorDetails = e.ToString(),
                Application = "Attribute Listener",
                ErrorCode = "AttrListenerReadFail",
                ErrorSeverity = "Error",
                MessageID = args.Message.GetProperty("MessageID"),
                MessageProperties = new NameValuePair[]
                {
                    new NameValuePair
                    {
                        name = "nonReceivingSysName",
                        value = args.Message.GetProperty("nonReceivingSysName")
                    },
                    new NameValuePair
                    {
                        name = "TransactionType",
                        value = args.Message.GetProperty("TransactionType")
                    },
                    new NameValuePair
                    {
                        name = "Source",
                        value = args.Message.GetProperty("Source")
                    }
                }
            };

            string serializedMessage = JsonConvert.SerializeObject(errorMessage);

            errorProducer.Send(serializedMessage);
        }
    }
}