using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Irma.Framework;
using GlobalEventController.DataAccess.Commands;
using System.Linq;
using System.Collections;
using System.Data.Entity;

namespace GlobalEventController.Tests.DataAccess.CommandTests
{
    [TestClass]
    [Ignore] //Ignoring these tests since this functionality has been turned off for 2 years
    public class UpdateTaxCommandHandlerTests
    {
        private IrmaContext context;
        private UpdateTaxClassCommand command;
        private UpdateTaxClassCommandHandler handler;
        private DbContextTransaction transaction;

        [TestInitialize]
        public void InitializeData()
        {
            this.context = new IrmaContext();
            this.command = new UpdateTaxClassCommand();
            this.handler = new UpdateTaxClassCommandHandler(this.context);
            this.transaction = this.context.Database.BeginTransaction();
        }

        [TestCleanup]
        public void CleanupData()
        {
            if (this.transaction != null)
            {
                this.transaction.Rollback();
            }

            if (this.context != null)
            {
                this.context.Dispose();
            }
        }

        [TestMethod]
        public void UpdateTax_TaxClassNotFoundInIrma_TaxClassIdIsSetToZero()
        {
            // Given
            this.command.TaxCode = "8765432";
            this.command.TaxClassId = -3;

            // When
            this.handler.Handle(this.command);

            // Then
            Assert.IsTrue(this.command.TaxClassId == 0, "The TaxClassId is not set to zero as expected");
        }

        [TestMethod]
        public void UpdateTax_TaxClassInIrma_TaxClassIdIsSetToIrmaTaxClassID()
        {
            // Given
            TaxClass taxClass = this.context.TaxClass.First(tc => !tc.TaxClassDesc.ToLower().Contains("do not use"));
            this.command.TaxClassDescription = taxClass.TaxClassDesc;
            this.command.TaxCode = taxClass.ExternalTaxGroupCode;

            // When
            this.handler.Handle(this.command);

            // Then
            Assert.IsTrue(this.command.TaxClassId == taxClass.TaxClassID, "The TaxClassId is not set to the expected value.");
        }

        [TestMethod]
        public void UpdateTax_TaxClassInIrma_TaxClassDescAndExternalCodeAreUpdated()
        {
            // Given
            TaxClass taxClass = this.context.TaxClass.First(tc => !tc.TaxClassDesc.ToLower().Contains("do not use"));
            string description = taxClass.TaxClassDesc;

            this.command.TaxCode = taxClass.ExternalTaxGroupCode;
            this.command.TaxClassDescription = taxClass.TaxClassDesc + "test";

            // When
            this.handler.Handle(this.command);

            // Then
            TaxClass actual = this.context.TaxClass.First(tc => tc.TaxClassID == taxClass.TaxClassID);
            Assert.AreEqual(this.command.TaxClassDescription, actual.TaxClassDesc, "The TaxClassDesc did not match the expected value.");
            Assert.AreNotEqual(description, actual.TaxClassDesc, "The TaxClassDesc was not updated as expected");
        }
    }
}
