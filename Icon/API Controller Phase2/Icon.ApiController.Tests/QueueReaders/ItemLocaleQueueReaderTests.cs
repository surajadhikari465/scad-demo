using Icon.ApiController.Common;
using Icon.ApiController.Controller.Mappers;
using Icon.ApiController.Controller.QueueReaders;
using Icon.ApiController.DataAccess.Commands;
using Icon.ApiController.DataAccess.Queries;
using Icon.Common.DataAccess;
using Icon.Common.Email;
using Icon.Framework;
using Icon.Logging;
using Icon.Testing.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Contracts = Icon.Esb.Schemas.Wfm.Contracts;

namespace Icon.ApiController.Tests.QueueReaderTests
{
    [TestClass]
    public class ItemLocaleQueueReaderTests
    {
        private IconContext context;
        private ItemLocaleQueueReader queueReader;
        private Mock<ILogger<ItemLocaleQueueReader>> mockLogger;
        private Mock<IEmailClient> mockEmailClient;
        private Mock<IQueryHandler<GetMessageQueueParameters<MessageQueueItemLocale>, List<MessageQueueItemLocale>>> mockGetMessageQueueQuery;
        private Mock<IQueryHandler<GetItemByScanCodeParameters, Item>> mockGetItemByScanCodeQuery;
        private Mock<ICommandHandler<UpdateMessageQueueStatusCommand<MessageQueueItemLocale>>> mockUpdateMessageQueueStatusCommandHandler;
        private Mock<IProductSelectionGroupsMapper> mockProductSelectionGroupsMapper;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            mockLogger = new Mock<ILogger<ItemLocaleQueueReader>>();
            mockEmailClient = new Mock<IEmailClient>();
            mockGetMessageQueueQuery = new Mock<IQueryHandler<GetMessageQueueParameters<MessageQueueItemLocale>, List<MessageQueueItemLocale>>>();
            mockGetItemByScanCodeQuery = new Mock<IQueryHandler<GetItemByScanCodeParameters, Item>>();
            mockUpdateMessageQueueStatusCommandHandler = new Mock<ICommandHandler<UpdateMessageQueueStatusCommand<MessageQueueItemLocale>>>();
            mockProductSelectionGroupsMapper = new Mock<IProductSelectionGroupsMapper>();

            queueReader = new ItemLocaleQueueReader(
                mockLogger.Object,
                mockEmailClient.Object,
                mockGetMessageQueueQuery.Object,
                mockGetItemByScanCodeQuery.Object,
                mockUpdateMessageQueueStatusCommandHandler.Object,
                mockProductSelectionGroupsMapper.Object);
        }

        [TestCleanup]
        public void Cleanup()
        {
            Cache.scanCodeToItem.Clear();
        }

        [TestMethod]
        public void GroupItemLocaleMessages_InvalidArgument_ShouldThrowException()
        {
            // Given.
            int exceptionCount = 0;

            // When.
            var messages = new List<MessageQueueItemLocale>();

            try { queueReader.GroupMessagesForMiniBulk(messages); }
            catch (Exception) { exceptionCount++; }

            messages = null;

            try { queueReader.GroupMessagesForMiniBulk(messages); }
            catch (Exception) { exceptionCount++; }

            // Then.
            Assert.AreEqual(2, exceptionCount);
        }

        [TestMethod]
        public void GroupItemLocaleMessages_OneRetailSaleMessage_ShouldReturnOneMessageWithRetailSaleItemTypeForTheMiniBulk()
        {
            // Given.
            var fakeMessageQueueItemLocales = new List<MessageQueueItemLocale>
            {
                TestHelpers.GetFakeMessageQueueItemLocale(1, 100, ItemTypeCodes.RetailSale)
            };

            // When.
            var messages = queueReader.GroupMessagesForMiniBulk(fakeMessageQueueItemLocales);

            // Then.
            Assert.AreEqual(1, messages.Count);
            Assert.AreEqual(1, messages[0].ItemId);
            Assert.AreEqual(100, messages[0].LocaleId);
            Assert.AreEqual(ItemTypeCodes.RetailSale, messages[0].ItemTypeCode);
        }

        [TestMethod]
        public void GroupItemLocaleMessages_OneBottleReturnMessage_ShouldReturnOneMessageWithReturnItemTypeForTheMiniBulk()
        {
            // Given.
            var fakeMessageQueueItemLocales = new List<MessageQueueItemLocale>
            {
                TestHelpers.GetFakeMessageQueueItemLocale(1, 100, ItemTypeCodes.Return)
            };

            // When.
            var messages = queueReader.GroupMessagesForMiniBulk(fakeMessageQueueItemLocales);

            // Then.
            Assert.AreEqual(1, messages.Count);
            Assert.AreEqual(1, messages[0].ItemId);
            Assert.AreEqual(100, messages[0].LocaleId);
            Assert.AreEqual(ItemTypeCodes.Return, messages[0].ItemTypeCode);
        }

