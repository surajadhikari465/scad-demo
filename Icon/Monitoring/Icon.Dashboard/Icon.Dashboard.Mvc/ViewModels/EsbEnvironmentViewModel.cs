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
            this.AppsInEnvironment = new List<IconApplicationViewModel>();
        }

        public EsbEnvironmentViewModel(IEsbEnvironmentDefinition model)
            : this(model, (IEnumerable<IconApplicationViewModel>)null) { }

        public EsbEnvironmentViewModel(IEsbEnvironmentDefinition model,
            IEnumerable<IconApplicationViewModel> apps) : this()
        {
            this.Name = model.Name;
            this.ServerUrl = model.ServerUrl;
            this.TargetHostName = model.TargetHostName;
            this.JmsUsername = model.JmsUsername;
            this.JmsPassword = model.JmsPassword;
            this.JndiUsername = model.JndiUsername;
            this.JndiPassword = model.JndiPassword;
            //this.ConnectionFactoryName = model.ConnectionFactoryName;
            this.SslPassword = model.SslPassword;
            //this.QueueName = model.QueueName;
            this.SessionMode = model.SessionMode;
            this.CertificateName = model.CertificateName;
            this.CertificateStoreName = model.CertificateStoreName;
            this.CertificateStoreLocation = model.CertificateStoreLocation;
            this.ReconnectDelay = model.ReconnectDelay;
            this.NumberOfListenerThreads = model.NumberOfListenerThreads;

            this.AppsInEnvironment = (apps == null) 
                ? new List<IconApplicationViewModel>() 
                : apps.OrderBy(a => a.Name).ToList();
        }

        public IEsbEnvironmentDefinition ToDataModel()
        {
            IEsbEnvironmentDefinition environment = new EsbEnvironmentDefinition
            {
                Name = this.Name,
                ServerUrl = this.ServerUrl,
                TargetHostName = this.TargetHostName,
                JmsUsername = this.JmsUsername,
                JmsPassword = this.JmsPassword,
                JndiUsername = this.JndiUsername,
                JndiPassword = this.JndiPassword,
                //ConnectionFactoryName = this.ConnectionFactoryName,
                SslPassword = this.SslPassword,
                //QueueName = this.QueueName,
                SessionMode = this.SessionMode,
                CertificateName = this.CertificateName,
                CertificateStoreName = this.CertificateStoreName,
                CertificateStoreLocation = this.CertificateStoreLocation,
                ReconnectDelay = this.ReconnectDelay,
                NumberOfListenerThreads = this.NumberOfListenerThreads,
            };

            return environment;
        }

        public string Name { get; set; }

        [DisplayName("Server Url")]
        public string ServerUrl { get; set; }

        [DisplayName("Target Host Name")]
        public string TargetHostName { get; set; }

        [DisplayName("JMS Username")]
        public string JmsUsername { get; set; }

        [DisplayName("JMS Password")]
        public string JmsPassword { get; set; }

        [DisplayName("JNDI Username")]
        public string JndiUsername { get; set; }

        [DisplayName("JNDI Password")]
        public string JndiPassword { get; set; }

        //[DisplayName("Cnxn. Factory")]
        //public string ConnectionFactoryName { get; set; }

        [DisplayName("SSL Password")]
        public string SslPassword { get; set; }

        //[DisplayName("Queue Name")]
        //public string QueueName { get; set; }

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

        [DisplayName("Host")]
        public string TargetHostNameOnly
        {
            get
            {
                if (TargetHostName != null && TargetHostName.Contains('.'))
                {
                    return TargetHostName.Split('.')[0];
                }
                return String.Empty;
            }
        }

        public virtual IList<IconApplicationViewModel> AppsInEnvironment { get; set; }
    }
}