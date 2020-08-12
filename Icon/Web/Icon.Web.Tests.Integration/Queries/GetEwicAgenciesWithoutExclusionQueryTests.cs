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
    public class GetEwicAgenciesWithoutExclusionQueryTests
    {
        private GetEwicAgenciesWithoutExclusionQuery query;
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

            query = new GetEwicAgenciesWithoutExclusionQuery(this.context);

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

        private void StageTestItem()
        {
            itemTestHelper.TestScanCode = testExclusion;
            var tmpItem = itemTestHelper.CreateDefaultTestItem();
            itemTestHelper.SaveItem(tmpItem);

            testItem = context.Item.First(i => i.ItemId == tmpItem.ItemId);
        }

        [TestMethod]
        public void GetEwicAgenciesWithoutExclusion_AllAgenciesHaveTheExclusion_NoResultsShouldBeReturned()
        {
            // Given.
            testAgencies = new List<Agency>
            {
                new TestAgencyBuilder().WithAgencyId(testAgenciesId[0]),
                new TestAgencyBuilder().WithAgencyId(testAgenciesId[1])
            };

            context.Agency.AddRange(testAgencies);
            context.SaveChanges();

            StageTestItem();

            var agencies = context.Agency.ToList();

            foreach (var agency in agencies)
            {
                agency.ScanCode.Add(testItem.ScanCode.Single());
            }

            context.SaveChanges();

            var parameters = new GetEwicAgenciesWithoutExclusionParameters
            {
                ScanCode = testExclusion
            };

            // When.
            var results = query.Search(parameters);

            // Then.
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void GetEwicAgenciesWithoutExclusion_NoAgenciesHaveTheExclusion_AgenciesWithoutTheExclusionShouldBeReturned()
        {
            // Given.
            testAgencies = new List<Agency>
            {
                new TestAgencyBuilder().WithAgencyId(testAgenciesId[0]),
                new TestAgencyBuilder().WithAgencyId(testAgenciesId[1])
            };

            context.Agency.AddRange(testAgencies);
            context.SaveChanges();

            var parameters = new GetEwicAgenciesWithoutExclusionParameters
            {
                ScanCode = testExclusion
            };

            // When.
            var results = query.Search(parameters).Where(q => testAgenciesId.Contains(q.AgencyId)).ToList();

            // Then.
            Assert.AreEqual(testAgencies.Count, results.Count);
        }
    }
}
