using Icon.Common.Email;
using Icon.DbContextFactory;
using Icon.Esb.Producer;
using Icon.Esb;
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
using SimpleInjector.Diagnostics;
using JobScheduler.Service.Infra;

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
            container.RegisterSingleton<ILogger<MesssagePublisher>, NLogLogger<MesssagePublisher>>();
            Registration extractServiceEsbProducerRegistration = Lifestyle.Singleton.CreateRegistration(() => new Sb1EsbProducer(EsbConnectionSettings.CreateSettingsFromNamedConnectionConfig("ExtractServiceProducerEmsConnection")), container);
            container.RegisterConditional<IEsbProducer>(
                extractServiceEsbProducerRegistration,
                c => !c.HasConsumer || c.Consumer.Target.Name.Equals("extractServiceEsbProducer"));
            Registration activePriceEsbProducerRegistration = Lifestyle.Singleton.CreateRegistration(() => new Sb1EsbProducer(EsbConnectionSettings.CreateSettingsFromNamedConnectionConfig("ActivePriceServiceProducerEmsConnection")), container);
            container.RegisterConditional<IEsbProducer>(
                activePriceEsbProducerRegistration,
                c => !c.HasConsumer || c.Consumer.Target.Name.Equals("activePriceServiceEsbProducer"));
            Registration expiringTprEsbProducerRegistration = Lifestyle.Singleton.CreateRegistration(() => new Sb1EsbProducer(EsbConnectionSettings.CreateSettingsFromNamedConnectionConfig("ExpiringTprServiceProducerEmsConnection")), container);
            container.RegisterConditional<IEsbProducer>(
                expiringTprEsbProducerRegistration,
                c => !c.HasConsumer || c.Consumer.Target.Name.Equals("expiringTprServiceEsbProducer"));
            container.RegisterSingleton<IS3Facade, S3Facade>();
            // adding suppressions
            extractServiceEsbProducerRegistration.SuppressDiagnosticWarning(DiagnosticType.DisposableTransientComponent, "Disposing of the producer is taken care of by the application.");
            activePriceEsbProducerRegistration.SuppressDiagnosticWarning(DiagnosticType.DisposableTransientComponent, "Disposing of the producer is taken care of by the application.");
            expiringTprEsbProducerRegistration.SuppressDiagnosticWarning(DiagnosticType.DisposableTransientComponent, "Disposing of the producer is taken care of by the application.");
            container.Verify();
            return container;
        }
    }
}
