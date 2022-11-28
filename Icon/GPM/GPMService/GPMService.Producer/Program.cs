using Topshelf;
using System.Configuration;
using GPMService.Producer.Service;
using SimpleInjector;

namespace GPMService.Producer
{
    class Program
    {
        static void Main(string[] args)
        {
            string apiDescription = ConfigurationManager.AppSettings["ApiDescription"].ToString();
            string apiDisplayName = ConfigurationManager.AppSettings["ApiDisplayName"].ToString();
            string apiServiceName = ConfigurationManager.AppSettings["ApiServiceName"].ToString();
            string serviceType = ConfigurationManager.AppSettings["ServiceType"].ToString();
            Container container = SimpleInjectorInitializer.InitializeContainer(serviceType);

            HostFactory.Run(r =>
            {
                r.Service<IGPMProducerService>(s =>
                {
                    s.ConstructUsing(c => container.GetInstance<IGPMProducerService>());
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
