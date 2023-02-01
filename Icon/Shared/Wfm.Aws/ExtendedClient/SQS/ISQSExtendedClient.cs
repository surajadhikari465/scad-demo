using Amazon.SQS.Model;
using System.Collections.Generic;
using Wfm.Aws.ExtendedClient.SQS.Model;

namespace Wfm.Aws.ExtendedClient.SQS
{
    public interface ISQSExtendedClient
    {
        IList<SQSExtendedClientReceiveModel> ReceiveMessage(string queueURL, int maxNumberOfMessages = 1, int waitTimeInSeconds = 0);
        DeleteMessageResponse DeleteMessage(string queueURL, string receiptHandle);
        SendMessageResponse SendMessage(string queueURL, string s3BucketName, string s3Key, string data, IDictionary<string, string> messageAttributes);
    }
}
