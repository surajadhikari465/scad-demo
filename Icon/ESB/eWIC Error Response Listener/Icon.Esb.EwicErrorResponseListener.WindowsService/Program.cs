using System;
using System.ServiceProcess;
using System.Threading;

namespace Icon.Esb.EwicErrorResponseListener.WindowsService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
#if DEBUG
            EwicErrorResponseListenerBuilder.Build().Run();

            Console.ReadLine();

            while (true)
            {
                Thread.Sleep(60000);
            }
#else
            ServiceBase[] ServicesToRun;

            ServicesToRun = new ServiceBase[] 
            { 
                new EwicErrorResponseListenerWindowsService() 
            };

            ServiceBase.Run(ServicesToRun);
#endif
        }
    }
}