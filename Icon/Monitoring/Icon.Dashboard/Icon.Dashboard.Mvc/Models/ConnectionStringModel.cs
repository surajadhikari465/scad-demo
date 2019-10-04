using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Icon.Dashboard.Mvc.Models
{
    /// <summary>
    /// Represents an app.config entry for a connectionString element
    /// </summary>
    public class ConnectionStringModel
    {
        public ConnectionStringModel() { }

        public ConnectionStringModel(string name, string providerName, string connectionString)
            : this()
        {
            this.Name = name;
            this.ProviderName = providerName;
            this.ConnectionString = connectionString;
        }

        public string Name { get; set; }
        public string ProviderName { get; set; }
        public string ConnectionString { get; set; }
    }

}