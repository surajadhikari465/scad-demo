using Mammoth.Common;
using Mammoth.Common.ControllerApplication;
using Mammoth.Common.ControllerApplication.ConnectionBuilders;
using Mammoth.Common.ControllerApplication.Http;
using Mammoth.Common.ControllerApplication.Models;
using Mammoth.Common.ControllerApplication.Services;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Common.DataAccess.ConnectionBuilders;
using Mammoth.Common.DataAccess.DbProviders;
using Mammoth.Common.DataAccess.Decorators;
using Mammoth.Common.Email;
using Mammoth.Logging;
using Mammoth.Price.Controller.ApplicationModules;
using Mammoth.Price.Controller.DataAccess.Commands;
using Mammoth.Price.Controller.DataAccess.Models;
using Mammoth.Price.Controller.DataAccess.Queries;
using Mammoth.Price.Controller.Services;
using SimpleInjector;
using System.Collections.Generic;
using System.Reflection;

namespace Mammoth.Price.Controller
{
    public class SimpleInjectorInitializer
    {
        public static Container InitializeContainer()
        {
            var container = new Container();
            var dataAccessAssembly = Assembly.Load("Mammoth.Price.Controller.DataAccess");

            container.RegisterSingleton<ILogger>(() => new NLoggerInstance(typeof(NLoggerInstance),
                AppSettingsAccessor.GetStringSetting("InstanceID")));
            container.RegisterSingleton<IControllerApplication, ControllerApplication>();
            container.RegisterSingleton<IQueueManager<PriceEventModel>, PriceQueueManager>();
            container.RegisterSingleton<IService<PriceEventModel>, PriceService>();
            container.RegisterSingleton<IDbProvider, SqlDbProvider>();
            container.RegisterSingleton<IHttpClientWrapper, HttpClientWrapper>();
            container.RegisterSingleton<HttpClientSettings>(() => HttpClientSettings.CreateFromConfig());
            container.RegisterSingleton<IConnectionBuilder, RegionConnectionBuilder>();
            container.RegisterSingleton<ITopShelfService, PriceTopShelfService>();

            container.Register(typeof(ICommandHandler<>), new[] { dataAccessAssembly }, Lifestyle.Singleton);
            container.Register(typeof(IQueryHandler<,>), new[] { dataAccessAssembly }, Lifestyle.Singleton);
            container.RegisterDecorator(typeof(ICommandHandler<>), typeof(DbProviderCommandHandlerDecorator<>), Lifestyle.Singleton);
            container.RegisterDecorator(typeof(IQueryHandler<,>), typeof(DbProviderQueryHandlerDecorator<,>), Lifestyle.Singleton);
            container.RegisterSingleton<IEmailClient>(() => EmailClient.CreateFromConfig());
            container.RegisterSingleton(typeof(IEmailMessageBuilder<>), typeof(EnumerableEmailMessageBuilder<>));

            var settingsRegistration = Lifestyle.Singleton.CreateRegistration<PriceControllerApplicationSettings>(() => PriceControllerApplicationSettings.CreateFromConfig(), container);
            container.AddRegistration(typeof(IRegionalControllerApplicationSettings), settingsRegistration);
            container.AddRegistration(typeof(IControllerApplicationSettings), settingsRegistration);
            container.AddRegistration(typeof(PriceControllerApplicationSettings), settingsRegistration);

            return container;
        }
    }
}
