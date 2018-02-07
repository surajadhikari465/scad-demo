using Icon.Common;
using Icon.Logging;
using System;
using Topshelf;

namespace PrimeAffinityController
{
    class Program
    {
        static void Main(string[] args)
        {
            NLogLogger<Program> logger = new NLogLogger<Program>();

            try
            {
                string apiDescription = AppSettingsAccessor.GetStringSetting("ServiceDescription");
                string apiDisplayName = AppSettingsAccessor.GetStringSetting("ServiceDisplayName");
                string apiServiceName = AppSettingsAccessor.GetStringSetting("ServiceName");

                HostFactory.Run(r =>
                {
                    r.Service<IPrimeAffinityController>(s =>
                    {
                        s.ConstructUsing(c => SimpleInjectorInitializer.CreateContainer().GetInstance<PrimeAffinityController>());
                        s.WhenStarted(cm => cm.Start());
                        s.WhenStopped(cm => cm.Stop());
                    });
                    r.SetDescription(apiDescription);
                    r.SetDisplayName(apiDisplayName);
                    r.SetServiceName(apiServiceName);
                });
            }
            catch(Exception ex)
            {
                logger.Error(new
                    {
                        Message = "Failed to start the controller because of an unexpected error.",
                        Error = ex.ToString()
                    }.ToJson());
            }
        }
    }
}
