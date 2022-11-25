using GPMService.Producer.Settings;
using Icon.Esb.Producer;
using Icon.Logging;
using Polly;
using Polly.Retry;
using System;
using System.Collections.Generic;

namespace GPMService.Producer.Publish
{
    internal class JustInTimeMessagePublisher : IMessagePublisher
    {
        private readonly IEsbProducer esbProducer;
        private readonly GPMProducerServiceSettings gpmProducerServiceSettings;
        private readonly RetryPolicy sendMessageRetryPolicy;
        private readonly ILogger<JustInTimeMessagePublisher> logger;

        public JustInTimeMessagePublisher (
            IEsbProducer esbProducer, 
            GPMProducerServiceSettings gpmProducerServiceSettings,
            ILogger<JustInTimeMessagePublisher> logger
            )
        {
            this.esbProducer=esbProducer;
            this.gpmProducerServiceSettings=gpmProducerServiceSettings;
            this.logger=logger;
            this.sendMessageRetryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetry(
                gpmProducerServiceSettings.SendMessageRetryCount,
                retryAttempt => TimeSpan.FromMilliseconds(gpmProducerServiceSettings.SendMessageRetryDelayInMilliseconds)
                );
        }

        public void PublishMessage(string message, Dictionary<string, string> messageProperties)
        {
            sendMessageRetryPolicy.Execute(() =>
            {
                esbProducer.Send(message, messageProperties);
            });
        }
    }
}
