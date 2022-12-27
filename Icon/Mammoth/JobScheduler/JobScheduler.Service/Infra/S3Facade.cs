using Amazon.S3;
using Amazon.S3.Model;
using JobScheduler.Service.Settings;
using System.Collections.Generic;

namespace JobScheduler.Service.Infra
{
    internal class S3Facade : IS3Facade
    {
        private readonly JobSchedulerServiceSettings jobSchedulerServiceSettings;
        private readonly AmazonS3Client amazonS3Client;

        public S3Facade(JobSchedulerServiceSettings jobSchedulerServiceSettings)
        {
            this.jobSchedulerServiceSettings = jobSchedulerServiceSettings;
            this.amazonS3Client = new AmazonS3Client(jobSchedulerServiceSettings.AwsAccessKey, jobSchedulerServiceSettings.AwsSecretKey);
        }

        public void PutObject(string s3BucketName, string s3Key, string data, IDictionary<string, string> metadata)
        {
            PutObjectRequest putObjectRequest = new PutObjectRequest()
            {
                BucketName = s3BucketName,
                Key= s3Key,
                ContentBody = data,
            };
            foreach(KeyValuePair<string, string> entry in metadata)
            {
                putObjectRequest.Metadata.Add(entry.Key, entry.Value);
            }
            amazonS3Client.PutObject(putObjectRequest);
        }
    }
}
