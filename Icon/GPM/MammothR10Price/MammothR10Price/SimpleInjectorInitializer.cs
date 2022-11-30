using Icon.Common.Email;
using Icon.Esb;
using Icon.Esb.ListenerApplication;
using Icon.Logging;
using SimpleInjector;
using MammothR10Price.Esb.Listener;
using MammothR10Price.Publish;
using MammothR10Price.Message.Processor;
using MammothR10Price.Service;

namespace MammothR10Price
{
    internal class SimpleInjectorInitializer
    {
        public static Container InitializeContainer()
        {
            var container = new Container();
            var serviceSettings = MammothR10PriceServiceSettings.CreateSettingsFromConfig();

            container.RegisterSingleton<IEmailClient>(() => { return EmailClient.CreateFromConfig(); });
            container.RegisterSingleton<IErrorEventPublisher, ErrorEventPublisher>();
            container.RegisterSingleton<IMessageProcessor, MammothR10PriceProcessor>();
            container.RegisterSingleton<MammothR10PriceListener>();
            container.RegisterSingleton(() => serviceSettings);
            container.RegisterSingleton(() => EsbConnectionSettings.CreateSettingsFromConfig());
            container.RegisterSingleton(() => ListenerApplicationSettings.CreateDefaultSettings(serviceSettings.ApplicationName));
            container.RegisterSingleton<ILogger<MammothR10PriceProcessor>, NLogLogger<MammothR10PriceProcessor>>();
            container.RegisterSingleton<ILogger<MammothR10PriceService>, NLogLogger<MammothR10PriceService>>();
            container.RegisterSingleton<ILogger<MammothR10PriceListener>, NLogLogger<MammothR10PriceListener>>();
            container.RegisterSingleton<IProducerService, MammothR10PriceService>();
            
            return container;
        }
    }
}