        [TestMethod]
        public void GroupItemLocaleMessages_TwoMessagesWithDifferentLocaleId_ShouldReturnOneMessageForTheMiniBulk()
        {
            // Given.
            var fakeMessageQueueItemLocales = new List<MessageQueueItemLocale>
            {
                TestHelpers.GetFakeMessageQueueItemLocale(1, 100, ItemTypeCodes.RetailSale),
                TestHelpers.GetFakeMessageQueueItemLocale(2, 101, ItemTypeCodes.RetailSale)
            };

            // When.
            var messages = queueReader.GroupMessagesForMiniBulk(fakeMessageQueueItemLocales);

            // Then.
            Assert.AreEqual(1, messages.Count);
            Assert.AreEqual(1, messages[0].ItemId);
            Assert.AreEqual(100, messages[0].LocaleId);
        }

        [TestMethod]
        public void GroupItemLocaleMessages_TwoMessagesWithSameItemId_ShouldReturnOneMessageForTheMiniBulk()
        {
            // Given.
            var fakeMessageQueueItemLocales = new List<MessageQueueItemLocale>
            {
                TestHelpers.GetFakeMessageQueueItemLocale(1, 100, ItemTypeCodes.RetailSale),
                TestHelpers.GetFakeMessageQueueItemLocale(1, 100, ItemTypeCodes.RetailSale)
            };

            // When.
            var messages = queueReader.GroupMessagesForMiniBulk(fakeMessageQueueItemLocales);

            // Then.
            Assert.AreEqual(1, messages.Count);
            Assert.AreEqual(1, messages[0].ItemId);
            Assert.AreEqual(100, messages[0].LocaleId);
        }

        [TestMethod]
        public void GroupItemLocaleMessages_DistinctFirstItemIdWithDuplicateSecondAndThirdItemId_ShouldReturnTwoMessagesForTheMiniBulk()
        {
            // Given.
            var fakeMessageQueueItemLocales = new List<MessageQueueItemLocale>
            {
                TestHelpers.GetFakeMessageQueueItemLocale(1, 100, ItemTypeCodes.RetailSale),
                TestHelpers.GetFakeMessageQueueItemLocale(2, 100, ItemTypeCodes.RetailSale),
                TestHelpers.GetFakeMessageQueueItemLocale(2, 100, ItemTypeCodes.RetailSale)
            };

            // When.
            var messages = queueReader.GroupMessagesForMiniBulk(fakeMessageQueueItemLocales);

            // Then.
            Assert.AreEqual(2, messages.Count);
        }

        [TestMethod]
        public void GroupItemLocaleMessages_TwoMessagesWithDifferentItemIdAndSameLocaleId_ShouldReturnTwoMessagesForTheMiniBulk()
        {
            // Given.
            var fakeMessageQueueItemLocales = new List<MessageQueueItemLocale>
            {
                TestHelpers.GetFakeMessageQueueItemLocale(1, 100, ItemTypeCodes.RetailSale),
                TestHelpers.GetFakeMessageQueueItemLocale(2, 100, ItemTypeCodes.RetailSale)
            };

            // When.
            var messages = queueReader.GroupMessagesForMiniBulk(fakeMessageQueueItemLocales);

            // Then.
            Assert.AreEqual(2, messages.Count);
            Assert.AreEqual(1, messages[0].ItemId);
            Assert.AreEqual(100, messages[0].LocaleId);
            Assert.AreEqual(2, messages[1].ItemId);
            Assert.AreEqual(100, messages[1].LocaleId);
        }

        [TestMethod]
        public void GroupItemLocaleMessages_TwoRetailSaleAndTwoReturnMessages_ShouldReturnTwoMessageWithItemTypeRetailSaleForTheMiniBulk()
        {
            // Given.
            var fakeMessageQueueItemLocales = new List<MessageQueueItemLocale>
            {
                TestHelpers.GetFakeMessageQueueItemLocale(1, 100, ItemTypeCodes.RetailSale),
                TestHelpers.GetFakeMessageQueueItemLocale(2, 100, ItemTypeCodes.RetailSale),
                TestHelpers.GetFakeMessageQueueItemLocale(3, 100, ItemTypeCodes.Return),
                TestHelpers.GetFakeMessageQueueItemLocale(4, 100, ItemTypeCodes.Return)
            };

            // When.
            var messages = queueReader.GroupMessagesForMiniBulk(fakeMessageQueueItemLocales);

            // Then.
            Assert.AreEqual(2, messages.Count);
            Assert.AreEqual(1, messages[0].ItemId);
            Assert.AreEqual(100, messages[0].LocaleId);
            Assert.AreEqual(ItemTypeCodes.RetailSale, messages[0].ItemTypeCode);
            Assert.AreEqual(2, messages[1].ItemId);
            Assert.AreEqual(100, messages[1].LocaleId);
            Assert.AreEqual(ItemTypeCodes.RetailSale, messages[1].ItemTypeCode);
        }

