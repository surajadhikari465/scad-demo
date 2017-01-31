using Icon.Common.Email;
using Icon.Framework;
using Icon.Framework.RenewableContext;
using Icon.Logging;
using Icon.Testing.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PushController.Common;
using PushController.Common.Models;
using PushController.Controller.CacheHelpers;
using PushController.Controller.UdmEntityBuilders;
using PushController.DataAccess.Commands;
using PushController.DataAccess.Queries;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace PushController.Tests.Controller.UdmEntityBuilderTests
{
    [TestClass]
    public class ItemLinkEntityBuilderTests
    {
        private GlobalIconContext context;
        private DbContextTransaction transaction;
        private IRMAPush posDataRecord;
        private ItemLinkEntityBuilder entityBuilder;
        private Mock<ILogger<ItemLinkEntityBuilder>> mockItemLinkBuilderLogger;
        private Mock<IEmailClient> mockEmailClient;
        private ScanCodeCacheHelper scanCodeCacheHelper;
        private LocaleCacheHelper localeCacheHelper;
        private Mock<ILogger<GetScanCodesByIdentifierBulkQueryHandler>> mockGetScanCodesLogger;
        private GetLocalesByBusinessUnitIdQueryHandler getLocaleQueryHandler;
        private UpdateStagingTableDatesForUdmCommandHandler updateStagingTableDatesForUdmCommandHandler;
        private Item testItem;
        private Item testLinkedItem;
        private Locale testLocale;
        private string testScanCode;
        private string unknownScanCode;
        private int testBusinessUnitId;
        private int unknownBusinessUnitId;
        private string unknownChangeType;
        private string testLinkedIdentifier;

        [TestInitialize]
        public void Initialize()
        {
            this.testScanCode = "2222222";
            this.unknownScanCode = "2222221";
            this.testBusinessUnitId = 88888;
            this.unknownBusinessUnitId = 99999;
            this.unknownChangeType = "Unknown";
            this.testLinkedIdentifier = "2016052612";

            this.context = new GlobalIconContext(new IconContext());

            this.mockItemLinkBuilderLogger = new Mock<ILogger<ItemLinkEntityBuilder>>();
            this.mockEmailClient = new Mock<IEmailClient>();
            this.mockGetScanCodesLogger = new Mock<ILogger<GetScanCodesByIdentifierBulkQueryHandler>>();
            this.scanCodeCacheHelper = new ScanCodeCacheHelper(new GetScanCodesByIdentifierBulkQueryHandler(mockGetScanCodesLogger.Object, context));
            this.localeCacheHelper = new LocaleCacheHelper(new GetLocalesByBusinessUnitIdQueryHandler(context));
            this.getLocaleQueryHandler = new GetLocalesByBusinessUnitIdQueryHandler(context);
            this.updateStagingTableDatesForUdmCommandHandler = new UpdateStagingTableDatesForUdmCommandHandler(new Mock<ILogger<UpdateStagingTableDatesForUdmCommandHandler>>().Object, context);

            this.entityBuilder = new ItemLinkEntityBuilder(
                mockItemLinkBuilderLogger.Object,
                mockEmailClient.Object,
                scanCodeCacheHelper,
                localeCacheHelper,
                getLocaleQueryHandler,
                updateStagingTableDatesForUdmCommandHandler);

            Cache.identifierToScanCode.Clear();
            Cache.businessUnitToLocale.Clear();

            this.transaction = context.Context.Database.BeginTransaction();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
        }

        private void StagePosData(IRMAPush posDataRecord)
        {
            this.context.Context.IRMAPush.Add(posDataRecord);
            this.context.Context.SaveChanges();
        }

        private void StageTestItem()
        {
            this.testItem = new TestItemBuilder().WithScanCode(this.testScanCode);
            this.testItem.ItemType = this.context.Context.ItemType.Single(it => it.itemTypeCode == ItemTypeCodes.RetailSale);
            this.testItem.ScanCode.Single().ScanCodeType = this.context.Context.ScanCodeType.Single(sct => sct.scanCodeTypeID == ScanCodeTypes.Upc);
            this.context.Context.Item.Add(this.testItem);
            this.context.Context.SaveChanges();

            var scanCodeModel = new ScanCodeModel
            {
                ScanCode = this.testScanCode,
                ScanCodeId = this.testItem.ScanCode.Single().scanCodeID,
                ScanCodeTypeId = this.testItem.ScanCode.Single().scanCodeTypeID,
                ScanCodeTypeDesc = this.testItem.ScanCode.Single().ScanCodeType.scanCodeTypeDesc,
                ItemId = this.testItem.itemID,
                ItemTypeCode = this.testItem.ItemType.itemTypeCode,
                ItemTypeDesc = this.testItem.ItemType.itemTypeDesc,
                ValidationDate = null,
                NonMerchandiseTrait = null
            };

            Cache.identifierToScanCode.Add(scanCodeModel.ScanCode, scanCodeModel);
        }

        private void StageTestLinkedItem(string nonMerchandiseTrait, string itemTypeCode = ItemTypeCodes.Deposit)
        {
            this.testLinkedItem = new TestItemBuilder().WithScanCode(this.testLinkedIdentifier);
            this.testLinkedItem.ItemType = this.context.Context.ItemType.Single(it => it.itemTypeCode == itemTypeCode);
            this.testLinkedItem.ScanCode.Single().ScanCodeType = this.context.Context.ScanCodeType.Single(sct => sct.scanCodeTypeID == ScanCodeTypes.PosPlu);
            this.context.Context.Item.Add(this.testLinkedItem);
            this.context.Context.SaveChanges();

            var scanCodeModel = new ScanCodeModel
            {
                ScanCode = this.testLinkedIdentifier,
                ScanCodeId = this.testLinkedItem.ScanCode.Single().scanCodeID,
                ScanCodeTypeId = this.testLinkedItem.ScanCode.Single().scanCodeTypeID,
                ScanCodeTypeDesc = this.testLinkedItem.ScanCode.Single().ScanCodeType.scanCodeTypeDesc,
                ItemId = this.testLinkedItem.itemID,
                ItemTypeCode = this.testLinkedItem.ItemType.itemTypeCode,
                ItemTypeDesc = this.testLinkedItem.ItemType.itemTypeDesc,
                ValidationDate = null,
                NonMerchandiseTrait = nonMerchandiseTrait
            };

            Cache.identifierToScanCode.Add(scanCodeModel.ScanCode, scanCodeModel);
        }

        private void StageTestLocale()
        {
            this.testLocale = new TestLocaleBuilder().WithBusinessUnitId(this.testBusinessUnitId);
            this.context.Context.Locale.Add(this.testLocale);
            this.context.Context.SaveChanges();

            Cache.businessUnitToLocale.Add(testBusinessUnitId, testLocale);
        }

        [TestMethod]
        public void BuildItemLinkEntities_PushRecordContainsNoLinkedItem_ItemLinkDeleteShouldNotBeConstructed()
        {
            // Given.
            this.posDataRecord = new TestIrmaPushBuilder()
                .WithBusinessUnitId(this.testBusinessUnitId)
                .WithIdentifier(this.testScanCode)
                .WithChangeType(Constants.IrmaPushChangeTypes.ItemLocaleAttributeChange);

            StagePosData(posDataRecord);
            StageTestItem();
            StageTestLocale();

            // When.
            var constructedEntities = entityBuilder.BuildEntities(new List<IRMAPush> { posDataRecord });

            // Then.
            Assert.AreEqual(1, constructedEntities.Count);
        }

        [TestMethod]
        public void BuildItemLinkEntities_LinkedItemDoesNotExistInIcon_EntityShouldNotBeConstructed()
        {
            // Given.
            this.posDataRecord = new TestIrmaPushBuilder()
                .WithBusinessUnitId(this.testBusinessUnitId)
                .WithIdentifier(this.testScanCode)
                .WithChangeType(Constants.IrmaPushChangeTypes.ItemLocaleAttributeChange)
                .WithLinkedIdentifier(this.testLinkedIdentifier);

            StagePosData(posDataRecord);
            StageTestItem();
            StageTestLocale();

            // When.
            var constructedEntities = entityBuilder.BuildEntities(new List<IRMAPush> { posDataRecord });

            // Then.
            Assert.AreEqual(0, constructedEntities.Count);
        }

        [TestMethod]
        public void BuildItemLinkEntities_LinkedItemIsNotDepositCrvOrFee_EntityShouldNotBeConstructed()
        {
            // Given.
            this.posDataRecord = new TestIrmaPushBuilder()
                .WithBusinessUnitId(this.testBusinessUnitId)
                .WithIdentifier(this.testScanCode)
                .WithChangeType(Constants.IrmaPushChangeTypes.ItemLocaleAttributeChange)
                .WithLinkedIdentifier(this.testLinkedIdentifier);

            StagePosData(posDataRecord);
            StageTestItem();
            StageTestLinkedItem(null);
            StageTestLocale();

            // When.
            var constructedEntities = entityBuilder.BuildEntities(new List<IRMAPush> { posDataRecord });

            // Then.
            Assert.AreEqual(0, constructedEntities.Count);
        }

        [TestMethod]
        public void BuildItemLinkEntities_UseItemTypeAndLinkedItemIsNotDepositOrFee_EntityShouldNotBeConstructed()
        {
            // Given.
            StartupOptions.UseItemTypeInsteadOfNonMerchTrait = true;

            this.posDataRecord = new TestIrmaPushBuilder()
                .WithBusinessUnitId(this.testBusinessUnitId)
                .WithIdentifier(this.testScanCode)
                .WithChangeType(Constants.IrmaPushChangeTypes.ItemLocaleAttributeChange)
                .WithLinkedIdentifier(this.testLinkedIdentifier);

            StagePosData(posDataRecord);
            StageTestItem();
            StageTestLinkedItem(nonMerchandiseTrait: null, itemTypeCode: ItemTypeCodes.RetailSale);
            StageTestLocale();

            // When.
            var constructedEntities = entityBuilder.BuildEntities(new List<IRMAPush> { posDataRecord });

            // Then.
            Assert.AreEqual(0, constructedEntities.Count);

            StartupOptions.UseItemTypeInsteadOfNonMerchTrait = false;
        }

        [TestMethod]
        public void BuildItemLinkEntities_ChangeTypeItemLocaleAttributeChange_EntityShouldBeConstructedWithLinkUpdate()
        {
            // Given.
            this.posDataRecord = new TestIrmaPushBuilder()
                .WithBusinessUnitId(this.testBusinessUnitId)
                .WithIdentifier(this.testScanCode)
                .WithChangeType(Constants.IrmaPushChangeTypes.ItemLocaleAttributeChange)
                .WithLinkedIdentifier(this.testLinkedIdentifier);

            StagePosData(posDataRecord);
            StageTestItem();
            StageTestLinkedItem(NonMerchandiseTraits.BottleDeposit);
            StageTestLocale();

            // When.
            var constructedEntities = entityBuilder.BuildEntities(new List<IRMAPush> { posDataRecord });

            // Then.
            Assert.AreEqual(testLinkedItem.itemID, constructedEntities[0].ParentItemId);
            Assert.AreEqual(testItem.itemID, constructedEntities[0].ChildItemId);
            Assert.AreEqual(testLocale.localeID, constructedEntities[0].LocaleId);
        }

        [TestMethod]
        public void BuildItemLinkEntities_ChangeTypeScanCodeAdd_EntityShouldBeConstructedWithLinkUpdate()
        {
            // Given.
            this.posDataRecord = new TestIrmaPushBuilder()
                .WithBusinessUnitId(this.testBusinessUnitId)
                .WithIdentifier(this.testScanCode)
                .WithChangeType(Constants.IrmaPushChangeTypes.ScanCodeAdd)
                .WithLinkedIdentifier(this.testLinkedIdentifier);

            StagePosData(posDataRecord);
            StageTestItem();
            StageTestLinkedItem(NonMerchandiseTraits.BottleDeposit);
            StageTestLocale();

            // When.
            var constructedEntities = entityBuilder.BuildEntities(new List<IRMAPush> { posDataRecord });

            // Then.
            Assert.AreEqual(testLinkedItem.itemID, constructedEntities[0].ParentItemId);
            Assert.AreEqual(testItem.itemID, constructedEntities[0].ChildItemId);
            Assert.AreEqual(testLocale.localeID, constructedEntities[0].LocaleId);
        }

        [TestMethod]
        public void BuildItemLinkEntities_ChangeTypeScanCodeAuthorization_EntityShouldBeConstructedWithLinkUpdate()
        {
            // Given.
            this.posDataRecord = new TestIrmaPushBuilder()
                .WithBusinessUnitId(this.testBusinessUnitId)
                .WithIdentifier(this.testScanCode)
                .WithChangeType(Constants.IrmaPushChangeTypes.ScanCodeAuthorization)
                .WithLinkedIdentifier(this.testLinkedIdentifier);

            StagePosData(posDataRecord);
            StageTestItem();
            StageTestLinkedItem(NonMerchandiseTraits.BottleDeposit);
            StageTestLocale();

            // When.
            var constructedEntities = entityBuilder.BuildEntities(new List<IRMAPush> { posDataRecord });

            // Then.
            Assert.AreEqual(testLinkedItem.itemID, constructedEntities[0].ParentItemId);
            Assert.AreEqual(testItem.itemID, constructedEntities[0].ChildItemId);
            Assert.AreEqual(testLocale.localeID, constructedEntities[0].LocaleId);
        }

        [TestMethod]
        public void BuildItemLinkEntities_ChangeTypeScanCodeDeauthorization_EntityShouldNotBeConstructed()
        {
            // Given.
            this.posDataRecord = new TestIrmaPushBuilder()
                .WithBusinessUnitId(this.testBusinessUnitId)
                .WithIdentifier(this.testScanCode)
                .WithChangeType(Constants.IrmaPushChangeTypes.ScanCodeDeauthorization)
                .WithLinkedIdentifier(this.testLinkedIdentifier);

            StagePosData(posDataRecord);
            StageTestItem();
            StageTestLinkedItem(NonMerchandiseTraits.BottleDeposit);
            StageTestLocale();

            // When.
            var constructedEntities = entityBuilder.BuildEntities(new List<IRMAPush> { posDataRecord });

            // Then.
            Assert.AreEqual(0, constructedEntities.Count);
        }

        [TestMethod]
        public void BuildItemLinkEntities_ChangeTypeScanCodeDelete_EntityShouldNotBeConstructed()
        {
            // Given.
            this.posDataRecord = new TestIrmaPushBuilder()
                .WithBusinessUnitId(this.testBusinessUnitId)
                .WithIdentifier(this.testScanCode)
                .WithChangeType(Constants.IrmaPushChangeTypes.ScanCodeDelete)
                .WithLinkedIdentifier(this.testLinkedIdentifier);

            StagePosData(posDataRecord);
            StageTestItem();
            StageTestLinkedItem(NonMerchandiseTraits.BottleDeposit);
            StageTestLocale();

            // When.
            var constructedEntities = entityBuilder.BuildEntities(new List<IRMAPush> { posDataRecord });

            // Then.
            Assert.AreEqual(0, constructedEntities.Count);
        }

        [TestMethod]
        public void BuildItemLinkEntities_UnknownChangeType_EntityShouldNotBeConstructed()
        {
            // Given.
            this.posDataRecord = new TestIrmaPushBuilder()
                .WithBusinessUnitId(this.testBusinessUnitId)
                .WithIdentifier(this.testScanCode)
                .WithChangeType(this.unknownChangeType)
                .WithLinkedIdentifier(this.testLinkedIdentifier);

            StagePosData(posDataRecord);
            StageTestItem();
            StageTestLinkedItem(NonMerchandiseTraits.BottleDeposit);
            StageTestLocale();

            // When.
            var constructedEntities = entityBuilder.BuildEntities(new List<IRMAPush> { posDataRecord });

            // Then.
            Assert.AreEqual(0, constructedEntities.Count);
        }

        [TestMethod]
        public void BuildItemLinkEntities_UnknownChangeType_ErrorShouldBeLogged()
        {
            // Given.
            this.posDataRecord = new TestIrmaPushBuilder()
                .WithBusinessUnitId(this.testBusinessUnitId)
                .WithIdentifier(this.testScanCode)
                .WithChangeType(this.unknownChangeType)
                .WithLinkedIdentifier(this.testLinkedIdentifier);

            StagePosData(posDataRecord);
            StageTestItem();
            StageTestLinkedItem(NonMerchandiseTraits.BottleDeposit);
            StageTestLocale();

            // When.
            var constructedEntities = entityBuilder.BuildEntities(new List<IRMAPush> { posDataRecord });

            // Then.
            mockItemLinkBuilderLogger.Verify(l => l.Error(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void BuildItemLinkEntities_UnknownChangeType_AlertEmailShouldBeSent()
        {
            // Given.
            this.posDataRecord = new TestIrmaPushBuilder()
                .WithBusinessUnitId(this.testBusinessUnitId)
                .WithIdentifier(this.testScanCode)
                .WithChangeType(this.unknownChangeType)
                .WithLinkedIdentifier(this.testLinkedIdentifier);

            StagePosData(posDataRecord);
            StageTestItem();
            StageTestLinkedItem(NonMerchandiseTraits.BottleDeposit);
            StageTestLocale();

            // When.
            var constructedEntities = entityBuilder.BuildEntities(new List<IRMAPush> { posDataRecord });

            // Then.
            mockEmailClient.Verify(e => e.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void BuildItemLinkEntities_UnknownChangeType_UdmFailedDateShouldBeUpdated()
        {
            // Given.
            this.posDataRecord = new TestIrmaPushBuilder()
                .WithBusinessUnitId(this.testBusinessUnitId)
                .WithIdentifier(this.testScanCode)
                .WithChangeType(this.unknownChangeType)
                .WithLinkedIdentifier(this.testLinkedIdentifier);

            StagePosData(posDataRecord);
            StageTestItem();
            StageTestLinkedItem(NonMerchandiseTraits.BottleDeposit);
            StageTestLocale();

            // When.
            var constructedEntities = entityBuilder.BuildEntities(new List<IRMAPush> { posDataRecord });

            // Then.
            var updatedPosDataRecord = this.context.Context.IRMAPush.Single(ip => ip.IRMAPushID == posDataRecord.IRMAPushID);

            // Have to reload the entity since the price update was done via stored procedure.
            context.Context.Entry(updatedPosDataRecord).Reload();

            Assert.AreEqual(DateTime.Now.Date, updatedPosDataRecord.UdmFailedDate.Value.Date);
            Assert.IsNull(updatedPosDataRecord.InUdmDate);
        }

        [TestMethod]
        public void BuildItemLinkEntities_BusinessUnitIdNotFoundInIcon_EntityShouldNotBeConstructed()
        {
            // Given.
            this.posDataRecord = new TestIrmaPushBuilder()
                .WithBusinessUnitId(this.unknownBusinessUnitId)
                .WithIdentifier(this.testScanCode)
                .WithLinkedIdentifier(this.testLinkedIdentifier);

            StagePosData(posDataRecord);
            StageTestItem();
            StageTestLinkedItem(NonMerchandiseTraits.BottleDeposit);
            StageTestLocale();

            // When.
            var constructedEntities = entityBuilder.BuildEntities(new List<IRMAPush> { posDataRecord });

            // Then.
            Assert.AreEqual(0, constructedEntities.Count);
        }

        [TestMethod]
        public void BuildItemLinkEntities_BusinessUnitIdNotFoundInIcon_ErrorShouldBeLogged()
        {
            // Given.
            this.posDataRecord = new TestIrmaPushBuilder()
                .WithBusinessUnitId(this.unknownBusinessUnitId)
                .WithIdentifier(this.testScanCode)
                .WithLinkedIdentifier(this.testLinkedIdentifier);

            StagePosData(posDataRecord);
            StageTestItem();
            StageTestLinkedItem(NonMerchandiseTraits.BottleDeposit);
            StageTestLocale();

            // When.
            var constructedEntities = entityBuilder.BuildEntities(new List<IRMAPush> { posDataRecord });

            // Then.
            mockItemLinkBuilderLogger.Verify(l => l.Error(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void BuildItemLinkEntities_BusinessUnitIdNotFoundInIcon_AlertEmailShouldBeSent()
        {
            // Given.
            this.posDataRecord = new TestIrmaPushBuilder()
                .WithBusinessUnitId(this.unknownBusinessUnitId)
                .WithIdentifier(this.testScanCode)
                .WithLinkedIdentifier(this.testLinkedIdentifier);

            StagePosData(posDataRecord);
            StageTestItem();
            StageTestLinkedItem(NonMerchandiseTraits.BottleDeposit);
            StageTestLocale();

            // When.
            var constructedEntities = entityBuilder.BuildEntities(new List<IRMAPush> { posDataRecord });

            // Then.
            mockEmailClient.Verify(e => e.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void BuildItemLinkEntities_BusinessUnitIdNotFoundInIcon_UdmFailedDateShouldBeUpdated()
        {
            // Given.
            this.posDataRecord = new TestIrmaPushBuilder()
                .WithBusinessUnitId(this.unknownBusinessUnitId)
                .WithIdentifier(this.testScanCode)
                .WithLinkedIdentifier(this.testLinkedIdentifier);

            StagePosData(posDataRecord);
            StageTestItem();
            StageTestLinkedItem(NonMerchandiseTraits.BottleDeposit);
            StageTestLocale();

            // When.
            var constructedEntities = entityBuilder.BuildEntities(new List<IRMAPush> { posDataRecord });

            // Then.
            var updatedPosDataRecord = this.context.Context.IRMAPush.Single(ip => ip.IRMAPushID == posDataRecord.IRMAPushID);

            // Have to reload the entity since the price update was done via stored procedure.
            context.Context.Entry(updatedPosDataRecord).Reload();

            Assert.AreEqual(DateTime.Now.Date, updatedPosDataRecord.UdmFailedDate.Value.Date);
            Assert.IsNull(updatedPosDataRecord.InUdmDate);
        }

        [TestMethod]
        public void BuildItemLinkEntities_ScanCodeNotFoundNotFoundInIcon_EntityShouldNotBeConstructed()
        {
            // Given.
            this.posDataRecord = new TestIrmaPushBuilder()
                .WithBusinessUnitId(this.testBusinessUnitId)
                .WithIdentifier(this.unknownScanCode)
                .WithLinkedIdentifier(this.testLinkedIdentifier);

            StagePosData(posDataRecord);
            StageTestItem();
            StageTestLocale();

            // When.
            var constructedEntities = entityBuilder.BuildEntities(new List<IRMAPush> { posDataRecord });

            // Then.
            Assert.AreEqual(0, constructedEntities.Count);
        }

        [TestMethod]
        public void BuildItemLinkEntities_ScanCodeNotFoundNotFoundInIcon_ErrorShouldBeLogged()
        {
            // Given.
            this.posDataRecord = new TestIrmaPushBuilder()
                .WithBusinessUnitId(this.testBusinessUnitId)
                .WithIdentifier(this.unknownScanCode)
                .WithLinkedIdentifier(this.testLinkedIdentifier);

            StagePosData(posDataRecord);
            StageTestItem();
            StageTestLocale();

            // When.
            var constructedEntities = entityBuilder.BuildEntities(new List<IRMAPush> { posDataRecord });

            // Then.
            mockItemLinkBuilderLogger.Verify(l => l.Error(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void BuildItemLinkEntities_ScanCodeNotFoundNotFoundInIcon_AlertEmailShouldBeSent()
        {
            // Given.
            this.posDataRecord = new TestIrmaPushBuilder()
                .WithBusinessUnitId(this.testBusinessUnitId)
                .WithIdentifier(this.unknownScanCode)
                .WithLinkedIdentifier(this.testLinkedIdentifier);

            StagePosData(posDataRecord);
            StageTestItem();
            StageTestLocale();

            // When.
            var constructedEntities = entityBuilder.BuildEntities(new List<IRMAPush> { posDataRecord });

            // Then.
            mockEmailClient.Verify(e => e.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void BuildItemLinkEntities_ScanCodeNotFoundNotFoundInIcon_UdmFailedDateShouldBeUpdated()
        {
            // Given.
            this.posDataRecord = new TestIrmaPushBuilder()
                .WithBusinessUnitId(this.testBusinessUnitId)
                .WithIdentifier(this.unknownScanCode)
                .WithLinkedIdentifier(this.testLinkedIdentifier);

            StagePosData(posDataRecord);
            StageTestItem();
            StageTestLocale();

            // When.
            var constructedEntities = entityBuilder.BuildEntities(new List<IRMAPush> { posDataRecord });

            // Then.
            var updatedPosDataRecord = this.context.Context.IRMAPush.Single(ip => ip.IRMAPushID == posDataRecord.IRMAPushID);

            // Have to reload the entity since the price update was done via stored procedure.
            context.Context.Entry(updatedPosDataRecord).Reload();

            Assert.AreEqual(DateTime.Now.Date, updatedPosDataRecord.UdmFailedDate.Value.Date);
            Assert.IsNull(updatedPosDataRecord.InUdmDate);
        }
    }
}
