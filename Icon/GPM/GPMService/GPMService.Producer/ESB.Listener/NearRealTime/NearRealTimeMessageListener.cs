using GPMService.Producer.Message.Parser;
using GPMService.Producer.Message.Processor;
using GPMService.Producer.Model;
using Icon.Common.Email;
using Icon.Esb;
using Icon.Esb.ListenerApplication;
using Icon.Esb.Subscriber;
using Icon.Logging;
using System.Transactions;
using TIBCO.EMS;

namespace GPMService.Producer.ESB.Listener.NearRealTime
{
    internal class NearRealTimeMessageListener : ListenerApplication<NearRealTimeMessageListener, ListenerApplicationSettings>
    {
        private readonly IMessageProcessor messageProcessor;
        public NearRealTimeMessageListener(
            ListenerApplicationSettings listenerApplicationSettings,
            EsbConnectionSettings esbConnectionSettings,
            IEsbSubscriber subscriber,
            IEmailClient emailClient,
            ILogger<NearRealTimeMessageListener> logger,
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
            logger.Info(
                    $@"Received Message with 
                    MessageID: ${receivedMessage.esbMessage.GetProperty("TransactionID")}, 
                    PatchFamilyID: ${receivedMessage.esbMessage.GetProperty("CorrelationID")}, 
                    SequenceId: ${receivedMessage.esbMessage.GetProperty("SequenceID")} 
                    and Region Code: ${receivedMessage.esbMessage.GetProperty("RegionCode")}"
                );
            messageProcessor.ProcessMessage(receivedMessage);
            if ((esbConnectionSettings.SessionMode == SessionMode.ClientAcknowledge ||
                esbConnectionSettings.SessionMode == SessionMode.ExplicitClientAcknowledge ||
                esbConnectionSettings.SessionMode == SessionMode.ExplicitClientDupsOkAcknowledge))
            {
                args.Message.Acknowledge();
            }
        }
    }
}
