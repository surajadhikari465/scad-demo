using Icon.RenewableContext;
using Icon.Esb.EwicAplListener.DataAccess.Commands;
using Icon.Framework;
using Icon.Framework.RenewableContext;
using Icon.Testing.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.Entity;
using System.Linq;

namespace Icon.Esb.EwicAplListener.Tests.Integration.Commands
{
    [TestClass]
    public class AddOrUpdateAuthorizedProductListCommandTests
    {
        private AddOrUpdateAuthorizedProductListCommand command;
        private IRenewableContext<IconContext> globalContext;
        private IconContext context;
        private DbContextTransaction transaction;
        private AuthorizedProductList testApl;
        private string testAgencyId;
        private string testScanCode;
        private string updatedItemDescription;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            globalContext = new GlobalIconContext(context);

            command = new AddOrUpdateAuthorizedProductListCommand(globalContext);

            testAgencyId = "ZZ";
            testScanCode = "222222222";
            updatedItemDescription = "Updated Description";

            transaction = context.Database.BeginTransaction();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
        }

        private void StageTestAgency()
        {
            context.Agency.Add(new Agency { AgencyId = testAgencyId });
            context.SaveChanges();
        }

        private void StageTestApl()
        {
            testApl = new TestAplBuilder().WithAgencyId(testAgencyId).WithScanCode(testScanCode);

            context.AuthorizedProductList.Add(testApl);
            context.SaveChanges();
        }

        [TestMethod]
        public void AddOrUpdateApl_AplEntryDoesNotExist_AplEntryShouldBeAdded()
        {
            // Given.
            StageTestAgency();

            testApl = new TestAplBuilder().WithAgencyId(testAgencyId).WithScanCode(testScanCode);

            var parameters = new AddOrUpdateAuthorizedProductListParameters { Apl = testApl };

            // When.
            command.Execute(parameters);

            // Then.
            var newApl = context.AuthorizedProductList.SingleOrDefault(apl => apl.AgencyId == testAgencyId && apl.ScanCode == testScanCode);

            Assert.IsNotNull(newApl);
            Assert.AreEqual(testApl.ScanCode, newApl.ScanCode);
            Assert.AreEqual(testApl.ItemDescription, newApl.ItemDescription);
            Assert.AreEqual(testApl.ItemPrice, newApl.ItemPrice);
            Assert.AreEqual(testApl.PackageSize, newApl.PackageSize);
            Assert.AreEqual(testApl.BenefitQuantity, newApl.BenefitQuantity);
            Assert.AreEqual(testApl.BenefitUnitDescription, newApl.BenefitUnitDescription);
            Assert.AreEqual(testApl.PriceType, newApl.PriceType);
            Assert.AreEqual(testApl.UnitOfMeasure, newApl.UnitOfMeasure);
            Assert.AreEqual(DateTime.Now.Date, newApl.InsertDate.Date);
            Assert.IsNull(newApl.ModifiedDate);
        }

        [TestMethod]
        public void AddOrUpdateApl_NewAplEntryContainsOnlyRequiredFields_AplEntryShouldBeAdded()
        {
            // Given.
            StageTestAgency();

            testApl = new TestAplBuilder().Empty().WithAgencyId(testAgencyId).WithScanCode(testScanCode);

            var parameters = new AddOrUpdateAuthorizedProductListParameters { Apl = testApl };

            // When.
            command.Execute(parameters);

            // Then.
            var newApl = context.AuthorizedProductList.SingleOrDefault(apl => apl.AgencyId == testAgencyId && apl.ScanCode == testScanCode);

            Assert.IsNotNull(newApl);
        }

        [TestMethod]
        public void AddOrUpdateApl_AplEntryExists_AplEntryShouldBeUpdated()
        {
            // Given.
            StageTestAgency();
            StageTestApl();

            var updatedApl = new TestAplBuilder().WithAgencyId(testAgencyId).WithScanCode(testScanCode).WithItemDescription(updatedItemDescription);

            var parameters = new AddOrUpdateAuthorizedProductListParameters { Apl = updatedApl };

            // When.
            command.Execute(parameters);

            // Then.
            var aplEntry = context.AuthorizedProductList.Single(apl => apl.AgencyId == testAgencyId && apl.ScanCode == testScanCode);

            Assert.AreEqual(updatedItemDescription, aplEntry.ItemDescription);
            Assert.AreEqual(DateTime.Now.Date, aplEntry.ModifiedDate.Value.Date);
        }
    }
}
