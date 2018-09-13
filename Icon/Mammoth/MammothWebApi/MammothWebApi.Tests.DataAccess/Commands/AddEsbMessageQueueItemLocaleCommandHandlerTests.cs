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
        private List<StagingItemLocaleSupplierModel> testStagingItemLocaleSupplierModel;
        private List<Item> testItems;
        private List<string> testScanCodes;
        private Locales testLocale;
        private int testBusinessUnitId;
        private int itemTypeId;
        private string testSupplierName;
        private string testSupplierItemId;
        private List<bool> testDefaultScanCodes;
        private List<int> testIrmaItemKeys;

        [TestInitialize]
        public void Initialize()
        {
            now = DateTime.Now;
            transactionId = Guid.NewGuid();
            testRegion = "SW";
            testBusinessUnitId = 44444;
            testSupplierName = "Test Supplier";
            testSupplierItemId = "Test Suppplier";
            testDefaultScanCodes = new List<bool>
            {
                true,
                false,
                true
            };
            testIrmaItemKeys = new List<int>
            {
                45678901,
                45678902,
                45678903
            };

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

        private void StageItemLocaleSupplierData()
        {
            testStagingItemLocaleSupplierModel = new List<StagingItemLocaleSupplierModel>
            {
           new TestStagingItemLocaleSupplierModelBuilder().WithBusinessUnit(testBusinessUnitId).WithScanCode(testScanCodes[0])
                .WithTimestamp(now).WithRegion(this.testRegion).WithTransactionId(this.transactionId)
                .WithSupplierName(testSupplierName).WithSupplierItemId(testSupplierItemId).Build(),
           new TestStagingItemLocaleSupplierModelBuilder().WithBusinessUnit(testBusinessUnitId).WithScanCode(testScanCodes[1])
                .WithTimestamp(now).WithRegion(this.testRegion).WithTransactionId(this.transactionId)
                .WithSupplierName(testSupplierName).WithSupplierItemId(testSupplierItemId).Build(),
           new TestStagingItemLocaleSupplierModelBuilder().WithBusinessUnit(testBusinessUnitId).WithScanCode(testScanCodes[2])
                .WithTimestamp(now).WithRegion(this.testRegion).WithTransactionId(this.transactionId)
                .WithSupplierName(testSupplierName).WithSupplierItemId(testSupplierItemId).Build(),
            };

            string sql = @"INSERT INTO stage.ItemLocaleSupplier
                            (
	                            Region,
                                ScanCode,
	                            BusinessUnitID,
	                            SupplierName,
	                            SupplierItemId,
	                            SupplierCaseSize,
	                            IrmaVendorKey,
	                            Timestamp,
                                TransactionId
                            )
                            VALUES
                            (
	                            @Region,
                                @ScanCode,
	                            @BusinessUnitID,
	                            @SupplierName,
	                            @SupplierItemId,
	                            @SupplierCaseSize,
	                            @IrmaVendorKey,
	                            @Timestamp,
                                @TransactionId
                            )";

            db.Connection.Execute(sql, testStagingItemLocaleSupplierModel, transaction: db.Transaction);
        }

        private void StageCoreItemLocaleData(Dictionary<string,object> customItemLocaleProperties = null)
        {
            testStagingItemLocaleData = new List<StagingItemLocaleModel>
            {
                new TestStagingItemLocaleModelBuilder().WithScanCode(testScanCodes[0]).WithOrderedByInfor(true).WithAltRetailSize(9.8m).WithAltRetailUom("EA")
                    .WithBusinessUnit(testBusinessUnitId).WithTimestamp(now).WithRegion(this.testRegion).WithTransactionId(this.transactionId)
                    .WithDefaultScanCode(testDefaultScanCodes[0]).WithIrmaItemKey(testIrmaItemKeys[0]).Build(),                 
                new TestStagingItemLocaleModelBuilder().WithScanCode(testScanCodes[1]).WithOrderedByInfor(true).WithAltRetailSize(9.8m).WithAltRetailUom("EA")
                    .WithBusinessUnit(testBusinessUnitId).WithTimestamp(now).WithRegion(this.testRegion).WithTransactionId(this.transactionId)
                    .WithDefaultScanCode(testDefaultScanCodes[1]).WithIrmaItemKey(testIrmaItemKeys[1]).Build(),
                new TestStagingItemLocaleModelBuilder().WithScanCode(testScanCodes[2]).WithOrderedByInfor(true).WithAltRetailSize(9.8m).WithAltRetailUom("EA")
                    .WithBusinessUnit(testBusinessUnitId).WithTimestamp(now).WithRegion(this.testRegion).WithTransactionId(this.transactionId)
                    .WithDefaultScanCode(testDefaultScanCodes[1]).WithIrmaItemKey(testIrmaItemKeys[2]).Build(),
            };

            if (customItemLocaleProperties != null)
            {
                foreach (var customPropertyNameValuePair in customItemLocaleProperties)
                {
                    foreach (var testDataItem in testStagingItemLocaleData)
                    {
                        AssignCustomPropertyValue(customPropertyNameValuePair, testDataItem);
                    }
                }
            }

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
                                ScaleItem,
	                            Product_Code,
	                            RetailUnit,
	                            Sign_Desc,
	                            Locality,
	                            Sign_RomanceText_Long,
	                            Sign_RomanceText_Short,
                                Msrp,
	                            Timestamp,
                                TransactionId,
                                OrderedByInfor,
                                AltRetailSize,
                                AltRetailUOM,
                                DefaultScanCode,
                                IrmaItemKey
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
                                @ScaleItem,
	                            @Product_Code,
	                            @RetailUnit,
	                            @Sign_Desc,
	                            @Locality,
	                            @Sign_RomanceText_Long,
	                            @Sign_RomanceText_Short,
                                @Msrp,
	                            @Timestamp,
                                @TransactionId,
                                @OrderedByInfor,
                                @AltRetailSize,
                                @AltRetailUOM,
                                @DefaultScanCode,
                                @IrmaItemKey
                            )";

            db.Connection.Execute(sql, testStagingItemLocaleData, transaction: db.Transaction);
        }

        private void AssignCustomPropertyValue(KeyValuePair<string,object> customPropertyNameValuePair, StagingItemLocaleModel testDataItem)
        {
            switch (customPropertyNameValuePair.Key)
            {
                case nameof(StagingItemLocaleModel.Region):
                    testDataItem.Region = (string)customPropertyNameValuePair.Value;
                    break;
                case nameof(StagingItemLocaleModel.BusinessUnitID):
                    testDataItem.BusinessUnitID = (int)customPropertyNameValuePair.Value;
                    break;
                case nameof(StagingItemLocaleModel.ScanCode):
                    testDataItem.ScanCode = (string)customPropertyNameValuePair.Value;
                    break;
                case nameof(StagingItemLocaleModel.Discount_Case):
                    testDataItem.Discount_Case = (bool)customPropertyNameValuePair.Value;
                    break;
                case nameof(StagingItemLocaleModel.Discount_TM):
                    testDataItem.Discount_TM = (bool)customPropertyNameValuePair.Value;
                    break;
                case nameof(StagingItemLocaleModel.Restriction_Age):
                    testDataItem.Restriction_Age = (int?)customPropertyNameValuePair.Value;
                    break;
                case nameof(StagingItemLocaleModel.Restriction_Hours):
                    testDataItem.Restriction_Hours = (bool)customPropertyNameValuePair.Value;
                    break;
                case nameof(StagingItemLocaleModel.Authorized):
                    testDataItem.Authorized = (bool)customPropertyNameValuePair.Value;
                    break;
                case nameof(StagingItemLocaleModel.Discontinued):
                    testDataItem.Discontinued = (bool)customPropertyNameValuePair.Value;
                    break;
                case nameof(StagingItemLocaleModel.LabelTypeDesc):
                    testDataItem.LabelTypeDesc = (string)customPropertyNameValuePair.Value;
                    break;
                case nameof(StagingItemLocaleModel.LocalItem):
                    testDataItem.LocalItem = (bool)customPropertyNameValuePair.Value;
                    break;
                case nameof(StagingItemLocaleModel.Product_Code):
                    testDataItem.Product_Code = (string)customPropertyNameValuePair.Value;
                    break;
                case nameof(StagingItemLocaleModel.RetailUnit):
                    testDataItem.RetailUnit = (string)customPropertyNameValuePair.Value;
                    break;
                case nameof(StagingItemLocaleModel.Sign_Desc):
                    testDataItem.Sign_Desc = (string)customPropertyNameValuePair.Value;
                    break;
                case nameof(StagingItemLocaleModel.Locality):
                    testDataItem.Locality = (string)customPropertyNameValuePair.Value;
                    break;
                case nameof(StagingItemLocaleModel.Sign_RomanceText_Long):
                    testDataItem.Sign_RomanceText_Long = (string)customPropertyNameValuePair.Value;
                    break;
                case nameof(StagingItemLocaleModel.Sign_RomanceText_Short):
                    testDataItem.Sign_RomanceText_Short = (string)customPropertyNameValuePair.Value;
                    break;
                case nameof(StagingItemLocaleModel.Msrp):
                    testDataItem.Msrp = (decimal)customPropertyNameValuePair.Value;
                    break;
                case nameof(StagingItemLocaleModel.OrderedByInfor):
                    testDataItem.OrderedByInfor = (bool)customPropertyNameValuePair.Value;
                    break;
                case nameof(StagingItemLocaleModel.AltRetailSize):
                    testDataItem.AltRetailSize = (decimal?)customPropertyNameValuePair.Value;
                    break;
                case nameof(StagingItemLocaleModel.AltRetailUOM):
                    testDataItem.AltRetailUOM = (string)customPropertyNameValuePair.Value;
                    break;
                case nameof(StagingItemLocaleModel.DefaultScanCode):
                    testDataItem.DefaultScanCode = (bool)customPropertyNameValuePair.Value;
                    break;
                case nameof(StagingItemLocaleModel.ScaleItem):
                    testDataItem.ScaleItem = (bool)customPropertyNameValuePair.Value;
                    break;
                case nameof(StagingItemLocaleModel.Timestamp):
                    testDataItem.Timestamp = (DateTime)customPropertyNameValuePair.Value;
                    break;
                case nameof(StagingItemLocaleModel.TransactionId):
                    testDataItem.TransactionId = (Guid)customPropertyNameValuePair.Value;
                    break;
                default:
                    break;
            }
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
            StageItemLocaleSupplierData();

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
                AssertQueuedMessageMatchesStagingItemLocaleModel(queuedMessages[i], testStagingItemLocaleData[i],
                    now.Date, testRegion, testBusinessUnitId, testLocale.StoreName, testItems[i].ItemID, testScanCodes[i]);
                AssertQueuedMessageMatchesStagingItemSupplierModel(queuedMessages[i], testStagingItemLocaleSupplierModel[i]);
            }
        }

        [TestMethod]
        public void AddToMessageQueueItemLocale_ExtendedItemLocaleDataIsStaged_ExtendedItemLocaleDataShouldBeQueued()
        {
            // Given.
            StageCoreItemLocaleData();
            StageItemLocaleSupplierData();
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
                AssertQueuedMessageMatchesStagingItemLocaleModel(queuedMessages[i], testStagingItemLocaleData[i],
                    now.Date, testRegion, testBusinessUnitId, testLocale.StoreName, testItems[i].ItemID, testScanCodes[i]);
                AssertQueuedMessageMatchesStagingItemLocaleExtendedModel(queuedMessages[i], testStagingExtendedItemLocaleData[i]);
                AssertQueuedMessageMatchesStagingItemSupplierModel(queuedMessages[i], testStagingItemLocaleSupplierModel[i]);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(SqlException))]
        public void AddToMessageQueueItemLocale_SignRomanceTextLong305Length_ExtendedItemLocaleDataShouldNotBeQueued()
        {
            // Given.
            string longSignRomanceText = "ERROR123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890";
            var customItemLocaleData = new Dictionary<string, object>()
            {
                {nameof(StagingItemLocaleModel.Sign_RomanceText_Long),longSignRomanceText }
            };
            StageCoreItemLocaleData(customItemLocaleData);
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
                AssertQueuedMessageMatchesStagingItemLocaleModel(queuedMessages[i], testStagingItemLocaleData[i],
                    now.Date, testRegion, testBusinessUnitId, testLocale.StoreName, testItems[i].ItemID, testScanCodes[i]);
                AssertQueuedMessageMatchesStagingItemLocaleExtendedModel(queuedMessages[i], testStagingExtendedItemLocaleData[i]);
            }
        }

        [TestMethod]
        public void AddToMessageQueueItemLocale_SignRomanceTextLong300Length_ExtendedItemLocaleDataShouldBeQueued()
        {
            // Given.
            string longSignRomanceText = "123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890";
            var customItemLocaleData = new Dictionary<string, object>()
            {
                {nameof(StagingItemLocaleModel.Sign_RomanceText_Long),longSignRomanceText }
            };
            StageCoreItemLocaleData(customItemLocaleData);
            StageExtendedItemLocaleData();
            StageItemLocaleSupplierData();

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
                AssertQueuedMessageMatchesStagingItemLocaleModel(queuedMessages[i], testStagingItemLocaleData[i],
                    now.Date, testRegion, testBusinessUnitId, testLocale.StoreName, testItems[i].ItemID, testScanCodes[i]);
                AssertQueuedMessageMatchesStagingItemLocaleExtendedModel(queuedMessages[i], testStagingExtendedItemLocaleData[i]);
            }
        }

        [TestMethod]
        public void AddToMessageQueueItemLocale_NoSupplierDataStaged_MessagesShouldStillBeQueued()
        {
            // Given.
            StageCoreItemLocaleData();
            // do NOT stage any item locale supplier data
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
            dynamic queuedMessages = db.Connection.Query<dynamic>(
                    "select * from esb.MessageQueueItemLocale where RegionCode = @Region and InsertDate > @Timestamp",
                    new { Region = testRegion, Timestamp = now.Subtract(TimeSpan.FromMilliseconds(1000)) }, transaction: db.Transaction)
                .OrderBy(i => i.ItemId) //Ordering the results so that the assert works below
                .ToList();

            // even with no supplier data, the messages should still have been queued
            Assert.AreEqual(testStagingItemLocaleData.Count, queuedMessages.Count);

            for (int i = 0; i < queuedMessages.Count; i++)
            {
                AssertQueuedMessageMatchesStagingItemLocaleModel(queuedMessages[i], testStagingItemLocaleData[i],
                    now.Date, testRegion, testBusinessUnitId, testLocale.StoreName, testItems[i].ItemID, testScanCodes[i]);
                AssertQueuedMessageMatchesStagingItemLocaleExtendedModel(queuedMessages[i], testStagingExtendedItemLocaleData[i]);
            }
        }

        [TestMethod]
        public void AddToMessageQueueItemLocale_DefaultScanCodeAndIrmaItemKey_MessagesQueuedWithExpectedValues()
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
            dynamic queuedMessages = db.Connection.Query<dynamic>(
                    "select * from esb.MessageQueueItemLocale where RegionCode = @Region and InsertDate > @Timestamp",
                    new { Region = testRegion, Timestamp = now.Subtract(TimeSpan.FromMilliseconds(1000)) }, transaction: db.Transaction)
                .OrderBy(i => i.ItemId) //Ordering the results so that the assert works below
                .ToList();

            Assert.AreEqual(testStagingItemLocaleData.Count, queuedMessages.Count);

            for (int i = 0; i < queuedMessages.Count; i++)
            {
                AssertQueuedMessageMatchesStagingItemLocaleModel(queuedMessages[i], testStagingItemLocaleData[i],
                    now.Date, testRegion, testBusinessUnitId, testLocale.StoreName, testItems[i].ItemID, testScanCodes[i],
                    testDefaultScanCodes[i], testIrmaItemKeys[i]);
            }
        }

        private void AssertQueuedMessageMatchesStagingItemLocaleModel(dynamic queuedMessage,
            StagingItemLocaleModel stagingItemLocaleModel,
            DateTime expectedDate,
            string expectedRegion,
            int expectedBusinessUnit,
            string expectedStoreName,
            int expectedItemID,
            string expectedScanCode,
            bool expectedDefaultScanCode = true,
            int? expectedIrmaItemKey = null,
            int expectedMessageType = MessageTypes.ItemLocale,
            int messageStatusType = MessageStatusTypes.Ready,
            int expectedMessageAction = MessageActions.AddOrUpdate,
            string expectedItemTypeCode = ItemTypes.Codes.RetailSale,
            string expectedItemTypeDescription = ItemTypes.Descriptions.RetailSale,
            int? expectedMessageHistoryId = null)
        {
            Assert.AreEqual(expectedDate, queuedMessage.InsertDate.Date);
            Assert.AreEqual(expectedRegion, queuedMessage.RegionCode);
            Assert.AreEqual(expectedBusinessUnit, queuedMessage.BusinessUnitId);
            Assert.AreEqual(expectedStoreName, queuedMessage.LocaleName);
            Assert.AreEqual(expectedItemID, queuedMessage.ItemId);
            Assert.AreEqual(expectedScanCode, queuedMessage.ScanCode);
            Assert.AreEqual(expectedMessageType, queuedMessage.MessageTypeId);
            Assert.AreEqual(messageStatusType, queuedMessage.MessageStatusId);
            Assert.AreEqual(expectedMessageAction, queuedMessage.MessageActionId);
            Assert.AreEqual(expectedItemTypeCode, queuedMessage.ItemTypeCode);
            Assert.AreEqual(expectedItemTypeDescription, queuedMessage.ItemTypeDesc);
            Assert.AreEqual(expectedMessageHistoryId, queuedMessage.MessageHistoryId);

            Assert.AreEqual(stagingItemLocaleModel.Discount_Case, queuedMessage.CaseDiscount);
            Assert.AreEqual(stagingItemLocaleModel.Discount_TM, queuedMessage.TmDiscount);
            Assert.AreEqual(stagingItemLocaleModel.Restriction_Age, queuedMessage.AgeRestriction);
            Assert.AreEqual(stagingItemLocaleModel.Restriction_Hours, queuedMessage.RestrictedHours);
            Assert.AreEqual(stagingItemLocaleModel.Authorized, queuedMessage.Authorized);
            Assert.AreEqual(stagingItemLocaleModel.Discontinued, queuedMessage.Discontinued);
            Assert.AreEqual(stagingItemLocaleModel.LabelTypeDesc, queuedMessage.LabelTypeDescription);
            Assert.AreEqual(stagingItemLocaleModel.LocalItem, queuedMessage.LocalItem);
            Assert.AreEqual(stagingItemLocaleModel.Product_Code, queuedMessage.ProductCode);
            Assert.AreEqual(stagingItemLocaleModel.RetailUnit, queuedMessage.RetailUnit);
            Assert.AreEqual(stagingItemLocaleModel.Sign_Desc, queuedMessage.SignDescription);
            Assert.AreEqual(stagingItemLocaleModel.Locality, queuedMessage.Locality);
            Assert.AreEqual(stagingItemLocaleModel.Sign_RomanceText_Long, queuedMessage.SignRomanceLong);
            Assert.AreEqual(stagingItemLocaleModel.Sign_RomanceText_Short, queuedMessage.SignRomanceShort);
            Assert.AreEqual(stagingItemLocaleModel.Msrp, queuedMessage.Msrp);
            Assert.AreEqual(stagingItemLocaleModel.OrderedByInfor, queuedMessage.OrderedByInfor);
            Assert.AreEqual(stagingItemLocaleModel.DefaultScanCode, queuedMessage.DefaultScanCode);
            Assert.AreEqual(stagingItemLocaleModel.IrmaItemKey, queuedMessage.IrmaItemKey);

            Assert.IsNull(queuedMessage.InProcessBy);
            Assert.IsNull(queuedMessage.ProcessedDate);
        }

        private void AssertQueuedMessageMatchesStagingItemLocaleExtendedModel(dynamic queuedMessage,
            IList<StagingItemLocaleExtendedModel> extendedStagingItemLocaleData)
        {
            Assert.AreEqual(!extendedStagingItemLocaleData.Single(il => il.AttributeId == Attributes.ColorAdded).AttributeValue.Equals("0"), queuedMessage.ColorAdded);
            Assert.AreEqual(extendedStagingItemLocaleData.Single(il => il.AttributeId == Attributes.CountryOfProcessing).AttributeValue, queuedMessage.CountryOfProcessing);
            Assert.AreEqual(extendedStagingItemLocaleData.Single(il => il.AttributeId == Attributes.Origin).AttributeValue, queuedMessage.Origin);
            Assert.AreEqual(!extendedStagingItemLocaleData.Single(il => il.AttributeId == Attributes.ElectronicShelfTag).AttributeValue.Equals("0"), queuedMessage.ElectronicShelfTag);
            Assert.AreEqual(extendedStagingItemLocaleData.Single(il => il.AttributeId == Attributes.Exclusive).AttributeValue, queuedMessage.Exclusive.ToString());
            Assert.AreEqual(extendedStagingItemLocaleData.Single(il => il.AttributeId == Attributes.NumberOfDigitsSentToScale).AttributeValue, queuedMessage.NumberOfDigitsSentToScale.ToString());
            Assert.AreEqual(extendedStagingItemLocaleData.Single(il => il.AttributeId == Attributes.ChicagoBaby).AttributeValue, queuedMessage.ChicagoBaby);
            Assert.AreEqual(extendedStagingItemLocaleData.Single(il => il.AttributeId == Attributes.TagUom).AttributeValue, queuedMessage.TagUom);
            Assert.AreEqual(extendedStagingItemLocaleData.Single(il => il.AttributeId == Attributes.LinkedScanCode).AttributeValue, queuedMessage.LinkedItem);
            Assert.AreEqual(extendedStagingItemLocaleData.Single(il => il.AttributeId == Attributes.ScaleExtraText).AttributeValue, queuedMessage.ScaleExtraText);
        }

        private void AssertQueuedMessageMatchesStagingItemSupplierModel(dynamic queuedMessage,
            StagingItemLocaleSupplierModel stagingItemLocaleSupplierModel)
        {
            Assert.AreEqual(stagingItemLocaleSupplierModel.SupplierName, queuedMessage.SupplierName);
            Assert.AreEqual(stagingItemLocaleSupplierModel.SupplierItemId, queuedMessage.SupplierItemID);
            Assert.AreEqual(stagingItemLocaleSupplierModel.IrmaVendorKey, queuedMessage.IrmaVendorKey);
            Assert.AreEqual(stagingItemLocaleSupplierModel.SupplierCaseSize, queuedMessage.SupplierCaseSize);
        }
    }
}
