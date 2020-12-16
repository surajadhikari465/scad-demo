using Esb.Core.Serializer;
using Icon.Esb;
using Icon.Esb.Factory;
using Icon.Esb.Schemas.Wfm.Contracts;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Common.DataAccess.DbProviders;
using Mammoth.Common.Email;
using Mammoth.Logging;
using MammothWebApi.Common;
using MammothWebApi.DataAccess.Commands;
using MammothWebApi.DataAccess.Models;
using MammothWebApi.DataAccess.Queries;
using MammothWebApi.DataAccess.Settings;
using MammothWebApi.Email;
using MammothWebApi.Service.Decorators;
using MammothWebApi.Service.Services;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using SimpleInjector.Lifestyles;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Web.Http;
using MammothWebApi.DataAccess.Models.DataMonster;

namespace MammothWebApi
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
            container.RegisterSingleton<IEmailClient>(() => EmailClient.CreateFromConfig());
            container.Register<IDbProvider, SqlDbProvider>(Lifestyle.Scoped);

            // Services
            container.Register(typeof(IUpdateService<>), new[] { typeof(IUpdateService<>).Assembly }, Lifestyle.Scoped);
            container.Register(typeof(IQueryService<,>), new[] { typeof(IQueryService<,>).Assembly }, Lifestyle.Scoped);

            // Email Builders
            container.Register(typeof(IEmailMessageBuilder<>), typeof(EmailMessageBuilder<>), Lifestyle.Scoped);

            // Data Access
            var dataAccessAssembly = Assembly.Load("MammothWebApi.DataAccess");
            container.Register(typeof(IQueryHandler<,>), new[] { dataAccessAssembly }, Lifestyle.Scoped);
            container.RegisterDecorator<ICommandHandler<CancelAllSalesCommand>, RetryCommandHandlerDecorator<CancelAllSalesCommand>>(Lifestyle.Scoped);
            container.Register(() => DataAccessSettings.Load(), Lifestyle.Scoped);
            container.Register(typeof(ICommandHandler<>), new[] { dataAccessAssembly }, Lifestyle.Scoped);

            // Decorators  
            container.RegisterDecorator(typeof(IUpdateService<>), typeof(DbConnectionServiceDecorator<>));
            container.RegisterDecorator<IQueryHandler<GetItemNutritionAttributesByItemIdQuery, IEnumerable<ItemNutritionAttributes>>, DbConnectionQueryHandlerDecorator<GetItemNutritionAttributesByItemIdQuery, IEnumerable<ItemNutritionAttributes>>>();
            container.RegisterDecorator<IQueryHandler<GetAllBusinessUnitsQuery, List<int>>, DbConnectionQueryHandlerDecorator<GetAllBusinessUnitsQuery, List<int>>>();
            container.RegisterDecorator<IQueryHandler<GetLocalesByBusinessUnitsQuery, IEnumerable<Locales>>,
                DbConnectionQueryHandlerDecorator<GetLocalesByBusinessUnitsQuery, IEnumerable<Locales>>>(Lifestyle.Scoped);
            container.RegisterDecorator<IQueryHandler<GetItemPriceAttributesByStoreAndScanCodeQuery, IEnumerable<ItemStorePriceModel>>,
                DbConnectionQueryHandlerDecorator<GetItemPriceAttributesByStoreAndScanCodeQuery, IEnumerable<ItemStorePriceModel>>>(Lifestyle.Scoped);
            container.RegisterDecorator<IQueryHandler<GetItemsQuery, ItemComposite>,
                DbConnectionQueryHandlerDecorator<GetItemsQuery, ItemComposite>>(Lifestyle.Scoped);
            container.RegisterDecorator<IQueryHandler<GetItemsBySearchCriteriaQuery, IEnumerable<ItemDetail>>,
               DbConnectionQueryHandlerDecorator<GetItemsBySearchCriteriaQuery, IEnumerable<ItemDetail>>>(Lifestyle.Scoped);

            container.RegisterDecorator<IQueryHandler<GetHealthCheckQuery, int>,
                DbConnectionQueryHandlerDecorator<GetHealthCheckQuery, int>>(Lifestyle.Scoped);

            container.RegisterWebApiControllers(GlobalConfiguration.Configuration);
            container.Verify();

            GlobalConfiguration.Configuration.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container);
            GlobalConfiguration.Configure(WebApiConfig.Register);


       
        }

       
    }
}
