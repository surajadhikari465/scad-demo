using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using StructureMap;

namespace OOS.Model.UnitTests
{

    [TestFixture]
    public class RegionRepositoryTests
    {

        [SetUp]
        public void Setup()
        {
            Bootstrapper.Bootstrap();
        }

        [Test]
        public void TestRegionByAbbrev()
        {
            var repository = CreateObjectUnderTest();
            var region = repository.ForAbbrev("SW");
            Assert.AreEqual(region.Abbrev, "SW");
        }

        private IRegionRepository CreateObjectUnderTest()
        {
            return ObjectFactory.GetInstance<IRegionRepository>();
        }


        [Test]
        [Category("Integration Test")]
        public void TestRegionWithStores()
        {
            var repository = CreateObjectUnderTest();
            var region = repository.ForAbbrev("sw");
            Assert.AreEqual(28, region.GetStores().Count);
        }

        [Test]
        [Category("Integration Test")]
        public void TestGetOpenStoreList()
        {
            var regionRepository = CreateObjectUnderTest();
            var region = regionRepository.ForAbbrev("sw");
            var openStores = region.OpenStores();
            Assert.AreEqual(23, openStores.Count);
        }


        [Test]
        [Category("Integration Test")]
        [Ignore]
        public void TestGetClosedStoreList()
        {
            var regionRepository = CreateObjectUnderTest();
            var region = regionRepository.ForAbbrev("sw");
            var closedStoreNames = region.ClosedStores().Select(p => p.Abbrev);
            Assert.IsTrue(closedStoreNames.SequenceEqual(new List<string> { "DLS", "ESP", "PRS", "SIX", "SKL" }));

        }

        [Test]
        public void TestAllStoreCountMinusClosedStoreCountEqualsOpenStoreCountForRegion()
        {
            var regionRepository = CreateObjectUnderTest();
            var region = regionRepository.ForAbbrev("sw");

            var allStores = region.GetStores();
            var closedStores = region.ClosedStores();
            var openStores = region.OpenStores();
            Assert.AreEqual(openStores.Count, allStores.Count - closedStores.Count);
        }

        [Test]
        [Category("Integration Test")]
        public void TestGetAllRegionAbbreviations()
        {
            var repository = CreateObjectUnderTest();
            var regions = repository.RegionAbbreviations().ToList();

            Assert.AreEqual(12, regions.Count);
            Assert.IsTrue(regions.Contains("SW"));
        }

        [Test]
        [Category("Integration Test")]
        public void TestAllRegionNames()
        {
            var repository = CreateObjectUnderTest();
            var regions = repository.RegionNames().ToList();

            Assert.AreEqual(12, regions.Count);
            Assert.IsTrue(regions.Contains("Southwest"));
        }

        [Test]
        [Category("Integration Test")]
        public void TestRegionForBadNameReturnsNull()
        {
            var repository = CreateObjectUnderTest();
            var region = repository.ForName("bad name");
            
            Assert.IsNull(region);
        }


        [Test]
        [Category("Integration Test")]
        public void TestRegionForNameValid()
        {
            var repository = CreateObjectUnderTest();
            var region = repository.ForName("Southwest");
            Assert.IsNotNull(region);
            Assert.AreEqual(23, region.OpenStores().Count);
        }

        [Test]
        [Category("Integration Test")]
        public void TestAverageStoresPerRegion()
        {
            var repository = CreateObjectUnderTest();
            var regions = repository.RegionNames().ToList();
            int sum = regions.Select(repository.ForName).Select(region => region.OpenStores().Count).Aggregate(0, (current, count) => current + count);
            var average = sum/regions.Count;
            Assert.AreEqual(24, average);
        }

    }
}
