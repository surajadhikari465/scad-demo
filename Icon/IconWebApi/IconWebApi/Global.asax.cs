using Icon.Common.DataAccess;
using Icon.Logging;
using Icon.Shared.DataAccess.Dapper.DbProviders;
using IconWebApi.Common;
using IconWebApi.DataAccess.Models;
using IconWebApi.DataAccess.Queries;
using IconWebApi.Service.Decorators;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using SimpleInjector.Lifestyles;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace IconWebApi
{
	public class WebApiApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			// Simple Injector Registration
			var container = new Container();
			container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

			// Cross-cutting concerns
			container.RegisterSingleton<ILogger>(() => new NLogLogger(typeof(NLogLogger)));
			container.Register(typeof(Icon.Logging.ILogger<>), typeof(Icon.Logging.NLogLogger<>));
			container.RegisterSingleton<IServiceSettings>(() => ServiceSettings.CreateFromConfig());
			container.Register<IDbProvider, SqlDbProvider>(Lifestyle.Scoped);

			// Data Access
			var dataAccessAssembly = Assembly.Load("IconWebApi.DataAccess");
			container.Register(typeof(IQueryHandler<,>), new[] { dataAccessAssembly }, Lifestyle.Scoped);

			container.RegisterDecorator<IQueryHandler<GetLocalesQuery, IEnumerable<GenericLocale>>, 
				DbConnectionQueryHandlerDecorator<GetLocalesQuery, IEnumerable<GenericLocale>>>(Lifestyle.Scoped);
            container.RegisterDecorator<IQueryHandler<GetContactsByHierarchyClassIdsQuery, IEnumerable<AssociatedContact>>,
                DbConnectionQueryHandlerDecorator<GetContactsByHierarchyClassIdsQuery, IEnumerable<AssociatedContact>>>(Lifestyle.Scoped);
            container.RegisterWebApiControllers(GlobalConfiguration.Configuration);
			container.Verify();

			GlobalConfiguration.Configuration.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container);
			AreaRegistration.RegisterAllAreas();
			GlobalConfiguration.Configure(WebApiConfig.Register);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			//BundleConfig.RegisterBundles(BundleTable.Bundles);
		}

	}
}
