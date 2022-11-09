using System;
using Amazon.Runtime;
using Amazon.SQS;
using Amazon.S3;

namespace Icon.Dvs
{
    public class DvsClientUtil
    {
        private DvsClientUtil()
        {

        }

        public static IAmazonSQS GetSqsClient(DvsListenerSettings settings)
        {
            return new AmazonSQSClient(
                new BasicAWSCredentials(settings.AwsAccessKey, settings.AwsSecretKey), 
                Amazon.RegionEndpoint.GetBySystemName(settings.Region)
            );
        }

        public static IAmazonS3 GetS3Client(DvsListenerSettings settings)
        {
            return new AmazonS3Client(
                new BasicAWSCredentials(settings.AwsAccessKey, settings.AwsSecretKey),
                Amazon.RegionEndpoint.GetBySystemName(settings.Region)
            );
        }
    }
}
