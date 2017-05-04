using Dapper;
using FastMember;
using Mammoth.Common.DataAccess.DbProviders;
using MammothWebApi.DataAccess.Commands;
using MammothWebApi.DataAccess.Models;
using MammothWebApi.Tests.DataAccess.ModelBuilders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;


namespace MammothWebApi.Tests.DataAccess.CommandTests
{
    [TestClass]
    public class AddOrUpdatePricesCommandHandlerTests
    {
        private AddOrUpdatePricesCommandHandler commandHandler;
        private AddOrUpdatePricesCommand commandParameters;
        private IDbProvider db;
        private DateTime timestamp;
        private int buId;
        private string region = "FL";
        private List<Item> items;
        private Guid transactionId;
        private int? maxItemId;
        private int numberOfItemsTested;

        [TestInitialize]
        public void InitializeTests()
        {
            this.numberOfItemsTested = 100;
            this.timestamp = DateTime.Now;
            this.transactionId = Guid.NewGuid();
            string connectionString = ConfigurationManager.ConnectionStrings["Mammoth"].ConnectionString;
            this.db = new SqlDbProvider();
            this.db.Connection = new SqlConnection(connectionString);
            this.db.Connection.Open();

            this.commandHandler = new AddOrUpdatePricesCommandHandler(this.db);

            this.db.Connection
                .Execute(String.Format("INSERT INTO Locales_{0} (BusinessUnitID, StoreName, StoreAbbrev, AddedDate) VALUES (1, 'TEST STORE', 'TES', GETDATE())",
                    this.region),
                    transaction: this.db.Transaction);

            this.buId = 1; // the businessUnit inserted above

            // Add Test Items to DB
            this.items = BuildItems(this.numberOfItemsTested);
            AddToItemsTable(this.items);

            // Currency added in DatabaseInitialization class

            this.commandParameters = new AddOrUpdatePricesCommand { Region = this.region, Timestamp = this.timestamp, TransactionId = this.transactionId };
        }

        [TestCleanup]
        public void CleanupTest()
        {
            this.db.Connection.Execute($"DELETE FROM Locales_{this.region} WHERE BusinessUnitID = 1", this.db.Transaction);
            this.db.Connection.Execute("DELETE FROM stage.Price WHERE TransactionId = @TranId", new { TranId = this.transactionId }, this.db.Transaction);
            this.db.Connection.Execute($@"SELECT ItemID
                                        INTO #itemsToDelete
                                        FROM Items
                                        WHERE ItemID IN @ItemIDs
    
                                        DELETE p
                                        FROM Price_{this.region} p
                                        JOIN #itemsToDelete d on p.ItemID = d.ItemID
                                        WHERE BusinessUnitID = @BuId

                                        DELETE i
                                        FROM Items i
                                        JOIN #itemsToDelete d on i.ItemID = d.ItemID",
                new { ItemIDs = this.items.Select(i => i.ItemID).ToList(), BuId = this.buId },
                this.db.Transaction);

            if (this.db.Transaction != null)
            {
                this.db.Transaction.Rollback();
                this.db.Transaction.Dispose();
            }
            this.db.Connection.Close();
            this.db.Connection.Dispose();
        }

