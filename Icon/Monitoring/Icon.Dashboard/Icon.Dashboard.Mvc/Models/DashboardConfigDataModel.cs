using Icon.Dashboard.Mvc.Enums;
using Icon.Dashboard.Mvc.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Icon.Dashboard.Mvc.Models
{
    public class DashboardConfigDataModel
    {
        public DashboardConfigDataModel()
        {
            SecurityGroupsWithReadRights = new List<string>();
            SecurityGroupsWithEditRights = new List<string>();
            EnvironmentDefinitions = new List<EnvironmentModel>();
            EsbEnvironmentDefinitions = new List<EsbEnvironmentModel>();

            EnvironmentCookieName = Constants.DashboardAppSettings.DefaultValues.EnvironmentNameCookieName;
            EnvironmentAppServersCookieName = Constants.DashboardAppSettings.DefaultValues.EnvironmentAppServersCookieName;
        }

        public DashboardConfigDataModel(IDashboardConfigDataLoader configDataLoader) : this()
        {
            this.LoadConfigData(configDataLoader);
        }

        public EnvironmentEnum HostingEnvironmentSetting { get; set; }
        public List<string> SecurityGroupsWithReadRights { get; set; }
        public List<string> SecurityGroupsWithEditRights { get; set; }
        public int ServiceCommandTimeout { get; set; }
        public int HoursForRecentErrors { get; set; }
        public int MillisecondsForRecentErrorsPolling { get; set; }
        public string EnvironmentCookieName { get; set; }
        public string EnvironmentAppServersCookieName { get; set; }
        public int EnvironmentCookieDurationHours { get; set; }
        public List<EnvironmentModel> EnvironmentDefinitions { get; set; }
        public List<EsbEnvironmentModel> EsbEnvironmentDefinitions { get; set; }

        internal void LoadConfigData(IDashboardConfigDataLoader configDataLoader)
        {
            this.HostingEnvironmentSetting = configDataLoader.GetTypedSettingValueOrDefault(
                Constants.DashboardAppSettings.Keys.HostingEnvironment,
                Constants.DashboardAppSettings.DefaultValues.HostingEnvironmentEnum);
            this.EnvironmentCookieDurationHours = configDataLoader.GetTypedSettingValueOrDefault(
                Constants.DashboardAppSettings.Keys.EnvironmentCookieDuration,
                Constants.DashboardAppSettings.DefaultValues.EnvironmentCookieDurationHours);
            this.ServiceCommandTimeout = configDataLoader.GetTypedSettingValueOrDefault(
                Constants.DashboardAppSettings.Keys.ServiceCommandTimeoutMilliseconds,
                Constants.DashboardAppSettings.DefaultValues.ServiceCommandTimeoutMilliseconds);
            this.HoursForRecentErrors = configDataLoader.GetTypedSettingValueOrDefault(
                Constants.DashboardAppSettings.Keys.HoursForRecentErrors,
                Constants.DashboardAppSettings.DefaultValues.HoursForRecentErros);
            this.MillisecondsForRecentErrorsPolling = configDataLoader.GetTypedSettingValueOrDefault(
                Constants.DashboardAppSettings.Keys.SecondsForRecentErrorsPolling,
                Constants.DashboardAppSettings.DefaultValues.SecondsForRecentErrorsPolling) * 1000;

            this.SecurityGroupsWithReadRights = configDataLoader.SplitAppSettingValuesToList(
                    Constants.DashboardAppSettings.Keys.SecurityGroupsReadOnly,
                    Constants.DashboardAppSettings.DefaultValues.SecurityGroupsWithReadOnly);
            this.SecurityGroupsWithEditRights = configDataLoader.SplitAppSettingValuesToList(
                    Constants.DashboardAppSettings.Keys.SecurityGroupsEditRights,
                    Constants.DashboardAppSettings.DefaultValues.SecurityGroupsWithEditRights);

            this.EnvironmentDefinitions = configDataLoader.GetEnvironments();

            this.EsbEnvironmentDefinitions = configDataLoader.GetEsbEnvironments();
         }
    }
}