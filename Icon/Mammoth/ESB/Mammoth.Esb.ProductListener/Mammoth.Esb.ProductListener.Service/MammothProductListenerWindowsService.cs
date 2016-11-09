using System.ServiceProcess;
using SimpleInjector;

namespace Mammoth.Esb.ProductListener.Service
{
    public partial class MammothProductListenerWindowsService : ServiceBase
    {
        ProductListener listener;

        public MammothProductListenerWindowsService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            listener = SimpleInjectorInitializer.InitializeContainer().GetInstance<ProductListener>();
            listener.Run();
        }

        protected override void OnStop()
        {
            listener.Close();
        }
    }
}
