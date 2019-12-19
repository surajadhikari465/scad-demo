using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OOS.Model.Repository;
using OOSCommon;
using Rhino.Mocks;
using SharedKernel;
using StructureMap;

namespace OOS.Model.UnitTests
{
    public class Bootstrapper
    {
        public static void Bootstrap()
        {
            ObjectFactory.Initialize(x =>
            {
                x.For<IConfigurator>().Use(MockConfigurator.New());
                x.For<ILogService>().Singleton().Use<LogService>();
                x.For<IOOSEntitiesFactory>().Use<OOSEntitiesFactory>();

                //var productRepository = MockRepository.GenerateStub<IProductRepository>();
                //productRepository.Stub(p => p.For(Arg<string>.Is.Anything)).Return(null);
                //x.For<IProductRepository>().Use(productRepository);

                x.For<IProductRepository>().Use<ProductRepository>();
                x.For<IOOSEntitiesFactory>().Use<OOSEntitiesFactory>();
                x.For<IEventCreatorFactory>().Use<EventCreatorFactory>();
                x.For<IRegionRepository>().Use<RegionRepository>();
                x.For<IStoreRepository>().Use<StoreRepository>();
                x.For<IProductFactory>().Use<ProductFactory>();
                x.For<ISummaryRepositoryFactory>().Use<SummaryRepositoryFactory>();
                x.For<IRetailItemRepository>().Use<RetailItemRepository>();
                x.For<IOffShelfUploadRepository>().Use<OffShelfUploadRepository>();
                x.For<ISkuCountRepository>().Use<SkuCountRepository>();
            });


        }

        public static IUserProfile GetUserProfile()
        {
            var userProfile = MockRepository.GenerateStub<IUserProfile>();
            userProfile.Stub(p => p.IsCentral()).Return(true).Repeat.Once();
            userProfile.Stub(p => p.IsStoreLevel()).Return(true).Repeat.Once();
            return userProfile;
        }

    }
}
