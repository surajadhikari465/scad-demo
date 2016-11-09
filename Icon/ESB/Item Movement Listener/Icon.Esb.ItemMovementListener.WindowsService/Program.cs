using System.ServiceProcess;
using System.Threading;

namespace Icon.Esb.ItemMovementListener.WindowsService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
#if DEBUG
            ItemMovementListenerBuilder.Build().Run();

            while (true)
            {
                Thread.Sleep(60000);
            }
#else
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new ItemMovementSubscriberWindowsService() 
            };
            ServiceBase.Run(ServicesToRun);
#endif            
        }
    }
}
