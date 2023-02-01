using System.Collections.Generic;

namespace Wfm.Aws.ExtendedClient.SQS.Model
{
    public class SQSExtendedClientReceiveModel
    {
        public IList<SQSExtendedClientReceiveModelS3Detail> S3Details { get; set; }
        public IDictionary<string, string> MessageAttributes { get; set; }
        public string RawSQS { get; set; }
        public string SQSMessageID { get; set; }
        public string SQSReceiptHandle { get; set; }
    }

    public class SQSExtendedClientReceiveModelS3Detail
    {
        public string Data { get; set; }
        public string S3Key { get; set; }
        public string S3BucketName { get; set; }
        public IDictionary<string, string> Metadata { get; set; }
    }
}
