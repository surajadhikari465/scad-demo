using System.ServiceProcess;
using System.Threading;

namespace Icon.Esb.CchTax.WindowsService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
#if DEBUG
            CchTaxListenerBuilder.Build().Start();

            while (true)
            {
                Thread.Sleep(60000);
            }
#else
            ServiceBase[] ServicesToRun;

            ServicesToRun = new ServiceBase[] 
            { 
                new CchTaxSubscriberWindowsService() 
            };

            ServiceBase.Run(ServicesToRun);
#endif            
        }
    }
}
