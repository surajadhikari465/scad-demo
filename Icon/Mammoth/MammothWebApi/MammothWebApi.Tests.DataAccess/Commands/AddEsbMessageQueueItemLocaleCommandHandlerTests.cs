using Dapper;
using Mammoth.Common.DataAccess;
using Mammoth.Common.DataAccess.DbProviders;
using Mammoth.Logging;
using MammothWebApi.DataAccess.Commands;
using MammothWebApi.DataAccess.Models;
using MammothWebApi.Tests.DataAccess.ModelBuilders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;

namespace MammothWebApi.Tests.DataAccess.Commands
{
    [TestClass]
    public class AddEsbMessageQueueItemLocaleCommandHandlerTests
    {
        private AddEsbMessageQueueItemLocaleCommandHandler commandHandler;
        private SqlDbProvider db;
        private Mock<ILogger> logger;
        private string testRegion;
        private DateTime now;
        private Guid transactionId;
        private List<StagingItemLocaleModel> testStagingItemLocaleData;
        private List<List<StagingItemLocaleExtendedModel>> testStagingExtendedItemLocaleData;
        private List<Item> testItems;
        private List<string> testScanCodes;
        private Locales testLocale;
        private int testBusinessUnitId;
        private int itemTypeId;

        [TestInitialize]
        public void Initialize()
        {
            now = DateTime.Now;
            transactionId = Guid.NewGuid();
            testRegion = "SW";
            testBusinessUnitId = 44444;

            string connectionString = ConfigurationManager.ConnectionStrings["Mammoth"].ConnectionString;

            db = new SqlDbProvider();
            logger = new Mock<ILogger>();
            commandHandler = new AddEsbMessageQueueItemLocaleCommandHandler(logger.Object, db);

            db.Connection = new SqlConnection(connectionString);
            db.Connection.Open();
            db.Transaction = db.Connection.BeginTransaction();

            // DatabaseInitialization class adds ItemTypes
            this.itemTypeId = this.db.Connection.Query<int>("SELECT TOP 1 itemTypeID FROM ItemTypes", transaction: this.db.Transaction).First();

            // Add test items and locales and itemLocale data
            AddTestItems();
            AddTestLocale();
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (db.Transaction != null)
            {
                db.Transaction.Rollback();
                db.Transaction.Dispose();
            }

            db.Connection.Dispose();
        }

