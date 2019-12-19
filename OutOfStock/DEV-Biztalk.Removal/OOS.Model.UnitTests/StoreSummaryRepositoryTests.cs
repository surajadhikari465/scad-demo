using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using OOS.Model.Repository;
using StructureMap;

namespace OOS.Model.UnitTests
{
    [TestFixture]
    public class StoreSummaryRepositoryTests
    {
        private DateTime startDate;
        private DateTime endDate;


        [SetUp]
        public void Setup()
        {
            Bootstrapper.Bootstrap();

            DateTime.TryParse("2/13/2012", out startDate);
            DateTime.TryParse("2/19/2012", out endDate);

        }

        [Test]
        [Category("Integration Test")]
        public void TestOOSCountSummaryForStore()
        {
            var repository = CreateObjectUnderTest("kir");
            var summary = repository.OOSCountSummaryFor(startDate, endDate);
            Assert.AreEqual(1, summary.GetStores().Count);
            Assert.AreEqual("Kirby", summary.GetStores()[0]);
        }

        private StoreSummaryRepository CreateObjectUnderTest(string storeAbbrev)
        {
            var store = ObjectFactory.GetInstance<StoreRepository>().ForAbbrev(storeAbbrev);
            var dbFactory = ObjectFactory.GetInstance<IOOSEntitiesFactory>();
            return new StoreSummaryRepository(store, dbFactory);
        }

        
        [Test]
        [Category("Integration Test")]
        public void TestSKUSummaryBy()
        {
            var repository = CreateObjectUnderTest("lmr");
            SKUSummary summary = repository.SKUSummaryBy();

            Assert.AreEqual(2, summary.Count());
            Assert.AreEqual(11547, summary.For("Lamar", "Grocery"));
        }

        [Test]
        [Category("Integration Test")]
        public void TestNumberOfScans()
        {
            var repository = CreateObjectUnderTest("kir");
            ScanSummary scanSummary = repository.NumberOfScansBy(startDate, endDate);
            Assert.AreEqual(604, scanSummary.NumberOfScansFor("Kirby"));

        }

        [Test]
        [Category("Integration Test")]
        public void TestOOSCountSummaryForStoreWithNoReporting()
        {
            var repository = CreateObjectUnderTest("frk");
            var summary = repository.OOSSummaryFor(startDate, endDate);
            Assert.AreEqual(1, summary.GetStores().Count);
        }

    }
}
