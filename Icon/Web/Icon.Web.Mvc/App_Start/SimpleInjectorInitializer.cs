using Icon.Common.Validators;
using Icon.Common.Validators.ItemAttributes;

namespace Icon.Web.Mvc.App_Start
{
    using AutoMapper;
    using FluentValidation;
    using Icon.Common;
    using Icon.Common.DataAccess;
    using Icon.Esb.Factory;
    using Icon.Ewic.Serialization.Serializers;
    using Icon.Ewic.Transmission.Producers;
    using Icon.FeatureFlags;
    using Icon.Framework;
    using Icon.Logging;
    using Icon.Shared.DataAccess.Dapper.ConnectionBuilders;
    using Icon.Shared.DataAccess.Dapper.DbProviders;
    using Icon.Shared.DataAccess.Dapper.Decorators;
    using Icon.Web.DataAccess.Commands;
    using Icon.Web.DataAccess.Decorators;
    using Icon.Web.DataAccess.Infrastructure;
    using Icon.Web.DataAccess.Managers;
    using Icon.Web.DataAccess.Queries;
    using Icon.Web.Mvc.Domain.BulkImport;
    using Icon.Web.Mvc.Exporters;
    using Icon.Web.Mvc.Importers;
    using Icon.Web.Mvc.InfragisticsHelpers;
    using Icon.Web.Mvc.RegionalItemCatalogs;
    using Icon.Web.Mvc.Utility.ItemHistory;
    using Icon.Web.Mvc.Validators;
    using Icon.Web.Mvc.Validators.Managers;
    using SimpleInjector;
    using SimpleInjector.Integration.Web;
    using SimpleInjector.Integration.Web.Mvc;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.Reflection;
    using System.Web.Mvc;
    using Utility;

