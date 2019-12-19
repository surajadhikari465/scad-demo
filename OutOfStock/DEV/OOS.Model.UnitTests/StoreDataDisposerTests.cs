using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using OOS.Model.Repository;
using Rhino.Mocks;
using StructureMap;

namespace OOS.Model.UnitTests
{
    [TestFixture]
    public class StoreDataDisposerTests
    {
        private const string STORE_ABBREV = "LMR";
        private DateTime startDate;
        private DateTime endDate;
        private IStoreRepository storeRepository;
        [SetUp]
        public void Setup()
        {
            DateTime.TryParse("01/31/2012", out startDate);
            DateTime.TryParse("02/07/2012", out endDate);

        }

        [Test]
        public void TestDisposeStoreData()
        {
            var dataDisposer = CreateObjectUnderTest();
            var result = dataDisposer.DeleteStoreData(STORE_ABBREV, startDate, endDate);
            Assert.AreEqual(0, result);
            storeRepository.VerifyAllExpectations();
        }

        private StoreDataDisposer CreateObjectUnderTest()
        {
            storeRepository = MockRepository.GenerateStub<IStoreRepository>();
            storeRepository.Stub(s => s.ForAbbrev(STORE_ABBREV)).Return(new Store(0)).Repeat.Once();
            var dataDisposer = new StoreDataDisposer(MockConfigurator.New(), storeRepository);
            return dataDisposer;
        }

        [Test]
        public void TestDeleteStoreDetail()
        {
            var dataDisposer = CreateObjectUnderTest();
            DateTime startDate;
            DateTime endDate;
            DateTime.TryParse("12/26/2011", out startDate);
            DateTime.TryParse("2/19/2012", out endDate);

            var n = dataDisposer.DeleteStoreDetail(STORE_ABBREV, startDate, endDate);
            Assert.AreEqual(0, n);
        }

        [Test]
        public void TestDeleteAllStoreDetail()
        {
            var dataDisposer = CreateObjectUnderTest();
            var n = dataDisposer.DeleteAllStoreDetail(STORE_ABBREV);
            Assert.AreEqual(0, n);
        }

        [Test]
        public void TestDeleteStoreHeader()
        {
            var dataDisposer = CreateObjectUnderTest();
            DateTime startDate;
            DateTime endDate;
            DateTime.TryParse("12/26/2011", out startDate);
            DateTime.TryParse("2/19/2012", out endDate);


            var n = dataDisposer.DeleteStoreHeader(STORE_ABBREV, startDate, endDate);
            Assert.AreEqual(0, n);
        }

        [Test]
        public void TestDeleteAllStoreHeader()
        {
            var dataDisposer = CreateObjectUnderTest();
            var n = dataDisposer.DeleteAllStoreHeader(STORE_ABBREV);
            Assert.AreEqual(0, n);
        }

        [Test]
        public void TestDeleteStoreData()
        {
            var dataDisposer = CreateObjectUnderTest();
            DateTime startDate;
            DateTime endDate;
            DateTime.TryParse("12/26/2011", out startDate);
            DateTime.TryParse("2/19/2012", out endDate);

            var n = dataDisposer.DeleteStoreData(STORE_ABBREV, startDate, endDate);
            Assert.AreEqual(0, n);
        }

        [Test]
        public void TestDeleteAllStoresInRegion()
        {
            var dataDisposer = CreateObjectUnderTest();
            var n = dataDisposer.DeleteAllStoreData(STORE_ABBREV);
            Assert.AreEqual(0, n);
        }

        [Test]
        [Category("Do Not Run")]
        public void TestDeleteAllStoreData()
        {
            Bootstrapper.Bootstrap();
            ObjectFactory.Configure(x => x.For<IUserProfile>().Use(Bootstrapper.GetUserProfile()));
            
            // I am commenting the code out to insure we will not run the code unless intentionally
            // until TFS Category support is functional

            //var stores = ObjectFactory.GetInstance<IRegionRepository>().ForName("Southwest").OpenStores();
            //var dataDisposer = ObjectFactory.GetInstance<StoreDataDisposer>();
            //stores.ForEach(p => dataDisposer.DeleteAllStoreData(p.Abbrev));
        }
        
    }
}
