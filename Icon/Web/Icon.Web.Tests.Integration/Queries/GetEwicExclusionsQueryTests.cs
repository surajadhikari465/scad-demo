using Icon.Framework;
using Icon.Testing.Builders;
using Icon.Web.DataAccess.Queries;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Icon.Web.Tests.Integration.Queries
{
    [TestClass] [Ignore]
    public class GetEwicExclusionsQueryTests
    {
        private IconContext context;
        private GetEwicExclusionsQuery query;
        private DbContextTransaction transaction;
        private List<Agency> testAgencies;
        private List<Item> testItems;
        private List<string> testAgenciesId;
        private List<string> testScanCodes;

        [TestInitialize]
        public void InitializeData()
        {
            context = new IconContext();
            query = new GetEwicExclusionsQuery(this.context);

            testAgenciesId = new List<string>
            {
                "ZZ",
                "XX"
            };

            testScanCodes = new List<string>
            {
                "2222222",
                "2222223",
                "2222224"
            };

            transaction = context.Database.BeginTransaction();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
        }

        [TestMethod]
        public void GetEwicExclusionsQuery_ThreeExclusionsForOneAgency_ThreeExclusionsShouldBeReturned()
        {
            // Given.
            testAgencies = new List<Agency>
            {
                new TestAgencyBuilder().WithAgencyId(testAgenciesId[0])
            };

            testItems = new List<Item>
            {
                new TestItemBuilder().WithScanCode(testScanCodes[0]),
                new TestItemBuilder().WithScanCode(testScanCodes[1]),
                new TestItemBuilder().WithScanCode(testScanCodes[2])
            };

            testAgencies[0].ScanCode = testItems.Select(i => i.ScanCode.Single()).ToList();

            context.Item.AddRange(testItems);
            context.Agency.Add(testAgencies[0]);
            context.SaveChanges();

            // When.
            var exclusions = query.Search(new GetEwicExclusionsParameters());

            // Then.
            exclusions = exclusions.Where(e => testScanCodes.Contains(e.ScanCode)).ToList();
            bool productDescriptionValueIsPresent = exclusions.TrueForAll(e => !String.IsNullOrEmpty(e.ProductDescription));

            Assert.AreEqual(testItems.Count, exclusions.Count);
            Assert.IsTrue(productDescriptionValueIsPresent);
        }

        [TestMethod]
        public void GetEwicExclusionsQuery_SameThreeExclusionsForTwoDifferentAgencies_ThreeExclusionsShouldBeReturned()
        {
            // Given.
            testAgencies = new List<Agency>
            {
                new TestAgencyBuilder().WithAgencyId(testAgenciesId[0]),
                new TestAgencyBuilder().WithAgencyId(testAgenciesId[1])
            };

            testItems = new List<Item>
            {
                new TestItemBuilder().WithScanCode(testScanCodes[0]),
                new TestItemBuilder().WithScanCode(testScanCodes[1]),
                new TestItemBuilder().WithScanCode(testScanCodes[2])
            };

            testAgencies[0].ScanCode = testItems.Select(i => i.ScanCode.Single()).ToList();
            testAgencies[1].ScanCode = testItems.Select(i => i.ScanCode.Single()).ToList();

            context.Item.AddRange(testItems);
            context.Agency.AddRange(testAgencies);
            context.SaveChanges();

            // When.
            var exclusions = query.Search(new GetEwicExclusionsParameters());

            // Then.
            exclusions = exclusions.Where(e => testScanCodes.Contains(e.ScanCode)).ToList();
            bool productDescriptionValueIsPresent = exclusions.TrueForAll(e => !String.IsNullOrEmpty(e.ProductDescription));

            Assert.AreEqual(testItems.Count, exclusions.Count);
            Assert.IsTrue(productDescriptionValueIsPresent);
        }

        [TestMethod]
        public void GetEwicExclusionsQuery_DifferentExclusionForTwoDifferentAgencies_TwoExclusionsShouldBeReturned()
        {
            // Given.
            testAgencies = new List<Agency>
            {
                new TestAgencyBuilder().WithAgencyId(testAgenciesId[0]),
                new TestAgencyBuilder().WithAgencyId(testAgenciesId[1])
            };

            testItems = new List<Item>
            {
                new TestItemBuilder().WithScanCode(testScanCodes[0]),
                new TestItemBuilder().WithScanCode(testScanCodes[1])
            };

            testAgencies[0].ScanCode = testItems[0].ScanCode;
            testAgencies[1].ScanCode = testItems[1].ScanCode;

            context.Item.AddRange(testItems);
            context.Agency.AddRange(testAgencies);
            context.SaveChanges();

            // When.
            var exclusions = query.Search(new GetEwicExclusionsParameters());

            // Then.
            exclusions = exclusions.Where(e => testScanCodes.Contains(e.ScanCode)).ToList();
            bool productDescriptionValueIsPresent = exclusions.TrueForAll(e => !String.IsNullOrEmpty(e.ProductDescription));

            Assert.AreEqual(testItems.Count, exclusions.Count);
            Assert.IsTrue(productDescriptionValueIsPresent);
        }

        [TestMethod]
        public void GetEwicExclusionsQuery_OneAgencyHasOneExclusionAndOneHasNoExclusions_OneExclusionShouldBeReturned()
        {
            // Given.
            testAgencies = new List<Agency>
            {
                new TestAgencyBuilder().WithAgencyId(testAgenciesId[0]),
                new TestAgencyBuilder().WithAgencyId(testAgenciesId[1])
            };

            testItems = new List<Item>
            {
                new TestItemBuilder().WithScanCode(testScanCodes[0])
            };

            testAgencies[0].ScanCode = testItems[0].ScanCode;

            context.Item.AddRange(testItems);
            context.Agency.AddRange(testAgencies);
            context.SaveChanges();

            // When.
            var exclusions = query.Search(new GetEwicExclusionsParameters());

            // Then.
            exclusions = exclusions.Where(e => testScanCodes.Contains(e.ScanCode)).ToList();
            bool productDescriptionValueIsPresent = exclusions.TrueForAll(e => !String.IsNullOrEmpty(e.ProductDescription));

            Assert.AreEqual(testItems.Count, exclusions.Count);
            Assert.IsTrue(productDescriptionValueIsPresent);
        }
    }
}
