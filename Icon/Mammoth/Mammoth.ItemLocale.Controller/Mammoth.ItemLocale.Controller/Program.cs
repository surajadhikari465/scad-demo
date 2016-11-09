using Mammoth.Common.ControllerApplication;
using Mammoth.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace Mammoth.ItemLocale.Controller
{
    class Program
    {
        private static ILogger logger = new NLogLogger(typeof(Program));

        static void Main(string[] args)
        {
            // InstanceID moved to app.config
            // Change the InstanceID appsetting to deploy multiple instances.            
            HostFactory.Run(m =>
            {
                var container = SimpleInjectorInitializer.InitializeContainer();
                container.Verify();

                m.Service<ITopShelfService>(s =>
                {
                    s.ConstructUsing(c => container.GetInstance<ITopShelfService>());
                    s.WhenStarted(cm => cm.Start());
                    s.WhenStopped(cm => cm.Stop());
                });

                m.SetDescription("Processes events off the Mammoth.ItemLocaleChangeQueue and sends them to the WebApi.");
                m.SetDisplayName("Mammoth ItemLocale Controller");
                m.SetServiceName("Mammoth.ItemLocale.Controller");
            });
        }
    }
}
