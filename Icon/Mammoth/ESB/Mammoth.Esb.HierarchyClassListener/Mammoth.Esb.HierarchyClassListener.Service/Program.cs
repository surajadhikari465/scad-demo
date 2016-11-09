﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mammoth.Esb.HierarchyClassListener.Service
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
#if DEBUG
            SimpleInjectorInitializer.Initialize().GetInstance<MammothHierarchyClassListener>().Run();

            System.Console.ReadLine();

            while (true)
            {
                Thread.Sleep(60000);
            }
#else
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new MammothHierarchyClassListenerService()
            };
            ServiceBase.Run(ServicesToRun);
#endif
        }
    }
}
