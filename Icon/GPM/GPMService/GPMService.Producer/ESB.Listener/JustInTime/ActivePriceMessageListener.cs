using GPMService.Producer.Message.Processor;
using GPMService.Producer.Model;
using Icon.Common.Email;
using Icon.Esb.ListenerApplication;
using Icon.Esb.Subscriber;
using Icon.Esb;
using Icon.Logging;

namespace GPMService.Producer.ESB.Listener.JustInTime
{
    internal class ActivePriceMessageListener : ListenerApplication<ActivePriceMessageListener, ListenerApplicationSettings>
    {
        private readonly IMessageProcessor messageProcessor;
        public ActivePriceMessageListener(
            ListenerApplicationSettings listenerApplicationSettings,
            EsbConnectionSettings esbConnectionSettings,
            IEsbSubscriber subscriber,
            IEmailClient emailClient,
            ILogger<ActivePriceMessageListener> logger,
            IMessageProcessor messageProcessor
            )
            : base(listenerApplicationSettings, esbConnectionSettings, subscriber, emailClient, logger)
        {
            this.messageProcessor = messageProcessor;
        }

        public override void HandleMessage(object sender, EsbMessageEventArgs args)
        {
            ReceivedMessage receivedMessage = new ReceivedMessage
            {
                esbMessage = args.Message
            };
            messageProcessor.ProcessMessage(receivedMessage);
        }
    }
}