        [TestMethod]
        public void GroupItemLocaleMessages_TwoReturnAndTwoRetailSaleMessages_ShouldReturnTwoMessageWithItemTypeReturnSaleForTheMiniBulk()
        {
            // Given.
            var fakeMessageQueueItemLocales = new List<MessageQueueItemLocale>
            {
                TestHelpers.GetFakeMessageQueueItemLocale(1, 100, ItemTypeCodes.Return),
                TestHelpers.GetFakeMessageQueueItemLocale(2, 100, ItemTypeCodes.Return),
                TestHelpers.GetFakeMessageQueueItemLocale(3, 100, ItemTypeCodes.RetailSale),
                TestHelpers.GetFakeMessageQueueItemLocale(4, 100, ItemTypeCodes.RetailSale)
            };

            // When.
            var messages = queueReader.GroupMessagesForMiniBulk(fakeMessageQueueItemLocales);

            // Then.
            Assert.AreEqual(2, messages.Count);
            Assert.AreEqual(1, messages[0].ItemId);
            Assert.AreEqual(100, messages[0].LocaleId);
            Assert.AreEqual(ItemTypeCodes.Return, messages[0].ItemTypeCode);
            Assert.AreEqual(2, messages[1].ItemId);
            Assert.AreEqual(100, messages[1].LocaleId);
            Assert.AreEqual(ItemTypeCodes.Return, messages[1].ItemTypeCode);
        }

        [TestMethod]
        public void GroupItemLocaleMessages_OneItemWithLinkedItem_ShouldReturnOneMessageWithLinkedItem()
        {
            // Given.
            var fakeMessageQueueItemLocales = new List<MessageQueueItemLocale>
            {
                new TestItemLocaleMessageBuilder().WithLinkedItem("123")
            };

            // When.
            var messages = queueReader.GroupMessagesForMiniBulk(fakeMessageQueueItemLocales);

            // Then.
            Assert.AreEqual(fakeMessageQueueItemLocales[0].LinkedItemScanCode, messages[0].LinkedItemScanCode);
        }

        [TestMethod]
        public void GroupItemLocaleMessages_TwoItemsWithLinkedItem_ShouldReturnTwoMessagesWithLinkedItem()
        {
            // Given.
            var fakeMessageQueueItemLocales = new List<MessageQueueItemLocale>
            {
                new TestItemLocaleMessageBuilder().WithItemId(123).WithLinkedItem("123"),
                new TestItemLocaleMessageBuilder().WithItemId(456).WithLinkedItem("123")
            };

            // When.
            var messages = queueReader.GroupMessagesForMiniBulk(fakeMessageQueueItemLocales);

            // Then.
            Assert.AreEqual(fakeMessageQueueItemLocales[0].LinkedItemScanCode, messages[0].LinkedItemScanCode);
            Assert.AreEqual(fakeMessageQueueItemLocales[1].LinkedItemScanCode, messages[1].LinkedItemScanCode);
        }

        [TestMethod]
        public void GroupItemLocaleMessages_TwoItemsWithoutLinkedItems_ShouldReturnTwoMessagesWithoutLinkedItems()
        {
            // Given.
            var fakeMessageQueueItemLocales = new List<MessageQueueItemLocale>
            {
                new TestItemLocaleMessageBuilder().WithItemId(123),
                new TestItemLocaleMessageBuilder().WithItemId(456)
            };

            // When.
            var messages = queueReader.GroupMessagesForMiniBulk(fakeMessageQueueItemLocales);

            // Then.
            Assert.AreEqual(fakeMessageQueueItemLocales[0].LinkedItemScanCode, messages[0].LinkedItemScanCode);
            Assert.AreEqual(fakeMessageQueueItemLocales[1].LinkedItemScanCode, messages[1].LinkedItemScanCode);
        }

        [TestMethod]
        public void GroupItemLocaleMessages_OneItemWithLinkedItemAndOneWithout_ShouldReturnOneMessageWithLinkedItem()
        {
            // Given.
            var fakeMessageQueueItemLocales = new List<MessageQueueItemLocale>
            {
                new TestItemLocaleMessageBuilder().WithItemId(123).WithLinkedItem("123"),
                new TestItemLocaleMessageBuilder().WithItemId(456)
            };

            // When.
            var messages = queueReader.GroupMessagesForMiniBulk(fakeMessageQueueItemLocales);

            // Then.
            Assert.AreEqual(1, messages.Count);
            Assert.AreEqual(fakeMessageQueueItemLocales[0].LinkedItemScanCode, messages[0].LinkedItemScanCode);
        }

