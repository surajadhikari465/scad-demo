using Icon.Common.Email;
using Icon.Esb.CchTax.Commands;
using Icon.Esb.CchTax.Models;
using Icon.Esb.Factory;
using Icon.Logging;
using System;
using System.Configuration;
using System.Linq;
using System.ServiceProcess;
using System.Threading;

namespace Icon.Esb.CchTax.WindowsService
{
    public partial class CchTaxSubscriberWindowsService : ServiceBase
    {
        private CchTaxListener listener;

        public CchTaxSubscriberWindowsService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            listener = CchTaxListenerBuilder.Build();
            listener.Run();
        }

        protected override void OnStop()
        {
            listener.Close();
        }
    }
}
