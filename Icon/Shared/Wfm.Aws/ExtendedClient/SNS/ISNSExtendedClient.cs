using Amazon.SimpleNotificationService.Model;
using System.Collections.Generic;

namespace Wfm.Aws.ExtendedClient.SNS
{
    public interface ISNSExtendedClient
    {
        PublishResponse Publish(string topicArn, string s3BucketName, string s3Key, string data, IDictionary<string, string> messageAttributes);
    }
}
