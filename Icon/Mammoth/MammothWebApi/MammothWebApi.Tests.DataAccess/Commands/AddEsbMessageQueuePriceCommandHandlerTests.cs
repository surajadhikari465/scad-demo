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

namespace MammothWebApi.Tests.DataAccess.CommandTests
{
    [TestClass]
    public class AddEsbMessageQueuePriceCommandHandlerTests
    {
        private AddEsbMessageQueuePriceCommandHandler commandHandler;
        private AddEsbMessageQueuePriceCommand commandParameters;
        private Mock<ILogger> logger;
        private SqlDbProvider db;
        private DateTime timestamp;
        private Locales locale;
        private string region = "SW";
        private int? maxItemId;
        private List<Item> items;
        private Guid transactionId;
        private int maxMessageQueueId;
        private int itemTypeId;

        [TestInitialize]
        public void InitializeTests()
        {
            this.timestamp = DateTime.Now;
            this.transactionId = Guid.NewGuid();
            string connectionString = ConfigurationManager.ConnectionStrings["Mammoth"].ConnectionString;
            this.db = new SqlDbProvider();
            this.db.Connection = new SqlConnection(connectionString);
            this.db.Connection.Open();
            this.db.Transaction = this.db.Connection.BeginTransaction();
            this.logger = new Mock<ILogger>();

            this.commandHandler = new AddEsbMessageQueuePriceCommandHandler(this.logger.Object, this.db);
            this.commandParameters = new AddEsbMessageQueuePriceCommand();

            // Add Test Locale
            this.locale = new Locales
            {
                BusinessUnitID = 77777,
                LocaleID = 70000,
                StoreAbbrev = "TEST",
                StoreName = "TEST STORE NAME 1"
            };
            AddLocaleToDb(locale, this.region);

            // Lookup data added in DatabaseInitialization class
            // Get ItemTypeId
            this.itemTypeId = this.db.Connection.Query<int>("SELECT TOP 1 itemTypeID FROM ItemTypes", transaction: this.db.Transaction).First();
            //AddItemTypesToDb();
            //AddEsbMessageQueueLookupValues();

            // Add Test Items
            this.maxItemId = this.db.Connection.Query<int?>("SELECT MAX(ItemID) FROM Items", transaction: this.db.Transaction).FirstOrDefault();
            this.items = new List<Item>();
            this.items.Add(new TestItemBuilder().WithScanCode("111111777771").WithItemId((maxItemId ?? default(int)) + 1).WithItemTypeId(this.itemTypeId).Build());
            this.items.Add(new TestItemBuilder().WithScanCode("111111777772").WithItemId((maxItemId ?? default(int)) + 2).WithItemTypeId(this.itemTypeId).Build());
            this.items.Add(new TestItemBuilder().WithScanCode("111111777773").WithItemId((maxItemId ?? default(int)) + 3).WithItemTypeId(this.itemTypeId).Build());
            AddToItemsTable(this.items);

            int? messageQueueId = this.db.Connection.Query<int?>("SELECT MAX(MessageQueueId) FROM esb.MessageQueuePrice", transaction: this.db.Transaction).FirstOrDefault();
            this.maxMessageQueueId = messageQueueId.HasValue ? messageQueueId.Value : 0;
        }

        [TestCleanup]
        public void CleanupTest()
        {
            if (this.db.Transaction != null)
            {
                this.db.Transaction.Rollback();
                this.db.Transaction.Dispose();
            }
            this.db.Connection.Dispose();
        }

