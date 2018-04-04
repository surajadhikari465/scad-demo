using Icon.Framework;
using Icon.Testing.Builders;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Icon.Web.Tests.Integration.Commands
{
    [TestClass] [Ignore]
    public class RemoveEwicExclusionCommandHandlerTests
    {
        private RemoveEwicExclusionCommandHandler commandHandler;
        private IconContext context;
        private DbContextTransaction transaction;
        private List<string> testExclusions;
        private List<Agency> testAgencies;
        private List<string> testAgenciesId;
        private List<Item> testItems;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
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

            transaction = context.Database.BeginTransaction();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
        }

        private void StageTestItems()
        {
            testItems = new List<Item>
            {
                new TestItemBuilder().WithScanCode(testExclusions[0]),
                new TestItemBuilder().WithScanCode(testExclusions[1])
            };

            context.Item.AddRange(testItems);
            context.SaveChanges();
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

            testItems = new List<Item>
            {
                new TestItemBuilder().WithScanCode(testExclusions[0])
            };

            context.Item.AddRange(testItems);
            context.SaveChanges();

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

            testItems = new List<Item>
            {
                new TestItemBuilder().WithScanCode(testExclusions[0])
            };

            context.Item.AddRange(testItems);
            context.SaveChanges();

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

            testItems = new List<Item>
            {
                new TestItemBuilder().WithScanCode(testExclusions[0]),
                new TestItemBuilder().WithScanCode(testExclusions[1])
            };

            context.Item.AddRange(testItems);
            context.SaveChanges();

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
