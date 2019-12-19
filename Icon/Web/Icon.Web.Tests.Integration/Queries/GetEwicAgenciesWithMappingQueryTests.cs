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
    public class GetEwicAgenciesWithMappingQueryTests
    {
        private GetEwicAgenciesWithMappingQuery query;
        private IconContext context;
        private DbContextTransaction transaction;
        private string testAplScanCode;
        private string testWfmScanCode;
        private List<Agency> testAgencies;
        private List<AuthorizedProductList> testApl;
        private List<Mapping> testMappings;
        private List<string> testAgenciesId;
        private Item testItem;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            query = new GetEwicAgenciesWithMappingQuery(this.context);

            testAplScanCode = "22222222228";
            testWfmScanCode = "22222222229";

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
        public void GetEwicAgenciesWithMapping_NoAgenciesHaveTheMapping_NoResultsShouldBeReturned()
        {
            // Given.
            var parameters = new GetEwicAgenciesWithMappingParameters
            {
                AplScanCode = testAplScanCode,
                WfmScanCode = testWfmScanCode
            };

            // When.
            var results = query.Search(parameters);

            // Then.
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void GetEwicAgenciesWithMapping_OneAgencyHasTheMapping_OneAgencyShouldBeReturned()
        {
            // Given.
            testAgencies = new List<Agency>
            {
                new TestAgencyBuilder().WithAgencyId(testAgenciesId[0])
            };

            context.Agency.AddRange(testAgencies);
            context.SaveChanges();

            testItem = new TestItemBuilder().WithScanCode(testWfmScanCode);
            context.Item.Add(testItem);
            context.SaveChanges();

            testApl = new List<AuthorizedProductList>
            {
                new TestAplBuilder().WithAgencyId(testAgenciesId[0]).WithScanCode(testAplScanCode)
            };

            context.AuthorizedProductList.AddRange(testApl);
            context.SaveChanges();

            testMappings = new List<Mapping>
            {
                new TestEwicMappingBuilder().WithAgencyId(testAgenciesId[0]).WithAplScanCode(testAplScanCode).WithWfmScanCode(testItem.ScanCode.Single())
            };

            context.Mapping.AddRange(testMappings);
            context.SaveChanges();

            var parameters = new GetEwicAgenciesWithMappingParameters
            {
                AplScanCode = testAplScanCode,
                WfmScanCode = testWfmScanCode
            };

            // When.
            var results = query.Search(parameters);

            // Then.
            Assert.AreEqual(testAgencies.Count, results.Count);
        }

        [TestMethod]
        public void GetEwicAgenciesWithMapping_ThreeAgenciesHaveTheMapping_ThreeAgenciesShouldBeReturned()
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

            testItem = new TestItemBuilder().WithScanCode(testWfmScanCode);
            context.Item.Add(testItem);
            context.SaveChanges();

            testApl = new List<AuthorizedProductList>
            {
                new TestAplBuilder().WithAgencyId(testAgenciesId[0]).WithScanCode(testAplScanCode),
                new TestAplBuilder().WithAgencyId(testAgenciesId[1]).WithScanCode(testAplScanCode),
                new TestAplBuilder().WithAgencyId(testAgenciesId[2]).WithScanCode(testAplScanCode)
            };

            context.AuthorizedProductList.AddRange(testApl);
            context.SaveChanges();

            testMappings = new List<Mapping>
            {
                new TestEwicMappingBuilder().WithAgencyId(testAgenciesId[0]).WithAplScanCode(testAplScanCode).WithWfmScanCode(testItem.ScanCode.Single()),
                new TestEwicMappingBuilder().WithAgencyId(testAgenciesId[1]).WithAplScanCode(testAplScanCode).WithWfmScanCode(testItem.ScanCode.Single()),
                new TestEwicMappingBuilder().WithAgencyId(testAgenciesId[2]).WithAplScanCode(testAplScanCode).WithWfmScanCode(testItem.ScanCode.Single())
            };

            context.Mapping.AddRange(testMappings);
            context.SaveChanges();

            var parameters = new GetEwicAgenciesWithMappingParameters
            {
                AplScanCode = testAplScanCode,
                WfmScanCode = testWfmScanCode
            };

            // When.
            var results = query.Search(parameters);

            // Then.
            Assert.AreEqual(testAgencies.Count, results.Count);
        }
    }
}
