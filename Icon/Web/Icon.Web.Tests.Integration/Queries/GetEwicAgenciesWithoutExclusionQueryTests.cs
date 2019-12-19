using Icon.Framework;
using Icon.Testing.Builders;
using Icon.Web.DataAccess.Queries;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Icon.Web.Tests.Integration.Queries
{
    [TestClass] [Ignore]
    public class GetEwicAgenciesWithoutExclusionQueryTests
    {
        private GetEwicAgenciesWithoutExclusionQuery query;
        private IconContext context;
        private DbContextTransaction transaction;
        private string testExclusion;
        private List<Agency> testAgencies;
        private List<string> testAgenciesId;
        private Item testItem;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            query = new GetEwicAgenciesWithoutExclusionQuery(this.context);

            testExclusion = "22222222228";

            testAgenciesId = new List<string>
            {
                "ZZ",
                "XX",
                "YY"
            };

            transaction = context.Database.BeginTransaction();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
        }

        private void StageTestItem()
        {
            testItem = new TestItemBuilder().WithScanCode(testExclusion);

            context.Item.Add(testItem);
            context.SaveChanges();
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
