using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using System.Collections.Generic;
using Wfm.Aws.SNS.Settings;

namespace Wfm.Aws.SNS
{
    public class SNSFacade : ISNSFacade
    {
        private readonly SNSFacadeSettings snsFacadeSettings;
        public AmazonSimpleNotificationServiceClient amazonSNSClient { get; private set; }

        public SNSFacade(SNSFacadeSettings snsFacadeSettings)
        {
            this.snsFacadeSettings = snsFacadeSettings;
            this.amazonSNSClient = new AmazonSimpleNotificationServiceClient(snsFacadeSettings.AwsAccessKey, snsFacadeSettings.AwsSecretKey);
        }

        public PublishResponse Publish(string topicArn, string message, IDictionary<string, string> attributes)
        {
            Dictionary<string, MessageAttributeValue> snsMessageAttributes = new Dictionary<string, MessageAttributeValue>();
            foreach (KeyValuePair<string, string> attribute in attributes)
            {
                snsMessageAttributes[attribute.Key] = new MessageAttributeValue()
                {
                    StringValue = attribute.Value,
                    DataType = "String",
                };
            }
            PublishRequest publishRequest = new PublishRequest()
            {
                TopicArn = topicArn,
                Message = message,
                MessageAttributes = snsMessageAttributes
            };
            return amazonSNSClient.Publish(publishRequest);
        }
    }
}
