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
    public class GetEwicAgenciesWithoutMappingQueryTests
    {
        private GetEwicAgenciesWithoutMappingQuery query;
        private IconContext context;
        private string testAplScanCode;
        private string testWfmScanCode;
        private List<Agency> testAgencies;
        private List<string> testAgenciesId;
        private List<AuthorizedProductList> testApl;
        private Item testItem;
        private List<Mapping> testMappings;
        private ItemTestHelper itemTestHelper;
        private TransactionScope transactionScope;

        [TestInitialize]
        public void Initialize()
        {
            transactionScope = new TransactionScope();
            context = new IconContext();
            itemTestHelper = new ItemTestHelper();
            itemTestHelper.Initialize(context.Database.Connection, false, false);

            query = new GetEwicAgenciesWithoutMappingQuery(this.context);

            testAplScanCode = "22222222228";
            testWfmScanCode = "22222222229";

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
        public void GetEwicAgenciesWithoutMapping_AllAgenciesHaveTheMapping_NoResultsShouldBeReturned()
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

            itemTestHelper.TestScanCode = testWfmScanCode;
            var tmpItem = itemTestHelper.CreateDefaultTestItem();
            itemTestHelper.SaveItem(tmpItem);

            testItem = context.Item.First(i => i.ItemId == tmpItem.ItemId);

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

            var parameters = new GetEwicAgenciesWithoutMappingParameters
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
        public void GetEwicAgenciesWithoutMapping_NoAgenciesHaveTheExclusion_AgenciesWithoutTheExclusionShouldBeReturned()
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

            testApl = new List<AuthorizedProductList>
            {
                new TestAplBuilder().WithAgencyId(testAgenciesId[0]).WithScanCode(testAplScanCode),
                new TestAplBuilder().WithAgencyId(testAgenciesId[1]).WithScanCode(testAplScanCode),
                new TestAplBuilder().WithAgencyId(testAgenciesId[2]).WithScanCode(testAplScanCode)
            };

            context.AuthorizedProductList.AddRange(testApl);
            context.SaveChanges();

            var parameters = new GetEwicAgenciesWithoutMappingParameters
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
