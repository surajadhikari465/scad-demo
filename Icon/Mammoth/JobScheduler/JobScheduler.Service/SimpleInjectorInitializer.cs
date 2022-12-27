using Icon.Common.Email;
using Icon.DbContextFactory;
using Icon.Esb.Schemas.Mammoth;
using Icon.Logging;
using JobScheduler.Service.DataAccess;
using JobScheduler.Service.ErrorHandler;
using JobScheduler.Service.Processor;
using JobScheduler.Service.Publish;
using JobScheduler.Service.Serializer;
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
            container.RegisterSingleton<ILogger<JobSchedulerProcessor>, NLogLogger<JobSchedulerProcessor>>();
            container.RegisterSingleton<IJobScheduerDAL, JobScheduerDAL>();
            container.RegisterSingleton<IMessagePublisher, MesssagePublisher>();
            container.RegisterSingleton<IErrorEventPublisher, ErrorEventPublisher>();
            container.RegisterSingleton<ISerializer<JobSchedule>, Serializer<JobSchedule>>();
            container.RegisterSingleton<ISerializer<ErrorMessage>, Serializer<ErrorMessage>>();
            container.Verify();
            return container;
        }
    }
}
