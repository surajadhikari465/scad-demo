using Icon.Esb.Subscriber;
using System;
using System.ServiceProcess;
using System.Threading;
using System.Xml.Linq;
using TIBCO.EMS;

namespace Icon.Esb.EwicAplListener.WindowsService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
#if DEBUG
            bool readLocalFile = false;

            if (readLocalFile)
            {
                string path = @"";
                string xml = XDocument.Load(path).ToString();
                var textMessage = new TextMessage(null, xml);
                var esbMessage = new EsbMessage(textMessage);
                    
                var listener = EwicAplListenerBuilder.Build();

                var args = new EsbMessageEventArgs
                {
                    Message = esbMessage
                };

                listener.HandleMessage(null, args);
            }
            else
            {
                EwicAplListenerBuilder.Build().Run();

                Console.ReadLine();

                while (true)
                {
                    Thread.Sleep(60000);
                }
            }
#else
            ServiceBase[] ServicesToRun;

            ServicesToRun = new ServiceBase[] 
            { 
                new EwicAplListenerWindowsService() 
            };

            ServiceBase.Run(ServicesToRun);
#endif
        }
    }
}