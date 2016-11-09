using Dapper;
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
    public class DeletePricesCommandHandlerTests
    {
        private DeletePricesCommandHandler deleteCommandHandler;
        private DeletePricesCommand commandParameters;
        private IDbProvider db;
        private DateTime timestamp;
        private int buId;
        private string region = "FL";
        private List<Item> items;
        private int? maxItemId;
        private Guid transactionId;

        [TestInitialize]
        public void InitializeTests()
        {
            this.timestamp = DateTime.Now;
            string connectionString = ConfigurationManager.ConnectionStrings["Mammoth"].ConnectionString;
            this.db = new SqlDbProvider();
            this.db.Connection = new SqlConnection(connectionString);
            this.db.Connection.Open();
            this.db.Transaction = this.db.Connection.BeginTransaction();
            this.transactionId = Guid.NewGuid();

            this.deleteCommandHandler = new DeletePricesCommandHandler(this.db);

            this.db.Connection
                .Execute(String.Format("INSERT INTO Locales_{0} (BusinessUnitID, StoreName, StoreAbbrev, AddedDate) VALUES (1, 'TEST STORE', 'TES', GETDATE())", this.region),
                    transaction: this.db.Transaction);

            this.buId = 1; // the businessUnit inserted above

            // Add Test Items
            this.maxItemId = this.db.Connection.Query<int?>("SELECT MAX(ItemID) FROM Items", transaction: this.db.Transaction).FirstOrDefault();
            this.items = new List<Item>();
            this.items.Add(new TestItemBuilder().WithScanCode("111111777771").WithItemId((maxItemId ?? default(int)) + 1).Build());
            this.items.Add(new TestItemBuilder().WithScanCode("111111777772").WithItemId((maxItemId ?? default(int)) + 2).Build());
            this.items.Add(new TestItemBuilder().WithScanCode("111111777773").WithItemId((maxItemId ?? default(int)) + 3).Build());
            AddToItemsTable(this.items);

            this.commandParameters = new DeletePricesCommand { Region = this.region, Timestamp = this.timestamp, TransactionId = this.transactionId };
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
        public void DeletePricesCommand_RegPriceExistsAndStaged_ShouldDeletePrice()
        {
            // Given
            DateTime startDate = new DateTime(2015, 1, 16);
            decimal existingPrice = 2.99M;
            string expectedPriceType = "REG";

            List<Prices> existingPrices = BuildExistingPrices(this.items, this.buId, existingPrice, expectedPriceType, startDate);
            List<StagingPriceModel> expectedPrices = BuildStagedPrices(this.items, this.buId, existingPrice, expectedPriceType, startDate, this.transactionId);

            AddPriceToPriceTable(existingPrices);
            AddPricesToStagingTable(expectedPrices);
            
            // When
            this.deleteCommandHandler.Execute(commandParameters);

            // Then
            string actualSql = String.Format(@"SELECT * FROM Price_{0} 
                WHERE BusinessUnitID = @BU 
                    AND ItemID IN @ItemID 
                    AND StartDate = @StartDate
                    AND EndDate IS NULL
                    AND PriceType = @PriceType
                    AND Price = @Price;", this.region);

            var actual = this.db.Connection.Query<Prices>(actualSql,
                new { BU = this.buId, ItemID = this.items.Select(i => i.ItemID), StartDate = startDate, PriceType = expectedPriceType, Price = existingPrice },
                this.db.Transaction);

            Assert.AreEqual(0, actual.Count(), "Rows were returned instead of being deleted.");
        }

        [TestMethod]
        public void DeletePricesCommand_PromoPriceExistsAndStagedWithMatchingGuid_ShouldDeletePrices()
        {
            // Given
            DateTime startDate = new DateTime(2016, 1, 16);
            DateTime endDate = new DateTime(2016, 1, 25);
            decimal existingPrice = 3.99M;
            string expectedPriceType = "SAL";

            List<Prices> existingPrices = BuildExistingPrices(this.items, this.buId, existingPrice, expectedPriceType, startDate, endDate);
            List<StagingPriceModel> expectedPrices = BuildStagedPrices(this.items, this.buId, existingPrice, expectedPriceType, startDate, this.transactionId, endDate);

            AddPriceToPriceTable(existingPrices);
            AddPricesToStagingTable(expectedPrices);

            // When
            this.deleteCommandHandler.Execute(commandParameters);

            // Then
            string actualSql = String.Format(@"SELECT * FROM Price_{0} 
                WHERE BusinessUnitID = @BU 
                    AND ItemID IN @ItemID 
                    AND StartDate = @StartDate
                    AND EndDate = @EndDate
                    AND PriceType = @PriceType
                    AND Price = @Price;", this.region);

            var actual = this.db.Connection.Query<Prices>(actualSql,
                new { BU = this.buId, ItemID = this.items.Select(i => i.ItemID), StartDate = startDate, EndDate = endDate, PriceType = expectedPriceType, Price = existingPrice },
                this.db.Transaction);

            Assert.AreEqual(0, actual.Count(), "Rows were returned instead of being deleted.");
        }

        [TestMethod]
        public void DeletePricesCommand_PromoPricesAndRegPricesExistsAndStagedWithMatchingGuid_ShouldDeleteAllPrices()
        {
            // Given
            DateTime startDate = new DateTime(2016, 1, 16);
            DateTime endDate = new DateTime(2016, 1, 25);
            decimal existingPrice = 3.99M;

            List<Prices> existingPrices = BuildExistingPrices(this.items.OrderBy(i => i.ItemID).Take(2), this.buId, existingPrice, "REG", startDate);
            existingPrices.AddRange(BuildExistingPrices(this.items.OrderByDescending(i => i.ItemID).Take(1), this.buId, existingPrice, "SAL", startDate, endDate));
            List<StagingPriceModel> expectedPrices = BuildStagedPrices(this.items.OrderBy(i => i.ItemID).Take(2), this.buId, existingPrice, "REG", startDate, this.transactionId);
            expectedPrices.AddRange(BuildStagedPrices(this.items.OrderByDescending(i => i.ItemID).Take(1), this.buId, existingPrice, "SAL", startDate, this.transactionId, endDate));

            AddPriceToPriceTable(existingPrices);
            AddPricesToStagingTable(expectedPrices);

            // When
            this.deleteCommandHandler.Execute(commandParameters);

            // Then
            string actualSql = String.Format(@"SELECT * FROM Price_{0} 
                WHERE BusinessUnitID = @BU 
                    AND ItemID IN @ItemID 
                    AND StartDate = @StartDate
                    AND EndDate = @EndDate
                    AND Price = @Price;", this.region);

            var actual = this.db.Connection.Query<Prices>(actualSql,
                new { BU = this.buId, ItemID = this.items.Select(i => i.ItemID), StartDate = startDate, EndDate = endDate, Price = existingPrice },
                this.db.Transaction);

            Assert.AreEqual(0, actual.Count(), "Rows were returned instead of being deleted.");
        }

        [TestMethod]
        public void DeletePricesCommand_PriceExistsAndStagedButDoNotMatchStartDate_ShouldNotDeletePrices()
        {
            // Given
            DateTime startDate = new DateTime(2016, 1, 16);
            DateTime endDate = new DateTime(2016, 1, 25);
            decimal existingPrice = 3.99M;
            string expectedPriceType = "SAL";

            List<Prices> existingPrices = BuildExistingPrices(this.items, this.buId, existingPrice, expectedPriceType, startDate, endDate);
            List<StagingPriceModel> expectedPrices = BuildStagedPrices(this.items, this.buId, existingPrice, expectedPriceType, new DateTime(2016, 1, 1), this.transactionId, endDate);

            AddPriceToPriceTable(existingPrices);
            AddPricesToStagingTable(expectedPrices);

            // When
            this.deleteCommandHandler.Execute(commandParameters);

            // Then
            string actualSql = String.Format(@"SELECT * FROM Price_{0} 
                WHERE BusinessUnitID = @BU 
                    AND ItemID IN @ItemID 
                    AND StartDate = @StartDate
                    AND EndDate = @EndDate
                    AND PriceType = @PriceType
                    AND Price = @Price;", this.region);

            var actual = this.db.Connection.Query<Prices>(actualSql,
                new { BU = this.buId, ItemID = this.items.Select(i => i.ItemID), StartDate = startDate, EndDate = endDate, PriceType = expectedPriceType, Price = existingPrice },
                this.db.Transaction);

            Assert.AreEqual(this.items.Count, actual.Count(), "Rows were deleted when they should not have been.");
        }

        [TestMethod]
        public void DeletePricesCommand_PriceExistsAndStagedButDoNotMatchPrice_ShouldNotDeletePrices()
        {
            // Given
            DateTime startDate = new DateTime(2016, 1, 16);
            DateTime endDate = new DateTime(2016, 1, 25);
            decimal existingPrice = 3.99M;
            string expectedPriceType = "SAL";

            List<Prices> existingPrices = BuildExistingPrices(this.items, this.buId, existingPrice, expectedPriceType, startDate, endDate);
            List<StagingPriceModel> expectedPrices = BuildStagedPrices(this.items, this.buId, 5.99M, expectedPriceType, startDate, this.transactionId, endDate);

            AddPriceToPriceTable(existingPrices);
            AddPricesToStagingTable(expectedPrices);

            // When
            this.deleteCommandHandler.Execute(commandParameters);

            // Then
            string actualSql = String.Format(@"SELECT * FROM Price_{0} 
                WHERE BusinessUnitID = @BU 
                    AND ItemID IN @ItemID 
                    AND StartDate = @StartDate
                    AND EndDate = @EndDate
                    AND PriceType = @PriceType
                    AND Price = @Price;", this.region);

            var actual = this.db.Connection.Query<Prices>(actualSql,
                new { BU = this.buId, ItemID = this.items.Select(i => i.ItemID), StartDate = startDate, EndDate = endDate, PriceType = expectedPriceType, Price = existingPrice },
                this.db.Transaction);

            Assert.AreEqual(this.items.Count, actual.Count(), "Rows were deleted when they should not have been.");
        }

        [TestMethod]
        public void DeletePricesCommand_PriceExistsAndStagedButDoNotMatchEndDateAndEndDateIsNotNull_ShouldNotDeletePrices()
        {
            // Given
            DateTime startDate = new DateTime(2016, 1, 16);
            DateTime endDate = new DateTime(2016, 1, 25);
            decimal existingPrice = 3.99M;
            string expectedPriceType = "SAL";

            List<Prices> existingPrices = BuildExistingPrices(this.items, this.buId, existingPrice, expectedPriceType, startDate, endDate);
            List<StagingPriceModel> expectedPrices = BuildStagedPrices(this.items, this.buId, 5.99M, expectedPriceType, startDate, this.transactionId, new DateTime(2016, 2, 1));

            AddPriceToPriceTable(existingPrices);
            AddPricesToStagingTable(expectedPrices);

            // When
            this.deleteCommandHandler.Execute(commandParameters);

            // Then
            string actualSql = String.Format(@"SELECT * FROM Price_{0} 
                WHERE BusinessUnitID = @BU 
                    AND ItemID IN @ItemID 
                    AND StartDate = @StartDate
                    AND EndDate = @EndDate
                    AND PriceType = @PriceType
                    AND Price = @Price;", this.region);

            var actual = this.db.Connection.Query<Prices>(actualSql,
                new { BU = this.buId, ItemID = this.items.Select(i => i.ItemID), StartDate = startDate, EndDate = endDate, PriceType = expectedPriceType, Price = existingPrice },
                this.db.Transaction);

            Assert.AreEqual(this.items.Count, actual.Count(), "Rows were deleted when they should not have been.");
        }

        [TestMethod]
        public void DeletePricesCommand_PriceExistsAndStagedButDoNotMatchPriceType_ShouldNotDeletePrices()
        {
            // Given
            DateTime startDate = new DateTime(2016, 1, 16);
            DateTime endDate = new DateTime(2016, 1, 25);
            decimal existingPrice = 3.99M;
            string expectedPriceType = "SAL";

            List<Prices> existingPrices = BuildExistingPrices(this.items, this.buId, existingPrice, expectedPriceType, startDate, endDate);
            List<StagingPriceModel> expectedPrices = BuildStagedPrices(this.items, this.buId, 5.99M, "ISS", startDate, this.transactionId, endDate);

            AddPriceToPriceTable(existingPrices);
            AddPricesToStagingTable(expectedPrices);

            // When
            this.deleteCommandHandler.Execute(commandParameters);

            // Then
            string actualSql = String.Format(@"SELECT * FROM Price_{0} 
                WHERE BusinessUnitID = @BU 
                    AND ItemID IN @ItemID 
                    AND StartDate = @StartDate
                    AND EndDate = @EndDate
                    AND PriceType = @PriceType
                    AND Price = @Price;", this.region);

            var actual = this.db.Connection.Query<Prices>(actualSql,
                new { BU = this.buId, ItemID = this.items.Select(i => i.ItemID), StartDate = startDate, EndDate = endDate, PriceType = expectedPriceType, Price = existingPrice },
                this.db.Transaction);

            Assert.AreEqual(existingPrices.Count, actual.Count(), "Rows were deleted when they should not have been.");
        }

        [TestMethod]
        public void DeletePricesCommand_PricesInStagingWithNonMatchingGuid_ShouldNotDeletePrices()
        {
            // Given
            DateTime startDate = new DateTime(2016, 1, 16);
            DateTime endDate = new DateTime(2016, 1, 25);
            decimal existingPrice = 3.99M;
            string expectedPriceType = "SAL";
            Guid nonMatchingGuid = Guid.NewGuid();

            List<Prices> existingPrices = BuildExistingPrices(this.items, this.buId, existingPrice, expectedPriceType, startDate, endDate);
            List<StagingPriceModel> expectedPrices = BuildStagedPrices(this.items, this.buId, existingPrice, expectedPriceType, startDate, nonMatchingGuid, endDate);

            AddPriceToPriceTable(existingPrices);
            AddPricesToStagingTable(expectedPrices);

            this.commandParameters.Timestamp = DateTime.Today;

            // When
            this.deleteCommandHandler.Execute(commandParameters);

            // Then
            string actualSql = String.Format(@"SELECT * FROM Price_{0} 
                WHERE BusinessUnitID = @BU 
                    AND ItemID IN @ItemID 
                    AND StartDate = @StartDate
                    AND EndDate = @EndDate
                    AND PriceType = @PriceType
                    AND Price = @Price;", this.region);

            var actual = this.db.Connection.Query<Prices>(actualSql,
                new { BU = this.buId, ItemID = this.items.Select(i => i.ItemID), StartDate = startDate, EndDate = endDate, PriceType = expectedPriceType, Price = existingPrice },
                this.db.Transaction);

            Assert.AreEqual(this.items.Count, actual.Count(), "Rows were deleted when they should not have been.");
        }

        private List<StagingPriceModel> BuildStagedPrices(
            IEnumerable<Item> items, 
            int businessUnit, 
            decimal price, 
            string priceType, 
            DateTime startDate,
            Guid guid, 
            DateTime? endDate = null)
        {
            // new prices for staging
            List<StagingPriceModel> expectedPrices = new List<StagingPriceModel>();
            foreach (var item in items)
            {
                StagingPriceModel stagingPrice = new TestStagingPriceModelBuilder()
                    .WithScanCode(item.ScanCode)
                    .WithTimestamp(this.timestamp)
                    .WithBusinessUnit(buId)
                    .WithStartDate(startDate)
                    .WithEndDate(endDate)
                    .WithRegion(this.region)
                    .WithPriceType(priceType)
                    .WithPrice(price)
                    .WithTransactionId(guid)
                    .Build();
                expectedPrices.Add(stagingPrice);
            }

            return expectedPrices;
        }

        private List<Prices> BuildExistingPrices(
            IEnumerable<Item> items,
            int businessUnit,
            decimal price,
            string priceType,
            DateTime startDate,
            DateTime? endDate = null)
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

        private void AddPriceToPriceTable(List<Prices> prices)
        {
            string sql = String.Format(@"INSERT INTO Price_{0}
                                        (
	                                        Region,
	                                        ItemID,
	                                        BusinessUnitId,
	                                        Multiple,
	                                        Price,
	                                        StartDate,
                                            EndDate,
	                                        PriceUOM,
                                            PriceType,
	                                        CurrencyID,
	                                        AddedDate
                                        )
                                        VALUES
                                        (
	                                        @Region,
	                                        @ItemID,
	                                        @BusinessUnitID,
	                                        @Multiple,
	                                        @Price,
	                                        @StartDate,
                                            @EndDate,
	                                        @PriceUom,
                                            @PriceType,
	                                        @CurrencyID,
                                            @AddedDate
                                        )", this.region);

            this.db.Connection.Execute(sql, prices, this.db.Transaction);
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
    }
}
