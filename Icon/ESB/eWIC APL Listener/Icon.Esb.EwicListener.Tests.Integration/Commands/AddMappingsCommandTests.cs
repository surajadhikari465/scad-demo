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
    public class AddMappingsCommandTests
    {
        private AddMappingsCommand command;
        private IRenewableContext<IconContext> globalContext;
        private IconContext context;
        private DbContextTransaction transaction;
        private List<Mapping> testMappings;
        private string testAgencyId;
        private string testAplScanCode;
        private List<Item> testItems;
        private List<string> testWfmScanCodes;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            globalContext = new GlobalIconContext(context);

            command = new AddMappingsCommand(globalContext);

            testAgencyId = "ZZ";
            testAplScanCode = "222222222";
            testWfmScanCodes = new List<string>
            {
                "222222223",
                "222222224",
                "222222225"
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
            context.Agency.Add(new TestAgencyBuilder().WithAgencyId(testAgencyId));
            context.SaveChanges();
        }

        private void StageTestApl()
        {
            context.AuthorizedProductList.Add(new TestAplBuilder().WithAgencyId(testAgencyId).WithScanCode(testAplScanCode));
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

        [TestMethod]
        public void AddMappings_SuccessfulCommandExecution_MappingsShouldBeAdded()
        {
            // Given.
            StageTestAgency();
            StageTestApl();
            StageTestItems();

            testMappings = new List<Mapping>
            {
                new TestEwicMappingBuilder().WithAgencyId(testAgencyId).WithAplScanCode(testAplScanCode).WithWfmScanCode(testItems[0].ScanCode.Single()),
                new TestEwicMappingBuilder().WithAgencyId(testAgencyId).WithAplScanCode(testAplScanCode).WithWfmScanCode(testItems[1].ScanCode.Single()),
                new TestEwicMappingBuilder().WithAgencyId(testAgencyId).WithAplScanCode(testAplScanCode).WithWfmScanCode(testItems[2].ScanCode.Single())
            };

            var parameters = new AddMappingsParameters { Mappings = testMappings };

            // When.
            command.Execute(parameters);

            // Then.
            var newMappings = context.Mapping.Where(m => m.AgencyId == testAgencyId && testWfmScanCodes.Contains(m.ScanCode.scanCode)).ToList();

            Assert.AreEqual(testMappings.Count, newMappings.Count);
        }
    }
}
