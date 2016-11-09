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
    public class ItemLocaleMessageBuilderTests
    {
        private GlobalIconContext context;
        private DbContextTransaction transaction;
        private IRMAPush posDataRecord;
        private ItemLocaleMessageBuilder messageBuilder;
        private Mock<ILogger<ItemLocaleMessageBuilder>> mockMessageBuilderLogger;
        private Mock<IEmailClient> mockEmailClient;
        private ScanCodeCacheHelper scanCodeCacheHelper;
        private LinkedScanCodeCacheHelper linkedScanCodeCacheHelper;
        private LocaleCacheHelper localeCacheHelper;
        private Mock<ILogger<GetScanCodesByIdentifierBulkQueryHandler>> mockGetScanCodesLogger;
        private Mock<ILogger<GetCurrentLinkedScanCodesQueryHandler>> mockGetCurrentLinkedScanCodesLogger;
        private GetLocalesByBusinessUnitIdQueryHandler getLocaleQueryHandler;
        private UpdateStagingTableDatesForEsbCommandHandler updateStagingTableDatesForEsbCommandHandler;
        private Item testItem;
        private Item testLinkedItem;
        private HierarchyClass testNonMerchandiseClass;
        private Locale testLocale;
        private string testScanCode;
        private string unknownScanCode;
        private int testBusinessUnitId;
        private int unknownBusinessUnitId;
        private string testChangeType;
        private string unknownChangeType;
        private string testLinkedIdentifier;
        private string testPreviousLinkedIdentifier;

        [TestInitialize]
        public void Initialize()
        {
            this.testScanCode = "2222222";
            this.unknownScanCode = "2222221";
            this.testBusinessUnitId = 88888;
            this.unknownBusinessUnitId = 99999;
            this.testChangeType = Constants.IrmaPushChangeTypes.ScanCodeAdd;
            this.unknownChangeType = "Unknown";
            this.testLinkedIdentifier = "2222111";
            this.testPreviousLinkedIdentifier = "2222112";

            this.context = new GlobalIconContext(new IconContext());

            this.mockMessageBuilderLogger = new Mock<ILogger<ItemLocaleMessageBuilder>>();
            this.mockEmailClient = new Mock<IEmailClient>();
            this.mockGetScanCodesLogger = new Mock<ILogger<GetScanCodesByIdentifierBulkQueryHandler>>();
            this.mockGetCurrentLinkedScanCodesLogger = new Mock<ILogger<GetCurrentLinkedScanCodesQueryHandler>>();
            this.scanCodeCacheHelper = new ScanCodeCacheHelper(new GetScanCodesByIdentifierBulkQueryHandler(mockGetScanCodesLogger.Object, context));
            this.linkedScanCodeCacheHelper = new LinkedScanCodeCacheHelper(new GetCurrentLinkedScanCodesQueryHandler(mockGetCurrentLinkedScanCodesLogger.Object, context));
            this.localeCacheHelper = new LocaleCacheHelper(new GetLocalesByBusinessUnitIdQueryHandler(context));
            this.getLocaleQueryHandler = new GetLocalesByBusinessUnitIdQueryHandler(context);
            this.updateStagingTableDatesForEsbCommandHandler = new UpdateStagingTableDatesForEsbCommandHandler(new Mock<ILogger<UpdateStagingTableDatesForEsbCommandHandler>>().Object, context);

            this.messageBuilder = new ItemLocaleMessageBuilder(
                mockMessageBuilderLogger.Object,
                mockEmailClient.Object,
                scanCodeCacheHelper,
                linkedScanCodeCacheHelper,
                localeCacheHelper,
                getLocaleQueryHandler,
                updateStagingTableDatesForEsbCommandHandler);

            Cache.identifierToScanCode.Clear();
            Cache.businessUnitToLocale.Clear();
            Cache.scanCodeByBusinessUnitToLinkedScanCode.Clear();

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

        private void StageTestLinkedItem(string nonMerchandiseTraitName, bool validated)
        {
            this.testNonMerchandiseClass = new TestHierarchyClassBuilder().WithNonMerchandiseTrait(nonMerchandiseTraitName);
            this.context.Context.HierarchyClass.Add(this.testNonMerchandiseClass);
            this.context.Context.SaveChanges();

            if (validated)
            {
                this.testLinkedItem = new TestItemBuilder()
                    .WithScanCode(this.testLinkedIdentifier)
                    .WithSubBrickAssociation(this.testNonMerchandiseClass.hierarchyClassID)
                    .WithValidationDate(DateTime.Now.ToString());
            }
            else
            {
                this.testLinkedItem = new TestItemBuilder().WithScanCode(this.testLinkedIdentifier).WithSubBrickAssociation(this.testNonMerchandiseClass.hierarchyClassID);
            }

            this.testLinkedItem.ItemType = this.context.Context.ItemType.Single(it => it.itemTypeCode == ItemTypeCodes.Deposit);
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
                ValidationDate = (validated ? DateTime.Now.ToString() : null),
                NonMerchandiseTrait = nonMerchandiseTraitName
            };

            Cache.identifierToScanCode.Add(scanCodeModel.ScanCode, scanCodeModel);
        }

        [TestMethod]
        public void BuildItemLocaleMessages_ValidPosDataRecord_MessageShouldBeConstructedWithCorrectValues()
        {
            // Given.
            this.posDataRecord = new TestIrmaPushBuilder()
                .WithBusinessUnitId(this.testBusinessUnitId)
                .WithIdentifier(this.testScanCode)
                .WithChangeType(this.testChangeType);

            StagePosData(posDataRecord);
            StageTestItem(nonMerchandiseTraitName: null, departmentSale: false);
            StageTestLocale();

            Cache.scanCodeByBusinessUnitToLinkedScanCode.Add(new Tuple<string, int>(testScanCode, testBusinessUnitId), null);

            // When.
            var constructedMessages = messageBuilder.BuildMessages(new List<IRMAPush> { posDataRecord });

            // Then.
            Assert.AreEqual(1, constructedMessages.Count);
            Assert.AreEqual(this.posDataRecord.IRMAPushID, constructedMessages[0].IRMAPushID);
            Assert.AreEqual(MessageActionTypes.AddOrUpdate, constructedMessages[0].MessageActionId);
            Assert.IsNull(constructedMessages[0].MessageHistoryId);
            Assert.AreEqual(MessageStatusTypes.Ready, constructedMessages[0].MessageStatusId);
            Assert.AreEqual(this.posDataRecord.Identifier, constructedMessages[0].ScanCode);
            Assert.AreEqual(this.posDataRecord.BusinessUnit_ID, constructedMessages[0].BusinessUnit_ID);
            Assert.AreEqual(this.posDataRecord.AgeCode, constructedMessages[0].AgeCode);
            Assert.AreEqual(this.posDataRecord.Case_Discount, constructedMessages[0].Case_Discount);
            Assert.AreEqual(this.posDataRecord.ChangeType, constructedMessages[0].ChangeType);
            Assert.AreEqual(this.posDataRecord.LinkedIdentifier, constructedMessages[0].LinkedItemScanCode);
            Assert.AreEqual(this.posDataRecord.PosScaleTare, constructedMessages[0].PosScaleTare);
            Assert.AreEqual(this.posDataRecord.QtyProhibit, constructedMessages[0].QtyProhibit);
            Assert.AreEqual(this.posDataRecord.Quantity_Required, constructedMessages[0].Quantity_Required);
            Assert.AreEqual(this.posDataRecord.Recall_Flag, constructedMessages[0].Recall);
            Assert.AreEqual(this.posDataRecord.RegionCode, constructedMessages[0].RegionCode);
            Assert.AreEqual(this.posDataRecord.Restricted_Hours, constructedMessages[0].Restricted_Hours);
            Assert.AreEqual(this.posDataRecord.RestrictSale, constructedMessages[0].LockedForSale);
            Assert.AreEqual(this.posDataRecord.ScaleForcedTare, constructedMessages[0].ScaleForcedTare);
            Assert.AreEqual(this.posDataRecord.Sold_By_Weight, constructedMessages[0].Sold_By_Weight);
            Assert.AreEqual(this.posDataRecord.TMDiscountEligible, constructedMessages[0].TMDiscountEligible);
            Assert.AreEqual(this.posDataRecord.VisualVerify, constructedMessages[0].VisualVerify);
            Assert.IsNull(constructedMessages[0].PreviousLinkedItemScanCode);
            Assert.IsNull(constructedMessages[0].InProcessBy);
            Assert.IsNull(constructedMessages[0].ProcessedDate);
        }

        [TestMethod]
        public void BuildItemLocaleMessages_ChangeTypeItemLocaleAttributeChange_MessageShouldBeConstructedWithAddOrUpdateAction()
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
            Assert.AreEqual(MessageActionTypes.AddOrUpdate, constructedMessages[0].MessageActionId);
        }

        [TestMethod]
        public void BuildItemLocaleMessages_ChangeTypeScanCodeAdd_MessageShouldBeConstructedWithAddOrUpdateAction()
        {
            // Given.
            this.posDataRecord = new TestIrmaPushBuilder()
                .WithBusinessUnitId(this.testBusinessUnitId)
                .WithIdentifier(this.testScanCode)
                .WithChangeType(Constants.IrmaPushChangeTypes.ScanCodeAdd);

            StagePosData(posDataRecord);
            StageTestItem(nonMerchandiseTraitName: null, departmentSale: false);
            StageTestLocale();

            // When.
            var constructedMessages = messageBuilder.BuildMessages(new List<IRMAPush> { posDataRecord });

            // Then.
            Assert.AreEqual(MessageActionTypes.AddOrUpdate, constructedMessages[0].MessageActionId);
        }

        [TestMethod]
        public void BuildItemLocaleMessages_ChangeTypeScanCodeAuthorization_MessageShouldBeConstructedWithAddOrUpdateAction()
        {
            // Given.
            this.posDataRecord = new TestIrmaPushBuilder()
                .WithBusinessUnitId(this.testBusinessUnitId)
                .WithIdentifier(this.testScanCode)
                .WithChangeType(Constants.IrmaPushChangeTypes.ScanCodeAuthorization);

            StagePosData(posDataRecord);
            StageTestItem(nonMerchandiseTraitName: null, departmentSale: false);
            StageTestLocale();

            // When.
            var constructedMessages = messageBuilder.BuildMessages(new List<IRMAPush> { posDataRecord });

            // Then.
            Assert.AreEqual(MessageActionTypes.AddOrUpdate, constructedMessages[0].MessageActionId);
        }

        [TestMethod]
        public void BuildItemLocaleMessages_ChangeTypeScanCodeDeauthorization_MessageShouldBeConstructedWithDeleteAction()
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
            Assert.AreEqual(MessageActionTypes.Delete, constructedMessages[0].MessageActionId);
        }

        [TestMethod]
        public void BuildItemLocaleMessages_ChangeTypeScanCodeDelete_MessageShouldBeConstructedWithDeleteAction()
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
            Assert.AreEqual(MessageActionTypes.Delete, constructedMessages[0].MessageActionId);
        }

        [TestMethod]
        public void BuildItemLocaleMessages_ChangeTypeRegularPriceChange_NoMessageShouldBeConstructed()
        {
            // Given.
            this.posDataRecord = new TestIrmaPushBuilder()
                .WithBusinessUnitId(this.testBusinessUnitId)
                .WithIdentifier(this.testScanCode)
                .WithChangeType(Constants.IrmaPushChangeTypes.RegularPriceChange);

            StagePosData(posDataRecord);
            StageTestItem(nonMerchandiseTraitName: null, departmentSale: false);
            StageTestLocale();

            // When.
            var constructedMessages = messageBuilder.BuildMessages(new List<IRMAPush> { posDataRecord });

            // Then.
            Assert.AreEqual(0, constructedMessages.Count);
        }

        [TestMethod]
        public void BuildItemLocaleMessages_ChangeTypeNonRegularPriceChange_NoMessageShouldBeConstructed()
        {
            // Given.
            this.posDataRecord = new TestIrmaPushBuilder()
                .WithBusinessUnitId(this.testBusinessUnitId)
                .WithIdentifier(this.testScanCode)
                .WithChangeType(Constants.IrmaPushChangeTypes.NonRegularPriceChange);

            StagePosData(posDataRecord);
            StageTestItem(nonMerchandiseTraitName: null, departmentSale: false);
            StageTestLocale();

            // When.
            var constructedMessages = messageBuilder.BuildMessages(new List<IRMAPush> { posDataRecord });

            // Then.
            Assert.AreEqual(0, constructedMessages.Count);
        }

        [TestMethod]
        public void BuildItemLocaleMessages_NoLinkedIdentifier_MessageShouldBeConstructedWithNullLinkedScanCode()
        {
            // Given.
            this.posDataRecord = new TestIrmaPushBuilder()
                .WithBusinessUnitId(this.testBusinessUnitId)
                .WithIdentifier(this.testScanCode)
                .WithChangeType(this.testChangeType);

            StagePosData(posDataRecord);
            StageTestItem(nonMerchandiseTraitName: null, departmentSale: false);
            StageTestLocale();

            // When.
            var constructedMessages = messageBuilder.BuildMessages(new List<IRMAPush> { posDataRecord });

            // Then.
            Assert.AreEqual(this.posDataRecord.LinkedIdentifier, constructedMessages[0].LinkedItemScanCode);
        }

        [TestMethod]
        public void BuildItemLocaleMessages_LinkedIdentifierIsBottleDepositAndValidated_MessageShouldBeConstructedWithLinkedScanCode()
        {
            // Given.
            this.posDataRecord = new TestIrmaPushBuilder()
                .WithBusinessUnitId(this.testBusinessUnitId)
                .WithIdentifier(this.testScanCode)
                .WithChangeType(this.testChangeType)
                .WithLinkedIdentifier(this.testLinkedIdentifier);

            StagePosData(posDataRecord);
            StageTestItem(nonMerchandiseTraitName: null, departmentSale: false);
            StageTestLocale();
            StageTestLinkedItem(NonMerchandiseTraits.BottleDeposit, validated: true);

            // When.
            var constructedMessages = messageBuilder.BuildMessages(new List<IRMAPush> { posDataRecord });

            // Then.
            Assert.AreEqual(posDataRecord.LinkedIdentifier, constructedMessages[0].LinkedItemScanCode);
        }

        [TestMethod]
        public void BuildItemLocaleMessages_LinkedIdentifierIsCrvAndValidated_MessageShouldBeConstructedWithLinkedScanCode()
        {
            // Given.
            this.posDataRecord = new TestIrmaPushBuilder()
                .WithBusinessUnitId(this.testBusinessUnitId)
                .WithIdentifier(this.testScanCode)
                .WithChangeType(this.testChangeType)
                .WithLinkedIdentifier(this.testLinkedIdentifier);

            StagePosData(posDataRecord);
            StageTestItem(nonMerchandiseTraitName: null, departmentSale: false);
            StageTestLocale();
            StageTestLinkedItem(NonMerchandiseTraits.Crv, validated: true);

            // When.
            var constructedMessages = messageBuilder.BuildMessages(new List<IRMAPush> { posDataRecord });

            // Then.
            Assert.AreEqual(posDataRecord.LinkedIdentifier, constructedMessages[0].LinkedItemScanCode);
        }

        [TestMethod]
        public void BuildItemLocaleMessages_LinkedIdentifierIsFeeAndValidated_MessageShouldBeConstructedWithLinkedScanCode()
        {
            // Given.
            this.posDataRecord = new TestIrmaPushBuilder()
                .WithBusinessUnitId(this.testBusinessUnitId)
                .WithIdentifier(this.testScanCode)
                .WithChangeType(this.testChangeType)
                .WithLinkedIdentifier(this.testLinkedIdentifier);

            StagePosData(posDataRecord);
            StageTestItem(nonMerchandiseTraitName: null, departmentSale: false);
            StageTestLocale();
            StageTestLinkedItem(NonMerchandiseTraits.BlackhawkFee, validated: true);

            // When.
            var constructedMessages = messageBuilder.BuildMessages(new List<IRMAPush> { posDataRecord });

            // Then.
            Assert.AreEqual(posDataRecord.LinkedIdentifier, constructedMessages[0].LinkedItemScanCode);
        }

        [TestMethod]
        public void BuildItemLocaleMessages_LinkedIdentifierIsNotValidated_MessageShouldBeConstructedWithoutLinkedScanCode()
        {
            // Given.
            this.posDataRecord = new TestIrmaPushBuilder()
                .WithBusinessUnitId(this.testBusinessUnitId)
                .WithIdentifier(this.testScanCode)
                .WithChangeType(this.testChangeType)
                .WithLinkedIdentifier(this.testLinkedIdentifier);

            StagePosData(posDataRecord);
            StageTestItem(nonMerchandiseTraitName: null, departmentSale: false);
            StageTestLocale();
            StageTestLinkedItem(NonMerchandiseTraits.Crv, validated: false);

            // When.
            var constructedMessages = messageBuilder.BuildMessages(new List<IRMAPush> { posDataRecord });

            // Then.
            Assert.IsNull(constructedMessages[0].LinkedItemScanCode);
        }

        [TestMethod]
        public void BuildItemLocaleMessages_LinkedIdentifierIsNotDepositCrvOrFee_MessageShouldBeConstructedWithoutLinkedScanCode()
        {
            // Given.
            this.posDataRecord = new TestIrmaPushBuilder()
                .WithBusinessUnitId(this.testBusinessUnitId)
                .WithIdentifier(this.testScanCode)
                .WithChangeType(this.testChangeType)
                .WithLinkedIdentifier(this.testLinkedIdentifier);

            StagePosData(posDataRecord);
            StageTestItem(nonMerchandiseTraitName: null, departmentSale: false);
            StageTestLocale();
            StageTestLinkedItem(null, validated: false);

            // When.
            var constructedMessages = messageBuilder.BuildMessages(new List<IRMAPush> { posDataRecord });

            // Then.
            Assert.IsNull(constructedMessages[0].LinkedItemScanCode);
        }

        [TestMethod]
        public void BuildItemLocaleMessages_ItemHasPreviousLinkedIdentifier_MessageShouldBeConstructedWithPreviousLinkedScanCode()
        {
            // Given.
            this.posDataRecord = new TestIrmaPushBuilder()
                .WithBusinessUnitId(this.testBusinessUnitId)
                .WithIdentifier(this.testScanCode)
                .WithChangeType(this.testChangeType)
                .WithLinkedIdentifier(this.testLinkedIdentifier);

            StagePosData(posDataRecord);
            StageTestItem(nonMerchandiseTraitName: null, departmentSale: false);
            StageTestLocale();
            StageTestLinkedItem(NonMerchandiseTraits.BlackhawkFee, validated: true);

            this.testItem.ItemTrait.Add(new ItemTrait
            {
                traitID = Traits.LinkedScanCode,
                traitValue = this.testPreviousLinkedIdentifier,
                Locale = this.testLocale
            });
            context.Context.SaveChanges();

            Cache.scanCodeByBusinessUnitToLinkedScanCode.Add(new Tuple<string, int>(testScanCode, testBusinessUnitId), testPreviousLinkedIdentifier);

            // When.
            var constructedMessages = messageBuilder.BuildMessages(new List<IRMAPush> { posDataRecord });

            // Then.
            Assert.AreEqual(posDataRecord.LinkedIdentifier, constructedMessages[0].LinkedItemScanCode);
            Assert.AreEqual(testPreviousLinkedIdentifier, constructedMessages[0].PreviousLinkedItemScanCode);
        }

        [TestMethod]
        public void BuildItemLocaleMessages_LinkedIdentifierExistsAndNoPreviousLinkedIdentifierExists_MessageShouldBeConstructedWithNullPreviousLinkedScanCode()
        {
            // Given.
            this.posDataRecord = new TestIrmaPushBuilder()
                .WithBusinessUnitId(this.testBusinessUnitId)
                .WithIdentifier(this.testScanCode)
                .WithChangeType(this.testChangeType)
                .WithLinkedIdentifier(this.testLinkedIdentifier);

            StagePosData(posDataRecord);
            StageTestItem(nonMerchandiseTraitName: null, departmentSale: false);
            StageTestLocale();
            StageTestLinkedItem(NonMerchandiseTraits.BlackhawkFee, validated: true);

            // When.
            var constructedMessages = messageBuilder.BuildMessages(new List<IRMAPush> { posDataRecord });

            // Then.
            Assert.AreEqual(posDataRecord.LinkedIdentifier, constructedMessages[0].LinkedItemScanCode);
            Assert.AreEqual(null, constructedMessages[0].PreviousLinkedItemScanCode);
        }

        [TestMethod]
        public void BuildItemLocaleMessages_BusinessUnitIdNotFoundInIcon_MessageShouldNotBeConstructed()
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
        public void BuildItemLocaleMessages_BusinessUnitIdNotFoundInIcon_ErrorShouldBeLogged()
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
        public void BuildItemLocaleMessages_BusinessUnitIdNotFoundInIcon_AlertEmailShouldBeSent()
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
        public void BuildItemLocaleMessages_BusinessUnitIdNotFoundInIcon_EsbReadyFailedDateShouldBeUpdated()
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
        public void BuildItemLocaleMessages_ScanCodeNotFoundNotFoundInIcon_MessageShouldNotBeConstructed()
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
        public void BuildItemLocaleMessages_ScanCodeNotFoundNotFoundInIcon_ErrorShouldBeLogged()
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
        public void BuildItemLocaleMessages_ScanCodeNotFoundNotFoundInIcon_AlertEmailShouldBeSent()
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
        public void BuildItemLocaleMessages_ScanCodeNotFoundNotFoundInIcon_EsbReadyFailedDateShouldBeUpdated()
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
        public void BuildItemLocaleMessages_UnknownChangeType_MessageShouldNotBeConstructed()
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
        public void BuildItemLocaleMessages_UnknownChangeType_ErrorShouldBeLogged()
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
        public void BuildItemLocaleMessages_UnknownChangeType_AlertEmailShouldBeSent()
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
        public void BuildItemLocaleMessages_UnknownChangeType_EsbReadyFailedDateShouldBeUpdated()
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
        public void BuildItemLocaleMessages_ItemIsAssociatedToCouponSubBrick_MessageShouldNotBeBuilt()
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
        public void BuildItemLocaleMessages_ItemHasDepartmentSaleTrait_MessageShouldNotBeBuilt()
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
        public void BuildItemLocaleMessages_ItemIsAssociatedToLegacyPosOnlySubBrick_MessageShouldNotBeBuilt()
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
    }
}
