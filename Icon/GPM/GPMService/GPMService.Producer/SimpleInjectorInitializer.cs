using GPMService.Producer.Archive;
using GPMService.Producer.DataAccess;
using GPMService.Producer.ErrorHandler;
using GPMService.Producer.ESB.Infrastructure;
using GPMService.Producer.ESB.Listener.JustInTime;
using GPMService.Producer.ESB.Listener.NearRealTime;
using GPMService.Producer.Helpers;
using GPMService.Producer.Message.Parser;
using GPMService.Producer.Message.Processor;
using GPMService.Producer.Publish;
using GPMService.Producer.Serializer;
using GPMService.Producer.Service;
using GPMService.Producer.Service.ESB.Listener;
using GPMService.Producer.Settings;
using Icon.ActiveMQ;
using Icon.ActiveMQ.Producer;
using Icon.Common.Email;
using Icon.DbContextFactory;
using Icon.Esb;
using Icon.Esb.ListenerApplication;
using Icon.Esb.Producer;
using Icon.Esb.Schemas.Infor;
using Icon.Esb.Schemas.Mammoth;
using Icon.Esb.Schemas.Wfm.Contracts;
using Icon.Esb.Subscriber;
using Icon.Logging;
using Mammoth.Framework;
using SimpleInjector;
using SimpleInjector.Diagnostics;
using System;

namespace GPMService.Producer
{
    internal class SimpleInjectorInitializer
    {
        public static Container InitializeContainer(string serviceType)
        {
            Container container = new Container();
            container.RegisterSingleton<IEmailClient>(() => { return EmailClient.CreateFromConfig(); });
            container.RegisterSingleton<IDbContextFactory<MammothContext>, MammothContextFactory>();
            container.RegisterSingleton(() => GPMProducerServiceSettings.CreateSettings());
            container.RegisterSingleton<ErrorEventPublisher>();
            container.RegisterSingleton<ISerializer<ErrorMessage>, Serializer<ErrorMessage>>();
            RegisterServiceImplementation(container, serviceType);
            container.Verify();
            return container;
        }

