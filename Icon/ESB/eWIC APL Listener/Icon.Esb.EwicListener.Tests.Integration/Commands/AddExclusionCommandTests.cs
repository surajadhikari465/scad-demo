using Icon.RenewableContext;
using Icon.Esb.EwicAplListener.DataAccess.Commands;
using Icon.Framework;
using Icon.Framework.RenewableContext;
using Icon.Testing.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Icon.Esb.EwicAplListener.Tests.Integration.Commands
{
    [TestClass]
    public class AddExclusionCommandTests
    {
        private AddExclusionCommand command;
        private IRenewableContext<IconContext> globalContext;
        private IconContext context;
        private DbContextTransaction transaction;
        private string testAgencyId;
        private Item testItem;
        private string testExcludedScanCode;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            globalContext = new GlobalIconContext(context);

            command = new AddExclusionCommand(globalContext);

            testAgencyId = "ZZ";
            testExcludedScanCode = "222222222";

            transaction = context.Database.BeginTransaction();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
        }

        private void StageTestAgency()
        {
            context.Agency.Add(new TestAgencyBuilder().WithAgencyId(testAgencyId));
            context.SaveChanges();
        }

        private void StageTestItem()
        {
            testItem = new TestItemBuilder().WithScanCode(testExcludedScanCode);

            context.Item.Add(testItem);
            context.SaveChanges();
        }

        [TestMethod]
        public void AddExclusion_ValidExclusion_ExclusionShouldBeAdded()
        {
            // Given.
            StageTestAgency();
            StageTestItem();

            var parameters = new AddExclusionParameters { AgencyId = testAgencyId, ExclusdedScanCode = testExcludedScanCode };

            // When.
            command.Execute(parameters);

            // Then.
            string newExclusion = context.Agency.Single(a => a.AgencyId == testAgencyId).ScanCode.Single().scanCode;

            Assert.AreEqual(testExcludedScanCode, newExclusion);
        }
    }
}
