using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using System.Collections.Generic;
using Wfm.Aws.S3.Settings;

namespace Wfm.Aws.S3
{
    public class S3Facade : IS3Facade
    {
        private readonly S3FacadeSettings s3FacadeSettings;
        public AmazonS3Client amazonS3Client { get; private set;  }

        public S3Facade(S3FacadeSettings s3FacadeSettings)
        {
            this.s3FacadeSettings = s3FacadeSettings;
            this.amazonS3Client = new AmazonS3Client(s3FacadeSettings.AwsAccessKey, s3FacadeSettings.AwsSecretKey, RegionEndpoint.GetBySystemName(s3FacadeSettings.AwsRegion));
        }

        public PutObjectResponse PutObject(string s3BucketName, string s3Key, string data, IDictionary<string, string> metadata)
        {
            PutObjectRequest putObjectRequest = new PutObjectRequest()
            {
                BucketName = s3BucketName,
                Key = s3Key,
                ContentBody = data,
            };
            foreach (KeyValuePair<string, string> entry in metadata)
            {
                putObjectRequest.Metadata.Add(entry.Key, entry.Value);
            }
            return amazonS3Client.PutObject(putObjectRequest);
        }

        public GetObjectResponse GetObject(string s3BucketName, string s3Key)
        {
            GetObjectRequest getObjectRequest = new GetObjectRequest()
            {
                BucketName = s3BucketName,
                Key = s3Key
            };
            return amazonS3Client.GetObject(getObjectRequest);
        }
    }
}
