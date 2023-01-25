using System;
using System.Collections.Generic;
using Wfm.Aws.ExtendedClient.Model;

namespace Wfm.Aws.ExtendedClient.Serializer
{
    public class ExtendedClientMessageSerializer : IExtendedClientMessageSerializer
    {
        public IList<ExtendedClientMessageModel> Deserialize(string message)
        {
            throw new NotImplementedException();
        }

        public string Serialize(string s3BucketName, string s3Key, IDictionary<string, string> MessageAttributes)
        {
            throw new NotImplementedException();
        }
    }
}
