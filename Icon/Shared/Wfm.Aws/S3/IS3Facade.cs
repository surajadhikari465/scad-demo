using Amazon.S3.Model;
using System.Collections.Generic;

namespace Wfm.Aws.S3
{
    public interface IS3Facade
    {
        PutObjectResponse PutObject(string s3BucketName, string s3Key, string data, IDictionary<string, string> metadata);
        GetObjectResponse GetObject(string s3BucketName, string s3Key);
    }
}
