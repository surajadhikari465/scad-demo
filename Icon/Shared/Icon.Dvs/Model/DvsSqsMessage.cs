using Amazon.SQS.Model;
using System.Collections.Generic;

namespace Icon.Dvs.Model
{
    public class DvsSqsMessage
    {
        public string MessageId { get; set; }
        public IDictionary<string, string> MessageAttributes { get; set; }
        public string SqsReceiptHandle { get; set; }
        public string S3BucketName { get; set; }
        public string S3Key { get; set; }
    }
}
