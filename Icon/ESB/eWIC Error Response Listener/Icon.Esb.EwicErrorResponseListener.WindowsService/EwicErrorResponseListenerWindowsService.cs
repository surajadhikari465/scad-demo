using Icon.Logging;
using System.ServiceProcess;

namespace Icon.Esb.EwicErrorResponseListener.WindowsService
{
    public partial class EwicErrorResponseListenerWindowsService : ServiceBase
    {
        private NLogLogger<EwicErrorResponseListenerWindowsService> logger = new NLogLogger<EwicErrorResponseListenerWindowsService>();
        private EwicErrorResponseListener ewicListener;

        public EwicErrorResponseListenerWindowsService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            logger.Info("Starting eWIC error response listener...");

            ewicListener = EwicErrorResponseListenerBuilder.Build();
            ewicListener.Run();
        }

        protected override void OnStop()
        {
            ewicListener.Close();
        }
    }
}