        [TestMethod]
        public void AddEsbMessageQueuePriceCommand_RegularPricesSetupInStagingWithMatchingBatchIdAndRegion_AddsRowsToMessageQueuePriceTable()
        {
            // Given
            List<StagingPriceModel> expectedPrices = BuildStagedPrices(
                items: this.items,
                region: this.region,
                businessUnitId: this.locale.BusinessUnitID,
                price: 3.99M,
                multiple: 1,
                priceType: "REG",
                timestamp: this.timestamp,
                transactionId: this.transactionId,
                startDate: new DateTime(2015, 1, 18));
            AddPricesToStagingTable(expectedPrices);

            // When
            this.commandParameters.Region = this.region;
            this.commandParameters.Timestamp = this.timestamp;
            this.commandParameters.MessageActionId = MessageActions.AddOrUpdate;
            this.commandParameters.TransactionId = this.transactionId;
            this.commandHandler.Execute(this.commandParameters);

            // Then
            List<MessageQueuePrice> actual = this.db.Connection.Query<MessageQueuePrice>("SELECT * FROM esb.MessageQueuePrice WHERE MessageQueueId > @MaxMessageQueueId ORDER BY ScanCode",
                new { MaxMessageQueueId = this.maxMessageQueueId },
                this.db.Transaction).ToList();
            List<ItemType> itemTypes = this.db.Connection.Query<ItemType>("SELECT * FROM dbo.ItemTypes", transaction: this.db.Transaction).ToList();

            for (int i = 0; i < expectedPrices.Count; i++)
            {
                Assert.AreEqual(expectedPrices[i].BusinessUnitId, actual[i].BusinessUnitId, "BusinessUnitId is incorrect.");
                Assert.AreEqual(expectedPrices[i].CurrencyCode, actual[i].CurrencyCode, "CurrencyCode is incorrect.");
                Assert.IsNull(actual[i].EndDate, "EndDate is not null.");
                Assert.AreEqual(this.timestamp.ToString(), actual[i].InsertDate.ToString(), "InsertDate is incorrect.");
                Assert.IsNull(actual[i].InProcessBy, "InProcessBy is not null.");
                Assert.AreEqual(this.items[i].ItemID, actual[i].ItemId, "ItemID is incorrect.");
                Assert.AreEqual(itemTypes.First(t => t.ItemTypeID == this.items[i].ItemTypeID).ItemTypeCode, actual[i].ItemTypeCode, "ItemTypeCode is incorrect.");
                Assert.AreEqual(itemTypes.First(t => t.ItemTypeID == this.items[i].ItemTypeID).ItemTypeDesc, actual[i].ItemTypeDesc, "ItemTypeDesc is incorrect.");
                Assert.AreEqual(this.locale.StoreName, actual[i].LocaleName, "LocaleName is incorrect.");
                Assert.AreEqual(MessageActions.AddOrUpdate, actual[i].MessageActionId, "MessageActionId is incorrect.");
                Assert.IsNull(actual[i].MessageHistoryId, "MessageHistoryId is not null.");
                Assert.IsNotNull(actual[i].MessageQueueId, "MessageQueueId is null.");
                Assert.AreEqual(MessageTypes.Price, actual[i].MessageTypeId, "MessageQueueTypeId is incorrect.");
                Assert.AreEqual(MessageStatusTypes.Ready, actual[i].MessageStatusId, "MessageStatusId is incorrect.");
                Assert.AreEqual(expectedPrices[i].Multiple, actual[i].Multiple, "Multiple is incorrect.");
                Assert.AreEqual(expectedPrices[i].Price, actual[i].Price, "Price is incorrect.");
                Assert.AreEqual("REG", actual[i].PriceTypeCode, "PriceTypeCode is incorrect.");
                Assert.IsNull(actual[i].SubPriceTypeCode, "SubPriceTypeCode is not null.");
                Assert.IsNull(actual[i].ProcessedDate, "ProcessedDate is not null.");
                Assert.AreEqual(expectedPrices[i].ScanCode, actual[i].ScanCode, "ScanCode is incorrect.");
                Assert.AreEqual(expectedPrices[i].StartDate.ToString(), actual[i].StartDate.ToString(), "StartDate is incorrect.");
                Assert.AreEqual(expectedPrices[i].PriceUom, actual[i].UomCode, "UomCode is incorrect.");
            }
        }

