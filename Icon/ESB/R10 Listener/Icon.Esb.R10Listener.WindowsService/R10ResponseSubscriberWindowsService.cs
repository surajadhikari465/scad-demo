using System.ServiceProcess;

namespace Icon.Esb.R10Listener.WindowsService
{
    public partial class R10ResponseSubscriberWindowsService : ServiceBase
    {
        private R10Listener r10Listener;

        public R10ResponseSubscriberWindowsService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            r10Listener = R10ListenerBuilder.Build();
            r10Listener.Run();
        }

        protected override void OnStop()
        {
            r10Listener.Close();
        }
    }
}
