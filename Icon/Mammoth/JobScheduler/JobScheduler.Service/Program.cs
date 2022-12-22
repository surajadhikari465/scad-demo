using JobScheduler.Service.Service;
using SimpleInjector;
using System.Configuration;
using Topshelf;

namespace JobScheduler.Service
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
                r.Service<IJobSchedulerService>(s =>
                {
                    s.ConstructUsing(c => container.GetInstance<IJobSchedulerService>());
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
