using System.Collections.Generic;
using Wfm.Aws.ExtendedClient.Model;

namespace Wfm.Aws.ExtendedClient.Serializer
{
    public interface IExtendedClientMessageSerializer
    {
        IList<ExtendedClientMessageModel> Deserialize(string message);
        string Serialize(string s3BucketName, string s3Key, IDictionary<string, string> MessageAttributes);
    }
}
