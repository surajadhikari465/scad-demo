using Icon.Dashboard.Mvc.Enums;
using Icon.Dashboard.Mvc.Helpers;
using Icon.Dashboard.Mvc.Models.CustomConfigElements;
using Icon.Dashboard.Mvc.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Icon.Dashboard.Mvc.Models
{
    public class EnvironmentModel
    {
        public EnvironmentModel()
        {
            AppServers = new List<string>();
        }

        public EnvironmentModel(EnvironmentElement configElement) : this()
        {
            this.Name = configElement.Name;
            this.EnvironmentEnum = (EnvironmentEnum)Enum.Parse(typeof(EnvironmentEnum), configElement.Name);
            this.IsEnabled = configElement.IsEnabled;
            this.DashboardUrl = configElement.DashboardUrl;
            this.WebServer = configElement.WebServer;
            this.AppServers = Utils.SplitCommaSeparatedValuesToList(configElement.AppServers);
            this.IconWebUrl = configElement.IconWebUrl;
            this.MammothWebSupportUrl = configElement.MammothWebSupportUrl;
            this.TibcoAdminUrls = Utils.SplitCommaSeparatedValuesToList(configElement.TibcoAdminUrls);
            this.IconDatabaseServer = configElement.IconDatabaseServers;
            this.IconDatabaseName = configElement.IconDatabaseCatalogName;
            this.MammothDatabaseServer = configElement.MammothDatabaseServers;
            this.MammothDatabaseName = configElement.MammothDatabaseCatalogName;
            this.IrmaDatabaseServers = Utils.SplitCommaSeparatedValuesToList(configElement.IrmaDatabaseServers);
            this.IrmaDatabaseName = configElement.IrmaDatabaseCatalogName;
        }

        public EnvironmentModel(EnvironmentCookieModel cookieSettings) : this()
        {
            this.Name = cookieSettings.Name;
            this.EnvironmentEnum = cookieSettings.EnvironmentEnum;
            this.IsEnabled = true;
            if (cookieSettings.AppServers != null)
            {
                this.AppServers = new List<string>(cookieSettings.AppServers.Count);
                foreach (var appServerFromCookie in cookieSettings.AppServers)
                {
                    this.AppServers.Add(appServerFromCookie);
                }
            }
        }

        public string Name { get; set; }

        public EnvironmentEnum EnvironmentEnum { get; set; }

        public bool IsHostingEnvironment { get; set; }

        public bool IsEnabled { get; set; }

        public string DashboardUrl { get; set; }

        public string WebServer { get; set; }

        public List<string> AppServers { get; set; }

        public string MammothWebSupportUrl { get; set; }

        public string IconWebUrl { get; set; }

        public List<string> TibcoAdminUrls { get; set; }

        public string IconDatabaseServer { get; set; }

        public string IconDatabaseName { get; set; }

        public string MammothDatabaseServer { get; set; }

        public string MammothDatabaseName { get; set; }

        public List<string> IrmaDatabaseServers { get; set; }

        public string IrmaDatabaseName { get; set; }

        public EnvironmentViewModel ToViewModel(bool isHosting, bool isProduction)
        {
            var viewModel = new EnvironmentViewModel();

            viewModel.Name = this.Name;
            viewModel.EnvironmentEnum = this.EnvironmentEnum;
            viewModel.IsHostingEnvironment = isHosting;
            viewModel.IsProduction = isProduction;
            viewModel.BootstrapClass = Utils.GetBootstrapClassForEnvironment(this.EnvironmentEnum);
            viewModel.AppServers = new List<AppServerViewModel>(
                this.AppServers.Select(s => new AppServerViewModel { ServerName = s }));

            return viewModel;
        }
    }
}