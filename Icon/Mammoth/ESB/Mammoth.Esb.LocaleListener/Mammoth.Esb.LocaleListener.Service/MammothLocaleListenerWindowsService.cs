using System.ServiceProcess;

namespace Mammoth.Esb.LocaleListener.Service
{
    public partial class MammothLocaleListenerWindowsService : ServiceBase
    {
        MammothLocaleListener listener;

        public MammothLocaleListenerWindowsService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            listener = SimpleInjectorInitializer.InitializeContainer().GetInstance<MammothLocaleListener>();
            listener.Start();
        }

        protected override void OnStop()
        {
            listener.Stop();
        }
    }
}
