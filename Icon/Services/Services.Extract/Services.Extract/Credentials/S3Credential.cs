namespace Services.Extract.Credentials
{
    public class S3Credential
    {
        public S3Credential(string accessKey, string secretKey, string bucketName, string bucketRegion)
        {
            AccessKey = accessKey;
            SecretKey = secretKey;
            BucketName = bucketName;
            BucketRegion = bucketRegion;
        }

        public string AccessKey { get; set; }
        public string SecretKey { get;set; }
        public string BucketName { get; set; }
        public string BucketRegion { get; set; }
    }
}