using Icon.Dashboard.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Icon.Dashboard.Mvc.ViewModels
{
    public class EsbEnvironmentViewModel
    {
        public EsbEnvironmentViewModel()
        {
            this.AppsInEnvironment = new List<IconApplicationViewModel>();
        }

        public EsbEnvironmentViewModel(EsbEnvironmentElement configElement) : this()
        {
            this.Name = configElement.Name;
            this.ServerUrl = configElement.ServerUrl;
            this.TargetHostName = configElement.TargetHostName;
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

        public virtual IList<IconApplicationViewModel> AppsInEnvironment { get; set; }

        public string Name { get; set; }

        public EsbEnvironmentEnum EsbEnvironment { get; set; }

        [DisplayName("Server Url")]
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

        [DisplayName("Target Host Name")]
        public string TargetHostName { get; set; }

        [DisplayName("JMS Username for Icon Apps")]
        public string JmsUsernameIcon { get; set; }

        [DisplayName("JMS Password for Icon Apps")]
        public string JmsPasswordIcon { get; set; }

        [DisplayName("JNDI Username for Icon Apps")]
        public string JndiUsernameIcon { get; set; }

        [DisplayName("JNDI Password for Icon Apps")]
        public string JndiPasswordIcon { get; set; }

        [DisplayName("JMS Username for Mammoth Apps")]
        public string JmsUsernameMammoth { get; set; }

        [DisplayName("JMS Password for Mammoth Apps")]
        public string JmsPasswordMammoth { get; set; }

        [DisplayName("JNDI Username for Mammoth Apps")]
        public string JndiUsernameMammoth { get; set; }

        [DisplayName("JNDI Password for Mammoth Apps")]
        public string JndiPasswordMammoth { get; set; }

        [DisplayName("JMS Username for eWIC Apps")]
        public string JmsUsernameEwic { get; set; }

        [DisplayName("JMS Password for eWIC Apps")]
        public string JmsPasswordEwic { get; set; }

        [DisplayName("JNDI Username for eWIC Apps")]
        public string JndiUsernameEwic { get; set; }

        [DisplayName("JNDI Password for eWIC Apps")]
        public string JndiPasswordEwic { get; set; }
    }
}