using BulkItemUploadProcessor.Service.Interfaces;
using Icon.Common;
using SimpleInjector;
using System;
using Topshelf;
using Topshelf.SimpleInjector;

namespace BulkItemUploadProcessor.Service
{
    class Program
    {
        private static Container container;

        static void Main(string[] args)
        {
            container = new SimpleInjectorInitializer().Initialize(); 

            var rc = HostFactory.Run(config =>
            {
                config.UseSimpleInjector(container);

                config.Service<IService>(s =>
                {
                    s.ConstructUsingSimpleInjector();
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });
                config.RunAsLocalSystem();

                config.SetDescription(AppSettingsAccessor.GetStringSetting("ServiceDescription"));
                config.SetDisplayName(AppSettingsAccessor.GetStringSetting("ServiceDisplayName"));
                config.SetServiceName(AppSettingsAccessor.GetStringSetting("ServiceName"));
            });

            var exitCode = (int) Convert.ChangeType(rc, rc.GetTypeCode());
            Environment.ExitCode = exitCode;
        }

    }
}
