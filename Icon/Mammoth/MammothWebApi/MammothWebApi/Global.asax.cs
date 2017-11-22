﻿using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Common.DataAccess.DbProviders;
using Mammoth.Common.Email;
using Mammoth.Logging;
using MammothWebApi.Common;
using MammothWebApi.DataAccess.Models;
using MammothWebApi.DataAccess.Queries;
using MammothWebApi.Email;
using MammothWebApi.Service.Decorators;
using MammothWebApi.Service.Services;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Http;

namespace MammothWebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // Simple Injector Registration
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new WebApiRequestLifestyle();

            // Cross-cutting concerns
            container.RegisterSingleton<ILogger>(() => new NLogLogger(typeof(NLogLogger)));
            container.RegisterSingleton<IServiceSettings>(() => ServiceSettings.CreateFromConfig());
            container.RegisterSingleton<IEmailClient>(() => EmailClient.CreateFromConfig());
            container.RegisterWebApiRequest<IDbProvider, SqlDbProvider>();

            // Services
            container.Register(typeof(IUpdateService<>), new[] { typeof(IUpdateService<>).Assembly }, Lifestyle.Scoped);
            container.Register(typeof(IQueryService<,>), new[] { typeof(IQueryService<,>).Assembly }, Lifestyle.Scoped);
            
            // Email Builders
            container.Register(typeof(IEmailMessageBuilder<>), typeof(EmailMessageBuilder<>), Lifestyle.Scoped);

            // Data Access
            var dataAccessAssembly = Assembly.Load("MammothWebApi.DataAccess");
            container.Register(typeof(ICommandHandler<>), new[] { dataAccessAssembly }, Lifestyle.Scoped);
            container.Register(typeof(IQueryHandler<,>), new[] { dataAccessAssembly }, Lifestyle.Scoped);

            // Decorators
            container.RegisterDecorator(typeof(IUpdateService<AddUpdatePrice>), typeof(CancelAllSalesAddUpdatePriceServiceDecorator));
            container.RegisterDecorator(typeof(IUpdateService<>), typeof(DbConnectionServiceDecorator<>));
            container.RegisterDecorator<IQueryHandler<GetAllBusinessUnitsQuery, List<int>>, DbConnectionQueryHandlerDecorator<GetAllBusinessUnitsQuery, List<int>>>();
            container.RegisterDecorator<IQueryHandler<GetLocalesByBusinessUnitsQuery, IEnumerable<Locales>>,
                DbConnectionQueryHandlerDecorator<GetLocalesByBusinessUnitsQuery, IEnumerable<Locales>>>(Lifestyle.Scoped);
            container.RegisterDecorator<IQueryHandler<GetItemPriceAttributesByStoreAndScanCodeQuery, IEnumerable<ItemStorePriceModel>>,
                DbConnectionQueryHandlerDecorator<GetItemPriceAttributesByStoreAndScanCodeQuery, IEnumerable<ItemStorePriceModel>>>(Lifestyle.Scoped);

            container.RegisterWebApiControllers(GlobalConfiguration.Configuration);
            container.Verify();

            GlobalConfiguration.Configuration.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container);
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
