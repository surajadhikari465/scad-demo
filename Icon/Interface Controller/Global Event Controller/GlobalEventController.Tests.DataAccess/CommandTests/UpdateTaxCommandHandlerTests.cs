using GlobalEventController.DataAccess.Commands;
using Irma.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Transactions;

namespace GlobalEventController.Tests.DataAccess.CommandTests
{
    [TestClass]
    public class UpdateTaxCommandHandlerTests
    {
        private UpdateTaxClassCommandHandler handler;
        private UpdateTaxClassCommand command;
        private IrmaContext context;
        private IrmaDbContextFactory contextFactory;
        private TransactionScope transaction;

        [TestInitialize]
        public void InitializeData()
        {
            this.transaction = new TransactionScope();
            this.context = new IrmaContext();
            this.contextFactory = new IrmaDbContextFactory();
            this.command = new UpdateTaxClassCommand();
            this.handler = new UpdateTaxClassCommandHandler(contextFactory);
        }

        [TestCleanup]
        public void CleanupData()
        {
            transaction.Dispose();
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
            Assert.AreNotEqual(this.command.TaxClassDescription, actual.TaxClassDesc, "The TaxClassDesc did not match the expected value.");
            Assert.AreEqual(description, actual.TaxClassDesc, "The TaxClassDesc was not updated as expected");
        }

        [TestMethod]
        public void UpdateTax_TaxClassInIrma_TaxClassDescLengthGreatherThan50Updated()
        {
            // Given
            TaxClass taxClass = this.context.TaxClass.First(tc => !tc.TaxClassDesc.ToLower().Contains("do not use"));
            string description = taxClass.TaxClassDesc;

            this.command.TaxCode = taxClass.ExternalTaxGroupCode;
            this.command.TaxClassDescription = taxClass.TaxClassDesc + "testing tax description length";

            // When
            this.handler.Handle(this.command);

            // Then
            TaxClass actual = this.context.TaxClass.First(tc => tc.TaxClassID == taxClass.TaxClassID);
            TaxClass result = this.context.TaxClass.AsNoTracking().FirstOrDefault(tc => tc.TaxClassDesc == this.command.TaxClassDescription);
            
            Assert.AreEqual(this.command.TaxClassDescription, result.TaxClassDesc);
            Assert.AreNotEqual(description, result.TaxClassDesc);
            Assert.AreEqual(this.command.TaxCode, result.ExternalTaxGroupCode);
            Assert.AreEqual(actual.TaxClassID, this.command.TaxClassId);
        }
    }
}