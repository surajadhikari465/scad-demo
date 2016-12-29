
namespace GlobalEventController.Controller
{
    using Service;
    using System.Configuration;
    using Topshelf;
    using System;


    class Program
    {
        static void Main(string[] args)
        {
            string serviceDescription = ConfigurationManager.AppSettings["ServiceDescription"].ToString();
            string serviceDisplayName = ConfigurationManager.AppSettings["ServiceDisplayName"].ToString();
            string serviceName = ConfigurationManager.AppSettings["ServiceName"].ToString();

            HostFactory.Run(r =>
            {
                r.Service<IGlobalEventControllerService>(s =>
                {
                    s.ConstructUsing(c => new GlobalEventControllerService());
                    s.WhenStarted(cm => cm.Start());
                    s.WhenStopped(cm => cm.Stop());
                });                    
                r.SetDescription(serviceDescription);
                r.SetDisplayName(serviceDisplayName);
                r.SetServiceName(serviceName);
            });
        }
    }
}
