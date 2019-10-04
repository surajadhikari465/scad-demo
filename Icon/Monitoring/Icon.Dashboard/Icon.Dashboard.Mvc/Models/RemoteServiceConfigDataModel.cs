using Icon.Dashboard.Mvc.Enums;
using Icon.Dashboard.Mvc.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Dashboard.Mvc.Models
{
    /// <summary>
    /// Model of the relevant data from a remote service's configuration file
    /// </summary>
    public class RemoteServiceConfigDataModel
    {
        public RemoteServiceConfigDataModel()
        {
            NonEsbAppSettings = new Dictionary<string, string>();
            EsbConnections = new List<EsbConnectionViewModel>();
        }

        public string ConfigFilePath { get; set; }

        public bool IsConfigFilePathValid { get; set; }

        public int? LoggingID { get; set; }

        public Dictionary<string, string> NonEsbAppSettings { get; set; }

        public List<EsbConnectionViewModel> EsbConnections { get; set; }

        public DbConfigurationModel DatabaseConfiguration { get; set; }

        /// <summary>
        /// Flag indicating whether this service's ESB settings (if it has any) are stored
        ///  in a custom <esbEnvironments> section in its config file instead of being in the 
        ///  default app setttings.
        /// </summary>
        public bool HasEsbSettingsInCustomConfigSection { get; set; }
    }
}