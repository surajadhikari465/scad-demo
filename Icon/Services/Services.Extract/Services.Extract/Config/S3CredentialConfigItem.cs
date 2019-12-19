using System.Configuration;

namespace Services.Extract.Config
{
    public class S3CredentialConfigItem : ConfigurationElement
    {
        [ConfigurationProperty("profileName", IsKey = true, IsRequired = true)]
        public string ProfileName
        {
            get => (string)base["profileName"];
            set => base["profileName"] = value;
        }

        [ConfigurationProperty("accessKey", IsRequired = true)]
        public string AccessKey
        {
            get => (string)base["accessKey"];
            set => base["accessKey"] = value;
        }
        [ConfigurationProperty("secretKey", IsRequired = true)]
        public string SecretKey
        {
            get => (string)base["secretKey"];
            set => base["secretKey"] = value;
        }
        [ConfigurationProperty("bucketRegion", IsRequired = true)]
        public string BucketRegion
        {
            get => (string)base["bucketRegion"];
            set => base["bucketRegion"] = value;
        }

        [ConfigurationProperty("bucketName", IsRequired = true)]
        public string BucketName
        {
            get => (string)base["bucketName"];
            set => base["bucketName"] = value;
        }
    }
}