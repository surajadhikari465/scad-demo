[assembly: WebActivator.PostApplicationStartMethod(typeof(Icon.Web.Mvc.App_Start.SimpleInjectorInitializer), "Initialize")]

namespace Icon.Web.Mvc.App_Start
{
    using Excel.ExcelValidationRuleBuilders.Factories;
    using Excel.ModelMappers;
    using Excel.Models;
    using Excel.Services;
    using Excel.Validators.Factories;
    using Excel.WorksheetBuilders.Factories;
    using Icon.Common;
    using Icon.Common.DataAccess;
    using Icon.Esb.Factory;
    using Icon.Ewic.Serialization.Serializers;
    using Icon.Ewic.Transmission.Producers;
    using Icon.Framework;
    using Icon.Logging;
    using Icon.Web.Common.Cache;
    using Icon.Web.Common.Validators;
    using Icon.Web.DataAccess.Commands;
    using Icon.Web.DataAccess.Decorators;
    using Icon.Web.DataAccess.Infrastructure;
    using Icon.Web.DataAccess.Managers;
    using Icon.Web.DataAccess.Models;
    using Icon.Web.DataAccess.Queries;
    using Icon.Web.Mvc.Exporters;
    using Icon.Web.Mvc.Importers;
    using Icon.Web.Mvc.Models;
    using Icon.Web.Mvc.RegionalItemCatalogs;
    using Icon.Web.Mvc.Validators;
    using Icon.Web.Mvc.Validators.Managers;
    using SimpleInjector;
    using SimpleInjector.Extensions;
    using SimpleInjector.Integration.Web.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Reflection;
    using System.Web.Mvc;
    using Utility;
    public static class SimpleInjectorInitializer
    {
        /// <summary>Initialize the container and register it as MVC3 Dependency Resolver.</summary>
        public static void Initialize()
        {
            // Did you know the container can diagnose your configuration? 
            // Go to: https://simpleinjector.org/diagnostics
            var container = new Container();
            
            InitializeContainer(container);

            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());
            
            container.Verify();
            
            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
        }
     
