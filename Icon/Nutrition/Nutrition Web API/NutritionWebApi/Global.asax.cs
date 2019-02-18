using Icon.Logging;
using NutritionWebApi.Common;
using NutritionWebApi.Common.Interfaces;
using NutritionWebApi.Common.Models;
using NutritionWebApi.DataAccess.Commands;
using NutritionWebApi.DataAccess.Decorators;
using NutritionWebApi.DataAccess.Queries;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using System.Collections.Generic;
using System.Web.Http;

namespace NutritionWebApi
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode,
    // visit http://go.microsoft.com/?LinkId=9394801

    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // Simple Injector Registration
            var container = new Container();

            container.RegisterWebApiRequest<IDbConnectionProvider, DbConnectionProvider>();
            container.RegisterWebApiRequest<IQueryHandler<GetNutritionItemQuery, List<NutritionItemModel>>, GetNutritionItemQueryHandler>();
            container.RegisterWebApiRequest<ICommandHandler<AddOrUpdateNutritionItemCommand>, AddOrUpdateNutritionItemCommandHandler>();
            container.RegisterWebApiRequest<ICommandHandler<DeleteNutritionCommand>, DeleteNutritionCommandHandler>();
            
            container.RegisterDecorator(typeof(IQueryHandler<GetNutritionItemQuery, List<NutritionItemModel>>),
                typeof(DbQueryHandlerDecorator<GetNutritionItemQuery, List<NutritionItemModel>>));
            container.RegisterDecorator(typeof(ICommandHandler<AddOrUpdateNutritionItemCommand>),
                typeof(DbCommandHandlerDecorator<AddOrUpdateNutritionItemCommand>));
            container.RegisterDecorator(typeof(ICommandHandler<DeleteNutritionCommand>), typeof(DbCommandHandlerDecorator<DeleteNutritionCommand>));

            container.Register<ILogger>(() => new NLogLoggerSingleton(typeof(NLogLoggerSingleton)), Lifestyle.Singleton);

            container.RegisterWebApiControllers(GlobalConfiguration.Configuration);
            
            container.Verify();

            GlobalConfiguration.Configuration.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container);
            WebApiConfig.Register(GlobalConfiguration.Configuration);
        }
    }
}