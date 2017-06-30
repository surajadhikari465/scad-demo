using GlobalEventController.DataAccess.Commands;
using Irma.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.Entity;
using System.Linq;
using System.Transactions;

namespace GlobalEventController.Tests.DataAccess.CommandTests
{
    [TestClass]
    public class AddTaxClassCommandHandlerTests
    {
        private AddTaxClassCommandHandler handler;
        private AddTaxClassCommand command;
        private IrmaDbContextFactory contextFactory;
        private IrmaContext context;
        private TransactionScope transaction;

        [TestInitialize]
        public void InitializeData()
        {
            this.transaction = new TransactionScope();
            this.context = new IrmaContext();
            this.command = new AddTaxClassCommand();
            this.contextFactory = new IrmaDbContextFactory();
            this.handler = new AddTaxClassCommandHandler(contextFactory);
        }

        [TestCleanup]
        public void CleanupData()
        {
            this.transaction.Dispose();
        }

        [TestMethod]
        public void AddTaxClassCommandHandler_TaxClassDoesNotExist_TaxClassAdded()
        {
            // Given
            this.command.TaxClassDescription = "3142143 GlobalController Tax Class";
            this.command.TaxCode = "3142143";

            // When
            this.handler.Handle(this.command);

            // Then
            TaxClass actual = this.context.TaxClass.AsNoTracking().FirstOrDefault(tc => tc.TaxClassDesc == this.command.TaxClassDescription);
            if (actual == null)
            {
                Assert.Fail(String.Format("The Tax Class [{0}] was not added as expected.", this.command.TaxClassDescription));
            }

            var entry = this.context.Entry(actual);
            Assert.AreEqual(this.command.TaxClassDescription, actual.TaxClassDesc);
            Assert.AreEqual(this.command.TaxCode, actual.ExternalTaxGroupCode);
            Assert.AreEqual(actual.TaxClassID, this.command.TaxClassId);
        }

        [TestMethod]
        public void AddTaxClassCommandHandler_TaxClassExists_TaxClassIdIsSet()
        {
            // Given
            TaxClass existingTaxClass = this.context.TaxClass.First(tc => !tc.TaxClassDesc.ToLower().Contains("do not use"));
            string existingTaxDesc = existingTaxClass.TaxClassDesc;
            string existingTaxCode = existingTaxClass.ExternalTaxGroupCode;

            this.command.TaxClassDescription = existingTaxClass.TaxClassDesc;
            this.command.TaxCode = existingTaxCode;

            // When
            this.handler.Handle(this.command);

            // Then
            Assert.AreEqual(existingTaxClass.TaxClassID, this.command.TaxClassId, "The TaxClassId was not populated for the existing tax class.");
        }
    }
}
