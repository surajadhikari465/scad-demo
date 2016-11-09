namespace RegionalEventController.Controller
{
    using Icon.Logging;
    using Icon.Common.Email;
    using System;
    using RegionalEventController.Common;
    using RegionalEventController.Controller.Service;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using Topshelf;

    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(r =>
            {
                r.Service<IRegionalControllerService>(s =>
                {
                    s.ConstructUsing(c => new RegionalControllerService());
                    s.WhenStarted(cm => cm.Start());
                    s.WhenStopped(cm => cm.Stop());
                });
                r.SetDescription("Processes Regional Events from IRMA to Icon.");
                r.SetDisplayName("Icon Regional Event Controller");
                r.SetServiceName("RegionalEventController");
            });
        }
    }
}
