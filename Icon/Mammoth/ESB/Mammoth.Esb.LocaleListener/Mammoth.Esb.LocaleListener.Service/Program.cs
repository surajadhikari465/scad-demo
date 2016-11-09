using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mammoth.Esb.LocaleListener.Service
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
#if DEBUG
            SimpleInjectorInitializer.InitializeContainer().GetInstance<MammothLocaleListener>().Run();

            System.Console.ReadLine();

            while (true)
            {
                Thread.Sleep(60000);
            }
#else
            ServiceBase[] ServicesToRun;

            ServicesToRun = new ServiceBase[] 
            { 
                new MammothLocaleListenerWindowsService() 
            };

            ServiceBase.Run(ServicesToRun);
#endif
        }
    }
}
