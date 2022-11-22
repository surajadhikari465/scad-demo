using GPMService.Producer.DataAccess;
using GPMService.Producer.ErrorHandler;
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
using Icon.Esb.Schemas.Wfm.Contracts;
using Icon.Esb.Subscriber;
using Icon.Logging;
using InventoryProducer.Common.Schemas;
using Mammoth.Framework;
using SimpleInjector;
using SimpleInjector.Diagnostics;
using System;

namespace GPMService.Producer
{
    internal class SimpleInjectorInitializer
    {
        public static Container InitializeContainer(int instance, string serviceType)
        {
            Container container = new Container();
            container.Register<IEmailClient>(() => { return EmailClient.CreateFromConfig(); }, Lifestyle.Singleton);
            container.Register<IDbContextFactory<MammothContext>, MammothContextFactory>();
            container.Register(() => GPMProducerServiceSettings.CreateSettings());
            container.Register<ILogger<MessagePublisher>, NLogLogger<MessagePublisher>>();
            container.Register<IMessagePublisher, MessagePublisher>();
            container.Register<ErrorEventPublisher>();
            container.Register<ISerializer<ErrorMessage>, Serializer<ErrorMessage>>();
            RegisterServiceImplementation(container, serviceType);
            container.Verify();
            return container;
        }

        private static void RegisterServiceImplementation(Container container, string serviceType)
        {
            switch (serviceType)
            {
                case Constants.ProducerType.NearRealTime:
                    container.Register(() => ListenerApplicationSettings.CreateDefaultSettings<ListenerApplicationSettings>("GPM NearRealTimeMessage Listener"));
                    container.Register(() => EsbConnectionSettings.CreateSettingsFromConfig());
                    container.Register(() => ActiveMQConnectionSettings.CreateSettingsFromConfig());
                    container.Register<IEsbSubscriber, EsbSubscriber>();
                    container.Register<IEsbProducer, EsbProducer>();
                    container.Register<IActiveMQProducer, ActiveMQProducer>();
                    container.Register<ILogger<NearRealTimeMessageListener>, NLogLogger<NearRealTimeMessageListener>>();
                    container.Register<ILogger<NearRealTimeMessageProcessor>, NLogLogger<NearRealTimeMessageProcessor>>();
                    container.Register<ILogger<NearRealTimeProcessorDAL>, NLogLogger<NearRealTimeProcessorDAL>>();
                    container.Register<ILogger<NearRealTimeProducerService>, NLogLogger<NearRealTimeProducerService>>();
                    container.Register<IMessageParser<items>, NearRealTimeMessageParser>();
                    container.Register<INearRealTimeProcessorDAL, NearRealTimeProcessorDAL>();
                    container.Register<IMessageProcessor, NearRealTimeMessageProcessor>();
                    container.Register<IListenerApplication, NearRealTimeMessageListener>();
                    container.Register<IGPMProducerService, NearRealTimeProducerService>();
                    container.Register<ConfirmBODErrorHandler>();
                    container.Register<ProcessBODErrorHandler>();
                    container.Register<ISerializer<ConfirmBODType>, Serializer<ConfirmBODType>>();
                    container.Register<ISerializer<PriceChangeMaster>, Serializer<PriceChangeMaster>>();
                    container.Register<ISerializer<MammothPriceType>, Serializer<MammothPriceType>>();
                    container.Register<ISerializer<PriceMessageArchiveType>, Serializer<PriceMessageArchiveType>>();
                    // adding suppressions
                    container.GetRegistration(typeof(IEsbSubscriber)).Registration.SuppressDiagnosticWarning(DiagnosticType.DisposableTransientComponent, "Disposing of the subscriber is taken care of by the application.");
                    container.GetRegistration(typeof(IEsbProducer)).Registration.SuppressDiagnosticWarning(DiagnosticType.DisposableTransientComponent, "Disposing of the producer is taken care of by the application.");
                    container.GetRegistration(typeof(IActiveMQProducer)).Registration.SuppressDiagnosticWarning(DiagnosticType.DisposableTransientComponent, "Disposing of the ActiveMQ producer is taken care of by the application.");
                    break;
                case Constants.ProducerType.JustInTime.ActivePrice:
                    container.Register(() => ListenerApplicationSettings.CreateDefaultSettings<ListenerApplicationSettings>("GPM NearRealTimeMessage Listener"));
                    container.Register(() => EsbConnectionSettings.CreateSettingsFromConfig());
                    container.Register(() => ActiveMQConnectionSettings.CreateSettingsFromConfig());
                    container.Register<IEsbSubscriber, EsbSubscriber>();
                    container.Register<IEsbProducer, EsbProducer>();
                    container.Register<IActiveMQProducer, ActiveMQProducer>();
                    container.Register<ILogger<ActivePriceMessageListener>, NLogLogger<ActivePriceMessageListener>>();
                    container.Register<ILogger<ActivePriceMessageProcessor>, NLogLogger<ActivePriceMessageProcessor>>();
                    container.Register<ILogger<ActivePriceProcessorDAL>, NLogLogger<ActivePriceProcessorDAL>>();
                    container.Register<ILogger<ActivePriceProducerService>, NLogLogger<ActivePriceProducerService>>();
                    container.Register<IMessageParser<JobSchedule>, ActivePriceMessageParser>();
                    container.Register<IActivePriceProcessorDAL, ActivePriceProcessorDAL>();
                    container.Register<IMessageProcessor, ActivePriceMessageProcessor>();
                    container.Register<IListenerApplication, ActivePriceMessageListener>();
                    container.Register<IGPMProducerService, ActivePriceProducerService>();
                    container.Register<ISerializer<MammothPricesType>, Serializer<MammothPricesType>>();
                    // adding suppressions
                    container.GetRegistration(typeof(IEsbSubscriber)).Registration.SuppressDiagnosticWarning(DiagnosticType.DisposableTransientComponent, "Disposing of the subscriber is taken care of by the application.");
                    container.GetRegistration(typeof(IEsbProducer)).Registration.SuppressDiagnosticWarning(DiagnosticType.DisposableTransientComponent, "Disposing of the producer is taken care of by the application.");
                    container.GetRegistration(typeof(IActiveMQProducer)).Registration.SuppressDiagnosticWarning(DiagnosticType.DisposableTransientComponent, "Disposing of the ActiveMQ producer is taken care of by the application.");
                    break;
                default:
                    throw new ArgumentException(
                        $"No type implementation exists for service type argument {serviceType}");
            }
        }
    }
}
