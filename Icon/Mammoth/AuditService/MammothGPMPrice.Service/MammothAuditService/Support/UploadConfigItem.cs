using System.Configuration;

namespace Audit
{
  public class UploadConfigItem : ConfigurationElement
  {
    [ConfigurationProperty("profileName", IsKey = true, IsRequired = true)]
    public string ProfileName
    {
        get { return (string)base["profileName"]; }
        set { base["profileName"] = value; }
    }

    [ConfigurationProperty("accessKey", IsRequired = true)]
    public string AccessKey
    {
        get { return (string)base["accessKey"]; }
        set { base["accessKey"] = value; }
    }

    [ConfigurationProperty("secretKey", IsRequired = true)]
    public string SecretKey
    {
        get { return (string)base["secretKey"]; }
        set { base["secretKey"] = value; }
    }

    [ConfigurationProperty("bucketRegion", IsRequired = true)]
    public string BucketRegion
    {
        get { return (string)base["bucketRegion"]; }
        set { base["bucketRegion"] = value; }
    }

    [ConfigurationProperty("bucketName", IsRequired = true)]
    public string BucketName
    {
        get { return (string)base["bucketName"]; }
        set { base["bucketName"] = value; }
    }

    [ConfigurationProperty("destinationDir", IsRequired = true)]
    public string DestinationDir
    {
        get { return (string)base["destinationDir"]; }
        set { base["destinationDir"] = value; }
    }
  }
}
