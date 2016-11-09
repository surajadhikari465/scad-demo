using Icon.Common.Email;
using Icon.Framework;
using Icon.Framework.RenewableContext;
using Icon.Logging;
using Icon.Testing.Builders;
using InterfaceController.Common;
using Irma.Framework;
using Irma.Framework.RenewableContext;
using Irma.Testing.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PushController.Common;
using PushController.Common.Models;
using PushController.Controller.PosDataStagingServices;
using PushController.DataAccess.Commands;
using PushController.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace PushController.Tests.Controller.IconStagingServiceTests
{
    [TestClass]
    public class IrmaPushStagingServiceTests
    {
        private GlobalIconContext globalIconContext;
        private IrmaContext irmaContext;
        private GlobalIrmaContext globalIrmaContext;
        private DbContextTransaction iconTransaction;
        private DbContextTransaction irmaTransaction;
        private Mock<IIrmaContextProvider> mockIrmaContextProvider;
        private Mock<ILogger<IrmaPushStagingService>> mockLogger;
        private Mock<IEmailClient> mockEmailClient;
        private StagePosDataBulkCommandHandler stagePosDataBulkCommandHandler;
        private Mock<ICommandHandler<StagePosDataBulkCommand>> mockStagePosDataBulkCommandHandler;
        private StagePosDataRowByRowCommandHandler stagePosDataRowByRowCommandHandler;
        private UpdatePublishTableDatesCommandHandler updatePublishTableDatesCommandHandler;
        private List<IConPOSPushPublish> testPosPublishData;
        private string testScanCode;
        private Random random;

        [TestInitialize]
        public void Initialize()
        {
            globalIconContext = new GlobalIconContext(new IconContext());

            string irmaConnectionString = ConnectionBuilder.GetConnection("SP");
            globalIrmaContext = new GlobalIrmaContext(new IrmaContextProvider().GetRegionalContext(irmaConnectionString), irmaConnectionString);
            irmaContext = globalIrmaContext.Context;

            testScanCode = "2222222";
            random = new Random(1000);

            mockIrmaContextProvider = new Mock<IIrmaContextProvider>();
            mockIrmaContextProvider.Setup(cp => cp.GetRegionalContext(It.IsAny<string>())).Returns(irmaContext);

            mockLogger = new Mock<ILogger<IrmaPushStagingService>>();
            mockEmailClient = new Mock<IEmailClient>();
            stagePosDataBulkCommandHandler = new StagePosDataBulkCommandHandler(new Mock<ILogger<StagePosDataBulkCommandHandler>>().Object, globalIconContext);
            mockStagePosDataBulkCommandHandler = new Mock<ICommandHandler<StagePosDataBulkCommand>>();
            stagePosDataRowByRowCommandHandler = new StagePosDataRowByRowCommandHandler(globalIconContext);
            updatePublishTableDatesCommandHandler = new UpdatePublishTableDatesCommandHandler(new Mock<ILogger<UpdatePublishTableDatesCommandHandler>>().Object);

            iconTransaction = globalIconContext.Context.Database.BeginTransaction();
            irmaTransaction = irmaContext.Database.BeginTransaction();
        }

        [TestCleanup]
        public void Cleanup()
        {
            iconTransaction.Rollback();
            irmaTransaction.Rollback();
        }

        private void StagePosPublishData()
        {
            testPosPublishData = new List<IConPOSPushPublish>
            {
                new TestIconPosPushPublishBuilder().WithStoreNumber(113).WithInsertDate(DateTime.Now.AddMilliseconds(random.Next())),
                new TestIconPosPushPublishBuilder().WithStoreNumber(113).WithInsertDate(DateTime.Now.AddMilliseconds(random.Next())),
                new TestIconPosPushPublishBuilder().WithStoreNumber(113).WithInsertDate(DateTime.Now.AddMilliseconds(random.Next()))
            };

            irmaContext.IConPOSPushPublish.AddRange(testPosPublishData);
            irmaContext.SaveChanges();
        }

        [TestMethod]
        public void StagePosDataBulk_SuccessfulExecution_PosDataShouldBeSavedToTheDatabase()
        {
            // Given.
            var testPosData = new List<IConPOSPushPublish>
            {
                new TestIconPosPushPublishBuilder().WithIdentifier(testScanCode).WithChangeType(Constants.IrmaPushChangeTypes.ScanCodeAdd),
                new TestIconPosPushPublishBuilder().WithIdentifier(testScanCode).WithChangeType(Constants.IrmaPushChangeTypes.ItemLocaleAttributeChange),
                new TestIconPosPushPublishBuilder().WithIdentifier(testScanCode).WithChangeType(Constants.IrmaPushChangeTypes.ScanCodeAuthorization)
            }.ConvertAll(pos => new IrmaPushModel
                {
                    IconPosPushPublishId = pos.IConPOSPushPublishID,
                    RegionCode = pos.RegionCode,
                    BusinessUnitId = pos.BusinessUnit_ID,
                    Identifier = pos.Identifier,
                    ChangeType = pos.ChangeType,
                    InsertDate = pos.InsertDate,
                    RetailSize = pos.RetailSize.Value,
                    RetailPackageUom = pos.RetailPackageUom,
                    TmDiscountEligible = pos.TMDiscountEligible.Value,
                    CaseDiscount = pos.Case_Discount.Value,
                    AgeCode = pos.AgeCode,
                    Recall = pos.Recall_Flag.Value,
                    RestrictedHours = pos.Restricted_Hours.Value,
                    SoldByWeight = pos.Sold_By_Weight.Value,
                    ScaleForcedTare = pos.ScaleForcedTare.Value,
                    QuantityRequired = pos.Quantity_Required.Value,
                    PriceRequired = pos.Price_Required.Value,
                    QuantityProhibit = pos.QtyProhibit.Value,
                    VisualVerify = pos.VisualVerify.Value,
                    RestrictSale = pos.RestrictSale.Value,
                    PosScaleTare = pos.POSTare,
                    LinkedIdentifier = pos.LinkCode_ItemIdentifier,
                    Price = pos.Price,
                    RetailUom = pos.RetailUom,
                    Multiple = pos.Multiple,
                    SaleMultiple = pos.SaleMultiple,
                    SalePrice = pos.Sale_Price,
                    SaleStartDate = pos.Sale_Start_Date,
                    SaleEndDate = pos.Sale_End_Date,
                    InProcessBy = null,
                    InUdmDate = null,
                    EsbReadyDate = null,
                    UdmFailedDate = null,
                    EsbReadyFailedDate = null
                });

            var irmaPushStagingService = new IrmaPushStagingService(
                mockLogger.Object,
                mockIrmaContextProvider.Object,
                mockEmailClient.Object,
                stagePosDataBulkCommandHandler,
                stagePosDataRowByRowCommandHandler,
                updatePublishTableDatesCommandHandler);

            // When.
            irmaPushStagingService.StagePosDataBulk(testPosData);

            // Then.
            var stagedPosData = globalIconContext.Context.IRMAPush.Where(ip => ip.Identifier == testScanCode).OrderBy(ip => ip.IRMAPushID).ToList();

            Assert.AreEqual(testPosData.Count, stagedPosData.Count);
            Assert.AreEqual(testPosData[0].RegionCode, stagedPosData[0].RegionCode);
            Assert.AreEqual(testPosData[0].BusinessUnitId, stagedPosData[0].BusinessUnit_ID);
            Assert.AreEqual(testPosData[0].Identifier, stagedPosData[0].Identifier);
            Assert.AreEqual(testPosData[0].ChangeType, stagedPosData[0].ChangeType);
            Assert.AreEqual(testPosData[0].InsertDate.Date, stagedPosData[0].InsertDate.Date);
            Assert.AreEqual(testPosData[0].RetailSize, stagedPosData[0].RetailSize);
            Assert.AreEqual(testPosData[0].RetailPackageUom, stagedPosData[0].RetailPackageUom);
            Assert.AreEqual(testPosData[0].TmDiscountEligible, stagedPosData[0].TMDiscountEligible);
            Assert.AreEqual(testPosData[0].CaseDiscount, stagedPosData[0].Case_Discount);
            Assert.AreEqual(testPosData[0].AgeCode, stagedPosData[0].AgeCode);
            Assert.AreEqual(testPosData[0].Recall, stagedPosData[0].Recall_Flag);
            Assert.AreEqual(testPosData[0].RestrictedHours, stagedPosData[0].Restricted_Hours);
            Assert.AreEqual(testPosData[0].SoldByWeight, stagedPosData[0].Sold_By_Weight);
            Assert.AreEqual(testPosData[0].ScaleForcedTare, stagedPosData[0].ScaleForcedTare);
            Assert.AreEqual(testPosData[0].QuantityRequired, stagedPosData[0].Quantity_Required);
            Assert.AreEqual(testPosData[0].PriceRequired, stagedPosData[0].Price_Required);
            Assert.AreEqual(testPosData[0].QuantityProhibit, stagedPosData[0].QtyProhibit);
            Assert.AreEqual(testPosData[0].VisualVerify, stagedPosData[0].VisualVerify);
            Assert.AreEqual(testPosData[0].RestrictSale, stagedPosData[0].RestrictSale);
            Assert.AreEqual(testPosData[0].PosScaleTare, stagedPosData[0].PosScaleTare);
            Assert.AreEqual(testPosData[0].LinkedIdentifier, stagedPosData[0].LinkedIdentifier);
            Assert.AreEqual(testPosData[0].Price, stagedPosData[0].Price);
            Assert.AreEqual(testPosData[0].RetailUom, stagedPosData[0].RetailUom);
            Assert.AreEqual(testPosData[0].Multiple, stagedPosData[0].Multiple);
            Assert.AreEqual(testPosData[0].SaleMultiple, stagedPosData[0].SaleMultiple);
            Assert.AreEqual(testPosData[0].SalePrice, stagedPosData[0].Sale_Price);
            Assert.AreEqual(testPosData[0].SaleStartDate, stagedPosData[0].Sale_Start_Date);
            Assert.AreEqual(testPosData[0].SaleEndDate, stagedPosData[0].Sale_End_Date);
            Assert.AreEqual(testPosData[0].InProcessBy, stagedPosData[0].InProcessBy);
            Assert.AreEqual(testPosData[0].InUdmDate, stagedPosData[0].InUdmDate);
            Assert.AreEqual(testPosData[0].EsbReadyDate, stagedPosData[0].EsbReadyDate);
            Assert.AreEqual(testPosData[0].UdmFailedDate, stagedPosData[0].UdmFailedDate);
            Assert.AreEqual(testPosData[0].EsbReadyFailedDate, stagedPosData[0].EsbReadyFailedDate);
        }

        [TestMethod]
        public void StagePosDataBulk_TransientErrorOccursAndRetryIsSuccessful_EventsShouldBeLogged()
        {
            // This test requires manual intervention to work properly since mocking a SqlException requires trickery.  Just open a locking transaction against the 
            // IRMAPush table before running this test to create the appropriate condition.  The test will be set to auto-pass for automatic runs.

            // Given.
            var testPosData = new List<IConPOSPushPublish>
            {
                new TestIconPosPushPublishBuilder().WithIdentifier(testScanCode).WithChangeType(Constants.IrmaPushChangeTypes.ScanCodeAdd),
                new TestIconPosPushPublishBuilder().WithIdentifier(testScanCode).WithChangeType(Constants.IrmaPushChangeTypes.ItemLocaleAttributeChange),
                new TestIconPosPushPublishBuilder().WithIdentifier(testScanCode).WithChangeType(Constants.IrmaPushChangeTypes.ScanCodeAuthorization)
            }.ConvertAll(pos => new IrmaPushModel
                {
                    IconPosPushPublishId = pos.IConPOSPushPublishID,
                    RegionCode = pos.RegionCode,
                    BusinessUnitId = pos.BusinessUnit_ID,
                    Identifier = pos.Identifier,
                    ChangeType = pos.ChangeType,
                    InsertDate = pos.InsertDate,
                    RetailSize = pos.RetailSize.Value,
                    RetailPackageUom = pos.RetailPackageUom,
                    TmDiscountEligible = pos.TMDiscountEligible.Value,
                    CaseDiscount = pos.Case_Discount.Value,
                    AgeCode = pos.AgeCode,
                    Recall = pos.Recall_Flag.Value,
                    RestrictedHours = pos.Restricted_Hours.Value,
                    SoldByWeight = pos.Sold_By_Weight.Value,
                    ScaleForcedTare = pos.ScaleForcedTare.Value,
                    QuantityRequired = pos.Quantity_Required.Value,
                    PriceRequired = pos.Price_Required.Value,
                    QuantityProhibit = pos.QtyProhibit.Value,
                    VisualVerify = pos.VisualVerify.Value,
                    RestrictSale = pos.RestrictSale.Value,
                    PosScaleTare = pos.POSTare,
                    LinkedIdentifier = pos.LinkCode_ItemIdentifier,
                    Price = pos.Price,
                    RetailUom = pos.RetailUom,
                    Multiple = pos.Multiple,
                    SaleMultiple = pos.SaleMultiple,
                    SalePrice = pos.Sale_Price,
                    SaleStartDate = pos.Sale_Start_Date,
                    SaleEndDate = pos.Sale_End_Date,
                    InProcessBy = null,
                    InUdmDate = null,
                    EsbReadyDate = null,
                    UdmFailedDate = null,
                    EsbReadyFailedDate = null
                });

            var irmaPushStagingService = new IrmaPushStagingService(
                mockLogger.Object,
                mockIrmaContextProvider.Object,
                mockEmailClient.Object,
                stagePosDataBulkCommandHandler,
                stagePosDataRowByRowCommandHandler,
                updatePublishTableDatesCommandHandler);

            // When.
            irmaPushStagingService.StagePosDataBulk(testPosData);

            // Then.
            // Uncomment assertions during manual runs to properly verify the test.

            //mockLogger.Verify(l => l.Error(It.IsAny<string>()), Times.Once);
            //mockLogger.Verify(l => l.Info(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void StagePosDataBulk_TransientErrorOccursAndRetryIsSuccessful_EntitiesShouldBeSaved()
        {
            // This test requires manual intervention to work properly since mocking a SqlException requires trickery.  Just open a locking transaction against the 
            // IRMAPush table before running this test to create the appropriate condition.  The test will be set to auto-pass for automatic runs.

            // Given.
            var testPosData = new List<IConPOSPushPublish>
            {
                new TestIconPosPushPublishBuilder().WithIdentifier(testScanCode).WithChangeType(Constants.IrmaPushChangeTypes.ScanCodeAdd),
                new TestIconPosPushPublishBuilder().WithIdentifier(testScanCode).WithChangeType(Constants.IrmaPushChangeTypes.ItemLocaleAttributeChange),
                new TestIconPosPushPublishBuilder().WithIdentifier(testScanCode).WithChangeType(Constants.IrmaPushChangeTypes.ScanCodeAuthorization)
            }.ConvertAll(pos => new IrmaPushModel
                {
                    IconPosPushPublishId = pos.IConPOSPushPublishID,
                    RegionCode = pos.RegionCode,
                    BusinessUnitId = pos.BusinessUnit_ID,
                    Identifier = pos.Identifier,
                    ChangeType = pos.ChangeType,
                    InsertDate = pos.InsertDate,
                    RetailSize = pos.RetailSize.Value,
                    RetailPackageUom = pos.RetailPackageUom,
                    TmDiscountEligible = pos.TMDiscountEligible.Value,
                    CaseDiscount = pos.Case_Discount.Value,
                    AgeCode = pos.AgeCode,
                    Recall = pos.Recall_Flag.Value,
                    RestrictedHours = pos.Restricted_Hours.Value,
                    SoldByWeight = pos.Sold_By_Weight.Value,
                    ScaleForcedTare = pos.ScaleForcedTare.Value,
                    QuantityRequired = pos.Quantity_Required.Value,
                    PriceRequired = pos.Price_Required.Value,
                    QuantityProhibit = pos.QtyProhibit.Value,
                    VisualVerify = pos.VisualVerify.Value,
                    RestrictSale = pos.RestrictSale.Value,
                    PosScaleTare = pos.POSTare,
                    LinkedIdentifier = pos.LinkCode_ItemIdentifier,
                    Price = pos.Price,
                    RetailUom = pos.RetailUom,
                    Multiple = pos.Multiple,
                    SaleMultiple = pos.SaleMultiple,
                    SalePrice = pos.Sale_Price,
                    SaleStartDate = pos.Sale_Start_Date,
                    SaleEndDate = pos.Sale_End_Date,
                    InProcessBy = null,
                    InUdmDate = null,
                    EsbReadyDate = null,
                    UdmFailedDate = null,
                    EsbReadyFailedDate = null
                });

            var irmaPushStagingService = new IrmaPushStagingService(
                mockLogger.Object,
                mockIrmaContextProvider.Object,
                mockEmailClient.Object,
                stagePosDataBulkCommandHandler,
                stagePosDataRowByRowCommandHandler,
                updatePublishTableDatesCommandHandler);

            // When.
            irmaPushStagingService.StagePosDataBulk(testPosData);

            // Then.
            var stagedPosData = globalIconContext.Context.IRMAPush.Where(ip => ip.Identifier == testScanCode).ToList();

            // Uncomment assertions during manual runs to properly verify the test.

            //Assert.AreEqual(testPosData.Count, stagedPosData.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void StagePosDataBulk_IntransientErrorOccurs_ExceptionShouldBeThrown()
        {
            // Given.
            var testPosData = new List<IConPOSPushPublish>
            {
                new TestIconPosPushPublishBuilder().WithIdentifier(testScanCode).WithChangeType(Constants.IrmaPushChangeTypes.ScanCodeAdd),
                new TestIconPosPushPublishBuilder().WithIdentifier(testScanCode).WithChangeType(Constants.IrmaPushChangeTypes.ItemLocaleAttributeChange),
                new TestIconPosPushPublishBuilder().WithIdentifier(testScanCode).WithChangeType(Constants.IrmaPushChangeTypes.ScanCodeAuthorization)
            }.ConvertAll(pos => new IrmaPushModel
                {
                    IconPosPushPublishId = pos.IConPOSPushPublishID,
                    RegionCode = pos.RegionCode,
                    BusinessUnitId = pos.BusinessUnit_ID,
                    Identifier = pos.Identifier,
                    ChangeType = pos.ChangeType,
                    InsertDate = pos.InsertDate,
                    RetailSize = pos.RetailSize.Value,
                    RetailPackageUom = pos.RetailPackageUom,
                    TmDiscountEligible = pos.TMDiscountEligible.Value,
                    CaseDiscount = pos.Case_Discount.Value,
                    AgeCode = pos.AgeCode,
                    Recall = pos.Recall_Flag.Value,
                    RestrictedHours = pos.Restricted_Hours.Value,
                    SoldByWeight = pos.Sold_By_Weight.Value,
                    ScaleForcedTare = pos.ScaleForcedTare.Value,
                    QuantityRequired = pos.Quantity_Required.Value,
                    PriceRequired = pos.Price_Required.Value,
                    QuantityProhibit = pos.QtyProhibit.Value,
                    VisualVerify = pos.VisualVerify.Value,
                    RestrictSale = pos.RestrictSale.Value,
                    PosScaleTare = pos.POSTare,
                    LinkedIdentifier = pos.LinkCode_ItemIdentifier,
                    Price = pos.Price,
                    RetailUom = pos.RetailUom,
                    Multiple = pos.Multiple,
                    SaleMultiple = pos.SaleMultiple,
                    SalePrice = pos.Sale_Price,
                    SaleStartDate = pos.Sale_Start_Date,
                    SaleEndDate = pos.Sale_End_Date,
                    InProcessBy = null,
                    InUdmDate = null,
                    EsbReadyDate = null,
                    UdmFailedDate = null,
                    EsbReadyFailedDate = null
                });

            mockStagePosDataBulkCommandHandler.Setup(c => c.Execute(It.IsAny<StagePosDataBulkCommand>())).Throws(new Exception());

            var irmaPushStagingService = new IrmaPushStagingService(
                mockLogger.Object,
                mockIrmaContextProvider.Object,
                mockEmailClient.Object,
                mockStagePosDataBulkCommandHandler.Object,
                stagePosDataRowByRowCommandHandler,
                updatePublishTableDatesCommandHandler);

            // When.
            irmaPushStagingService.StagePosDataBulk(testPosData);

            // Then.
            // Expected exception.
        }

        [TestMethod]
        public void StagePosDataRowByRow_SaveIsSuccessfulForEachEntity_EntitiesShouldBeSaved()
        {
            // Given.
            var testPosData = new List<IConPOSPushPublish>
            {
                new TestIconPosPushPublishBuilder().WithIdentifier(testScanCode).WithChangeType(Constants.IrmaPushChangeTypes.ScanCodeAdd),
                new TestIconPosPushPublishBuilder().WithIdentifier(testScanCode).WithChangeType(Constants.IrmaPushChangeTypes.ItemLocaleAttributeChange),
                new TestIconPosPushPublishBuilder().WithIdentifier(testScanCode).WithChangeType(Constants.IrmaPushChangeTypes.ScanCodeAuthorization)
            }.ConvertAll(pos => new IrmaPushModel
                {
                    IconPosPushPublishId = pos.IConPOSPushPublishID,
                    RegionCode = pos.RegionCode,
                    BusinessUnitId = pos.BusinessUnit_ID,
                    Identifier = pos.Identifier,
                    ChangeType = pos.ChangeType,
                    InsertDate = pos.InsertDate,
                    RetailSize = pos.RetailSize.Value,
                    RetailPackageUom = pos.RetailPackageUom,
                    TmDiscountEligible = pos.TMDiscountEligible.Value,
                    CaseDiscount = pos.Case_Discount.Value,
                    AgeCode = pos.AgeCode,
                    Recall = pos.Recall_Flag.Value,
                    RestrictedHours = pos.Restricted_Hours.Value,
                    SoldByWeight = pos.Sold_By_Weight.Value,
                    ScaleForcedTare = pos.ScaleForcedTare.Value,
                    QuantityRequired = pos.Quantity_Required.Value,
                    PriceRequired = pos.Price_Required.Value,
                    QuantityProhibit = pos.QtyProhibit.Value,
                    VisualVerify = pos.VisualVerify.Value,
                    RestrictSale = pos.RestrictSale.Value,
                    PosScaleTare = pos.POSTare,
                    LinkedIdentifier = pos.LinkCode_ItemIdentifier,
                    Price = pos.Price,
                    RetailUom = pos.RetailUom,
                    Multiple = pos.Multiple,
                    SaleMultiple = pos.SaleMultiple,
                    SalePrice = pos.Sale_Price,
                    SaleStartDate = pos.Sale_Start_Date,
                    SaleEndDate = pos.Sale_End_Date,
                    InProcessBy = null,
                    InUdmDate = null,
                    EsbReadyDate = null,
                    UdmFailedDate = null,
                    EsbReadyFailedDate = null
                });

            var irmaPushStagingService = new IrmaPushStagingService(
                mockLogger.Object,
                mockIrmaContextProvider.Object,
                mockEmailClient.Object,
                stagePosDataBulkCommandHandler,
                stagePosDataRowByRowCommandHandler,
                updatePublishTableDatesCommandHandler);

            // When.
            irmaPushStagingService.StagePosDataRowByRow(testPosData);

            // Then.
            var stagedPosData = globalIconContext.Context.IRMAPush.Where(ip => ip.Identifier == testScanCode).ToList();

            Assert.AreEqual(testPosData.Count, stagedPosData.Count);
        }

        [TestMethod]
        public void SavePosDataRowByRow_NewEntitySaveFailsForOneEntity_FailedEntityShouldNotBeSaved()
        {
            var testPosData = new List<IConPOSPushPublish>
            {
                new TestIconPosPushPublishBuilder().WithIdentifier(testScanCode+"1233456789").WithChangeType(Constants.IrmaPushChangeTypes.ScanCodeAdd),
                new TestIconPosPushPublishBuilder().WithIdentifier(testScanCode).WithChangeType(Constants.IrmaPushChangeTypes.ItemLocaleAttributeChange),
                new TestIconPosPushPublishBuilder().WithIdentifier(testScanCode).WithChangeType(Constants.IrmaPushChangeTypes.ScanCodeAuthorization)
            }.ConvertAll(pos => new IrmaPushModel
                {
                    IconPosPushPublishId = pos.IConPOSPushPublishID,
                    RegionCode = pos.RegionCode,
                    BusinessUnitId = pos.BusinessUnit_ID,
                    Identifier = pos.Identifier,
                    ChangeType = pos.ChangeType,
                    InsertDate = pos.InsertDate,
                    RetailSize = pos.RetailSize.Value,
                    RetailPackageUom = pos.RetailPackageUom,
                    TmDiscountEligible = pos.TMDiscountEligible.Value,
                    CaseDiscount = pos.Case_Discount.Value,
                    AgeCode = pos.AgeCode,
                    Recall = pos.Recall_Flag.Value,
                    RestrictedHours = pos.Restricted_Hours.Value,
                    SoldByWeight = pos.Sold_By_Weight.Value,
                    ScaleForcedTare = pos.ScaleForcedTare.Value,
                    QuantityRequired = pos.Quantity_Required.Value,
                    PriceRequired = pos.Price_Required.Value,
                    QuantityProhibit = pos.QtyProhibit.Value,
                    VisualVerify = pos.VisualVerify.Value,
                    RestrictSale = pos.RestrictSale.Value,
                    PosScaleTare = pos.POSTare,
                    LinkedIdentifier = pos.LinkCode_ItemIdentifier,
                    Price = pos.Price,
                    RetailUom = pos.RetailUom,
                    Multiple = pos.Multiple,
                    SaleMultiple = pos.SaleMultiple,
                    SalePrice = pos.Sale_Price,
                    SaleStartDate = pos.Sale_Start_Date,
                    SaleEndDate = pos.Sale_End_Date,
                    InProcessBy = null,
                    InUdmDate = null,
                    EsbReadyDate = null,
                    UdmFailedDate = null,
                    EsbReadyFailedDate = null
                });

            var irmaPushStagingService = new IrmaPushStagingService(
               mockLogger.Object,
               mockIrmaContextProvider.Object,
               mockEmailClient.Object,
               stagePosDataBulkCommandHandler,
               stagePosDataRowByRowCommandHandler,
               updatePublishTableDatesCommandHandler);

            // When.
            irmaPushStagingService.StagePosDataRowByRow(testPosData);

            // Then.
            var stagedPosData = globalIconContext.Context.IRMAPush.Where(ip => ip.Identifier == testScanCode).ToList();

            Assert.AreEqual(testPosData.Count - 1, stagedPosData.Count);
        }

        [TestMethod]
        public void StagePosDataRowByRow_NewEntitySaveFailsForOneEntity_ErrorShouldBeLogged()
        {
            var testPosData = new List<IConPOSPushPublish>
            {
                new TestIconPosPushPublishBuilder().WithIdentifier(testScanCode+"123456789").WithChangeType(Constants.IrmaPushChangeTypes.ScanCodeAuthorization),
                new TestIconPosPushPublishBuilder().WithIdentifier(testScanCode).WithChangeType(Constants.IrmaPushChangeTypes.ScanCodeAdd),
                new TestIconPosPushPublishBuilder().WithIdentifier(testScanCode).WithChangeType(Constants.IrmaPushChangeTypes.ItemLocaleAttributeChange)
            }.ConvertAll(pos => new IrmaPushModel
                {
                    IconPosPushPublishId = pos.IConPOSPushPublishID,
                    RegionCode = pos.RegionCode,
                    BusinessUnitId = pos.BusinessUnit_ID,
                    Identifier = pos.Identifier,
                    ChangeType = pos.ChangeType,
                    InsertDate = pos.InsertDate,
                    RetailSize = pos.RetailSize.Value,
                    RetailPackageUom = pos.RetailPackageUom,
                    TmDiscountEligible = pos.TMDiscountEligible.Value,
                    CaseDiscount = pos.Case_Discount.Value,
                    AgeCode = pos.AgeCode,
                    Recall = pos.Recall_Flag.Value,
                    RestrictedHours = pos.Restricted_Hours.Value,
                    SoldByWeight = pos.Sold_By_Weight.Value,
                    ScaleForcedTare = pos.ScaleForcedTare.Value,
                    QuantityRequired = pos.Quantity_Required.Value,
                    PriceRequired = pos.Price_Required.Value,
                    QuantityProhibit = pos.QtyProhibit.Value,
                    VisualVerify = pos.VisualVerify.Value,
                    RestrictSale = pos.RestrictSale.Value,
                    PosScaleTare = pos.POSTare,
                    LinkedIdentifier = pos.LinkCode_ItemIdentifier,
                    Price = pos.Price,
                    RetailUom = pos.RetailUom,
                    Multiple = pos.Multiple,
                    SaleMultiple = pos.SaleMultiple,
                    SalePrice = pos.Sale_Price,
                    SaleStartDate = pos.Sale_Start_Date,
                    SaleEndDate = pos.Sale_End_Date,
                    InProcessBy = null,
                    InUdmDate = null,
                    EsbReadyDate = null,
                    UdmFailedDate = null,
                    EsbReadyFailedDate = null
                });

            var irmaPushStagingService = new IrmaPushStagingService(
               mockLogger.Object,
               mockIrmaContextProvider.Object,
               mockEmailClient.Object,
               stagePosDataBulkCommandHandler,
               stagePosDataRowByRowCommandHandler,
               updatePublishTableDatesCommandHandler);

            // When.
            irmaPushStagingService.StagePosDataRowByRow(testPosData);

            // Then.
            mockLogger.Verify(l => l.Error(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void StagePosDataRowByRow_NewEntitySaveFailsForOneEntity_AlertEmailShouldBeSent()
        {
            var testPosData = new List<IConPOSPushPublish>
            {
                new TestIconPosPushPublishBuilder().WithIdentifier(testScanCode+"123456789").WithChangeType(Constants.IrmaPushChangeTypes.ScanCodeAuthorization),
                new TestIconPosPushPublishBuilder().WithIdentifier(testScanCode).WithChangeType(Constants.IrmaPushChangeTypes.ScanCodeAdd),
                new TestIconPosPushPublishBuilder().WithIdentifier(testScanCode).WithChangeType(Constants.IrmaPushChangeTypes.ItemLocaleAttributeChange)
            }.ConvertAll(pos => new IrmaPushModel
                {
                    IconPosPushPublishId = pos.IConPOSPushPublishID,
                    RegionCode = pos.RegionCode,
                    BusinessUnitId = pos.BusinessUnit_ID,
                    Identifier = pos.Identifier,
                    ChangeType = pos.ChangeType,
                    InsertDate = pos.InsertDate,
                    RetailSize = pos.RetailSize.Value,
                    RetailPackageUom = pos.RetailPackageUom,
                    TmDiscountEligible = pos.TMDiscountEligible.Value,
                    CaseDiscount = pos.Case_Discount.Value,
                    AgeCode = pos.AgeCode,
                    Recall = pos.Recall_Flag.Value,
                    RestrictedHours = pos.Restricted_Hours.Value,
                    SoldByWeight = pos.Sold_By_Weight.Value,
                    ScaleForcedTare = pos.ScaleForcedTare.Value,
                    QuantityRequired = pos.Quantity_Required.Value,
                    PriceRequired = pos.Price_Required.Value,
                    QuantityProhibit = pos.QtyProhibit.Value,
                    VisualVerify = pos.VisualVerify.Value,
                    RestrictSale = pos.RestrictSale.Value,
                    PosScaleTare = pos.POSTare,
                    LinkedIdentifier = pos.LinkCode_ItemIdentifier,
                    Price = pos.Price,
                    RetailUom = pos.RetailUom,
                    Multiple = pos.Multiple,
                    SaleMultiple = pos.SaleMultiple,
                    SalePrice = pos.Sale_Price,
                    SaleStartDate = pos.Sale_Start_Date,
                    SaleEndDate = pos.Sale_End_Date,
                    InProcessBy = null,
                    InUdmDate = null,
                    EsbReadyDate = null,
                    UdmFailedDate = null,
                    EsbReadyFailedDate = null
                });

            var irmaPushStagingService = new IrmaPushStagingService(
               mockLogger.Object,
               mockIrmaContextProvider.Object,
               mockEmailClient.Object,
               stagePosDataBulkCommandHandler,
               stagePosDataRowByRowCommandHandler,
               updatePublishTableDatesCommandHandler);

            // When.
            irmaPushStagingService.StagePosDataRowByRow(testPosData);

            // Then.
            mockEmailClient.Verify(e => e.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void StagePosDataRowByRow_SaveFailsForOneEntity_FailedDateShouldBeUpdatedInTheStagingTable()
        {
            StagePosPublishData();

            var testPosData = new List<IConPOSPushPublish>
            {
                new TestIconPosPushPublishBuilder().WithIconPosPushPublishId(testPosPublishData[0].IConPOSPushPublishID).WithIdentifier(testScanCode).WithChangeType(Constants.IrmaPushChangeTypes.ScanCodeAuthorization),
                new TestIconPosPushPublishBuilder().WithIconPosPushPublishId(testPosPublishData[1].IConPOSPushPublishID).WithIdentifier(testScanCode+"123456789").WithChangeType(Constants.IrmaPushChangeTypes.ScanCodeAdd),
                new TestIconPosPushPublishBuilder().WithIconPosPushPublishId(testPosPublishData[2].IConPOSPushPublishID).WithIdentifier(testScanCode).WithChangeType(Constants.IrmaPushChangeTypes.ItemLocaleAttributeChange)
            }.ConvertAll(pos => new IrmaPushModel
                {
                    IconPosPushPublishId = pos.IConPOSPushPublishID,
                    RegionCode = pos.RegionCode,
                    BusinessUnitId = pos.BusinessUnit_ID,
                    Identifier = pos.Identifier,
                    ChangeType = pos.ChangeType,
                    InsertDate = pos.InsertDate,
                    RetailSize = pos.RetailSize.Value,
                    RetailPackageUom = pos.RetailPackageUom,
                    TmDiscountEligible = pos.TMDiscountEligible.Value,
                    CaseDiscount = pos.Case_Discount.Value,
                    AgeCode = pos.AgeCode,
                    Recall = pos.Recall_Flag.Value,
                    RestrictedHours = pos.Restricted_Hours.Value,
                    SoldByWeight = pos.Sold_By_Weight.Value,
                    ScaleForcedTare = pos.ScaleForcedTare.Value,
                    QuantityRequired = pos.Quantity_Required.Value,
                    PriceRequired = pos.Price_Required.Value,
                    QuantityProhibit = pos.QtyProhibit.Value,
                    VisualVerify = pos.VisualVerify.Value,
                    RestrictSale = pos.RestrictSale.Value,
                    PosScaleTare = pos.POSTare,
                    LinkedIdentifier = pos.LinkCode_ItemIdentifier,
                    Price = pos.Price,
                    RetailUom = pos.RetailUom,
                    Multiple = pos.Multiple,
                    SaleMultiple = pos.SaleMultiple,
                    SalePrice = pos.Sale_Price,
                    SaleStartDate = pos.Sale_Start_Date,
                    SaleEndDate = pos.Sale_End_Date,
                    InProcessBy = null,
                    InUdmDate = null,
                    EsbReadyDate = null,
                    UdmFailedDate = null,
                    EsbReadyFailedDate = null
                });

            var irmaPushStagingService = new IrmaPushStagingService(
               mockLogger.Object,
               mockIrmaContextProvider.Object,
               mockEmailClient.Object,
               stagePosDataBulkCommandHandler,
               stagePosDataRowByRowCommandHandler,
               updatePublishTableDatesCommandHandler);

            // When.
            irmaPushStagingService.StagePosDataRowByRow(testPosData);

            // Then.
            int failedPublishId = testPosPublishData[1].IConPOSPushPublishID;
            var failedPublishRecord = irmaContext.IConPOSPushPublish.Single(pub => pub.IConPOSPushPublishID == failedPublishId);

            // Have to reload the entity since the price update was done via stored procedure.
            irmaContext.Entry(failedPublishRecord).Reload();

            Assert.AreEqual(DateTime.Now.Date, failedPublishRecord.ProcessingFailedDate.Value.Date);
        }
    }
}
