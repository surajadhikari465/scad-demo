namespace Icon.Monitoring
{
    using System.Reflection;

    using Icon.Common.DataAccess;
    using Icon.Common.Email;
    using Icon.Logging;
    using Icon.Monitoring.Common.Opsgenie;
    using Icon.Monitoring.Common.Settings;
    using Icon.Monitoring.DataAccess;
    using Icon.Monitoring.DataAccess.Decorators;
    using Icon.Monitoring.DataAccess.Queries;
    using Icon.Monitoring.Monitors;
    using Icon.Monitoring.Service;

    using SimpleInjector;
    using NodaTime;
    using Common.IO;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Configuration;
    using Icon.Monitoring.Common;
    using Icon.Monitoring.Common.ApiController;

    public class SimpleInjectorInitializer
    {
        public static Container InitializeContainer()
        {
            var dataAccessAssembly = Assembly.Load("Icon.Monitoring.DataAccess, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");

            var container = new Container();
            container.RegisterSingleton<IConnectionBuilder, ConnectionBuilder>();
            container.RegisterSingleton<ILogger>(() => new NLogLoggerSingleton(typeof(IMonitorService)));
            container.RegisterSingleton<IDbProvider, SqlDbProvider>();
            container.RegisterSingleton<IOpsgenieTrigger, OpsgenieTrigger>();
            container.RegisterSingleton<IEmailClient>(() => EmailClient.CreateFromConfig());
            container.RegisterSingleton<IDateTimeZoneProvider>(() => DateTimeZoneProviders.Tzdb);
            container.RegisterSingleton<IClock>(() => SystemClock.Instance);

            container.Register(typeof(ICommandHandler<>), new[] { dataAccessAssembly }, Lifestyle.Singleton);
            container.Register(typeof(IQueryHandler<,>), new[] { dataAccessAssembly }, Lifestyle.Singleton);
            container.Register(typeof(IQueryByRegionHandler<,>), new[] { dataAccessAssembly }, Lifestyle.Singleton);
            container.Register(typeof(IQueryHandlerMammoth<,>), new[] { dataAccessAssembly }, Lifestyle.Singleton);

            container.RegisterDecorator(typeof(ICommandHandler<>), typeof(IconCommandHandlerDecorator<>), Lifestyle.Singleton);
            container.RegisterDecorator(typeof(IQueryHandler<,>), typeof(IconQueryHandlerDecorator<,>), Lifestyle.Singleton);
            container.RegisterDecorator(typeof(IQueryByRegionHandler<,>), typeof(IrmaQueryHandlerDecorator<,>), Lifestyle.Singleton);
            container.RegisterDecorator(typeof(IQueryHandlerMammoth<,>), typeof(MammothQueryHandlerDecorator<,>), Lifestyle.Singleton);

            container.RegisterSingleton<IDvoBulkImportJobMonitorSettings, DvoBulkImportJobMonitorSettings>();
            container.RegisterSingleton<IFileInfoAccessor, FileInfoAccessor>();

            container.RegisterSingleton<ITLogConJobMonitorSettings, TLogConJobMonitorSettings>();
            container.RegisterSingleton<IVimLocaleConJobMonitorSettings, VimLocaleConJobMonitorSettings>();
            container.RegisterSingleton<IMammothPrimeAffinityControllerMonitorSettings, MammothPrimeAffinityControllerMonitorSettings>();
            container.RegisterSingleton<IMammothExpiringTprServiceMonitorSettings, MammothExpiringTprServiceMonitorSettings>();
            container.RegisterSingleton<IMammothActivePriceServiceMonitorSettings, MammothActivePriceServiceMonitorSettings>();
            container.RegisterSingleton<IMonitorCache, MonitorCache>();

            container.RegisterSingleton<IMonitorService, MonitorService>();
            container.RegisterSingleton<IMonitorSettings>(() => MonitorSettings.CreateFromConfig());
            var monitors = GetMonitorsRegisteredInAppConfig();
            container.Collection.Register(typeof(IMonitor), monitors);

            container.Register<MessageQueueCache>();
            container.Register<MammothMessageQueueCache>();

            container.Verify();
            return container;
        }

        private static IEnumerable<Type> GetMonitorsRegisteredInAppConfig()
        {
            var monitorNamesInAppConfig = ConfigurationManager.AppSettings.AllKeys
                .Where(k => k.EndsWith("Timer"))
                .Select(k => k.Replace("Timer", ""))
                .ToList();

            return Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.BaseType == typeof(TimedControllerMonitor)
                    && monitorNamesInAppConfig.Contains(t.Name))
                .ToList();
        }
    }
}