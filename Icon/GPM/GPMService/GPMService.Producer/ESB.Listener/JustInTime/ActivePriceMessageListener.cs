using GPMService.Producer.Message.Processor;
using GPMService.Producer.Model;
using Icon.Common.Email;
using Icon.Esb.ListenerApplication;
using Icon.Esb.Subscriber;
using Icon.Esb;
using Icon.Logging;
using Wfm.Aws.ExtendedClient.Listener.SQS.Settings;
using Wfm.Aws.ExtendedClient.SQS;
using Wfm.Aws.ExtendedClient.Listener.SQS;
using Wfm.Aws.ExtendedClient.SQS.Model;

namespace GPMService.Producer.ESB.Listener.JustInTime
{
    internal class ActivePriceMessageListener : SQSExtendedClientListener<ActivePriceMessageListener>
    {
        private readonly IMessageProcessor messageProcessor;
        private readonly SQSExtendedClientListenerSettings listenerApplicationSettings;
        public ActivePriceMessageListener(
            SQSExtendedClientListenerSettings listenerApplicationSettings,
            // Using named injection.
            // Changing the variable name would require change in SimpleInjectiorInitializer.cs file as well.
            IEmailClient emailClient,
            ISQSExtendedClient sqsExtendedClient,
            ILogger<ActivePriceMessageListener> logger,
            IMessageProcessor messageProcessor
            )
            : base(listenerApplicationSettings, emailClient, sqsExtendedClient, logger)
        {
            this.messageProcessor = messageProcessor;
            this.listenerApplicationSettings = listenerApplicationSettings;
        }

        public override void HandleMessage(SQSExtendedClientReceiveModel message)
        {
            ReceivedMessage receivedMessage = new ReceivedMessage
            {
                sqsExtendedClientMessage = message,
                sqsExtendedClient = sqsExtendedClient,
                sqsExtendedClientSettings = listenerApplicationSettings
            };
            messageProcessor.ProcessReceivedMessage(receivedMessage);
        }
    }
}
