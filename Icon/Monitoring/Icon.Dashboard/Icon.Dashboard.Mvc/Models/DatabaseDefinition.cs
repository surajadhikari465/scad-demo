using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Icon.Dashboard.Mvc.Models
{
    /// <summary>
    /// Model representing a database, as used in one of the applications monitored in the dashboard
    /// </summary>
    public class DatabaseDefinition
    {
        public string ServerName { get; set; }
        public string DatabaseName { get; set; }
        public string ConnectionStringName { get; set; }
        public EnvironmentEnum Environment { get; set; }
        public DatabaseCategoryEnum Category { get; set; }
        public bool IsEntityFramework { get; set; }
        public bool IsUsedForLogging { get; set; }
    }
}