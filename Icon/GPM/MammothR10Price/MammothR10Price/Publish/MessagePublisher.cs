using System;
using System.Collections.Generic;
using Icon.ActiveMQ.Producer;
using Icon.Logging;
using Wfm.Aws.S3;
using System.Globalization;

namespace MammothR10Price.Publish
{
    public class MessagePublisher: IMessagePublisher
    {
        private readonly IS3Facade s3Facade;
        private readonly MammothR10PriceServiceSettings serviceSettings;
        private readonly ILogger<MessagePublisher> logger;

        public MessagePublisher(
            IS3Facade s3Facade,
            MammothR10PriceServiceSettings serviceSettings,
            ILogger<MessagePublisher> logger
            )
        {
            this.s3Facade = s3Facade;
            this.serviceSettings = serviceSettings;
            this.logger = logger;
            var computedClientId = $"MammothR10PriceService.{Environment.MachineName}.{Guid.NewGuid()}";
            var clientId = computedClientId.Substring(0, Math.Min(computedClientId.Length, 255));
        }

        public void Publish(string message, Dictionary<string, string> messageProperties)
        {
            PublishToS3(message, messageProperties);
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
