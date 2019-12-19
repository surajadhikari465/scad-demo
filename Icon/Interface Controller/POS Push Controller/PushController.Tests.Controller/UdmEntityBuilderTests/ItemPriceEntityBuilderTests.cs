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
    public class ItemPriceEntityBuilderTests
    {
        private GlobalIconContext context;
        private DbContextTransaction transaction;
        private IRMAPush posDataRecord;
        private ItemPriceEntityBuilder entityBuilder;
        private Mock<ILogger<ItemPriceEntityBuilder>> mockItemPriceBuilderLogger;
        private Mock<IEmailClient> mockEmailClient;
        private ScanCodeCacheHelper scanCodeCacheHelper;
        private LocaleCacheHelper localeCacheHelper;
        private Mock<ILogger<GetScanCodesByIdentifierBulkQueryHandler>> mockGetScanCodesLogger;
        private GetLocalesByBusinessUnitIdQueryHandler getLocaleQueryHandler;
        private UpdateStagingTableDatesForUdmCommandHandler updateStagingTableDatesForUdmCommandHandler;
        private Item testItem;
        private Locale testLocale;
        private string testScanCode;
        private string unknownScanCode;
        private int testBusinessUnitId;
        private int unknownBusinessUnitId;
        private string unknownChangeType;
        
        [TestInitialize]
        public void Initialize()
        {
            this.testScanCode = "2222222";
            this.unknownScanCode = "2222221";
            this.testBusinessUnitId = 88888;
            this.unknownBusinessUnitId = 99999;
            this.unknownChangeType = "Unknown";
            
            this.context = new GlobalIconContext(new IconContext());

            this.mockItemPriceBuilderLogger = new Mock<ILogger<ItemPriceEntityBuilder>>();
            this.mockEmailClient = new Mock<IEmailClient>();
            this.mockGetScanCodesLogger = new Mock<ILogger<GetScanCodesByIdentifierBulkQueryHandler>>();
            this.scanCodeCacheHelper = new ScanCodeCacheHelper(new GetScanCodesByIdentifierBulkQueryHandler(mockGetScanCodesLogger.Object, context));
            this.localeCacheHelper = new LocaleCacheHelper(new GetLocalesByBusinessUnitIdQueryHandler(context));
            this.getLocaleQueryHandler = new GetLocalesByBusinessUnitIdQueryHandler(context);
            this.updateStagingTableDatesForUdmCommandHandler = new UpdateStagingTableDatesForUdmCommandHandler(new Mock<ILogger<UpdateStagingTableDatesForUdmCommandHandler>>().Object, context);

            this.entityBuilder = new ItemPriceEntityBuilder(
                mockItemPriceBuilderLogger.Object,
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

        private void StageTestPosData(IRMAPush posDataRecord)
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
                ItemId = this.testItem.ItemId,
                ItemTypeCode = this.testItem.ItemType.itemTypeCode,
                ItemTypeDesc = this.testItem.ItemType.itemTypeDesc,
                ValidationDate = null,
                NonMerchandiseTrait = null
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
        public void BuildItemPriceEntities_RegularPriceChange_EntityShouldBeConstructedWithRegPriceType()
        {
            // Given.
            this.posDataRecord = new TestIrmaPushBuilder()
                .WithBusinessUnitId(this.testBusinessUnitId)
                .WithIdentifier(this.testScanCode)
                .WithChangeType(Constants.IrmaPushChangeTypes.RegularPriceChange);

            StageTestPosData(posDataRecord);
            StageTestItem();
            StageTestLocale();

            // When.
            var constructedEntities = entityBuilder.BuildEntities(new List<IRMAPush> { posDataRecord });

            // Then.
            Assert.AreEqual(1, constructedEntities.Count);
            Assert.AreEqual(ItemPriceTypes.Reg, constructedEntities[0].ItemPriceTypeId);
        }

        [TestMethod]
        public void BuildItemPriceEntities_NonRegularPriceChange_RegAndNonRegEntitiesShouldBeConstructed()
        {
            // Given.
            this.posDataRecord = new TestIrmaPushBuilder()
                .WithBusinessUnitId(this.testBusinessUnitId)
                .WithIdentifier(this.testScanCode)
                .WithChangeType(Constants.IrmaPushChangeTypes.NonRegularPriceChange)
                .WithSaleInformation(1.99m, 1, DateTime.Now, DateTime.Now.AddDays(1));

            StageTestPosData(posDataRecord);
            StageTestItem();
            StageTestLocale();

            // When.
            var constructedEntities = entityBuilder.BuildEntities(new List<IRMAPush> { posDataRecord });

            // Then.
            Assert.AreEqual(2, constructedEntities.Count);
            Assert.AreEqual(ItemPriceTypes.Reg, constructedEntities[0].ItemPriceTypeId);
            Assert.AreEqual(ItemPriceTypes.Tpr, constructedEntities[1].ItemPriceTypeId);
        }

        [TestMethod]
        public void BuildItemPriceEntities_NonRegularPriceChangeWithExpiredSaleDate_NoEntitiesShouldBeConstructed()
        {
            // Given.
            this.posDataRecord = new TestIrmaPushBuilder()
                .WithBusinessUnitId(this.testBusinessUnitId)
                .WithIdentifier(this.testScanCode)
                .WithChangeType(Constants.IrmaPushChangeTypes.NonRegularPriceChange)
                .WithSaleInformation(salePrice: null, saleMultiple: null, saleStartDate: null, saleEndDate: null);

            StageTestPosData(posDataRecord);
            StageTestItem();
            StageTestLocale();

            // When.
            var constructedEntities = entityBuilder.BuildEntities(new List<IRMAPush> { posDataRecord });

            // Then.
            Assert.AreEqual(0, constructedEntities.Count);
        }

        [TestMethod]
        public void BuildItemPriceEntities_ItemIsSoldByWeight_EntityShouldBeConstructedWithWeightedDefaultUom()
        {
            // Given.
            this.posDataRecord = new TestIrmaPushBuilder()
                .WithBusinessUnitId(this.testBusinessUnitId)
                .WithIdentifier(this.testScanCode)
                .WithChangeType(Constants.IrmaPushChangeTypes.RegularPriceChange)
                .WithSoldByWeight(true);

            StageTestPosData(posDataRecord);
            StageTestItem();
            StageTestLocale();

            // When.
            var constructedEntities = entityBuilder.BuildEntities(new List<IRMAPush> { posDataRecord });

            // Then.
            Assert.AreEqual(UOMs.Pound, constructedEntities[0].UomId);
        }

        [TestMethod]
        public void BuildItemPriceEntities_ItemIsNotSoldByWeight_EntityShouldBeConstructedWithNonWeightedDefaultUom()
        {
            // Given.
            this.posDataRecord = new TestIrmaPushBuilder()
                .WithBusinessUnitId(this.testBusinessUnitId)
                .WithIdentifier(this.testScanCode)
                .WithChangeType(Constants.IrmaPushChangeTypes.RegularPriceChange)
                .WithSoldByWeight(false);

            StageTestPosData(posDataRecord);
            StageTestItem();
            StageTestLocale();

            // When.
            var constructedEntities = entityBuilder.BuildEntities(new List<IRMAPush> { posDataRecord });

            // Then.
            Assert.AreEqual(UOMs.Each, constructedEntities[0].UomId);
        }

        [TestMethod]
        public void BuildItemPriceEntities_UnknownChangeType_EntityShouldNotBeConstructed()
        {
            // Given.
            this.posDataRecord = new TestIrmaPushBuilder()
                .WithBusinessUnitId(this.testBusinessUnitId)
                .WithIdentifier(this.testScanCode)
                .WithChangeType(this.unknownChangeType);

            StageTestPosData(posDataRecord);
            StageTestItem();
            StageTestLocale();

            // When.
            var constructedEntities = entityBuilder.BuildEntities(new List<IRMAPush> { posDataRecord });

            // Then.
            Assert.AreEqual(0, constructedEntities.Count);
        }

        [TestMethod]
        public void BuildItemPriceEntities_UnknownChangeType_ErrorShouldBeLogged()
        {
            // Given.
            this.posDataRecord = new TestIrmaPushBuilder()
                .WithBusinessUnitId(this.testBusinessUnitId)
                .WithIdentifier(this.testScanCode)
                .WithChangeType(this.unknownChangeType);

            StageTestPosData(posDataRecord);
            StageTestItem();
            StageTestLocale();

            // When.
            var constructedEntities = entityBuilder.BuildEntities(new List<IRMAPush> { posDataRecord });

            // Then.
            mockItemPriceBuilderLogger.Verify(l => l.Error(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void BuildItemPriceEntities_UnknownChangeType_EmailAlertShouldBeSent()
        {
            // Given.
            this.posDataRecord = new TestIrmaPushBuilder()
                .WithBusinessUnitId(this.testBusinessUnitId)
                .WithIdentifier(this.testScanCode)
                .WithChangeType(this.unknownChangeType);

            StageTestPosData(posDataRecord);
            StageTestItem();
            StageTestLocale();

            // When.
            var constructedEntities = entityBuilder.BuildEntities(new List<IRMAPush> { posDataRecord });

            // Then.
            mockEmailClient.Verify(e => e.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void BuildItemPriceEntities_UnknownChangeType_UdmFailedDateShouldBeUpdated()
        {
            // Given.
            this.posDataRecord = new TestIrmaPushBuilder()
                .WithBusinessUnitId(this.testBusinessUnitId)
                .WithIdentifier(this.testScanCode)
                .WithChangeType(this.unknownChangeType);

            StageTestPosData(posDataRecord);
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

        [TestMethod]
        public void BuildItemPriceEntities_BusinessUnitIdNotFoundInIcon_MessageShouldNotBeConstructed()
        {
            // Given.
            this.posDataRecord = new TestIrmaPushBuilder().WithBusinessUnitId(this.unknownBusinessUnitId).WithIdentifier(this.testScanCode);

            StageTestPosData(posDataRecord);
            StageTestItem();
            StageTestLocale();

            // When.
            var constructedEntities = entityBuilder.BuildEntities(new List<IRMAPush> { posDataRecord });

            // Then.
            Assert.AreEqual(0, constructedEntities.Count);
        }

        [TestMethod]
        public void BuildItemPriceEntities_BusinessUnitIdNotFoundInIcon_ErrorShouldBeLogged()
        {
            // Given.
            this.posDataRecord = new TestIrmaPushBuilder().WithBusinessUnitId(this.unknownBusinessUnitId).WithIdentifier(this.testScanCode);

            StageTestPosData(posDataRecord);
            StageTestItem();
            StageTestLocale();

            // When.
            var constructedEntities = entityBuilder.BuildEntities(new List<IRMAPush> { posDataRecord });

            // Then.
            mockItemPriceBuilderLogger.Verify(l => l.Error(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void BuildItemPriceEntities_BusinessUnitIdNotFoundInIcon_AlertEmailShouldBeSent()
        {
            // Given.
            this.posDataRecord = new TestIrmaPushBuilder().WithBusinessUnitId(this.unknownBusinessUnitId).WithIdentifier(this.testScanCode);

            StageTestPosData(posDataRecord);
            StageTestItem();
            StageTestLocale();

            // When.
            var constructedEntities = entityBuilder.BuildEntities(new List<IRMAPush> { posDataRecord });

            // Then.
            mockEmailClient.Verify(e => e.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void BuildItemPriceEntities_BusinessUnitIdNotFoundInIcon_UdmFailedDateShouldBeUpdated()
        {
            // Given.
            this.posDataRecord = new TestIrmaPushBuilder().WithBusinessUnitId(this.unknownBusinessUnitId).WithIdentifier(this.testScanCode);

            StageTestPosData(posDataRecord);
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

        [TestMethod]
        public void BuildItemPriceEntities_ScanCodeNotFoundNotFoundInIcon_MessageShouldNotBeConstructed()
        {
            // Given.
            this.posDataRecord = new TestIrmaPushBuilder().WithBusinessUnitId(this.testBusinessUnitId).WithIdentifier(this.unknownScanCode);

            StageTestPosData(posDataRecord);
            StageTestItem();
            StageTestLocale();

            // When.
            var constructedEntities = entityBuilder.BuildEntities(new List<IRMAPush> { posDataRecord });

            // Then.
            Assert.AreEqual(0, constructedEntities.Count);
        }

        [TestMethod]
        public void BuildItemPriceEntities_ScanCodeNotFoundNotFoundInIcon_ErrorShouldBeLogged()
        {
            // Given.
            this.posDataRecord = new TestIrmaPushBuilder().WithBusinessUnitId(this.testBusinessUnitId).WithIdentifier(this.unknownScanCode);

            StageTestPosData(posDataRecord);
            StageTestItem();
            StageTestLocale();

            // When.
            var constructedEntities = entityBuilder.BuildEntities(new List<IRMAPush> { posDataRecord });

            // Then.
            mockItemPriceBuilderLogger.Verify(l => l.Error(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void BuildItemPriceEntities_ScanCodeNotFoundNotFoundInIcon_AlertEmailShouldBeSent()
        {
            // Given.
            this.posDataRecord = new TestIrmaPushBuilder().WithBusinessUnitId(this.testBusinessUnitId).WithIdentifier(this.unknownScanCode);

            StageTestPosData(posDataRecord);
            StageTestItem();
            StageTestLocale();

            // When.
            var constructedEntities = entityBuilder.BuildEntities(new List<IRMAPush> { posDataRecord });

            // Then.
            mockEmailClient.Verify(e => e.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void BuildItemPriceEntities_ScanCodeNotFoundNotFoundInIcon_UdmFailedDateShouldBeUpdated()
        {
            // Given.
            this.posDataRecord = new TestIrmaPushBuilder().WithBusinessUnitId(this.testBusinessUnitId).WithIdentifier(this.unknownScanCode);

            StageTestPosData(posDataRecord);
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
