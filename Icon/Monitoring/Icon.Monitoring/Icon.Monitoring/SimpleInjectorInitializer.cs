namespace Icon.Monitoring
{
    using System.Reflection;

    using Icon.Common.DataAccess;
    using Icon.Common.Email;
    using Icon.Logging;
    using Icon.Monitoring.Common.PagerDuty;
    using Icon.Monitoring.Common.Settings;
    using Icon.Monitoring.DataAccess;
    using Icon.Monitoring.DataAccess.Decorators;
    using Icon.Monitoring.DataAccess.Queries;
    using Icon.Monitoring.Monitors;
    using Icon.Monitoring.Service;

    using SimpleInjector;
    using NodaTime;
    using Common.IO;

    public class SimpleInjectorInitializer
    {
        public static Container InitializeContainer()
        {
            var dataAccessAssembly = Assembly.Load("Icon.Monitoring.DataAccess, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");

            var container = new Container();
            container.RegisterSingleton<IConnectionBuilder, ConnectionBuilder>();           
            container.RegisterSingleton<ILogger>(() => new NLogLoggerSingleton(typeof(IMonitorService)));
            container.RegisterSingleton<IDbProvider, SqlDbProvider>();
            container.RegisterSingleton<IPagerDutyTrigger, PagerDutyTrigger>();
            container.RegisterSingleton<IEmailClient>(() => EmailClient.CreateFromConfig());
            container.RegisterSingleton<IDateTimeZoneProvider>(() => DateTimeZoneProviders.Tzdb);
            container.RegisterSingleton<IClock>(() => SystemClock.Instance);

            container.RegisterSingleton<IMonitorService, MonitorService>();
            container.RegisterSingleton<IMonitorSettings>(() => MonitorSettings.CreateFromConfig());
            container.RegisterCollection(typeof(IMonitor), new[] { typeof(SimpleInjectorInitializer).Assembly });

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

            container.Verify();
            return container;
        }
    }
}

