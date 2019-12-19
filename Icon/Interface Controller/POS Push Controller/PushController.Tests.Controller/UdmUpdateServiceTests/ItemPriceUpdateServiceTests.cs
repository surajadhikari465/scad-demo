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
    public class ItemPriceUpdateServiceTests
    {
        private GlobalIconContext context;
        private DbContextTransaction transaction;
        private ItemPriceUpdateService itemPriceUpdateService;
        private Mock<ILogger<ItemPriceUpdateService>> mockLogger;
        private Mock<IEmailClient> mockEmailClient;
        private AddOrUpdateItemPriceBulkCommandHandler addOrUpdateEntitiesBulkCommandHandler;
        private Mock<ICommandHandler<AddOrUpdateItemPriceBulkCommand>> mockAddOrUpdateEntitiesBulkCommandHandler;
        private AddOrUpdateItemPriceRowByRowCommandHandler addOrUpdateEntitiesRowByRowCommandHandler;
        private UpdateStagingTableDatesForUdmCommandHandler updateStagingTableDatesForUdmCommandHandler;
        private IRMAPush testPosData;
        private List<Item> testItems;
        private Locale testLocale;
        private ItemPrice testItemPrice;
        private decimal testOriginalPrice;
        private decimal testUpdatedPrice;
        private string testScanCode;
        private int testBusinessUnitId;
        private int unknownLocaleId;
        private int unknownUomId;

        [TestInitialize]
        public void Initialize()
        {
            this.testScanCode = "2222222";
            this.testBusinessUnitId = 88888;
            this.testOriginalPrice = 1.99m;
            this.testUpdatedPrice = 2.99m;
            this.unknownLocaleId = 99999;
            this.unknownUomId = 9999;

            this.context = new GlobalIconContext(new IconContext());

            this.mockLogger = new Mock<ILogger<ItemPriceUpdateService>>();
            this.mockEmailClient = new Mock<IEmailClient>();
            this.addOrUpdateEntitiesBulkCommandHandler = new AddOrUpdateItemPriceBulkCommandHandler(new Mock<ILogger<AddOrUpdateItemPriceBulkCommandHandler>>().Object, context);
            this.mockAddOrUpdateEntitiesBulkCommandHandler = new Mock<ICommandHandler<AddOrUpdateItemPriceBulkCommand>>();
            this.addOrUpdateEntitiesRowByRowCommandHandler = new AddOrUpdateItemPriceRowByRowCommandHandler(context);
            this.updateStagingTableDatesForUdmCommandHandler = new UpdateStagingTableDatesForUdmCommandHandler(new Mock<ILogger<UpdateStagingTableDatesForUdmCommandHandler>>().Object, context);

            this.transaction = this.context.Context.Database.BeginTransaction();
        }

        [TestCleanup]
        public void Cleanup()
        {
            this.transaction.Rollback();
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

        private void StageTestLocale()
        {
            this.testLocale = new TestLocaleBuilder().WithBusinessUnitId(this.testBusinessUnitId);
            this.context.Context.Locale.Add(this.testLocale);
            this.context.Context.SaveChanges();
        }

        private void StageTestItemPrice(int itemPriceTypeId)
        {
            this.testItemPrice = new TestItemPriceBuilder()
                .WithItemId(this.testItems[0].ItemId)
                .WithLocaleId(this.testLocale.localeID)
                .WithItemPriceTypeId(itemPriceTypeId)
                .WithPriceAmount(this.testOriginalPrice);

            this.context.Context.ItemPrice.Add(this.testItemPrice);
            this.context.Context.SaveChanges();
        }

        [TestMethod]
        public void SaveEntitiesBulk_AddNewRegItemPrice_EntitiesShouldBeSavedWithRegPriceType()
        {
            // Given.
            StageTestPosData();
            StageTestItems();
            StageTestLocale();

            var testEntities = new List<ItemPrice>
            { 
                new TestItemPriceBuilder().WithItemId(this.testItems[0].ItemId).WithLocaleId(this.testLocale.localeID)
            }.ConvertAll(e => new ItemPriceModel
                {
                    IrmaPushId = this.testPosData.IRMAPushID,
                    ItemId = e.itemID,
                    LocaleId = e.localeID,
                    ItemPriceTypeId = e.itemPriceTypeID,
                    ItemPriceAmount = e.itemPriceAmt,
                    UomId = e.uomID,
                    CurrencyTypeId = e.currencyTypeID,
                    BreakPointStartQuantity = e.breakPointStartQty,
                    StartDate = e.startDate,
                    EndDate = e.endDate
                });

            this.itemPriceUpdateService = new ItemPriceUpdateService(
                mockLogger.Object,
                mockEmailClient.Object,
                addOrUpdateEntitiesBulkCommandHandler,
                addOrUpdateEntitiesRowByRowCommandHandler,
                updateStagingTableDatesForUdmCommandHandler);

            // When.
            this.itemPriceUpdateService.SaveEntitiesBulk(testEntities);

            // Then.
            var testItemId = this.testItems[0].ItemId;
            var newItemPriceEntity = context.Context.ItemPrice.Single(ip => ip.localeID == this.testLocale.localeID && testItemId == ip.itemID);

            Assert.AreEqual(testEntities[0].ItemId, newItemPriceEntity.itemID);
            Assert.AreEqual(testEntities[0].LocaleId, newItemPriceEntity.localeID);
            Assert.AreEqual(testEntities[0].ItemPriceTypeId, newItemPriceEntity.itemPriceTypeID);
            Assert.AreEqual(testEntities[0].UomId, newItemPriceEntity.uomID);
            Assert.AreEqual(testEntities[0].CurrencyTypeId, newItemPriceEntity.currencyTypeID);
            Assert.AreEqual(testEntities[0].ItemPriceAmount, newItemPriceEntity.itemPriceAmt);
            Assert.AreEqual(testEntities[0].BreakPointStartQuantity, newItemPriceEntity.breakPointStartQty);
            Assert.AreEqual(testEntities[0].StartDate, newItemPriceEntity.startDate);
            Assert.AreEqual(testEntities[0].EndDate, newItemPriceEntity.endDate);
        }

        [TestMethod]
        public void SaveEntitiesBulk_UpdateExistingRegItemPrice_EntitiesShouldBeUpdatedWithNewPriceInformation()
        {
            // Given.
            StageTestPosData();
            StageTestItems();
            StageTestLocale();
            StageTestItemPrice(itemPriceTypeId: ItemPriceTypes.Reg);

            var testEntities = new List<ItemPrice>
            { 
                new TestItemPriceBuilder().WithItemId(this.testItems[0].ItemId).WithLocaleId(this.testLocale.localeID).WithPriceAmount(this.testUpdatedPrice)
            }.ConvertAll(e => new ItemPriceModel
                {
                    IrmaPushId = this.testPosData.IRMAPushID,
                    ItemId = e.itemID,
                    LocaleId = e.localeID,
                    ItemPriceTypeId = e.itemPriceTypeID,
                    ItemPriceAmount = e.itemPriceAmt,
                    UomId = e.uomID,
                    CurrencyTypeId = e.currencyTypeID,
                    BreakPointStartQuantity = e.breakPointStartQty,
                    StartDate = e.startDate,
                    EndDate = e.endDate
                });

            this.itemPriceUpdateService = new ItemPriceUpdateService(
                mockLogger.Object,
                mockEmailClient.Object,
                addOrUpdateEntitiesBulkCommandHandler,
                addOrUpdateEntitiesRowByRowCommandHandler,
                updateStagingTableDatesForUdmCommandHandler);

            // When.
            this.itemPriceUpdateService.SaveEntitiesBulk(testEntities);

            // Then.
            var testItemId = this.testItems[0].ItemId;
            var updatedItemPriceEntity = context.Context.ItemPrice.Single(ip => ip.localeID == this.testLocale.localeID && testItemId == ip.itemID);

            // Have to reload the entity since the price update was done via stored procedure.
            this.context.Context.Entry(updatedItemPriceEntity).Reload();

            Assert.AreEqual(testEntities[0].ItemId, updatedItemPriceEntity.itemID);
            Assert.AreEqual(testEntities[0].LocaleId, updatedItemPriceEntity.localeID);
            Assert.AreEqual(testEntities[0].ItemPriceTypeId, updatedItemPriceEntity.itemPriceTypeID);
            Assert.AreEqual(testEntities[0].UomId, updatedItemPriceEntity.uomID);
            Assert.AreEqual(testEntities[0].CurrencyTypeId, updatedItemPriceEntity.currencyTypeID);
            Assert.AreEqual(this.testUpdatedPrice, updatedItemPriceEntity.itemPriceAmt);
            Assert.AreEqual(testEntities[0].BreakPointStartQuantity, updatedItemPriceEntity.breakPointStartQty);
            Assert.AreEqual(testEntities[0].StartDate, updatedItemPriceEntity.startDate);
            Assert.AreEqual(testEntities[0].EndDate, updatedItemPriceEntity.endDate);
        }

        [TestMethod]
        public void SaveEntitiesBulk_AddNewNonRegItemPrice_EntitiesShouldBeSavedWithTprPriceType()
        {
            // Given.
            StageTestPosData();
            StageTestItems();
            StageTestLocale();

            var testEntities = new List<ItemPrice>
            { 
                new TestItemPriceBuilder().WithItemId(this.testItems[0].ItemId).WithLocaleId(this.testLocale.localeID).WithItemPriceTypeId(ItemPriceTypes.Tpr)
            }.ConvertAll(e => new ItemPriceModel
                {
                    IrmaPushId = this.testPosData.IRMAPushID,
                    ItemId = e.itemID,
                    LocaleId = e.localeID,
                    ItemPriceTypeId = e.itemPriceTypeID,
                    ItemPriceAmount = e.itemPriceAmt,
                    UomId = e.uomID,
                    CurrencyTypeId = e.currencyTypeID,
                    BreakPointStartQuantity = e.breakPointStartQty,
                    StartDate = e.startDate,
                    EndDate = e.endDate
                });

            this.itemPriceUpdateService = new ItemPriceUpdateService(
                mockLogger.Object,
                mockEmailClient.Object,
                addOrUpdateEntitiesBulkCommandHandler,
                addOrUpdateEntitiesRowByRowCommandHandler,
                updateStagingTableDatesForUdmCommandHandler);

            // When.
            this.itemPriceUpdateService.SaveEntitiesBulk(testEntities);

            // Then.
            var testItemsId = this.testItems.Select(i => i.ItemId).ToList();
            var newItemPriceEntity = context.Context.ItemPrice.Single(ip => ip.localeID == this.testLocale.localeID && testItemsId.Contains(ip.itemID));

            Assert.AreEqual(testEntities[0].ItemId, newItemPriceEntity.itemID);
            Assert.AreEqual(testEntities[0].LocaleId, newItemPriceEntity.localeID);
            Assert.AreEqual(testEntities[0].ItemPriceTypeId, newItemPriceEntity.itemPriceTypeID);
            Assert.AreEqual(testEntities[0].UomId, newItemPriceEntity.uomID);
            Assert.AreEqual(testEntities[0].CurrencyTypeId, newItemPriceEntity.currencyTypeID);
            Assert.AreEqual(testEntities[0].ItemPriceAmount, newItemPriceEntity.itemPriceAmt);
            Assert.AreEqual(testEntities[0].BreakPointStartQuantity, newItemPriceEntity.breakPointStartQty);
            Assert.AreEqual(testEntities[0].StartDate, newItemPriceEntity.startDate);
            Assert.AreEqual(testEntities[0].EndDate, newItemPriceEntity.endDate);
        }

        [TestMethod]
        public void SaveEntitiesBulk_UpdateExistingNonRegItemPrice_EntitiesShouldBeUpdatedWithNewPriceInformation()
        {
            // Given.
            StageTestPosData();
            StageTestItems();
            StageTestLocale();
            StageTestItemPrice(itemPriceTypeId: ItemPriceTypes.Tpr);

            var testEntities = new List<ItemPrice>
            { 
                new TestItemPriceBuilder()
                    .WithItemId(this.testItems[0].ItemId)
                    .WithLocaleId(this.testLocale.localeID)
                    .WithItemPriceTypeId(ItemPriceTypes.Tpr)
                    .WithPriceAmount(this.testUpdatedPrice)
            }.ConvertAll(e => new ItemPriceModel
                {
                    IrmaPushId = this.testPosData.IRMAPushID,
                    ItemId = e.itemID,
                    LocaleId = e.localeID,
                    ItemPriceTypeId = e.itemPriceTypeID,
                    ItemPriceAmount = e.itemPriceAmt,
                    UomId = e.uomID,
                    CurrencyTypeId = e.currencyTypeID,
                    BreakPointStartQuantity = e.breakPointStartQty,
                    StartDate = e.startDate,
                    EndDate = e.endDate
                });

            this.itemPriceUpdateService = new ItemPriceUpdateService(
                mockLogger.Object,
                mockEmailClient.Object,
                addOrUpdateEntitiesBulkCommandHandler,
                addOrUpdateEntitiesRowByRowCommandHandler,
                updateStagingTableDatesForUdmCommandHandler);

            // When.
            this.itemPriceUpdateService.SaveEntitiesBulk(testEntities);

            // Then.
            var testItemId = this.testItems[0].ItemId;

            var updatedItemPriceEntity = context.Context.ItemPrice.Single(ip => ip.localeID == this.testLocale.localeID && testItemId == ip.itemID);

            // Have to reload the entity since the price update was done via stored procedure.
            this.context.Context.Entry(updatedItemPriceEntity).Reload();

            Assert.AreEqual(testEntities[0].ItemId, updatedItemPriceEntity.itemID);
            Assert.AreEqual(testEntities[0].LocaleId, updatedItemPriceEntity.localeID);
            Assert.AreEqual(testEntities[0].ItemPriceTypeId, updatedItemPriceEntity.itemPriceTypeID);
            Assert.AreEqual(testEntities[0].UomId, updatedItemPriceEntity.uomID);
            Assert.AreEqual(testEntities[0].CurrencyTypeId, updatedItemPriceEntity.currencyTypeID);
            Assert.AreEqual(this.testUpdatedPrice, updatedItemPriceEntity.itemPriceAmt);
            Assert.AreEqual(testEntities[0].BreakPointStartQuantity, updatedItemPriceEntity.breakPointStartQty);
            Assert.AreEqual(testEntities[0].StartDate, updatedItemPriceEntity.startDate);
            Assert.AreEqual(testEntities[0].EndDate, updatedItemPriceEntity.endDate);
        }

        [TestMethod]
        public void SaveEntitiesBulk_TransientErrorOccursAndRetryIsSuccessful_EventsShouldBeLogged()
        {
            // This test requires manual intervention to work properly since mocking a SqlException requires trickery.  Just open a locking transaction against the 
            // ItemPrice table before running this test to create the appropriate condition.  The test will be set to auto-pass for automatic runs.

            // Given.
            StageTestPosData();
            StageTestItems();
            StageTestLocale();

            var testEntities = new List<ItemPrice>
            { 
                new TestItemPriceBuilder()
                    .WithItemId(this.testItems[0].ItemId)
                    .WithLocaleId(this.testLocale.localeID)
                    .WithItemPriceTypeId(ItemPriceTypes.Reg),
                    
                new TestItemPriceBuilder()
                    .WithItemId(this.testItems[1].ItemId)
                    .WithLocaleId(this.testLocale.localeID)
                    .WithItemPriceTypeId(ItemPriceTypes.Reg),

                new TestItemPriceBuilder()
                    .WithItemId(this.testItems[2].ItemId)
                    .WithLocaleId(this.testLocale.localeID)
                    .WithItemPriceTypeId(ItemPriceTypes.Reg)
            }.ConvertAll(e => new ItemPriceModel
                {
                    IrmaPushId = this.testPosData.IRMAPushID,
                    ItemId = e.itemID,
                    LocaleId = e.localeID,
                    ItemPriceTypeId = e.itemPriceTypeID,
                    ItemPriceAmount = e.itemPriceAmt,
                    UomId = e.uomID,
                    CurrencyTypeId = e.currencyTypeID,
                    BreakPointStartQuantity = e.breakPointStartQty,
                    StartDate = e.startDate,
                    EndDate = e.endDate
                });

            this.itemPriceUpdateService = new ItemPriceUpdateService(
                mockLogger.Object,
                mockEmailClient.Object,
                addOrUpdateEntitiesBulkCommandHandler,
                addOrUpdateEntitiesRowByRowCommandHandler,
                updateStagingTableDatesForUdmCommandHandler);

            // When.
            this.itemPriceUpdateService.SaveEntitiesBulk(testEntities);

            // Then.

            // Uncomment assertions during manual runs to properly verify the test.

            //mockLogger.Verify(l => l.Error(It.IsAny<string>()), Times.Once);
            //mockLogger.Verify(l => l.Info(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void SaveEntitiesBulk_TransientErrorOccursAndRetryIsSuccessful_EntitiesShouldBeSaved()
        {
            // This test requires manual intervention to work properly since mocking a SqlException requires trickery.  Just open a locking transaction against the 
            // ItemPrice table before running this test to create the appropriate condition.  The test will be set to auto-pass for automatic runs.

            // Given.
            StageTestPosData();
            StageTestItems();
            StageTestLocale();

            var testEntities = new List<ItemPrice>
            { 
                new TestItemPriceBuilder()
                    .WithItemId(this.testItems[0].ItemId)
                    .WithLocaleId(this.testLocale.localeID)
                    .WithItemPriceTypeId(ItemPriceTypes.Reg),
                    
                new TestItemPriceBuilder()
                    .WithItemId(this.testItems[1].ItemId)
                    .WithLocaleId(this.testLocale.localeID)
                    .WithItemPriceTypeId(ItemPriceTypes.Reg),

                new TestItemPriceBuilder()
                    .WithItemId(this.testItems[2].ItemId)
                    .WithLocaleId(this.testLocale.localeID)
                    .WithItemPriceTypeId(ItemPriceTypes.Reg)
            }.ConvertAll(e => new ItemPriceModel
                {
                    IrmaPushId = this.testPosData.IRMAPushID,
                    ItemId = e.itemID,
                    LocaleId = e.localeID,
                    ItemPriceTypeId = e.itemPriceTypeID,
                    ItemPriceAmount = e.itemPriceAmt,
                    UomId = e.uomID,
                    CurrencyTypeId = e.currencyTypeID,
                    BreakPointStartQuantity = e.breakPointStartQty,
                    StartDate = e.startDate,
                    EndDate = e.endDate
                });

            this.itemPriceUpdateService = new ItemPriceUpdateService(
                mockLogger.Object,
                mockEmailClient.Object,
                addOrUpdateEntitiesBulkCommandHandler,
                addOrUpdateEntitiesRowByRowCommandHandler,
                updateStagingTableDatesForUdmCommandHandler);

            // When.
            this.itemPriceUpdateService.SaveEntitiesBulk(testEntities);

            // Then.

            // Uncomment assertions during manual runs to properly verify the test.

            //var testItemsId = testItems.Select(i => i.ItemId).ToList();
            //var itemPriceEntities = context.Context.ItemPrice.Where(ip => ip.localeID == testLocale.localeID && testItemsId.Contains(ip.ItemId)).ToList();

            //Assert.AreEqual(testEntities.Count, itemPriceEntities.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void SaveEntitiesBulk_IntransientErrorOccurs_ExceptionShouldBeThrown()
        {
            // Given.
            StageTestPosData();

            var testEntities = new List<ItemPrice>
            { 
                new TestItemPriceBuilder(), 
                new TestItemPriceBuilder(),
                new TestItemPriceBuilder()
            }.ConvertAll(e => new ItemPriceModel
                {
                    IrmaPushId = this.testPosData.IRMAPushID,
                    ItemId = e.itemID,
                    LocaleId = e.localeID,
                    ItemPriceTypeId = e.itemPriceTypeID,
                    ItemPriceAmount = e.itemPriceAmt,
                    UomId = e.uomID,
                    CurrencyTypeId = e.currencyTypeID,
                    BreakPointStartQuantity = e.breakPointStartQty,
                    StartDate = e.startDate,
                    EndDate = e.endDate
                });

            this.mockAddOrUpdateEntitiesBulkCommandHandler.Setup(c => c.Execute(It.IsAny<AddOrUpdateItemPriceBulkCommand>())).Throws(new Exception());

            this.itemPriceUpdateService = new ItemPriceUpdateService(
                mockLogger.Object,
                mockEmailClient.Object,
                mockAddOrUpdateEntitiesBulkCommandHandler.Object,
                addOrUpdateEntitiesRowByRowCommandHandler,
                updateStagingTableDatesForUdmCommandHandler);

            // When.
            this.itemPriceUpdateService.SaveEntitiesBulk(testEntities);

            // Then.
            // Expected exception.
        }

        [TestMethod]
        public void SaveEntitiesRowByRow_SaveIsSuccessfulForEachEntity_EntitiesShouldBeSaved()
        {
            // Given.
            StageTestPosData();
            StageTestItems();
            StageTestLocale();

            var testEntities = new List<ItemPrice>
            { 
                new TestItemPriceBuilder()
                    .WithItemId(this.testItems[0].ItemId)
                    .WithLocaleId(this.testLocale.localeID)
                    .WithItemPriceTypeId(ItemPriceTypes.Reg),
                    
                new TestItemPriceBuilder()
                    .WithItemId(this.testItems[1].ItemId)
                    .WithLocaleId(this.testLocale.localeID)
                    .WithItemPriceTypeId(ItemPriceTypes.Reg),

                new TestItemPriceBuilder()
                    .WithItemId(this.testItems[2].ItemId)
                    .WithLocaleId(this.testLocale.localeID)
                    .WithItemPriceTypeId(ItemPriceTypes.Reg)
            }.ConvertAll(e => new ItemPriceModel
                {
                    IrmaPushId = this.testPosData.IRMAPushID,
                    ItemId = e.itemID,
                    LocaleId = e.localeID,
                    ItemPriceTypeId = e.itemPriceTypeID,
                    ItemPriceAmount = e.itemPriceAmt,
                    UomId = e.uomID,
                    CurrencyTypeId = e.currencyTypeID,
                    BreakPointStartQuantity = e.breakPointStartQty,
                    StartDate = e.startDate,
                    EndDate = e.endDate
                });

            this.itemPriceUpdateService = new ItemPriceUpdateService(
                mockLogger.Object,
                mockEmailClient.Object,
                addOrUpdateEntitiesBulkCommandHandler,
                addOrUpdateEntitiesRowByRowCommandHandler,
                updateStagingTableDatesForUdmCommandHandler);

            // When.
            this.itemPriceUpdateService.SaveEntitiesRowByRow(testEntities);

            // Then.
            var testItemsId = testItems.Select(i => i.ItemId).ToList();
            var itemPriceEntities = context.Context.ItemPrice.Where(ip => ip.localeID == testLocale.localeID && testItemsId.Contains(ip.itemID)).ToList();

            Assert.AreEqual(testEntities.Count, itemPriceEntities.Count);
        }

        [TestMethod]
        public void SaveEntitiesRowByRow_NewEntitySaveFailsForOneEntity_FailedEntityShouldNotBeSaved()
        {
            // Given.
            StageTestPosData();
            StageTestItems();
            StageTestLocale();

            var testEntities = new List<ItemPrice>
            { 
                new TestItemPriceBuilder()
                    .WithItemId(this.testItems[0].ItemId)
                    .WithLocaleId(this.testLocale.localeID)
                    .WithItemPriceTypeId(ItemPriceTypes.Reg),
                    
                new TestItemPriceBuilder()
                    .WithItemId(this.testItems[1].ItemId)
                    .WithLocaleId(this.unknownLocaleId)
                    .WithItemPriceTypeId(ItemPriceTypes.Reg),

                new TestItemPriceBuilder()
                    .WithItemId(this.testItems[2].ItemId)
                    .WithLocaleId(this.testLocale.localeID)
                    .WithItemPriceTypeId(ItemPriceTypes.Reg)
            }.ConvertAll(e => new ItemPriceModel
                {
                    IrmaPushId = this.testPosData.IRMAPushID,
                    ItemId = e.itemID,
                    LocaleId = e.localeID,
                    ItemPriceTypeId = e.itemPriceTypeID,
                    ItemPriceAmount = e.itemPriceAmt,
                    UomId = e.uomID,
                    CurrencyTypeId = e.currencyTypeID,
                    BreakPointStartQuantity = e.breakPointStartQty,
                    StartDate = e.startDate,
                    EndDate = e.endDate
                });

            this.itemPriceUpdateService = new ItemPriceUpdateService(
                mockLogger.Object,
                mockEmailClient.Object,
                addOrUpdateEntitiesBulkCommandHandler,
                addOrUpdateEntitiesRowByRowCommandHandler,
                updateStagingTableDatesForUdmCommandHandler);

            // When.
            this.itemPriceUpdateService.SaveEntitiesRowByRow(testEntities);

            // Then.
            var testItemsId = testItems.Select(i => i.ItemId).ToList();
            var successfulItemPriceEntities = context.Context.ItemPrice.Where(ip => ip.localeID == testLocale.localeID && testItemsId.Contains(ip.itemID)).ToList();

            Assert.AreEqual(testEntities.Count - 1, successfulItemPriceEntities.Count);
        }

        [TestMethod]
        public void SaveEntitiesRowByRow_NewEntitySaveFailsForOneEntity_ErrorShouldBeLogged()
        {
            // Given.
            StageTestPosData();
            StageTestItems();
            StageTestLocale();

            var testEntities = new List<ItemPrice>
            { 
                new TestItemPriceBuilder()
                    .WithItemId(this.testItems[0].ItemId)
                    .WithLocaleId(this.testLocale.localeID)
                    .WithItemPriceTypeId(ItemPriceTypes.Reg),
                    
                new TestItemPriceBuilder()
                    .WithItemId(this.testItems[1].ItemId)
                    .WithLocaleId(this.unknownLocaleId)
                    .WithItemPriceTypeId(ItemPriceTypes.Reg),

                new TestItemPriceBuilder()
                    .WithItemId(this.testItems[2].ItemId)
                    .WithLocaleId(this.testLocale.localeID)
                    .WithItemPriceTypeId(ItemPriceTypes.Reg)
            }.ConvertAll(e => new ItemPriceModel
                {
                    IrmaPushId = this.testPosData.IRMAPushID,
                    ItemId = e.itemID,
                    LocaleId = e.localeID,
                    ItemPriceTypeId = e.itemPriceTypeID,
                    ItemPriceAmount = e.itemPriceAmt,
                    UomId = e.uomID,
                    CurrencyTypeId = e.currencyTypeID,
                    BreakPointStartQuantity = e.breakPointStartQty,
                    StartDate = e.startDate,
                    EndDate = e.endDate
                });

            this.itemPriceUpdateService = new ItemPriceUpdateService(
                mockLogger.Object,
                mockEmailClient.Object,
                addOrUpdateEntitiesBulkCommandHandler,
                addOrUpdateEntitiesRowByRowCommandHandler,
                updateStagingTableDatesForUdmCommandHandler);

            // When.
            this.itemPriceUpdateService.SaveEntitiesRowByRow(testEntities);

            // Then.
            mockLogger.Verify(l => l.Error(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void SaveEntitiesRowByRow_EntityUpdateSaveFailsForOneEntity_FailedEntityShouldHaveOriginalValues()
        {
            // Given.
            StageTestPosData();
            StageTestItems();
            StageTestLocale();
            StageTestItemPrice(itemPriceTypeId: ItemPriceTypes.Reg);

            var testEntities = new List<ItemPrice>
            { 
                new TestItemPriceBuilder()
                    .WithItemId(this.testItems[0].ItemId)
                    .WithLocaleId(this.testLocale.localeID)
                    .WithItemPriceTypeId(ItemPriceTypes.Reg)
                    .WithPriceAmount(this.testUpdatedPrice)
                    .WithUomId(this.unknownUomId),
                    
                new TestItemPriceBuilder()
                    .WithItemId(this.testItems[1].ItemId)
                    .WithLocaleId(this.testLocale.localeID)
                    .WithItemPriceTypeId(ItemPriceTypes.Reg),

                new TestItemPriceBuilder()
                    .WithItemId(this.testItems[2].ItemId)
                    .WithLocaleId(this.testLocale.localeID)
                    .WithItemPriceTypeId(ItemPriceTypes.Reg)
            }.ConvertAll(e => new ItemPriceModel
                {
                    IrmaPushId = this.testPosData.IRMAPushID,
                    ItemId = e.itemID,
                    LocaleId = e.localeID,
                    ItemPriceTypeId = e.itemPriceTypeID,
                    ItemPriceAmount = e.itemPriceAmt,
                    UomId = e.uomID,
                    CurrencyTypeId = e.currencyTypeID,
                    BreakPointStartQuantity = e.breakPointStartQty,
                    StartDate = e.startDate,
                    EndDate = e.endDate
                });

            this.itemPriceUpdateService = new ItemPriceUpdateService(
                mockLogger.Object,
                mockEmailClient.Object,
                addOrUpdateEntitiesBulkCommandHandler,
                addOrUpdateEntitiesRowByRowCommandHandler,
                updateStagingTableDatesForUdmCommandHandler);

            // When.
            this.itemPriceUpdateService.SaveEntitiesRowByRow(testEntities);

            // Then.
            var testItemId = testItems[0].ItemId;
            var failedItemPriceEntity = context.Context.ItemPrice.Single(ip => ip.localeID == testLocale.localeID && ip.itemID == testItemId);

            Assert.AreEqual(UOMs.Each, failedItemPriceEntity.uomID);
            Assert.AreEqual(this.testOriginalPrice, failedItemPriceEntity.itemPriceAmt);
        }

        [TestMethod]
        public void SaveEntitiesRowByRow_EntityUpdateSaveFailsForOneEntity_ErrorShouldBeLogged()
        {
            // Given.
            StageTestPosData();
            StageTestItems();
            StageTestLocale();
            StageTestItemPrice(itemPriceTypeId: ItemPriceTypes.Reg);

            var testEntities = new List<ItemPrice>
            { 
                new TestItemPriceBuilder()
                    .WithItemId(this.testItems[0].ItemId)
                    .WithLocaleId(this.testLocale.localeID)
                    .WithItemPriceTypeId(ItemPriceTypes.Reg)
                    .WithUomId(this.unknownUomId),
                    
                new TestItemPriceBuilder()
                    .WithItemId(this.testItems[1].ItemId)
                    .WithLocaleId(this.testLocale.localeID)
                    .WithItemPriceTypeId(ItemPriceTypes.Reg),

                new TestItemPriceBuilder()
                    .WithItemId(this.testItems[2].ItemId)
                    .WithLocaleId(this.testLocale.localeID)
                    .WithItemPriceTypeId(ItemPriceTypes.Reg)
            }.ConvertAll(e => new ItemPriceModel
                {
                    IrmaPushId = this.testPosData.IRMAPushID,
                    ItemId = e.itemID,
                    LocaleId = e.localeID,
                    ItemPriceTypeId = e.itemPriceTypeID,
                    ItemPriceAmount = e.itemPriceAmt,
                    UomId = e.uomID,
                    CurrencyTypeId = e.currencyTypeID,
                    BreakPointStartQuantity = e.breakPointStartQty,
                    StartDate = e.startDate,
                    EndDate = e.endDate
                });

            this.itemPriceUpdateService = new ItemPriceUpdateService(
                mockLogger.Object,
                mockEmailClient.Object,
                addOrUpdateEntitiesBulkCommandHandler,
                addOrUpdateEntitiesRowByRowCommandHandler,
                updateStagingTableDatesForUdmCommandHandler);

            // When.
            this.itemPriceUpdateService.SaveEntitiesRowByRow(testEntities);

            // Then.
            mockLogger.Verify(l => l.Error(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void SaveEntitiesRowByRow_EntityUpdateSaveFailsForOneEntity_AlertEmailShouldBeSent()
        {
            // Given.
            StageTestPosData();
            StageTestItems();
            StageTestLocale();
            StageTestItemPrice(itemPriceTypeId: ItemPriceTypes.Reg);

            var testEntities = new List<ItemPrice>
            { 
                new TestItemPriceBuilder()
                    .WithItemId(this.testItems[0].ItemId)
                    .WithLocaleId(this.testLocale.localeID)
                    .WithItemPriceTypeId(ItemPriceTypes.Reg)
                    .WithUomId(this.unknownUomId),
                    
                new TestItemPriceBuilder()
                    .WithItemId(this.testItems[1].ItemId)
                    .WithLocaleId(this.testLocale.localeID)
                    .WithItemPriceTypeId(ItemPriceTypes.Reg),

                new TestItemPriceBuilder()
                    .WithItemId(this.testItems[2].ItemId)
                    .WithLocaleId(this.testLocale.localeID)
                    .WithItemPriceTypeId(ItemPriceTypes.Reg)
            }.ConvertAll(e => new ItemPriceModel
                {
                    IrmaPushId = this.testPosData.IRMAPushID,
                    ItemId = e.itemID,
                    LocaleId = e.localeID,
                    ItemPriceTypeId = e.itemPriceTypeID,
                    ItemPriceAmount = e.itemPriceAmt,
                    UomId = e.uomID,
                    CurrencyTypeId = e.currencyTypeID,
                    BreakPointStartQuantity = e.breakPointStartQty,
                    StartDate = e.startDate,
                    EndDate = e.endDate
                });

            this.itemPriceUpdateService = new ItemPriceUpdateService(
                mockLogger.Object,
                mockEmailClient.Object,
                addOrUpdateEntitiesBulkCommandHandler,
                addOrUpdateEntitiesRowByRowCommandHandler,
                updateStagingTableDatesForUdmCommandHandler);

            // When.
            this.itemPriceUpdateService.SaveEntitiesRowByRow(testEntities);

            // Then.
            mockEmailClient.Verify(e => e.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void SaveEntitiesRowByRow_SaveFailsForOneEntity_FailedDateShouldBeUpdatedInTheStagingTable()
        {
            // Given.
            StageTestPosData();
            StageTestItems();
            StageTestLocale();

            var testEntities = new List<ItemPrice>
            { 
                new TestItemPriceBuilder()
                    .WithItemId(this.testItems[0].ItemId)
                    .WithLocaleId(this.testLocale.localeID)
                    .WithItemPriceTypeId(ItemPriceTypes.Reg)
                    .WithUomId(this.unknownUomId),
                    
                new TestItemPriceBuilder()
                    .WithItemId(this.testItems[1].ItemId)
                    .WithLocaleId(this.testLocale.localeID)
                    .WithItemPriceTypeId(ItemPriceTypes.Reg),

                new TestItemPriceBuilder()
                    .WithItemId(this.testItems[2].ItemId)
                    .WithLocaleId(this.testLocale.localeID)
                    .WithItemPriceTypeId(ItemPriceTypes.Reg)
            }.ConvertAll(e => new ItemPriceModel
                {
                    IrmaPushId = this.testPosData.IRMAPushID,
                    ItemId = e.itemID,
                    LocaleId = e.localeID,
                    ItemPriceTypeId = e.itemPriceTypeID,
                    ItemPriceAmount = e.itemPriceAmt,
                    UomId = e.uomID,
                    CurrencyTypeId = e.currencyTypeID,
                    BreakPointStartQuantity = e.breakPointStartQty,
                    StartDate = e.startDate,
                    EndDate = e.endDate
                });

            this.itemPriceUpdateService = new ItemPriceUpdateService(
                mockLogger.Object,
                mockEmailClient.Object,
                addOrUpdateEntitiesBulkCommandHandler,
                addOrUpdateEntitiesRowByRowCommandHandler,
                updateStagingTableDatesForUdmCommandHandler);

            // When.
            this.itemPriceUpdateService.SaveEntitiesRowByRow(testEntities);

            // Then.
            int failedIrmaPushId = testEntities[0].IrmaPushId;
            var failedPosDataRecord = context.Context.IRMAPush.Single(ip => ip.IRMAPushID == failedIrmaPushId);

            // Have to reload the entity since the price update was done via stored procedure.
            context.Context.Entry(failedPosDataRecord).Reload();

            Assert.AreEqual(DateTime.Now.Date, failedPosDataRecord.UdmFailedDate.Value.Date);
        }
    }
}