        [TestMethod]
        public void GroupItemLocaleMessages_OneItemWithoutLinkedItemAndOneWith_ShouldReturnOneMessageWithoutLinkedItem()
        {
            // Given.
            var fakeMessageQueueItemLocales = new List<MessageQueueItemLocale>
            {
                new TestItemLocaleMessageBuilder().WithItemId(123),
                new TestItemLocaleMessageBuilder().WithItemId(456).WithLinkedItem("123")
            };

            // When.
            var messages = queueReader.GroupMessagesForMiniBulk(fakeMessageQueueItemLocales);

            // Then.
            Assert.AreEqual(1, messages.Count);
            Assert.AreEqual(fakeMessageQueueItemLocales[0].LinkedItemScanCode, messages[0].LinkedItemScanCode);
        }

        [TestMethod]
        public void BuildMiniBulk_InvalidArguments_ExceptionShouldBeThrown()
        {
            // Given.
            var messages = new List<MessageQueueItemLocale>();

            int caughtExceptions = 0;

            // When.
            try { var miniBulk = queueReader.BuildMiniBulk(messages); }
            catch (ArgumentException) { caughtExceptions++; }

            messages = null;
            try { var miniBulk = queueReader.BuildMiniBulk(messages); }
            catch (ArgumentException) { caughtExceptions++; }

            // Then.
            int expectedExceptions = 2;
            Assert.AreEqual(expectedExceptions, caughtExceptions);
        }

        [TestMethod]
        public void BuildMiniBulk_ThreeMessages_ShouldReturnMiniBulkWithThreeEntries()
        {
            // Given.
            var fakeMessageQueueItemLocales = new List<MessageQueueItemLocale>
            {
                TestHelpers.GetFakeMessageQueueItemLocale(1, 100, ItemTypeCodes.RetailSale),
                TestHelpers.GetFakeMessageQueueItemLocale(2, 100, ItemTypeCodes.RetailSale),
                TestHelpers.GetFakeMessageQueueItemLocale(3, 100, ItemTypeCodes.RetailSale)
            };

            // When.
            var miniBulk = queueReader.BuildMiniBulk(fakeMessageQueueItemLocales);

            // Then.
            Assert.AreEqual(3, miniBulk.item.Length);
            Assert.AreEqual(1, miniBulk.item[0].id);
            Assert.AreEqual(2, miniBulk.item[1].id);
            Assert.AreEqual(3, miniBulk.item[2].id);
        }

        [TestMethod]
        public void BuildMiniBulk_ValidLinkedItemExists_GroupIdShouldBeLinkedItemIdAndRetailItemIdSeparatedByUnderscore()
        {
            // Given.
            var fakeMessageQueueItemLocale = new List<MessageQueueItemLocale>
            {
                new TestItemLocaleMessageBuilder().WithLinkedItem("1234")
            };

            Item linkedItem = new Item
            {
                ScanCode = new ScanCode[]
                {
                    new ScanCode { scanCode = "1112223334445" }
                },
                ItemType = context.ItemType.Single(it => it.itemTypeCode == ItemTypeCodes.Deposit)
            };

            mockGetItemByScanCodeQuery.Setup(handler => handler.Search(It.IsAny<GetItemByScanCodeParameters>())).Returns(linkedItem);

            // When.
            var miniBulk = queueReader.BuildMiniBulk(fakeMessageQueueItemLocale);

            // Then.
            Assert.AreEqual(1, miniBulk.item.Length);

            var itemAttributes = miniBulk.item[0].locale[0].Item as Contracts.StoreItemAttributesType;
            int retailItemId = fakeMessageQueueItemLocale[0].ItemId;
            int linkedItemId = linkedItem.ItemId;

            Assert.AreEqual(1, itemAttributes.groups.group.Length);
            Assert.AreEqual(linkedItemId.ToString() + "_" + retailItemId.ToString(), itemAttributes.groups.group[0].id);
        }

        [TestMethod]
        public void BuildMiniBulk_DepositLinkedItemExists_ShouldReturnMiniBulkWithDepositGroupType()
        {
            // Given.
            var fakeMessageQueueItemLocale = new List<MessageQueueItemLocale>
            {
                new TestItemLocaleMessageBuilder().WithLinkedItem("1234")
            };

            Item linkedItem = new Item
            {
                ScanCode = new ScanCode[]
                {
                    new ScanCode { scanCode = "1112223334445" }
                },
                ItemType = context.ItemType.Single(it => it.itemTypeCode == ItemTypeCodes.Deposit)
            };

            mockGetItemByScanCodeQuery.Setup(handler => handler.Search(It.IsAny<GetItemByScanCodeParameters>())).Returns(linkedItem);

            // When.
            var miniBulk = queueReader.BuildMiniBulk(fakeMessageQueueItemLocale);

            // Then.
            Assert.AreEqual(fakeMessageQueueItemLocale.Count, miniBulk.item.Length);

            var itemAttributes = miniBulk.item[0].locale[0].Item as Contracts.StoreItemAttributesType;
            int retailItemId = fakeMessageQueueItemLocale[0].ItemId;
            int linkedItemId = linkedItem.ItemId;

            Assert.AreEqual(1, itemAttributes.links.Length);
            Assert.AreEqual(linkedItem.ItemId, itemAttributes.links[0].parentId);
            Assert.AreEqual(1, itemAttributes.links[0].childId);
            Assert.IsTrue(itemAttributes.links[0].childIdSpecified);
            Assert.IsTrue(itemAttributes.links[0].parentIdSpecified);

            Assert.AreEqual(1, itemAttributes.groups.group.Length);
            Assert.AreEqual(linkedItemId.ToString() + "_" + retailItemId.ToString(), itemAttributes.groups.group[0].id);
            Assert.AreEqual(Contracts.RetailTransactionItemTypeEnum.Deposit.ToString(), itemAttributes.groups.group[0].description);
        }

