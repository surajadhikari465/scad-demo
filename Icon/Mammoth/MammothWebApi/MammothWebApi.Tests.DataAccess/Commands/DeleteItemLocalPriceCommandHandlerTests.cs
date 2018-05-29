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
    public class DeleteItemLocalePriceCommandHandlerTests
    {
        private DeleteItemLocalePriceCommandHandler deleteItemLocalePriceCommandHandler;
        private DeleteItemLocalePriceCommand commandParameters;
        private IDbProvider db;
        private int businessUnitId;
        private string region = "FL";
        private List<Item> items;
        private int? maxItemId;

        [TestInitialize]
        public void InitializeTests()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Mammoth"].ConnectionString;
            this.db = new SqlDbProvider();
            this.db.Connection = new SqlConnection(connectionString);
            this.db.Connection.Open();
            this.db.Transaction = this.db.Connection.BeginTransaction();

            this.deleteItemLocalePriceCommandHandler = new DeleteItemLocalePriceCommandHandler(this.db);

            this.db.Connection
                .Execute(String.Format("INSERT INTO Locales_{0} (BusinessUnitID, StoreName, StoreAbbrev, AddedDate) VALUES (1, 'TEST STORE', 'TES', GETDATE())", this.region),
                    transaction: this.db.Transaction);

            this.businessUnitId = 1;

            this.maxItemId = this.db.Connection.Query<int?>("SELECT MAX(ItemID) FROM Items", transaction: this.db.Transaction).FirstOrDefault();
            this.items = new List<Item>();
            this.items.Add(new TestItemBuilder().WithScanCode("111111777771").WithItemId((maxItemId ?? default(int)) + 1).Build());
            AddToItemsTable(this.items);

            this.commandParameters = new DeleteItemLocalePriceCommand { Region = this.region, BusinessUnitId = this.businessUnitId, ScanCode = "111111777771" };
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
        public void DeleteItemLocalePriceCommand_ValidData_ShouldDeletePrices()
        {
            DateTime startDate = new DateTime(2018, 5, 25);
            DateTime endDate = startDate.AddDays(14);
            decimal existingPrice = 2.99M;
            string expectedPriceType = "REG";

            decimal futureSalePrice = 1.99M;
            string salePriceType = "SAL";

            List<Prices> existingPrices = BuildPrices(this.items, this.businessUnitId, existingPrice, expectedPriceType, startDate);
            List<Prices> salePrices = BuildPrices(this.items, this.businessUnitId, futureSalePrice, salePriceType, startDate, endDate);

            AddPriceToPriceTable(existingPrices);
            AddPriceToPriceTable(salePrices);

            // When
            this.deleteItemLocalePriceCommandHandler.Execute(commandParameters);

            // Then
            string actualSql = String.Format(@"SELECT * FROM Price_{0} 
                WHERE BusinessUnitID = @BU 
                    AND ItemID IN @ItemID;", this.region);

            var actual = this.db.Connection.Query<Prices>(actualSql,
                new { BU = this.businessUnitId, ItemID = this.items.Select(i => i.ItemID)},
                this.db.Transaction);

            Assert.AreEqual(0, actual.Count(), "Rows were returned instead of being deleted.");
        }

        private List<Prices> BuildPrices(
            IEnumerable<Item> items,
            int businessUnit,
            decimal price,
            string priceType,
            DateTime startDate,
            DateTime? endDate = null)
        {
            List<Prices> existingPrices = new List<Prices>();
            foreach (var item in items)
            {
                Prices existing = new Prices()
                {
                    Region = this.region,
                    ItemID = item.ItemID,
                    BusinessUnitID = this.businessUnitId,
                    Price = price,
                    PriceUOM = "EA",
                    StartDate = startDate,
                    EndDate = endDate,
                    AddedDate = DateTime.Now,
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
    }
}