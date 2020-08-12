using Icon.Framework;
using Icon.Testing.Builders;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Tests.Integration.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Transactions;

namespace Icon.Web.Tests.Integration.Queries
{
    [TestClass]
    public class GetEwicExclusionsQueryTests
    {
        private IconContext context;
        private GetEwicExclusionsQuery query;
        private List<Agency> testAgencies;
        private List<Item> testItems;
        private List<string> testAgenciesId;
        private List<string> testScanCodes;
        private ItemTestHelper itemTestHelper;
        private TransactionScope transactionScope;

        [TestInitialize]
        [Ignore("39840 - These unit tests need rewrite.")]
        public void InitializeData()
        {
            transactionScope = new TransactionScope();
            context = new IconContext();
            itemTestHelper = new ItemTestHelper();
            itemTestHelper.Initialize(context.Database.Connection, false, false);

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
        }

        [TestCleanup]
        public void Cleanup()
        {
            context.Dispose();
            transactionScope.Dispose();
        }

        [TestMethod]
        [Ignore("42363 - Tech Debt - Unit test need further investigation")]
        public void GetEwicExclusionsQuery_ThreeExclusionsForOneAgency_ThreeExclusionsShouldBeReturned()
        {
            // Given.
            testAgencies = new List<Agency>
            {
                new TestAgencyBuilder().WithAgencyId(testAgenciesId[0])
            };

            testItems = new List<Item>
            {
                BuildItemWithScanCode(testScanCodes[0]),
                BuildItemWithScanCode(testScanCodes[1]),
                BuildItemWithScanCode(testScanCodes[2])
            };

            testAgencies[0].ScanCode = testItems.Select(i => i.ScanCode.Single()).ToList();

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
        [Ignore("42363 - Tech Debt - Unit test need further investigation")]
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
                BuildItemWithScanCode(testScanCodes[0]),
                BuildItemWithScanCode(testScanCodes[1]),
                BuildItemWithScanCode(testScanCodes[2])
            };

            testAgencies[0].ScanCode = testItems.Select(i => i.ScanCode.Single()).ToList();
            testAgencies[1].ScanCode = testItems.Select(i => i.ScanCode.Single()).ToList();

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
        [Ignore("42363 - Tech Debt - Unit test need further investigation")]
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
                BuildItemWithScanCode(testScanCodes[0]),
                BuildItemWithScanCode(testScanCodes[1])
            };

            testAgencies[0].ScanCode = testItems[0].ScanCode;
            testAgencies[1].ScanCode = testItems[1].ScanCode;

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
        [Ignore("42363 - Tech Debt -Unit test need further investigation")]
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

        private Item BuildItemWithScanCode(string scanCode)
        {
            itemTestHelper.TestScanCode = scanCode;
            var tmpItem = itemTestHelper.CreateDefaultTestItem();
            itemTestHelper.SaveItem(tmpItem);

            return context.Item.First(i => i.ItemId == tmpItem.ItemId);
        }
    }
}