        [TestMethod]
        public void BuildMiniBulk_FeeLinkedItemExists_ShouldReturnMiniBulkWithWarrantyGroupType()
        {
            // Given.
            var fakeMessageQueueItemLocale = new List<MessageQueueItemLocale>
            {
                new TestItemLocaleMessageBuilder().WithLinkedItem("1234")
            };

            Item linkedItem = new Item
            {
                ScanCode = new ScanCode[]
                {
                    new ScanCode { scanCode = "1112223334445" }
                },
                ItemType = context.ItemType.Single(it => it.itemTypeCode == ItemTypeCodes.Fee)
            };

            mockGetItemByScanCodeQuery.Setup(handler => handler.Search(It.IsAny<GetItemByScanCodeParameters>())).Returns(linkedItem);

            // When.
            var miniBulk = queueReader.BuildMiniBulk(fakeMessageQueueItemLocale);

            // Then.
            Assert.AreEqual(fakeMessageQueueItemLocale.Count, miniBulk.item.Length);

            var itemAttributes = miniBulk.item[0].locale[0].Item as Contracts.StoreItemAttributesType;
            int retailItemId = fakeMessageQueueItemLocale[0].ItemId;
            int linkedItemId = linkedItem.ItemId;

            Assert.AreEqual(1, itemAttributes.links.Length);
            Assert.AreEqual(linkedItem.ItemId, itemAttributes.links[0].parentId);
            Assert.AreEqual(1, itemAttributes.links[0].childId);
            Assert.IsTrue(itemAttributes.links[0].childIdSpecified);
            Assert.IsTrue(itemAttributes.links[0].parentIdSpecified);

            Assert.AreEqual(1, itemAttributes.groups.group.Length);
            Assert.AreEqual(linkedItemId.ToString() + "_" + retailItemId.ToString(), itemAttributes.groups.group[0].id);
            Assert.AreEqual(Contracts.RetailTransactionItemTypeEnum.Warranty.ToString(), itemAttributes.groups.group[0].description);
        }

        [TestMethod]
        public void BuildMiniBulk_LinkedItemIsRetailSale_ShouldReturnMiniBulkWithNullItemLinksAndGroups()
        {
            // Given.
            var messageQueueItemLocaleList = new List<MessageQueueItemLocale>
            {
                new TestItemLocaleMessageBuilder().WithLinkedItem("1234")
            };

            Item linkedItem = new Item
            {
                ScanCode = new ScanCode[]
                {
                    new ScanCode { scanCode = "1112223334445" }
                },
                ItemType = context.ItemType.Single(it => it.itemTypeCode == ItemTypeCodes.RetailSale)
            };

            mockGetItemByScanCodeQuery.Setup(handler => handler.Search(It.IsAny<GetItemByScanCodeParameters>())).Returns(linkedItem);

            // When.
            var miniBulk = queueReader.BuildMiniBulk(messageQueueItemLocaleList);

            // Then.
            var itemAttributes = miniBulk.item[0].locale[0].Item as Contracts.StoreItemAttributesType;

            Assert.IsNull(itemAttributes.links);
            Assert.IsNull(itemAttributes.groups);
        }

