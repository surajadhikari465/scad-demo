using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OOS.Model;
using OOS.Model.Feed;
using OOS.Model.Repository;
using StructureMap;

namespace ConsumeStoreFeed
{
    public class StructuremapBootstrapper
    {
        public static void Bootstrap()
        {
            ObjectFactory.Initialize(x =>
                                         {
                                             x.For<IConfigurator>().Use<Configurator>();
                                             x.For<ILogService>().Singleton().Use<LogService>();
                                             x.For<IStoreFeedConsumer>().Use<StoreFeedConsumer>().Ctor<string>("serviceUrl").Is("http://www.wholefoodsmarket.com/common/irest");
                                             x.For<IStoreRepository>().Use<StoreRepository>();
                                             x.For<IOOSEntitiesFactory>().Use<OOSEntitiesFactory>();
                                             x.For<IUserProfile>().Use<UserProfile>();
                                             x.For<IStoreFactory>().Use<StoreFactory>();
                                             x.For<IRegionRepository>().Use<RegionRepository>();
                                         });
        }
    }
}
