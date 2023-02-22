using System.Configuration;

namespace Wfm.Aws.ConfigurationReader
{
    public class WfmAwsConfigurationReader : ConfigurationSection
    {
        public static WfmAwsConfigurationReader GetConfig()
        {
            return (WfmAwsConfigurationReader)ConfigurationManager.GetSection(Constants.NamedConfigurationProperties.WfmAwsConfigurations);
        }

        [ConfigurationProperty(Constants.NamedConfigurationProperties.S3FacadeConfigurations)]
        [ConfigurationCollection(typeof(BasicAWSConfigurations), AddItemName = Constants.NamedConfigurationProperties.S3FacadeConfiguration)]
        public BasicAWSConfigurations S3FacadeConfigurations
        {
            get
            {
                return (BasicAWSConfigurations)base[Constants.NamedConfigurationProperties.S3FacadeConfigurations];
            }
        }

        [ConfigurationProperty(Constants.NamedConfigurationProperties.SNSFacadeConfigurations)]
        [ConfigurationCollection(typeof(BasicAWSConfigurations), AddItemName = Constants.NamedConfigurationProperties.SNSFacadeConfiguration)]
        public BasicAWSConfigurations SNSFacadeConfigurations
        {
            get
            {
                return (BasicAWSConfigurations)base[Constants.NamedConfigurationProperties.SNSFacadeConfigurations];
            }
        }

        [ConfigurationProperty(Constants.NamedConfigurationProperties.SQSFacadeConfigurations)]
        [ConfigurationCollection(typeof(BasicAWSConfigurations), AddItemName = Constants.NamedConfigurationProperties.SQSFacadeConfiguration)]
        public BasicAWSConfigurations SQSFacadeConfigurations
        {
            get
            {
                return (BasicAWSConfigurations)base[Constants.NamedConfigurationProperties.SQSFacadeConfigurations];
            }
        }

        [ConfigurationProperty(Constants.NamedConfigurationProperties.SQSExtendedClientListenerConfigurations)]
        [ConfigurationCollection(typeof(BasicAWSSQSListenerConfigurations), AddItemName = Constants.NamedConfigurationProperties.SQSExtendedClientListenerConfiguration)]
        public BasicAWSSQSListenerConfigurations SQSExtendedClientListenerConfigurations
        {
            get
            {
                return (BasicAWSSQSListenerConfigurations)base[Constants.NamedConfigurationProperties.SQSExtendedClientListenerConfigurations];
            }
        }
    }

    public class BasicAWSConfigurations : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new BasicAWSConfiguration();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return (element as BasicAWSConfiguration).Name;
        }

        public BasicAWSConfiguration this[int index]
        {
            get
            {
                return (BasicAWSConfiguration)base.BaseGet(index);
            }
            set
            {
                if (base.BaseGet(index) != null)
                {
                    base.BaseRemoveAt(index);
                }
                base.BaseAdd(index, value);
            }
        }

        public new BasicAWSConfiguration this[string name]
        {
            get { return (BasicAWSConfiguration)base.BaseGet(name); }
            set
            {
                if (BaseGet(name) != null)
                {
                    BaseRemoveAt(BaseIndexOf(BaseGet(name)));
                }
                BaseAdd(value);
            }
        }
    }

    public class BasicAWSConfiguration : ConfigurationElement
    {
        [ConfigurationProperty(Constants.NamedConfigurationProperties.Name, IsRequired = true)]
        public string Name { get { return (string)base[Constants.NamedConfigurationProperties.Name]; } }

        [ConfigurationProperty(Constants.NamedConfigurationProperties.AwsAccessKey, IsRequired = true)]
        public string AwsAccessKey { get { return (string)base[Constants.NamedConfigurationProperties.AwsAccessKey]; } }

        [ConfigurationProperty(Constants.NamedConfigurationProperties.AwsSecretKey, IsRequired = true)]
        public string AwsSecretKey { get { return (string)base[Constants.NamedConfigurationProperties.AwsSecretKey]; } }

        [ConfigurationProperty(Constants.NamedConfigurationProperties.AwsRegion, IsRequired = true)]
        public string AwsRegion { get { return (string)base[Constants.NamedConfigurationProperties.AwsRegion]; } }
    }

    public class BasicAWSSQSListenerConfigurations : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new BasicAWSSQSListenerConfiguration();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return (element as BasicAWSSQSListenerConfiguration).Name;
        }

        public BasicAWSSQSListenerConfiguration this[int index]
        {
            get
            {
                return (BasicAWSSQSListenerConfiguration)base.BaseGet(index);
            }
            set
            {
                if (base.BaseGet(index) != null)
                {
                    base.BaseRemoveAt(index);
                }
                base.BaseAdd(index, value);
            }
        }

        public new BasicAWSSQSListenerConfiguration this[string name]
        {
            get { return (BasicAWSSQSListenerConfiguration)base.BaseGet(name); }
            set
            {
                if (BaseGet(name) != null)
                {
                    BaseRemoveAt(BaseIndexOf(BaseGet(name)));
                }
                BaseAdd(value);
            }
        }
    }
    public class BasicAWSSQSListenerConfiguration : ConfigurationElement
    {
        [ConfigurationProperty(Constants.NamedConfigurationProperties.Name, IsRequired = true)]
        public string Name { get { return (string)base[Constants.NamedConfigurationProperties.Name]; } }

        [ConfigurationProperty(Constants.NamedConfigurationProperties.SQSListenerApplicationName, IsRequired = true)]
        public string SQSListenerApplicationName { get { return (string)base[Constants.NamedConfigurationProperties.SQSListenerApplicationName]; } }

        [ConfigurationProperty(Constants.NamedConfigurationProperties.SQSListenerQueueUrl, IsRequired = true)]
        public string SQSListenerQueueUrl { get { return (string)base[Constants.NamedConfigurationProperties.SQSListenerQueueUrl]; } }

        [ConfigurationProperty(Constants.NamedConfigurationProperties.SQSListenerTimeoutInSeconds, IsRequired = false, DefaultValue = 15)]
        public int SQSListenerTimeoutInSeconds { get { return (int)base[Constants.NamedConfigurationProperties.SQSListenerTimeoutInSeconds]; } }

        [ConfigurationProperty(Constants.NamedConfigurationProperties.SQSListenerPollIntervalInSeconds, IsRequired = false, DefaultValue = 30)]
        public int SQSListenerPollIntervalInSeconds { get { return (int)base[Constants.NamedConfigurationProperties.SQSListenerPollIntervalInSeconds]; } }

        [ConfigurationProperty(Constants.NamedConfigurationProperties.SQSListenerSafeStopCheckInSeconds, IsRequired = false, DefaultValue = 15)]
        public int SQSListenerSafeStopCheckInSeconds { get { return (int)base[Constants.NamedConfigurationProperties.SQSListenerSafeStopCheckInSeconds]; } }
        
        [ConfigurationProperty(Constants.NamedConfigurationProperties.SQSListenerSafeStopCheckEnabled, IsRequired = false, DefaultValue = false)]
        public bool SQSListenerSafeStopCheckEnabled { get { return (bool)base[Constants.NamedConfigurationProperties.SQSListenerSafeStopCheckEnabled]; } }
    }
}
