using Icon.Logging;
using PushController.Common;
using PushController.Controller.Service;
using System;
using System.Configuration;
using Topshelf;

namespace PushController.Controller
{
    class Program
    {
        private static ILogger<Program> logger = new NLogLogger<Program>();

        static void Main(string[] args)
        {
            HostFactory.Run(r =>
            {
                r.Service<IPushControllerService>(s =>
                {
                    s.ConstructUsing(c => new PushControllerService());
                    s.WhenStarted(cm => cm.Start());
                    s.WhenStopped(cm => cm.Stop());
                });
                r.SetDescription("Processes updates from POS Pushes for R10 stores from IRMA to Icon.");
                r.SetDisplayName("Icon POS Push Controller");
                r.SetServiceName("PushController");
            });
        }
    }
}
