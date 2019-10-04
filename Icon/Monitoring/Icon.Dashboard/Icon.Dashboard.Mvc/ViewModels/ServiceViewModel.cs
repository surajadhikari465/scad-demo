using Icon.Dashboard.Mvc.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Icon.Dashboard.Mvc.ViewModels
{
    public class ServiceViewModel
    {
        public ServiceViewModel()
        {
            this.ValidCommands = new List<string>();
            this.EsbConnections = new List<EsbConnectionViewModel>();
        }
        
        /// <summary>
        /// non-ESB-related App Settings from the service's config file
        /// </summary>
        public Dictionary<string, string> AppSettings { get; set; }

        [DisplayName("Full Name")]
        public string Name { get; set; }

        [DisplayName("Config File")]
        [DataType(DataType.MultilineText)]
        public string ConfigFilePath { get; set; }

        [DisplayName("Display Name")]
        public string DisplayName { get; set; }

        /// <summary>
        /// human-readable server alias like "vm-icon-test1" as opposed to HostName "cewd6592"
        /// </summary>
        [DisplayName("Server")]
        public string Server { get; set; }

        [DisplayName("Family")]
        public string Family { get; set; }
        
        public string Status { get; set; }

        [DisplayName("Commands")]
        public List<string> ValidCommands { get; set; }

        public virtual bool StatusIsGreen { get; set; }

        [DisplayName("LogName")]
        public string LoggingName { get; set; }

        [DisplayName("Log ID")]
        public int? LoggingID { get; set; }

        [DisplayName("Valid Config?")]
        public bool ConfigFilePathIsValid { get; set; }

        public bool CommandsEnabled { get; set; }

        [DisplayName("Account")]
        public string AccountName { get; set; }

        [DisplayName("Database")]
        public string DatabaseSummary
        {
            get
            {
                return DatabaseConfiguration?.Summary;
            }
        }

        [DisplayName("Database Configuration")]
        public AppDatabaseConfigurationViewModel DatabaseConfiguration { get; set; }

        /// <summary>
        /// network server name, like "cewd6592" as opposed to alias "vm-icon-test1"
        /// </summary>
        public string HostName { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("ESB Environment")]
        public EsbEnvironmentEnum EsbEnvironmentEnum { get; set; }

        public List<EsbConnectionViewModel> EsbConnections { get; set; }

        public bool HasEsbConnections
        {
            get
            {
                return this.EsbConnections != null && this.EsbConnections.Any();
            }
        }

        /// <summary>
        /// Flag indicating whether this service's ESB settings (if it has any) are stored
        ///  in a custom <esbEnvironments> section in its config file instead of being in the 
        ///  default app setttings.
        /// </summary>
        public bool HasEsbSettingsInCustomConfigSection { get; set; }
    }
}