        private static void RegisterServiceImplementation(Container container, string serviceType)
        {
            switch (serviceType)
            {
                case Constants.ProducerType.NearRealTime:
                    container.RegisterSingleton(() => ListenerApplicationSettings.CreateDefaultSettings<ListenerApplicationSettings>("GPM NearRealTimeMessage Listener"));
                    container.RegisterSingleton(() => ActiveMQConnectionSettings.CreateSettingsFromConfig());
                    EsbConnectionSettings nearRealTimeListenerEsbSettings = EsbConnectionSettings.CreateSettingsFromNamedConnectionConfig("GPMNearRealTimeListenerEmsConnection");
                    container.RegisterConditional<EsbConnectionSettings>(
                        Lifestyle.Singleton.CreateRegistration(() => nearRealTimeListenerEsbSettings, container),
                        c => c.Consumer.Target.Name.Equals("nearRealTimeListenerEsbConnectionSettings"));
                    container.RegisterSingleton<IEsbSubscriber>(() => new EsbSubscriber(nearRealTimeListenerEsbSettings));
                    Registration nearRealTimeEsbProducerRegistration = Lifestyle.Singleton.CreateRegistration(() => new EsbProducer(EsbConnectionSettings.CreateSettingsFromNamedConnectionConfig("GPMNearRealTimeProducerEmsConnection")), container);
                    container.RegisterConditional<IEsbProducer>(
                        nearRealTimeEsbProducerRegistration, 
                        c => !c.HasConsumer || c.Consumer.Target.Name.Equals("nearRealTimeEsbProducer"));
                    Registration processBODEsbProducerRegistration = Lifestyle.Singleton.CreateRegistration(() => new EsbProducer(EsbConnectionSettings.CreateSettingsFromNamedConnectionConfig("GPMProcessBODProducerEmsConnection")), container);
                    container.RegisterConditional<IEsbProducer>(
                        processBODEsbProducerRegistration,
                        c => !c.HasConsumer || c.Consumer.Target.Name.Equals("processBODEsbProducer"));
                    Registration confirmBODEsbProducerRegistration = Lifestyle.Singleton.CreateRegistration(() => new EsbProducer(EsbConnectionSettings.CreateSettingsFromNamedConnectionConfig("GPMConfirmBODProducerEmsConnection")), container);
                    container.RegisterConditional<IEsbProducer>(
                        confirmBODEsbProducerRegistration,
                        c => !c.HasConsumer || c.Consumer.Target.Name.Equals("confirmBODEsbProducer"));
                    container.RegisterSingleton<IActiveMQProducer, ActiveMQProducer>();
                    container.RegisterSingleton<IMessagePublisher, NearRealTimeMessagePublisher>();
                    container.RegisterSingleton<ILogger<NearRealTimeMessagePublisher>, NLogLogger<NearRealTimeMessagePublisher>>();
                    container.RegisterSingleton<ILogger<NearRealTimeMessageListener>, NLogLogger<NearRealTimeMessageListener>>();
                    container.RegisterSingleton<ILogger<NearRealTimeMessageProcessor>, NLogLogger<NearRealTimeMessageProcessor>>();
                    container.RegisterSingleton<ILogger<NearRealTimeProcessorDAL>, NLogLogger<NearRealTimeProcessorDAL>>();
                    container.RegisterSingleton<ILogger<NearRealTimeProducerService>, NLogLogger<NearRealTimeProducerService>>();
                    container.RegisterSingleton<ILogger<ConfirmBODErrorHandler>, NLogLogger<ConfirmBODErrorHandler>>();
                    container.RegisterSingleton<ILogger<ProcessBODErrorHandler>, NLogLogger<ProcessBODErrorHandler>>();
                    container.RegisterSingleton<IMessageParser<items>, NearRealTimeMessageParser>();
                    container.RegisterSingleton<INearRealTimeProcessorDAL, NearRealTimeProcessorDAL>();
                    container.RegisterSingleton<IMessageProcessor, NearRealTimeMessageProcessor>();
                    container.RegisterSingleton<IListenerApplication, NearRealTimeMessageListener>();
                    container.RegisterSingleton<IGPMProducerService, NearRealTimeProducerService>();
                    container.RegisterSingleton<ConfirmBODErrorHandler>();
                    container.RegisterSingleton<ProcessBODErrorHandler>();
                    container.RegisterSingleton<ISerializer<ConfirmBODType>, Serializer<ConfirmBODType>>();
                    container.RegisterSingleton<ISerializer<PriceChangeMaster>, Serializer<PriceChangeMaster>>();
                    container.RegisterSingleton<ISerializer<MammothPriceType>, Serializer<MammothPriceType>>();
                    container.RegisterSingleton<ISerializer<PriceMessageArchiveType>, Serializer<PriceMessageArchiveType>>();
                    container.RegisterSingleton<ISerializer<items>, Serializer<items>>();
                    // adding suppressions
                    container.GetRegistration(typeof(IEsbSubscriber)).Registration.SuppressDiagnosticWarning(DiagnosticType.DisposableTransientComponent, "Disposing of the subscriber is taken care of by the application.");
                    nearRealTimeEsbProducerRegistration.SuppressDiagnosticWarning(DiagnosticType.DisposableTransientComponent, "Disposing of the producer is taken care of by the application.");
                    processBODEsbProducerRegistration.SuppressDiagnosticWarning(DiagnosticType.DisposableTransientComponent, "Disposing of the producer is taken care of by the application.");
                    confirmBODEsbProducerRegistration.SuppressDiagnosticWarning(DiagnosticType.DisposableTransientComponent, "Disposing of the producer is taken care of by the application.");
                    container.GetRegistration(typeof(IActiveMQProducer)).Registration.SuppressDiagnosticWarning(DiagnosticType.DisposableTransientComponent, "Disposing of the ActiveMQ producer is taken care of by the application.");
                    break;
                case Constants.ProducerType.JustInTime.ActivePrice:
                    container.RegisterSingleton(() => ListenerApplicationSettings.CreateDefaultSettings<ListenerApplicationSettings>("GPM ActivePrice Listener"));
                    EsbConnectionSettings activePriceListenerEsbSettings = EsbConnectionSettings.CreateSettingsFromNamedConnectionConfig("GPMActivePriceListenerEmsConnection");
                    container.RegisterConditional<EsbConnectionSettings>(
                        Lifestyle.Singleton.CreateRegistration(() => activePriceListenerEsbSettings, container),
                        c => c.Consumer.Target.Name.Equals("activePriceListenerEsbConnectionSettings"));
                    container.RegisterSingleton<IEsbSubscriber>(() => new Sb1EsbConsumer(activePriceListenerEsbSettings));
                    container.RegisterSingleton<IEsbProducer>(() => new Sb1EsbProducer(EsbConnectionSettings.CreateSettingsFromNamedConnectionConfig("GPMJustInTimeProducerEmsConnection")));
                    container.RegisterSingleton<IMessagePublisher, JustInTimeMessagePublisher>();
                    container.RegisterSingleton<ILogger<JustInTimeMessagePublisher>, NLogLogger<JustInTimeMessagePublisher>>();
                    container.RegisterSingleton<ILogger<ActivePriceMessageListener>, NLogLogger<ActivePriceMessageListener>>();
                    container.RegisterSingleton<ILogger<ActivePriceMessageProcessor>, NLogLogger<ActivePriceMessageProcessor>>();
                    container.RegisterSingleton<ILogger<ActivePriceProcessorDAL>, NLogLogger<ActivePriceProcessorDAL>>();
                    container.RegisterSingleton<ILogger<CommonDAL>, NLogLogger<CommonDAL>>();
                    container.RegisterSingleton<ILogger<ActivePriceProducerService>, NLogLogger<ActivePriceProducerService>>();
                    container.RegisterSingleton<ILogger<JustInTimePriceArchiver>, NLogLogger<JustInTimePriceArchiver>>();
                    container.RegisterSingleton<JustInTimePriceArchiver>();
                    container.RegisterSingleton<IMessageParser<JobSchedule>, ActivePriceMessageParser>();
                    container.RegisterSingleton<IActivePriceProcessorDAL, ActivePriceProcessorDAL>();
                    container.RegisterSingleton<ICommonDAL, CommonDAL>();
                    container.RegisterSingleton<IMessageProcessor, ActivePriceMessageProcessor>();
                    container.RegisterSingleton<IListenerApplication, ActivePriceMessageListener>();
                    container.RegisterSingleton<IGPMProducerService, ActivePriceProducerService>();
                    container.RegisterSingleton<ISerializer<MammothPricesType>, Serializer<MammothPricesType>>();
                    container.RegisterSingleton<ISerializer<JobSchedule>, Serializer<JobSchedule>>();
                    // adding suppressions
                    container.GetRegistration(typeof(IEsbSubscriber)).Registration.SuppressDiagnosticWarning(DiagnosticType.DisposableTransientComponent, "Disposing of the subscriber is taken care of by the application.");
                    container.GetRegistration(typeof(IEsbProducer)).Registration.SuppressDiagnosticWarning(DiagnosticType.DisposableTransientComponent, "Disposing of the producer is taken care of by the application.");
                    break;
                case Constants.ProducerType.JustInTime.ExpiringTpr:
                    container.RegisterSingleton(() => ListenerApplicationSettings.CreateDefaultSettings<ListenerApplicationSettings>("GPM ExpiringTpr Listener"));
                    EsbConnectionSettings expiringTprListenerEsbSettings = EsbConnectionSettings.CreateSettingsFromNamedConnectionConfig("GPMExpiringTprListenerEmsConnection");
                    container.RegisterConditional<EsbConnectionSettings>(
                        Lifestyle.Singleton.CreateRegistration(() => expiringTprListenerEsbSettings, container),
                        c => c.Consumer.Target.Name.Equals("expiringTprListenerEsbConnectionSettings"));
                    container.RegisterSingleton<IEsbSubscriber>(() => new Sb1EsbConsumer(expiringTprListenerEsbSettings));
                    container.RegisterSingleton<IEsbProducer>(() => new Sb1EsbProducer(EsbConnectionSettings.CreateSettingsFromNamedConnectionConfig("GPMJustInTimeProducerEmsConnection")));
                    container.RegisterSingleton<IMessagePublisher, JustInTimeMessagePublisher>();
                    container.RegisterSingleton<ILogger<JustInTimeMessagePublisher>, NLogLogger<JustInTimeMessagePublisher>>();
                    container.RegisterSingleton<ILogger<ExpiringTprMessageListener>, NLogLogger<ExpiringTprMessageListener>>();
                    container.RegisterSingleton<ILogger<ExpiringTprMessageProcessor>, NLogLogger<ExpiringTprMessageProcessor>>();
                    container.RegisterSingleton<ILogger<ExpiringTprProcessorDAL>, NLogLogger<ExpiringTprProcessorDAL>>();
                    container.RegisterSingleton<ILogger<CommonDAL>, NLogLogger<CommonDAL>>();
                    container.RegisterSingleton<ILogger<ExpiringTprProducerService>, NLogLogger<ExpiringTprProducerService>>();
                    container.RegisterSingleton<ILogger<JustInTimePriceArchiver>, NLogLogger<JustInTimePriceArchiver>>();
                    container.RegisterSingleton<JustInTimePriceArchiver>();
                    container.RegisterSingleton<IMessageParser<JobSchedule>, ExpiringTprMessageParser>();
                    container.RegisterSingleton<IExpiringTprProcessorDAL, ExpiringTprProcessorDAL>();
                    container.RegisterSingleton<ICommonDAL, CommonDAL>();
                    container.RegisterSingleton<IMessageProcessor, ExpiringTprMessageProcessor>();
                    container.RegisterSingleton<IListenerApplication, ExpiringTprMessageListener>();
                    container.RegisterSingleton<IGPMProducerService, ExpiringTprProducerService>();
                    container.RegisterSingleton<ISerializer<MammothPricesType>, Serializer<MammothPricesType>>();
                    container.RegisterSingleton<ISerializer<JobSchedule>, Serializer<JobSchedule>>();
                    // adding suppressions
                    container.GetRegistration(typeof(IEsbSubscriber)).Registration.SuppressDiagnosticWarning(DiagnosticType.DisposableTransientComponent, "Disposing of the subscriber is taken care of by the application.");
                    container.GetRegistration(typeof(IEsbProducer)).Registration.SuppressDiagnosticWarning(DiagnosticType.DisposableTransientComponent, "Disposing of the producer is taken care of by the application.");
                    break;
                case Constants.ProducerType.JustInTime.EmergencyPrice:
                    container.RegisterSingleton<IEsbProducer>(() => new Sb1EsbProducer(EsbConnectionSettings.CreateSettingsFromNamedConnectionConfig("GPMJustInTimeProducerEmsConnection")));
                    container.RegisterSingleton<IMessagePublisher, JustInTimeMessagePublisher>();
                    container.RegisterSingleton<ILogger<JustInTimeMessagePublisher>, NLogLogger<JustInTimeMessagePublisher>>();
                    container.RegisterSingleton<ILogger<EmergencyPriceMessageProcessor>, NLogLogger<EmergencyPriceMessageProcessor>>();
                    container.RegisterSingleton<ILogger<EmergencyPriceProcessorDAL>, NLogLogger<EmergencyPriceProcessorDAL>>();
                    container.RegisterSingleton<ILogger<EmergencyPriceProducerService>, NLogLogger<EmergencyPriceProducerService>>();
                    container.RegisterSingleton<ILogger<CommonDAL>, NLogLogger<CommonDAL>>();
                    container.RegisterSingleton<ILogger<JustInTimePriceArchiver>, NLogLogger<JustInTimePriceArchiver>>();
                    container.RegisterSingleton<JustInTimePriceArchiver>();
                    container.RegisterSingleton<IEmergencyPriceProcessorDAL, EmergencyPriceProcessorDAL>();
                    container.RegisterSingleton<ICommonDAL, CommonDAL>();
                    container.RegisterSingleton<IMessageProcessor, EmergencyPriceMessageProcessor>();
                    container.RegisterSingleton<IGPMProducerService, EmergencyPriceProducerService>();
                    container.RegisterSingleton<ISerializer<MammothPricesType>, Serializer<MammothPricesType>>();
                    container.RegisterSingleton<ISerializer<MammothPriceType>, Serializer<MammothPriceType>>();
                    // adding suppressions
                    container.GetRegistration(typeof(IEsbProducer)).Registration.SuppressDiagnosticWarning(DiagnosticType.DisposableTransientComponent, "Disposing of the producer is taken care of by the application.");
                    break;
                default:
                    throw new ArgumentException(
                        $"No type implementation exists for service type argument {serviceType}");
            }
        }
    }
}
