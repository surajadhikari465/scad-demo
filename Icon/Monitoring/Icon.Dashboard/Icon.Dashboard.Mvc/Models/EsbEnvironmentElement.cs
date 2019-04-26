using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Icon.Dashboard.Mvc.Models
{
    /// <summary>
    /// Represents an EsbEnvironment element in the configuration file
    /// </summary>
    public class EsbEnvironmentElement : ConfigurationElement
    {
        [ConfigurationProperty(nameof(Name), DefaultValue = "", IsKey = true, IsRequired = true)]  
        public string Name  
        {  
            get { return (string)this["Name"]; }  
            set { this["Name"] = value; }  
        }  
  
        [ConfigurationProperty(nameof(ServerUrl), DefaultValue = "", IsRequired = true)]  
        public string ServerUrl  
        {  
            get { return (string)this[nameof(ServerUrl)]; }  
            set { this[nameof(ServerUrl)] = value; }  
        }  
  
        [ConfigurationProperty(nameof(TargetHostName), DefaultValue = "", IsRequired = true)]  
        public string TargetHostName  
        {  
            get { return (string)this[nameof(TargetHostName)]; }  
            set { this[nameof(TargetHostName)] = value; }
        }

        [ConfigurationProperty(nameof(JmsUsernameIcon), DefaultValue = "", IsRequired = true)]
        public string JmsUsernameIcon
        {
            get { return (string)this[nameof(JmsUsernameIcon)]; }
            set { this[nameof(JmsUsernameIcon)] = value; }
        }

        [ConfigurationProperty(nameof(JmsPasswordIcon), DefaultValue = "", IsRequired = true)]
        public string JmsPasswordIcon
        {
            get { return (string)this[nameof(JmsPasswordIcon)]; }
            set { this[nameof(JmsPasswordIcon)] = value; }
        }

        [ConfigurationProperty(nameof(JndiUsernameIcon), DefaultValue = "", IsRequired = true)]
        public string JndiUsernameIcon
        {
            get { return (string)this[nameof(JndiUsernameIcon)]; }
            set { this[nameof(JndiUsernameIcon)] = value; }
        }

        [ConfigurationProperty(nameof(JndiPasswordIcon), DefaultValue = "", IsRequired = true)]
        public string JndiPasswordIcon
        {
            get { return (string)this[nameof(JndiPasswordIcon)]; }
            set { this[nameof(JndiPasswordIcon)] = value; }
        }

        [ConfigurationProperty(nameof(JmsUsernameMammoth), DefaultValue = "", IsRequired = true)]
        public string JmsUsernameMammoth
        {
            get { return (string)this[nameof(JmsUsernameMammoth)]; }
            set { this[nameof(JmsUsernameMammoth)] = value; }
        }

        [ConfigurationProperty(nameof(JmsPasswordMammoth), DefaultValue = "", IsRequired = true)]
        public string JmsPasswordMammoth
        {
            get { return (string)this[nameof(JmsPasswordMammoth)]; }
            set { this[nameof(JmsPasswordMammoth)] = value; }
        }

        [ConfigurationProperty(nameof(JndiUsernameMammoth), DefaultValue = "", IsRequired = true)]
        public string JndiUsernameMammoth
        {
            get { return (string)this[nameof(JndiUsernameMammoth)]; }
            set { this[nameof(JndiUsernameMammoth)] = value; }
        }

        [ConfigurationProperty(nameof(JndiPasswordMammoth), DefaultValue = "", IsRequired = true)]
        public string JndiPasswordMammoth
        {
            get { return (string)this[nameof(JndiPasswordMammoth)]; }
            set { this[nameof(JndiPasswordMammoth)] = value; }
        }

        [ConfigurationProperty(nameof(JmsUsernameEwic), DefaultValue = "", IsRequired = true)]
        public string JmsUsernameEwic
        {
            get { return (string)this[nameof(JmsUsernameEwic)]; }
            set { this[nameof(JmsUsernameEwic)] = value; }
        }

        [ConfigurationProperty(nameof(JmsPasswordEwic), DefaultValue = "", IsRequired = true)]
        public string JmsPasswordEwic
        {
            get { return (string)this[nameof(JmsPasswordEwic)]; }
            set { this[nameof(JmsPasswordEwic)] = value; }
        }

        [ConfigurationProperty(nameof(JndiUsernameEwic), DefaultValue = "", IsRequired = true)]
        public string JndiUsernameEwic
        {
            get { return (string)this[nameof(JndiUsernameEwic)]; }
            set { this[nameof(JndiUsernameEwic)] = value; }
        }

        [ConfigurationProperty(nameof(JndiPasswordEwic), DefaultValue = "", IsRequired = true)]
        public string JndiPasswordEwic
        {
            get { return (string)this[nameof(JndiPasswordEwic)]; }
            set { this[nameof(JndiPasswordEwic)] = value; }
        }

        [ConfigurationProperty(nameof(SslPassword), DefaultValue = "", IsRequired = false)]
        public string SslPassword
        {
            get { return (string)this[nameof(SslPassword)]; }
            set { this[nameof(SslPassword)] = value; }
        }

        [ConfigurationProperty(nameof(SessionMode), DefaultValue = "", IsRequired = false)]
        public string SessionMode
        {
            get { return (string)this[nameof(SessionMode)]; }
            set { this[nameof(SessionMode)] = value; }
        }

        [ConfigurationProperty(nameof(CertificateName), DefaultValue = "", IsRequired = false)]
        public string CertificateName
        {
            get { return (string)this[nameof(CertificateName)]; }
            set { this[nameof(CertificateName)] = value; }
        }

        [ConfigurationProperty(nameof(CertificateStoreName), DefaultValue = "", IsRequired = false)]
        public string CertificateStoreName
        {
            get { return (string)this[nameof(CertificateStoreName)]; }
            set { this[nameof(CertificateStoreName)] = value; }
        }

        [ConfigurationProperty(nameof(CertificateStoreLocation), DefaultValue = "", IsRequired = false)]
        public string CertificateStoreLocation
        {
            get { return (string)this[nameof(CertificateStoreLocation)]; }
            set { this[nameof(CertificateStoreLocation)] = value; }
        }

        [ConfigurationProperty(nameof(ReconnectDelay), DefaultValue = "", IsRequired = false)]
        public string ReconnectDelay
        {
            get { return (string)this[nameof(ReconnectDelay)]; }
            set { this[nameof(ReconnectDelay)] = value; }
        }

        [ConfigurationProperty(nameof(NumberOfListenerThreads), DefaultValue = "", IsRequired = false)]
        public string NumberOfListenerThreads
        {
            get { return (string)this[nameof(NumberOfListenerThreads)]; }
            set { this[nameof(NumberOfListenerThreads)] = value; }
        }
    }
}