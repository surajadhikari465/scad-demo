using Icon.Logging;
using System.ServiceProcess;

namespace Icon.Esb.EwicAplListener.WindowsService
{
    public partial class EwicAplListenerWindowsService : ServiceBase
    {
        private NLogLogger<EwicAplListenerWindowsService> logger = new NLogLogger<EwicAplListenerWindowsService>();
        private EwicAplListener ewicListener;

        public EwicAplListenerWindowsService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            logger.Info("Starting eWIC APL listener...");

            ewicListener = EwicAplListenerBuilder.Build();
            ewicListener.Run();
        }

        protected override void OnStop()
        {
            ewicListener.Close();
        }
    }
}
