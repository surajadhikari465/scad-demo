using System.Collections.Generic;
using System.Diagnostics.Tracing;

namespace Wfm.Aws.ExtendedClient.Model
{
    public class ExtendedClientMessageModel
    {
        public string EventSource { get; set; }
        public IList<ExtendedClientMessageModelS3Detail> S3Details { get; set; }
        public IDictionary<string, string> MessageAttributes { get; set; }
    }

    public class ExtendedClientMessageModelS3Detail
    {
        public string S3Key { get; set; }
        public string S3BucketName { get; set; }
    }
}
