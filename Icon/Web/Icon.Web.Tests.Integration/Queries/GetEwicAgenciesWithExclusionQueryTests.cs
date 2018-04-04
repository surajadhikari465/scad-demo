using Icon.Framework;
using Icon.Testing.Builders;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Queries;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Icon.Web.Tests.Integration.Queries
{
    [TestClass] [Ignore]
    public class GetEwicAgenciesWithExclusionQueryTests
    {
        private GetEwicAgenciesWithExclusionQuery query;
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
            query = new GetEwicAgenciesWithExclusionQuery(this.context);

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

            testItem = new TestItemBuilder().WithScanCode(testExclusion);
            context.Item.Add(testItem);
            context.SaveChanges();

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

            testItem = new TestItemBuilder().WithScanCode(testExclusion);
            context.Item.Add(testItem);
            context.SaveChanges();

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
