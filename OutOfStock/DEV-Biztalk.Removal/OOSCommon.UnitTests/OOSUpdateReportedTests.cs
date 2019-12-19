using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using OOS.Model;
using OOS.Model.Repository;
using OOSCommon.DataContext;
using OOSCommon.Import;
using OOSCommon.Movement;
using OOSCommon.VIM;
using OutOfStock;
using ProductDataBoundedContext;
using Rhino.Mocks;
using SharedKernel;
using StructureMap;

namespace OOSCommon.UnitTests
{
    [TestFixture]
    public class OOSUpdateReportedTests
    {
        private const string RASPBERRY_STRUDEL = "0009948244088";
        private const string OOS_EF_CONNECTION_STRING
            = "metadata=res://*/DataContext.OOS.csdl|res://*/DataContext.OOS.ssdl|res://*/DataContext.OOS.msl;provider=System.Data.SqlClient;provider connection string=\"data source=OOSDbTest;initial catalog=OOS;integrated security=True;multipleactiveresultsets=True;App=EntityFramework\"";

        [Test]
        [Category("Integration Test")]
        public void TestRemoveReportHeaderWhenStellaExceptionHappensInValidationMode()
        {
            var result = RemoveReportHeaderWhenStellaExceptionHappens(true);

            Assert.IsFalse(result);
        }

        private bool RemoveReportHeaderWhenStellaExceptionHappens(bool validationMode)
        {
            var scanDate = DateTime.Now;
            var store = new STORE { ID = 373, STORE_NAME = "Palo Alto", STORE_ABBREVIATION = "PAL", PS_BU = "10005" };
            var upcs = new List<string> { RASPBERRY_STRUDEL };

            var logger = MockRepository.GenerateStub<IOOSLog>();
            var vimRepository = MockRepository.GenerateStub<IVIMRepository>();
            vimRepository.Stub(p => p.GetVIMOOSItemDataView(upcs, store.PS_BU)).Return(new Dictionary<string, List<VIMOOSItemDataView>>()).Repeat.Once();

            var oosEFConnectionString = OOS_EF_CONNECTION_STRING;
            var movementRepository = MockRepository.GenerateStub<IMovementRepository>();
            movementRepository.Stub(p => p.GetMovementUnits(upcs, store.PS_BU, scanDate)).Throw(new Exception("Stella Exception")).Repeat.Times(5);

            var importer = new OOSUpdateReported(store, validationMode, logger, vimRepository, oosEFConnectionString, movementRepository);
            importer.BeginBatch(scanDate);
            var result = importer.WriteUPCs(upcs);
            return result;
        }

        [Test]
        [Category("Integration Test")]
        public void TestRemoveReportHeaderWhenStellaExceptionHappensInFalseValidationMode()
        {
            var result = RemoveReportHeaderWhenStellaExceptionHappens(false);

            Assert.IsFalse(result);
        }

        [Test]
        public void CreateProductDataService()
        {
            Bootstrap();
            var service = ObjectFactory.GetInstance<ProductDataService>();
            Assert.IsNotNull(service);

            
        }

        private static void Bootstrap()
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
