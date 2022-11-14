﻿using Icon.Logging;
using Mammoth.PrimeAffinity.Library.Esb;
using System.ServiceProcess;
using System.Threading;

namespace Mammoth.Esb.AttributeListener.Service
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
			var listener = container.GetInstance<AttributeListener>();
            listener.Start();
            System.Console.ReadLine();

            while (true)
            {
                Thread.Sleep(60000);
            }
#else
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new MammothAttributeListenerWindowsService()
            };

            ServiceBase.Run(ServicesToRun);
#endif
        }
    }
}