        [TestMethod]
        public void AddEsbMessageQueuePriceCommand_SalePricesSetupInStagingWithMatchingBatchIdAndRegion_AddsRowsToMessageQueuePriceTable()
        {
            // Given
            List<StagingPriceModel> expectedPrices = BuildStagedPrices(
                items: this.items,
                region: this.region,
                businessUnitId: this.locale.BusinessUnitID,
                price: 3.99M,
                multiple: 1,
                priceType: "SAL",
                timestamp: this.timestamp,
                transactionId: this.transactionId,
                startDate: new DateTime(2015, 1, 18),
                endDate: new DateTime(2015, 2, 1));
            AddPricesToStagingTable(expectedPrices);

            // When
            this.commandParameters.Region = this.region;
            this.commandParameters.Timestamp = this.timestamp;
            this.commandParameters.MessageActionId = MessageActions.AddOrUpdate;
            this.commandParameters.TransactionId = this.transactionId;
            this.commandHandler.Execute(this.commandParameters);

            // Then
            List<MessageQueuePrice> actual = this.db.Connection.Query<MessageQueuePrice>("SELECT * FROM esb.MessageQueuePrice WHERE MessageQueueId > @MaxMessageQueueId ORDER BY ScanCode",
                new { MaxMessageQueueId = this.maxMessageQueueId },
                this.db.Transaction).ToList();
            List<ItemType> itemTypes = this.db.Connection.Query<ItemType>("SELECT * FROM dbo.ItemTypes", transaction: this.db.Transaction).ToList();

            for (int i = 0; i < expectedPrices.Count; i++)
            {
                Assert.AreEqual(expectedPrices[i].BusinessUnitId, actual[i].BusinessUnitId, "BusinessUnitId is incorrect.");
                Assert.AreEqual(expectedPrices[i].CurrencyCode, actual[i].CurrencyCode, "CurrencyCode is incorrect.");
                Assert.IsNotNull(actual[i].EndDate, "EndDate is not null.");
                Assert.AreEqual(expectedPrices[i].EndDate, actual[i].EndDate, "EndDate is incorrect.");
                Assert.AreEqual(this.timestamp.ToString(), actual[i].InsertDate.ToString(), "InsertDate is incorrect.");
                Assert.IsNull(actual[i].InProcessBy, "InProcessBy is not null.");
                Assert.AreEqual(this.items[i].ItemID, actual[i].ItemId, "ItemID is incorrect.");
                Assert.AreEqual(itemTypes.First(t => t.ItemTypeID == this.items[i].ItemTypeID).ItemTypeCode, actual[i].ItemTypeCode, "ItemTypeCode is incorrect.");
                Assert.AreEqual(itemTypes.First(t => t.ItemTypeID == this.items[i].ItemTypeID).ItemTypeDesc, actual[i].ItemTypeDesc, "ItemTypeDesc is incorrect.");
                Assert.AreEqual(this.locale.StoreName, actual[i].LocaleName, "LocaleName is incorrect.");
                Assert.AreEqual(MessageActions.AddOrUpdate, actual[i].MessageActionId, "MessageActionId is incorrect.");
                Assert.IsNull(actual[i].MessageHistoryId, "MessageHistoryId is not null.");
                Assert.IsNotNull(actual[i].MessageQueueId, "MessageQueueId is null.");
                Assert.AreEqual(MessageTypes.Price, actual[i].MessageTypeId, "MessageQueueTypeId is incorrect.");
                Assert.AreEqual(MessageStatusTypes.Ready, actual[i].MessageStatusId, "MessageStatusId is incorrect.");
                Assert.AreEqual(expectedPrices[i].Multiple, actual[i].Multiple, "Multiple is incorrect.");
                Assert.AreEqual(expectedPrices[i].Price, actual[i].Price, "Price is incorrect.");
                Assert.AreEqual("TPR", actual[i].PriceTypeCode, "PriceTypeCode is incorrect.");
                Assert.IsNotNull(actual[i].SubPriceTypeCode, "SubPriceTypeCode is incorrect.");
                Assert.AreEqual(expectedPrices[i].PriceType, actual[i].SubPriceTypeCode, "SalePriceTypeCode is incorrect.");
                Assert.IsNull(actual[i].ProcessedDate, "ProcessedDate is not null.");
                Assert.AreEqual(expectedPrices[i].ScanCode, actual[i].ScanCode, "ScanCode is incorrect.");
                Assert.AreEqual(expectedPrices[i].StartDate.ToString(), actual[i].StartDate.ToString(), "StartDate is incorrect.");
                Assert.AreEqual(expectedPrices[i].PriceUom, actual[i].UomCode, "UomCode is incorrect.");
            }
        }

