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
    public class GetExclusionQueryTests
    {
        private GetExclusionQuery query;
        private IRenewableContext<IconContext> globalContext;
        private IconContext context;
        private DbContextTransaction transaction;
        private string testExclusion;
        private List<Item> testItems;
        private List<string> testWfmScanCodes;
        private List<string> testAgenciesId;
        private List<Agency> testAgencies;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            globalContext = new GlobalIconContext(context);

            query = new GetExclusionQuery(globalContext);

            testExclusion = "22222222";

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

        private void StageTestExclusionsForOneAgency()
        {
            string testAgencyId = testAgenciesId[0];
            var agency = context.Agency.Single(a => a.AgencyId == testAgencyId);

            foreach (var item in testItems)
            {
                agency.ScanCode.Add(item.ScanCode.Single());
            }

            context.SaveChanges();
        }

        private void StageTestExclusionsForTwoAgencies()
        {
            string firstTestAgencyId = testAgenciesId[0];
            var firstAgency = context.Agency.Single(a => a.AgencyId == firstTestAgencyId);

            string secondTestAgencyId = testAgenciesId[1];
            var secondAgency = context.Agency.Single(a => a.AgencyId == secondTestAgencyId);

            foreach (var item in testItems)
            {
                firstAgency.ScanCode.Add(item.ScanCode.Single());
                secondAgency.ScanCode.Add(item.ScanCode.Single());
            }

            context.SaveChanges();
        }

        [TestMethod]
        public void ExclusionExists_NoExclusions_EmptyModelShouldBeReturned()
        {
            // Given.
            var parameters = new GetExclusionParameters { ExcludedScanCode = testExclusion };

            // When.
            var result = query.Search(parameters);

            // Then.
            Assert.IsNull(result.ScanCode);
        }

        [TestMethod]
        public void ExclusionExists_ExclusionExistsForOneAgency_ExclusionShouldBeReturned()
        {
            // Given.
            StageTestAgency();
            StageTestItems();
            StageTestExclusionsForOneAgency();

            var parameters = new GetExclusionParameters { ExcludedScanCode = testExclusion };

            // When.
            var result = query.Search(parameters);

            // Then.
            Assert.AreEqual(testExclusion, result.ScanCode);
        }

        [TestMethod]
        public void ExclusionExists_ExclusionExistsForTwoAgencies_ExclusionShouldBeReturned()
        {
            // Given.
            StageTestAgencies();
            StageTestItems();
            StageTestExclusionsForTwoAgencies();

            var parameters = new GetExclusionParameters { ExcludedScanCode = testExclusion };

            // When.
            var result = query.Search(parameters);

            // Then.
            Assert.AreEqual(testExclusion, result.ScanCode);
        }
    }
}
