using Icon.Common.Email;
using Icon.Logging;
using MammothR10Price.Message.Processor;
using Wfm.Aws.ExtendedClient.Listener.SQS;
using Wfm.Aws.ExtendedClient.Listener.SQS.Settings;
using Wfm.Aws.ExtendedClient.SQS;
using Wfm.Aws.ExtendedClient.SQS.Model;

namespace MammothR10Price.Esb.Listener
{
    public class MammothR10PriceListener: SQSExtendedClientListener<MammothR10PriceListener>
    {
        private readonly IMessageProcessor messageProcessor;
        public MammothR10PriceListener(
            SQSExtendedClientListenerSettings listenerApplicationSettings,
            ISQSExtendedClient sqsExtendedClient,
            IEmailClient emailClient,
            ILogger<MammothR10PriceListener> logger,
            IMessageProcessor messageProcessor
            )
            : base(listenerApplicationSettings, emailClient, sqsExtendedClient, logger)
        {
            this.messageProcessor = messageProcessor;
        }

        public override void HandleMessage(SQSExtendedClientReceiveModel message)
        {
            try
            {
                messageProcessor.ProcessReceivedMessage(message);
            }
            finally
            {
                Acknowledge(message);
            }
        }
    }
}
