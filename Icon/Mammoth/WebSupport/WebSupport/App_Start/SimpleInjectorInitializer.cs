[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(WebSupport.App_Start.SimpleInjectorInitializer), "Initialize")]

namespace WebSupport.App_Start
{
    using System.Reflection;
    using System.Web.Mvc;

    using DataAccess;
    using Icon.Common.DataAccess;
    using Icon.Logging;
    using SimpleInjector;
    using SimpleInjector.Integration.Web;
    using SimpleInjector.Integration.Web.Mvc;
    using WebSupport.Models;
    using Esb.Core.EsbServices;
    using WebSupport.ViewModels;
    using Icon.Esb;
    using Icon.Esb.Factory;
    using Mammoth.Framework;
    using Esb.Core.MessageBuilders;
    using Icon.Esb.Schemas.Wfm.Contracts;
    using Esb.Core.Serializer;

    public class SimpleInjectorInitializer
    {
        public static void Initialize()
        {
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();

            InitializeContainer(container);

            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());

            container.Verify();

            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
        }

        private static void InitializeContainer(Container container)
        {
            // TODO: Have reviewed by someone for lifetime management.
            container.RegisterSingleton<ILogger>(() => new NLogLoggerSingleton(typeof(NLogLoggerSingleton)));
            container.Register<IIrmaContextFactory, IrmaContextFactory>(Lifestyle.Scoped);
            container.Register(typeof(ICommandHandler<>), new[] { Assembly.Load("WebSupport.DataAccess") }, Lifestyle.Scoped);
            container.Register(typeof(IQueryHandler<,>), new[] { Assembly.Load("WebSupport.DataAccess") }, Lifestyle.Scoped);
            container.Register<IEsbService<PriceResetRequestViewModel>, WebSupportPriceMessageService>(Lifestyle.Scoped);
            container.Register<EsbConnectionSettings>(() => EsbConnectionSettings.CreateSettingsFromConfig(), Lifestyle.Scoped);
            container.Register<IEsbConnectionFactory, EsbConnectionFactory>(Lifestyle.Scoped);
            container.Register<MammothContext>(Lifestyle.Scoped);
            container.Register<IMessageBuilder<PriceResetMessageBuilderModel>, PriceResetMessageBuilder>(Lifestyle.Scoped);
            container.Register<ISerializer<items>, Serializer<items>>(Lifestyle.Scoped);
        }
    }
}