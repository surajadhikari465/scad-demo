using System.Threading;

namespace Mammoth.Esb.ProductListener.Service
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
#if DEBUG
            var container = SimpleInjectorInitializer.InitializeContainer();
            container.Verify();
            var listener = container.GetInstance<ProductListener>();
            listener.Run();

            System.Console.ReadLine();

            while (true)
            {
                Thread.Sleep(60000);
            }
#else
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new MammothProductListenerWindowsService()
            };

            ServiceBase.Run(ServicesToRun);
#endif
        }
    }
}
