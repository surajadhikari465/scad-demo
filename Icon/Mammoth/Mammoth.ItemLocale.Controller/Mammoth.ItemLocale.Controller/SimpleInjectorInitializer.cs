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
using Mammoth.ItemLocale.Controller.ApplicationModules;
using Mammoth.ItemLocale.Controller.DataAccess.Commands;
using Mammoth.ItemLocale.Controller.DataAccess.Models;
using Mammoth.ItemLocale.Controller.DataAccess.Queries;
using Mammoth.ItemLocale.Controller.Services;
using Mammoth.Logging;
using SimpleInjector;
using System.Collections.Generic;
using System.Reflection;

namespace Mammoth.ItemLocale.Controller
{
    public class SimpleInjectorInitializer
    {
        public static Container InitializeContainer()
        {
            var container = new Container();
            var dataAccessAssembly = Assembly.Load("Mammoth.ItemLocale.Controller.DataAccess");

            container.RegisterSingleton<ITopShelfService, ItemLocaleTopShelfService>();
            container.RegisterSingleton<ILogger>(() => new NLoggerInstance(typeof(NLoggerInstance),
                AppSettingsAccessor.GetStringSetting("InstanceID")));
            container.RegisterSingleton<IControllerApplication, ControllerApplication>();
            container.RegisterSingleton<IQueueManager<ItemLocaleEventModel>, ItemLocaleQueueManager>();
            container.RegisterSingleton<IService<ItemLocaleEventModel>, ItemLocaleService>();
            container.RegisterSingleton<IDbProvider, SqlDbProvider>();
            container.RegisterSingleton<IHttpClientWrapper, HttpClientWrapper>();
            container.RegisterSingleton<HttpClientSettings>(() => HttpClientSettings.CreateFromConfig());
            container.RegisterSingleton<IConnectionBuilder, RegionConnectionBuilder>();
            container.Register(typeof(ICommandHandler<>), new[] { dataAccessAssembly }, Lifestyle.Singleton);
            container.Register(typeof(IQueryHandler<,>), new[] { dataAccessAssembly }, Lifestyle.Singleton);
            container.RegisterDecorator(typeof(ICommandHandler<>), typeof(DbProviderCommandHandlerDecorator<>), Lifestyle.Singleton);
            container.RegisterDecorator(typeof(IQueryHandler<,>), typeof(DbProviderQueryHandlerDecorator<,>), Lifestyle.Singleton);
            container.RegisterDecorator(typeof(IService<ItemLocaleEventModel>), typeof(ValidateItemLocaleServiceDecorator), Lifestyle.Singleton);
            container.RegisterSingleton<IEmailClient>(() => EmailClient.CreateFromConfig());
            container.RegisterSingleton(typeof(IEmailMessageBuilder<>), typeof(EnumerableEmailMessageBuilder<>));

            var settingsRegistration = Lifestyle.Singleton.CreateRegistration<ItemLocaleControllerApplicationSettings>(() => ItemLocaleControllerApplicationSettings.CreateFromConfig(), container);
            container.AddRegistration(typeof(IRegionalControllerApplicationSettings), settingsRegistration);
            container.AddRegistration(typeof(IControllerApplicationSettings), settingsRegistration);
            container.AddRegistration(typeof(ItemLocaleControllerApplicationSettings), settingsRegistration);            

            return container;
        }
    }
}
