using Icon.Dashboard.Mvc.Enums;
using Icon.Dashboard.Mvc.Helpers;
using Icon.Dashboard.Mvc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Icon.Dashboard.Mvc.ViewModels
{
    /// <summary>
    /// Models the ESB connection for a service
    /// </summary>
    public class EsbConnectionViewModel
    {
        public EsbConnectionViewModel()
        {
            this.CanViewPasswords = false;
        }

        public EsbConnectionViewModel(Dictionary<string, string> esbAppSettings) : this()
        {
            if (esbAppSettings != null)
            {
                var caseInsensitiveDictionary = new Dictionary<string, string>(esbAppSettings, StringComparer.CurrentCultureIgnoreCase);

                if (caseInsensitiveDictionary.ContainsKey("Name"))
                {
                    this.ConnectionName = caseInsensitiveDictionary["Name"];
                }
                else if (caseInsensitiveDictionary.ContainsKey("name"))
                {
                    this.ConnectionName = caseInsensitiveDictionary["name"];
                }
                else
                {
                    // esb connections from app settings are not named, so by default call it just "ESB"
                    this.ConnectionName = "ESB";
                }

                if (caseInsensitiveDictionary.ContainsKey(Constants.EsbSettingKeys.ServerUrlKey))
                {
                    this.ServerUrl = caseInsensitiveDictionary[Constants.EsbSettingKeys.ServerUrlKey];
                }
                if (caseInsensitiveDictionary.ContainsKey(Constants.EsbSettingKeys.TargetHostNameKey))
                {
                    this.TargetHostName = caseInsensitiveDictionary[Constants.EsbSettingKeys.TargetHostNameKey];
                }
                if (caseInsensitiveDictionary.ContainsKey(Constants.EsbSettingKeys.CertificateNameKey))
                {
                    this.CertificateName = caseInsensitiveDictionary[Constants.EsbSettingKeys.CertificateNameKey];
                }
                if (caseInsensitiveDictionary.ContainsKey(Constants.EsbSettingKeys.JmsUsernameKey))
                {
                    this.JmsUsername = caseInsensitiveDictionary[Constants.EsbSettingKeys.JmsUsernameKey];
                }
                if (caseInsensitiveDictionary.ContainsKey(Constants.EsbSettingKeys.JmsPasswordKey))
                {
                    this.JmsPassword = caseInsensitiveDictionary[Constants.EsbSettingKeys.JmsPasswordKey];
                }
                if (caseInsensitiveDictionary.ContainsKey(Constants.EsbSettingKeys.JndiUsernameKey))
                {
                    this.JndiUsername = caseInsensitiveDictionary[Constants.EsbSettingKeys.JndiUsernameKey];
                }
                if (caseInsensitiveDictionary.ContainsKey(Constants.EsbSettingKeys.JndiPasswordKey))
                {
                    this.JndiPassword = caseInsensitiveDictionary[Constants.EsbSettingKeys.JndiPasswordKey];
                }
                if (caseInsensitiveDictionary.ContainsKey(Constants.EsbSettingKeys.CertificateStoreNameKey))
                {
                    this.CertificateStoreName = caseInsensitiveDictionary[Constants.EsbSettingKeys.CertificateStoreNameKey];
                }
                if (caseInsensitiveDictionary.ContainsKey(Constants.EsbSettingKeys.CertificateStoreLocationKey))
                {
                    this.CertificateStoreLocation = caseInsensitiveDictionary[Constants.EsbSettingKeys.CertificateStoreLocationKey];
                }
                if (caseInsensitiveDictionary.ContainsKey(Constants.EsbSettingKeys.ConnectionFactoryNameKey))
                {
                    this.ConnectionFactoryName = caseInsensitiveDictionary[Constants.EsbSettingKeys.ConnectionFactoryNameKey];
                }
                if (caseInsensitiveDictionary.ContainsKey(Constants.EsbSettingKeys.SslPasswordKey))
                {
                    this.SslPassword = caseInsensitiveDictionary[Constants.EsbSettingKeys.SslPasswordKey];
                }
                if (caseInsensitiveDictionary.ContainsKey(Constants.EsbSettingKeys.SessionModeKey))
                {
                    this.SessionMode = caseInsensitiveDictionary[Constants.EsbSettingKeys.SessionModeKey];
                }
                if (caseInsensitiveDictionary.ContainsKey(Constants.EsbSettingKeys.ReconnectDelayKey))
                {
                    this.ReconnectDelay = caseInsensitiveDictionary[Constants.EsbSettingKeys.ReconnectDelayKey];
                }
                if (caseInsensitiveDictionary.ContainsKey(Constants.EsbSettingKeys.QueueNameKey))
                {
                    this.QueueName = caseInsensitiveDictionary[Constants.EsbSettingKeys.QueueNameKey];
                }
                if (caseInsensitiveDictionary.ContainsKey(Constants.EsbSettingKeys.LocaleQueueNameKey))
                {
                    this.LocaleQueueName = caseInsensitiveDictionary[Constants.EsbSettingKeys.LocaleQueueNameKey];
                }
                if (caseInsensitiveDictionary.ContainsKey(Constants.EsbSettingKeys.HierarchyQueueNameKey))
                {
                    this.HierarchyQueueName = caseInsensitiveDictionary[Constants.EsbSettingKeys.HierarchyQueueNameKey];
                }
                if (caseInsensitiveDictionary.ContainsKey(Constants.EsbSettingKeys.ItemQueueNameKey))
                {
                    this.ItemQueueName = caseInsensitiveDictionary[Constants.EsbSettingKeys.ItemQueueNameKey];
                }
                if (caseInsensitiveDictionary.ContainsKey(Constants.EsbSettingKeys.ProductSelectionGroupQueueNameKey))
                {
                    this.ProductSelectionGroupQueueName = caseInsensitiveDictionary[Constants.EsbSettingKeys.ProductSelectionGroupQueueNameKey];
                }

                this.ConnectionType = DetermineEsbConnectionTypeByJmsUsername(this.JmsUsername);
            } 
        }

        [DisplayName("Connection to")]
        public string ConnectionName { get; set; }

        public EsbConnectionTypeEnum ConnectionType { get; set; }

        [DisplayName("Environment")]
        public EsbEnvironmentEnum EnvironmentEnum { get; set; }

        public bool CanViewPasswords { get; set; }

        [DisplayName("Server URL")]
        public string ServerUrl { get; set; }

        [DisplayName("Target Host Name")]
        public string TargetHostName { get; set; }

        [DisplayName("JNDI User")]
        public string JmsUsername { get; set; }

        [DisplayName("JMS Password")]
        public string JmsPassword { get; set; }

        [DisplayName("Server URL")]
        public string JndiUsername { get; set; } 

        [DisplayName("JNDI Password")]

        public string JndiPassword { get; set; }

        [DisplayName("Certificate Name")]
        public string CertificateName { get; set; }

        [DisplayName("Cert. Store Name")]
        public string CertificateStoreName { get; set; }

        [DisplayName("Cert. Store Location")]
        public string CertificateStoreLocation { get; set; }

        [DisplayName("Connection Factory")]
        public string ConnectionFactoryName { get; set; }

        [DisplayName("Session Mode")]
        public string SessionMode { get; set; }

        [DisplayName("SSL Password")]
        public string SslPassword { get; set; }

        [DisplayName("Reconnect Delay")]
        public string ReconnectDelay { get; set; }

        [DisplayName("Queue")]
        public string QueueName { get; set; }

        [DisplayName("Locale Queue")]
        public string LocaleQueueName { get; set; }

        [DisplayName("Hierarchy Queue")]
        public string HierarchyQueueName { get; set; }

        [DisplayName("Item Queue")]
        public string ItemQueueName { get; set; }

        [DisplayName("PSG Queue")]
        public string ProductSelectionGroupQueueName { get; set; }

        public Dictionary<string, string> SettingsAsDictionary()
        {
            var esbSettings = new Dictionary<string, string>();

            if (!string.IsNullOrWhiteSpace(this.ServerUrl))
            {
                esbSettings.Add(Constants.EsbSettingKeys.ServerUrlKey, this.ServerUrl);
            };
            if (!string.IsNullOrWhiteSpace(this.TargetHostName))
            {
                esbSettings.Add(Constants.EsbSettingKeys.TargetHostNameKey, this.TargetHostName);
            };
            if (!string.IsNullOrWhiteSpace(this.CertificateName))
            {
                esbSettings.Add(Constants.EsbSettingKeys.CertificateNameKey, this.CertificateName);
            };
            if (!string.IsNullOrWhiteSpace(this.JmsUsername))
            {
                esbSettings.Add(Constants.EsbSettingKeys.JmsUsernameKey, this.JmsUsername);
            };
            if (!string.IsNullOrWhiteSpace(this.JmsPassword))
            {
                esbSettings.Add(Constants.EsbSettingKeys.JmsPasswordKey, this.JmsPassword);
            };
            if (!string.IsNullOrWhiteSpace(this.JndiUsername))
            {
                esbSettings.Add(Constants.EsbSettingKeys.JndiUsernameKey, this.JndiUsername);
            };
            if (!string.IsNullOrWhiteSpace(this.JndiPassword))
            {
                esbSettings.Add(Constants.EsbSettingKeys.JndiPasswordKey, this.JndiPassword);
            };
            if (!string.IsNullOrWhiteSpace(this.CertificateStoreName))
            {
                esbSettings.Add(Constants.EsbSettingKeys.CertificateStoreNameKey, this.CertificateStoreName);
            };
            if (!string.IsNullOrWhiteSpace(this.CertificateStoreLocation))
            {
                esbSettings.Add(Constants.EsbSettingKeys.CertificateStoreLocationKey, this.CertificateStoreLocation);
            };
            if (!string.IsNullOrWhiteSpace(this.ConnectionFactoryName))
            {
                esbSettings.Add(Constants.EsbSettingKeys.ConnectionFactoryNameKey, this.ConnectionFactoryName);
            };
            if (!string.IsNullOrWhiteSpace(this.SslPassword))
            {
                esbSettings.Add(Constants.EsbSettingKeys.SslPasswordKey, this.SslPassword);
            };
            if (!string.IsNullOrWhiteSpace(this.SessionMode))
            {
                esbSettings.Add(Constants.EsbSettingKeys.SessionModeKey, this.SessionMode);
            };
            if (!string.IsNullOrWhiteSpace(this.QueueName))
            {
                esbSettings.Add(Constants.EsbSettingKeys.QueueNameKey, this.QueueName);
            };
            if (!string.IsNullOrWhiteSpace(this.LocaleQueueName))
            {
                esbSettings.Add(Constants.EsbSettingKeys.LocaleQueueNameKey, this.LocaleQueueName);
            };
            if (!string.IsNullOrWhiteSpace(this.HierarchyQueueName))
            {
                esbSettings.Add(Constants.EsbSettingKeys.HierarchyQueueNameKey, this.HierarchyQueueName);
            };
            if (!string.IsNullOrWhiteSpace(this.ItemQueueName))
            {
                esbSettings.Add(Constants.EsbSettingKeys.ItemQueueNameKey, this.ItemQueueName);
            };
            if (!string.IsNullOrWhiteSpace(this.ProductSelectionGroupQueueName))
            {
                esbSettings.Add(Constants.EsbSettingKeys.ProductSelectionGroupQueueNameKey, this.ProductSelectionGroupQueueName);
            };

            return esbSettings;
        } 

        public static EsbConnectionTypeEnum DetermineEsbConnectionTypeByJmsUsername(string jmsUserName)
        {
            var esbConnectionType = EsbConnectionTypeEnum.None;
            if (!string.IsNullOrWhiteSpace(jmsUserName))
            {

                if (jmsUserName.IndexOf("ewic", Utils.StrcmpOption) >= 0)
                {
                    esbConnectionType = EsbConnectionTypeEnum.Ewic;
                }
                else if (jmsUserName.IndexOf("mammoth", Utils.StrcmpOption) >= 0)
                {
                    esbConnectionType = EsbConnectionTypeEnum.Mammoth;
                }
                else if (jmsUserName.IndexOf("icon", Utils.StrcmpOption) >= 0)
                {
                    esbConnectionType = EsbConnectionTypeEnum.Icon;
                }
            }
            return esbConnectionType;
        }
    }
}