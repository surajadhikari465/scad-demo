using GPMService.Producer.Message.Processor;
using GPMService.Producer.Model;
using Icon.Common.Email;
using Icon.Esb.ListenerApplication;
using Icon.Esb.Subscriber;
using Icon.Esb;
using Icon.Logging;

namespace GPMService.Producer.ESB.Listener.JustInTime
{
    internal class ExpiringTprMessageListener : ListenerApplication<ExpiringTprMessageListener, ListenerApplicationSettings>
    {
        private readonly IMessageProcessor messageProcessor;
        public ExpiringTprMessageListener(
            ListenerApplicationSettings listenerApplicationSettings,
            // Using named injection.
            // Changing the variable name would require change in SimpleInjectiorInitializer.cs file as well.
            EsbConnectionSettings expiringTprListenerEsbConnectionSettings,
            IEsbSubscriber subscriber,
            IEmailClient emailClient,
            ILogger<ExpiringTprMessageListener> logger,
            IMessageProcessor messageProcessor
            )
            : base(listenerApplicationSettings, expiringTprListenerEsbConnectionSettings, subscriber, emailClient, logger)
        {
            this.messageProcessor = messageProcessor;
        }

        public override void HandleMessage(object sender, EsbMessageEventArgs args)
        {
            ReceivedMessage receivedMessage = new ReceivedMessage
            {
                esbMessage = args.Message
            };
            messageProcessor.ProcessReceivedMessage(receivedMessage);
        }
    }
}
