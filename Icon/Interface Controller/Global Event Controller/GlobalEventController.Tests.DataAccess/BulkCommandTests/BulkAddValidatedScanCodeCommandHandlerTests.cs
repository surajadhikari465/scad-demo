using GlobalEventController.Common;
using GlobalEventController.DataAccess.BulkCommands;
using GlobalEventController.Testing.Common;
using Irma.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;

namespace GlobalEventController.Tests.DataAccess.BulkCommandTests
{
    [TestClass]
    public class BulkAddValidatedScanCodeCommandHandlerTests
    {
        private IrmaContext context;
        private BulkAddValidatedScanCodeCommand command;
        private BulkAddValidatedScanCodeCommandHandler handler;
        private List<ValidatedItemModel> validatedItems;
        private DbContextTransaction transaction;

        [TestInitialize]
        public void InitializeData()
        {
            this.context = new IrmaContext(); // this is the FL ItemCatalog_Test database
            this.command = new BulkAddValidatedScanCodeCommand();
            this.handler = new BulkAddValidatedScanCodeCommandHandler(this.context);
            this.validatedItems = new List<ValidatedItemModel>();

            this.transaction = this.context.Database.BeginTransaction();
        }

        [TestCleanup]
        public void CleanupData()
        {
            if (transaction != null)
            {
                this.transaction.Rollback();
            }
        }

        [TestMethod]
        [ExpectedException(typeof(SqlException))]
        public void BulkAddValidatedScanCode_ScanCodeIsNotValidatedButNotInIRMA_SqlExceptionExpected()
        {
            // Given
            string scanCode = "778877667755";
            if (this.context.ValidatedScanCode.Any(vs => vs.ScanCode == scanCode))
            {
                Assert.Fail(String.Format("The test scan code {0} is already validated.", scanCode));
            }

            this.validatedItems.Add(new TestValidatedItemModelBuilder().WithScanCode(scanCode).Build());
            this.command.ValidatedItems = this.validatedItems;

            // When
            this.handler.Handle(this.command);

            // Then
            // Expected SqlException based on the 'fn_DoesScanCodeExist' constraint on dbo.ValidatedScanCode table
        }

        [TestMethod]
        public void BulkAddValidatedScanCode_DefaultIdentifierIsNotValidated_ScanCodeAddedToValidatedScanCodeTable()
        {
            // Given
            string scanCode = (from ii in this.context.ItemIdentifier
                               join vs in this.context.ValidatedScanCode on ii.Identifier equals vs.ScanCode into temp
                               from vs in temp.DefaultIfEmpty()
                               where 
                                    vs.ScanCode == null
                                    && ii.Default_Identifier == 1
                                    && ii.Deleted_Identifier == 0
                               select ii.Identifier).First();

            if (this.context.ValidatedScanCode.Any(vs => vs.ScanCode == scanCode))
            {
                Assert.Fail(String.Format("The test scan code {0} is already validated.", scanCode));
            }

            this.validatedItems.Add(new TestValidatedItemModelBuilder().WithScanCode(scanCode).Build());
            this.command.ValidatedItems = this.validatedItems;

            // When
            this.handler.Handle(this.command);

            // Then
            ValidatedScanCode actualValidatedScanCode = this.context.ValidatedScanCode.SingleOrDefault(vs => vs.ScanCode == scanCode);
            Assert.IsNotNull(actualValidatedScanCode);
        }

        [TestMethod]
        public void BulkAddValidatedScanCode_NonDefaultIdentifierIsNotValidated_ScanCodeAddedtoValidatedScanCodeTable()
        {
            // Given
            string scanCode = (from ii in this.context.ItemIdentifier
                               join vs in this.context.ValidatedScanCode on ii.Identifier equals vs.ScanCode into temp
                               from vs in temp.DefaultIfEmpty()
                               where
                                    vs.ScanCode == null
                                    && ii.Default_Identifier == 0
                                    && ii.Deleted_Identifier == 0
                               select ii.Identifier).First();

            if (this.context.ValidatedScanCode.Any(vs => vs.ScanCode == scanCode))
            {
                Assert.Fail(String.Format("The test scan code {0} is already validated.", scanCode));
            }

            this.validatedItems.Add(new TestValidatedItemModelBuilder().WithScanCode(scanCode).Build());
            this.command.ValidatedItems = this.validatedItems;

            // When
            this.handler.Handle(this.command);

            // Then
            ValidatedScanCode actualValidatedScanCode = this.context.ValidatedScanCode.SingleOrDefault(vs => vs.ScanCode == scanCode);
            Assert.IsNotNull(actualValidatedScanCode);
        }

        [TestMethod]
        public void BulkAddValidatedScanCode_ScanCodeAlreadyValidated_NotAddedToValidatedScanCodeTableAgain()
        {
            // Given
            var scanCode = (from ii in this.context.ItemIdentifier
                            where !this.context.ValidatedScanCode.Any(vsc => vsc.ScanCode == ii.Identifier)
                                && ii.Default_Identifier == 0
                                && ii.Deleted_Identifier == 0
                            select ii.Identifier).First();

            if (this.context.ValidatedScanCode.Any(vs => vs.ScanCode == scanCode))
            {
                Assert.Fail(String.Format("The test scan code {0} is already validated.", scanCode));
            }

            this.validatedItems.Add(new TestValidatedItemModelBuilder().WithScanCode(scanCode).Build());
            this.command.ValidatedItems = this.validatedItems;

            // When
            this.handler.Handle(this.command);

            // Then
            IEnumerable<ValidatedScanCode> actualValidatedScanCodes = this.context.ValidatedScanCode.Where(vs => vs.ScanCode == scanCode);
            Assert.AreEqual(1, actualValidatedScanCodes.Count());
        }
    }
}
