using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OOS.Model;
using OOS.Model.Repository;
using SharedKernel;
using StructureMap;

namespace OOSCollectorService
{
    public static class Bootstrapper
    {
        public static void Bootstrap()
        {
            ObjectFactory.Initialize(x =>
            {
                x.For<IConfigurator>().Use<Configurator>();
                x.For<ILogService>().Singleton().Use<LogService>();
                x.For<IRetailItemRepository>().Use<RetailItemRepository>();
                x.For<IStoreRepository>().Use<StoreRepository>();
                x.For<IProductRepository>().Use<ProductRepository>();
                x.For<IOOSEntitiesFactory>().Use<OOSEntitiesFactory>();
                x.For<IUserProfile>().Use<UserProfile>();
                x.For<IProductFactory>().Use<ProductFactory>();
            });
        }

    }
}
