using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vim.Common.ControllerApplication;
using Vim.Common.ControllerApplication.Http;
using Vim.Common.ControllerApplication.Services;
using Vim.Common.DataAccess;
using Vim.Common.DataAccess.ConnectionBuilders;
using Vim.Common.DataAccess.Decorators;
using Vim.Common.Email;
using Vim.Locale.Controller.ApplicationModules;
using Vim.Locale.Controller.DataAccess.Models;
using Vim.Locale.Controller.Services;
using Vim.Logging;

namespace Vim.Locale.Controller
{
    public static class SimpleInjectorInitializer
    {
        public static Container InitializeContainer(int instance)
        {
            var container = new Container();

            container.RegisterSingleton<ILogger>(() => new NLoggerInstance(typeof(NLoggerInstance), instance.ToString()));
            container.RegisterSingleton<IControllerApplication, ControllerApplication>();
            container.RegisterDecorator(typeof(IControllerApplication), typeof(LoggingControllerDecorator), Lifestyle.Singleton);
            container.RegisterSingleton<IQueueManager<LocaleEventModel>, LocaleQueueManager>();
            container.RegisterSingleton<ControllerApplicationSettings>(() => ControllerApplicationSettings.CreateFromConfig(instance));
            container.RegisterSingleton<IService<LocaleEventModel>, LocaleService>();
            container.RegisterSingleton<IDbProvider, SqlDbProvider>();
            container.RegisterSingleton<IHttpClientWrapper, HttpClientWrapper>();
            container.RegisterSingleton<HttpClientSettings>(() => HttpClientSettings.CreateFromConfig());
            container.RegisterSingleton<IConnectionBuilder>(() => new ConnectionBuilder("Icon"));
            container.Register(typeof(ICommandHandler<>), new[] { System.Reflection.Assembly.Load("Vim.Locale.Controller.DataAccess") }, Lifestyle.Singleton);
            container.Register(typeof(IQueryHandler<,>), new[] { System.Reflection.Assembly.Load("Vim.Locale.Controller.DataAccess") }, Lifestyle.Singleton);
            container.Register(typeof(ICommandHandler<>), new[] { System.Reflection.Assembly.Load("Vim.Common.DataAccess") }, Lifestyle.Singleton);
            container.Register(typeof(IQueryHandler<,>), new[] { System.Reflection.Assembly.Load("Vim.Common.DataAccess") }, Lifestyle.Singleton);
            container.RegisterDecorator(typeof(ICommandHandler<>), typeof(DbProviderCommandHandlerDecorator<>), Lifestyle.Singleton);
            container.RegisterDecorator(typeof(IQueryHandler<,>), typeof(DbProviderQueryHandlerDecorator<,>), Lifestyle.Singleton);
            container.RegisterSingleton<IEmailClient>(() => EmailClient.CreateFromConfig());
            container.RegisterSingleton(typeof(IEmailMessageBuilder<>), typeof(EnumerableEmailMessageBuilder<>));

            return container;
        }
    }
}
