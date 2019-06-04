using Icon.Dashboard.Mvc.Helpers;
using Icon.Dashboard.Mvc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace Icon.Dashboard.Mvc.ViewModels
{
    public class IconApplicationViewModel
    {
        public IconApplicationViewModel()
        {
            this.ValidCommands = new List<string>();
        }
        
        public Dictionary<string, string> AppSettings { get; set; }

        public Dictionary<string, string> EsbConnectionSettings { get; set; }

        [DisplayName("Full Name")]
        public string Name { get; set; }

        [DisplayName("Config File")]
        [DataType(DataType.MultilineText)]
        public string ConfigFilePath { get; set; }

        [DisplayName("Display Name")]
        public string DisplayName { get; set; }

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

        [DisplayName("ESB Environment")]
        public string CurrentEsbEnvironment { get; set; }

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

        public string HostName { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        public EsbConnectionTypeEnum EsbConnectionType
        {
            get
            {
                var esbConnectionType = EsbConnectionTypeEnum.None;
                if (this.EsbConnectionSettings != null && this.EsbConnectionSettings.Any())
                {
                    if (this.Name.IndexOf("Ewic", Utils.StrcmpOption) > 0
                        || this.DisplayName.IndexOf("Ewic", Utils.StrcmpOption) > 0)
                    {
                        esbConnectionType = EsbConnectionTypeEnum.Ewic;
                    }
                    else if (this.Name.IndexOf("Mammoth", Utils.StrcmpOption) > 0
                        || this.DisplayName.IndexOf("Mammoth", Utils.StrcmpOption) > 0)
                    {
                        esbConnectionType = EsbConnectionTypeEnum.Mammoth;
                    }
                    else
                    {
                        esbConnectionType = EsbConnectionTypeEnum.Icon;
                    }
                }
                return esbConnectionType;
            }
        }

        public bool HasEsbConfiguration
        {
            get
            {
                //return this.EsbConnectionSettings != null && this.EsbConnectionSettings.Any();
                return EsbConnectionType != EsbConnectionTypeEnum.None;
            }
        }
    }
}