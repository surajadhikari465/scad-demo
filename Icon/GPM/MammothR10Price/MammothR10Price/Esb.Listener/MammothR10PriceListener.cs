using Icon.Common.Email;
using Icon.Dvs;
using Icon.Dvs.ListenerApplication;
using Icon.Dvs.Model;
using Icon.Dvs.Subscriber;
using Icon.Logging;
using MammothR10Price.Message.Processor;

namespace MammothR10Price.Esb.Listener
{
    public class MammothR10PriceListener: ListenerApplication<MammothR10PriceListener>
    {
        private readonly IMessageProcessor messageProcessor;
        public MammothR10PriceListener(
            DvsListenerSettings dvsListenerSettings,
            IDvsSubscriber subscriber,
            IEmailClient emailClient,
            ILogger<MammothR10PriceListener> logger,
            IMessageProcessor messageProcessor
            )
            : base(dvsListenerSettings, subscriber, emailClient, logger)
        {
            this.messageProcessor = messageProcessor;
        }

        public override void HandleMessage(DvsMessage dvsMessage)
        {
            messageProcessor.ProcessReceivedMessage(dvsMessage);
        }
    }
}
