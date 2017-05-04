using Icon.Dashboard.DataFileAccess.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
            this.ConnectionFactoryName = model.ConnectionFactoryName;
            this.SslPassword = model.SslPassword;
            this.QueueName = model.QueueName;
            this.SessionMode = model.SessionMode;
            this.CertificateName = model.CertificateName;
            this.CertificateStoreName = model.CertificateStoreName;
            this.CertificateStoreLocation = model.CertificateStoreLocation;
            this.ReconnectDelay = model.ReconnectDelay;
            this.NumberOfListenerThreads = model.NumberOfListenerThreads;

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

        [DisplayName("Server Url")]
        public string ServerUrl { get; set; }

        [DisplayName("Target Host")]
        public string TargetHostName { get; set; }

        [DisplayName("JMS Username")]
        public string JmsUsername { get; set; }

        [DisplayName("JMS Password")]
        public string JmsPassword { get; set; }

        [DisplayName("JNDI Username")]
        public string JndiUsername { get; set; }

        [DisplayName("JNDI Password")]
        public string JndiPassword { get; set; }

        [DisplayName("Cnxn. Factory")]
        public string ConnectionFactoryName { get; set; }

        [DisplayName("SSL Password")]
        public string SslPassword { get; set; }

        [DisplayName("Queue Name")]
        public string QueueName { get; set; }

        [DisplayName("Session Mode")]
        public string SessionMode { get; set; }

        [DisplayName("Cert. Name")]
        public string CertificateName { get; set; }

        [DisplayName("Cert. Store Name")]
        public string CertificateStoreName { get; set; }

        [DisplayName("Cert. Store Loc.")]
        public string CertificateStoreLocation { get; set; }

        [DisplayName("ReconnectDelay")]
        public string ReconnectDelay { get; set; }

        [DisplayName("# Listnr. Threads")]
        public int NumberOfListenerThreads { get; set; }

        public virtual IList<IconApplicationIdentifierViewModel> Applications { get; set; }
    }
}