        [TestMethod]
        public void AddOrUpdatePricesCommand_PricesInStagingHaveSameStartDateAndPriceTypeForItemAndStore_ShouldUpdatePrice()
        {
            // Given
            DateTime existingStartDate = new DateTime(2015, 10, 1);

            List<Prices> existingPrices = BuildExistingPrices(this.items, this.buId, 2.99M, "REG", existingStartDate);
            List<StagingPriceModel> expectedPrices = BuildStagedPrices(this.items, this.buId, 3.99M, "REG", existingStartDate);

            AddPriceToPriceTable(existingPrices);
            AddPricesToStagingTable(expectedPrices);

            // When
            this.commandHandler.Execute(commandParameters);

            // Then
            List<int> itemIds = items.Select(i => i.ItemID).OrderBy(i => i).ToList();
            string getActualSql = @"SELECT * FROM Price_{0} WHERE ItemID IN @ItemIDs AND BusinessUnitID = @BusinessUnitID AND StartDate = @StartDate ORDER BY ItemID";
            List<Prices> actualPrices = this.db.Connection
                .Query<Prices>(String.Format(getActualSql,this.region),
                    new { ItemIDs = itemIds, BusinessUnitID = this.buId, StartDate = existingStartDate },
                    transaction: this.db.Transaction).ToList();

            if (actualPrices.Count == 0)
            {
                Assert.Fail("No prices were found.  Please update test.");
            }

            for (int i = 0; i < expectedPrices.Count; i++)
            {
                Assert.AreEqual(expectedPrices[i].BusinessUnitId, actualPrices[i].BusinessUnitID, "BusinessUnitID does not match.");
                Assert.AreEqual(expectedPrices[i].Region, actualPrices[i].Region, "Region does not match.");
                Assert.AreEqual(itemIds[i], actualPrices[i].ItemID, "ItemID does not match.");
                Assert.AreEqual(expectedPrices[i].Price, actualPrices[i].Price, "Price does not match.");
                Assert.AreEqual(expectedPrices[i].PriceUom, actualPrices[i].PriceUOM, "PriceUOM does not match.");
                Assert.AreEqual(expectedPrices[i].Multiple, actualPrices[i].Multiple, "Multiple does not match.");
                Assert.AreEqual(expectedPrices[i].StartDate, actualPrices[i].StartDate, "BusinessUnitID does not match.");
                Assert.AreEqual(expectedPrices[i].PriceType, actualPrices[i].PriceType, "PriceType does not match.");
                Assert.IsTrue(actualPrices[i].AddedDate.ToString() == this.timestamp.ToString(),
                    String.Format("AddedDate was not as expected. Expected value: {0}; Actual Value: {1}.", this.timestamp, actualPrices[i].AddedDate));
                Assert.IsTrue(actualPrices[i].ModifiedDate.ToString() == commandParameters.Timestamp.ToString(),
                    String.Format("ModifiedDate was not as expected. Expected Value: {0}; Actual Value: {1}.", commandParameters.Timestamp, actualPrices[i].ModifiedDate));
            }
        }

       
        [TestMethod]
        public void AddOrUpdatePricesCommand_PricesInStagingHaveDifferentStartDateButSamePriceTypeForItemAndStore_ShouldAddNewPrices()
        {
            // Given
            DateTime existingStartDate = new DateTime(2015, 10, 1);
            DateTime newStartDate = new DateTime(2015, 10, 15);

            List<Prices> existingPrices = BuildExistingPrices(this.items, this.buId, 2.99M, "REG", existingStartDate);
            List<StagingPriceModel> expectedPrices = BuildStagedPrices(this.items, this.buId, 4.99M, "REG", newStartDate);

            AddPriceToPriceTable(existingPrices);
            AddPricesToStagingTable(expectedPrices);

            // When
            this.commandHandler.Execute(commandParameters);

            // Then
            List<int> itemIds = items.Select(i => i.ItemID).OrderBy(i => i).ToList();
            string getActualSql = @"SELECT * FROM Price_{0} WHERE ItemID IN @ItemIDs AND BusinessUnitID = @BusinessUnitID AND StartDate = @StartDate ORDER BY ItemID";

            // Make sure old reg price is still there
            List<Prices> oldActualPrices = this.db.Connection
                .Query<Prices>(String.Format(getActualSql, this.region),
                    new { ItemIDs = itemIds, BusinessUnitID = this.buId, StartDate = existingStartDate },
                    transaction: this.db.Transaction).ToList();

            Assert.AreEqual(existingPrices.Count, oldActualPrices.Count, "The existing prices do not exist.");

            // new prices
            List<Prices> actualPrices = this.db.Connection
                .Query<Prices>(String.Format(getActualSql, this.region),
                    new { ItemIDs = itemIds, BusinessUnitID = this.buId, StartDate = newStartDate },
                    transaction: this.db.Transaction).ToList();

            if (actualPrices.Count == 0)
            {
                Assert.Fail("Actual Prices were not found.  Test needs to be updated.");
            }

            for (int i = 0; i < expectedPrices.Count; i++)
            {
                Assert.AreEqual(expectedPrices[i].BusinessUnitId, actualPrices[i].BusinessUnitID, "BusinessUnitID does not match.");
                Assert.AreEqual(expectedPrices[i].Region, actualPrices[i].Region, "Region does not match.");
                Assert.AreEqual(itemIds[i], actualPrices[i].ItemID, "ItemID does not match.");
                Assert.AreEqual(expectedPrices[i].Price, actualPrices[i].Price, "Price does not match.");
                Assert.AreEqual(expectedPrices[i].PriceUom, actualPrices[i].PriceUOM, "PriceUOM does not match.");
                Assert.AreEqual(expectedPrices[i].Multiple, actualPrices[i].Multiple, "Multiple does not match.");
                Assert.AreEqual(expectedPrices[i].StartDate, actualPrices[i].StartDate, "BusinessUnitID does not match.");
                Assert.IsTrue(actualPrices[i].AddedDate.ToString() == this.timestamp.ToString(),
                    String.Format("AddedDate was not as expected. Expected value: {0}; Actual Value: {1}.", this.timestamp, actualPrices[i].AddedDate));
                Assert.IsNull(actualPrices[i].ModifiedDate,
                    String.Format("ModifiedDate was not as expected. Expected Value: null; Actual Value: {0}.", actualPrices[i].ModifiedDate));
            }
        }

