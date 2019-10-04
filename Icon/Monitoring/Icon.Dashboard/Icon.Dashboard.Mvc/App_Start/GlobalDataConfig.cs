using Icon.Dashboard.Mvc.Helpers;
using Icon.Dashboard.Mvc.Models;
using Icon.Dashboard.Mvc.Models.CustomConfigElements;
using Icon.Dashboard.Mvc.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Dashboard.Mvc
{
    public class GlobalDataConfig
    {
        public static DashboardConfigDataModel LoadConfigData(IDashboardConfigDataLoader configDataReader = null)
        {
            if (configDataReader == null)
            {
                var configuration = ConfigAccess.OpenInternalWebConfiguration();
                var appSettings = ConfigAccess.GetAppSettingsAsDictionary(configuration);
                var environmentsConfigSection = ConfigAccess.GetCustomConfigSection<EnvironmentsSection>(
                    configuration, Constants.CustomConfigSectionGroupName);
                var esbEnvironmentsConfigSection = ConfigAccess.GetCustomConfigSection<EsbEnvironmentsSection>(
                    configuration, Constants.CustomConfigSectionGroupName);
                configDataReader = new DashboardConfigDataLoader(
                    appSettings, environmentsConfigSection, esbEnvironmentsConfigSection);
            }
            return new DashboardConfigDataModel(configDataReader);
        }
    }
}