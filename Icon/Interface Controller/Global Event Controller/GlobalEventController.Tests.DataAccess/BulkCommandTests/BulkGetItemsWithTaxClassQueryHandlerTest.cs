using GlobalEventController.Common;
using GlobalEventController.DataAccess.BulkCommands;
using GlobalEventController.Testing.Common;
using Irma.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace GlobalEventController.Tests.DataAccess.BulkCommandTests
{
    [TestClass]
    public class BulkGetItemsWithTaxClassQueryHandlerTest
    {
        private BulkGetItemsWithTaxClassQueryHandler handler;
        private BulkGetItemsWithTaxClassQuery command;
        private IrmaDbContextFactory contextFactory;
        private IrmaContext context;
        private List<ValidatedItemModel> validatedItems;
        private TransactionScope transaction;

        [TestInitialize]
        public void InitializeData()
        {
            this.transaction = new TransactionScope();
            this.context = new IrmaContext();
            this.command = new BulkGetItemsWithTaxClassQuery();
            this.contextFactory = new IrmaDbContextFactory();
            this.handler = new BulkGetItemsWithTaxClassQueryHandler(contextFactory);
            this.validatedItems = new List<ValidatedItemModel>();
        }

        [TestCleanup]
        public void CleanupData()
        {
            this.transaction.Dispose();
        }

        [TestMethod]
        public void BulkGetTax_ItemsWithTwoTaxClassCodesThatDoNotExist_ReturnsNoRows()
        {
            // Given
            string expectedTaxClassOne = "3456345 Test Tax Class 1";
            string expectedTaxClassTwo = "3456344 Test Tax Class 2";
            string identifierOne = this.context.ItemIdentifier.First(ii => ii.Deleted_Identifier == 0 && ii.Default_Identifier == 1 && ii.Item.Retail_Sale == true).Identifier;
            string identifierTwo = this.context.ItemIdentifier
                .First(ii => ii.Deleted_Identifier == 0 && ii.Default_Identifier == 1 && ii.Item.Retail_Sale == true && ii.Identifier != identifierOne).Identifier;
            this.validatedItems.Add(BuildValidatedItem(expectedTaxClassOne, identifierOne, 1));
            this.validatedItems.Add(BuildValidatedItem(expectedTaxClassTwo, identifierTwo, 2));
            this.command.ValidatedItems = this.validatedItems;

            // When
            List<ValidatedItemModel> actualItems = this.handler.Handle(this.command);

            // Then
           
            Assert.AreEqual(actualItems.Count, 0, "The Tax Class was not null as expected");
        }

        [TestMethod]
        public void BulkGetTax_ItemWithTaxClassThatExistsAlready_RetrunsCorrectCount()
        {
            // Given
            TaxClass existingTaxClass = this.context.TaxClass.First();
            string identifier = this.context.ItemIdentifier.First(ii => ii.Deleted_Identifier == 0 && ii.Default_Identifier == 1 && ii.Item.Retail_Sale == true).Identifier;
            this.validatedItems.Add(BuildValidatedItem(existingTaxClass.TaxClassDesc, identifier, 1));
            this.command.ValidatedItems = this.validatedItems;

            string existingExternalCode = existingTaxClass.ExternalTaxGroupCode;

            // When
            List<ValidatedItemModel> actualItems = this.handler.Handle(this.command);

            // Then
            List<TaxClass> actualTaxClass = this.context.TaxClass.Where(tc => tc.TaxClassDesc == existingTaxClass.TaxClassDesc).ToList();

            Assert.AreEqual(validatedItems.Count, actualItems.Count, "The extepcted no of items with existing tax codes does not match");
        }

        private ValidatedItemModel BuildValidatedItem(string taxClass, string identifier, int itemId)
        {
            return new TestValidatedItemModelBuilder()
                .WithItemId(itemId)
                .WithTaxClass(taxClass)
                .WithScanCode(identifier)
                .Build();
        }
    }
}

