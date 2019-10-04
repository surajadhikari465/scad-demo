using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Icon.Dashboard.Mvc.Models.CustomConfigElements
{
    /// <summary>
    /// Represents an Environment element in the configuration file
    /// </summary>
    public class EnvironmentElement : ConfigurationElement
    {
        [ConfigurationProperty(nameof(Name), DefaultValue = "", IsKey = true, IsRequired = true)]  
        public string Name  
        {  
            get { return (string)this["Name"]; }  
            set { this["Name"] = value; }
        }

        [ConfigurationProperty(nameof(IsEnabled), DefaultValue = false, IsKey = true, IsRequired = true)]
        public bool IsEnabled
        {
            get { return (bool)this["IsEnabled"]; }
            set { this["IsEnabled"] = value; }
        }

        [ConfigurationProperty(nameof(DashboardUrl), DefaultValue = "", IsKey = true, IsRequired = false)]
        public string DashboardUrl
        {
            get { return (string)this["DashboardUrl"]; }
            set { this["DashboardUrl"] = value; }
        }

        [ConfigurationProperty(nameof(WebServer), DefaultValue = "", IsKey = true, IsRequired = false)]  
        public string WebServer  
        {  
            get { return (string)this["WebServer"]; }  
            set { this["WebServer"] = value; }  
        }  
  
        [ConfigurationProperty(nameof(AppServers), DefaultValue = "", IsKey = true, IsRequired = false)]  
        public string AppServers
        {  
            get { return (string)this["AppServers"]; }  
            set { this["AppServers"] = value; }  
        }

        [ConfigurationProperty(nameof(MammothWebSupportUrl), DefaultValue = "", IsKey = true, IsRequired = false)]
        public string MammothWebSupportUrl
        {
            get { return (string)this["MammothWebSupportUrl"]; }
            set { this["MammothWebSupportUrl"] = value; }
        }

        [ConfigurationProperty(nameof(IconWebUrl), DefaultValue = "", IsKey = true, IsRequired = false)]
        public string IconWebUrl
        {
            get { return (string)this["IconWebUrl"]; }
            set { this["IconWebUrl"] = value; }
        }

        [ConfigurationProperty(nameof(TibcoAdminUrls), DefaultValue = "", IsKey = true, IsRequired = false)]
        public string TibcoAdminUrls
        {
            get { return (string)this["TibcoAdminUrls"]; }
            set { this["TibcoAdminUrls"] = value; }
        }

        [ConfigurationProperty(nameof(IconDatabaseServers), DefaultValue = "", IsKey = true, IsRequired = false)]
        public string IconDatabaseServers
        {
            get { return (string)this["IconDatabaseServers"]; }
            set { this["IconDatabaseServers"] = value; }
        }

        [ConfigurationProperty(nameof(IconDatabaseCatalogName), DefaultValue = "", IsKey = true, IsRequired = false)]
        public string IconDatabaseCatalogName
        {
            get { return (string)this["IconDatabaseCatalogName"]; }
            set { this["IconDatabaseCatalogName"] = value; }
        }

        [ConfigurationProperty(nameof(MammothDatabaseServers), DefaultValue = "", IsKey = true, IsRequired = false)]
        public string MammothDatabaseServers
        {
            get { return (string)this["MammothDatabaseServers"]; }
            set { this["MammothDatabaseServers"] = value; }
        }

        [ConfigurationProperty(nameof(MammothDatabaseCatalogName), DefaultValue = "", IsKey = true, IsRequired = false)]
        public string MammothDatabaseCatalogName
        {
            get { return (string)this["MammothDatabaseCatalogName"]; }
            set { this["MammothDatabaseCatalogName"] = value; }
        }

        [ConfigurationProperty(nameof(IrmaDatabaseServers), DefaultValue = "", IsKey = true, IsRequired = false)]
        public string IrmaDatabaseServers
        {
            get { return (string)this["IrmaDatabaseServers"]; }
            set { this["IrmaDatabaseServers"] = value; }
        }

        [ConfigurationProperty(nameof(IrmaDatabaseCatalogName), DefaultValue = "", IsKey = true, IsRequired = false)]
        public string IrmaDatabaseCatalogName
        {
            get { return (string)this["IrmaDatabaseCatalogName"]; }
            set { this["IrmaDatabaseCatalogName"] = value; }
        }
    }
}