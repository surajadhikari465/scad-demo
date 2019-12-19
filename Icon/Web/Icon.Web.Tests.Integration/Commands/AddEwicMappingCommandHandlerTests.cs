using Icon.Framework;
using Icon.Testing.Builders;
using Icon.Web.DataAccess.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Icon.Web.Tests.Integration.Commands
{
    [TestClass] [Ignore]
    public class AddEwicMappingCommandHandlerTests
    {
        private AddEwicMappingCommandHandler commandHandler;
        
        private IconContext context;
        private DbContextTransaction transaction;
        private string testMapping;
        private string testAplScanCode;
        private List<Agency> testAgencies;
        private List<string> testAgenciesId;
        private Item testItem;
        private List<AuthorizedProductList> testApl;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            commandHandler = new AddEwicMappingCommandHandler(this.context);

            testMapping = "22222222228";
            testAplScanCode = "22222222229";

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

        [TestMethod]
        public void AddEwicMapping_OneAgencyHasTheAplScanCode_MappingShouldBeAddedForOneAgency()
        {
            // Given.
            testAgencies = new List<Agency>
            {
                new TestAgencyBuilder().WithAgencyId(testAgenciesId[0])
            };

            context.Agency.AddRange(testAgencies);
            context.SaveChanges();

            testApl = new List<AuthorizedProductList> { new TestAplBuilder().WithAgencyId(testAgenciesId[0]).WithScanCode(testAplScanCode) };
            context.AuthorizedProductList.AddRange(testApl);
            context.SaveChanges();

            testItem = new TestItemBuilder().WithScanCode(testMapping);
            context.Item.Add(testItem);
            context.SaveChanges();

            var command = new AddEwicMappingCommand
            {
                WfmScanCode = testMapping,
                AplScanCode = testAplScanCode,
                Agencies = testAgencies
            };

            // When.
            commandHandler.Execute(command);

            // Then.
            string testAgencyId = testAgenciesId[0];
            var newMapping = context.Mapping.Single(m => m.AgencyId == testAgencyId && m.AplScanCode == command.AplScanCode && m.ScanCode.scanCode == command.WfmScanCode);

            Assert.AreEqual(testItem.ScanCode.Single().scanCodeID, newMapping.ScanCodeId);
            Assert.AreEqual(testAplScanCode, newMapping.AplScanCode);
            Assert.AreEqual(testAgencyId, newMapping.AgencyId);
        }

        [TestMethod]
        public void AddEwicMapping_TwoAgenciesHaveTheAplScanCode_MappingShouldBeAddedForBothAgencies()
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
                new TestAplBuilder().WithAgencyId(testAgenciesId[0]).WithScanCode(testAplScanCode),
                new TestAplBuilder().WithAgencyId(testAgenciesId[1]).WithScanCode(testAplScanCode)
            };

            context.AuthorizedProductList.AddRange(testApl);
            context.SaveChanges();

            testItem = new TestItemBuilder().WithScanCode(testMapping);
            context.Item.Add(testItem);
            context.SaveChanges();

            var command = new AddEwicMappingCommand
            {
                WfmScanCode = testMapping,
                AplScanCode = testAplScanCode,
                Agencies = testAgencies
            };

            // When.
            commandHandler.Execute(command);

            // Then.
            string firstTestAgencyId = testAgenciesId[0];
            string secondTestAgencyId = testAgenciesId[1];

            var firstMapping = context.Mapping.Single(m => m.AgencyId == firstTestAgencyId && m.AplScanCode == command.AplScanCode && m.ScanCode.scanCode == command.WfmScanCode);
            var secondMapping = context.Mapping.Single(m => m.AgencyId == secondTestAgencyId && m.AplScanCode == command.AplScanCode && m.ScanCode.scanCode == command.WfmScanCode);

            Assert.AreEqual(testItem.ScanCode.Single().scanCodeID, firstMapping.ScanCodeId);
            Assert.AreEqual(testAplScanCode, firstMapping.AplScanCode);
            Assert.AreEqual(firstTestAgencyId, firstMapping.AgencyId);
            Assert.AreEqual(testItem.ScanCode.Single().scanCodeID, secondMapping.ScanCodeId);
            Assert.AreEqual(testAplScanCode, secondMapping.AplScanCode);
            Assert.AreEqual(secondTestAgencyId, secondMapping.AgencyId);
        }

        [TestMethod]
        public void AddEwicMapping_OneAgencyHasTheAplScanCodeAndOneDoesNot_MappingShouldBeAddedForTheAgencyWithTheAplScanCode()
        {
            // Given.
            testAgencies = new List<Agency>
            {
                new TestAgencyBuilder().WithAgencyId(testAgenciesId[0]),
                new TestAgencyBuilder().WithAgencyId(testAgenciesId[1])
            };

            context.Agency.AddRange(testAgencies);
            context.SaveChanges();

            testApl = new List<AuthorizedProductList> { new TestAplBuilder().WithAgencyId(testAgenciesId[0]).WithScanCode(testAplScanCode) };
            context.AuthorizedProductList.AddRange(testApl);
            context.SaveChanges();

            testItem = new TestItemBuilder().WithScanCode(testMapping);
            context.Item.Add(testItem);
            context.SaveChanges();

            var command = new AddEwicMappingCommand
            {
                WfmScanCode = testMapping,
                AplScanCode = testAplScanCode,
                Agencies = new List<Agency> { testAgencies[0] }
            };

            // When.
            commandHandler.Execute(command);

            // Then.
            string firstTestAgencyId = testAgenciesId[0];
            string secondTestAgencyId = testAgenciesId[1];

            var firstMapping = context.Mapping.Single(m => m.AgencyId == firstTestAgencyId && m.AplScanCode == command.AplScanCode && m.ScanCode.scanCode == command.WfmScanCode);
            var secondMapping = context.Mapping.SingleOrDefault(m => m.AgencyId == secondTestAgencyId && m.AplScanCode == command.AplScanCode && m.ScanCode.scanCode == command.WfmScanCode);

            Assert.AreEqual(testItem.ScanCode.Single().scanCodeID, firstMapping.ScanCodeId);
            Assert.AreEqual(testAplScanCode, firstMapping.AplScanCode);
            Assert.AreEqual(firstTestAgencyId, firstMapping.AgencyId);

            Assert.IsNull(secondMapping);
        }
    }
}