        [TestMethod]
        public void AddOrUpdatePricesCommand_PricesInStagingHaveDifferentPriceTypeButSameStartDateForItemAndStore_ShouldAddNewPrices()
        {
            // Given
            DateTime existingStartDate = new DateTime(2015, 10, 1);

            List<Prices> existingPrices = BuildExistingPrices(this.items, this.buId, 2.99M, "REG", existingStartDate);
            List<StagingPriceModel> expectedPrices = BuildStagedPrices(this.items, this.buId, 4.99M, "SAL", existingStartDate);

            AddPriceToPriceTable(existingPrices);
            AddPricesToStagingTable(expectedPrices);

            // When
            this.commandHandler.Execute(commandParameters);

            // Then
            List<int> itemIds = items.Select(i => i.ItemID).OrderBy(i => i).ToList();
            string getActualSql = @"SELECT * FROM Price_{0} WHERE ItemID IN @ItemIDs AND BusinessUnitID = @BusinessUnitID AND StartDate = @StartDate AND PriceType = @PriceType ORDER BY ItemID";

            // Make sure old reg price is still there
            List<Prices> oldActualPrices = this.db.Connection
                .Query<Prices>(String.Format(getActualSql, this.region),
                    new { ItemIDs = itemIds, BusinessUnitID = this.buId, StartDate = existingStartDate, PriceType = "REG" },
                    transaction: this.db.Transaction).ToList();

            Assert.AreEqual(existingPrices.Count, oldActualPrices.Count, "The existing prices do not exist.");

            // new prices
            List<Prices> actualPrices = this.db.Connection
                .Query<Prices>(String.Format(getActualSql, this.region),
                    new { ItemIDs = itemIds, BusinessUnitID = this.buId, StartDate = existingStartDate, PriceType = "SAL" },
                    transaction: this.db.Transaction).ToList();

            if (actualPrices.Count == 0)
            {
                Assert.Fail("Actual Prices were not found.  Test needs to be updated.");
            }

            for (int i = 0; i < expectedPrices.Count; i++)
            {
                Assert.AreEqual(expectedPrices[i].BusinessUnitId, actualPrices[i].BusinessUnitID, "BusinessUnitID does not match.");
                Assert.AreEqual(expectedPrices[i].Region, actualPrices[i].Region, "Region does not match.");
                Assert.AreEqual(itemIds[i], actualPrices[i].ItemID, "ItemID does not match.");
                Assert.AreEqual(expectedPrices[i].Price, actualPrices[i].Price, "Price does not match.");
                Assert.AreEqual(expectedPrices[i].PriceUom, actualPrices[i].PriceUOM, "PriceUOM does not match.");
                Assert.AreEqual(expectedPrices[i].Multiple, actualPrices[i].Multiple, "Multiple does not match.");
                Assert.AreEqual(expectedPrices[i].StartDate, actualPrices[i].StartDate, "BusinessUnitID does not match.");
                Assert.IsTrue(actualPrices[i].AddedDate.ToString() == this.timestamp.ToString(),
                    String.Format("AddedDate was not as expected. Expected value: {0}; Actual Value: {1}.", this.timestamp, actualPrices[i].AddedDate));
                Assert.IsNull(actualPrices[i].ModifiedDate,
                    String.Format("ModifiedDate was not as expected. Expected Value: null; Actual Value: {0}.", actualPrices[i].ModifiedDate));
            }
        }

