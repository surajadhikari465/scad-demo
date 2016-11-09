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
using PushController.Controller.MessageBuilders;
using PushController.DataAccess.Commands;
using PushController.DataAccess.Queries;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace PushController.Tests.Controller.MessageBuilderTests
{
    [TestClass]
    public class PriceMessageBuilderTests
    {
        private GlobalIconContext context;
        private DbContextTransaction transaction;
        private PriceMessageBuilder messageBuilder;
        private Mock<ILogger<PriceMessageBuilder>> mockMessageBuilderLogger;
        private Mock<IEmailClient> mockEmailClient;
        private ScanCodeCacheHelper scanCodeCacheHelper;
        private LocaleCacheHelper localeCacheHelper;
        private Mock<ILogger<GetScanCodesByIdentifierBulkQueryHandler>> mockGetScanCodesLogger;
        private GetLocalesByBusinessUnitIdQueryHandler getLocaleQueryHandler;
        private GetPriceUomQueryHandler getPriceUomQueryHandler;
        private GetItemPriceQueryHandler getItemPriceQueryHandler;
        private UpdateStagingTableDatesForEsbCommandHandler updateStagingTableDatesForEsbCommandHandler;
        private IRMAPush posDataRecord;
        private Item testItem;
        private Locale testLocale;
        private ItemPrice testSale;
        private string testScanCode;
        private string unknownScanCode;
        private int testBusinessUnitId;
        private int unknownBusinessUnitId;
        private string testRegPriceChangeType;
        private string testNonRegPriceChangeType;
        private string unknownChangeType;
        private string cancelAllSalesChangeType;

        [TestInitialize]
        public void Initialize()
        {
            this.context = new GlobalIconContext(new IconContext());

            this.testScanCode = "2222222";
            this.unknownScanCode = "2222221";
            this.testBusinessUnitId = 88888;
            this.unknownBusinessUnitId = 99999;
            this.testRegPriceChangeType = Constants.IrmaPushChangeTypes.ScanCodeAdd;
            this.testNonRegPriceChangeType = Constants.IrmaPushChangeTypes.NonRegularPriceChange;
            this.unknownChangeType = "Unknown";
            this.cancelAllSalesChangeType = "CancelAllSales";

            this.mockMessageBuilderLogger = new Mock<ILogger<PriceMessageBuilder>>();
            this.mockEmailClient = new Mock<IEmailClient>();
            this.mockGetScanCodesLogger = new Mock<ILogger<GetScanCodesByIdentifierBulkQueryHandler>>();
            this.scanCodeCacheHelper = new ScanCodeCacheHelper(new GetScanCodesByIdentifierBulkQueryHandler(mockGetScanCodesLogger.Object, context));
            this.localeCacheHelper = new LocaleCacheHelper(new GetLocalesByBusinessUnitIdQueryHandler(context));
            this.getLocaleQueryHandler = new GetLocalesByBusinessUnitIdQueryHandler(context);
            this.getPriceUomQueryHandler = new GetPriceUomQueryHandler(context);
            this.getItemPriceQueryHandler = new GetItemPriceQueryHandler(context);
            this.updateStagingTableDatesForEsbCommandHandler = new UpdateStagingTableDatesForEsbCommandHandler(new Mock<ILogger<UpdateStagingTableDatesForEsbCommandHandler>>().Object, context);

            this.messageBuilder = new PriceMessageBuilder(
                mockMessageBuilderLogger.Object,
                mockEmailClient.Object,
                scanCodeCacheHelper,
                localeCacheHelper,
                getLocaleQueryHandler,
                getPriceUomQueryHandler,
                getItemPriceQueryHandler,
                updateStagingTableDatesForEsbCommandHandler);

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

        private void StageTestItem(string nonMerchandiseTraitName, bool departmentSale)
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
                DepartmentSaleTrait = departmentSale ? "1" : null,
                NonMerchandiseTrait = nonMerchandiseTraitName
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

        private void StageTestSale(bool expired = false)
        {
            this.testSale = new TestItemPriceBuilder()
                .WithItemId(this.testItem.itemID)
                .WithLocaleId(this.testLocale.localeID)
                .WithItemPriceTypeId(ItemPriceTypes.Tpr);

            if (expired)
            {
                this.testSale.endDate = DateTime.Now.Date.AddDays(-1);
            }
                
            this.context.Context.ItemPrice.Add(this.testSale);
            this.context.Context.SaveChanges();
        }

        [TestMethod]
        public void BuildPriceMessage_ValidPosDataRecord_MessageShouldBeConstructedWithCorrectValues()
        {
            // Given.
            this.posDataRecord = new TestIrmaPushBuilder()
                .WithBusinessUnitId(this.testBusinessUnitId)
                .WithIdentifier(this.testScanCode)
                .WithChangeType(this.testRegPriceChangeType);

            StagePosData(posDataRecord);
            StageTestItem(nonMerchandiseTraitName: null, departmentSale: false);
            StageTestLocale();

            // When.
            var constructedMessages = messageBuilder.BuildMessages(new List<IRMAPush> { posDataRecord });

            // Then.
            Assert.AreEqual(MessageTypes.Price, constructedMessages[0].MessageTypeId);
            Assert.AreEqual(MessageStatusTypes.Ready, constructedMessages[0].MessageStatusId);
            Assert.IsNull(constructedMessages[0].MessageHistoryId);
            Assert.AreEqual(this.posDataRecord.BusinessUnit_ID, constructedMessages[0].BusinessUnit_ID);
            Assert.AreEqual(this.posDataRecord.RegionCode, constructedMessages[0].RegionCode);
            Assert.AreEqual(this.posDataRecord.Identifier, constructedMessages[0].ScanCode);
            Assert.AreNotEqual(default(int), constructedMessages[0].ScanCodeId);
            Assert.AreNotEqual(default(int), constructedMessages[0].ScanCodeTypeId);
            Assert.IsFalse(String.IsNullOrWhiteSpace(constructedMessages[0].ScanCodeTypeDesc));
            Assert.AreEqual(CurrencyCodes.Usd, constructedMessages[0].CurrencyCode);
            Assert.AreEqual(posDataRecord.RetailUom, constructedMessages[0].UomCode);
            Assert.AreEqual(this.posDataRecord.Price, constructedMessages[0].Price);
            Assert.AreEqual(this.posDataRecord.Multiple, constructedMessages[0].Multiple);
            Assert.IsNull(constructedMessages[0].SalePrice);
            Assert.IsNull(constructedMessages[0].SaleMultiple);
            Assert.IsNull(constructedMessages[0].SaleStartDate);
            Assert.IsNull(constructedMessages[0].SaleEndDate);
            Assert.IsNull(constructedMessages[0].PreviousSalePrice);
            Assert.IsNull(constructedMessages[0].PreviousSaleStartDate);
            Assert.IsNull(constructedMessages[0].PreviousSaleEndDate);
            Assert.IsNull(constructedMessages[0].PreviousSaleMultiple);
            Assert.IsNull(constructedMessages[0].InProcessBy);
            Assert.IsNull(constructedMessages[0].ProcessedDate);
        }

        [TestMethod]
        public void BuildPriceMessage_RegularPriceOnly_MessageShouldBeConstructedWithNoSaleInformation()
        {
            // Given.
            this.posDataRecord = new TestIrmaPushBuilder()
                .WithBusinessUnitId(this.testBusinessUnitId)
                .WithIdentifier(this.testScanCode)
                .WithChangeType(this.testRegPriceChangeType);

            StagePosData(posDataRecord);
            StageTestItem(nonMerchandiseTraitName: null, departmentSale: false);
            StageTestLocale();

            // When.
            var constructedMessages = messageBuilder.BuildMessages(new List<IRMAPush> { posDataRecord });

            // Then.
            Assert.AreEqual(this.posDataRecord.Price, constructedMessages[0].Price);
            Assert.IsNull(constructedMessages[0].SalePrice);
            Assert.IsNull(constructedMessages[0].SaleMultiple);
            Assert.IsNull(constructedMessages[0].SaleStartDate);
            Assert.IsNull(constructedMessages[0].SaleEndDate);
            Assert.IsNull(constructedMessages[0].PreviousSalePrice);
            Assert.IsNull(constructedMessages[0].PreviousSaleStartDate);
            Assert.IsNull(constructedMessages[0].PreviousSaleEndDate);
            Assert.IsNull(constructedMessages[0].PreviousSaleMultiple);
        }

        [TestMethod]
        public void BuildPriceMessage_RegularPriceWithNewTpr_MessageShouldBeConstructedWithSaleInformation()
        {
            // Given.
            this.posDataRecord = new TestIrmaPushBuilder()
                .WithBusinessUnitId(this.testBusinessUnitId)
                .WithIdentifier(this.testScanCode)
                .WithChangeType(this.testRegPriceChangeType)
                .WithSaleInformation(salePrice: 1.49m, saleMultiple: 1, saleStartDate: DateTime.Now, saleEndDate: DateTime.Now.AddDays(1));

            StagePosData(posDataRecord);
            StageTestItem(nonMerchandiseTraitName: null, departmentSale: false);
            StageTestLocale();

            // When.
            var constructedMessages = messageBuilder.BuildMessages(new List<IRMAPush> { posDataRecord });

            // Then.
            Assert.AreEqual(this.posDataRecord.Price, constructedMessages[0].Price);
            Assert.AreEqual(this.posDataRecord.Sale_Price, constructedMessages[0].SalePrice);
            Assert.AreEqual(this.posDataRecord.SaleMultiple, constructedMessages[0].SaleMultiple);
            Assert.AreEqual(this.posDataRecord.Sale_Start_Date, constructedMessages[0].SaleStartDate);
            Assert.AreEqual(this.posDataRecord.Sale_End_Date, constructedMessages[0].SaleEndDate);
            Assert.IsNull(constructedMessages[0].PreviousSalePrice);
            Assert.IsNull(constructedMessages[0].PreviousSaleStartDate);
            Assert.IsNull(constructedMessages[0].PreviousSaleEndDate);
            Assert.IsNull(constructedMessages[0].PreviousSaleMultiple);
        }

        [TestMethod]
        public void BuildPriceMessage_RegularPriceWithExistingSale_MessageShouldBeConstructedWithSaleAndPreviousSaleInformation()
        {
            // Given.
            this.posDataRecord = new TestIrmaPushBuilder()
                .WithBusinessUnitId(this.testBusinessUnitId)
                .WithIdentifier(this.testScanCode)
                .WithChangeType(this.testNonRegPriceChangeType)
                .WithSaleInformation(salePrice: 1.49m, saleMultiple: 1, saleStartDate: DateTime.Now, saleEndDate: DateTime.Now.AddDays(1));

            StagePosData(posDataRecord);
            StageTestItem(nonMerchandiseTraitName: null, departmentSale: false);
            StageTestLocale();
            StageTestSale();

            // When.
            var constructedMessages = messageBuilder.BuildMessages(new List<IRMAPush> { posDataRecord });

            // Then.
            Assert.AreEqual(this.posDataRecord.Price, constructedMessages[0].Price);
            Assert.AreEqual(this.posDataRecord.Sale_Price, constructedMessages[0].SalePrice);
            Assert.AreEqual(this.posDataRecord.SaleMultiple, constructedMessages[0].SaleMultiple);
            Assert.AreEqual(this.posDataRecord.Sale_Start_Date, constructedMessages[0].SaleStartDate);
            Assert.AreEqual(this.posDataRecord.Sale_End_Date, constructedMessages[0].SaleEndDate);
            Assert.AreEqual(this.testSale.itemPriceAmt, constructedMessages[0].PreviousSalePrice);
            Assert.AreEqual(this.testSale.breakPointStartQty, constructedMessages[0].PreviousSaleMultiple);
            Assert.AreEqual(this.testSale.startDate, constructedMessages[0].PreviousSaleStartDate);
            Assert.AreEqual(this.testSale.endDate, constructedMessages[0].PreviousSaleEndDate);
        }

        [TestMethod]
        public void BuildPriceMessage_ChangeTypeItemLocaleAttributeChange_NoMessageShouldBeConstructed()
        {
            // Given.
            this.posDataRecord = new TestIrmaPushBuilder()
                .WithBusinessUnitId(this.testBusinessUnitId)
                .WithIdentifier(this.testScanCode)
                .WithChangeType(Constants.IrmaPushChangeTypes.ItemLocaleAttributeChange);

            StagePosData(posDataRecord);
            StageTestItem(nonMerchandiseTraitName: null, departmentSale: false);
            StageTestLocale();

            // When.
            var constructedMessages = messageBuilder.BuildMessages(new List<IRMAPush> { posDataRecord });

            // Then.
            Assert.AreEqual(0, constructedMessages.Count);
        }

        [TestMethod]
        public void BuildPriceMessage_ChangeTypeScanCodeDeauthorization_NoMessageShouldBeConstructed()
        {
            // Given.
            this.posDataRecord = new TestIrmaPushBuilder()
                .WithBusinessUnitId(this.testBusinessUnitId)
                .WithIdentifier(this.testScanCode)
                .WithChangeType(Constants.IrmaPushChangeTypes.ScanCodeDeauthorization);

            StagePosData(posDataRecord);
            StageTestItem(nonMerchandiseTraitName: null, departmentSale: false);
            StageTestLocale();

            // When.
            var constructedMessages = messageBuilder.BuildMessages(new List<IRMAPush> { posDataRecord });

            // Then.
            Assert.AreEqual(0, constructedMessages.Count);
        }

        [TestMethod]
        public void BuildPriceMessage_ChangeTypeScanCodeDelete_NoMessageShouldBeConstructed()
        {
            // Given.
            this.posDataRecord = new TestIrmaPushBuilder()
                .WithBusinessUnitId(this.testBusinessUnitId)
                .WithIdentifier(this.testScanCode)
                .WithChangeType(Constants.IrmaPushChangeTypes.ScanCodeDelete);

            StagePosData(posDataRecord);
            StageTestItem(nonMerchandiseTraitName: null, departmentSale: false);
            StageTestLocale();

            // When.
            var constructedMessages = messageBuilder.BuildMessages(new List<IRMAPush> { posDataRecord });

            // Then.
            Assert.AreEqual(0, constructedMessages.Count);
        }

        [TestMethod]
        public void BuildPriceMessages_BusinessUnitIdNotFoundInIcon_MessageShouldNotBeConstructed()
        {
            // Given.
            this.posDataRecord = new TestIrmaPushBuilder().WithBusinessUnitId(this.unknownBusinessUnitId).WithIdentifier(this.testScanCode);

            StagePosData(posDataRecord);
            StageTestItem(nonMerchandiseTraitName: null, departmentSale: false);
            StageTestLocale();

            // When.
            var constructedMessages = messageBuilder.BuildMessages(new List<IRMAPush> { posDataRecord });

            // Then.
            Assert.AreEqual(0, constructedMessages.Count);
        }

        [TestMethod]
        public void BuildPriceMessages_BusinessUnitIdNotFoundInIcon_ErrorShouldBeLogged()
        {
            // Given.
            this.posDataRecord = new TestIrmaPushBuilder().WithBusinessUnitId(this.unknownBusinessUnitId).WithIdentifier(this.testScanCode);

            StagePosData(posDataRecord);
            StageTestItem(nonMerchandiseTraitName: null, departmentSale: false);
            StageTestLocale();

            // When.
            var constructedMessages = messageBuilder.BuildMessages(new List<IRMAPush> { posDataRecord });

            // Then.
            mockMessageBuilderLogger.Verify(l => l.Error(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void BuildPriceMessages_BusinessUnitIdNotFoundInIcon_AlertEmailShouldBeSent()
        {
            // Given.
            this.posDataRecord = new TestIrmaPushBuilder().WithBusinessUnitId(this.unknownBusinessUnitId).WithIdentifier(this.testScanCode);

            StagePosData(posDataRecord);
            StageTestItem(nonMerchandiseTraitName: null, departmentSale: false);
            StageTestLocale();

            // When.
            var constructedMessages = messageBuilder.BuildMessages(new List<IRMAPush> { posDataRecord });

            // Then.
            mockEmailClient.Verify(e => e.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void BuildPriceMessages_BusinessUnitIdNotFoundInIcon_EsbReadyFailedDateShouldBeUpdated()
        {
            // Given.
            this.posDataRecord = new TestIrmaPushBuilder().WithBusinessUnitId(this.unknownBusinessUnitId).WithIdentifier(this.testScanCode);

            StagePosData(posDataRecord);
            StageTestItem(nonMerchandiseTraitName: null, departmentSale: false);
            StageTestLocale();

            // When.
            var constructedMessages = messageBuilder.BuildMessages(new List<IRMAPush> { posDataRecord });

            // Then.
            var updatedPosDataRecord = this.context.Context.IRMAPush.Single(ip => ip.IRMAPushID == posDataRecord.IRMAPushID);
            
            // Have to reload the entity since the price update was done via stored procedure.
            context.Context.Entry(updatedPosDataRecord).Reload();

            Assert.AreEqual(DateTime.Now.Date, updatedPosDataRecord.EsbReadyFailedDate.Value.Date);
            Assert.IsNull(updatedPosDataRecord.EsbReadyDate);
        }

        [TestMethod]
        public void BuildPriceMessages_ScanCodeNotFoundNotFoundInIcon_MessageShouldNotBeConstructed()
        {
            // Given.
            this.posDataRecord = new TestIrmaPushBuilder().WithBusinessUnitId(this.testBusinessUnitId).WithIdentifier(this.unknownScanCode);

            StagePosData(posDataRecord);
            StageTestItem(nonMerchandiseTraitName: null, departmentSale: false);
            StageTestLocale();

            // When.
            var constructedMessages = messageBuilder.BuildMessages(new List<IRMAPush> { posDataRecord });

            // Then.
            Assert.AreEqual(0, constructedMessages.Count);
        }

        [TestMethod]
        public void BuildPriceMessages_ScanCodeNotFoundNotFoundInIcon_ErrorShouldBeLogged()
        {
            // Given.
            this.posDataRecord = new TestIrmaPushBuilder().WithBusinessUnitId(this.testBusinessUnitId).WithIdentifier(this.unknownScanCode);

            StagePosData(posDataRecord);
            StageTestItem(nonMerchandiseTraitName: null, departmentSale: false);
            StageTestLocale();

            // When.
            var constructedMessages = messageBuilder.BuildMessages(new List<IRMAPush> { posDataRecord });

            // Then.
            mockMessageBuilderLogger.Verify(l => l.Error(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void BuildPriceMessages_ScanCodeNotFoundNotFoundInIcon_AlertEmailShouldBeSent()
        {
            // Given.
            this.posDataRecord = new TestIrmaPushBuilder().WithBusinessUnitId(this.testBusinessUnitId).WithIdentifier(this.unknownScanCode);

            StagePosData(posDataRecord);
            StageTestItem(nonMerchandiseTraitName: null, departmentSale: false);
            StageTestLocale();

            // When.
            var constructedMessages = messageBuilder.BuildMessages(new List<IRMAPush> { posDataRecord });

            // Then.
            mockEmailClient.Verify(e => e.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void BuildPriceMessages_ScanCodeNotFoundNotFoundInIcon_EsbReadyFailedDateShouldBeUpdated()
        {
            // Given.
            this.posDataRecord = new TestIrmaPushBuilder().WithBusinessUnitId(this.testBusinessUnitId).WithIdentifier(this.unknownScanCode);

            StagePosData(posDataRecord);
            StageTestItem(nonMerchandiseTraitName: null, departmentSale: false);
            StageTestLocale();

            // When.
            var constructedMessages = messageBuilder.BuildMessages(new List<IRMAPush> { posDataRecord });

            // Then.
            var updatedPosDataRecord = this.context.Context.IRMAPush.Single(ip => ip.IRMAPushID == posDataRecord.IRMAPushID);

            // Have to reload the entity since the price update was done via stored procedure.
            context.Context.Entry(updatedPosDataRecord).Reload();

            Assert.AreEqual(DateTime.Now.Date, updatedPosDataRecord.EsbReadyFailedDate.Value.Date);
            Assert.IsNull(updatedPosDataRecord.EsbReadyDate);
        }

        [TestMethod]
        public void BuildPriceMessages_UnknownChangeType_MessageShouldNotBeConstructed()
        {
            // Given.
            this.posDataRecord = new TestIrmaPushBuilder()
                .WithBusinessUnitId(this.testBusinessUnitId)
                .WithIdentifier(this.testScanCode)
                .WithChangeType(this.unknownChangeType);

            StagePosData(posDataRecord);
            StageTestItem(nonMerchandiseTraitName: null, departmentSale: false);
            StageTestLocale();

            // When.
            var constructedMessages = messageBuilder.BuildMessages(new List<IRMAPush> { posDataRecord });

            // Then.
            Assert.AreEqual(0, constructedMessages.Count);
        }

        [TestMethod]
        public void BuildPriceMessages_UnknownChangeType_ErrorShouldBeLogged()
        {
            // Given.
            this.posDataRecord = new TestIrmaPushBuilder()
                .WithBusinessUnitId(this.testBusinessUnitId)
                .WithIdentifier(this.testScanCode)
                .WithChangeType(this.unknownChangeType);

            StagePosData(posDataRecord);
            StageTestItem(nonMerchandiseTraitName: null, departmentSale: false);
            StageTestLocale();

            // When.
            var constructedMessages = messageBuilder.BuildMessages(new List<IRMAPush> { posDataRecord });

            // Then.
            mockMessageBuilderLogger.Verify(l => l.Error(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void BuildPriceMessages_UnknownChangeType_AlertEmailShouldBeSent()
        {
            // Given.
            this.posDataRecord = new TestIrmaPushBuilder()
                .WithBusinessUnitId(this.testBusinessUnitId)
                .WithIdentifier(this.testScanCode)
                .WithChangeType(this.unknownChangeType);

            StagePosData(posDataRecord);
            StageTestItem(nonMerchandiseTraitName: null, departmentSale: false);
            StageTestLocale();

            // When.
            var constructedMessages = messageBuilder.BuildMessages(new List<IRMAPush> { posDataRecord });

            // Then.
            mockEmailClient.Verify(e => e.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void BuildPriceMessages_UnknownChangeType_EsbReadyFailedDateShouldBeUpdated()
        {
            // Given.
            this.posDataRecord = new TestIrmaPushBuilder()
                .WithBusinessUnitId(this.testBusinessUnitId)
                .WithIdentifier(this.testScanCode)
                .WithChangeType(this.unknownChangeType);

            StagePosData(posDataRecord);
            StageTestItem(nonMerchandiseTraitName: null, departmentSale: false);
            StageTestLocale();

            // When.
            var constructedMessages = messageBuilder.BuildMessages(new List<IRMAPush> { posDataRecord });

            // Then.
            var updatedPosDataRecord = this.context.Context.IRMAPush.Single(ip => ip.IRMAPushID == posDataRecord.IRMAPushID);

            // Have to reload the entity since the price update was done via stored procedure.
            context.Context.Entry(updatedPosDataRecord).Reload();

            Assert.AreEqual(DateTime.Now.Date, updatedPosDataRecord.EsbReadyFailedDate.Value.Date);
            Assert.IsNull(updatedPosDataRecord.EsbReadyDate);
        }

        [TestMethod]
        public void BuildPriceMessages_ItemIsAssociatedToCouponSubBrick_MessageShouldNotBeBuilt()
        {
            // Given.
            this.posDataRecord = new TestIrmaPushBuilder()
                .WithBusinessUnitId(this.testBusinessUnitId)
                .WithIdentifier(this.testScanCode)
                .WithChangeType(this.unknownChangeType);

            StagePosData(posDataRecord);
            StageTestItem(NonMerchandiseTraits.Coupon, departmentSale: false);
            StageTestLocale();

            // When.
            var constructedMessages = messageBuilder.BuildMessages(new List<IRMAPush> { posDataRecord });

            // Then.
            Assert.AreEqual(0, constructedMessages.Count);
        }

        [TestMethod]
        public void BuildPriceMessages_ItemHasDepartmentSaleTrait_MessageShouldNotBeBuilt()
        {
            // Given.
            this.posDataRecord = new TestIrmaPushBuilder()
                .WithBusinessUnitId(this.testBusinessUnitId)
                .WithIdentifier(this.testScanCode)
                .WithChangeType(this.unknownChangeType);

            StagePosData(posDataRecord);
            StageTestItem(nonMerchandiseTraitName: null, departmentSale: true);
            StageTestLocale();

            // When.
            var constructedMessages = messageBuilder.BuildMessages(new List<IRMAPush> { posDataRecord });

            // Then.
            Assert.AreEqual(0, constructedMessages.Count);
        }

        [TestMethod]
        public void BuildPriceMessages_ItemIsAssociatedToLegacyPosOnlySubBrick_MessageShouldNotBeBuilt()
        {
            // Given.
            this.posDataRecord = new TestIrmaPushBuilder()
                .WithBusinessUnitId(this.testBusinessUnitId)
                .WithIdentifier(this.testScanCode)
                .WithChangeType(this.unknownChangeType);

            StagePosData(posDataRecord);
            StageTestItem(NonMerchandiseTraits.LegacyPosOnly, departmentSale: true);
            StageTestLocale();

            // When.
            var constructedMessages = messageBuilder.BuildMessages(new List<IRMAPush> { posDataRecord });

            // Then.
            Assert.AreEqual(0, constructedMessages.Count);
        }

        [TestMethod]
        public void BuildPriceMessages_CancelAllSalesMessage_MessageShouldBeConstructedWithCancelAllSalesChangeType()
        {
            // Given.
            this.posDataRecord = new TestIrmaPushBuilder()
                .WithBusinessUnitId(this.testBusinessUnitId)
                .WithIdentifier(this.testScanCode)
                .WithChangeType(this.cancelAllSalesChangeType);

            StagePosData(posDataRecord);
            StageTestItem(nonMerchandiseTraitName: String.Empty, departmentSale: false);
            StageTestLocale();
            StageTestSale();

            // When.
            var constructedMessages = messageBuilder.BuildMessages(new List<IRMAPush> { posDataRecord }).Single();

            // Then.
            Assert.AreEqual(this.cancelAllSalesChangeType, constructedMessages.ChangeType);
        }

        [TestMethod]
        public void BuildPriceMessages_CancelAllSalesMessageWithNullPreviousSale_MessageShouldBeNotBeConstructed()
        {
            // Given.
            this.posDataRecord = new TestIrmaPushBuilder()
                .WithBusinessUnitId(this.testBusinessUnitId)
                .WithIdentifier(this.testScanCode)
                .WithChangeType(this.cancelAllSalesChangeType);

            StagePosData(posDataRecord);
            StageTestItem(nonMerchandiseTraitName: String.Empty, departmentSale: false);
            StageTestLocale();
            
            // When.
            var constructedMessages = messageBuilder.BuildMessages(new List<IRMAPush> { posDataRecord }).SingleOrDefault();

            // Then.
            Assert.IsNull(constructedMessages);
        }

        [TestMethod]
        public void BuildPriceMessages_CancelAllSalesMessageWithExpiredSaleEndDate_MessageShouldBeNotBeConstructed()
        {
            // Given.
            this.posDataRecord = new TestIrmaPushBuilder()
                .WithBusinessUnitId(this.testBusinessUnitId)
                .WithIdentifier(this.testScanCode)
                .WithChangeType(this.cancelAllSalesChangeType);

            StagePosData(posDataRecord);
            StageTestItem(nonMerchandiseTraitName: String.Empty, departmentSale: false);
            StageTestLocale();
            StageTestSale(expired: true);

            // When.
            var constructedMessages = messageBuilder.BuildMessages(new List<IRMAPush> { posDataRecord }).SingleOrDefault();

            // Then.
            Assert.IsNull(constructedMessages);
        }
    }
}
