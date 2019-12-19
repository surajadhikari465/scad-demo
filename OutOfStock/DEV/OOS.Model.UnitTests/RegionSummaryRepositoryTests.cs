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
    public class RegionSummaryRepositoryTests
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
        public void TestRegionOOSCountSummary()
        {
            var repository = CreateObjectUnderTest("sw");
            OOSCountSummary summary = repository.OOSCountSummaryFor(startDate, endDate);

            Assert.AreEqual(32, summary.Count());
        }

        private RegionSummaryRepository CreateObjectUnderTest(string regionAbbrev)
        {
            var region = ObjectFactory.GetInstance<IRegionRepository>().ForAbbrev(regionAbbrev);
            var dbFactory = ObjectFactory.GetInstance<IOOSEntitiesFactory>();
            var config = ObjectFactory.GetInstance<IConfigurator>();
            return new RegionSummaryRepository(region, dbFactory, config);
        }


        [Test]
        [Category("Integration Test")]
        public void TestNumberOfSKUs()
        {
            var repository = CreateObjectUnderTest("sw");
            SKUSummary summary = repository.SKUSummaryBy();

            Assert.AreEqual(46, summary.Count());
        }

        [Test]
        public void TestOOSSummary()
        {
            var repository = CreateObjectUnderTest("sw");
            OOSSummary summary = repository.OOSSummaryFor(startDate, endDate);
            Assert.AreEqual(23, summary.GetStores().Count);
        }

        [Test]
        [Category("Integration Test")]
        public void TestNumberOfScans()
        {
            var repository = CreateObjectUnderTest("sw");
            ScanSummary scanSummary = repository.NumberOfScansBy(startDate, endDate);
            Assert.AreEqual(41, scanSummary.NumberOfScansFor("Kirby"));
        }

        [Test]
        [Category("Integration Test")]
        public void TestOOSSummaryForRegionNotReporting()
        {
            var repository = CreateObjectUnderTest("nc");
            OOSSummary summary = repository.OOSSummaryFor(startDate, endDate);
            Assert.AreEqual(35, summary.GetStores().Count);
        }


    }
}
