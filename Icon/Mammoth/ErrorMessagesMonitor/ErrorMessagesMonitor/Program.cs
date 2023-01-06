using System.Configuration;
using ErrorMessagesMonitor.Service;
using Topshelf;
using Container = SimpleInjector.Container;

namespace ErrorMessagesMonitor
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string apiDescription = ConfigurationManager.AppSettings["ApiDescription"].ToString();
            string apiDisplayName = ConfigurationManager.AppSettings["ApiDisplayName"].ToString();
            string apiServiceName = ConfigurationManager.AppSettings["ApiServiceName"].ToString();

            Container container = SimpleInjectorInitializer.InitializeContainer();
            HostFactory.Run(r =>
            {
                r.Service<IErrorMessagesMonitorService>(s =>
                {
                    s.ConstructUsing(c => container.GetInstance<IErrorMessagesMonitorService>());
                    s.WhenStarted(cm => cm.Start());
                    s.WhenStopped(cm => cm.Stop());
                });
                r.SetDescription(apiDescription);
                r.SetDisplayName(apiDisplayName);
                r.SetServiceName(apiServiceName);
            });
        }
    }
}
