using System;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OOS.Model;
using OOS.Model.Repository;
using OOS.Services.DAL;
using OOS.Services.DataModels;
using OOS.Services.Tests.Fakes;
using OOSCommon;
using OutOfStock;
using OutOfStock.ScanManagement;
using OutOfStock.WebService;
using ProductDataBoundedContext;
using SharedKernel;
using StructureMap;

namespace OOS.Services.Tests
{
    [TestClass]
    public class UpdateStoresTests
    {

       
        [TestMethod]
        public void StoreUpdater_ClosedStore()
        {
            var vim = new FakeVim();
            var repo = new FakeOosRepo();

            var model = new StoreUpdater(repo, vim);

            var closed = model.VimStores.FirstOrDefault(x => x.STORE_ABBR.Equals("DDD"));
            closed.STATUS = "CLOSED";

            model.Compare();

            Assert.AreEqual("DDD", model.UpdateList[0].STORE_ABBREVIATION);
        }


        [TestMethod]
        public void StoreUpdater_UpdatedName()
        {
            var vim = new FakeVim();
            var repo = new FakeOosRepo();

            var model = new StoreUpdater(repo, vim);

            var UPDATED  = model.VimStores.FirstOrDefault(x => x.STORE_ABBR.Equals("CCC"));
            UPDATED.STORE_NAME = "CANTANKEROUSCOOT";

            model.Compare();

            Assert.AreEqual("CCC", model.UpdateList[0].STORE_ABBREVIATION);
        }

        [TestMethod]
        public void StoreUpdater_NewSageStore()
        {
            var vim = new FakeVim();
            var repo = new FakeOosRepo();

            var model = new StoreUpdater(repo, vim);
            model.VimStores.Add(new VimStore() { STORE_ABBR = "EEE", REGION = "EE", STATUS = "new", STORE_NAME = "extraEvent" });

            model.Compare();

            Assert.AreEqual("EEE", model.InsertList[0].STORE_ABBREVIATION);

            var newStatus =
                model.StoreStatuses.FirstOrDefault(
                    x => x.STATUS.Equals("new", StringComparison.CurrentCultureIgnoreCase));
            Assert.AreEqual(newStatus.ID, model.InsertList[0].STATUS_ID);

        }

        [TestMethod]
        public void StoreUpdater_UpdatedStatusFromNewToSoon()
        {
            var vim = new FakeVim();
            var repo = new FakeOosRepo();

            var model = new StoreUpdater(repo, vim);
            model.VimStores.Add(new VimStore() { STORE_ABBR = "EEE", REGION = "DD", STATUS = "new", STORE_NAME = "extraEvent" });

            var newStatus =
                model.StoreStatuses.FirstOrDefault(
                    x => x.STATUS.Equals("new", StringComparison.CurrentCultureIgnoreCase));

            var newish = model.Existing.FirstOrDefault(x => x.STORE_ABBREVIATION == "CCC");
            newish.STATUS_ID = newStatus.ID;

            var soonish = model.VimStores.FirstOrDefault(x => x.STORE_ABBR == "CCC");
            soonish.STATUS = "soon";
            
            model.Compare();



            Assert.AreEqual("CCC", model.UpdateList[0].STORE_ABBREVIATION);

            var soonStatus =
                model.StoreStatuses.FirstOrDefault(
                    x => x.STATUS.Equals("soon", StringComparison.CurrentCultureIgnoreCase));
            Assert.AreEqual(soonStatus.ID, model.UpdateList[0].STATUS_ID);

        }

        [TestMethod]
        public void StoreUpdater_CantFindRegion_ShouldntGetToDatabase()
        {
            var vim = new FakeVim();
            var repo = new FakeOosRepo();

            var model = new StoreUpdater(repo, vim);
            model.VimStores.Add(new VimStore() { STORE_ABBR = "EEE", REGION = "FF", STATUS = "new", STORE_NAME = "extraEvent" });
            model.Compare();
            Assert.IsFalse(model.InsertList.Any(x => x.STORE_ABBREVIATION == "EEE"));
            ;
        }

