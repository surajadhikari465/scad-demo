using Icon.Dashboard.DataFileAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Icon.Dashboard.Mvc.ViewModels
{
    public class EsbEnvironmentViewModel
    {
        public EsbEnvironmentViewModel()
        {
            this.Applications = new List<IconApplicationIdentifierViewModel>();
        }

        public EsbEnvironmentViewModel(IEsbEnvironment model)
        {
            this.Name = model.Name;
            this.ServerUrl = model.ServerUrl;
            this.TargetHostName = model.TargetHostName;
            this.JmsUsername = model.JmsUsername;
            this.JmsPassword = model.JmsPassword;
            this.JndiUsername = model.JndiUsername;
            this.JndiPassword = model.JndiPassword;

            if (this.Applications == null)
            {
                this.Applications = new List<IconApplicationIdentifierViewModel>();
            }

            if (model.Applications != null)
            {
                foreach (var appDefinition in model.Applications)
                {
                    this.Applications.Add(new IconApplicationIdentifierViewModel(appDefinition.Name, appDefinition.Server));
                }
            }
        }

        public string Name { get; set; }

        public string ServerUrl { get; set; }

        public string TargetHostName { get; set; }

        public string JmsUsername { get; set; }

        public string JmsPassword { get; set; }

        public string JndiUsername { get; set; }

        public string JndiPassword { get; set; }

        public virtual IList<IconApplicationIdentifierViewModel> Applications { get; set; }

    }
}