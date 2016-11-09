using Icon.Common.Email;
using Icon.Framework;
using Icon.Framework.RenewableContext;
using Icon.Logging;
using Icon.Testing.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PushController.Common;
using PushController.Controller.UdmUpdateServices;
using PushController.DataAccess.Commands;
using PushController.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace PushController.Tests.Controller.UdmUpdateServiceTests
{
    [TestClass]
    public class ItemLinkUpdateServiceTests
    {
        private ItemLinkUpdateService itemLinkUpdateService;
        private GlobalIconContext context;
        private DbContextTransaction transaction;
        private Mock<ILogger<ItemLinkUpdateService>> mockLogger;
        private Mock<IEmailClient> mockEmailClient;
        private AddOrUpdateItemLinkBulkCommandHandler addOrUpdateEntitiesBulkCommandHandler;
        private Mock<ICommandHandler<AddOrUpdateItemLinkBulkCommand>> mockAddOrUpdateEntitiesBulkCommandHandler;
        private AddOrUpdateItemLinkRowByRowCommandHandler addOrUpdateEntitiesRowByRowCommandHandler;
        private UpdateStagingTableDatesForUdmCommandHandler updateStagingTableDatesForUdmCommandHandler;
        private IRMAPush testPosData;
        private List<Item> testItems;
        private List<Item> testLinkedItems;
        private Locale testLocale;
        private ItemLink testItemLink;
        private string testScanCode;
        private string testLinkedScanCode;
        private int testBusinessUnitId;
        private int unknownLocaleId;
        
        [TestInitialize]
        public void Initialize()
        {
            this.testScanCode = "2222222";
            this.testLinkedScanCode = "8888888333";
            this.testBusinessUnitId = 88888;
            this.unknownLocaleId = 99999;

            this.context = new GlobalIconContext(new IconContext());

            this.mockLogger = new Mock<ILogger<ItemLinkUpdateService>>();
            this.mockEmailClient = new Mock<IEmailClient>();
            this.addOrUpdateEntitiesBulkCommandHandler = new AddOrUpdateItemLinkBulkCommandHandler(new Mock<ILogger<AddOrUpdateItemLinkBulkCommandHandler>>().Object, context);
            this.mockAddOrUpdateEntitiesBulkCommandHandler = new Mock<ICommandHandler<AddOrUpdateItemLinkBulkCommand>>();
            this.addOrUpdateEntitiesRowByRowCommandHandler = new AddOrUpdateItemLinkRowByRowCommandHandler(context);
            this.updateStagingTableDatesForUdmCommandHandler = new UpdateStagingTableDatesForUdmCommandHandler(new Mock<ILogger<UpdateStagingTableDatesForUdmCommandHandler>>().Object, context);

            this.transaction = this.context.Context.Database.BeginTransaction();

            StageTestPosData();
            StageTestItems();
            StageTestLinkedItems();
            StageTestLocale();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
        }

        private void StageTestPosData()
        {
            this.testPosData = new TestIrmaPushBuilder();
            this.context.Context.IRMAPush.Add(this.testPosData);
            this.context.Context.SaveChanges();
        }

        private void StageTestItems()
        {
            this.testItems = new List<Item>
            {
                new TestItemBuilder().WithScanCode(this.testScanCode),
                new TestItemBuilder().WithScanCode(this.testScanCode+"0"),
                new TestItemBuilder().WithScanCode(this.testScanCode+"1")
            };

            var itemType = this.context.Context.ItemType.Single(it => it.itemTypeCode == ItemTypeCodes.RetailSale);
            var scanCodeType = this.context.Context.ScanCodeType.Single(sct => sct.scanCodeTypeID == ScanCodeTypes.Upc);

            foreach (var item in this.testItems)
            {
                item.ItemType = itemType;
                item.ScanCode.Single().ScanCodeType = scanCodeType;
            }

            this.context.Context.Item.AddRange(this.testItems);
            this.context.Context.SaveChanges();
        }

        private void StageTestLinkedItems()
        {
            this.testLinkedItems = new List<Item>
            {
                new TestItemBuilder().WithScanCode(this.testLinkedScanCode),
                new TestItemBuilder().WithScanCode(this.testLinkedScanCode+"0"),
                new TestItemBuilder().WithScanCode(this.testLinkedScanCode+"1")
            };

            var itemType = this.context.Context.ItemType.Single(it => it.itemTypeCode == ItemTypeCodes.Deposit);
            var scanCodeType = this.context.Context.ScanCodeType.Single(sct => sct.scanCodeTypeID == ScanCodeTypes.PosPlu);

            foreach (var item in this.testLinkedItems)
            {
                item.ItemType = itemType;
                item.ScanCode.Single().ScanCodeType = scanCodeType;
            }

            this.context.Context.Item.AddRange(this.testLinkedItems);
            this.context.Context.SaveChanges();
        }

        private void StageTestLocale()
        {
            this.testLocale = new TestLocaleBuilder().WithBusinessUnitId(this.testBusinessUnitId);
            this.context.Context.Locale.Add(this.testLocale);
            this.context.Context.SaveChanges();
        }

        private void StageExistingItemLink()
        {
            this.testItemLink = new TestItemLinkBuilder()
                .WithLocaleId(this.testLocale.localeID)
                .WithParentItemId(this.testLinkedItems[0].itemID)
                .WithChildItemId(this.testItems[0].itemID);

            this.context.Context.ItemLink.Add(testItemLink);
            this.context.Context.SaveChanges();
        }

        [TestMethod]
        public void SaveEntitiesBulk_AddNewItemLink_EntitiesShouldBeSavedWithCorrectValues()
        {
            // Given.
            var testEntities = new List<ItemLink>
            {
                new TestItemLinkBuilder()
                    .WithParentItemId(this.testLinkedItems[0].itemID)
                    .WithChildItemId(this.testItems[0].itemID)
                    .WithLocaleId(this.testLocale.localeID)
            }.ConvertAll(e => new ItemLinkModel
            {
                IrmaPushId = this.testPosData.IRMAPushID,
                ParentItemId = e.parentItemID,
                ChildItemId = e.childItemID,
                LocaleId = e.localeID
            });

            this.itemLinkUpdateService = new ItemLinkUpdateService(
                mockLogger.Object,
                mockEmailClient.Object,
                addOrUpdateEntitiesBulkCommandHandler,
                addOrUpdateEntitiesRowByRowCommandHandler,
                updateStagingTableDatesForUdmCommandHandler);

            // When.
            this.itemLinkUpdateService.SaveEntitiesBulk(testEntities);

            // Then.
            var testParentItemId = this.testLinkedItems[0].itemID;
            var testChildItemId = this.testItems[0].itemID;

            var newItemLinkEntity = context.Context.ItemLink.Single(il =>
                il.localeID == this.testLocale.localeID && il.parentItemID == testParentItemId && il.childItemID == testChildItemId);

            Assert.AreEqual(testEntities[0].ParentItemId, newItemLinkEntity.parentItemID);
            Assert.AreEqual(testEntities[0].ChildItemId, newItemLinkEntity.childItemID);
            Assert.AreEqual(testEntities[0].LocaleId, newItemLinkEntity.localeID);
        }

        [TestMethod]
        public void SaveEntitiesBulk_UpdateExistingItemLink_EntitiesShouldBeUpdatedWithNewItemLinkInformation()
        {
            // Given.
            StageExistingItemLink();

            var testEntities = new List<ItemLink>
            {
                new TestItemLinkBuilder()
                    .WithParentItemId(this.testLinkedItems[1].itemID)
                    .WithChildItemId(this.testItems[0].itemID)
                    .WithLocaleId(this.testLocale.localeID)
            }.ConvertAll(e => new ItemLinkModel
            {
                IrmaPushId = this.testPosData.IRMAPushID,
                ParentItemId = e.parentItemID,
                ChildItemId = e.childItemID,
                LocaleId = e.localeID
            });

            this.itemLinkUpdateService = new ItemLinkUpdateService(
                mockLogger.Object,
                mockEmailClient.Object,
                addOrUpdateEntitiesBulkCommandHandler,
                addOrUpdateEntitiesRowByRowCommandHandler,
                updateStagingTableDatesForUdmCommandHandler);

            // When.
            this.itemLinkUpdateService.SaveEntitiesBulk(testEntities);

            // Then.
            var testChildItemId = this.testItems[0].itemID;

            var updatedItemLinkEntity = context.Context.ItemLink.Single(il =>
                il.localeID == this.testLocale.localeID && il.childItemID == testChildItemId);

            // Have to reload the entity since the price update was done via stored procedure.
            this.context.Context.Entry(updatedItemLinkEntity).Reload();

            Assert.AreEqual(testEntities[0].ParentItemId, updatedItemLinkEntity.parentItemID);
            Assert.AreEqual(testEntities[0].ChildItemId, updatedItemLinkEntity.childItemID);
            Assert.AreEqual(testEntities[0].LocaleId, updatedItemLinkEntity.localeID);
        }

        [TestMethod]
        public void SaveEntitiesBulk_TransientErrorOccursAndRetryIsSuccessful_EntitiesShouldBeUpdated()
        {
            // Given.
            var testEntities = new List<ItemLink>
                {
                    new TestItemLinkBuilder()
                        .WithParentItemId(this.testLinkedItems[0].itemID)
                        .WithChildItemId(this.testItems[0].itemID)
                        .WithLocaleId(this.testLocale.localeID)
                }.ConvertAll(e => new ItemLinkModel
                {
                    IrmaPushId = this.testPosData.IRMAPushID,
                    ParentItemId = e.parentItemID,
                    ChildItemId = e.childItemID,
                    LocaleId = e.localeID
                });

            bool firstTimeExecuted = true;

            mockAddOrUpdateEntitiesBulkCommandHandler.Setup(c => c.Execute(It.IsAny<AddOrUpdateItemLinkBulkCommand>()))
                .Callback(() =>
                    {
                        if (firstTimeExecuted)
                        {
                            firstTimeExecuted = false;
                            throw new TimeoutException();
                        }
                    });

            this.itemLinkUpdateService = new ItemLinkUpdateService(
                mockLogger.Object,
                mockEmailClient.Object,
                mockAddOrUpdateEntitiesBulkCommandHandler.Object,
                addOrUpdateEntitiesRowByRowCommandHandler,
                updateStagingTableDatesForUdmCommandHandler);

            // When.
            this.itemLinkUpdateService.SaveEntitiesBulk(testEntities);

            // Then.
            mockLogger.Verify(l => l.Error(It.IsAny<string>()), Times.Once);
            mockLogger.Verify(l => l.Info(It.IsAny<string>()), Times.Once);
            mockAddOrUpdateEntitiesBulkCommandHandler.Verify(c => c.Execute(It.IsAny<AddOrUpdateItemLinkBulkCommand>()), Times.Exactly(2));
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void SaveEntitiesBulk_IntransientErrorOccurs_ExceptionShouldBeThrown()
        {
            // Given.
            var testEntities = new List<ItemLink>
                {
                    new TestItemLinkBuilder()
                        .WithParentItemId(this.testLinkedItems[0].itemID)
                        .WithChildItemId(this.testItems[0].itemID)
                        .WithLocaleId(this.testLocale.localeID)
                }.ConvertAll(e => new ItemLinkModel
                {
                    IrmaPushId = this.testPosData.IRMAPushID,
                    ParentItemId = e.parentItemID,
                    ChildItemId = e.childItemID,
                    LocaleId = e.localeID
                });

            this.mockAddOrUpdateEntitiesBulkCommandHandler.Setup(c => c.Execute(It.IsAny<AddOrUpdateItemLinkBulkCommand>())).Throws(new Exception());

            this.itemLinkUpdateService = new ItemLinkUpdateService(
                mockLogger.Object,
                mockEmailClient.Object,
                mockAddOrUpdateEntitiesBulkCommandHandler.Object,
                addOrUpdateEntitiesRowByRowCommandHandler,
                updateStagingTableDatesForUdmCommandHandler);

            // When.
            this.itemLinkUpdateService.SaveEntitiesBulk(testEntities);

            // Then.
            // Expected exception.
        }

        [TestMethod]
        public void SaveEntitiesRowByRow_SaveIsSuccessfulForEachEntity_EntitiesShouldBeSaved()
        {
            // Given.
            var testEntities = new List<ItemLink>
                {
                    new TestItemLinkBuilder()
                        .WithParentItemId(this.testLinkedItems[0].itemID)
                        .WithChildItemId(this.testItems[0].itemID)
                        .WithLocaleId(this.testLocale.localeID),
                    new TestItemLinkBuilder()
                        .WithParentItemId(this.testLinkedItems[1].itemID)
                        .WithChildItemId(this.testItems[1].itemID)
                        .WithLocaleId(this.testLocale.localeID),
                    new TestItemLinkBuilder()
                        .WithParentItemId(this.testLinkedItems[2].itemID)
                        .WithChildItemId(this.testItems[2].itemID)
                        .WithLocaleId(this.testLocale.localeID)
                }.ConvertAll(e => new ItemLinkModel
                {
                    IrmaPushId = this.testPosData.IRMAPushID,
                    ParentItemId = e.parentItemID,
                    ChildItemId = e.childItemID,
                    LocaleId = e.localeID
                });

            this.itemLinkUpdateService = new ItemLinkUpdateService(
                mockLogger.Object,
                mockEmailClient.Object,
                addOrUpdateEntitiesBulkCommandHandler,
                addOrUpdateEntitiesRowByRowCommandHandler,
                updateStagingTableDatesForUdmCommandHandler);

            // When.
            this.itemLinkUpdateService.SaveEntitiesRowByRow(testEntities);

            // Then.
            var testItemsId = testItems.Select(i => i.itemID).ToList();

            var itemLinkEntities = context.Context.ItemLink.Where(il =>
                il.localeID == testLocale.localeID && testItemsId.Contains(il.childItemID)).ToList();

            Assert.AreEqual(testEntities.Count, itemLinkEntities.Count);
        }

        [TestMethod]
        public void SaveEntitiesRowByRow_NewEntitySaveFailsForOneEntity_FailedEntityShouldNotBeSaved()
        {
            // Given.
            var testEntities = new List<ItemLink>
                {
                    new TestItemLinkBuilder()
                        .WithParentItemId(this.testLinkedItems[0].itemID)
                        .WithChildItemId(this.testItems[0].itemID)
                        .WithLocaleId(this.testLocale.localeID),
                    new TestItemLinkBuilder()
                        .WithParentItemId(this.testLinkedItems[1].itemID)
                        .WithChildItemId(this.testItems[1].itemID)
                        .WithLocaleId(unknownLocaleId),
                    new TestItemLinkBuilder()
                        .WithParentItemId(this.testLinkedItems[2].itemID)
                        .WithChildItemId(this.testItems[2].itemID)
                        .WithLocaleId(this.testLocale.localeID)
                }.ConvertAll(e => new ItemLinkModel
                {
                    IrmaPushId = this.testPosData.IRMAPushID,
                    ParentItemId = e.parentItemID,
                    ChildItemId = e.childItemID,
                    LocaleId = e.localeID
                });

            this.itemLinkUpdateService = new ItemLinkUpdateService(
                mockLogger.Object,
                mockEmailClient.Object,
                addOrUpdateEntitiesBulkCommandHandler,
                addOrUpdateEntitiesRowByRowCommandHandler,
                updateStagingTableDatesForUdmCommandHandler);

            // When.
            this.itemLinkUpdateService.SaveEntitiesRowByRow(testEntities);

            // Then.
            var testItemsId = testItems.Select(i => i.itemID).ToList();
            var successfulItemLinkEntities = context.Context.ItemLink.Where(il =>
                il.localeID == testLocale.localeID && testItemsId.Contains(il.childItemID)).ToList();

            Assert.AreEqual(testEntities.Count - 1, successfulItemLinkEntities.Count);
        }

        [TestMethod]
        public void SaveEntitiesRowByRow_NewEntitySaveFailsForOneEntity_ErrorShouldBeLogged()
        {
            // Given.
            var testEntities = new List<ItemLink>
                {
                    new TestItemLinkBuilder()
                        .WithParentItemId(this.testLinkedItems[0].itemID)
                        .WithChildItemId(this.testItems[0].itemID)
                        .WithLocaleId(this.testLocale.localeID),
                    new TestItemLinkBuilder()
                        .WithParentItemId(this.testLinkedItems[1].itemID)
                        .WithChildItemId(this.testItems[1].itemID)
                        .WithLocaleId(unknownLocaleId),
                    new TestItemLinkBuilder()
                        .WithParentItemId(this.testLinkedItems[2].itemID)
                        .WithChildItemId(this.testItems[2].itemID)
                        .WithLocaleId(this.testLocale.localeID)
                }.ConvertAll(e => new ItemLinkModel
                {
                    IrmaPushId = this.testPosData.IRMAPushID,
                    ParentItemId = e.parentItemID,
                    ChildItemId = e.childItemID,
                    LocaleId = e.localeID
                });

            this.itemLinkUpdateService = new ItemLinkUpdateService(
                mockLogger.Object,
                mockEmailClient.Object,
                addOrUpdateEntitiesBulkCommandHandler,
                addOrUpdateEntitiesRowByRowCommandHandler,
                updateStagingTableDatesForUdmCommandHandler);

            // When.
            this.itemLinkUpdateService.SaveEntitiesRowByRow(testEntities);

            // Then.
            mockLogger.Verify(l => l.Error(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void SaveEntitiesRowByRow_EntityUpdateSaveFailsForOneEntity_FailedEntityShouldHaveOriginalValues()
        {
            // Given.
            StageExistingItemLink();

            var testEntities = new List<ItemLink>
                {
                    new TestItemLinkBuilder()
                        .WithParentItemId(this.testLinkedItems[0].itemID)
                        .WithChildItemId(this.testItems[0].itemID)
                        .WithLocaleId(unknownLocaleId),
                    new TestItemLinkBuilder()
                        .WithParentItemId(this.testLinkedItems[1].itemID)
                        .WithChildItemId(this.testItems[1].itemID)
                        .WithLocaleId(this.testLocale.localeID),
                    new TestItemLinkBuilder()
                        .WithParentItemId(this.testLinkedItems[2].itemID)
                        .WithChildItemId(this.testItems[2].itemID)
                        .WithLocaleId(this.testLocale.localeID)
                }.ConvertAll(e => new ItemLinkModel
                {
                    IrmaPushId = this.testPosData.IRMAPushID,
                    ParentItemId = e.parentItemID,
                    ChildItemId = e.childItemID,
                    LocaleId = e.localeID
                });

            this.itemLinkUpdateService = new ItemLinkUpdateService(
                mockLogger.Object,
                mockEmailClient.Object,
                addOrUpdateEntitiesBulkCommandHandler,
                addOrUpdateEntitiesRowByRowCommandHandler,
                updateStagingTableDatesForUdmCommandHandler);

            // When.
            this.itemLinkUpdateService.SaveEntitiesRowByRow(testEntities);

            // Then.
            var testItemId = testItems[0].itemID;
            var failedItemLinkEntity = context.Context.ItemLink.Single(il =>
                il.localeID == testLocale.localeID && il.childItemID == testItemId);

            Assert.AreEqual(this.testLocale.localeID, failedItemLinkEntity.localeID);
            Assert.AreEqual(testEntities[0].ParentItemId, failedItemLinkEntity.parentItemID);
        }

        [TestMethod]
        public void SaveEntitiesRowByRow_EntityUpdateSaveFailsForOneEntity_ErrorShouldBeLogged()
        {
            // Given.
            StageExistingItemLink();

            var testEntities = new List<ItemLink>
                {
                    new TestItemLinkBuilder()
                        .WithParentItemId(this.testLinkedItems[0].itemID)
                        .WithChildItemId(this.testItems[0].itemID)
                        .WithLocaleId(unknownLocaleId),
                    new TestItemLinkBuilder()
                        .WithParentItemId(this.testLinkedItems[1].itemID)
                        .WithChildItemId(this.testItems[1].itemID)
                        .WithLocaleId(this.testLocale.localeID),
                    new TestItemLinkBuilder()
                        .WithParentItemId(this.testLinkedItems[2].itemID)
                        .WithChildItemId(this.testItems[2].itemID)
                        .WithLocaleId(this.testLocale.localeID)
                }.ConvertAll(e => new ItemLinkModel
                {
                    IrmaPushId = this.testPosData.IRMAPushID,
                    ParentItemId = e.parentItemID,
                    ChildItemId = e.childItemID,
                    LocaleId = e.localeID
                });

            this.itemLinkUpdateService = new ItemLinkUpdateService(
                mockLogger.Object,
                mockEmailClient.Object,
                addOrUpdateEntitiesBulkCommandHandler,
                addOrUpdateEntitiesRowByRowCommandHandler,
                updateStagingTableDatesForUdmCommandHandler);

            // When.
            this.itemLinkUpdateService.SaveEntitiesRowByRow(testEntities);

            // Then.
            mockLogger.Verify(l => l.Error(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void SaveEntitiesRowByRow_EntityUpdateSaveFailsForOneEntity_AlertEmailShouldBeSent()
        {
            // Given.
            StageExistingItemLink();

            var testEntities = new List<ItemLink>
                {
                    new TestItemLinkBuilder()
                        .WithParentItemId(this.testLinkedItems[0].itemID)
                        .WithChildItemId(this.testItems[0].itemID)
                        .WithLocaleId(unknownLocaleId),
                    new TestItemLinkBuilder()
                        .WithParentItemId(this.testLinkedItems[1].itemID)
                        .WithChildItemId(this.testItems[1].itemID)
                        .WithLocaleId(this.testLocale.localeID),
                    new TestItemLinkBuilder()
                        .WithParentItemId(this.testLinkedItems[2].itemID)
                        .WithChildItemId(this.testItems[2].itemID)
                        .WithLocaleId(this.testLocale.localeID)
                }.ConvertAll(e => new ItemLinkModel
                {
                    IrmaPushId = this.testPosData.IRMAPushID,
                    ParentItemId = e.parentItemID,
                    ChildItemId = e.childItemID,
                    LocaleId = e.localeID
                });

            this.itemLinkUpdateService = new ItemLinkUpdateService(
                mockLogger.Object,
                mockEmailClient.Object,
                addOrUpdateEntitiesBulkCommandHandler,
                addOrUpdateEntitiesRowByRowCommandHandler,
                updateStagingTableDatesForUdmCommandHandler);

            // When.
            this.itemLinkUpdateService.SaveEntitiesRowByRow(testEntities);

            // Then.
            mockEmailClient.Verify(e => e.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void SaveEntitiesRowByRow_SaveFailsForOneEntity_FailedDateShouldBeUpdatedInTheStagingTable()
        {
            // Given.
            var testEntities = new List<ItemLink>
                {
                    new TestItemLinkBuilder()
                        .WithParentItemId(this.testLinkedItems[0].itemID)
                        .WithChildItemId(this.testItems[0].itemID)
                        .WithLocaleId(unknownLocaleId),
                    new TestItemLinkBuilder()
                        .WithParentItemId(this.testLinkedItems[1].itemID)
                        .WithChildItemId(this.testItems[1].itemID)
                        .WithLocaleId(this.testLocale.localeID),
                    new TestItemLinkBuilder()
                        .WithParentItemId(this.testLinkedItems[2].itemID)
                        .WithChildItemId(this.testItems[2].itemID)
                        .WithLocaleId(this.testLocale.localeID)
                }.ConvertAll(e => new ItemLinkModel
                {
                    IrmaPushId = this.testPosData.IRMAPushID,
                    ParentItemId = e.parentItemID,
                    ChildItemId = e.childItemID,
                    LocaleId = e.localeID
                });

            this.itemLinkUpdateService = new ItemLinkUpdateService(
                mockLogger.Object,
                mockEmailClient.Object,
                addOrUpdateEntitiesBulkCommandHandler,
                addOrUpdateEntitiesRowByRowCommandHandler,
                updateStagingTableDatesForUdmCommandHandler);

            // When.
            this.itemLinkUpdateService.SaveEntitiesRowByRow(testEntities);

            // Then.
            int failedIrmaPushId = testEntities[0].IrmaPushId;
            var failedPosDataRecord = context.Context.IRMAPush.Single(ip => ip.IRMAPushID == failedIrmaPushId);

            // Have to reload the entity since the price update was done via stored procedure.
            context.Context.Entry(failedPosDataRecord).Reload();

            Assert.AreEqual(DateTime.Now.Date, failedPosDataRecord.UdmFailedDate.Value.Date);
        }
    }
}
