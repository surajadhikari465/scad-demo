using Icon.Framework;
using Icon.Testing.Builders;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Tests.Integration.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Transactions;

namespace Icon.Web.Tests.Integration.Queries
{
    [TestClass]
    public class GetEwicAgenciesWithExclusionQueryTests
    {
        private GetEwicAgenciesWithExclusionQuery query;
        private IconContext context;
        private string testExclusion;
        private List<Agency> testAgencies;
        private List<string> testAgenciesId;
        private Item testItem;
        private ItemTestHelper itemTestHelper;
        private TransactionScope transactionScope;

        [TestInitialize]
        public void Initialize()
        {
            transactionScope = new TransactionScope();
            context = new IconContext();
            itemTestHelper = new ItemTestHelper();
            itemTestHelper.Initialize(context.Database.Connection, false, false);

            query = new GetEwicAgenciesWithExclusionQuery(this.context);

            testExclusion = "22222222228";

            testAgenciesId = new List<string>
            {
                "ZZ",
                "XX",
                "YY"
            };
        }

        [TestCleanup]
        public void Cleanup()
        {
            context.Dispose();
            transactionScope.Dispose();
        }

        [TestMethod]
        public void GetEwicAgenciesWithExclusion_NoAgenciesHaveTheExclusion_NoResultsShouldBeReturned()
        {
            // Given.
            var parameters = new GetEwicAgenciesWithExclusionParameters
            {
                ScanCode = testExclusion
            };

            // When.
            var results = query.Search(parameters);

            // Then.
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void GetEwicAgenciesWithExclusion_OneAgencyHasTheExclusion_OneAgencyShouldBeReturned()
        {
            // Given.
            testAgencies = new List<Agency>
            {
                new TestAgencyBuilder().WithAgencyId(testAgenciesId[0])
            };

            context.Agency.AddRange(testAgencies);
            context.SaveChanges();

            itemTestHelper.TestScanCode = testExclusion;
            var tmpItem = itemTestHelper.CreateDefaultTestItem();
            itemTestHelper.SaveItem(tmpItem);

            testItem = context.Item.First(i => i.ItemId == tmpItem.ItemId);

            testAgencies[0].ScanCode.Add(testItem.ScanCode.Single());
            context.SaveChanges();

            var parameters = new GetEwicAgenciesWithExclusionParameters
            {
                ScanCode = testExclusion
            };

            // When.
            var results = query.Search(parameters);

            // Then.
            Assert.AreEqual(testAgencies.Count, results.Count);
        }

        [TestMethod]
        public void GetEwicAgenciesWithExclusion_ThreeAgenciesHaveTheExclusion_ThreeAgenciesShouldBeReturned()
        {
            // Given.
            testAgencies = new List<Agency>
            {
                new TestAgencyBuilder().WithAgencyId(testAgenciesId[0]),
                new TestAgencyBuilder().WithAgencyId(testAgenciesId[1]),
                new TestAgencyBuilder().WithAgencyId(testAgenciesId[2])
            };

            context.Agency.AddRange(testAgencies);
            context.SaveChanges();
            itemTestHelper.TestScanCode = testExclusion;
            var tmpItem = itemTestHelper.CreateDefaultTestItem();
            itemTestHelper.SaveItem(tmpItem);

            testItem = context.Item.First(i => i.ItemId == tmpItem.ItemId);

            testAgencies[0].ScanCode.Add(testItem.ScanCode.Single());
            testAgencies[1].ScanCode.Add(testItem.ScanCode.Single());
            testAgencies[2].ScanCode.Add(testItem.ScanCode.Single());
            context.SaveChanges();

            var parameters = new GetEwicAgenciesWithExclusionParameters
            {
                ScanCode = testExclusion
            };

            // When.
            var results = query.Search(parameters);

            // Then.
            Assert.AreEqual(testAgencies.Count, results.Count);
        }
    }
}
