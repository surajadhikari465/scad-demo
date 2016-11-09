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
    public class AddOrUpdateItemLinkBulkCommandTests
    {
        private AddOrUpdateItemLinkBulkCommandHandler addOrUpdateItemLinkCommandHandler;
        private GlobalIconContext context;
        private DbContextTransaction transaction;
        private Mock<ILogger<AddOrUpdateItemLinkBulkCommandHandler>> mockLogger;
        private List<ItemLink> testItemLinks;
        private List<Item> testItems;
        private List<Item> testLinkedItems;
        private Locale testLocale;
        private string testScanCode;
        private string testLinkedScanCode;

        [TestInitialize]
        public void Initialize()
        {
            this.testScanCode = "2222222";
            this.testLinkedScanCode = "88887777666";

            this.context = new GlobalIconContext(new IconContext());

            this.mockLogger = new Mock<ILogger<AddOrUpdateItemLinkBulkCommandHandler>>();
            this.addOrUpdateItemLinkCommandHandler = new AddOrUpdateItemLinkBulkCommandHandler(mockLogger.Object, context);

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

            this.testLinkedItems = new List<Item>
            {
                new TestItemBuilder().WithScanCode(this.testLinkedScanCode),
                new TestItemBuilder().WithScanCode(this.testLinkedScanCode+"0"),
                new TestItemBuilder().WithScanCode(this.testLinkedScanCode+"1")
            };

            this.context.Context.Item.AddRange(testItems);
            this.context.Context.Item.AddRange(testLinkedItems);

            this.testLocale = new TestLocaleBuilder();
            this.context.Context.Locale.Add(testLocale);

            this.context.Context.SaveChanges();
        }

        private void SetupExistingItemLink()
        {
            this.testItemLinks = new List<ItemLink>
            {
                new TestItemLinkBuilder()
                    .WithParentItemId(this.testLinkedItems[0].itemID)
                    .WithChildItemId(this.testItems[0].itemID)
                    .WithLocaleId(this.testLocale.localeID),
                new TestItemLinkBuilder()
                    .WithParentItemId(this.testLinkedItems[0].itemID)
                    .WithChildItemId(this.testItems[1].itemID)
                    .WithLocaleId(this.testLocale.localeID),
                new TestItemLinkBuilder()
                    .WithParentItemId(this.testLinkedItems[0].itemID)
                    .WithChildItemId(this.testItems[2].itemID)
                    .WithLocaleId(this.testLocale.localeID)
            };

            this.context.Context.ItemLink.AddRange(testItemLinks);
            this.context.Context.SaveChanges();
        }

        [TestMethod]
        public void AddOrUpdateItemLink_ItemLinkDoesNotExist_ItemLinkShouldBeAdded()
        {
            // Given.
            SetupTestData();

            this.testItemLinks = new List<ItemLink>
            {
                new TestItemLinkBuilder()
                    .WithParentItemId(testLinkedItems[0].itemID)
                    .WithChildItemId(testItems[0].itemID)
                    .WithLocaleId(testLocale.localeID),
                new TestItemLinkBuilder()
                    .WithParentItemId(testLinkedItems[0].itemID)
                    .WithChildItemId(testItems[1].itemID)
                    .WithLocaleId(testLocale.localeID),
                new TestItemLinkBuilder()
                    .WithParentItemId(testLinkedItems[0].itemID)
                    .WithChildItemId(testItems[2].itemID)
                    .WithLocaleId(testLocale.localeID)
            };

            // When.
            var command = new AddOrUpdateItemLinkBulkCommand
            {
                ItemLinks = testItemLinks
            };

            addOrUpdateItemLinkCommandHandler.Execute(command);

            // Then.

            int testItemId0 = testItemLinks[0].childItemID;
            int testItemId1 = testItemLinks[1].childItemID;
            int testItemId2 = testItemLinks[2].childItemID;

            var newItemLinks = new List<ItemLink>
            {
                context.Context.ItemLink.Single(il =>
                    il.childItemID == testItemId0 && il.localeID == testLocale.localeID),

                context.Context.ItemLink.Single(il =>
                    il.childItemID == testItemId1 && il.localeID == testLocale.localeID),

                context.Context.ItemLink.Single(il =>
                    il.childItemID == testItemId2 && il.localeID == testLocale.localeID)
            };

            int newLinkedItemId = testLinkedItems[0].itemID;
            bool newItemLinksMatch = newItemLinks.TrueForAll(il => il.parentItemID == newLinkedItemId);

            Assert.IsTrue(newItemLinksMatch);
        }

        [TestMethod]
        public void AddOrUpdateItemLink_TraitExists_TraitShouldBeUpdated()
        {
            // Given.
            SetupTestData();
            SetupExistingItemLink();

            testItemLinks[0].parentItemID = testLinkedItems[1].itemID;
            testItemLinks[1].parentItemID = testLinkedItems[1].itemID;
            testItemLinks[2].parentItemID = testLinkedItems[1].itemID;

            var command = new AddOrUpdateItemLinkBulkCommand
            {
                ItemLinks = testItemLinks
            };

            // When.
            addOrUpdateItemLinkCommandHandler.Execute(command);

            // Then.
            int testItemId0 = testItemLinks[0].childItemID;
            int testItemId1 = testItemLinks[1].childItemID;
            int testItemId2 = testItemLinks[2].childItemID;

            var updatedItemLinks = new List<ItemLink>
            {
                context.Context.ItemLink.Single(il =>
                    il.childItemID == testItemId0 && il.localeID == testLocale.localeID),

                context.Context.ItemLink.Single(il =>
                    il.childItemID == testItemId1 && il.localeID == testLocale.localeID),

                context.Context.ItemLink.Single(il =>
                    il.childItemID == testItemId2 && il.localeID == testLocale.localeID)
            };

            int newLinkedItemId = testLinkedItems[1].itemID;
            bool updatedItemLinksMatch = updatedItemLinks.TrueForAll(il => il.parentItemID == newLinkedItemId);

            Assert.IsTrue(updatedItemLinksMatch);
        }
    }
}
