using System.Web;
//using MassTransit;
//using MassTransit.NLogIntegration;
using OOS.Model;
using OOS.Model.Feed;
using OOS.Model.Repository;
using OOSCommon;
using OutOfStock.ScanManagement;
using SharedKernel;
using StructureMap;

namespace OutOfStock
{
    public static class ContainerBootstrapper
    {
        public static void Bootstrap()
        {
            ObjectFactory.Configure(cfg =>
            {
                cfg.For<IConfigure>().Use<BasicConfigurator>();
                cfg.For<IConfigurator>().Use<Configurator>();
                cfg.For<ILogService>().Singleton().Use<LogService>();
                cfg.For<IProductRepository>().Use<ProductRepository>();
                cfg.For<IOOSEntitiesFactory>().Use<OOSEntitiesFactory>();
                cfg.For<IEventCreatorFactory>().Use<EventCreatorFactory>();
                cfg.For<IRegionRepository>().Use<RegionRepository>();
                cfg.For<IStoreRepository>().Use<StoreRepository>();
                cfg.For<IUserProfile>().Use<UserProfile>();
                cfg.For<IProductFactory>().Use<ProductFactory>();
                cfg.For<ISummaryRepositoryFactory>().Use<SummaryRepositoryFactory>();
                cfg.For<IStoreFactory>().Use<StoreFactory>();
                cfg.For<IRetailItemRepository>().Use<RetailItemRepository>();
                cfg.For<IApplicationConfig>().Use<ApplcationConfig>();
                cfg.For<IOutOfStockNotificationManager>().Use<OutOfStockNotificationManager>();
                cfg.For<IUserLoginManager>().Use<UserLoginManager>();
                cfg.For<IScanOutOfStockItemService>().Use<ScanOutOfStockItemService>();
                cfg.For<IRawScanRepository>().Use<RawScanRepository>();
            });
        }
    }
}