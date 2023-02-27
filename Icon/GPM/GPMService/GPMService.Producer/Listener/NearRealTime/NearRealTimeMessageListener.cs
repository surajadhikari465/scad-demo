using GPMService.Producer.Message.Processor;
using GPMService.Producer.Model;
using Icon.Common.Email;
using Icon.Logging;
using Wfm.Aws.ExtendedClient.SQS.Model;
using Wfm.Aws.ExtendedClient.Listener.SQS;
using Wfm.Aws.ExtendedClient.Listener.SQS.Settings;
using Wfm.Aws.ExtendedClient.SQS;
using GPMService.Producer.Helpers;
using Wfm.Aws.Helpers;

namespace GPMService.Producer.Listener.NearRealTime
{
    internal class NearRealTimeMessageListener : SQSExtendedClientListener<NearRealTimeMessageListener>
    {
        private readonly IMessageProcessor messageProcessor;
        private readonly SQSExtendedClientListenerSettings listenerApplicationSettings;
        public NearRealTimeMessageListener(
            SQSExtendedClientListenerSettings listenerApplicationSettings,
            // Using named injection.
            // Changing the variable name would require change in SimpleInjectiorInitializer.cs file as well.
            IEmailClient emailClient,
            ISQSExtendedClient sqsExtendedClient,
            ILogger<NearRealTimeMessageListener> logger,
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
            //received S3Details list will have only one SQSExtendedClientReceiveModelS3Detail element , hence fetching the first element  
            logger.Info($@"Received Message with 
MessageID:{message.S3Details[0].Metadata.GetValueOrDefault(Constants.MessageHeaders.TransactionID.ToLower())}, 
PatchFamilyID: {message.S3Details[0].Metadata.GetValueOrDefault(Constants.MessageHeaders.CorrelationID.ToLower())}, 
SequenceId: {message.S3Details[0].Metadata.GetValueOrDefault(Constants.MessageHeaders.SequenceID.ToLower())} 
and Region Code: {message.S3Details[0].Metadata.GetValueOrDefault(Constants.MessageHeaders.RegionCode.ToLower())}"
);
            messageProcessor.ProcessReceivedMessage(receivedMessage);
        }
    }
}
