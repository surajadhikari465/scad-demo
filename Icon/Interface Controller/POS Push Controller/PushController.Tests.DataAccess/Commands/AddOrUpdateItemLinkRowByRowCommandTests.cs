using Icon.Framework;
using Icon.Framework.RenewableContext;
using Icon.Testing.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PushController.DataAccess.Commands;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace PushController.Tests.DataAccess.Commands
{
    [TestClass]
    public class AddOrUpdateItemLinkRowByRowCommandTests
    {
        private AddOrUpdateItemLinkRowByRowCommandHandler addOrUpdateItemLinkCommandHandler;
        private GlobalIconContext context;
        private DbContextTransaction transaction;
        private ItemLink testItemLink;
        private Item testItem;
        private List<Item> testLinkedItems;
        private Locale testLocale;
        private int unknownLocaleId;
        
        [TestInitialize]
        public void Initialize()
        {
            context = new GlobalIconContext(new IconContext());

            unknownLocaleId = 9999;
            
            addOrUpdateItemLinkCommandHandler = new AddOrUpdateItemLinkRowByRowCommandHandler(context);

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
            testLinkedItems = new List<Item>
            {
                new TestItemBuilder(),
                new TestItemBuilder()
            };

            testLocale = new TestLocaleBuilder();
            
            context.Context.Item.Add(testItem);
            context.Context.Locale.Add(testLocale);
            context.Context.SaveChanges();
        }

        private void SetupExistingItemLink()
        {
            testItemLink = new TestItemLinkBuilder()
                .WithParentItemId(testLinkedItems[0].ItemId).WithChildItemId(testItem.ItemId).WithLocaleId(testLocale.localeID);
                   
            context.Context.ItemLink.Add(testItemLink);
            context.Context.SaveChanges();
        }

        [TestMethod]
        public void AddOrUpdateItemLink_ItemLinkDoesNotExist_ItemLinkShouldBeAdded()
        {
            // Given.
            SetupTestData();

            testItemLink = new TestItemLinkBuilder()
                .WithParentItemId(testLinkedItems[0].ItemId).WithChildItemId(testItem.ItemId).WithLocaleId(testLocale.localeID);

            // When.
            var command = new AddOrUpdateItemLinkRowByRowCommand
            {
                ItemLinkEntity = testItemLink
            };

            addOrUpdateItemLinkCommandHandler.Execute(command);

            // Then.
            var newItemLink = context.Context.ItemLink.Single(il =>
                il.childItemID == testItem.ItemId && il.localeID == testLocale.localeID);

            Assert.IsNotNull(newItemLink);
            Assert.AreEqual(testLinkedItems[0].ItemId, newItemLink.parentItemID);
        }

        [TestMethod]
        public void AddOrUpdateItemLink_ItemLinkDoesNotExistAndInsertFails_EntityShouldBeDetachedFromTheContext()
        {
            // Given.
            SetupTestData();

            testItemLink = new TestItemLinkBuilder()
                .WithParentItemId(testLinkedItems[0].ItemId).WithChildItemId(testItem.ItemId).WithLocaleId(unknownLocaleId);

            var command = new AddOrUpdateItemLinkRowByRowCommand
            {
                ItemLinkEntity = testItemLink
            };

            // When.
            try { addOrUpdateItemLinkCommandHandler.Execute(command); }
            catch (Exception) { }

            // Then.
            Assert.AreEqual(EntityState.Detached, context.Context.Entry(command.ItemLinkEntity).State);
        }

        [TestMethod]
        public void AddOrUpdateItemLink_ItemLinkExists_ItemLinkShouldBeUpdated()
        {
            // Given.
            SetupTestData();
            SetupExistingItemLink();

            testItemLink.parentItemID = testLinkedItems[1].ItemId;

            var command = new AddOrUpdateItemLinkRowByRowCommand
            {
                ItemLinkEntity = testItemLink
            };

            // When.
            addOrUpdateItemLinkCommandHandler.Execute(command);

            // Then.
            var existingItemLink = context.Context.ItemLink.Single(il =>
                il.childItemID == testItem.ItemId && il.localeID == testLocale.localeID);

            Assert.IsNotNull(existingItemLink);
            Assert.AreEqual(testLinkedItems[1].ItemId, existingItemLink.parentItemID);
        }

        [TestMethod]
        public void AddOrUpdateItemLink_ItemLinkExistsAndUpdateFails_EntityShouldBeDetachedFromTheContext()
        {
            // Given.
            SetupTestData();
            SetupExistingItemLink();

            var updatedItemLink = new ItemLink
            {
                parentItemID = testLinkedItems[1].ItemId,
                childItemID = testItem.ItemId,
                localeID = unknownLocaleId
            };
            
            var command = new AddOrUpdateItemLinkRowByRowCommand
            {
                ItemLinkEntity = updatedItemLink
            };

            // When.
            try { addOrUpdateItemLinkCommandHandler.Execute(command); }
            catch (Exception) { }

            // Then.
            Assert.AreEqual(EntityState.Detached, context.Context.Entry(command.ItemLinkEntity).State);
        }
    }
}
