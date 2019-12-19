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
    public class SummarySpecificationTests
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
        public void TestSummarySpec()
        {
            var spec = new SummarySpecification(startDate, endDate);
            var regionRepository = CreateRegionSummaryRepository("sw");
            var summary = regionRepository.SelectSummarySatisfying(spec);

            Assert.AreEqual(13033, summary.OOSCountFor("Kirby", "Grocery"));
        }

        private RegionSummaryRepository CreateRegionSummaryRepository(string regionAbbrev)
        {
            var region = ObjectFactory.GetInstance<IRegionRepository>().ForAbbrev(regionAbbrev);
            var dbFactory = ObjectFactory.GetInstance<IOOSEntitiesFactory>();
            var config = ObjectFactory.GetInstance<IConfigurator>();
            return new RegionSummaryRepository(region, dbFactory, config);
        }


        [Test]
        [Category("Integration Test")]
        public void TestSummarySpecForStore()
        {
            var spec = new SummarySpecification(startDate, endDate);
            var repository = CreateStoreSummaryRepository("kir");
            var summary = repository.SelectSummarySatisfying(spec);
            Assert.AreEqual(1, summary.GetStores().Count);
            Assert.AreEqual(13033, summary.OOSCountFor("Kirby", "Grocery"));
        }

        private ISummaryRepository CreateStoreSummaryRepository(string storeAbbrev)
        {
            var store = ObjectFactory.GetInstance<StoreRepository>().ForAbbrev(storeAbbrev);
            var dbFactory = ObjectFactory.GetInstance<IOOSEntitiesFactory>();
            return new StoreSummaryRepository(store, dbFactory);
        }


        [Test]
        [Category("Integration Test")]
        public void TestSummarySpecWhenRegionReportedNoData()
        {
            var spec = new SummarySpecification(startDate, endDate);
            var regionRepository = CreateRegionSummaryRepository("nc");
            var summary = regionRepository.SelectSummarySatisfying(spec);

            Assert.AreEqual(0, summary.OOSCountFor("Franklin", "Grocery"));
        }

        [Test]
        [Category("Integration Test")]
        public void TestSummarySpecWhenStoreReportedNoData()
        {
            var spec = new SummarySpecification(startDate, endDate);
            var repository = CreateStoreSummaryRepository("frk");
            var summary = repository.SelectSummarySatisfying(spec);
            Assert.AreEqual(1, summary.GetStores().Count);
            Assert.AreEqual(0, summary.OOSCountFor("Franklin", "Grocery"));
        }

    }
}
