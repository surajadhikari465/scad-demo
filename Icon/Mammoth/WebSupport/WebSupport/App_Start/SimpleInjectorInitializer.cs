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
        }
    }
}