        [TestMethod]
        public void AddOrUpdatePricesCommand_SomePricesInStagingAreNewAndSomeAreExisting_ShouldAddNewPricesAndUpdateExistingPrices()
        {
            // Given
            DateTime existingStartDate = DateTime.Today.AddDays(1);
            DateTime newStartDate = DateTime.Today.AddDays(3);
            string newPriceType = "REG";
            List<Item> itemsForNewPrices = this.items.Take(this.numberOfItemsTested - 10).ToList();
            List<Item> itemsForUpdatePrices = this.items.Where(i => !itemsForNewPrices.Select(x => x.ItemID).Contains(i.ItemID)).ToList();

            List<Prices> existingPrices = BuildExistingPrices(this.items, this.buId, 2.99M, "REG", existingStartDate);
            List<StagingPriceModel> expectedPricesNew = BuildStagedPrices(itemsForNewPrices, this.buId, 4.99M, newPriceType, newStartDate);
            List<StagingPriceModel> expectedPricesUpdate = BuildStagedPrices(itemsForUpdatePrices, this.buId, 3.99M, "REG", existingStartDate);

            AddPriceToPriceTable(existingPrices);
            AddPricesToStagingTable(expectedPricesNew);
            AddPricesToStagingTable(expectedPricesUpdate);

            // When
            this.commandHandler.Execute(commandParameters);

            // Then
            List<int> itemIds = items.Select(i => i.ItemID).OrderBy(i => i).ToList();
            string getActualSql = @"SELECT * FROM Price_{0} WHERE ItemID IN @ItemIDs AND BusinessUnitID = @BusinessUnitID AND StartDate = @StartDate AND PriceType = @PriceType ORDER BY ItemID";

            // Make sure old reg price is still there
            List<Prices> oldActualPrices = this.db.Connection
                .Query<Prices>(String.Format(getActualSql, this.region),
                    new { ItemIDs = itemIds, BusinessUnitID = this.buId, StartDate = existingStartDate, PriceType = "REG" },
                    transaction: this.db.Transaction).ToList();

            Assert.AreEqual(existingPrices.Count, oldActualPrices.Count, "The existing prices do not exist.");

            // updated prices
            List<Prices> updatedActualPrices = this.db.Connection
                .Query<Prices>(String.Format(getActualSql, this.region),
                    new { itemIds = itemsForUpdatePrices.Select(i => i.ItemID), BusinessUnitID = this.buId, StartDate = existingStartDate, PriceType = "REG" },
                    transaction: this.db.Transaction).ToList();

            if (updatedActualPrices.Count != itemsForUpdatePrices.Count)
            {
                Assert.Fail("The updated prices were not found.  Test needs to be updated.");
            }

            // new prices
            List<Prices> actualPrices = this.db.Connection
                .Query<Prices>(String.Format(getActualSql, this.region),
                    new { ItemIDs = itemIds, BusinessUnitID = this.buId, StartDate = newStartDate, PriceType = newPriceType },
                    transaction: this.db.Transaction).ToList();

            if (actualPrices.Count == 0)
            {
                Assert.Fail("Actual Prices were not found.  Test needs to be updated.");
            }

            for (int i = 0; i < expectedPricesNew.Count; i++)
            {
                Assert.AreEqual(expectedPricesNew[i].BusinessUnitId, actualPrices[i].BusinessUnitID, "BusinessUnitID does not match.");
                Assert.AreEqual(expectedPricesNew[i].Region, actualPrices[i].Region, "Region does not match.");
                Assert.AreEqual(itemsForNewPrices.Select(inp => inp.ItemID).ToList()[i], actualPrices[i].ItemID, "ItemID does not match.");
                Assert.AreEqual(expectedPricesNew[i].Price, actualPrices[i].Price, "Price does not match.");
                Assert.AreEqual(expectedPricesNew[i].PriceUom, actualPrices[i].PriceUOM, "PriceUOM does not match.");
                Assert.AreEqual(expectedPricesNew[i].Multiple, actualPrices[i].Multiple, "Multiple does not match.");
                Assert.AreEqual(expectedPricesNew[i].StartDate, actualPrices[i].StartDate, "BusinessUnitID does not match.");
                Assert.IsTrue(actualPrices[i].AddedDate.ToString() == this.timestamp.ToString(),
                    String.Format("AddedDate was not as expected. Expected value: {0}; Actual Value: {1}.", this.timestamp, actualPrices[i].AddedDate));
                Assert.IsNull(actualPrices[i].ModifiedDate,
                    String.Format("ModifiedDate was not as expected. Expected Value: null; Actual Value: {0}.", actualPrices[i].ModifiedDate));
            }

            for (int i = 0; i < expectedPricesUpdate.Count; i++)
            {
                Assert.AreEqual(expectedPricesUpdate[i].BusinessUnitId, updatedActualPrices[i].BusinessUnitID, "BusinessUnitID does not match.");
                Assert.AreEqual(expectedPricesUpdate[i].Region, updatedActualPrices[i].Region, "Region does not match.");
                Assert.AreEqual(itemsForUpdatePrices.Select(iup => iup.ItemID).ToList()[i], updatedActualPrices[i].ItemID, "ItemID does not match.");
                Assert.AreEqual(expectedPricesUpdate[i].Price, updatedActualPrices[i].Price, "Price does not match.");
                Assert.AreEqual(expectedPricesUpdate[i].PriceUom, updatedActualPrices[i].PriceUOM, "PriceUOM does not match.");
                Assert.AreEqual(expectedPricesUpdate[i].Multiple, updatedActualPrices[i].Multiple, "Multiple does not match.");
                Assert.AreEqual(expectedPricesUpdate[i].StartDate, updatedActualPrices[i].StartDate, "BusinessUnitID does not match.");
                Assert.IsTrue(updatedActualPrices[i].AddedDate.ToString() == this.timestamp.ToString(),
                    String.Format("AddedDate was not as expected. Expected value: {0}; Actual Value: {1}.", this.timestamp, updatedActualPrices[i].AddedDate));
                Assert.IsTrue(updatedActualPrices[i].ModifiedDate.ToString() == this.timestamp.ToString(),
                    String.Format("ModifiedDate was not as expected. Expected Value: {0}; Actual Value: {1}.", commandParameters.Timestamp, updatedActualPrices[i].ModifiedDate));
            }
        }

