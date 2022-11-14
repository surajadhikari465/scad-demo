using GPMService.Producer.DataAccess;
using GPMService.Producer.ESB.Listener.NearRealTime;
using GPMService.Producer.Helpers;
using GPMService.Producer.Message.Parser;
using GPMService.Producer.Message.Processor;
using GPMService.Producer.Service;
using GPMService.Producer.Service.ESB.Listener;
using GPMService.Producer.Settings;
using Icon.Common.Email;
using Icon.DbContextFactory;
using Icon.Esb;
using Icon.Esb.ListenerApplication;
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
        public static Container InitializeContainer(int instance, string serviceType)
        {
            Container container = new Container();
            container.Register<IEmailClient>(() => { return EmailClient.CreateFromConfig(); }, Lifestyle.Singleton);
            container.Register<IDbContextFactory<MammothContext>, MammothContextFactory>();
            container.Register(() => GPMProducerServiceSettings.CreateSettings());
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
                    container.Register<IEsbSubscriber, EsbSubscriber>();
                    container.Register<ILogger<NearRealTimeMessageListener>, NLogLogger<NearRealTimeMessageListener>>();
                    container.Register<IMessageParser<items>, NearRealTimeMessageParser>();
                    container.Register<INearRealTimeProcessorDAL, NearRealTimeProcessorDAL>();
                    container.Register<IMessageProcessor, NearRealTimeMessageProcessor>();
                    container.Register<IListenerApplication, NearRealTimeMessageListener>();
                    container.Register<IGPMProducerService, NearRealTimeProducerService>();
                    Registration subscriberRegistration = container.GetRegistration(typeof(IEsbSubscriber)).Registration;
                    subscriberRegistration.SuppressDiagnosticWarning(DiagnosticType.DisposableTransientComponent, "Disposing of the subscriber is taken care of by the application.");
                    break;
                default:
                    throw new ArgumentException(
                        $"No type implementation exists for service type argument {serviceType}");
            }
        }
    }
}
