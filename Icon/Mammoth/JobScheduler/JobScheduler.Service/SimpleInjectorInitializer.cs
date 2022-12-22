using Icon.Common.Email;
using Icon.DbContextFactory;
using Icon.Logging;
using JobScheduler.Service.Processor;
using JobScheduler.Service.Service;
using JobScheduler.Service.Settings;
using Mammoth.Framework;
using SimpleInjector;

namespace JobScheduler.Service
{
    internal class SimpleInjectorInitializer
    {
        public static Container InitializeContainer()
        {
            Container container = new Container();
            container.RegisterSingleton<IJobSchedulerService, JobSchedulerService>();
            container.RegisterSingleton(() => JobSchedulerServiceSettings.CreateSettings());
            container.RegisterSingleton<IJobSchedulerProcessor, JobSchedulerProcessor>();
            container.RegisterSingleton<ILogger<JobSchedulerService>, NLogLogger<JobSchedulerService>>();
            container.RegisterSingleton<IEmailClient>(() => { return EmailClient.CreateFromConfig(); });
            container.RegisterSingleton<IDbContextFactory<MammothContext>, MammothContextFactory>();
            container.Verify();
            return container;
        }
    }
}