        [TestMethod]
        public void StoreUpdater_CantFindStatus_ShouldntGetToDatabase()
        {
            var vim = new FakeVim();
            var repo = new FakeOosRepo();

            var model = new StoreUpdater(repo, vim);
            model.VimStores.Add(new VimStore() { STORE_ABBR = "EEE", REGION = "FF", STATUS = "Hanging", STORE_NAME = "extraEvent" });
            model.Compare();
            Assert.IsFalse(model.InsertList.Any(x => x.STORE_ABBREVIATION == "EEE"));
            
        }
    }

    [TestClass]
    public class ServiceTester
    {
        private  static IOosBackend _service;
        private static string[] _upcs;
        private static string[] _upcs2;
        private static string[] _upcsAll;

        [ClassInitialize()]
        public static void ClassInit(TestContext context)
        {
            //ObjectFactory.Configure(cfg =>
            //{
            //    cfg.For<IConfigure>().Use<BasicConfigurator>();
            //    cfg.For<IApplicationConfig>().Use<ApplcationConfig>();
            //    cfg.For<IRawScanRepository>().Use<RawScanRepository>();
            //    cfg.For<IScanOutOfStockItemService>().Use<ScanOutOfStockItemService>();


            //});

            ObjectFactory.Configure(cfg =>
            {
                cfg.For<IRawScanRepository>().Use<RawScanRepository>();
                cfg.For<IConfigure>().Use<BasicConfigurator>();
                cfg.For<IScanOutOfStockItemService>().Use<ScanOutOfStockItemService>();
                cfg.For<ILogService>().Use<LogService>();
                cfg.For<IConfigurator>().Use<ScanProcessorConfigurator>();
                cfg.For<IEventCreatorFactory>().Use<EventCreatorFactory>();
                cfg.For<IOOSEntitiesFactory>().Use<OOSEntitiesFactory>();
                cfg.For<IRegionRepository>().Use<RegionRepository>();
                cfg.For<IProductRepository>().Use<ProductRepository>();
                cfg.For<ProductDataService>().Use<ProductDataService>();
                cfg.For<IStoreRepository>().Use<StoreRepository>();
                cfg.For<IRetailItemRepository>().Use<RetailItemRepository>();
                cfg.For<IUserProfile>().Use<ScanProcessorUserProfile>();
                cfg.For<IProductFactory>().Use<ProductFactory>();
                
            });
            _service = new OosBackend();
            _upcs = Enumerable.Repeat("0009948228578", 100).ToArray();
            _upcs2 = Enumerable.Repeat("0009948228577", 100).ToArray();
            _upcsAll = _upcs.Concat(_upcs2).ToArray();
        }


       

        [TestMethod]

        public void TestRawSave()

        {
            
            _service.ScanProductsByStoreAbbreviation(DateTime.Now, "SW", "LMR", _upcsAll, string.Empty, string.Empty, string.Empty);

        }

    }

    public class ScanProcessorConfigurator : BasicConfigurator, IConfigurator
    {
        public string GetSessionID()
        {
            //var sessionId = " ".PadBoth(24);
            //if (HttpContext.Current != null && HttpContext.Current.Session != null &&
            //    !string.IsNullOrWhiteSpace(HttpContext.Current.Session.SessionID))
            //    sessionId = HttpContext.Current.Session.SessionID;
            //return sessionId;
            return "[sessionid]";
        }

        public string TemporaryDownloadFilePath()
        {
            return "c:\\";
        }
    }

    public class ScanProcessorUserProfile : IUserProfile
    {
        public bool IsRegionBuyer()
        {
            throw new NotImplementedException();
        }

        public string UserStoreAbbreviation()
        {
            throw new NotImplementedException();
        }

        public bool IsCentral()
        {
            throw new NotImplementedException();
        }

        public string UserRegion()
        {
            throw new NotImplementedException();
        }

        public bool IsStoreLevel()
        {
            throw new NotImplementedException();
        }

        public string UserName { get; }
    }
}
