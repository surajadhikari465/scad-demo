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
    public class GetCurrentEwicMappingQueryTests
    {
        private IconContext context;
        private GetCurrentEwicMappingQuery query;
        private DbContextTransaction transaction;
        private List<Agency> testAgencies;
        private List<AuthorizedProductList> testApl;
        private List<Item> testItems;
        private List<Mapping> testMappings;
        private List<string> testAgenciesId;
        private List<string> testWfmScanCodes;
        private List<string> testAplScanCodes;

        [TestInitialize]
        public void InitializeData()
        {
            context = new IconContext();
            query = new GetCurrentEwicMappingQuery(this.context);

            testAgenciesId = new List<string>
            {
                "ZZ",
                "XX",
                "YY"
            };

            testWfmScanCodes = new List<string>
            {
                "2222222",
                "2222223",
                "2222224"
            };

            testAplScanCodes = new List<string>
            {
                "2222",
                "2223",
                "2224"
            };

            transaction = context.Database.BeginTransaction();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
        }

        [TestMethod]
        public void GetCurrentEwicMapping_NoCurrentMappingExists_EmptyStringShouldBeReturned()
        {
            // Given.
            var parameters = new GetCurrentEwicMappingParameters
            {
                AplScanCode = testAplScanCodes[0],
                WfmScanCode = testWfmScanCodes[0]
            };

            // When.
            string currentMapping = query.Search(parameters);

            // Then.
            Assert.AreEqual(String.Empty, currentMapping);
        }

        [TestMethod]
        public void GetCurrentEwicMapping_MappingExistsForOneAgency_CurrentMappingShouldBeReturned()
        {
            // Given.
            testAgencies = new List<Agency>
            {
                new TestAgencyBuilder().WithAgencyId(testAgenciesId[0])
            };

            context.Agency.AddRange(testAgencies);
            context.SaveChanges();

            testApl = new List<AuthorizedProductList>
            {
                new TestAplBuilder().WithAgencyId(testAgenciesId[0]).WithScanCode(testAplScanCodes[0])
            };

            context.AuthorizedProductList.AddRange(testApl);
            context.SaveChanges();

            testItems = new List<Item>
            {
                new TestItemBuilder().WithScanCode(testWfmScanCodes[0])
            };

            context.Item.AddRange(testItems);
            context.SaveChanges();

            testMappings = new List<Mapping>
            {
                new TestEwicMappingBuilder().WithAgencyId(testAgenciesId[0]).WithAplScanCode(testAplScanCodes[0]).WithWfmScanCode(testItems[0].ScanCode.Single())
            };

            context.Mapping.AddRange(testMappings);
            context.SaveChanges();
            
            var parameters = new GetCurrentEwicMappingParameters
            {
                AplScanCode = testAplScanCodes[0],
                WfmScanCode = testWfmScanCodes[0]
            };

            // When.
            string currentMapping = query.Search(parameters);

            // Then.
            Assert.AreEqual(testAplScanCodes[0], currentMapping);
        }

        [TestMethod]
        public void GetCurrentEwicMapping_MappingExistsForTwoAgencies_CurrentMappingShouldBeReturned()
        {
            // Given.
            testAgencies = new List<Agency>
            {
                new TestAgencyBuilder().WithAgencyId(testAgenciesId[0]),
                new TestAgencyBuilder().WithAgencyId(testAgenciesId[1])
            };

            context.Agency.AddRange(testAgencies);
            context.SaveChanges();

            testApl = new List<AuthorizedProductList>
            {
                new TestAplBuilder().WithAgencyId(testAgenciesId[0]).WithScanCode(testAplScanCodes[0]),
                new TestAplBuilder().WithAgencyId(testAgenciesId[1]).WithScanCode(testAplScanCodes[0])
            };

            context.AuthorizedProductList.AddRange(testApl);
            context.SaveChanges();

            testItems = new List<Item>
            {
                new TestItemBuilder().WithScanCode(testWfmScanCodes[0])
            };

            context.Item.AddRange(testItems);
            context.SaveChanges();

            testMappings = new List<Mapping>
            {
                new TestEwicMappingBuilder().WithAgencyId(testAgenciesId[0]).WithAplScanCode(testAplScanCodes[0]).WithWfmScanCode(testItems[0].ScanCode.Single()),
                new TestEwicMappingBuilder().WithAgencyId(testAgenciesId[1]).WithAplScanCode(testAplScanCodes[0]).WithWfmScanCode(testItems[0].ScanCode.Single())
            };

            context.Mapping.AddRange(testMappings);
            context.SaveChanges();

            var parameters = new GetCurrentEwicMappingParameters
            {
                AplScanCode = testAplScanCodes[0],
                WfmScanCode = testWfmScanCodes[0]
            };

            // When.
            string currentMapping = query.Search(parameters);

            // Then.
            Assert.AreEqual(testAplScanCodes[0], currentMapping);
        }
    }
}