        public static void InitializeContainer(Container container)
        {
            container.Register<IconContext>(() => GetIconContext());

            container.RegisterManyForOpenGeneric(typeof(ICommandHandler<>), typeof(AddAddressCommand).Assembly);
            container.RegisterManyForOpenGeneric(typeof(IQueryHandler<,>), typeof(GetAffinitySubBricksParameters).Assembly);
            container.RegisterManyForOpenGeneric(typeof(IManagerHandler<>), typeof(IManagerHandler<>).Assembly);
            container.RegisterManyForOpenGeneric(typeof(ISpreadsheetImporter<>), typeof(ISpreadsheetImporter<>).Assembly);
            container.RegisterManyForOpenGeneric(typeof(ISerializer<>), typeof(ISerializer<>).Assembly);
            container.RegisterSingle<ILogger>(() => new NLogLoggerSingleton(typeof(NLogLoggerSingleton)));
            container.Register<IPluSpreadsheetImporter, PluSpreadsheetImporter>(Lifestyle.Transient);
            container.Register<IExcelExporterService, ExcelExporterService>(Lifestyle.Transient);
            container.Register<IGenericQuery, GenericGetDbSet>(Lifestyle.Transient);
            container.Register<IRegionalItemCatalogFactory, RegionalItemCatalogFactory>();
            container.Register<ICache, Cache>(Lifestyle.Singleton);
            container.Register<IObjectValidator<ItemViewModel>, ItemViewModelValidator>(Lifestyle.Transient);
            container.Register<IObjectValidator<AddItemManager>, AddItemManagerValidator>(Lifestyle.Transient);
            container.Register<IObjectValidator<UpdateItemManager>, UpdateItemManagerValidator>(Lifestyle.Transient);
            container.Register<IObjectValidator<AddPluRequestManager>, AddPluRequestManagerValidator>(Lifestyle.Transient);
            container.Register<IObjectValidator<UpdatePluRequestManager>, UpdatePluRequestManagerValidator>(Lifestyle.Transient);
            container.Register<IObjectValidator<AddEwicExclusionManager>, AddEwicExclusionManagerValidator>(Lifestyle.Transient);
            container.Register<IObjectValidator<AddEwicMappingManager>, AddEwicMappingManagerValidator>(Lifestyle.Transient);
            container.Register<IMessageProducer, EwicMessageProducer>(Lifestyle.Singleton);
            container.Register<IEsbConnectionFactory, EsbConnectionFactory>(Lifestyle.Singleton);
            container.Register<IInfragisticsHelper, InfragisticsHelper>(Lifestyle.Singleton);
            container.RegisterOpenGeneric(typeof(ICachePolicy<>), typeof(CachePolicy<>), Lifestyle.Singleton);
            
            container.RegisterManyForOpenGeneric(typeof(IExcelModelMapper<,>), typeof(IExcelModelMapper<,>).Assembly);
            container.RegisterManyForOpenGeneric(typeof(IExcelValidatorFactory<>), typeof(IExcelValidatorFactory<>).Assembly);
            container.RegisterManyForOpenGeneric(typeof(IWorksheetBuilderFactory<>), typeof(IWorksheetBuilderFactory<>).Assembly);
            container.RegisterManyForOpenGeneric(typeof(IExcelValidationRuleBuilderFactory<>), typeof(IExcelValidationRuleBuilderFactory<>).Assembly);
            container.Register<IExcelService<ItemExcelModel>, ItemExcelService>();

            container.RegisterDecorator(typeof(ICommandHandler<BulkImportCommand<BulkImportNewItemModel>>), typeof(AddProductMammothEventDecorator));
            container.RegisterDecorator(typeof(ICommandHandler<BulkImportCommand<BulkImportItemModel>>), typeof(UpdateProductMammothEventDecorator));
            container.RegisterDecorator(typeof(ICommandHandler<UpdateHierarchyClassTraitCommand>), typeof(UpdateSubTeamMammothEventDecorator));
            container.RegisterDecorator(typeof(ICommandHandler<AddHierarchyClassCommand>), typeof(AddHierarchyClassMammothEventDecorator));
            container.RegisterDecorator(typeof(ICommandHandler<DeleteHierarchyClassCommand>), typeof(DeleteHierarchyClassMammothEventDecorator));
            container.RegisterDecorator(typeof(ICommandHandler<DeleteHierarchyClassCommand>), typeof(DeleteHierarchyClassIconEventDecorator));
            container.RegisterDecorator(typeof(ICommandHandler<UpdateHierarchyClassCommand>), typeof(UpdateHierarchyClassMammothEventDecorator));
            container.RegisterDecorator(typeof(ICommandHandler<AddBrandCommand>), typeof(AddBrandMammothEventDecorator));
            container.RegisterDecorator(typeof(ICommandHandler<UpdateBrandCommand>), typeof(UpdateBrandMammothEventDecorator));
            container.RegisterDecorator(typeof(ICommandHandler<BulkImportCommand<BulkImportBrandModel>>), typeof(BulkImportBrandMammothEventDecorator));

            container.RegisterDecorator(typeof(IQueryHandler<,>), typeof(CachingQueryHandlerDecorator<GetUomParameters, List<UOM>>));
            container.RegisterDecorator(typeof(IQueryHandler<,>), typeof(CachingQueryHandlerDecorator<GetHierarchyLineageParameters, HierarchyClassListModel>));
            container.RegisterDecorator(typeof(IQueryHandler<,>), typeof(CachingQueryHandlerDecorator<GetBrandsParameters, List<BrandModel>>));
            container.RegisterDecorator(typeof(IQueryHandler<,>), typeof(CachingQueryHandlerDecorator<GetCertificationAgenciesParameters, List<CertificationAgencyModel>>));

            container.RegisterDecorator(typeof(IManagerHandler<>), typeof(TransactionManagerHandlerDecorator<AddBrandManager>));
            container.RegisterDecorator(typeof(IManagerHandler<>), typeof(TransactionManagerHandlerDecorator<UpdateBrandManager>));
            container.RegisterDecorator(typeof(IManagerHandler<>), typeof(TransactionManagerHandlerDecorator<AddEwicExclusionManager>));
            container.RegisterDecorator(typeof(IManagerHandler<>), typeof(TransactionManagerHandlerDecorator<RemoveEwicExclusionManager>));
            container.RegisterDecorator(typeof(IManagerHandler<>), typeof(TransactionManagerHandlerDecorator<AddEwicMappingManager>));
            container.RegisterDecorator(typeof(IManagerHandler<>), typeof(TransactionManagerHandlerDecorator<RemoveEwicMappingManager>));
            container.RegisterDecorator(typeof(IManagerHandler<>), typeof(TransactionManagerHandlerDecorator<UpdateMerchTaxAssociationManager>));
            container.RegisterDecorator(typeof(IManagerHandler<>), typeof(TransactionManagerHandlerDecorator<AddMerchTaxAssociationManager>));
            container.RegisterDecorator(typeof(IManagerHandler<>), typeof(TransactionManagerHandlerDecorator<AddCertificationAgencyManager>));
            container.RegisterDecorator(typeof(IManagerHandler<>), typeof(TransactionManagerHandlerDecorator<UpdateCertificationAgencyManager>));

            container.RegisterDecorator(typeof(ICommandHandler<>), typeof(TransactionCommandHandlerDecorator<BulkImportCommand<BulkImportBrandModel>>));
            container.RegisterDecorator(typeof(ICommandHandler<>), typeof(TransactionCommandHandlerDecorator<BulkImportCommand<BulkImportItemModel>>));
            container.RegisterDecorator(typeof(ICommandHandler<>), typeof(TransactionCommandHandlerDecorator<BulkImportCommand<BulkImportNewItemModel>>));
            container.RegisterDecorator(typeof(ICommandHandler<>), typeof(TransactionCommandHandlerDecorator<AddHierarchyClassCommand>));
            container.RegisterDecorator(typeof(ICommandHandler<>), typeof(TransactionCommandHandlerDecorator<UpdateHierarchyClassCommand>));
            container.RegisterDecorator(typeof(ICommandHandler<>), typeof(TransactionCommandHandlerDecorator<DeleteHierarchyClassCommand>));

            // Set Caching policies for Caching Decorator
            var uomPolicy = SetupCachePolicy<GetUomParameters>(appSetting: "UomQueryCacheExpirationInMinutes", defaultExpiry: 720);
            container.RegisterSingle<ICachePolicy<GetUomParameters>>(uomPolicy);

            var hierarchyClassPolicy = SetupCachePolicy<GetHierarchyLineageParameters>(appSetting: "HierarchyClassQueryCacheExpirationInMinutes", defaultExpiry: 15);
            container.RegisterSingle<ICachePolicy<GetHierarchyLineageParameters>>(hierarchyClassPolicy);

            var brandPolicy = SetupCachePolicy<GetBrandsParameters>(appSetting: "GetBrandsQueryCacheExpirationInMinutes", defaultExpiry: 5);
            container.RegisterSingle<ICachePolicy<GetBrandsParameters>>(brandPolicy);   

            var certificationAgencyPolicy = SetupCachePolicy<GetCertificationAgenciesParameters>(appSetting: "CertificationAgencyQueryCacheExpirationInMinutes", defaultExpiry: 5);
            container.RegisterSingle<ICachePolicy<GetCertificationAgenciesParameters>>(certificationAgencyPolicy); 
        }

        private static IconContext GetIconContext()
        {
            IconContext context = new IconContext();

            context.Database.CommandTimeout = AppSettingsAccessor.GetIntSetting("IconContextCommandTimeout", 30);

            return context;
        }

        private static CachePolicy<T> SetupCachePolicy<T>(string appSetting, double defaultExpiry)
        {
            double timeExpiryInMinutes;
            if (!Double.TryParse(ConfigurationManager.AppSettings[appSetting], out timeExpiryInMinutes))
            {
                timeExpiryInMinutes = defaultExpiry;
            }
            var cachePolicy = new CachePolicy<T> { AbsoluteExpiration = DateTime.Now.AddMinutes(timeExpiryInMinutes) };
            return cachePolicy;
        }
    }
}