        [TestMethod]
        public void AddOrUpdatePricesCommand_PricesInStagingHaveNewPromosAndNewRegularPricesForSameItemAndStore_ShouldAddNewRegularAndPromoPrices()
        {
            // Given
            // Add existing prices to price table and new prices to staging table
            List<StagingPriceModel> expectedPrices = new List<StagingPriceModel>();

            // new prices for staging
            StagingPriceModel regularPrice = new TestStagingPriceModelBuilder().WithScanCode(this.items[0].ScanCode).WithTimestamp(this.timestamp)
                .WithBusinessUnit(this.buId).WithStartDate(DateTime.Today).WithEndDate(null).WithRegion(this.region).WithPriceType("REG")
                .WithTransactionId(this.transactionId).Build();
            expectedPrices.Add(regularPrice);

            StagingPriceModel promoPrice = new TestStagingPriceModelBuilder().WithScanCode(this.items[0].ScanCode).WithTimestamp(this.timestamp)
                .WithBusinessUnit(this.buId).WithStartDate(DateTime.Today).WithEndDate(new DateTime(2015, 11, 20)).WithRegion(this.region)
                .WithPriceType("ISS").WithTransactionId(this.transactionId).Build();
            expectedPrices.Add(promoPrice);

            AddPricesToStagingTable(expectedPrices);

            // When
            this.commandHandler.Execute(commandParameters);

            // Then
            List<int> itemIds = items.Select(i => i.ItemID).OrderBy(i => i).ToList();
            string getActualSql = @"SELECT * FROM Price_{0} WHERE ItemID IN @ItemIDs AND BusinessUnitID = @BusinessUnitID AND StartDate = @StartDate";

            // new prices
            List<Prices> actualPrices = this.db.Connection
                .Query<Prices>(String.Format(getActualSql, this.region),
                    new { ItemIDs = itemIds, BusinessUnitID = this.buId, StartDate = DateTime.Today },
                    transaction: this.db.Transaction)
                .OrderBy(p => p.PriceType)
                .ToList();

            if (actualPrices.Count == 0)
            {
                Assert.Fail("Actual Prices were not found.  Test needs to be updated.");
            }

            expectedPrices = expectedPrices.OrderBy(p => p.PriceType).ToList();
            int expectedItemId = this.db.Connection.Query<int>("SELECT ItemID FROM Items WHERE ScanCode = @ScanCode", new { ScanCode = this.items[0].ScanCode }, this.db.Transaction).First();

            for (int i = 0; i < expectedPrices.Count; i++)
            {
                Assert.AreEqual(expectedPrices[i].BusinessUnitId, actualPrices[i].BusinessUnitID, "BusinessUnitID does not match.");
                Assert.AreEqual(expectedPrices[i].Region, actualPrices[i].Region, "Region does not match.");
                Assert.AreEqual(expectedItemId, actualPrices[i].ItemID, "ItemID does not match.");
                Assert.AreEqual(expectedPrices[i].Price, actualPrices[i].Price, "Price does not match.");
                Assert.AreEqual(expectedPrices[i].PriceUom, actualPrices[i].PriceUOM, "PriceUOM does not match.");
                Assert.AreEqual(expectedPrices[i].Multiple, actualPrices[i].Multiple, "Multiple does not match.");
                Assert.AreEqual(expectedPrices[i].StartDate, actualPrices[i].StartDate, "BusinessUnitID does not match.");
                Assert.IsTrue(actualPrices[i].AddedDate < DateTime.UtcNow || actualPrices[i].AddedDate >= this.timestamp,
                    String.Format("AddedDate was not as expected. Expected value: {0}; Actual Value: {1}.", DateTime.UtcNow, actualPrices[i].AddedDate));
                Assert.IsNull(actualPrices[i].ModifiedDate,
                    String.Format("ModifiedDate was not as expected. Expected Value: null; Actual Value: {0}.", actualPrices[i].ModifiedDate));
            }
        }

