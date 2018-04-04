using Icon.Framework;
using Icon.Testing.Builders;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Icon.Web.Tests.Integration.Commands
{
    [TestClass] [Ignore]
    public class AddEwicExclusionCommandHandlerTests
    {
        private AddEwicExclusionCommandHandler commandHandler;
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
            commandHandler = new AddEwicExclusionCommandHandler(this.context);

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
        public void AddEwicExclusion_ForOneAgency_ExclusionShouldBeAddedForTheAgency()
        {
            // Given.
            testAgencies = new List<Agency>
            {
                new TestAgencyBuilder().WithAgencyId(testAgenciesId[0])
            };

            context.Agency.AddRange(testAgencies);
            context.SaveChanges();

            StageTestItem();

            var command = new AddEwicExclusionCommand
            {
                ScanCode = testExclusion,
                Agencies = testAgencies
            };

            // When.
            commandHandler.Execute(command);

            // Then.
            string testAgencyId = testAgenciesId[0];
            string newExclusion = context.Agency.Single(a => a.AgencyId == testAgencyId).ScanCode.Single().scanCode;

            Assert.AreEqual(testExclusion, newExclusion);
        }

        [TestMethod]
        public void AddEwicExclusion_ForTwoAgencies_ExclusionShouldBeAddedForBothAgencies()
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

            var command = new AddEwicExclusionCommand
            {
                ScanCode = testExclusion,
                Agencies = testAgencies
            };

            // When.
            commandHandler.Execute(command);

            // Then.
            string firstTestAgencyId = testAgenciesId[0];
            string secondTestAgencyId = testAgenciesId[1];

            string newExclusionForFirstAgency = context.Agency.Single(a => a.AgencyId == firstTestAgencyId).ScanCode.Single().scanCode;
            string newExclusionForSecondAgency = context.Agency.Single(a => a.AgencyId == secondTestAgencyId).ScanCode.Single().scanCode;

            Assert.AreEqual(testExclusion, newExclusionForFirstAgency);
            Assert.AreEqual(testExclusion, newExclusionForSecondAgency);
        }
    }
}