        [TestMethod]
        public void AddEsbMessageQueuePriceCommand_RegularAndSalePricesSetupInStagingWithMatchingBatchIdAndRegion_AddsRowsToMessageQueuePriceTable()
        {
            // Given
            List<StagingPriceModel> expectedPrices = BuildStagedPrices( // setup reg prices
                items: this.items.Take(2),
                region: this.region,
                businessUnitId: this.locale.BusinessUnitID,
                price: 3.99M,
                multiple: 1,
                priceType: "REG",
                timestamp: this.timestamp,
                transactionId: this.transactionId,
                startDate: new DateTime(2015, 1, 18));

            expectedPrices.AddRange(BuildStagedPrices( // setup sale prices
                items: this.items.Where(i => !expectedPrices.Select(p => p.ScanCode).Contains(i.ScanCode)),
                region: this.region,
                businessUnitId: this.locale.BusinessUnitID,
                price: 3.99M,
                multiple: 1,
                priceType: "SAL",
                timestamp: this.timestamp,
                transactionId: this.transactionId,
                startDate: new DateTime(2015, 1, 18),
                endDate: new DateTime(2015, 2, 1)));
            AddPricesToStagingTable(expectedPrices);

            // When
            this.commandParameters.Region = this.region;
            this.commandParameters.Timestamp = this.timestamp;
            this.commandParameters.MessageActionId = MessageActions.AddOrUpdate;
            this.commandParameters.TransactionId = this.transactionId;
            this.commandHandler.Execute(this.commandParameters);

            // Then
            List<MessageQueuePrice> actual = this.db.Connection.Query<MessageQueuePrice>("SELECT * FROM esb.MessageQueuePrice WHERE MessageQueueId > @MaxMessageQueueId ORDER BY ScanCode",
                new { MaxMessageQueueId = this.maxMessageQueueId },
                this.db.Transaction).ToList();
            List<ItemType> itemTypes = this.db.Connection.Query<ItemType>("SELECT * FROM dbo.ItemTypes", transaction: this.db.Transaction).ToList();

            for (int i = 0; i < expectedPrices.Count; i++)
            {
                Assert.AreEqual(expectedPrices[i].BusinessUnitId, actual[i].BusinessUnitId, "BusinessUnitId is incorrect.");
                Assert.AreEqual(expectedPrices[i].CurrencyCode, actual[i].CurrencyCode, "CurrencyCode is incorrect.");
                Assert.AreEqual(expectedPrices[i].EndDate, actual[i].EndDate, "EndDate is incorrect.");
                Assert.AreEqual(this.timestamp.ToString(), actual[i].InsertDate.ToString(), "InsertDate is incorrect.");
                Assert.IsNull(actual[i].InProcessBy, "InProcessBy is not null.");
                Assert.AreEqual(this.items[i].ItemID, actual[i].ItemId, "ItemID is incorrect.");
                Assert.AreEqual(itemTypes.First(t => t.ItemTypeID == this.items[i].ItemTypeID).ItemTypeCode, actual[i].ItemTypeCode, "ItemTypeCode is incorrect.");
                Assert.AreEqual(itemTypes.First(t => t.ItemTypeID == this.items[i].ItemTypeID).ItemTypeDesc, actual[i].ItemTypeDesc, "ItemTypeDesc is incorrect.");
                Assert.AreEqual(this.locale.StoreName, actual[i].LocaleName, "LocaleName is incorrect.");
                Assert.AreEqual(MessageActions.AddOrUpdate, actual[i].MessageActionId, "MessageActionId is incorrect.");
                Assert.IsNull(actual[i].MessageHistoryId, "MessageHistoryId is not null.");
                Assert.IsNotNull(actual[i].MessageQueueId, "MessageQueueId is null.");
                Assert.AreEqual(MessageTypes.Price, actual[i].MessageTypeId, "MessageQueueTypeId is incorrect.");
                Assert.AreEqual(MessageStatusTypes.Ready, actual[i].MessageStatusId, "MessageStatusId is incorrect.");
                Assert.AreEqual(expectedPrices[i].Multiple, actual[i].Multiple, "Multiple is incorrect.");
                Assert.AreEqual(expectedPrices[i].Price, actual[i].Price, "Price is incorrect.");
                Assert.AreEqual((expectedPrices[i].PriceType != "REG" ? "TPR" : "REG"), actual[i].PriceTypeCode, "PriceTypeCode is incorrect.");
                if (expectedPrices[i].PriceType == "REG")
                {
                    Assert.IsNull(actual[i].SubPriceTypeCode, "SubPricetypeCost is not null.");
                    Assert.IsNull(actual[i].EndDate, "EndDate is not null.");
                }
                else
                {
                    Assert.IsNotNull(actual[i].SubPriceTypeCode, "SubPriceTypeCode is null.");
                    Assert.AreEqual(expectedPrices[i].PriceType, actual[i].SubPriceTypeCode, "SalePriceTypeCode is incorrect.");
                    Assert.IsNotNull(actual[i].EndDate, "EndDate is not null.");

                }
                Assert.IsNull(actual[i].ProcessedDate, "ProcessedDate is not null.");
                Assert.AreEqual(expectedPrices[i].ScanCode, actual[i].ScanCode, "ScanCode is incorrect.");
                Assert.AreEqual(expectedPrices[i].StartDate.ToString(), actual[i].StartDate.ToString(), "StartDate is incorrect.");
                Assert.AreEqual(expectedPrices[i].PriceUom, actual[i].UomCode, "UomCode is incorrect.");
            }
        }

        [TestMethod]
        public void AddEsbMessageQueuePriceCommand_PricesSetupInStagingWithNonMatchingBatchId_DoesNotAddRowsToMessageQueuePriceTable()
        {
            // Given
            List<StagingPriceModel> expectedPrices = BuildStagedPrices(
                items: this.items,
                region: this.region,
                businessUnitId: this.locale.BusinessUnitID,
                price: 3.99M,
                multiple: 1,
                priceType: "REG",
                timestamp: this.timestamp,
                transactionId: this.transactionId,
                startDate: new DateTime(2015, 1, 18));
            AddPricesToStagingTable(expectedPrices);

            // When
            this.commandParameters.Region = this.region;
            this.commandParameters.Timestamp = DateTime.UtcNow;
            this.commandParameters.MessageActionId = MessageActions.AddOrUpdate;
            this.transactionId = Guid.NewGuid();
            this.commandHandler.Execute(this.commandParameters);

            // Then
            List<MessageQueuePrice> actual = this.db.Connection.Query<MessageQueuePrice>("SELECT * FROM esb.MessageQueuePrice WHERE MessageQueueId > @MaxMessageQueueId ORDER BY ScanCode",
                new { MaxMessageQueueId = this.maxMessageQueueId },
                this.db.Transaction).ToList();
            Assert.IsTrue(actual.Count == 0, "The rows were inserted into esb.MessageQueuePrice when they should not have been.");
        }

