﻿using GPMService.Producer.Message.Processor;
using GPMService.Producer.Model;
using Icon.Common.Email;
using Icon.Logging;
using Wfm.Aws.ExtendedClient.Listener.SQS;
using Wfm.Aws.ExtendedClient.Listener.SQS.Settings;
using Wfm.Aws.ExtendedClient.SQS;
using Wfm.Aws.ExtendedClient.SQS.Model;

namespace GPMService.Producer.ESB.Listener.JustInTime
{
    internal class ExpiringTprMessageListener : SQSExtendedClientListener<ExpiringTprMessageListener>
    {
        private readonly IMessageProcessor messageProcessor;
        private readonly SQSExtendedClientListenerSettings listenerApplicationSettings;
        public ExpiringTprMessageListener(
            SQSExtendedClientListenerSettings listenerApplicationSettings,
            // Using named injection.
            // Changing the variable name would require change in SimpleInjectiorInitializer.cs file as well.
            IEmailClient emailClient,
            ISQSExtendedClient sqsExtendedClient,
            ILogger<ExpiringTprMessageListener> logger,
            IMessageProcessor messageProcessor
            )
            : base(listenerApplicationSettings, emailClient, sqsExtendedClient, logger)
        {
            this.messageProcessor = messageProcessor;
            this.listenerApplicationSettings= listenerApplicationSettings;

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
