using System.ServiceProcess;
using System.Threading;

namespace Icon.Esb.R10Listener.WindowsService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
#if DEBUG
            R10ListenerBuilder.Build().Run();

            System.Console.ReadLine();

            while (true)
            {
                Thread.Sleep(60000);
            }
#else
            ServiceBase[] ServicesToRun;

            ServicesToRun = new ServiceBase[] 
            { 
                new R10ResponseSubscriberWindowsService() 
            };

            ServiceBase.Run(ServicesToRun);
#endif
        }
    }
}
