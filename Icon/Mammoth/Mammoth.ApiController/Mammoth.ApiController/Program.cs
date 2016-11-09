using Icon.ApiController.Controller;
using Icon.Logging;
using System;
using System.Linq;

namespace Mammoth.ApiController
{

    using Topshelf;
    using System.Configuration;
    using Mammoth.ApiController.Service;
    public class Program
    {
        static void Main(string[] args)
        {
            string apiDescription = ConfigurationManager.AppSettings["ApiDescription"].ToString();
            string apiDisplayName = ConfigurationManager.AppSettings["ApiDisplayName"].ToString();
            string apiServiceName = ConfigurationManager.AppSettings["ApiServiceName"].ToString();

            HostFactory.Run(r =>
            {
                r.Service<IApiControllerService>(s =>
                {
                    s.ConstructUsing(c => new ApiControllerService());
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