        [TestMethod]
        public void AddOrUpdatePricesCommand_PricesInStagingWithDifferentBatchId_ShouldNotInsertNewPrice()
        {
            // Given
            DateTime existingStartDate = new DateTime(2015, 10, 1);
            DateTime newStartDate = new DateTime(2016, 2, 1);

            List<Prices> existingPrices = BuildExistingPrices(this.items, this.buId, 2.99M, "REG", existingStartDate);
            List<StagingPriceModel> expectedPrices = BuildStagedPrices(this.items, this.buId, 4.99M, "REG", newStartDate);

            AddPriceToPriceTable(existingPrices);
            AddPricesToStagingTable(expectedPrices);

            this.commandParameters.TransactionId = Guid.NewGuid();

            // When
            this.commandHandler.Execute(commandParameters);

            // Then
            List<int> itemIds = items.Select(i => i.ItemID).OrderBy(i => i).ToList();
            string getActualSql = @"SELECT * FROM Price_{0} WHERE ItemID IN @ItemIDs AND BusinessUnitID = @BusinessUnitID AND StartDate = @StartDate ORDER BY ItemID";

            // Make sure old reg price is still there
            List<Prices> actualPrices = this.db.Connection
                .Query<Prices>(String.Format(getActualSql, this.region),
                    new { ItemIDs = itemIds, BusinessUnitID = this.buId, StartDate = newStartDate },
                    transaction: this.db.Transaction).ToList();

            Assert.AreEqual(0, actualPrices.Count,
                String.Format("The new prices were inserted when they weren't supposed to be.  Expected count: {0}; Actual count: {1}.", 0, actualPrices.Count));
        }

        private List<StagingPriceModel> BuildStagedPrices(IEnumerable<Item> items, int businessUnit, decimal price, string priceType, DateTime startDate, DateTime? endDate = null)
        {
            // new prices for staging
            List<StagingPriceModel> expectedPrices = new List<StagingPriceModel>();
            foreach (var item in items)
            {
                StagingPriceModel stagingPrice = new TestStagingPriceModelBuilder()
                    .WithScanCode(item.ScanCode)
                    .WithTimestamp(this.timestamp)
                    .WithTransactionId(this.transactionId)
                    .WithBusinessUnit(businessUnit)
                    .WithStartDate(startDate)
                    .WithEndDate(endDate)
                    .WithRegion(this.region)
                    .WithPriceType(priceType)
                    .WithPrice(price)
                    .Build();
                expectedPrices.Add(stagingPrice);
            }

            return expectedPrices;
        }

