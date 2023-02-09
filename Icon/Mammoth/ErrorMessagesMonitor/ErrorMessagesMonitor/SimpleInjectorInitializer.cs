using ErrorMessagesMonitor.DataAccess;
using ErrorMessagesMonitor.Message.Processor;
using ErrorMessagesMonitor.Service;
using ErrorMessagesMonitor.Settings;
using Icon.Common.Email;
using Icon.DbContextFactory;
using Icon.Logging;
using Mammoth.Framework;
using OpsgenieAlert;
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
            container.RegisterSingleton<IErrorMessagesProcessor, ErrorMessagesProcessor>();
            container.RegisterSingleton<IErrorMessagesMonitorService, ErrorMessagesMonitorService>();
            container.RegisterSingleton<IErrorMessagesMonitorDAL, ErrorMessagesMonitorDAL>();
            container.RegisterSingleton<IDbContextFactory<MammothContext>, MammothContextFactory>();
            container.RegisterSingleton(() => serviceSettings);
            container.RegisterSingleton<IEmailClient>(() => { return EmailClient.CreateFromConfig(); });
            container.RegisterSingleton<IOpsgenieAlert, OpsgenieAlert.OpsgenieAlert>();
            container.RegisterSingleton<ILogger<ErrorMessagesProcessor>, NLogLogger<ErrorMessagesProcessor>>();
            container.Verify();
            return container;
        }
    }
}