        [TestMethod]
        public void BuildMiniBulk_PreviousLinkedItemScanCodeExists_ShouldAddDeleteGroupForItemMessage()
        {
            // Given.
            var linkedItemScanCode = "1112223334445";
            var previousLinkedItemScanCode = "12345";
            var fakeMessageQueueItemLocale = new List<MessageQueueItemLocale>
            {
                new TestItemLocaleMessageBuilder().WithLinkedItem("1112223334445").WithPreviousLinkedItem("12345")
            };

            Item linkedItem = new Item
            {
                ScanCode = new ScanCode[]
                {
                    new ScanCode { scanCode = linkedItemScanCode }
                },
                ItemType = context.ItemType.Single(it => it.itemTypeCode == ItemTypeCodes.Fee)
            };

            Item previousLinkedItem = new Item
            {
                ScanCode = new ScanCode[]
                {
                    new ScanCode { scanCode = previousLinkedItemScanCode }
                },
                ItemType = context.ItemType.Single(it => it.itemTypeCode == ItemTypeCodes.Fee)
            };

            mockGetItemByScanCodeQuery.Setup(handler => handler.Search(It.Is<GetItemByScanCodeParameters>(p => p.ScanCode == linkedItemScanCode))).Returns(linkedItem);
            mockGetItemByScanCodeQuery.Setup(handler => handler.Search(It.Is<GetItemByScanCodeParameters>(p => p.ScanCode == previousLinkedItemScanCode))).Returns(previousLinkedItem);

            // When.
            var miniBulk = queueReader.BuildMiniBulk(fakeMessageQueueItemLocale);

            // Then.
            Assert.AreEqual(fakeMessageQueueItemLocale.Count, miniBulk.item.Length);

            var itemAttributes = miniBulk.item[0].locale[0].Item as Contracts.StoreItemAttributesType;
            int retailItemId = fakeMessageQueueItemLocale[0].ItemId;
            int linkedItemId = linkedItem.ItemId;

            Assert.AreEqual(2, itemAttributes.links.Length);
            Assert.AreEqual(linkedItemId, itemAttributes.links[0].parentId);
            Assert.AreEqual(1, itemAttributes.links[0].childId);
            Assert.IsTrue(itemAttributes.links[0].childIdSpecified);
            Assert.IsTrue(itemAttributes.links[0].parentIdSpecified);

            Assert.AreEqual(previousLinkedItem.ItemId, itemAttributes.links[1].parentId);
            Assert.AreEqual(1, itemAttributes.links[1].childId);
            Assert.IsTrue(itemAttributes.links[1].childIdSpecified);
            Assert.IsTrue(itemAttributes.links[1].parentIdSpecified);

            Assert.AreEqual(2, itemAttributes.groups.group.Length);
            Assert.AreEqual(linkedItemId.ToString() + "_" + retailItemId.ToString(), itemAttributes.groups.group[0].id);
            Assert.AreEqual(Contracts.RetailTransactionItemTypeEnum.Warranty.ToString(), itemAttributes.groups.group[0].description);
            Assert.AreEqual(Contracts.ActionEnum.AddOrUpdate, itemAttributes.groups.group[0].Action);
            Assert.IsTrue(itemAttributes.groups.group[0].ActionSpecified);

            Assert.AreEqual(previousLinkedItem.ItemId.ToString() + "_" + retailItemId.ToString(), itemAttributes.groups.group[1].id);
            Assert.AreEqual(Contracts.RetailTransactionItemTypeEnum.Warranty.ToString(), itemAttributes.groups.group[1].description);
            Assert.AreEqual(Contracts.ActionEnum.Delete, itemAttributes.groups.group[1].Action);
            Assert.IsTrue(itemAttributes.groups.group[1].ActionSpecified);
        }

        [TestMethod]
        public void BuildMiniBulk_NoPreviousLinkedItemScanCodeExists_LinksAndGroupsShouldNotBeIncluded()
        {
            // Given.
            var fakeMessageQueueItemLocale = new List<MessageQueueItemLocale>
            {
                new TestItemLocaleMessageBuilder()
            };

            // When.
            var miniBulk = queueReader.BuildMiniBulk(fakeMessageQueueItemLocale);

            // Then.
            Assert.AreEqual(fakeMessageQueueItemLocale.Count, miniBulk.item.Length);

            var itemAttributes = miniBulk.item[0].locale[0].Item as Contracts.StoreItemAttributesType;

            Assert.IsNull(itemAttributes.links);
            Assert.IsNull(itemAttributes.groups);
        }

        [TestMethod]
        public void BuildMiniBulk_LinkedScanCodeExistsButNotPreviousLinkedItemScanCode_LinksAndGroupsShouldBePresentWithAddOrUpdateAction()
        {
            // Given.
            var linkedItemScanCode = "1112223334445";

            var fakeMessageQueueItemLocale = new List<MessageQueueItemLocale>
            {
                new TestItemLocaleMessageBuilder().WithLinkedItem("1112223334445")
            };

            Item linkedItem = new Item
            {
                ScanCode = new ScanCode[]
                {
                    new ScanCode { scanCode = linkedItemScanCode }
                },
                ItemType = context.ItemType.Single(it => it.itemTypeCode == ItemTypeCodes.Fee)
            };

            mockGetItemByScanCodeQuery.Setup(handler => handler.Search(It.Is<GetItemByScanCodeParameters>(p => p.ScanCode == linkedItemScanCode))).Returns(linkedItem);

            // When.
            var miniBulk = queueReader.BuildMiniBulk(fakeMessageQueueItemLocale);

            // Then.
            Assert.AreEqual(fakeMessageQueueItemLocale.Count, miniBulk.item.Length);

            var itemAttributes = miniBulk.item[0].locale[0].Item as Contracts.StoreItemAttributesType;
            int retailItemId = fakeMessageQueueItemLocale[0].ItemId;
            int linkedItemId = linkedItem.ItemId;

            Assert.AreEqual(1, itemAttributes.links.Length);
            Assert.AreEqual(linkedItemId, itemAttributes.links[0].parentId);
            Assert.AreEqual(1, itemAttributes.links[0].childId);
            Assert.IsTrue(itemAttributes.links[0].childIdSpecified);
            Assert.IsTrue(itemAttributes.links[0].parentIdSpecified);

            Assert.AreEqual(1, itemAttributes.groups.group.Length);
            Assert.AreEqual(linkedItemId.ToString() + "_" + retailItemId.ToString(), itemAttributes.groups.group[0].id);
            Assert.AreEqual(Contracts.RetailTransactionItemTypeEnum.Warranty.ToString(), itemAttributes.groups.group[0].description);
            Assert.AreEqual(Contracts.ActionEnum.AddOrUpdate, itemAttributes.groups.group[0].Action);
            Assert.IsTrue(itemAttributes.groups.group[0].ActionSpecified);
        }

