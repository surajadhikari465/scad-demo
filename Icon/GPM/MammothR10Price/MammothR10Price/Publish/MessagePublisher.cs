using System;
using System.Collections.Generic;
using Icon.Esb.Producer;
using Icon.ActiveMQ.Producer;
using Icon.Logging;
using Wfm.Aws.S3;
using System.Globalization;

namespace MammothR10Price.Publish
{
    public class MessagePublisher: IMessagePublisher
    {
        private readonly IS3Facade s3Facade;
        private readonly IEsbProducer esbProducer;
        private readonly MammothR10PriceServiceSettings serviceSettings;
        private readonly ILogger<MessagePublisher> logger;

        public MessagePublisher(
            IS3Facade s3Facade,
            IEsbProducer esbProducer,
            MammothR10PriceServiceSettings serviceSettings,
            ILogger<MessagePublisher> logger
            )
        {
            this.s3Facade = s3Facade;
            this.esbProducer = esbProducer;
            this.serviceSettings = serviceSettings;
            this.logger = logger;
            var computedClientId = $"MammothR10PriceService.{Environment.MachineName}.{Guid.NewGuid()}";
            var clientId = computedClientId.Substring(0, Math.Min(computedClientId.Length, 255));
            logger.Info("Opening ESB producer connection");
            this.esbProducer.OpenConnection(clientId);
            logger.Info("ESB producer connection opened");
        }

        public void Publish(string message, Dictionary<string, string> messageProperties)
        {
            PublishToS3(message, messageProperties);
            PublishToEsb(message, messageProperties);
        }

        private void PublishToEsb(string message, Dictionary<string, string> messageProperties)
        {
            esbProducer.Send(message, messageProperties);
        }

        private void PublishToS3(string message, Dictionary<string, string> messageProperties)
        {
            s3Facade.PutObject(
                serviceSettings.DvsGpmSourceBucket,
                $"JustInTime/{DateTime.UtcNow.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture)}/{Guid.NewGuid()}",
                message,
                messageProperties
                );
        }
    }
}