        [TestMethod]
        public void AddEsbMessageQueuePriceCommand_PricesSetupInStagingWithNonMatchingRegion_DoesNotAddRowsToMessageQueuePriceTable()
        {
            // Given
            List<StagingPriceModel> expectedPrices = BuildStagedPrices(
                items: this.items,
                region: this.region,
                businessUnitId: this.locale.BusinessUnitID,
                price: 3.99M,
                multiple: 1,
                priceType: "REG",
                timestamp: this.timestamp,
                transactionId: this.transactionId,
                startDate: new DateTime(2015, 1, 18));
            AddPricesToStagingTable(expectedPrices);

            // When
            this.commandParameters.Region = "NA";
            this.commandParameters.Timestamp = this.timestamp;
            this.commandParameters.MessageActionId = MessageActions.AddOrUpdate;
            this.commandParameters.TransactionId = this.transactionId;
            this.commandHandler.Execute(this.commandParameters);

            // Then
            List<MessageQueuePrice> actual = this.db.Connection.Query<MessageQueuePrice>("SELECT * FROM esb.MessageQueuePrice WHERE MessageQueueId > @MaxMessageQueueId ORDER BY ScanCode",
                new { MaxMessageQueueId = this.maxMessageQueueId },
                this.db.Transaction).ToList();
            Assert.IsTrue(actual.Count == 0, "The rows were inserted into esb.MessageQueuePrice when they should not have been.");
        }

        [TestMethod]
        public void AddEsbMessageQueuePriceCommand_RegularPriceDeletesSetupInStagingWithMatchingBatchIdAndRegion_AddsDeleteActionRowsToMessageQueuePriceTable()
        {
            // Given
            List<StagingPriceModel> expectedPrices = BuildStagedPrices(
                items: this.items,
                region: this.region,
                businessUnitId: this.locale.BusinessUnitID,
                price: 3.99M,
                multiple: 1,
                priceType: "REG",
                timestamp: this.timestamp,
                transactionId: this.transactionId,
                startDate: new DateTime(2015, 1, 18));
            AddPricesToStagingTable(expectedPrices);

            // When
            this.commandParameters.Region = this.region;
            this.commandParameters.Timestamp = this.timestamp;
            this.commandParameters.MessageActionId = MessageActions.Delete;
            this.commandParameters.TransactionId = this.transactionId;
            this.commandHandler.Execute(this.commandParameters);

            // Then
            List<MessageQueuePrice> actual = this.db.Connection.Query<MessageQueuePrice>("SELECT * FROM esb.MessageQueuePrice WHERE MessageQueueId > @MaxMessageQueueId ORDER BY ScanCode",
                new { MaxMessageQueueId = this.maxMessageQueueId },
                this.db.Transaction).ToList();
            List<ItemType> itemTypes = this.db.Connection.Query<ItemType>("SELECT * FROM dbo.ItemTypes", transaction: this.db.Transaction).ToList();

            for (int i = 0; i < expectedPrices.Count; i++)
            {
                Assert.AreEqual(expectedPrices[i].BusinessUnitId, actual[i].BusinessUnitId, "BusinessUnitId is incorrect.");
                Assert.AreEqual(expectedPrices[i].CurrencyCode, actual[i].CurrencyCode, "CurrencyCode is incorrect.");
                Assert.IsNull(actual[i].EndDate, "EndDate is not null.");
                Assert.AreEqual(this.timestamp.ToString(), actual[i].InsertDate.ToString(), "InsertDate is incorrect.");
                Assert.IsNull(actual[i].InProcessBy, "InProcessBy is not null.");
                Assert.AreEqual(this.items[i].ItemID, actual[i].ItemId, "ItemID is incorrect.");
                Assert.AreEqual(itemTypes.First(t => t.ItemTypeID == this.items[i].ItemTypeID).ItemTypeCode, actual[i].ItemTypeCode, "ItemTypeCode is incorrect.");
                Assert.AreEqual(itemTypes.First(t => t.ItemTypeID == this.items[i].ItemTypeID).ItemTypeDesc, actual[i].ItemTypeDesc, "ItemTypeDesc is incorrect.");
                Assert.AreEqual(this.locale.StoreName, actual[i].LocaleName, "LocaleName is incorrect.");
                Assert.AreEqual(MessageActions.Delete, actual[i].MessageActionId, "MessageActionId is incorrect.");
                Assert.IsNull(actual[i].MessageHistoryId, "MessageHistoryId is not null.");
                Assert.IsNotNull(actual[i].MessageQueueId, "MessageQueueId is null.");
                Assert.AreEqual(MessageTypes.Price, actual[i].MessageTypeId, "MessageQueueTypeId is incorrect.");
                Assert.AreEqual(MessageStatusTypes.Ready, actual[i].MessageStatusId, "MessageStatusId is incorrect.");
                Assert.AreEqual(expectedPrices[i].Multiple, actual[i].Multiple, "Multiple is incorrect.");
                Assert.AreEqual(expectedPrices[i].Price, actual[i].Price, "Price is incorrect.");
                Assert.AreEqual("REG", actual[i].PriceTypeCode, "PriceTypeCode is incorrect.");
                Assert.IsNull(actual[i].SubPriceTypeCode, "SubPriceTypeCode is not null.");
                Assert.IsNull(actual[i].ProcessedDate, "ProcessedDate is not null.");
                Assert.AreEqual(expectedPrices[i].ScanCode, actual[i].ScanCode, "ScanCode is incorrect.");
                Assert.AreEqual(expectedPrices[i].StartDate.ToString(), actual[i].StartDate.ToString(), "StartDate is incorrect.");
                Assert.AreEqual(expectedPrices[i].PriceUom, actual[i].UomCode, "UomCode is incorrect.");
            }
        }

