﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace OOSCollectorService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            Bootstrap();
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
			{ 
				new CollectorService() 
			};
            ServiceBase.Run(ServicesToRun);
        }

        private static void Bootstrap()
        {
            Bootstrapper.Bootstrap();
        }

    }
}