        private List<Prices> BuildExistingPrices(IEnumerable<Item> items, int businessUnit, decimal price, string priceType, DateTime startDate, DateTime? endDate = null)
        {
            // new prices for staging
            List<Prices> existingPrices = new List<Prices>();
            foreach (var item in items)
            {
                // Existing prices
                Prices existing = new Prices()
                {
                    Region = this.region,
                    ItemID = item.ItemID,
                    BusinessUnitID = this.buId,
                    Price = price,
                    PriceUOM = "EA",
                    StartDate = startDate,
                    EndDate = endDate,
                    AddedDate = this.timestamp,
                    CurrencyID = 1,
                    Multiple = 1,
                    ModifiedDate = null,
                    PriceType = priceType
                };
                existingPrices.Add(existing);
            }

            return existingPrices;
        }

        private void AddPriceToPriceTable(List<Prices> prices)
        {
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(this.db.Connection as SqlConnection))
            {
                bulkCopy.ColumnMappings.Add("Region", "Region");
                bulkCopy.ColumnMappings.Add("ItemID", "ItemID");
                bulkCopy.ColumnMappings.Add("BusinessUnitID", "BusinessUnitID");
                bulkCopy.ColumnMappings.Add("StartDate", "StartDate");
                bulkCopy.ColumnMappings.Add("EndDate", "EndDate");
                bulkCopy.ColumnMappings.Add("Price", "Price");
                bulkCopy.ColumnMappings.Add("PriceType", "PriceType");
                bulkCopy.ColumnMappings.Add("PriceUOM", "PriceUOM");
                bulkCopy.ColumnMappings.Add("CurrencyID", "CurrencyID");
                bulkCopy.ColumnMappings.Add("Multiple", "Multiple");
                bulkCopy.ColumnMappings.Add("AddedDate", "AddedDate");
                bulkCopy.ColumnMappings.Add("ModifiedDate", "ModifiedDate");

                using (var reader = ObjectReader.Create(
                    prices))
                {
                    bulkCopy.DestinationTableName = $"[dbo].[Price_{this.region}]";
                    bulkCopy.WriteToServer(reader);
                }
            }
        }

        private void AddPricesToStagingTable(List<StagingPriceModel> pricesInStaging)
        {
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(this.db.Connection as SqlConnection))
            {
                using (var reader = ObjectReader.Create(
                    pricesInStaging,
                    nameof(StagingPriceModel.Region),
                    nameof(StagingPriceModel.ScanCode),
                    nameof(StagingPriceModel.BusinessUnitId),
                    nameof(StagingPriceModel.Multiple),
                    nameof(StagingPriceModel.Price),
                    nameof(StagingPriceModel.PriceType),
                    nameof(StagingPriceModel.StartDate),
                    nameof(StagingPriceModel.EndDate),
                    nameof(StagingPriceModel.PriceUom),
                    nameof(StagingPriceModel.CurrencyCode),
                    nameof(StagingPriceModel.Timestamp),
                    nameof(StagingPriceModel.TransactionId)))
                {
                    bulkCopy.DestinationTableName = "stage.Price";
                    bulkCopy.WriteToServer(reader);
                }
            }
        }

        private void AddToItemsTable(List<Item> items)
        {
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(this.db.Connection as SqlConnection))
            {
                using (var reader = ObjectReader.Create(
                    items,
                    nameof(Item.ItemID),
                    nameof(Item.ItemTypeID),
                    nameof(Item.ScanCode),
                    nameof(Item.HierarchyMerchandiseID),
                    nameof(Item.HierarchyNationalClassID),
                    nameof(Item.BrandHCID),
                    nameof(Item.TaxClassHCID),
                    nameof(Item.PSNumber),
                    nameof(Item.Desc_POS),
                    nameof(Item.Desc_Product),
                    nameof(Item.PackageUnit),
                    nameof(Item.RetailSize),
                    nameof(Item.RetailUOM),
                    nameof(Item.FoodStampEligible)))
                {
                    bulkCopy.DestinationTableName = "[dbo].[Items]";
                    bulkCopy.WriteToServer(reader);
                }
            }
        }

        private List<Item> BuildItems(int numberOfItems)
        {
            this.maxItemId = this.db.Connection.Query<int?>("SELECT MAX(ItemID) FROM Items", transaction: this.db.Transaction).FirstOrDefault();
            var newItems = new List<Item>();

            for (int i = 0; i < numberOfItems; i++)
            {
                newItems.Add(new TestItemBuilder().WithScanCode($"11111177{i.ToString()}").WithItemId((maxItemId ?? default(int)) + (i+1)).Build());
            }

            return newItems;
        }
    }
}