        [TestMethod]
        public void AddEsbMessageQueuePriceCommand_SalePriceDeletesSetupInStagingWithMatchingBatchIdAndRegion_AddsDeleteActionRowsToMessageQueuePriceTable()
        {
            // Given
            List<StagingPriceModel> expectedPrices = BuildStagedPrices(
                items: this.items,
                region: this.region,
                businessUnitId: this.locale.BusinessUnitID,
                price: 3.99M,
                multiple: 1,
                priceType: "SAL",
                timestamp: this.timestamp,
                transactionId: this.transactionId,
                startDate: new DateTime(2015, 1, 18),
                endDate: new DateTime(2015, 2, 1));
            AddPricesToStagingTable(expectedPrices);

            // When
            this.commandParameters.Region = this.region;
            this.commandParameters.Timestamp = this.timestamp;
            this.commandParameters.MessageActionId = MessageActions.Delete;
            this.commandParameters.TransactionId = this.transactionId;
            this.commandHandler.Execute(this.commandParameters);

            // Then
            List<MessageQueuePrice> actual = this.db.Connection.Query<MessageQueuePrice>("SELECT * FROM esb.MessageQueuePrice WHERE MessageQueueId > @MaxMessageQueueId ORDER BY ScanCode",
                new { MaxMessageQueueId = this.maxMessageQueueId },
                this.db.Transaction).ToList();
            List<ItemType> itemTypes = this.db.Connection.Query<ItemType>("SELECT * FROM dbo.ItemTypes", transaction: this.db.Transaction).ToList();

            for (int i = 0; i < expectedPrices.Count; i++)
            {
                Assert.AreEqual(expectedPrices[i].BusinessUnitId, actual[i].BusinessUnitId, "BusinessUnitId is incorrect.");
                Assert.AreEqual(expectedPrices[i].CurrencyCode, actual[i].CurrencyCode, "CurrencyCode is incorrect.");
                Assert.IsNotNull(actual[i].EndDate, "EndDate is not null.");
                Assert.AreEqual(expectedPrices[i].EndDate, actual[i].EndDate, "EndDate is incorrect.");
                Assert.AreEqual(this.timestamp.ToString(), actual[i].InsertDate.ToString(), "InsertDate is incorrect.");
                Assert.IsNull(actual[i].InProcessBy, "InProcessBy is not null.");
                Assert.AreEqual(this.items[i].ItemID, actual[i].ItemId, "ItemID is incorrect.");
                Assert.AreEqual(itemTypes.First(t => t.ItemTypeID == this.items[i].ItemTypeID).ItemTypeCode, actual[i].ItemTypeCode, "ItemTypeCode is incorrect.");
                Assert.AreEqual(itemTypes.First(t => t.ItemTypeID == this.items[i].ItemTypeID).ItemTypeDesc, actual[i].ItemTypeDesc, "ItemTypeDesc is incorrect.");
                Assert.AreEqual(this.locale.StoreName, actual[i].LocaleName, "LocaleName is incorrect.");
                Assert.AreEqual(MessageActions.Delete, actual[i].MessageActionId, "MessageActionId is incorrect.");
                Assert.IsNull(actual[i].MessageHistoryId, "MessageHistoryId is not null.");
                Assert.IsNotNull(actual[i].MessageQueueId, "MessageQueueId is null.");
                Assert.AreEqual(MessageTypes.Price, actual[i].MessageTypeId, "MessageQueueTypeId is incorrect.");
                Assert.AreEqual(MessageStatusTypes.Ready, actual[i].MessageStatusId, "MessageStatusId is incorrect.");
                Assert.AreEqual(expectedPrices[i].Multiple, actual[i].Multiple, "Multiple is incorrect.");
                Assert.AreEqual(expectedPrices[i].Price, actual[i].Price, "Price is incorrect.");
                Assert.AreEqual("TPR", actual[i].PriceTypeCode, "PriceTypeCode is incorrect.");
                Assert.IsNotNull(actual[i].SubPriceTypeCode, "SubPriceTypeCode is incorrect.");
                Assert.AreEqual(expectedPrices[i].PriceType, actual[i].SubPriceTypeCode, "SalePriceTypeCode is incorrect.");
                Assert.IsNull(actual[i].ProcessedDate, "ProcessedDate is not null.");
                Assert.AreEqual(expectedPrices[i].ScanCode, actual[i].ScanCode, "ScanCode is incorrect.");
                Assert.AreEqual(expectedPrices[i].StartDate.ToString(), actual[i].StartDate.ToString(), "StartDate is incorrect.");
                Assert.AreEqual(expectedPrices[i].PriceUom, actual[i].UomCode, "UomCode is incorrect.");
            }
        }

