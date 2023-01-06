using ErrorMessagesMonitor.Service;
using ErrorMessagesMonitor.Settings;
using Icon.Logging;
using Container = SimpleInjector.Container;

namespace ErrorMessagesMonitor
{
    internal class SimpleInjectorInitializer
    {
        public static Container InitializeContainer()
        {
            var container = new Container();
            var serviceSettings = ErrorMessagesMonitorServiceSettings.CreateSettingsFromConfig();

            container.RegisterSingleton<ILogger<ErrorMessagesMonitorService>, NLogLogger<ErrorMessagesMonitorService>>();
            container.RegisterSingleton<IErrorMessagesMonitorService, ErrorMessagesMonitorService>();
            // TODO - Add more attributes 
            container.Verify();
            return container;
        }
    }
}
