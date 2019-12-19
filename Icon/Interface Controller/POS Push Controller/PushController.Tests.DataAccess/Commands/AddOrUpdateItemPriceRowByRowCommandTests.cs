using Icon.Framework;
using Icon.Framework.RenewableContext;
using Icon.Testing.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PushController.DataAccess.Commands;
using System;
using System.Data.Entity;
using System.Linq;

namespace PushController.Tests.DataAccess.Commands
{
    [TestClass]
    public class AddOrUpdateItemPriceRowByRowCommandTests
    {
        private AddOrUpdateItemPriceRowByRowCommandHandler addOrUpdateItemPriceCommandHandler;
        private GlobalIconContext context;
        private DbContextTransaction transaction;
        private ItemPrice testItemPrice;
        private Item testItem;
        private Locale testLocale;
        private decimal updatedPrice;
        private int unknownLocaleId;
        private int unknownUomId;

        [TestInitialize]
        public void Initialize()
        {
            context = new GlobalIconContext(new IconContext());

            addOrUpdateItemPriceCommandHandler = new AddOrUpdateItemPriceRowByRowCommandHandler(context);

            unknownLocaleId = 9999;
            unknownUomId = 99;

            transaction = context.Context.Database.BeginTransaction();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
        }

        private void SetupTestData()
        {
            testItem = new TestItemBuilder();
            testLocale = new TestLocaleBuilder();
            updatedPrice = 8.99m;

            context.Context.Item.Add(testItem);
            context.Context.Locale.Add(testLocale);
            context.Context.SaveChanges();
        }

        private void SetupExistingItemPrice()
        {
            testItemPrice = new TestItemPriceBuilder().WithItemId(testItem.ItemId).WithLocaleId(testLocale.localeID);
            context.Context.ItemPrice.Add(testItemPrice);
            context.Context.SaveChanges();
        }

        [TestMethod]
        public void AddOrUpdateItemPrice_PriceDoesNotExist_PriceShouldBeAdded()
        {
            // Given.
            SetupTestData();
            testItemPrice = new TestItemPriceBuilder().WithItemId(testItem.ItemId).WithLocaleId(testLocale.localeID);

            var command = new AddOrUpdateItemPriceRowByRowCommand
            {
                ItemPriceEntity = testItemPrice
            };

            // When.
            addOrUpdateItemPriceCommandHandler.Execute(command);

            // Then.
            var newItemPrice = context.Context.ItemPrice.Single(ip => 
                ip.itemID == testItem.ItemId && ip.localeID == testLocale.localeID && ip.itemPriceTypeID == testItemPrice.itemPriceTypeID);

            Assert.IsNotNull(newItemPrice);
            Assert.AreEqual(testItemPrice.itemPriceAmt, newItemPrice.itemPriceAmt);
        }

        [TestMethod]
        public void AddOrUpdateItemPrice_PriceDoesNotExistAndInsertFails_EntityShouldBeDetachedFromTheContext()
        {
            // Given.
            SetupTestData();

            testItemPrice = new TestItemPriceBuilder().WithItemId(testItem.ItemId).WithLocaleId(unknownLocaleId);

            var command = new AddOrUpdateItemPriceRowByRowCommand
            {
                ItemPriceEntity = testItemPrice
            };

            // When.
            try { addOrUpdateItemPriceCommandHandler.Execute(command); }
            catch (Exception) { }

            // Then.
            Assert.AreEqual(EntityState.Detached, context.Context.Entry(command.ItemPriceEntity).State);
        }

        [TestMethod]
        public void AddOrUpdateItemPrice_PriceExists_PriceShouldBeUpdated()
        {
            // Given.
            SetupTestData();
            SetupExistingItemPrice();

            testItemPrice.itemPriceAmt = updatedPrice;

            var command = new AddOrUpdateItemPriceRowByRowCommand
            {
                ItemPriceEntity = testItemPrice
            };

            // When.
            addOrUpdateItemPriceCommandHandler.Execute(command);

            // Then.
            var updatedItemPrice = context.Context.ItemPrice.Single(ip =>
                ip.itemID == testItem.ItemId && ip.localeID == testLocale.localeID && ip.itemPriceTypeID == testItemPrice.itemPriceTypeID);

            Assert.IsNotNull(updatedItemPrice);
            Assert.AreEqual(updatedPrice, updatedItemPrice.itemPriceAmt);
        }

        [TestMethod]
        public void AddOrUpdateItemPrice_PriceExistsAndUpdateFails_EntityShouldBeDetachedFromTheContext()
        {
            // Given.
            SetupTestData();
            SetupExistingItemPrice();

            testItemPrice.itemPriceAmt = updatedPrice;
            testItemPrice.uomID = unknownUomId;

            var command = new AddOrUpdateItemPriceRowByRowCommand
            {
                ItemPriceEntity = testItemPrice
            };

            // When.
            try { addOrUpdateItemPriceCommandHandler.Execute(command); }
            catch (Exception) { }

            // Then.
            Assert.AreEqual(EntityState.Detached, context.Context.Entry(command.ItemPriceEntity).State);
        }
    }
}