        [TestMethod]
        public void BuildMiniBulk_PreviousLinkedItemScanCodeExistsButNotLinkedScanCode_ShouldAddDeleteGroupForItemMessage()
        {
            // Given.
            var previousLinkedItemScanCode = "12345";

            var fakeMessageQueueItemLocale = new List<MessageQueueItemLocale>
            {
                new TestItemLocaleMessageBuilder().WithPreviousLinkedItem("12345")
            };

            Item previousLinkedItem = new Item
            {
                ScanCode = new ScanCode[]
                {
                    new ScanCode { scanCode = previousLinkedItemScanCode }
                },
                ItemType = context.ItemType.Single(it => it.itemTypeCode == ItemTypeCodes.Fee)
            };

            mockGetItemByScanCodeQuery.Setup(handler => handler.Search(It.Is<GetItemByScanCodeParameters>(p => p.ScanCode == previousLinkedItemScanCode))).Returns(previousLinkedItem);

            // When.
            var miniBulk = queueReader.BuildMiniBulk(fakeMessageQueueItemLocale);

            // Then.
            Assert.AreEqual(fakeMessageQueueItemLocale.Count, miniBulk.item.Length);

            var itemAttributes = miniBulk.item[0].locale[0].Item as Contracts.StoreItemAttributesType;
            int retailItemId = fakeMessageQueueItemLocale[0].ItemId;

            Assert.AreEqual(previousLinkedItem.ItemId, itemAttributes.links[0].parentId);
            Assert.AreEqual(1, itemAttributes.links[0].childId);
            Assert.IsTrue(itemAttributes.links[0].childIdSpecified);
            Assert.IsTrue(itemAttributes.links[0].parentIdSpecified);

            Assert.AreEqual(previousLinkedItem.ItemId.ToString() + "_" + retailItemId.ToString(), itemAttributes.groups.group[0].id);
            Assert.AreEqual(Contracts.RetailTransactionItemTypeEnum.Warranty.ToString(), itemAttributes.groups.group[0].description);
            Assert.AreEqual(Contracts.ActionEnum.Delete, itemAttributes.groups.group[0].Action);
            Assert.IsTrue(itemAttributes.groups.group[0].ActionSpecified);
        }

        [TestMethod]
        public void BuildMiniBulk_PreviousLinkedItemScanCodeExistsAndIsNotAFeeOrDeposit_ShouldAddDeleteGroupForItemMessageWithDepositGroupType()
        {
            // Given.
            var previousLinkedItemScanCode = "12345";

            var fakeMessageQueueItemLocale = new List<MessageQueueItemLocale>
            {
                new TestItemLocaleMessageBuilder().WithPreviousLinkedItem("12345")
            };

            Item previousLinkedItem = new Item
            {
                ScanCode = new ScanCode[]
                {
                    new ScanCode { scanCode = previousLinkedItemScanCode }
                },
                ItemType = context.ItemType.Single(it => it.itemTypeCode == ItemTypeCodes.RetailSale)
            };

            mockGetItemByScanCodeQuery.Setup(handler => handler.Search(It.Is<GetItemByScanCodeParameters>(p => p.ScanCode == previousLinkedItemScanCode))).Returns(previousLinkedItem);

            // When.
            var miniBulk = queueReader.BuildMiniBulk(fakeMessageQueueItemLocale);

            // Then.
            Assert.AreEqual(fakeMessageQueueItemLocale.Count, miniBulk.item.Length);

            var itemAttributes = miniBulk.item[0].locale[0].Item as Contracts.StoreItemAttributesType;
            int retailItemId = fakeMessageQueueItemLocale[0].ItemId;

            Assert.AreEqual(previousLinkedItem.ItemId, itemAttributes.links[0].parentId);
            Assert.AreEqual(1, itemAttributes.links[0].childId);
            Assert.AreEqual(1, itemAttributes.links.Length);
            Assert.IsTrue(itemAttributes.links[0].childIdSpecified);
            Assert.IsTrue(itemAttributes.links[0].parentIdSpecified);

            Assert.AreEqual(1, itemAttributes.groups.group.Length);
            Assert.AreEqual(previousLinkedItem.ItemId.ToString() + "_" + retailItemId.ToString(), itemAttributes.groups.group[0].id);
            Assert.AreEqual(Contracts.RetailTransactionItemTypeEnum.Deposit.ToString(), itemAttributes.groups.group[0].description);
            Assert.AreEqual(Contracts.ActionEnum.Delete, itemAttributes.groups.group[0].Action);
            Assert.IsTrue(itemAttributes.groups.group[0].ActionSpecified);
        }