        [TestMethod]
        public void AddEsbMessageQueuePriceCommand_RegularAndSalePriceDeletesSetupInStagingWithMatchingBatchIdAndRegion_AddsDeleteActionRowsToMessageQueuePriceTable()
        {
            // Given
            List<StagingPriceModel> expectedPrices = BuildStagedPrices( // setup reg prices
                items: this.items.Take(2),
                region: this.region,
                businessUnitId: this.locale.BusinessUnitID,
                price: 3.99M,
                multiple: 1,
                priceType: "REG",
                timestamp: this.timestamp,
                transactionId: this.transactionId,
                startDate: new DateTime(2015, 1, 18));

            expectedPrices.AddRange(BuildStagedPrices( // setup sale prices
                items: this.items.Where(i => !expectedPrices.Select(p => p.ScanCode).Contains(i.ScanCode)),
                region: this.region,
                businessUnitId: this.locale.BusinessUnitID,
                price: 3.99M,
                multiple: 1,
                priceType: "SAL",
                timestamp: this.timestamp,
                transactionId: this.transactionId,
                startDate: new DateTime(2015, 1, 18),
                endDate: new DateTime(2015, 2, 1)));
            AddPricesToStagingTable(expectedPrices);

            // When
            this.commandParameters.Region = this.region;
            this.commandParameters.Timestamp = this.timestamp;
            this.commandParameters.MessageActionId = MessageActions.Delete;
            this.commandParameters.TransactionId = this.transactionId;
            this.commandHandler.Execute(this.commandParameters);

            // Then
            List<MessageQueuePrice> actual = this.db.Connection.Query<MessageQueuePrice>("SELECT * FROM esb.MessageQueuePrice WHERE MessageQueueId > @MaxMessageQueueId ORDER BY ScanCode",
                new { MaxMessageQueueId = this.maxMessageQueueId },
                this.db.Transaction).ToList();
            List<ItemType> itemTypes = this.db.Connection.Query<ItemType>("SELECT * FROM dbo.ItemTypes", transaction: this.db.Transaction).ToList();

            for (int i = 0; i < expectedPrices.Count; i++)
            {
                Assert.AreEqual(expectedPrices[i].BusinessUnitId, actual[i].BusinessUnitId, "BusinessUnitId is incorrect.");
                Assert.AreEqual(expectedPrices[i].CurrencyCode, actual[i].CurrencyCode, "CurrencyCode is incorrect.");
                Assert.AreEqual(expectedPrices[i].EndDate, actual[i].EndDate, "EndDate is incorrect.");
                Assert.AreEqual(this.timestamp.ToString(), actual[i].InsertDate.ToString(), "InsertDate is incorrect.");
                Assert.IsNull(actual[i].InProcessBy, "InProcessBy is not null.");
                Assert.AreEqual(this.items[i].ItemID, actual[i].ItemId, "ItemID is incorrect.");
                Assert.AreEqual(itemTypes.First(t => t.ItemTypeID == this.items[i].ItemTypeID).ItemTypeCode, actual[i].ItemTypeCode, "ItemTypeCode is incorrect.");
                Assert.AreEqual(itemTypes.First(t => t.ItemTypeID == this.items[i].ItemTypeID).ItemTypeDesc, actual[i].ItemTypeDesc, "ItemTypeDesc is incorrect.");
                Assert.AreEqual(this.locale.StoreName, actual[i].LocaleName, "LocaleName is incorrect.");
                Assert.AreEqual(MessageActions.Delete, actual[i].MessageActionId, "MessageActionId is incorrect.");
                Assert.IsNull(actual[i].MessageHistoryId, "MessageHistoryId is not null.");
                Assert.IsNotNull(actual[i].MessageQueueId, "MessageQueueId is null.");
                Assert.AreEqual(MessageTypes.Price, actual[i].MessageTypeId, "MessageQueueTypeId is incorrect.");
                Assert.AreEqual(MessageStatusTypes.Ready, actual[i].MessageStatusId, "MessageStatusId is incorrect.");
                Assert.AreEqual(expectedPrices[i].Multiple, actual[i].Multiple, "Multiple is incorrect.");
                Assert.AreEqual(expectedPrices[i].Price, actual[i].Price, "Price is incorrect.");
                Assert.AreEqual((expectedPrices[i].PriceType != "REG" ? "TPR" : "REG"), actual[i].PriceTypeCode, "PriceTypeCode is incorrect.");
                if (expectedPrices[i].PriceType == "REG")
                {
                    Assert.IsNull(actual[i].SubPriceTypeCode, "SubPricetypeCost is not null.");
                    Assert.IsNull(actual[i].EndDate, "EndDate is not null.");
                }
                else
                {
                    Assert.IsNotNull(actual[i].SubPriceTypeCode, "SubPriceTypeCode is null.");
                    Assert.AreEqual(expectedPrices[i].PriceType, actual[i].SubPriceTypeCode, "SalePriceTypeCode is incorrect.");
                    Assert.IsNotNull(actual[i].EndDate, "EndDate is not null.");

                }
                Assert.IsNull(actual[i].ProcessedDate, "ProcessedDate is not null.");
                Assert.AreEqual(expectedPrices[i].ScanCode, actual[i].ScanCode, "ScanCode is incorrect.");
                Assert.AreEqual(expectedPrices[i].StartDate.ToString(), actual[i].StartDate.ToString(), "StartDate is incorrect.");
                Assert.AreEqual(expectedPrices[i].PriceUom, actual[i].UomCode, "UomCode is incorrect.");
            }
        }

