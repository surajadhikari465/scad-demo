using System;
using System.Configuration;
using MammothR10Price.Service;
using SimpleInjector;
using Topshelf;

namespace MammothR10Price
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string apiDescription = ConfigurationManager.AppSettings["ApiDescription"].ToString();
            string apiDisplayName = ConfigurationManager.AppSettings["ApiDisplayName"].ToString();
            string apiServiceName = ConfigurationManager.AppSettings["ApiServiceName"].ToString();
            string producerInstanceId = ConfigurationManager.AppSettings["ProducerInstanceId"].ToString();

            Container container = SimpleInjectorInitializer.InitializeContainer(
                int.Parse(producerInstanceId)
            );
            HostFactory.Run(r =>
            {
                r.Service<IProducerService>(s =>
                {
                    s.ConstructUsing(c => container.GetInstance<IProducerService>());
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
