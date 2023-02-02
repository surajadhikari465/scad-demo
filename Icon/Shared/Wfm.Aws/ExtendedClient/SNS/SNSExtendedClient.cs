using Amazon.SimpleNotificationService.Model;
using System.Collections.Generic;
using Wfm.Aws.ExtendedClient.Serializer;
using Wfm.Aws.S3;
using Wfm.Aws.SNS;

namespace Wfm.Aws.ExtendedClient.SNS
{
    public class SNSExtendedClient : ISNSExtendedClient
    {
        public ISNSFacade snsFacade { get; private set; }
        public IS3Facade s3Facade { get; private set; }
        private readonly IExtendedClientMessageSerializer extendedClientMessageSerializer;

        public SNSExtendedClient(ISNSFacade snsFacade, IS3Facade s3Facade, IExtendedClientMessageSerializer extendedClientMessageSerializer)
        {
            this.snsFacade = snsFacade;
            this.s3Facade = s3Facade;
            this.extendedClientMessageSerializer = extendedClientMessageSerializer;
        }

        public PublishResponse Publish(string topicArn, string s3BucketName, string s3Key, string data, IDictionary<string, string> messageAttributes)
        {
            string snsMessage = extendedClientMessageSerializer.Serialize(s3BucketName, s3Key, messageAttributes);
            s3Facade.PutObject(s3BucketName, s3Key, data, new Dictionary<string, string>());
            return snsFacade.Publish(topicArn, snsMessage, messageAttributes);
        }
    }
}
