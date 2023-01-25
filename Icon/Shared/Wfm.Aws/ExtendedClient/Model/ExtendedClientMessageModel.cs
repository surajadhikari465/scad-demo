using System.Collections.Generic;

namespace Wfm.Aws.ExtendedClient.Model
{
    public class ExtendedClientMessageModel
    {
        public string S3Key { get; set; }
        public string S3BucketName { get; set; }
        public IDictionary<string, string> MessageAttributes { get; set; }
    }
}
