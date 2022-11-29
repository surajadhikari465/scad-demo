using Icon.Common.Email;
using Icon.Esb;
using Icon.Esb.ListenerApplication;
using Icon.Esb.Subscriber;
using Icon.Logging;
using MammothR10Price.Message.Processor;

namespace MammothR10Price.Esb.Listener
{
    public class MammothR10PriceListener: ListenerApplication<MammothR10PriceListener, ListenerApplicationSettings>
    {
        private readonly IMessageProcessor messageProcessor;
        public MammothR10PriceListener(
            ListenerApplicationSettings listenerApplicationSettings,
            EsbConnectionSettings esbConnectionSettings,
            IEsbSubscriber subscriber,
            IEmailClient emailClient,
            ILogger<MammothR10PriceListener> logger,
            IMessageProcessor messageProcessor
            )
            : base(listenerApplicationSettings, esbConnectionSettings, subscriber, emailClient, logger)
        {
            this.messageProcessor = messageProcessor;
        }

        public override void HandleMessage(object sender, EsbMessageEventArgs args)
        {
            messageProcessor.ProcessReceivedMessage(args.Message);
        }
    }
}
