using Amazon.SQS;
using Amazon.SQS.Model;
using System.Collections.Generic;
using Wfm.Aws.SQS.Settings;

namespace Wfm.Aws.SQS
{
    public class SQSFacade : ISQSFacade
    {
        private readonly SQSFacadeSettings sqsFacadeSettings;
        public AmazonSQSClient amazonSQSClient { get; private set; }

        public SQSFacade(SQSFacadeSettings sqsFacadeSettings, AmazonSQSClient amazonSQSClient)
        {
            this.sqsFacadeSettings = sqsFacadeSettings;
            this.amazonSQSClient = amazonSQSClient;
        }

        public SendMessageResponse SendMessage(string queueURL, string message, IDictionary<string, string> attributes)
        {
            Dictionary<string, MessageAttributeValue> sqsMessageAttributes = new Dictionary<string, MessageAttributeValue>();
            foreach (KeyValuePair<string, string> attribute in attributes)
            {
                sqsMessageAttributes[attribute.Key] = new MessageAttributeValue()
                {
                    StringValue = attribute.Value,
                    DataType = "String",
                };
            }
            SendMessageRequest sendMessageRequest = new SendMessageRequest()
            {
                QueueUrl= queueURL,
                MessageBody = message,
                MessageAttributes = sqsMessageAttributes
            };
            return amazonSQSClient.SendMessage(sendMessageRequest);
        }

        public ReceiveMessageResponse ReceiveMessage(string queueURL, int maxNumberOfMessages = 1, int waitTimeInSeconds = 0)
        {
            ReceiveMessageRequest receiveMessageRequest = new ReceiveMessageRequest()
            {
                QueueUrl= queueURL,
                MaxNumberOfMessages = maxNumberOfMessages,
                WaitTimeSeconds = waitTimeInSeconds
            };
            return amazonSQSClient.ReceiveMessage(receiveMessageRequest);
        }
    }
}
