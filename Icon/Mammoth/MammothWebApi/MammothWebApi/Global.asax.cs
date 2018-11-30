using Esb.Core.MessageBuilders;
using Esb.Core.Serializer;
using Icon.Esb;
using Icon.Esb.Factory;
using Icon.Esb.Schemas.Wfm.Contracts;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Common.DataAccess.DbProviders;
using Mammoth.Common.Email;
using Mammoth.Logging;
using Mammoth.PrimeAffinity.Library.Commands;
using Mammoth.PrimeAffinity.Library.Esb;
using Mammoth.PrimeAffinity.Library.MessageBuilders;
using Mammoth.PrimeAffinity.Library.Processors;
using MammothWebApi.BackgroundJobs;
using MammothWebApi.Common;
using MammothWebApi.DataAccess.Commands;
using MammothWebApi.DataAccess.Models;
using MammothWebApi.DataAccess.Queries;
using MammothWebApi.DataAccess.Settings;
using MammothWebApi.Email;
using MammothWebApi.Service.Decorators;
using MammothWebApi.Service.Services;
using Quartz;
using Quartz.Impl;
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
            container.Register<IPrimeAffinityPsgSettings, PrimeAffinityPsgSettings>(Lifestyle.Scoped);
            container.Register(() => PrimeAffinityPsgProcessorSettings.Load());
            container.Register(() => PrimeAffinityMessageBuilderSettings.Load());

            // Services
            container.Register(typeof(IUpdateService<>), new[] { typeof(IUpdateService<>).Assembly }, Lifestyle.Scoped);
            container.Register(typeof(IQueryService<,>), new[] { typeof(IQueryService<,>).Assembly }, Lifestyle.Scoped);

            // Email Builders
            container.Register(typeof(IEmailMessageBuilder<>), typeof(EmailMessageBuilder<>), Lifestyle.Scoped);

            // Data Access
            var dataAccessAssembly = Assembly.Load("MammothWebApi.DataAccess");
            var primeAffinityLibraryAssembly = Assembly.GetAssembly(typeof(ArchivePrimeAffinityMessageCommandHandler));
            container.Register(typeof(ICommandHandler<>), new[] { dataAccessAssembly, primeAffinityLibraryAssembly }, Lifestyle.Scoped);
            container.Register(typeof(IQueryHandler<,>), new[] { dataAccessAssembly }, Lifestyle.Scoped);
            container.RegisterDecorator<ICommandHandler<CancelAllSalesCommand>, RetryCommandHandlerDecorator<CancelAllSalesCommand>>(Lifestyle.Scoped);
            container.Register(() => DataAccessSettings.Load(), Lifestyle.Scoped);

            //PrimeAffinity
            container.Register<IMessageBuilder<PrimeAffinityMessageBuilderParameters>, PrimeAffinityMessageBuilder>();
            container.Register<IPrimeAffinityPsgProcessor<PrimeAffinityPsgProcessorParameters>, PrimeAffinityPsgProcessor>();
            container.Register<IEsbConnectionFactory>(() => new EsbConnectionFactory { Settings = EsbConnectionSettings.CreateSettingsFromNamedConnectionConfig("R10") });
            container.Register<IDbConnection>(() => new SqlConnection(ConfigurationManager.ConnectionStrings["Mammoth"].ConnectionString), Lifestyle.Scoped);
            container.Register<ISerializer<items>, Serializer<items>>();
            container.Register<IEsbConnectionCacheFactory, EsbConnectionCacheFactory>();

            // Decorators  
            container.RegisterDecorator(typeof(IUpdateService<AddUpdatePrice>), typeof(PrimeAffinityPsgAddPriceServiceDecorator));
            container.RegisterDecorator(typeof(IUpdateService<DeletePrice>), typeof(PrimeAffinityPsgDeletePriceServiceDecorator));
            container.RegisterDecorator(typeof(IUpdateService<CancelAllSales>), typeof(PrimeAffinityPsgCancelAllSalesPriceServiceDecorator));
            container.RegisterDecorator(typeof(IUpdateService<>), typeof(DbConnectionServiceDecorator<>));
            container.RegisterDecorator<IQueryHandler<GetItemNutritionAttributesByItemIdQuery, IEnumerable<ItemNutritionAttributes>>, DbConnectionQueryHandlerDecorator<GetItemNutritionAttributesByItemIdQuery, IEnumerable<ItemNutritionAttributes>>>();
            container.RegisterDecorator<IQueryHandler<GetAllBusinessUnitsQuery, List<int>>, DbConnectionQueryHandlerDecorator<GetAllBusinessUnitsQuery, List<int>>>();
            container.RegisterDecorator<IQueryHandler<GetLocalesByBusinessUnitsQuery, IEnumerable<Locales>>,
                DbConnectionQueryHandlerDecorator<GetLocalesByBusinessUnitsQuery, IEnumerable<Locales>>>(Lifestyle.Scoped);
            container.RegisterDecorator<IQueryHandler<GetItemPriceAttributesByStoreAndScanCodeQuery, IEnumerable<ItemStorePriceModel>>,
                DbConnectionQueryHandlerDecorator<GetItemPriceAttributesByStoreAndScanCodeQuery, IEnumerable<ItemStorePriceModel>>>(Lifestyle.Scoped);
            container.RegisterDecorator<IQueryHandler<GetItemsQuery, ItemComposite>,
                DbConnectionQueryHandlerDecorator<GetItemsQuery, ItemComposite>>(Lifestyle.Scoped);

            
            container.Register<ReconnectToEsbJob>();

            container.RegisterWebApiControllers(GlobalConfiguration.Configuration);
            container.Verify();

            GlobalConfiguration.Configuration.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container);
            GlobalConfiguration.Configure(WebApiConfig.Register);

            EsbConnectionCache.EsbConnectionSettings = EsbConnectionSettings.CreateSettingsFromNamedConnectionConfig("R10");
            EsbConnectionCache.InitializeConnectionFactoryAndConnection();
            ScheduleEsbReconnectJob(container);
        }

        private void ScheduleEsbReconnectJob(Container container)
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler().Result;
            scheduler.JobFactory = new JobFactory(container);
            scheduler.Start();

            IJobDetail job = JobBuilder.Create<ReconnectToEsbJob>().Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithSimpleSchedule(
                    s => s.WithIntervalInSeconds(3)
                        .RepeatForever())
                .Build();

            scheduler.ScheduleJob(job, trigger);
        }
    }
}
