using Icon.RenewableContext;
using Icon.Esb.EwicAplListener.DataAccess.Queries;
using Icon.Framework;
using Icon.Framework.RenewableContext;
using Icon.Testing.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Icon.Esb.EwicAplListener.Tests.Integration.Queries
{
    [TestClass]
    public class GetExistingMappingsQueryTests
    {
        private GetExistingMappingsQuery query;
        private IRenewableContext<IconContext> globalContext;
        private IconContext context;
        private DbContextTransaction transaction;
        private List<string> testAgenciesId;
        private List<Agency> testAgencies;
        private List<AuthorizedProductList> testApl;
        private List<Item> testItems;
        private string testAplScanCode;
        private List<string> testWfmScanCodes;
        private List<Mapping> testMappings;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            globalContext = new GlobalIconContext(context);

            query = new GetExistingMappingsQuery(globalContext);

            testAgenciesId = new List<string>
            {
                "ZZ",
                "XX"
            };
            
            testWfmScanCodes = new List<string>
            {
                "22222222",
                "22222223",
                "22222224"
            };

            testAplScanCode = "2222222229";

            transaction = context.Database.BeginTransaction();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
        }

        private void StageTestAgency()
        {
            context.Agency.Add(new TestAgencyBuilder().WithAgencyId(testAgenciesId[0]));
            context.SaveChanges();
        }

        private void StageTestAgencies()
        {
            testAgencies = new List<Agency>
            {
                new TestAgencyBuilder().WithAgencyId(testAgenciesId[0]),
                new TestAgencyBuilder().WithAgencyId(testAgenciesId[1])
            };

            context.Agency.AddRange(testAgencies);
            context.SaveChanges();
        }

        private void StageTestAplForOneAgency()
        {
            context.AuthorizedProductList.Add(new TestAplBuilder().WithAgencyId(testAgenciesId[0]).WithScanCode(testAplScanCode));
            context.SaveChanges();
        }

        private void StageTestAplForMultipleAgencies()
        {
            testApl = new List<AuthorizedProductList>
            {
                new TestAplBuilder().WithAgencyId(testAgenciesId[0]).WithScanCode(testAplScanCode),
                new TestAplBuilder().WithAgencyId(testAgenciesId[1]).WithScanCode(testAplScanCode)
            };

            context.AuthorizedProductList.AddRange(testApl);
            context.SaveChanges();
        }

        private void StageTestItems()
        {
            testItems = new List<Item>
            {
                new TestItemBuilder().WithScanCode(testWfmScanCodes[0]),
                new TestItemBuilder().WithScanCode(testWfmScanCodes[1]),
                new TestItemBuilder().WithScanCode(testWfmScanCodes[2])
            };

            context.Item.AddRange(testItems);
            context.SaveChanges();
        }

        private void StageTestMappingsForOneAgency()
        {
            testMappings = new List<Mapping>
            {
                new TestEwicMappingBuilder().WithAgencyId(testAgenciesId[0]).WithAplScanCode(testAplScanCode).WithWfmScanCode(testItems[0].ScanCode.Single()),
                new TestEwicMappingBuilder().WithAgencyId(testAgenciesId[0]).WithAplScanCode(testAplScanCode).WithWfmScanCode(testItems[1].ScanCode.Single()),
                new TestEwicMappingBuilder().WithAgencyId(testAgenciesId[0]).WithAplScanCode(testAplScanCode).WithWfmScanCode(testItems[2].ScanCode.Single())
            };

            context.Mapping.AddRange(testMappings);
            context.SaveChanges();
        }

        private void StageTestMappingsForTwoAgencies()
        {
            testMappings = new List<Mapping>
            {
                new TestEwicMappingBuilder().WithAgencyId(testAgenciesId[0]).WithAplScanCode(testAplScanCode).WithWfmScanCode(testItems[0].ScanCode.Single()),
                new TestEwicMappingBuilder().WithAgencyId(testAgenciesId[0]).WithAplScanCode(testAplScanCode).WithWfmScanCode(testItems[1].ScanCode.Single()),
                new TestEwicMappingBuilder().WithAgencyId(testAgenciesId[0]).WithAplScanCode(testAplScanCode).WithWfmScanCode(testItems[2].ScanCode.Single()),
                new TestEwicMappingBuilder().WithAgencyId(testAgenciesId[1]).WithAplScanCode(testAplScanCode).WithWfmScanCode(testItems[0].ScanCode.Single()),
                new TestEwicMappingBuilder().WithAgencyId(testAgenciesId[1]).WithAplScanCode(testAplScanCode).WithWfmScanCode(testItems[1].ScanCode.Single()),
            };

            context.Mapping.AddRange(testMappings);
            context.SaveChanges();
        }

        [TestMethod]
        public void GetExistingMappings_NoMappingsExist_NoResultsShouldBeReturned()
        {
            // Given.
            var parameters = new GetExistingMappingsParameters
            {
                AplScanCode = testAplScanCode
            };

            // When.
            var results = query.Search(parameters);
            
            // Then.
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void GetExistingMappings_MappingsExistForOneAgency_AllMappingsShouldBeReturned()
        {
            // Given.
            StageTestAgency();
            StageTestAplForOneAgency();
            StageTestItems();
            StageTestMappingsForOneAgency();

            var parameters = new GetExistingMappingsParameters
            {
                AplScanCode = testAplScanCode
            };

            // When.
            var results = query.Search(parameters);

            // Then.
            Assert.AreEqual(3, results.Count);
        }

        [TestMethod]
        public void GetExistingMappings_MappingsExistForTwoAgencies_AllMappingsShouldBeReturnedWithNoDuplicates()
        {
            // Given.
            StageTestAgencies();
            StageTestAplForMultipleAgencies();
            StageTestItems();
            StageTestMappingsForTwoAgencies();

            var parameters = new GetExistingMappingsParameters
            {
                AplScanCode = testAplScanCode
            };

            // When.
            var results = query.Search(parameters);

            // Then.
            Assert.AreEqual(3, results.Count);
        }
    }
}
