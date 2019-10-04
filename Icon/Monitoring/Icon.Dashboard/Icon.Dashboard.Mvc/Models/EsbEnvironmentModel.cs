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
    public class EsbEnvironmentModel
    {
        public EsbEnvironmentModel()
        {

        }

        public EsbEnvironmentModel(EsbEnvironmentElement configElement) : this()
        {
            this.Name = configElement.Name;
            this.EsbEnvironment = Utils.ParseEsbEnvironment(configElement.Name);
            this.ServerUrl = configElement.ServerUrl;
            this.TargetHostName = configElement.TargetHostName;
            this.CertificateName = configElement.CertificateName;
            this.CertificateStoreName = configElement.CertificateStoreName;
            this.CertificateStoreLocation = configElement.CertificateStoreLocation;
            this.SslPassword = configElement.SslPassword;
            this.SessionMode = configElement.SessionMode;
            this.JmsUsernameIcon = configElement.JmsUsernameIcon;
            this.JmsPasswordIcon = configElement.JmsPasswordIcon;
            this.JndiUsernameIcon = configElement.JndiUsernameIcon;
            this.JndiPasswordIcon = configElement.JndiPasswordIcon;
            this.JmsUsernameMammoth = configElement.JmsUsernameMammoth;
            this.JmsPasswordMammoth = configElement.JmsPasswordMammoth;
            this.JndiUsernameMammoth = configElement.JndiUsernameMammoth;
            this.JndiPasswordMammoth = configElement.JndiPasswordMammoth;
            this.JmsUsernameEwic = configElement.JmsUsernameEwic;
            this.JmsPasswordEwic = configElement.JmsPasswordEwic;
            this.JndiUsernameEwic = configElement.JndiUsernameEwic;
            this.JndiPasswordEwic = configElement.JndiPasswordEwic;
        }

        public string Name { get; set; }

        public EsbEnvironmentEnum EsbEnvironment { get; set; }

        public string ServerUrl { get; set; }

        public List<string> ServerUrls
        {
            get
            {
                var urls = new List<string>();
                if (!string.IsNullOrWhiteSpace(this.ServerUrl))
                {
                    if (this.ServerUrl.Contains(','))
                    {
                        foreach (var url in ServerUrl.Split(','))
                        {
                            urls.Add(url);
                        }
                    }
                    else
                    {
                        urls.Add(this.ServerUrl);
                    }
                }
                return urls;
            }
        }

        public string TargetHostName { get; set; }

        public string CertificateName { get; set; }

        public string CertificateStoreName { get; set; }

        public string CertificateStoreLocation { get; set; }

        public string SslPassword { get; set; }

        public string SessionMode { get; set; }

        public string JmsUsernameIcon { get; set; }

        public string JmsPasswordIcon { get; set; }

        public string JndiUsernameIcon { get; set; }

        public string JndiPasswordIcon { get; set; }

        public string JmsUsernameMammoth { get; set; }

        public string JmsPasswordMammoth { get; set; }

        public string JndiUsernameMammoth { get; set; }

        public string JndiPasswordMammoth { get; set; }

        public string JmsUsernameEwic { get; set; }

        public string JmsPasswordEwic { get; set; }

        public string JndiUsernameEwic { get; set; }

        public string JndiPasswordEwic { get; set; }

        public EsbEnvironmentViewModel ToViewModel()
        {
            var viewModel = new EsbEnvironmentViewModel();

            viewModel.Name = this.Name;
            viewModel.EsbEnvironment = this.EsbEnvironment;
            viewModel.ServerUrl = this.ServerUrl;
            viewModel.TargetHostName = this.TargetHostName;
            viewModel.JmsUsernameIcon = this.JmsUsernameIcon;
            viewModel.JmsPasswordIcon = this.JmsPasswordIcon;
            viewModel.JndiUsernameIcon = this.JndiUsernameIcon;
            viewModel.JndiPasswordIcon = this.JndiPasswordIcon;
            viewModel.JmsUsernameMammoth = this.JmsUsernameMammoth;
            viewModel.JmsPasswordMammoth = this.JmsPasswordMammoth;
            viewModel.JndiUsernameMammoth = this.JndiUsernameMammoth;
            viewModel.JndiPasswordMammoth = this.JndiPasswordMammoth;
            viewModel.JmsUsernameEwic = this.JmsUsernameEwic;
            viewModel.JmsPasswordEwic = this.JmsPasswordEwic;
            viewModel.JndiUsernameEwic = this.JndiUsernameEwic;
            viewModel.JndiPasswordEwic = this.JndiPasswordEwic;

            return viewModel;
        }
    }
}