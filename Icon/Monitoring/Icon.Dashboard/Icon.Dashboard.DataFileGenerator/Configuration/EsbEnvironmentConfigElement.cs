using Icon.Dashboard.DataFileAccess.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Dashboard.DataFileGenerator.Configuration
{
    public class EsbEnvironmentConfigElement : ConfigurationElement, IEsbEnvironmentDefinition
    {
        [ConfigurationProperty("Name", IsRequired = true, IsKey = true)]
        public string Name
        {
            get
            {
                return (string)base["Name"];
            }
            set
            {
                this["Name"] = value;
            }
        }

        [ConfigurationProperty("ServerUrl", IsRequired = true)]
        public string ServerUrl
        {
            get
            {
                return (string)base["ServerUrl"];
            }
            set
            {
                this["ServerUrl"] = value;
            }
        }

        [ConfigurationProperty("TargetHostName", IsRequired = true)]
        public string TargetHostName
        {
            get
            {
                return (string)base["TargetHostName"];
            }
            set
            {
                this["TargetHostName"] = value;
            }
        }

        [ConfigurationProperty("JmsUsername", IsRequired = true)]
        public string JmsUsername
        {
            get
            {
                return (string)base["JmsUsername"];
            }
            set
            {
                base["JmsUsername"] = value;
            }
        }

        [ConfigurationProperty("JmsPassword", IsRequired = true)]
        public string JmsPassword
        {
            get
            {
                return (string)base["JmsPassword"];
            }
            set
            {
                base["JmsPassword"] = value;
            }
        }

        [ConfigurationProperty("JndiUsername", IsRequired = true)]
        public string JndiUsername
        {
            get
            {
                return (string)base["JndiUsername"];
            }
            set { 
                base ["JndiUsername"] = value;
            }
        }

        [ConfigurationProperty("JndiPassword", IsRequired = true)]
        public string JndiPassword
        {
            get
            {
                return (string)base["JndiPassword"];
            }
            set
            {
                base["JndiPassword"] = value;
            }
        }

        [ConfigurationProperty("SslPassword", IsRequired = false)]
        public string SslPassword
        {
            get
            {
                return (string)base["SslPassword"];
            }
            set
            {
                base["SslPassword"] = value;
            }
        }

        [ConfigurationProperty("SessionMode", IsRequired = false)]
        public string SessionMode
        {
            get
            {
                return (string)base["SessionMode"];
            }
            set
            {
                base["SessionMode"] = value;
            }
        }

        [ConfigurationProperty("CertificateName", IsRequired = false)]
        public string CertificateName
        {
            get
            {
                return (string)base["CertificateName"];
            }
            set
            {
                base["CertificateName"] = value;
            }
        }

        [ConfigurationProperty("CertificateStoreName", IsRequired = false)]
        public string CertificateStoreName
        {
            get
            {
                return (string)base["CertificateStoreName"];
            }
            set
            {
                base["CertificateStoreName"] = value;
            }
        }

        [ConfigurationProperty("CertificateStoreLocation", IsRequired = false)]
        public string CertificateStoreLocation
        {
            get
            {
                return (string)base["CertificateStoreLocation"];
            }
            set
            {
                base["CertificateStoreLocation"] = value;
            }
        }

        [ConfigurationProperty("ReconnectDelay", IsRequired = false)]
        public string ReconnectDelay
        {
            get
            {
                return (string)base["ReconnectDelay"];
            }
            set
            {
                base["ReconnectDelay"] = value;
            }
        }

        [ConfigurationProperty("NumberOfListenerThreads", IsRequired = false)]
        public int NumberOfListenerThreads
        {
            get
            {
                return (int)base["NumberOfListenerThreads"];
            }
            set
            {
                base["NumberOfListenerThreads"] = value;
            }
        }
    }
}
