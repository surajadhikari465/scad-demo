using Icon.Framework;
using Icon.Testing.Builders;
using Icon.Web.DataAccess.Commands;
using Icon.Web.Tests.Integration.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Transactions;

namespace Icon.Web.Tests.Integration.Commands
{
    [TestClass] 
    public class RemoveEwicExclusionCommandHandlerTests
    {
        private RemoveEwicExclusionCommandHandler commandHandler;
        private IconContext context;
        private List<string> testExclusions;
        private List<Agency> testAgencies;
        private List<string> testAgenciesId;
        private List<Item> testItems;
        private ItemTestHelper itemTestHelper;
        private TransactionScope transactionScope;

        [TestInitialize]
        public void Initialize()
        {
            transactionScope = new TransactionScope();
            context = new IconContext();
            itemTestHelper = new ItemTestHelper();
            itemTestHelper.Initialize(context.Database.Connection, false, false);
            commandHandler = new RemoveEwicExclusionCommandHandler(this.context);

            testExclusions = new List<string>
            {
                "22222222228",
                "22222222229"
            };

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
            transactionScope.Dispose();
        }

        private void StageTestItems()
        {
            itemTestHelper.TestScanCode = testExclusions[0];
            var testItem1 = itemTestHelper.CreateDefaultTestItem();
            itemTestHelper.SaveItem(testItem1);

            itemTestHelper.TestScanCode = testExclusions[1];
            var testItem2 = itemTestHelper.CreateDefaultTestItem();
            itemTestHelper.SaveItem(testItem2);
            testItems = new List<Item>
            {
                context.Item.First(i => i.ItemId == testItem1.ItemId),
                context.Item.First(i => i.ItemId == testItem2.ItemId),
            };
        }

        [TestMethod]
        public void RemoveEwicExclusion_ForOneAgency_ExclusionShouldBeRemovedFromTheAgency()
        {
            // Given.
            testAgencies = new List<Agency>
            {
                new TestAgencyBuilder().WithAgencyId(testAgenciesId[0])
            };

            context.Agency.AddRange(testAgencies);
            context.SaveChanges();

            StageTestItems();

            testAgencies[0].ScanCode = testItems[0].ScanCode;
            context.SaveChanges();

            var command = new RemoveEwicExclusionCommand
            {
                ScanCode = testExclusions[0]
            };

            // When.
            commandHandler.Execute(command);

            // Then.
            string testAgencyId = testAgenciesId[0];

            var exclusion = context.Agency.Single(a => a.AgencyId == testAgencyId).ScanCode.SingleOrDefault();

            Assert.IsNull(exclusion);
        }

        [TestMethod]
        public void RemoveEwicExclusion_ForThreeAgencies_ExclusionShouldBeRemovedFromThreeAgencies()
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

            StageTestItems();

            testAgencies[0].ScanCode = testItems[0].ScanCode;
            testAgencies[1].ScanCode = testItems[0].ScanCode;
            testAgencies[2].ScanCode = testItems[0].ScanCode;
            context.SaveChanges();

            var command = new RemoveEwicExclusionCommand
            {
                ScanCode = testExclusions[0]
            };

            // When.
            commandHandler.Execute(command);

            // Then.
            string firstTestAgencyId = testAgenciesId[0];
            string secondTestAgencyId = testAgenciesId[1];
            string thirdTestAgencyId = testAgenciesId[2];

            var exclusionForFirstAgency = context.Agency.Single(a => a.AgencyId == firstTestAgencyId).ScanCode.SingleOrDefault();
            var exclusionForSecondAgency = context.Agency.Single(a => a.AgencyId == secondTestAgencyId).ScanCode.SingleOrDefault();
            var exclusionForThirdAgency = context.Agency.Single(a => a.AgencyId == thirdTestAgencyId).ScanCode.SingleOrDefault();

            Assert.IsNull(exclusionForFirstAgency);
            Assert.IsNull(exclusionForSecondAgency);
            Assert.IsNull(exclusionForThirdAgency);
        }

        [TestMethod]
        public void RemoveEwicExclusion_AgencyHasMultipleExclusions_OnlyOneExclusionShouldBeRemoved()
        {
            // Given.
            testAgencies = new List<Agency>
            {
                new TestAgencyBuilder().WithAgencyId(testAgenciesId[0])
            };

            context.Agency.AddRange(testAgencies);
            context.SaveChanges();

            StageTestItems();

            testAgencies[0].ScanCode.Add(testItems[0].ScanCode.Single());
            testAgencies[0].ScanCode.Add(testItems[1].ScanCode.Single());
            context.SaveChanges();

            var command = new RemoveEwicExclusionCommand
            {
                ScanCode = testExclusions[0]
            };

            // When.
            commandHandler.Execute(command);

            // Then.
            string testAgencyId = testAgenciesId[0];

            var removedExclusion = context.Agency.Single(a => a.AgencyId == testAgencyId).ScanCode.Where(sc => sc.scanCode == testExclusions[0]).SingleOrDefault();
            string remainingExclusion = context.Agency.Single(a => a.AgencyId == testAgencyId).ScanCode.Single().scanCode;

            Assert.IsNull(removedExclusion);
            Assert.AreEqual(testExclusions[1], remainingExclusion);
        }
    }
}
