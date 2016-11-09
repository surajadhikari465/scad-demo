using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Irma.Framework;
using System.Data.Entity;
using System.Linq;
using GlobalEventController.DataAccess.Commands;

namespace GlobalEventController.Tests.DataAccess.CommandTests
{
    [TestClass]
    public class AddTaxClassCommandHandlerTests
    {
        private IrmaContext context;
        private DbContextTransaction transaction;
        private AddTaxClassCommand command;
        private AddTaxClassCommandHandler handler;

        [TestInitialize]
        public void InitializeData()
        {
            this.context = new IrmaContext();
            this.command = new AddTaxClassCommand();
            this.handler = new AddTaxClassCommandHandler(this.context);

            this.transaction = this.context.Database.BeginTransaction();
        }

        [TestCleanup]
        public void CleanupData()
        {
            this.transaction.Rollback();
            this.context.Dispose();
        }

        [TestMethod]
        public void AddTaxClassCommandHandler_TaxClassDoesNotExist_TaxClassAdded()
        {
            // Given
            this.command.TaxClassDescription = "3142143 GlobalController Tax Class";
            this.command.TaxCode = "3142143";

            // When
            this.handler.Handle(this.command);
            this.context.SaveChanges();

            // Then
            TaxClass actual = this.context.TaxClass.FirstOrDefault(tc => tc.TaxClassDesc == this.command.TaxClassDescription);
            if (actual == null)
            {
                Assert.Fail(String.Format("The Tax Class [{0}] was not added as expected.", this.command.TaxClassDescription));
            }

            var entry = this.context.Entry(actual);
            Assert.IsTrue(entry.State == EntityState.Unchanged);
            Assert.AreEqual(this.command.TaxClassDescription, actual.TaxClassDesc);
            Assert.AreEqual(this.command.TaxCode, actual.ExternalTaxGroupCode);
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
            this.context.SaveChanges();

            // Then
            Assert.AreEqual(existingTaxClass.TaxClassID, this.command.TaxClassId, "The TaxClassId was not populated for the existing tax class.");
        }
    }
}
