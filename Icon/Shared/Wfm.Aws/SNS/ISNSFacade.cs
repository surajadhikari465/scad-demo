using Amazon.SimpleNotificationService.Model;
using System.Collections.Generic;

namespace Wfm.Aws.SNS
{
    public interface ISNSFacade
    {
        PublishResponse Publish(string topicArn, string message, IDictionary<string, string> attributes);
    }
}
