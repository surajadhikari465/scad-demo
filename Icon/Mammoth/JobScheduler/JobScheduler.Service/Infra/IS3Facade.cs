using System.Collections.Generic;

namespace JobScheduler.Service.Infra
{
    internal interface IS3Facade
    {
        void PutObject(string s3BucketName, string s3Key, string data, IDictionary<string, string> metadata);
    }
}