        [TestMethod]
        public void BuildMiniBulk_ItemLocaleMessage_ShouldContainPosScaleTareElement()
        {
            // Given.
            var fakeMessage = TestHelpers.GetFakeMessageQueueItemLocale(1, 100, ItemTypeCodes.RetailSale);

            var fakeMessageQueueItemLocales = new List<MessageQueueItemLocale>
            {
                fakeMessage
            };

            // When.
            var miniBulk = queueReader.BuildMiniBulk(fakeMessageQueueItemLocales);

            // Then.
            var scaleTare = (miniBulk.item[0].locale[0].Item as Contracts.StoreItemAttributesType).traits.Single(it => it.code == TraitCodes.PosScaleTare).type.value[0].value;
            Assert.AreEqual((fakeMessage.PosScaleTare * 0.01).ToString(), scaleTare);
        }

        [TestMethod]
        public void BuildMiniBulk_IntegerPosScaleTare_PosScaleTareShouldBeConvertedToDecimal()
        {
            // Given.
            var fakeMessage = TestHelpers.GetFakeMessageQueueItemLocale(1, 100, ItemTypeCodes.RetailSale);

            var fakeMessageQueueItemLocales = new List<MessageQueueItemLocale>
            {
                fakeMessage
            };

            // When.
            var miniBulk = queueReader.BuildMiniBulk(fakeMessageQueueItemLocales);

            // Then.
            var scaleTare = (miniBulk.item[0].locale[0].Item as Contracts.StoreItemAttributesType).traits.Single(it => it.code == TraitCodes.PosScaleTare).type.value[0].value;
            Assert.AreEqual((fakeMessage.PosScaleTare * .01).ToString(), scaleTare);
        }

        [TestMethod]
        public void BuildMiniBulk_ItemLocaleMessage_LocaleElementShouldContainActionAttribute()
        {
            // Given.
            var fakeMessage = TestHelpers.GetFakeMessageQueueItemLocale(1, 100, ItemTypeCodes.RetailSale);

            var fakeMessageQueueItemLocales = new List<MessageQueueItemLocale>
            {
                fakeMessage
            };

            // When.
            var miniBulk = queueReader.BuildMiniBulk(fakeMessageQueueItemLocales);

            // Then.
            var localeElement = miniBulk.item[0].locale[0];

            Assert.AreEqual("AddOrUpdate", localeElement.Action.ToString());
            Assert.IsTrue(localeElement.ActionSpecified);
        }

        [TestMethod]
        public void BuildMiniBulk_ItemLocaleMessage_ItemLocaleTraitsShouldContainActionAttribute()
        {
            // Given.
            var fakeMessage = TestHelpers.GetFakeMessageQueueItemLocale(1, 100, ItemTypeCodes.RetailSale);

            var fakeMessageQueueItemLocales = new List<MessageQueueItemLocale>
            {
                fakeMessage
            };

            // When.
            var miniBulk = queueReader.BuildMiniBulk(fakeMessageQueueItemLocales);

            // Then.
            var traits = (miniBulk.item[0].locale[0].Item as Contracts.StoreItemAttributesType).traits;
            var lockedForSaleTrait = traits.First(t => t.code == TraitCodes.LockedForSale);
            var priceRequiredTrait = traits.First(t => t.code == TraitCodes.PriceRequired);
            var visualVerifyTrait = traits.First(t => t.code == TraitCodes.VisualVerify);
            var quantityRequiredTrait = traits.First(t => t.code == TraitCodes.QuantityRequired);
            var quantityProhibitTrait = traits.First(t => t.code == TraitCodes.QuantityProhibit);

            Assert.AreEqual(Contracts.ActionEnum.Delete, lockedForSaleTrait.Action);
            Assert.IsTrue(lockedForSaleTrait.ActionSpecified);
            Assert.AreEqual(Contracts.ActionEnum.AddOrUpdate, priceRequiredTrait.Action);
            Assert.IsTrue(priceRequiredTrait.ActionSpecified);
            Assert.AreEqual(Contracts.ActionEnum.Delete, visualVerifyTrait.Action);
            Assert.IsTrue(visualVerifyTrait.ActionSpecified);
            Assert.AreEqual(Contracts.ActionEnum.Delete, quantityRequiredTrait.Action);
            Assert.IsTrue(quantityRequiredTrait.ActionSpecified);
            Assert.AreEqual(Contracts.ActionEnum.Delete, quantityProhibitTrait.Action);
            Assert.IsTrue(quantityProhibitTrait.ActionSpecified);
        }
    }
}