        private List<StagingPriceModel> BuildStagedPrices(IEnumerable<Item> items,
            string region,
            int businessUnitId,
            decimal price,
            int multiple,
            string priceType,
            DateTime timestamp,
            Guid transactionId,
            DateTime startDate,
            DateTime? endDate = null)
        {
            // new prices for staging
            List<StagingPriceModel> expectedPrices = new List<StagingPriceModel>();
            foreach (var item in items)
            {
                StagingPriceModel stagingPrice = new TestStagingPriceModelBuilder()
                    .WithScanCode(item.ScanCode)
                    .WithTimestamp(timestamp)
                    .WithTransactionId(transactionId)
                    .WithBusinessUnit(businessUnitId)
                    .WithStartDate(startDate)
                    .WithEndDate(endDate)
                    .WithRegion(region)
                    .WithPriceType(priceType)
                    .WithPrice(price)
                    .WithMultiple(multiple)
                    .Build();
                expectedPrices.Add(stagingPrice);
            }

            return expectedPrices;
        }

        private void AddToItemsTable(List<Item> items)
        {
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
            int affectedRows = this.db.Connection.Execute(sql, items, transaction: this.db.Transaction);
        }

        private void AddPricesToStagingTable(List<StagingPriceModel> pricesInStaging)
        {
            string sql = @"INSERT INTO stage.Price
                            (
	                            Region,
	                            ScanCode,
	                            BusinessUnitId,
	                            Multiple,
	                            Price,
	                            PriceType,
	                            StartDate,
	                            EndDate,
                                PriceUom,
	                            CurrencyCode,
	                            Timestamp,
                                TransactionId
                            )
                            VALUES
                            (
	                            @Region,
	                            @ScanCode,
	                            @BusinessUnitId,
	                            @Multiple,
	                            @Price,
	                            @PriceType,
	                            @StartDate,
	                            @EndDate,
                                RTRIM(@PriceUom),
	                            @CurrencyCode,
	                            @Timestamp,
                                @TransactionId
                            )";

            int affectedRows = this.db.Connection.Execute(sql, pricesInStaging, transaction: this.db.Transaction);
        }

        private void AddLocaleToDb(Locales locale, string region)
        {
            string sql = String.Format(@"INSERT INTO dbo.Locales_{0}
                                        (
	                                        BusinessUnitID,
	                                        StoreName,
	                                        StoreAbbrev
                                        )
                                        VALUES
                                        (
	                                        @BusinessUnitID,
	                                        @StoreName,
	                                        @StoreAbbrev
                                        )", region);

            int affectedRows = this.db.Connection.Execute(sql, locale, transaction: this.db.Transaction);
        }
    }
}
