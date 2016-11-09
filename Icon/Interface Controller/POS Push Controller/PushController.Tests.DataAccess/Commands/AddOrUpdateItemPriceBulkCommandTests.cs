using Icon.Framework;
using Icon.Framework.RenewableContext;
using Icon.Logging;
using Icon.Testing.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PushController.DataAccess.Commands;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace PushController.Tests.DataAccess.Commands
{
    [TestClass]
    public class AddOrUpdateItemPriceBulkCommandTests
    {
        private AddOrUpdateItemPriceBulkCommandHandler addOrUpdateItemPriceCommandHandler;
        private GlobalIconContext context;
        private DbContextTransaction transaction;
        private Mock<ILogger<AddOrUpdateItemPriceBulkCommandHandler>> mockLogger;
        private List<ItemPrice> testItemPrices;
        private List<Item> testItems;
        private Locale testLocale;
        private string testScanCode;
        private decimal updatedPriceAmount;

        [TestInitialize]
        public void Initialize()
        {
            this.testScanCode = "2222222";
            this.updatedPriceAmount = 8.99m;

            this.context = new GlobalIconContext(new IconContext());

            this.mockLogger = new Mock<ILogger<AddOrUpdateItemPriceBulkCommandHandler>>();
            this.addOrUpdateItemPriceCommandHandler = new AddOrUpdateItemPriceBulkCommandHandler(mockLogger.Object, context);

            this.transaction = context.Context.Database.BeginTransaction();
        }

        [TestCleanup]
        public void Cleanup()
        {
            this.transaction.Rollback();
        }

        private void SetupTestData()
        {
            this.testItems = new List<Item>
            {
                new TestItemBuilder().WithScanCode(this.testScanCode),
                new TestItemBuilder().WithScanCode(this.testScanCode+"0"),
                new TestItemBuilder().WithScanCode(this.testScanCode+"1")
            };

            this.context.Context.Item.AddRange(testItems);

            this.testLocale = new TestLocaleBuilder();
            this.context.Context.Locale.Add(testLocale);
            
            this.context.Context.SaveChanges();
        }

        private void SetupExistingItemPrice()
        {
            this.testItemPrices = new List<ItemPrice>
            {
                new TestItemPriceBuilder().WithItemId(testItems[0].itemID).WithLocaleId(testLocale.localeID),
                new TestItemPriceBuilder().WithItemId(testItems[1].itemID).WithLocaleId(testLocale.localeID),
                new TestItemPriceBuilder().WithItemId(testItems[2].itemID).WithLocaleId(testLocale.localeID)
            };

            this.context.Context.ItemPrice.AddRange(testItemPrices);
            this.context.Context.SaveChanges();
        }

        [TestMethod]
        public void AddOrUpdateItemPrice_PriceDoesNotExist_PriceShouldBeAdded()
        {
            // Given.
            SetupTestData();

            this.testItemPrices = new List<ItemPrice>
            {
                new TestItemPriceBuilder().WithItemId(testItems[0].itemID).WithLocaleId(testLocale.localeID),
                new TestItemPriceBuilder().WithItemId(testItems[1].itemID).WithLocaleId(testLocale.localeID),
                new TestItemPriceBuilder().WithItemId(testItems[2].itemID).WithLocaleId(testLocale.localeID)
            };

            // When.
            var command = new AddOrUpdateItemPriceBulkCommand
            {
                ItemPrices = testItemPrices
            };

            addOrUpdateItemPriceCommandHandler.Execute(command);

            // Then.

            int testItemId0 = testItemPrices[0].itemID;
            int testItemId1 = testItemPrices[1].itemID;
            int testItemId2 = testItemPrices[2].itemID;
            
            var newItemPrices = new List<ItemPrice>
            {
                context.Context.ItemPrice.Single(ip => 
                    ip.itemID == testItemId0 && ip.localeID == testLocale.localeID && ip.itemPriceTypeID == ItemPriceTypes.Reg),

                context.Context.ItemPrice.Single(ip => 
                    ip.itemID == testItemId1 && ip.localeID == testLocale.localeID && ip.itemPriceTypeID == ItemPriceTypes.Reg),

                context.Context.ItemPrice.Single(ip => 
                    ip.itemID == testItemId2 && ip.localeID == testLocale.localeID && ip.itemPriceTypeID == ItemPriceTypes.Reg)
            };

            decimal newPriceAmount = testItemPrices[0].itemPriceAmt;
            bool newPricesMatch = newItemPrices.TrueForAll(p => p.itemPriceAmt == newPriceAmount);

            Assert.IsTrue(newPricesMatch);
        }

        [TestMethod]
        public void AddOrUpdateItemPrice_PriceExists_PriceShouldBeUpdated()
        {
            // Given.
            SetupTestData();
            SetupExistingItemPrice();
            
            // When.
            testItemPrices[1].itemPriceAmt = updatedPriceAmount;
            var command = new AddOrUpdateItemPriceBulkCommand
            {
                ItemPrices = testItemPrices
            };

            addOrUpdateItemPriceCommandHandler.Execute(command);

            // Then.

            int testItemId0 = testItemPrices[0].itemID;
            int testItemId1 = testItemPrices[1].itemID;
            int testItemId2 = testItemPrices[2].itemID;

            var newItemPrices = new List<ItemPrice>
            {
                context.Context.ItemPrice.Single(ip => 
                    ip.itemID == testItemId0 && ip.localeID == testLocale.localeID && ip.itemPriceTypeID == ItemPriceTypes.Reg),

                context.Context.ItemPrice.Single(ip => 
                    ip.itemID == testItemId1 && ip.localeID == testLocale.localeID && ip.itemPriceTypeID == ItemPriceTypes.Reg),

                context.Context.ItemPrice.Single(ip => 
                    ip.itemID == testItemId2 && ip.localeID == testLocale.localeID && ip.itemPriceTypeID == ItemPriceTypes.Reg)
            };

            decimal existingPriceAmount = testItemPrices[0].itemPriceAmt;
            
            Assert.AreEqual(existingPriceAmount, testItemPrices[0].itemPriceAmt);
            Assert.AreEqual(updatedPriceAmount, testItemPrices[1].itemPriceAmt);
            Assert.AreEqual(existingPriceAmount, testItemPrices[2].itemPriceAmt);
        }
    }
}
