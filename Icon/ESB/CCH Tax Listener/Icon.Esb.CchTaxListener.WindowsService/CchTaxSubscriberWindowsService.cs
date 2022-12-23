using System.ServiceProcess;

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
            listener.Start();
        }

        protected override void OnStop()
        {
            listener.Stop();
        }
    }
}
