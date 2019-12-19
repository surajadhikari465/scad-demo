using System.ServiceProcess;
using SimpleInjector;

namespace Mammoth.Esb.AttributeListener.Service
{
    public partial class MammothAttributeListenerWindowsService : ServiceBase
    {
        AttributeListener listener;

        public MammothAttributeListenerWindowsService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            listener = SimpleInjectorInitializer.InitializeContainer().GetInstance<AttributeListener>();
            listener.Run();
        }

        protected override void OnStop()
        {
            listener.Close();
        }
    }
}
