using Amazon.SQS.Model;
using System.Collections.Generic;

namespace Wfm.Aws.SQS
{
    public interface ISQSFacade
    {
        SendMessageResponse SendMessage(string queueURL, string message, IDictionary<string, string> attributes);
        ReceiveMessageResponse ReceiveMessage(string queueURL, int maxNumberOfMessages = 1, int waitTimeInSeconds = 0);
    }
}