        private void AddTestLocale()
        {
            testLocale = new Locales
            {
                Region = this.testRegion,
                BusinessUnitID = testBusinessUnitId,
                StoreAbbrev = "TEST",
                StoreName = "TEST STORE NAME 1",
                AddedDate = DateTime.Now
            };

            string sql = String.Format(@"INSERT INTO dbo.Locales_{0}
                                        (
                                            Region,
	                                        BusinessUnitID,
	                                        StoreName,
	                                        StoreAbbrev,
                                            AddedDate
                                        )
                                        VALUES
                                        (
                                            @Region,
	                                        @BusinessUnitID,
	                                        @StoreName,
	                                        @StoreAbbrev,
                                            @AddedDate
                                        )", testRegion);

            db.Connection.Execute(sql, testLocale, transaction: db.Transaction);
        }

        private void AddTestItems()
        {
            testScanCodes = new List<string>
            {
                "222222222221",
                "222222222222",
                "222222222223"
            };

            int? maxItemId = db.Connection.Query<int?>("SELECT MAX(ItemID) FROM Items", transaction: db.Transaction).SingleOrDefault();

            testItems = new List<Item>
            {
                new TestItemBuilder().WithItemId((maxItemId ?? default(int)) + 1).WithScanCode(testScanCodes[0]).WithItemTypeId(this.itemTypeId).Build(),
                new TestItemBuilder().WithItemId((maxItemId ?? default(int)) + 2).WithScanCode(testScanCodes[1]).WithItemTypeId(this.itemTypeId).Build(),
                new TestItemBuilder().WithItemId((maxItemId ?? default(int)) + 3).WithScanCode(testScanCodes[2]).WithItemTypeId(this.itemTypeId).Build()
            };

            string sql = @"INSERT INTO Items
                            (
	                            ItemID,
	                            ItemTypeID,
	                            ScanCode,
	                            HierarchyMerchandiseID,
	                            HierarchyNationalClassID,
	                            BrandHCID,
	                            TaxClassHCID,
	                            PSNumber,
	                            Desc_POS,
	                            Desc_Product,
	                            PackageUnit,
	                            RetailSize,
	                            RetailUOM,
	                            FoodStampEligible
                            )
                            VALUES
                            (
	                            @ItemID,
	                            @ItemTypeID,
	                            @ScanCode,
	                            @HierarchyMerchandiseID,
	                            @HierarchyNationalClassID,
	                            @BrandHCID,
	                            @TaxClassHCID,
	                            @PSNumber,
	                            @Desc_POS,
	                            @Desc_Product,
	                            @PackageUnit,
	                            @RetailSize,
	                            @RetailUOM,
	                            @FoodStampEligible
                            )";

            db.Connection.Execute(sql, testItems, transaction: db.Transaction);
        }

        private void StageCoreItemLocaleData()
        {
            testStagingItemLocaleData = new List<StagingItemLocaleModel>
            {
                new TestStagingItemLocaleModelBuilder().WithScanCode(testScanCodes[0])
                .WithBusinessUnit(testBusinessUnitId).WithTimestamp(now).WithRegion(this.testRegion).WithTransactionId(this.transactionId).Build(),
                new TestStagingItemLocaleModelBuilder().WithScanCode(testScanCodes[1])
                .WithBusinessUnit(testBusinessUnitId).WithTimestamp(now).WithRegion(this.testRegion).WithTransactionId(this.transactionId).Build(),
                new TestStagingItemLocaleModelBuilder().WithScanCode(testScanCodes[2])
                .WithBusinessUnit(testBusinessUnitId).WithTimestamp(now).WithRegion(this.testRegion).WithTransactionId(this.transactionId).Build()
            };

            string sql = @"INSERT INTO stage.ItemLocale
                            (
	                            Region,
	                            BusinessUnitID,
	                            ScanCode,
	                            Discount_Case,
	                            Discount_TM,
	                            Restriction_Age,
	                            Restriction_Hours,
	                            Authorized,
	                            Discontinued,
	                            LabelTypeDesc,
	                            LocalItem,
	                            Product_Code,
	                            RetailUnit,
	                            Sign_Desc,
	                            Locality,
	                            Sign_RomanceText_Long,
	                            Sign_RomanceText_Short,
                                Msrp,
	                            Timestamp,
                                TransactionId
                            )
                            VALUES
                            (
	                            @Region,
	                            @BusinessUnitID,
	                            @ScanCode,
	                            @Discount_Case,
	                            @Discount_TM,
	                            @Restriction_Age,
	                            @Restriction_Hours,
	                            @Authorized,
	                            @Discontinued,
	                            @LabelTypeDesc,
	                            @LocalItem,
	                            @Product_Code,
	                            @RetailUnit,
	                            @Sign_Desc,
	                            @Locality,
	                            @Sign_RomanceText_Long,
	                            @Sign_RomanceText_Short,
                                @Msrp,
	                            @Timestamp,
                                @TransactionId
                            )";

            db.Connection.Execute(sql, testStagingItemLocaleData, transaction: db.Transaction);
        }

        private void StageExtendedItemLocaleData()
        {
            testStagingExtendedItemLocaleData = new List<List<StagingItemLocaleExtendedModel>>
            {
                new TestStagingExtendedItemLocaleModelBuilder().WithScanCode(testScanCodes[0])
                .WithBusinessUnit(testBusinessUnitId).WithTimestamp(now).WithRegion(this.testRegion).WithTransactionId(this.transactionId).Build(),
                new TestStagingExtendedItemLocaleModelBuilder().WithScanCode(testScanCodes[1])
                .WithBusinessUnit(testBusinessUnitId).WithTimestamp(now).WithRegion(this.testRegion).WithTransactionId(this.transactionId).Build(),
                new TestStagingExtendedItemLocaleModelBuilder().WithScanCode(testScanCodes[2])
                .WithBusinessUnit(testBusinessUnitId).WithTimestamp(now).WithRegion(this.testRegion).WithTransactionId(this.transactionId).Build()
            };

            foreach (var extendedItemSet in testStagingExtendedItemLocaleData)
            {
                string sql = @"INSERT INTO stage.ItemLocaleExtended
                                (
	                                Region,
	                                BusinessUnitID,
	                                ScanCode,
	                                AttributeId,
                                    AttributeValue,
	                                Timestamp,
                                    TransactionId
                                )
                                VALUES
                                (
	                                @Region,
	                                @BusinessUnitID,
	                                @ScanCode,
	                                @AttributeId,
                                    @AttributeValue,
	                                @Timestamp,
                                    @TransactionId
                                )";

                db.Connection.Execute(sql, extendedItemSet, transaction: db.Transaction);
            }
        }

        private void StageDataWithSignRomanceLong(string romanceLong)
        {
            testStagingItemLocaleData = new List<StagingItemLocaleModel>
            {
                new TestStagingItemLocaleModelBuilder().WithScanCode(testScanCodes[0])
                    .WithBusinessUnit(testBusinessUnitId).WithTimestamp(now).WithTransactionId(this.transactionId).Build(),
                new TestStagingItemLocaleModelBuilder().WithScanCode(testScanCodes[1])
                    .WithBusinessUnit(testBusinessUnitId).WithTimestamp(now).WithTransactionId(this.transactionId).Build(),
                new TestStagingItemLocaleModelBuilder().WithScanCode(testScanCodes[2])
                    .WithBusinessUnit(testBusinessUnitId).WithTimestamp(now).WithTransactionId(this.transactionId).WithRomanceLong(romanceLong).Build()
            };

            string sql = @"INSERT INTO stage.ItemLocale
                            (
	                            Region,
	                            BusinessUnitID,
	                            ScanCode,
	                            Discount_Case,
	                            Discount_TM,
	                            Restriction_Age,
	                            Restriction_Hours,
	                            Authorized,
	                            Discontinued,
	                            LabelTypeDesc,
	                            LocalItem,
	                            Product_Code,
	                            RetailUnit,
	                            Sign_Desc,
	                            Locality,
	                            Sign_RomanceText_Long,
	                            Sign_RomanceText_Short,
                                Msrp,
	                            Timestamp,
                                TransactionId
                            )
                            VALUES
                            (
	                            @Region,
	                            @BusinessUnitID,
	                            @ScanCode,
	                            @Discount_Case,
	                            @Discount_TM,
	                            @Restriction_Age,
	                            @Restriction_Hours,
	                            @Authorized,
	                            @Discontinued,
	                            @LabelTypeDesc,
	                            @LocalItem,
	                            @Product_Code,
	                            @RetailUnit,
	                            @Sign_Desc,
	                            @Locality,
	                            @Sign_RomanceText_Long,
	                            @Sign_RomanceText_Short,
                                @Msrp,
	                            @Timestamp,
                                @TransactionId
                            )";

            db.Connection.Execute(sql, testStagingItemLocaleData, transaction: db.Transaction);
        }

        [TestMethod]
        public void AddToMessageQueueItemLocale_NoStagedData_NoMessagesShouldBeQueued()
        {
            // Given.
            var command = new AddEsbMessageQueueItemLocaleCommand
            {
                Region = testRegion,
                Timestamp = now
            };

            // When.
            commandHandler.Execute(command);

            // Then.
            dynamic queuedMessages = db.Connection.Query<dynamic>("select * from esb.MessageQueueItemLocale where RegionCode = @Region and InsertDate > @Timestamp",
                new { Region = testRegion, Timestamp = now.Subtract(TimeSpan.FromMilliseconds(1000)) }, transaction: db.Transaction).ToList();

            Assert.AreEqual(0, queuedMessages.Count);
        }

        [TestMethod]
        public void AddToMessageQueueItemLocale_NoStagedDataForGivenRegion_NoMessagesShouldBeQueued()
        {
            // Given.
            var command = new AddEsbMessageQueueItemLocaleCommand
            {
                Region = "NA",
                Timestamp = now
            };

            // When.
            commandHandler.Execute(command);

            // Then.
            dynamic queuedMessages = db.Connection.Query<dynamic>("select * from esb.MessageQueueItemLocale where RegionCode = @Region and InsertDate > @Timestamp",
                new { Region = testRegion, Timestamp = now.Subtract(TimeSpan.FromMilliseconds(1000)) }, transaction: db.Transaction).ToList();

            Assert.AreEqual(0, queuedMessages.Count);
        }

        [TestMethod]
        public void AddToMessageQueueItemLocale_NoStagedDataForGivenTimestamp_NoMessagesShouldBeQueued()
        {
            // Given.
            var command = new AddEsbMessageQueueItemLocaleCommand
            {
                Region = testRegion,
                Timestamp = now.AddSeconds(1)
            };

            // When.
            commandHandler.Execute(command);

            // Then.
            dynamic queuedMessages = db.Connection.Query<dynamic>("select * from esb.MessageQueueItemLocale where RegionCode = @Region and InsertDate > @Timestamp",
                new { Region = testRegion, Timestamp = now.Subtract(TimeSpan.FromMilliseconds(1000)) }, transaction: db.Transaction).ToList();

            Assert.AreEqual(0, queuedMessages.Count);
        }

        [TestMethod]
        public void AddToMessageQueueItemLocale_CoreItemLocaleDataIsStaged_CoreItemLocaleDataShouldBeQueued()
        {
            // Given.
            StageCoreItemLocaleData();

            var command = new AddEsbMessageQueueItemLocaleCommand
            {
                Region = testRegion,
                Timestamp = now,
                TransactionId = this.transactionId
            };

            // When.
            commandHandler.Execute(command);

            // Then.
            dynamic queuedMessages = db.Connection.Query<dynamic>("select * from esb.MessageQueueItemLocale where RegionCode = @Region and InsertDate > @Timestamp",
                new { Region = testRegion, Timestamp = now.Subtract(TimeSpan.FromMilliseconds(1000)) }, transaction: db.Transaction).ToList();

            Assert.AreEqual(testStagingItemLocaleData.Count, queuedMessages.Count);

            for (int i = 0; i < queuedMessages.Count; i++)
            {
                Assert.AreEqual(MessageTypes.ItemLocale, queuedMessages[i].MessageTypeId);
                Assert.AreEqual(MessageStatusTypes.Ready, queuedMessages[i].MessageStatusId);
                Assert.IsNull(queuedMessages[i].MessageHistoryId);
                Assert.AreEqual(MessageActions.AddOrUpdate, queuedMessages[i].MessageActionId);
                Assert.AreEqual(now.Date, queuedMessages[i].InsertDate.Date);
                Assert.AreEqual(testRegion, queuedMessages[i].RegionCode);
                Assert.AreEqual(testBusinessUnitId, queuedMessages[i].BusinessUnitId);
                Assert.AreEqual(testItems[i].ItemID, queuedMessages[i].ItemId);
                Assert.AreEqual(ItemTypes.Codes.RetailSale, queuedMessages[i].ItemTypeCode);
                Assert.AreEqual(ItemTypes.Descriptions.RetailSale, queuedMessages[i].ItemTypeDesc);
                Assert.AreEqual(testLocale.StoreName, queuedMessages[i].LocaleName);
                Assert.AreEqual(testScanCodes[i], queuedMessages[i].ScanCode);
                Assert.AreEqual(testStagingItemLocaleData[i].Discount_Case, queuedMessages[i].CaseDiscount);
                Assert.AreEqual(testStagingItemLocaleData[i].Discount_TM, queuedMessages[i].TmDiscount);
                Assert.AreEqual(testStagingItemLocaleData[i].Restriction_Age, queuedMessages[i].AgeRestriction);
                Assert.AreEqual(testStagingItemLocaleData[i].Restriction_Hours, queuedMessages[i].RestrictedHours);
                Assert.AreEqual(testStagingItemLocaleData[i].Authorized, queuedMessages[i].Authorized);
                Assert.AreEqual(testStagingItemLocaleData[i].Discontinued, queuedMessages[i].Discontinued);
                Assert.AreEqual(testStagingItemLocaleData[i].LabelTypeDesc, queuedMessages[i].LabelTypeDescription);
                Assert.AreEqual(testStagingItemLocaleData[i].LocalItem, queuedMessages[i].LocalItem);
                Assert.AreEqual(testStagingItemLocaleData[i].Product_Code, queuedMessages[i].ProductCode);
                Assert.AreEqual(testStagingItemLocaleData[i].RetailUnit, queuedMessages[i].RetailUnit);
                Assert.AreEqual(testStagingItemLocaleData[i].Discontinued, queuedMessages[i].Discontinued);
                Assert.AreEqual(testStagingItemLocaleData[i].Sign_Desc, queuedMessages[i].SignDescription);
                Assert.AreEqual(testStagingItemLocaleData[i].Locality, queuedMessages[i].Locality);
                Assert.AreEqual(testStagingItemLocaleData[i].Sign_RomanceText_Long, queuedMessages[i].SignRomanceLong);
                Assert.AreEqual(testStagingItemLocaleData[i].Sign_RomanceText_Short, queuedMessages[i].SignRomanceShort);
                Assert.AreEqual(testStagingItemLocaleData[i].Msrp, queuedMessages[i].Msrp);
                Assert.IsNull(queuedMessages[i].ColorAdded);
                Assert.IsNull(queuedMessages[i].CountryOfProcessing);
                Assert.IsNull(queuedMessages[i].Origin);
                Assert.IsNull(queuedMessages[i].ElectronicShelfTag);
                Assert.IsNull(queuedMessages[i].Exclusive);
                Assert.IsNull(queuedMessages[i].NumberOfDigitsSentToScale);
                Assert.IsNull(queuedMessages[i].ChicagoBaby);
                Assert.IsNull(queuedMessages[i].TagUom);
                Assert.IsNull(queuedMessages[i].LinkedItem);
                Assert.IsNull(queuedMessages[i].ScaleExtraText);
                Assert.IsNull(queuedMessages[i].InProcessBy);
                Assert.IsNull(queuedMessages[i].ProcessedDate);
            }
        }

        [TestMethod]
        public void AddToMessageQueueItemLocale_ExtendedItemLocaleDataIsStaged_ExtendedItemLocaleDataShouldBeQueued()
        {
            // Given.
            StageCoreItemLocaleData();
            StageExtendedItemLocaleData();

            var command = new AddEsbMessageQueueItemLocaleCommand
            {
                Region = testRegion,
                Timestamp = now,
                TransactionId = this.transactionId
            };

            // When.
            commandHandler.Execute(command);

            // Then.
            dynamic queuedMessages = db.Connection.Query<dynamic>("select * from esb.MessageQueueItemLocale where RegionCode = @Region and InsertDate > @Timestamp",
                new { Region = testRegion, Timestamp = now.Subtract(TimeSpan.FromMilliseconds(10000)) }, transaction: db.Transaction).ToList();

            Assert.AreEqual(testStagingItemLocaleData.Count, queuedMessages.Count);

            for (int i = 0; i < queuedMessages.Count; i++)
            {
                Assert.AreEqual(MessageTypes.ItemLocale, queuedMessages[i].MessageTypeId);
                Assert.AreEqual(MessageStatusTypes.Ready, queuedMessages[i].MessageStatusId);
                Assert.IsNull(queuedMessages[i].MessageHistoryId);
                Assert.AreEqual(MessageActions.AddOrUpdate, queuedMessages[i].MessageActionId);
                Assert.AreEqual(now.Date, queuedMessages[i].InsertDate.Date);
                Assert.AreEqual(testRegion, queuedMessages[i].RegionCode);
                Assert.AreEqual(testBusinessUnitId, queuedMessages[i].BusinessUnitId);
                Assert.AreEqual(testItems[i].ItemID, queuedMessages[i].ItemId);
                Assert.AreEqual(ItemTypes.Codes.RetailSale, queuedMessages[i].ItemTypeCode);
                Assert.AreEqual(ItemTypes.Descriptions.RetailSale, queuedMessages[i].ItemTypeDesc);
                Assert.AreEqual(testLocale.StoreName, queuedMessages[i].LocaleName);
                Assert.AreEqual(testScanCodes[i], queuedMessages[i].ScanCode);
                Assert.AreEqual(testStagingItemLocaleData[i].Discount_Case, queuedMessages[i].CaseDiscount);
                Assert.AreEqual(testStagingItemLocaleData[i].Discount_TM, queuedMessages[i].TmDiscount);
                Assert.AreEqual(testStagingItemLocaleData[i].Restriction_Age, queuedMessages[i].AgeRestriction);
                Assert.AreEqual(testStagingItemLocaleData[i].Restriction_Hours, queuedMessages[i].RestrictedHours);
                Assert.AreEqual(testStagingItemLocaleData[i].Authorized, queuedMessages[i].Authorized);
                Assert.AreEqual(testStagingItemLocaleData[i].Discontinued, queuedMessages[i].Discontinued);
                Assert.AreEqual(testStagingItemLocaleData[i].LabelTypeDesc, queuedMessages[i].LabelTypeDescription);
                Assert.AreEqual(testStagingItemLocaleData[i].LocalItem, queuedMessages[i].LocalItem);
                Assert.AreEqual(testStagingItemLocaleData[i].Product_Code, queuedMessages[i].ProductCode);
                Assert.AreEqual(testStagingItemLocaleData[i].RetailUnit, queuedMessages[i].RetailUnit);
                Assert.AreEqual(testStagingItemLocaleData[i].Discontinued, queuedMessages[i].Discontinued);
                Assert.AreEqual(testStagingItemLocaleData[i].Sign_Desc, queuedMessages[i].SignDescription);
                Assert.AreEqual(testStagingItemLocaleData[i].Locality, queuedMessages[i].Locality);
                Assert.AreEqual(testStagingItemLocaleData[i].Sign_RomanceText_Long, queuedMessages[i].SignRomanceLong);
                Assert.AreEqual(testStagingItemLocaleData[i].Sign_RomanceText_Short, queuedMessages[i].SignRomanceShort);
                Assert.AreEqual(testStagingItemLocaleData[i].Msrp, queuedMessages[i].Msrp);
                Assert.AreEqual(!testStagingExtendedItemLocaleData[i].Single(il => il.AttributeId == Attributes.ColorAdded).AttributeValue.Equals("0"), queuedMessages[i].ColorAdded);
                Assert.AreEqual(testStagingExtendedItemLocaleData[i].Single(il => il.AttributeId == Attributes.CountryOfProcessing).AttributeValue, queuedMessages[i].CountryOfProcessing);
                Assert.AreEqual(testStagingExtendedItemLocaleData[i].Single(il => il.AttributeId == Attributes.Origin).AttributeValue, queuedMessages[i].Origin);
                Assert.AreEqual(!testStagingExtendedItemLocaleData[i].Single(il => il.AttributeId == Attributes.ElectronicShelfTag).AttributeValue.Equals("0"), queuedMessages[i].ElectronicShelfTag);
                Assert.AreEqual(testStagingExtendedItemLocaleData[i].Single(il => il.AttributeId == Attributes.Exclusive).AttributeValue, queuedMessages[i].Exclusive.ToString());
                Assert.AreEqual(testStagingExtendedItemLocaleData[i].Single(il => il.AttributeId == Attributes.NumberOfDigitsSentToScale).AttributeValue, queuedMessages[i].NumberOfDigitsSentToScale.ToString());
                Assert.AreEqual(testStagingExtendedItemLocaleData[i].Single(il => il.AttributeId == Attributes.ChicagoBaby).AttributeValue, queuedMessages[i].ChicagoBaby);
                Assert.AreEqual(testStagingExtendedItemLocaleData[i].Single(il => il.AttributeId == Attributes.TagUom).AttributeValue, queuedMessages[i].TagUom);
                Assert.AreEqual(testStagingExtendedItemLocaleData[i].Single(il => il.AttributeId == Attributes.LinkedScanCode).AttributeValue, queuedMessages[i].LinkedItem);
                Assert.AreEqual(testStagingExtendedItemLocaleData[i].Single(il => il.AttributeId == Attributes.ScaleExtraText).AttributeValue, queuedMessages[i].ScaleExtraText);
                Assert.IsNull(queuedMessages[i].InProcessBy);
                Assert.IsNull(queuedMessages[i].ProcessedDate);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(SqlException))]
        public void AddToMessageQueueItemLocale_SignRomanceTextLong305Lenght_ExtendedItemLocaleDataShouldNotBeQueued()
        {
            // Given.
            StageDataWithSignRomanceLong("ERROR123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890");
            StageExtendedItemLocaleData();

            var command = new AddEsbMessageQueueItemLocaleCommand
            {
                Region = testRegion,
                Timestamp = now,
                TransactionId = this.transactionId
            };

            // When.
            commandHandler.Execute(command);

            // Then.
            dynamic queuedMessages = db.Connection.Query<dynamic>("select * from esb.MessageQueueItemLocale where RegionCode = @Region and InsertDate > @Timestamp",
                new { Region = testRegion, Timestamp = now.Subtract(TimeSpan.FromMilliseconds(1000)) }, transaction: db.Transaction).ToList();

            Assert.AreEqual(testStagingItemLocaleData.Count, queuedMessages.Count);

            for (int i = 0; i < queuedMessages.Count; i++)
            {
                Assert.AreEqual(MessageTypes.ItemLocale, queuedMessages[i].MessageTypeId);
                Assert.AreEqual(MessageStatusTypes.Ready, queuedMessages[i].MessageStatusId);
                Assert.IsNull(queuedMessages[i].MessageHistoryId);
                Assert.AreEqual(MessageActions.AddOrUpdate, queuedMessages[i].MessageActionId);
                Assert.AreEqual(now.Date, queuedMessages[i].InsertDate.Date);
                Assert.AreEqual(testRegion, queuedMessages[i].RegionCode);
                Assert.AreEqual(testBusinessUnitId, queuedMessages[i].BusinessUnitId);
                Assert.AreEqual(testItems[i].ItemID, queuedMessages[i].ItemId);
                Assert.AreEqual(ItemTypes.Codes.RetailSale, queuedMessages[i].ItemTypeCode);
                Assert.AreEqual(ItemTypes.Descriptions.RetailSale, queuedMessages[i].ItemTypeDesc);
                Assert.AreEqual(testLocale.StoreName, queuedMessages[i].LocaleName);
                Assert.AreEqual(testScanCodes[i], queuedMessages[i].ScanCode);
                Assert.AreEqual(testStagingItemLocaleData[i].Discount_Case, queuedMessages[i].CaseDiscount);
                Assert.AreEqual(testStagingItemLocaleData[i].Discount_TM, queuedMessages[i].TmDiscount);
                Assert.AreEqual(testStagingItemLocaleData[i].Restriction_Age, queuedMessages[i].AgeRestriction);
                Assert.AreEqual(testStagingItemLocaleData[i].Restriction_Hours, queuedMessages[i].RestrictedHours);
                Assert.AreEqual(testStagingItemLocaleData[i].Authorized, queuedMessages[i].Authorized);
                Assert.AreEqual(testStagingItemLocaleData[i].Discontinued, queuedMessages[i].Discontinued);
                Assert.AreEqual(testStagingItemLocaleData[i].LabelTypeDesc, queuedMessages[i].LabelTypeDescription);
                Assert.AreEqual(testStagingItemLocaleData[i].LocalItem, queuedMessages[i].LocalItem);
                Assert.AreEqual(testStagingItemLocaleData[i].Product_Code, queuedMessages[i].ProductCode);
                Assert.AreEqual(testStagingItemLocaleData[i].RetailUnit, queuedMessages[i].RetailUnit);
                Assert.AreEqual(testStagingItemLocaleData[i].Discontinued, queuedMessages[i].Discontinued);
                Assert.AreEqual(testStagingItemLocaleData[i].Sign_Desc, queuedMessages[i].SignDescription);
                Assert.AreEqual(testStagingItemLocaleData[i].Locality, queuedMessages[i].Locality);
                Assert.AreEqual(testStagingItemLocaleData[i].Sign_RomanceText_Long, queuedMessages[i].SignRomanceLong);
                Assert.AreEqual(testStagingItemLocaleData[i].Sign_RomanceText_Short, queuedMessages[i].SignRomanceShort);
                Assert.AreEqual(testStagingItemLocaleData[i].Msrp, queuedMessages[i].Msrp);
                Assert.AreEqual(!testStagingExtendedItemLocaleData[i].Single(il => il.AttributeId == Attributes.ColorAdded).AttributeValue.Equals("0"), queuedMessages[i].ColorAdded);
                Assert.AreEqual(testStagingExtendedItemLocaleData[i].Single(il => il.AttributeId == Attributes.CountryOfProcessing).AttributeValue, queuedMessages[i].CountryOfProcessing);
                Assert.AreEqual(testStagingExtendedItemLocaleData[i].Single(il => il.AttributeId == Attributes.Origin).AttributeValue, queuedMessages[i].Origin);
                Assert.AreEqual(!testStagingExtendedItemLocaleData[i].Single(il => il.AttributeId == Attributes.ElectronicShelfTag).AttributeValue.Equals("0"), queuedMessages[i].ElectronicShelfTag);
                Assert.AreEqual(testStagingExtendedItemLocaleData[i].Single(il => il.AttributeId == Attributes.Exclusive).AttributeValue, queuedMessages[i].Exclusive.ToString());
                Assert.AreEqual(testStagingExtendedItemLocaleData[i].Single(il => il.AttributeId == Attributes.NumberOfDigitsSentToScale).AttributeValue, queuedMessages[i].NumberOfDigitsSentToScale.ToString());
                Assert.AreEqual(testStagingExtendedItemLocaleData[i].Single(il => il.AttributeId == Attributes.ChicagoBaby).AttributeValue, queuedMessages[i].ChicagoBaby);
                Assert.AreEqual(testStagingExtendedItemLocaleData[i].Single(il => il.AttributeId == Attributes.TagUom).AttributeValue, queuedMessages[i].TagUom);
                Assert.AreEqual(testStagingExtendedItemLocaleData[i].Single(il => il.AttributeId == Attributes.LinkedScanCode).AttributeValue, queuedMessages[i].LinkedItem);
                Assert.AreEqual(testStagingExtendedItemLocaleData[i].Single(il => il.AttributeId == Attributes.ScaleExtraText).AttributeValue, queuedMessages[i].ScaleExtraText);
                Assert.IsNull(queuedMessages[i].InProcessBy);
                Assert.IsNull(queuedMessages[i].ProcessedDate);
            }
        }

        [TestMethod]
        public void AddToMessageQueueItemLocale_SignRomanceTextLong300Length_ExtendedItemLocaleDataShouldBeQueued()
        {
            // Given.
            StageDataWithSignRomanceLong("123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890");
            StageExtendedItemLocaleData();

            var command = new AddEsbMessageQueueItemLocaleCommand
            {
                Region = testRegion,
                Timestamp = now,
                TransactionId = this.transactionId
            };

            // When.
            commandHandler.Execute(command);

            // Then.
            dynamic queuedMessages = db.Connection.Query<dynamic>("select * from esb.MessageQueueItemLocale where RegionCode = @Region and InsertDate > @Timestamp",
                new { Region = testRegion, Timestamp = now.Subtract(TimeSpan.FromMilliseconds(1000)) }, transaction: db.Transaction).ToList();

            Assert.AreEqual(testStagingItemLocaleData.Count, queuedMessages.Count);

            for (int i = 0; i < queuedMessages.Count; i++)
            {
                Assert.AreEqual(MessageTypes.ItemLocale, queuedMessages[i].MessageTypeId);
                Assert.AreEqual(MessageStatusTypes.Ready, queuedMessages[i].MessageStatusId);
                Assert.IsNull(queuedMessages[i].MessageHistoryId);
                Assert.AreEqual(MessageActions.AddOrUpdate, queuedMessages[i].MessageActionId);
                Assert.AreEqual(now.Date, queuedMessages[i].InsertDate.Date);
                Assert.AreEqual(testRegion, queuedMessages[i].RegionCode);
                Assert.AreEqual(testBusinessUnitId, queuedMessages[i].BusinessUnitId);
                Assert.AreEqual(testItems[i].ItemID, queuedMessages[i].ItemId);
                Assert.AreEqual(ItemTypes.Codes.RetailSale, queuedMessages[i].ItemTypeCode);
                Assert.AreEqual(ItemTypes.Descriptions.RetailSale, queuedMessages[i].ItemTypeDesc);
                Assert.AreEqual(testLocale.StoreName, queuedMessages[i].LocaleName);
                Assert.AreEqual(testScanCodes[i], queuedMessages[i].ScanCode);
                Assert.AreEqual(testStagingItemLocaleData[i].Discount_Case, queuedMessages[i].CaseDiscount);
                Assert.AreEqual(testStagingItemLocaleData[i].Discount_TM, queuedMessages[i].TmDiscount);
                Assert.AreEqual(testStagingItemLocaleData[i].Restriction_Age, queuedMessages[i].AgeRestriction);
                Assert.AreEqual(testStagingItemLocaleData[i].Restriction_Hours, queuedMessages[i].RestrictedHours);
                Assert.AreEqual(testStagingItemLocaleData[i].Authorized, queuedMessages[i].Authorized);
                Assert.AreEqual(testStagingItemLocaleData[i].Discontinued, queuedMessages[i].Discontinued);
                Assert.AreEqual(testStagingItemLocaleData[i].LabelTypeDesc, queuedMessages[i].LabelTypeDescription);
                Assert.AreEqual(testStagingItemLocaleData[i].LocalItem, queuedMessages[i].LocalItem);
                Assert.AreEqual(testStagingItemLocaleData[i].Product_Code, queuedMessages[i].ProductCode);
                Assert.AreEqual(testStagingItemLocaleData[i].RetailUnit, queuedMessages[i].RetailUnit);
                Assert.AreEqual(testStagingItemLocaleData[i].Discontinued, queuedMessages[i].Discontinued);
                Assert.AreEqual(testStagingItemLocaleData[i].Sign_Desc, queuedMessages[i].SignDescription);
                Assert.AreEqual(testStagingItemLocaleData[i].Locality, queuedMessages[i].Locality);
                Assert.AreEqual(testStagingItemLocaleData[i].Sign_RomanceText_Long, queuedMessages[i].SignRomanceLong);
                Assert.AreEqual(testStagingItemLocaleData[i].Sign_RomanceText_Short, queuedMessages[i].SignRomanceShort);
                Assert.AreEqual(testStagingItemLocaleData[i].Msrp, queuedMessages[i].Msrp);
                Assert.AreEqual(!testStagingExtendedItemLocaleData[i].Single(il => il.AttributeId == Attributes.ColorAdded).AttributeValue.Equals("0"), queuedMessages[i].ColorAdded);
                Assert.AreEqual(testStagingExtendedItemLocaleData[i].Single(il => il.AttributeId == Attributes.CountryOfProcessing).AttributeValue, queuedMessages[i].CountryOfProcessing);
                Assert.AreEqual(testStagingExtendedItemLocaleData[i].Single(il => il.AttributeId == Attributes.Origin).AttributeValue, queuedMessages[i].Origin);
                Assert.AreEqual(!testStagingExtendedItemLocaleData[i].Single(il => il.AttributeId == Attributes.ElectronicShelfTag).AttributeValue.Equals("0"), queuedMessages[i].ElectronicShelfTag);
                Assert.AreEqual(testStagingExtendedItemLocaleData[i].Single(il => il.AttributeId == Attributes.Exclusive).AttributeValue, queuedMessages[i].Exclusive.ToString());
                Assert.AreEqual(testStagingExtendedItemLocaleData[i].Single(il => il.AttributeId == Attributes.NumberOfDigitsSentToScale).AttributeValue, queuedMessages[i].NumberOfDigitsSentToScale.ToString());
                Assert.AreEqual(testStagingExtendedItemLocaleData[i].Single(il => il.AttributeId == Attributes.ChicagoBaby).AttributeValue, queuedMessages[i].ChicagoBaby);
                Assert.AreEqual(testStagingExtendedItemLocaleData[i].Single(il => il.AttributeId == Attributes.TagUom).AttributeValue, queuedMessages[i].TagUom);
                Assert.AreEqual(testStagingExtendedItemLocaleData[i].Single(il => il.AttributeId == Attributes.LinkedScanCode).AttributeValue, queuedMessages[i].LinkedItem);
                Assert.AreEqual(testStagingExtendedItemLocaleData[i].Single(il => il.AttributeId == Attributes.ScaleExtraText).AttributeValue, queuedMessages[i].ScaleExtraText);
                Assert.IsNull(queuedMessages[i].InProcessBy);
                Assert.IsNull(queuedMessages[i].ProcessedDate);
            }
        }
    }
}
