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
    public class RemoveEwicMappingCommandHandlerTests
    {
        private RemoveEwicMappingCommandHandler commandHandler;
        private IconContext context;
        private List<string> testWfmScanCodes;
        private List<Agency> testAgencies;
        private List<string> testAgenciesId;
        private List<Item> testItems;
        private string testAplScanCode;
        private List<AuthorizedProductList> testApl;
        private ItemTestHelper itemTestHelper;
        private TransactionScope transactionScope;

        [TestInitialize]
        public void Initialize()
        {
            transactionScope = new TransactionScope();

            context = new IconContext();

            itemTestHelper = new ItemTestHelper();
            itemTestHelper.Initialize(context.Database.Connection, false, false);

            commandHandler = new RemoveEwicMappingCommandHandler(this.context);

            testWfmScanCodes = new List<string>
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

            testAplScanCode = "22222222228";
            
        }

        [TestCleanup]
        public void Cleanup()
        {
            transactionScope.Dispose();
        }

        private void StageTestItems()
        {

            itemTestHelper.TestScanCode = testWfmScanCodes[0];
            var testItem1 = itemTestHelper.CreateDefaultTestItem();
            itemTestHelper.SaveItem(testItem1);

            itemTestHelper.TestScanCode = testWfmScanCodes[1];
            var testItem2 = itemTestHelper.CreateDefaultTestItem();
            itemTestHelper.SaveItem(testItem2);
            testItems = new List<Item>
            {
                context.Item.First(i => i.ItemId == testItem1.ItemId),
                context.Item.First(i => i.ItemId == testItem2.ItemId),
            };

        }

        private void StageTestApl()
        {
            testApl = new List<AuthorizedProductList>
            {
                new TestAplBuilder().WithAgencyId(testAgenciesId[0]).WithScanCode(testAplScanCode),
                new TestAplBuilder().WithAgencyId(testAgenciesId[1]).WithScanCode(testAplScanCode),
            };

            context.AuthorizedProductList.AddRange(testApl);
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

        [TestMethod]
        public void RemoveEwicMapping_AgencyDoesNotHaveTheMapping_NoActionShouldBeTaken()
        {
            // Given.
            StageTestAgencies();
            
            var command = new RemoveEwicMappingCommand
            {
                AplScanCode = testAplScanCode,
                WfmScanCode = testWfmScanCodes[0]
            };

            // When.
            commandHandler.Execute(command);

            // Then.
            string testAgencyId = testAgenciesId[0];
            var mappings = context.Mapping.Where(m => m.AplScanCode == command.AplScanCode && m.ScanCode.scanCode == command.WfmScanCode).ToList();

            Assert.AreEqual(0, mappings.Count);
        }

        [TestMethod]
        public void RemoveEwicMapping_OneAgencyHasTheMapping_TheMappingShouldBeRemoved()
        {
            // Given.
            StageTestAgencies();
            StageTestItems();
            StageTestApl();

            Mapping testMapping = new TestEwicMappingBuilder().WithAgencyId(testAgenciesId[0]).WithAplScanCode(testAplScanCode).WithWfmScanCode(testItems[0].ScanCode.Single());
            context.Mapping.Add(testMapping);
            context.SaveChanges();

            var command = new RemoveEwicMappingCommand
            {
                AplScanCode = testAplScanCode,
                WfmScanCode = testWfmScanCodes[0]
            };

            // When.
            commandHandler.Execute(command);

            // Then.
            string testAgencyId = testAgenciesId[0];
            var mappings = context.Mapping.Where(m => m.AplScanCode == command.AplScanCode && m.ScanCode.scanCode == command.WfmScanCode).ToList();

            Assert.AreEqual(0, mappings.Count);
        }

        [TestMethod]
        public void RemoveEwicMapping_TwoAgenciesHaveTheMapping_BothMappingsShouldBeRemoved()
        {
            // Given.
            StageTestAgencies();
            StageTestItems();
            StageTestApl();

            List<Mapping> testMappings = new List<Mapping>
            {
                new TestEwicMappingBuilder().WithAgencyId(testAgenciesId[0]).WithAplScanCode(testAplScanCode).WithWfmScanCode(testItems[0].ScanCode.Single()),
                new TestEwicMappingBuilder().WithAgencyId(testAgenciesId[1]).WithAplScanCode(testAplScanCode).WithWfmScanCode(testItems[0].ScanCode.Single())
            };
                
            context.Mapping.AddRange(testMappings);
            context.SaveChanges();

            var command = new RemoveEwicMappingCommand
            {
                AplScanCode = testAplScanCode,
                WfmScanCode = testWfmScanCodes[0]
            };

            // When.
            commandHandler.Execute(command);

            // Then.
            string firstTestAgencyId = testAgenciesId[0];
            string secondTestAgencyId = testAgenciesId[1];

            var firstMapping = context.Mapping.SingleOrDefault(m => m.AgencyId == firstTestAgencyId && m.AplScanCode == command.AplScanCode && m.ScanCode.scanCode == command.WfmScanCode);
            var secondMapping = context.Mapping.SingleOrDefault(m => m.AgencyId == secondTestAgencyId && m.AplScanCode == command.AplScanCode && m.ScanCode.scanCode == command.WfmScanCode);

            Assert.IsNull(firstMapping);
            Assert.IsNull(secondMapping);
        }

        [TestMethod]
        public void RemoveEwicMapping_OneAgencyHasTheMappingAndOneDoesNot_OneMappingShouldBeRemovedAndOneUnmodified()
        {
            // Given.
            StageTestAgencies();
            StageTestItems();
            StageTestApl();

            List<Mapping> testMappings = new List<Mapping>
            {
                new TestEwicMappingBuilder().WithAgencyId(testAgenciesId[0]).WithAplScanCode(testAplScanCode).WithWfmScanCode(testItems[0].ScanCode.Single()),
                new TestEwicMappingBuilder().WithAgencyId(testAgenciesId[1]).WithAplScanCode(testAplScanCode).WithWfmScanCode(testItems[1].ScanCode.Single())
            };

            context.Mapping.AddRange(testMappings);
            context.SaveChanges();

            var command = new RemoveEwicMappingCommand
            {
                AplScanCode = testAplScanCode,
                WfmScanCode = testWfmScanCodes[0]
            };

            // When.
            commandHandler.Execute(command);

            // Then.
            string firstTestAgencyId = testAgenciesId[0];
            string secondTestAgencyId = testAgenciesId[1];
            string unmodifiedTestWfmScanCode = testWfmScanCodes[1];

            var firstMapping = context.Mapping.SingleOrDefault(m => m.AgencyId == firstTestAgencyId && m.AplScanCode == command.AplScanCode && m.ScanCode.scanCode == command.WfmScanCode);
            var secondMapping = context.Mapping.SingleOrDefault(m => m.AgencyId == secondTestAgencyId && m.AplScanCode == command.AplScanCode && m.ScanCode.scanCode == unmodifiedTestWfmScanCode);

            Assert.IsNull(firstMapping);
            Assert.IsNotNull(secondMapping);
        }
    }
}
