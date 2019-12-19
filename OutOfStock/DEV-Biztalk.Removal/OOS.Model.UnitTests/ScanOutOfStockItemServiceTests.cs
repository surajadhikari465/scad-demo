using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using OOSCommon;
using StructureMap;

namespace OOS.Model.UnitTests
{
    [TestFixture]
    public class ScanOutOfStockItemServiceTests
    {
        
        [SetUp]
        public void Setup()
        {
            Bootstrapper.Bootstrap();
            var userProfile = Bootstrapper.GetUserProfile();
            ObjectFactory.Configure(x => x.For<IUserProfile>().Use(userProfile));
        }



        [Test]
        public void TestCreateScanOutOfStockItemService()
        {
            var service = CreateObjectUnderTest();
            Assert.IsNotNull(service);
        }

        private IScanOutOfStockItemService CreateObjectUnderTest()
        {
            return ObjectFactory.GetInstance<ScanOutOfStockItemService>();
        }


        [Test]
        [Category("Integration Test")]
        public void TestCreateEventForBadUPC()
        {
            var service = CreateObjectUnderTest();
            string upc = "xxx00yyklf";
            var success = service.CreateEventFor("Alamo Quarry", upc, DateTime.Now);
            Assert.IsFalse(success);
        }

        [Test]
        [Category("Integration Test")]
        public void TestNewEventCreatorForGoodUPC()
        {
            var service = CreateObjectUnderTest();
            string upc = "0009492255667";
            var success = service.CreateEventFor("Plantation", upc, DateTime.Now);
            Assert.IsTrue(success);
        }


        [Test]
        [Category("Integration Test")]
        public void TestGetAllRegionAbbreviations()
        {
            var service = CreateObjectUnderTest();

            Assert.AreEqual(12, service.RegionAbbreviations().Count());
        }

        [Test]
        [Category("Integration Test")]
        public void TestGetAllRegionNames()
        {
            var service = CreateObjectUnderTest();

            Assert.AreEqual(12, service.RegionNames().Count());
        }
        
        [Test]
        [Category("Integration Test")]
        public void TestStoreNamesWhenRegionNameIsGood()
        {
            var service = CreateObjectUnderTest();
            var storeNames = service.StoreNamesFor("Southwest");
            Assert.AreEqual(23, storeNames.Count());
            Assert.IsTrue(storeNames.Contains("Lamar"));
        }

        [Test]
        [Category("Integration Test")]
        public void TestStoreNamesWhenRegionNameIsBad()
        {
            var service = CreateObjectUnderTest();
            var storeNames = service.StoreNamesFor("North Pole");
            Assert.AreEqual(0, storeNames.Count());
        }

        [Test]
        [Category("Integration Test")]
        public void ScanProducts()
        {
            var service = CreateObjectUnderTest();
            DateTime scanDate = DateTime.Now;
            string regionName = "Southwest";
            string storeName = "Lamar";
            var upcs = new List<string> { "0005342300066" };
            service.ScanProducts(regionName, storeName, upcs.ToArray(), scanDate);
        }

        [Test]
        [Category("Integration Test")]
        public void ScanProductsByStoreAbbreviation()
        {
            var service = CreateObjectUnderTest();
            DateTime scanDate = DateTime.Now;
            const string regionAbbrev = "sw";
            const string storeAbbrev = "LMR";
            var upcs = new List<string> { "0005342300066" };
            service.ScanProductsByStoreAbbreviation(regionAbbrev, storeAbbrev, upcs.ToArray(), scanDate);
        }

        [Test]
        [ExpectedException(typeof(InvalidScanDateException))]
        [Category("Integration Test")]
        public void TryDateTime2ForScanDate()
        {
            var service = CreateObjectUnderTest();
            string storeAbbrev = "LMR";
            string regionAbbrev = "SW";
            var upcs = new List<string> {"0005342300066"};
            var scanDate = Convert.ToDateTime("01/01/0001");
            service.ScanProductsByStoreAbbreviation(regionAbbrev, storeAbbrev, upcs.ToArray(), scanDate);
        }

        [Test]
        [Category("Integration Test")]
        public void GivenValidScanDateNoExceptionThrown()
        {
            var service = CreateObjectUnderTest();
            string storeAbbrev = "bca";
            string regionAbbrev = "fl";
            var upcs = new List<string> { "0005342300066" };
            var scanDate = Convert.ToDateTime("04/21/2012");
            service.ScanProductsByStoreAbbreviation(regionAbbrev, storeAbbrev, upcs.ToArray(), scanDate);
        }

        [Test]
        [Category("Integration Test")]
        public void GivenUpcsFromFileScanProducts()
        {
            var upcs = File.ReadAllLines("Upc.txt");
            var scanDate = Convert.ToDateTime("2012-08-06T09:31:40-07:00");
            string storeAbbrev = "PET";
            string regionAbbrev = "NC";
            var service = CreateObjectUnderTest();
            service.ScanProductsByStoreAbbreviation(regionAbbrev, storeAbbrev, upcs.ToArray(), scanDate);

        }
    }
}