    public static class SimpleInjectorInitializer
    {
        public static Container Initialize(IMapper mapper = null)
        {
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();

            container.Register<IconContext>(() => GetIconContext(), Lifestyle.Scoped);
            container.RegisterSingleton<IDbProvider, SqlDbProvider>();
            container.Register<IConnectionBuilder>(() => new ConnectionBuilder("Icon"), Lifestyle.Scoped);
            container.Register<IDbConnection>(() => new SqlConnection(ConfigurationManager.ConnectionStrings["Icon"].ConnectionString), Lifestyle.Scoped);

            container.RegisterDecorator<IManagerHandler<AddItemManager>, RetryUniqueConstraintManagerHandlerDecorator<AddItemManager>>(Lifestyle.Transient);
            container.Register(typeof(ICommandHandler<>), typeof(AddAddressCommand).Assembly);
            container.Register(typeof(IQueryHandler<,>), typeof(GetAffinitySubBricksParameters).Assembly);
            container.Register(typeof(IManagerHandler<>), typeof(IManagerHandler<>).Assembly);
            container.Register(typeof(ISpreadsheetImporter<>), typeof(ISpreadsheetImporter<>).Assembly);
            container.Register(typeof(ISerializer<>), typeof(ISerializer<>).Assembly);
            container.RegisterSingleton<ILogger>(() => new NLogLoggerSingleton(typeof(NLogLoggerSingleton)));
            container.Register<IExcelExporterService, ExcelExporterService>(Lifestyle.Transient);
            container.Register<IGenericQuery, GenericGetDbSet>(Lifestyle.Transient);
            container.Register<IRegionalItemCatalogFactory, RegionalItemCatalogFactory>();
            container.Register<IObjectValidator<AddEwicExclusionManager>, AddEwicExclusionManagerValidator>(Lifestyle.Transient);
            container.Register<IObjectValidator<AddEwicMappingManager>, AddEwicMappingManagerValidator>(Lifestyle.Transient);
            container.Register<IMessageProducer, EwicMessageProducer>(Lifestyle.Singleton);
            container.Register<IEsbConnectionFactory, EsbConnectionFactory>(Lifestyle.Singleton);
            container.Register<IInfragisticsHelper, InfragisticsHelper>(Lifestyle.Singleton);
            container.RegisterSingleton(() => IconWebAppSettings.CreateSettingsFromConfig());

            container.Register<IAttributesHelper, AttributesHelper>();
            container.Register<IDonutCacheManager, DonutCacheManager>();
            container.Register<IItemQueryBuilder, ItemQueryBuilder>();

            container.Register<IValidatorFactory, SimpleInjectorValidatorFactory>(Lifestyle.Singleton);
            container.Register(typeof(IValidator<>), typeof(ItemCreateViewModelValidator).Assembly);
            // Add unregistered type resolution for objects missing an IValidator<T>
            // This should be placed after the registration of IValidator<>
            container.RegisterConditional(typeof(IValidator<>), typeof(ValidateNothingDecorator<>), Lifestyle.Singleton, context => !context.Handled);

            container.Register<IItemAttributesValidatorFactory, ItemAttributesValidatorFactory>();
            container.Register<IItemHistoryBuilder, ItemHistoryBuilder>();
            container.Register<IHistoryModelTransformer, HistoryModelTransformer>();

            container.Register<IBulkUploadService, BulkUploadService>();

            container.Register<IFeatureFlagService, FeatureFlagService>();

            container.RegisterDecorator(typeof(ICommandHandler<UpdateHierarchyClassTraitCommand>), typeof(UpdateSubTeamMammothEventDecorator));
            container.RegisterDecorator(typeof(ICommandHandler<UpdateHierarchyClassTraitCommand>), typeof(UpdateItemTypeHierarchyClassTraitDecorator));
            container.RegisterDecorator(typeof(ICommandHandler<UpdateHierarchyClassTraitCommand>), typeof(UpdateItemProhibitDiscountHierarchyClassTraitDecorator));
            container.RegisterDecorator(typeof(ICommandHandler<UpdateHierarchyClassTraitCommand>), typeof(UpdateItemSubTeamHierarchyClassTraitDecorator));
            container.RegisterDecorator(typeof(ICommandHandler<UpdateHierarchyClassTraitCommand>), typeof(AddItemMessageQueueHierarchyClassTraitDecorator));
            container.RegisterDecorator(typeof(ICommandHandler<AddHierarchyClassCommand>), typeof(AddHierarchyClassMammothEventDecorator));
            container.RegisterDecorator(typeof(ICommandHandler<DeleteHierarchyClassCommand>), typeof(DeleteHierarchyClassMammothEventDecorator));
            container.RegisterDecorator(typeof(ICommandHandler<DeleteHierarchyClassCommand>), typeof(DeleteHierarchyClassIconEventDecorator));
            container.RegisterDecorator(typeof(ICommandHandler<UpdateHierarchyClassCommand>), typeof(UpdateHierarchyClassMammothEventDecorator));
            container.RegisterDecorator(typeof(ICommandHandler<AddBrandCommand>), typeof(AddBrandMammothEventDecorator));
            container.RegisterDecorator(typeof(ICommandHandler<BrandCommand>), typeof(UpdateBrandMammothEventDecorator));
            container.RegisterDecorator(typeof(ICommandHandler<AddItemCommand>), typeof(DapperTransactionCommandHandlerDecorator<AddItemCommand>));
            container.RegisterDecorator(typeof(ICommandHandler<UpdateItemCommand>), typeof(DapperTransactionCommandHandlerDecorator<UpdateItemCommand>));
            container.RegisterDecorator(typeof(IManagerHandler<>), typeof(TransactionManagerHandlerDecorator<BrandManager>));
            container.RegisterDecorator(typeof(IManagerHandler<>), typeof(TransactionManagerHandlerDecorator<ManufacturerManager>));
            container.RegisterDecorator(typeof(IManagerHandler<>), typeof(TransactionManagerHandlerDecorator<AddEwicExclusionManager>));
            container.RegisterDecorator(typeof(IManagerHandler<>), typeof(TransactionManagerHandlerDecorator<RemoveEwicExclusionManager>));
            container.RegisterDecorator(typeof(IManagerHandler<>), typeof(TransactionManagerHandlerDecorator<AddEwicMappingManager>));
            container.RegisterDecorator(typeof(IManagerHandler<>), typeof(TransactionManagerHandlerDecorator<RemoveEwicMappingManager>));
            container.RegisterDecorator(typeof(IManagerHandler<>), typeof(TransactionManagerHandlerDecorator<AddCertificationAgencyManager>));
            container.RegisterDecorator(typeof(IManagerHandler<>), typeof(TransactionManagerHandlerDecorator<UpdateNationalHierarchyManager>));
            container.RegisterDecorator(typeof(IManagerHandler<>), typeof(TransactionManagerHandlerDecorator<AddNationalHierarchyManager>));
            container.RegisterDecorator(typeof(IManagerHandler<>), typeof(TransactionManagerHandlerDecorator<DeleteNationalHierarchyManager>));
            container.RegisterDecorator(typeof(IManagerHandler<>), typeof(TransactionManagerHandlerDecorator<AddAttributeManager>));
            container.RegisterDecorator(typeof(IManagerHandler<>), typeof(TransactionManagerHandlerDecorator<UpdateAttributeManager>));

            container.RegisterDecorator(typeof(ICommandHandler<>), typeof(DbProviderCommandHandlerDecorator<>)); // moving to bottom so that connections are build before transactions
            container.RegisterDecorator(typeof(IQueryHandler<,>), typeof(DbProviderQueryHandlerDecorator<,>));

            //Register AutoMapper
            container.Register(() => mapper, Lifestyle.Singleton);

            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());

            container.Verify();

            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));

            return container;
        }

        private static IconContext GetIconContext()
        {
            IconContext context = new IconContext();

            context.Database.CommandTimeout = AppSettingsAccessor.GetIntSetting("IconContextCommandTimeout", 30);

            return context;
        }
    }
}