using Icon.Common.Email;
using Icon.Logging;
using Icon.Testing.Builders;
using InterfaceController.Common;
using Irma.Framework;
using Irma.Framework.RenewableContext;
using Irma.Testing.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PushController.Common;
using PushController.Controller.PosDataConverters;
using PushController.DataAccess.Commands;
using PushController.DataAccess.Interfaces;
using PushController.DataAccess.Queries;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace PushController.Tests.Controller.IconPosDataConverterTests
{
    [TestClass]
    public class IrmaPushDataConverterTests
    {
        private IrmaContext context;
        private GlobalIrmaContext globalContext;
        private DbContextTransaction transaction;
        private Mock<ILogger<IrmaPushDataConverter>> mockLogger;
        private Mock<IEmailClient> mockEmailClient;
        private Mock<IIrmaContextProvider> mockContextProvider;
        private Mock<IQueryHandler<GetAppConfigKeysQuery, List<GetAppConfigKeysResult>>> mockGetAppConfigKeysQueryHandler;
        private Mock<ICommandHandler<UpdatePublishTableDatesCommand>> mockUpdatePublishTableDatesCommandHandler;
        private UpdatePublishTableDatesCommandHandler updatePublishTableDatesCommandHandler;
        private List<IConPOSPushPublish> testPosData;
        private string testScanCode;
        private Random random;

        [TestInitialize]
        public void Initialize()
        {
            this.testScanCode = "2222222";

            this.mockLogger = new Mock<ILogger<IrmaPushDataConverter>>();
            this.mockEmailClient = new Mock<IEmailClient>();
            this.mockContextProvider = new Mock<IIrmaContextProvider>();
            this.mockGetAppConfigKeysQueryHandler = new Mock<IQueryHandler<GetAppConfigKeysQuery, List<GetAppConfigKeysResult>>>();
            this.mockUpdatePublishTableDatesCommandHandler = new Mock<ICommandHandler<UpdatePublishTableDatesCommand>>();
            this.updatePublishTableDatesCommandHandler = new UpdatePublishTableDatesCommandHandler(new Mock<ILogger<UpdatePublishTableDatesCommandHandler>>().Object);

            string irmaConnectionString = ConnectionBuilder.GetConnection("SP");
            this.globalContext = new GlobalIrmaContext(new IrmaContextProvider().GetRegionalContext(irmaConnectionString), irmaConnectionString);
            this.context = globalContext.Context;
            this.mockContextProvider.Setup(cp => cp.GetRegionalContext(It.IsAny<string>())).Returns(this.context);

            this.random = new Random();

            this.transaction = this.context.Database.BeginTransaction();
        }

        [TestCleanup]
        public void Cleanup()
        {
            this.transaction.Rollback();
        }

        private void StagePosData()
        {
            this.context.IConPOSPushPublish.AddRange(this.testPosData);
            this.context.SaveChanges();
        }

        [TestMethod]
        public void ConvertPosData_ValidPosDataRecord_PosDataShouldBeConvertedToIrmaPushRecord()
        {
            // Given.
            this.testPosData = new List<IConPOSPushPublish>
            {
                new TestIconPosPushPublishBuilder().WithStoreNumber(113).WithIdentifier(this.testScanCode).WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(1000))),
                new TestIconPosPushPublishBuilder().WithStoreNumber(113).WithIdentifier(this.testScanCode).WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(1000))),
                new TestIconPosPushPublishBuilder().WithStoreNumber(113).WithIdentifier(this.testScanCode).WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(1000)))
            };

            StagePosData();

            var irmaPushDataConverter = new IrmaPushDataConverter(
                mockLogger.Object,
                mockEmailClient.Object,
                mockContextProvider.Object,
                mockGetAppConfigKeysQueryHandler.Object,
                mockUpdatePublishTableDatesCommandHandler.Object);

            // When.
            var convertedObjects = irmaPushDataConverter.ConvertPosData(this.testPosData);

            // Then.
            Assert.AreEqual(this.testPosData.Count, convertedObjects.Count);
            Assert.AreEqual(this.testPosData[0].RegionCode, convertedObjects[0].RegionCode);
            Assert.AreEqual(this.testPosData[0].BusinessUnit_ID, convertedObjects[0].BusinessUnitId);
            Assert.AreEqual(this.testPosData[0].Identifier, convertedObjects[0].Identifier);
            Assert.AreEqual(this.testPosData[0].ChangeType, convertedObjects[0].ChangeType);
            Assert.AreEqual(default(DateTime), convertedObjects[0].InsertDate.Date);
            Assert.AreEqual(this.testPosData[0].RetailSize, convertedObjects[0].RetailSize);
            Assert.AreEqual(this.testPosData[0].RetailPackageUom, convertedObjects[0].RetailPackageUom);
            Assert.AreEqual(this.testPosData[0].TMDiscountEligible, convertedObjects[0].TmDiscountEligible);
            Assert.AreEqual(this.testPosData[0].Case_Discount, convertedObjects[0].CaseDiscount);
            Assert.AreEqual(this.testPosData[0].AgeCode, convertedObjects[0].AgeCode);
            Assert.AreEqual(this.testPosData[0].Recall_Flag, convertedObjects[0].Recall);
            Assert.AreEqual(this.testPosData[0].Restricted_Hours, convertedObjects[0].RestrictedHours);
            Assert.AreEqual(this.testPosData[0].Sold_By_Weight, convertedObjects[0].SoldByWeight);
            Assert.AreEqual(this.testPosData[0].ScaleForcedTare, convertedObjects[0].ScaleForcedTare);
            Assert.AreEqual(this.testPosData[0].Quantity_Required, convertedObjects[0].QuantityRequired);
            Assert.AreEqual(this.testPosData[0].Price_Required, convertedObjects[0].PriceRequired);
            Assert.AreEqual(this.testPosData[0].QtyProhibit, convertedObjects[0].QuantityProhibit);
            Assert.AreEqual(this.testPosData[0].VisualVerify, convertedObjects[0].VisualVerify);
            Assert.AreEqual(this.testPosData[0].RestrictSale, convertedObjects[0].RestrictSale);
            Assert.AreEqual(this.testPosData[0].POSTare, convertedObjects[0].PosScaleTare);
            Assert.AreEqual(this.testPosData[0].LinkCode_ItemIdentifier, convertedObjects[0].LinkedIdentifier);
            Assert.AreEqual(this.testPosData[0].Price, convertedObjects[0].Price);
            Assert.AreEqual(this.testPosData[0].RetailUom, convertedObjects[0].RetailUom);
            Assert.AreEqual(this.testPosData[0].Multiple, convertedObjects[0].Multiple);
            Assert.AreEqual(this.testPosData[0].SaleMultiple, convertedObjects[0].SaleMultiple);
            Assert.AreEqual(this.testPosData[0].Sale_Price, convertedObjects[0].SalePrice);
            Assert.AreEqual(this.testPosData[0].Sale_Start_Date, convertedObjects[0].SaleStartDate);
            Assert.AreEqual(this.testPosData[0].Sale_End_Date, convertedObjects[0].SaleEndDate);
            Assert.IsNull(convertedObjects[0].InProcessBy);
            Assert.IsNull(convertedObjects[0].InUdmDate);
            Assert.IsNull(convertedObjects[0].EsbReadyDate);
            Assert.IsNull(convertedObjects[0].UdmFailedDate);
            Assert.IsNull(convertedObjects[0].EsbReadyFailedDate);
        }

        [TestMethod]
        public void ConvertPosData_SaleStartDateIsAfterEndDate_AlertEmailShouldBeSent()
        {
            // Given.
            this.testPosData = new List<IConPOSPushPublish>
            {
                new TestIconPosPushPublishBuilder()
                    .WithStoreNumber(113).WithIdentifier(this.testScanCode).WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(1000))).WithSaleEndDate(DateTime.Now).WithSaleStartDate(DateTime.Now.AddDays(1)).WithChangeType(Constants.IrmaPushChangeTypes.NonRegularPriceChange).WithSalePrice(1.99m),
                new TestIconPosPushPublishBuilder().WithStoreNumber(113).WithIdentifier(this.testScanCode).WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(1000))),
                new TestIconPosPushPublishBuilder().WithStoreNumber(113).WithIdentifier(this.testScanCode).WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(1000)))
            };

            StagePosData();

            mockGetAppConfigKeysQueryHandler.Setup(q => q.Execute(It.IsAny<GetAppConfigKeysQuery>())).Returns(new List<GetAppConfigKeysResult>());

            var irmaPushDataConverter = new IrmaPushDataConverter(
                mockLogger.Object,
                mockEmailClient.Object,
                mockContextProvider.Object,
                mockGetAppConfigKeysQueryHandler.Object,
                mockUpdatePublishTableDatesCommandHandler.Object);

            // When.
            var convertedObjects = irmaPushDataConverter.ConvertPosData(this.testPosData);

            // Then.
            mockEmailClient.Verify(e => e.Send(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string[]>()), Times.Once);
        }

        [TestMethod]
        public void ConvertPosData_SaleStartDateIsAfterEndDate_ProcessingFailedDateShouldBeUpdated()
        {
            // Given.
            this.testPosData = new List<IConPOSPushPublish>
            {
                new TestIconPosPushPublishBuilder()
                    .WithStoreNumber(113).WithIdentifier(this.testScanCode).WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(1000))).WithSaleEndDate(DateTime.Now).WithSaleStartDate(DateTime.Now.AddDays(1)).WithChangeType(Constants.IrmaPushChangeTypes.NonRegularPriceChange).WithSalePrice(1.99m),
                new TestIconPosPushPublishBuilder().WithStoreNumber(113).WithIdentifier(this.testScanCode).WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(1000))),
                new TestIconPosPushPublishBuilder().WithStoreNumber(113).WithIdentifier(this.testScanCode).WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(1000)))
            };

            StagePosData();

            mockGetAppConfigKeysQueryHandler.Setup(q => q.Execute(It.IsAny<GetAppConfigKeysQuery>())).Returns(new List<GetAppConfigKeysResult>());

            var irmaPushDataConverter = new IrmaPushDataConverter(
                mockLogger.Object,
                mockEmailClient.Object,
                mockContextProvider.Object,
                mockGetAppConfigKeysQueryHandler.Object,
                updatePublishTableDatesCommandHandler);

            // When.
            var convertedObjects = irmaPushDataConverter.ConvertPosData(this.testPosData);

            // Then.
            int failedRecordId = testPosData[0].IConPOSPushPublishID;
            var failedRecord = context.IConPOSPushPublish.Single(publish => publish.IConPOSPushPublishID == failedRecordId);

            // Have to reload the entity because the record was updated via stored procedure.
            context.Entry(failedRecord).Reload();

            Assert.AreEqual(DateTime.Now.Date, failedRecord.ProcessingFailedDate.Value.Date);
            Assert.IsNull(failedRecord.ProcessedDate);
        }

        [TestMethod]
        public void ConvertPosData_SaleEndDateIsExpired_SaleInformationShouldBeRemovedFromTheRecord()
        {
            // Given.
            this.testPosData = new List<IConPOSPushPublish>
            {
                new TestIconPosPushPublishBuilder()
                    .WithStoreNumber(113).WithIdentifier(this.testScanCode).WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(1000))).WithSaleStartDate(DateTime.Now.AddDays(-2)).WithSaleEndDate(DateTime.Now.AddDays(-1)).WithChangeType(Constants.IrmaPushChangeTypes.NonRegularPriceChange).WithSalePrice(1.99m),
                new TestIconPosPushPublishBuilder().WithStoreNumber(113).WithIdentifier(this.testScanCode).WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(1000))),
                new TestIconPosPushPublishBuilder().WithStoreNumber(113).WithIdentifier(this.testScanCode).WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(1000)))
            };

            StagePosData();

            mockGetAppConfigKeysQueryHandler.Setup(q => q.Execute(It.IsAny<GetAppConfigKeysQuery>())).Returns(new List<GetAppConfigKeysResult>());

            var irmaPushDataConverter = new IrmaPushDataConverter(
                mockLogger.Object,
                mockEmailClient.Object,
                mockContextProvider.Object,
                mockGetAppConfigKeysQueryHandler.Object,
                updatePublishTableDatesCommandHandler);

            // When.
            var convertedObjects = irmaPushDataConverter.ConvertPosData(this.testPosData);

            // Then.
            int failedRecordId = testPosData[0].IConPOSPushPublishID;
            var failedRecord = context.IConPOSPushPublish.Single(publish => publish.IConPOSPushPublishID == failedRecordId);

            Assert.IsNull(failedRecord.Sale_Price);
            Assert.IsNull(failedRecord.SaleMultiple);
            Assert.IsNull(failedRecord.Sale_Start_Date);
            Assert.IsNull(failedRecord.Sale_End_Date);
            Assert.IsNull(failedRecord.ProcessedDate);
            Assert.IsNull(failedRecord.ProcessingFailedDate);
        }

        [TestMethod]
        public void ConvertPosData_SaleEndDateIsToday_RecordShouldBeConvertedSuccessfully()
        {
            // Given.
            this.testPosData = new List<IConPOSPushPublish>
            {
                new TestIconPosPushPublishBuilder()
                    .WithStoreNumber(113).WithIdentifier(this.testScanCode).WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(1000))).WithSaleStartDate(DateTime.Now.AddDays(-1)).WithSaleEndDate(DateTime.Now).WithSalePrice(1.99m),
                new TestIconPosPushPublishBuilder().WithStoreNumber(113).WithIdentifier(this.testScanCode).WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(1000))),
                new TestIconPosPushPublishBuilder().WithStoreNumber(113).WithIdentifier(this.testScanCode).WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(1000)))
            };

            StagePosData();

            mockGetAppConfigKeysQueryHandler.Setup(q => q.Execute(It.IsAny<GetAppConfigKeysQuery>())).Returns(new List<GetAppConfigKeysResult>());

            var irmaPushDataConverter = new IrmaPushDataConverter(
                mockLogger.Object,
                mockEmailClient.Object,
                mockContextProvider.Object,
                mockGetAppConfigKeysQueryHandler.Object,
                updatePublishTableDatesCommandHandler);

            // When.
            var convertedObjects = irmaPushDataConverter.ConvertPosData(this.testPosData);

            // Then.
            Assert.AreEqual(testPosData.Count, convertedObjects.Count);
        }
    }
}
