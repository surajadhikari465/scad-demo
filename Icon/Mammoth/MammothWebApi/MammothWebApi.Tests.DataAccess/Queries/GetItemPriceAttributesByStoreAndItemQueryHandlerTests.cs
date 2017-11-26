using Dapper;
using Mammoth.Common.DataAccess.DbProviders;
using MammothWebApi.DataAccess.Models;
using MammothWebApi.DataAccess.Queries;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using Testing.Core;

namespace MammothWebApi.Tests.DataAccess.Queries
{
    [TestClass]
    public class GetItemPriceAttributesByStoreAndItemQueryHandlerTests
    {
        private IDbProvider db;
        private GetItemPriceAttributesByStoreAndScanCodeQueryHandler queryHandler;
        private GetItemPriceAttributesByStoreAndScanCodeQuery query;

        private int expectedStoreBuId = 70001;
        private int expectedItemId = 900001;
        private string expectedItemScanCode = "7000000000001";
        private DateTime expectedEffectiveDate = DateTime.Today;
        private string expectedRegion = "FL";
        private string expectedBrandName = "TestBrandName";
        private int expectedBrandId = 444444444;
        private string expectedSubTeamName = "TestSubTeamName";
        private int expectedPsNumber = 50000005;
        private bool expectedAuthorized = true;
        private string expectedCurrencyCode = "USD";
        private string expectedItemDescription = "My Test Product 1";

        private ObjectBuilderFactory objectFactory;
        private DapperSqlFactory dapperFactory;

        private string insertStoreSql;
        private string insertBrandSql;
        private string insertSubTeamSql;
        private string insertItemLocaleSql;
        private string insertItemSql;
        private string insertPriceSql;

        private bool? expectedFoodStamp = true;
        private string expectedPosDescription = "My Test Pos 1";
        private string expectedPackageUnit = "1";
        private string expectedRetailSize = "1";
        private string expectedRetailUom = "EA";
        private string expectedSignDescription = "My Test Sign Description";
        private bool expectedDiscountCase = true;
        private bool expectedDiscountTm = true;
        private bool expectedRestrictedHours = false;
        private bool expectedDiscontinued = false;
        private bool expectedLocalItem = false;
        private decimal expectedMsrp = 4.99m;

        [TestInitialize()]
        public void MyTestInitialize()
        {
            db = new SqlDbProvider();
            db.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Mammoth"].ConnectionString);
            db.Connection.Open();
            db.Transaction = db.Connection.BeginTransaction();

            this.query = new GetItemPriceAttributesByStoreAndScanCodeQuery();
            this.queryHandler = new GetItemPriceAttributesByStoreAndScanCodeQueryHandler(this.db);
            this.objectFactory = new ObjectBuilderFactory(this.GetType().Assembly);
            this.dapperFactory = new DapperSqlFactory(this.GetType().Assembly);

            SetupInsertSqlStatements();
            InsertItemAttributeData();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            this.db.Transaction.Rollback();
            this.db.Transaction.Dispose();
            this.db.Connection.Dispose();
        }

        [TestMethod]
        public void GetPricesGpmQuery_ValidStoreAndItem_ReturnsPrices()
        {
            // Given
            DateTime expectedStartDate = DateTime.UtcNow.Date.AddDays(-3);

            this.db.Connection.Execute(
                insertPriceSql,
                objectFactory.Build<PricesGpm>()
                    .With(p => p.Region, expectedRegion)
                    .With(p => p.ItemID, expectedItemId)
                    .With(p => p.BusinessUnitID, expectedStoreBuId)
                    .With(p => p.Price, 3.50m)
                    .With(p => p.PriceType, "REG")
                    .With(p => p.StartDate, expectedStartDate)
                    .With(p => p.CurrencyCode, "USD")
                    .With(p => p.GpmID, Guid.NewGuid())
                    .With(p => p.SellableUOM, "EA")
                    .With(p => p.InsertDateUtc, DateTime.UtcNow)
                    .With(p => p.PriceTypeAttribute, "REG").CreatedObject,
                this.db.Transaction);

            this.query = new GetItemPriceAttributesByStoreAndScanCodeQuery
            {
                Region = expectedRegion,
                EffectiveDate = expectedEffectiveDate,
                StoreScanCodeCollection = new List<StoreScanCode>
                {
                    new StoreScanCode
                    {
                        ScanCode = expectedItemScanCode,
                        BusinessUnitID = expectedStoreBuId
                    }
                }
            };

            // When
            var prices = this.queryHandler.Search(this.query);

            // Then
            var price = prices.Single(p => p.ScanCode == expectedItemScanCode && p.BusinessUnitID == expectedStoreBuId);
            Assert.AreEqual(3.50m, price.Price);
            Assert.AreEqual(expectedStartDate, price.StartDate);
            Assert.AreEqual(expectedAuthorized, price.Authorized);
            Assert.AreEqual(expectedItemId, price.ItemId);
            Assert.AreEqual(expectedItemScanCode, price.ScanCode);
            Assert.AreEqual(expectedStoreBuId, price.BusinessUnitID);
            Assert.AreEqual(expectedCurrencyCode, price.Currency);
            Assert.IsNull(price.EndDate, "EndDate is not null");
            Assert.AreEqual(expectedFoodStamp.GetValueOrDefault(true), price.FoodStamp);
            Assert.AreEqual(expectedItemDescription, price.ItemDescription);
            Assert.AreEqual(expectedSignDescription, price.SignDescription);
            Assert.AreEqual(expectedBrandName, price.BrandName);
            Assert.AreEqual(expectedRetailSize, price.RetailSize);
            Assert.AreEqual(expectedRetailUom, price.RetailUom);
            Assert.AreEqual("EA", price.SellableUom);
            Assert.AreEqual(expectedSubTeamName, price.SubTeam);
        }

        [TestMethod]
        public void GetPricesGpmQuery_ExpiredRegularPriceExists_ReturnsOnlyActiveRegularPrice()
        {
            // Given
            this.db.Connection.Execute(
                insertPriceSql,
                objectFactory.Build<PricesGpm>()
                    .With(p => p.Region, expectedRegion)
                    .With(p => p.ItemID, expectedItemId)
                    .With(p => p.BusinessUnitID, expectedStoreBuId)
                    .With(p => p.Price, 3.50m)
                    .With(p => p.PriceType, "REG")
                    .With(p => p.StartDate, DateTime.UtcNow.Date.AddDays(-3))
                    .With(p => p.CurrencyCode, "USD")
                    .With(p => p.GpmID, Guid.NewGuid())
                    .With(p => p.SellableUOM, "EA")
                    .With(p => p.InsertDateUtc, DateTime.UtcNow)
                    .With(p => p.PriceTypeAttribute, "REG").CreatedObject,
                this.db.Transaction);

            this.db.Connection.Execute(
                insertPriceSql,
                objectFactory.Build<PricesGpm>()
                    .With(p => p.Region, expectedRegion)
                    .With(p => p.ItemID, expectedItemId)
                    .With(p => p.BusinessUnitID, expectedStoreBuId)
                    .With(p => p.Price, 3.50m)
                    .With(p => p.PriceType, "REG")
                    .With(p => p.StartDate, DateTime.UtcNow.Date.AddDays(-20)) // another REG starting earlier than the first price
                    .With(p => p.CurrencyCode, "USD")
                    .With(p => p.GpmID, Guid.NewGuid())
                    .With(p => p.SellableUOM, "EA")
                    .With(p => p.InsertDateUtc, DateTime.UtcNow)
                    .With(p => p.PriceTypeAttribute, "REG").CreatedObject,
                this.db.Transaction);

            this.query = new GetItemPriceAttributesByStoreAndScanCodeQuery
            {
                Region = expectedRegion,
                EffectiveDate = expectedEffectiveDate,
                IncludeFuturePrices = false,
                StoreScanCodeCollection = new List<StoreScanCode>
                {
                    new StoreScanCode { BusinessUnitID = expectedStoreBuId, ScanCode = expectedItemScanCode }
                }
            };

            // When
            var prices = this.queryHandler.Search(this.query);

            // Then
            // expect to only get one REG price with the newer StartDate
            var price = prices.Single(p => p.ScanCode == expectedItemScanCode && p.BusinessUnitID == expectedStoreBuId);
            Assert.AreEqual(DateTime.UtcNow.Date.AddDays(-3), price.StartDate);

        }

        [TestMethod]
        public void GetPricesGpmQuery_ExpiredSalePriceExists_ReturnsOnlyActiveSalePrice()
        {
            // Given
            DateTime expectedRegularStartDate = DateTime.UtcNow.Date.AddDays(-3);
            DateTime expectedTprStartDate = DateTime.UtcNow.Date.AddDays(-2);
            DateTime expectedTprEndDate = DateTime.UtcNow.Date.AddDays(15);

            this.db.Connection.Execute(
                insertPriceSql,
                objectFactory.Build<PricesGpm>()
                    .With(p => p.Region, expectedRegion)
                    .With(p => p.ItemID, expectedItemId)
                    .With(p => p.BusinessUnitID, expectedStoreBuId)
                    .With(p => p.Price, 3.50m)
                    .With(p => p.PriceType, "REG")
                    .With(p => p.StartDate, expectedRegularStartDate)
                    .With(p => p.CurrencyCode, "USD")
                    .With(p => p.GpmID, Guid.NewGuid())
                    .With(p => p.SellableUOM, "EA")
                    .With(p => p.InsertDateUtc, DateTime.UtcNow)
                    .With(p => p.PriceTypeAttribute, "REG").CreatedObject,
                this.db.Transaction);

            this.db.Connection.Execute(
                insertPriceSql,
                objectFactory.Build<PricesGpm>()
                    .With(p => p.Region, expectedRegion)
                    .With(p => p.ItemID, expectedItemId)
                    .With(p => p.BusinessUnitID, expectedStoreBuId)
                    .With(p => p.Price, 1.50m)
                    .With(p => p.PriceType, "TPR")
                    .With(p => p.StartDate, expectedTprStartDate)
                    .With(p => p.EndDate, expectedTprEndDate)
                    .With(p => p.CurrencyCode, "USD")
                    .With(p => p.GpmID, Guid.NewGuid())
                    .With(p => p.SellableUOM, "EA")
                    .With(p => p.InsertDateUtc, DateTime.UtcNow)
                    .With(p => p.PriceTypeAttribute, "MSAL").CreatedObject,
                this.db.Transaction);

            this.db.Connection.Execute(
                insertPriceSql,
                objectFactory.Build<PricesGpm>()
                    .With(p => p.Region, expectedRegion)
                    .With(p => p.ItemID, expectedItemId)
                    .With(p => p.BusinessUnitID, expectedStoreBuId)
                    .With(p => p.Price, 2.50m)
                    .With(p => p.PriceType, "TPR")
                    .With(p => p.StartDate, DateTime.UtcNow.Date.AddDays(-8))
                    .With(p => p.EndDate, DateTime.UtcNow.Date.AddDays(-4)) // EndDate is before today
                    .With(p => p.CurrencyCode, "USD")
                    .With(p => p.GpmID, Guid.NewGuid())
                    .With(p => p.SellableUOM, "EA")
                    .With(p => p.InsertDateUtc, DateTime.UtcNow)
                    .With(p => p.PriceTypeAttribute, "MSAL").CreatedObject,
                this.db.Transaction);

            this.query = new GetItemPriceAttributesByStoreAndScanCodeQuery
            {
                Region = expectedRegion,
                EffectiveDate = expectedEffectiveDate,
                StoreScanCodeCollection = new List<StoreScanCode>
                {
                    new StoreScanCode { ScanCode = expectedItemScanCode, BusinessUnitID = expectedStoreBuId }
                }
            };

            // When
            var prices = this.queryHandler.Search(this.query);

            // Then
            // Expect only one REG and one TPR
            Assert.AreEqual(1, prices.Count(p => p.PriceType == "REG"));
            Assert.AreEqual(expectedRegularStartDate, prices.Single(p => p.PriceType == "REG").StartDate);
            Assert.AreEqual(expectedTprStartDate, prices.First(p => p.PriceType == "TPR").StartDate);
            Assert.AreEqual(expectedTprEndDate, prices.First(p => p.PriceType == "TPR").EndDate);
        }

        [TestMethod]
        public void GetPricesGpmQuery_SaleAndRewardPriceActive_ReturnsRegularAndSaleAndRewardPrice()
        {
            // Given
            DateTime expectedRegularStartDate = DateTime.UtcNow.Date.AddDays(-3);
            DateTime expectedTprStartDate = DateTime.UtcNow.Date.AddDays(-2);
            DateTime expectedTprEndDate = DateTime.UtcNow.Date.AddDays(15);
            DateTime expectedRewardStartDate = DateTime.UtcNow.Date.AddDays(-2);
            DateTime expectedRewardEndDate = DateTime.UtcNow.Date.AddDays(15);

            this.db.Connection.Execute(
                insertPriceSql,
                objectFactory.Build<PricesGpm>()
                    .With(p => p.Region, expectedRegion)
                    .With(p => p.ItemID, expectedItemId)
                    .With(p => p.BusinessUnitID, expectedStoreBuId)
                    .With(p => p.Price, 3.50m)
                    .With(p => p.PriceType, "REG")
                    .With(p => p.StartDate, expectedRegularStartDate)
                    .With(p => p.CurrencyCode, "USD")
                    .With(p => p.GpmID, Guid.NewGuid())
                    .With(p => p.SellableUOM, "EA")
                    .With(p => p.InsertDateUtc, DateTime.UtcNow)
                    .With(p => p.PriceTypeAttribute, "REG").CreatedObject,
                this.db.Transaction);

            this.db.Connection.Execute(
                insertPriceSql,
                objectFactory.Build<PricesGpm>()
                    .With(p => p.Region, expectedRegion)
                    .With(p => p.ItemID, expectedItemId)
                    .With(p => p.BusinessUnitID, expectedStoreBuId)
                    .With(p => p.Price, 1.50m)
                    .With(p => p.PriceType, "TPR")
                    .With(p => p.StartDate, expectedTprStartDate)
                    .With(p => p.EndDate, expectedTprEndDate)
                    .With(p => p.CurrencyCode, "USD")
                    .With(p => p.GpmID, Guid.NewGuid())
                    .With(p => p.SellableUOM, "EA")
                    .With(p => p.InsertDateUtc, DateTime.UtcNow)
                    .With(p => p.PriceTypeAttribute, "MSAL").CreatedObject,
                this.db.Transaction);

            this.db.Connection.Execute(
                insertPriceSql,
                objectFactory.Build<PricesGpm>()
                    .With(p => p.Region, expectedRegion)
                    .With(p => p.ItemID, expectedItemId)
                    .With(p => p.BusinessUnitID, expectedStoreBuId)
                    .With(p => p.Price, 2.50m)
                    .With(p => p.PriceType, "TPR")
                    .With(p => p.StartDate, DateTime.UtcNow.Date.AddDays(-8))
                    .With(p => p.EndDate, DateTime.UtcNow.Date.AddDays(-4)) // EndDate is before today
                    .With(p => p.CurrencyCode, "USD")
                    .With(p => p.GpmID, Guid.NewGuid())
                    .With(p => p.SellableUOM, "EA")
                    .With(p => p.InsertDateUtc, DateTime.UtcNow)
                    .With(p => p.PriceTypeAttribute, "MSAL").CreatedObject,
                this.db.Transaction);

            this.db.Connection.Execute(
                insertPriceSql,
                objectFactory.Build<PricesGpm>()
                    .With(p => p.Region, expectedRegion)
                    .With(p => p.ItemID, expectedItemId)
                    .With(p => p.BusinessUnitID, expectedStoreBuId)
                    .With(p => p.Price, 2.50m)
                    .With(p => p.PriceType, "RWD")
                    .With(p => p.StartDate, expectedRewardStartDate)
                    .With(p => p.EndDate, expectedRewardEndDate)
                    .With(p => p.CurrencyCode, "USD")
                    .With(p => p.GpmID, Guid.NewGuid())
                    .With(p => p.SellableUOM, "EA")
                    .With(p => p.InsertDateUtc, DateTime.UtcNow)
                    .With(p => p.PriceTypeAttribute, "PRM").CreatedObject,
                this.db.Transaction);

            this.query = new GetItemPriceAttributesByStoreAndScanCodeQuery
            {
                Region = expectedRegion,
                EffectiveDate = expectedEffectiveDate,
                StoreScanCodeCollection = new List<StoreScanCode>
                {
                    new StoreScanCode { ScanCode = expectedItemScanCode, BusinessUnitID = expectedStoreBuId }
                }
            };

            // When
            var prices = this.queryHandler.Search(this.query);

            // Then
            // Expect only one REG and one TPR and one RWD price
            Assert.AreEqual(1, prices.Count(p => p.PriceType == "REG"));
            Assert.AreEqual(3, prices.Count());
            Assert.AreEqual(expectedRegularStartDate, prices.Single(p => p.PriceType == "REG").StartDate);
            Assert.AreEqual(expectedTprStartDate, prices.First(p => p.PriceType == "TPR").StartDate);
            Assert.AreEqual(expectedTprEndDate, prices.First(p => p.PriceType == "TPR").EndDate);
            Assert.AreEqual(expectedRewardStartDate, prices.First(p => p.PriceType == "RWD").StartDate);
            Assert.AreEqual(expectedRewardEndDate, prices.First(p => p.PriceType == "RWD").EndDate);
        }

        [TestMethod]
        public void GetPricesGpmQuery_FuturePricesExistsAndIncludeFuturePricesIsTrue_ReturnsCurrentAndFuturePrice()
        {
            // Given
            DateTime expectedRegularStartDate = DateTime.UtcNow.Date.AddDays(-3);
            DateTime expectedTprStartDate = DateTime.UtcNow.Date.AddDays(-2);
            DateTime expectedTprEndDate = DateTime.UtcNow.Date.AddDays(15);
            DateTime expectedFutureRegStartDate = DateTime.UtcNow.Date.AddDays(30);
            DateTime expectedFutureTprStartDate = DateTime.UtcNow.Date.AddDays(20);
            DateTime expectedFutureTprEndDate = DateTime.UtcNow.Date.AddDays(40);

            this.db.Connection.Execute(
                insertPriceSql,
                objectFactory.Build<PricesGpm>()
                    .With(p => p.Region, expectedRegion)
                    .With(p => p.ItemID, expectedItemId)
                    .With(p => p.BusinessUnitID, expectedStoreBuId)
                    .With(p => p.Price, 3.50m)
                    .With(p => p.PriceType, "REG")
                    .With(p => p.StartDate, expectedRegularStartDate)
                    .With(p => p.CurrencyCode, "USD")
                    .With(p => p.GpmID, Guid.NewGuid())
                    .With(p => p.SellableUOM, "EA")
                    .With(p => p.InsertDateUtc, DateTime.UtcNow)
                    .With(p => p.PriceTypeAttribute, "REG").CreatedObject,
                this.db.Transaction);

            this.db.Connection.Execute(
                insertPriceSql,
                objectFactory.Build<PricesGpm>()
                    .With(p => p.Region, expectedRegion)
                    .With(p => p.ItemID, expectedItemId)
                    .With(p => p.BusinessUnitID, expectedStoreBuId)
                    .With(p => p.Price, 1.50m)
                    .With(p => p.PriceType, "TPR")
                    .With(p => p.StartDate, expectedTprStartDate)
                    .With(p => p.EndDate, expectedTprEndDate)
                    .With(p => p.CurrencyCode, "USD")
                    .With(p => p.GpmID, Guid.NewGuid())
                    .With(p => p.SellableUOM, "EA")
                    .With(p => p.InsertDateUtc, DateTime.UtcNow)
                    .With(p => p.PriceTypeAttribute, "MSAL").CreatedObject,
                this.db.Transaction);

            this.db.Connection.Execute(
                insertPriceSql,
                objectFactory.Build<PricesGpm>()
                    .With(p => p.Region, expectedRegion)
                    .With(p => p.ItemID, expectedItemId)
                    .With(p => p.BusinessUnitID, expectedStoreBuId)
                    .With(p => p.Price, 2.50m)
                    .With(p => p.PriceType, "TPR")
                    .With(p => p.StartDate, DateTime.UtcNow.Date.AddDays(-8))
                    .With(p => p.EndDate, DateTime.UtcNow.Date.AddDays(-4)) // EndDate is before today
                    .With(p => p.CurrencyCode, "USD")
                    .With(p => p.GpmID, Guid.NewGuid())
                    .With(p => p.SellableUOM, "EA")
                    .With(p => p.InsertDateUtc, DateTime.UtcNow)
                    .With(p => p.PriceTypeAttribute, "MSAL").CreatedObject,
                this.db.Transaction);

            this.db.Connection.Execute(
                insertPriceSql,
                objectFactory.Build<PricesGpm>()
                    .With(p => p.Region, expectedRegion)
                    .With(p => p.ItemID, expectedItemId)
                    .With(p => p.BusinessUnitID, expectedStoreBuId)
                    .With(p => p.Price, 4.49m)
                    .With(p => p.PriceType, "REG")
                    .With(p => p.StartDate, expectedFutureRegStartDate) // future REG
                    .With(p => p.CurrencyCode, "USD")
                    .With(p => p.GpmID, Guid.NewGuid())
                    .With(p => p.SellableUOM, "EA")
                    .With(p => p.InsertDateUtc, DateTime.UtcNow)
                    .With(p => p.PriceTypeAttribute, "REG").CreatedObject,
                this.db.Transaction);

            this.db.Connection.Execute(
                insertPriceSql,
                objectFactory.Build<PricesGpm>()
                    .With(p => p.Region, expectedRegion)
                    .With(p => p.ItemID, expectedItemId)
                    .With(p => p.BusinessUnitID, expectedStoreBuId)
                    .With(p => p.Price, 4.49m)
                    .With(p => p.PriceType, "TPR")
                    .With(p => p.StartDate, expectedFutureTprStartDate) // future TPR
                    .With(p => p.EndDate, expectedFutureTprEndDate)
                    .With(p => p.CurrencyCode, "USD")
                    .With(p => p.GpmID, Guid.NewGuid())
                    .With(p => p.SellableUOM, "EA")
                    .With(p => p.InsertDateUtc, DateTime.UtcNow)
                    .With(p => p.PriceTypeAttribute, "SSAL").CreatedObject,
                this.db.Transaction);

            this.query = new GetItemPriceAttributesByStoreAndScanCodeQuery
            {
                Region = expectedRegion,
                EffectiveDate = expectedEffectiveDate,
                IncludeFuturePrices = true,
                StoreScanCodeCollection = new List<StoreScanCode>
                {
                    new StoreScanCode { BusinessUnitID = expectedStoreBuId, ScanCode = expectedItemScanCode }
                }
            };

            // When
            var prices = this.queryHandler.Search(this.query);

            // Then
            // Expect current and future prices
            Assert.AreEqual(4, prices.Count(), "Price count is not correct.");
        }

        [TestMethod]
        public void GetPricesGpmQuery_FuturePricesExistsAndIncludeFuturePricesIsFalse_ReturnsOnlyCurrentPrices()
        {
            // Given
            DateTime expectedRegularStartDate = DateTime.UtcNow.Date.AddDays(-3);
            DateTime expectedTprStartDate = DateTime.UtcNow.Date.AddDays(-2);
            DateTime expectedTprEndDate = DateTime.UtcNow.Date.AddDays(15);
            DateTime expectedFutureRegStartDate = DateTime.UtcNow.Date.AddDays(30);
            DateTime expectedFutureTprStartDate = DateTime.UtcNow.Date.AddDays(20);
            DateTime expectedFutureTprEndDate = DateTime.UtcNow.Date.AddDays(40);

            this.db.Connection.Execute(
                insertPriceSql,
                objectFactory.Build<PricesGpm>()
                    .With(p => p.Region, expectedRegion)
                    .With(p => p.ItemID, expectedItemId)
                    .With(p => p.BusinessUnitID, expectedStoreBuId)
                    .With(p => p.Price, 3.50m)
                    .With(p => p.PriceType, "REG")
                    .With(p => p.StartDate, expectedRegularStartDate)
                    .With(p => p.CurrencyCode, "USD")
                    .With(p => p.GpmID, Guid.NewGuid())
                    .With(p => p.SellableUOM, "EA")
                    .With(p => p.InsertDateUtc, DateTime.UtcNow)
                    .With(p => p.PriceTypeAttribute, "REG").CreatedObject,
                this.db.Transaction);

            this.db.Connection.Execute(
                insertPriceSql,
                objectFactory.Build<PricesGpm>()
                    .With(p => p.Region, expectedRegion)
                    .With(p => p.ItemID, expectedItemId)
                    .With(p => p.BusinessUnitID, expectedStoreBuId)
                    .With(p => p.Price, 1.50m)
                    .With(p => p.PriceType, "TPR")
                    .With(p => p.StartDate, expectedTprStartDate)
                    .With(p => p.EndDate, expectedTprEndDate)
                    .With(p => p.CurrencyCode, "USD")
                    .With(p => p.GpmID, Guid.NewGuid())
                    .With(p => p.SellableUOM, "EA")
                    .With(p => p.InsertDateUtc, DateTime.UtcNow)
                    .With(p => p.PriceTypeAttribute, "MSAL").CreatedObject,
                this.db.Transaction);

            this.db.Connection.Execute(
                insertPriceSql,
                objectFactory.Build<PricesGpm>()
                    .With(p => p.Region, expectedRegion)
                    .With(p => p.ItemID, expectedItemId)
                    .With(p => p.BusinessUnitID, expectedStoreBuId)
                    .With(p => p.Price, 2.50m)
                    .With(p => p.PriceType, "TPR")
                    .With(p => p.StartDate, DateTime.UtcNow.Date.AddDays(-8))
                    .With(p => p.EndDate, DateTime.UtcNow.Date.AddDays(-4)) // EndDate is before today
                    .With(p => p.CurrencyCode, "USD")
                    .With(p => p.GpmID, Guid.NewGuid())
                    .With(p => p.SellableUOM, "EA")
                    .With(p => p.InsertDateUtc, DateTime.UtcNow)
                    .With(p => p.PriceTypeAttribute, "MSAL").CreatedObject,
                this.db.Transaction);

            this.db.Connection.Execute(
                insertPriceSql,
                objectFactory.Build<PricesGpm>()
                    .With(p => p.Region, expectedRegion)
                    .With(p => p.ItemID, expectedItemId)
                    .With(p => p.BusinessUnitID, expectedStoreBuId)
                    .With(p => p.Price, 4.49m)
                    .With(p => p.PriceType, "REG")
                    .With(p => p.StartDate, expectedFutureRegStartDate) // future REG
                    .With(p => p.CurrencyCode, "USD")
                    .With(p => p.GpmID, Guid.NewGuid())
                    .With(p => p.SellableUOM, "EA")
                    .With(p => p.InsertDateUtc, DateTime.UtcNow)
                    .With(p => p.PriceTypeAttribute, "REG").CreatedObject,
                this.db.Transaction);

            this.db.Connection.Execute(
                insertPriceSql,
                objectFactory.Build<PricesGpm>()
                    .With(p => p.Region, expectedRegion)
                    .With(p => p.ItemID, expectedItemId)
                    .With(p => p.BusinessUnitID, expectedStoreBuId)
                    .With(p => p.Price, 4.49m)
                    .With(p => p.PriceType, "TPR")
                    .With(p => p.StartDate, expectedFutureTprStartDate) // future TPR
                    .With(p => p.EndDate, expectedFutureTprEndDate)
                    .With(p => p.CurrencyCode, "USD")
                    .With(p => p.GpmID, Guid.NewGuid())
                    .With(p => p.SellableUOM, "EA")
                    .With(p => p.InsertDateUtc, DateTime.UtcNow)
                    .With(p => p.PriceTypeAttribute, "SSAL").CreatedObject,
                this.db.Transaction);

            this.query = new GetItemPriceAttributesByStoreAndScanCodeQuery
            {
                Region = expectedRegion,
                EffectiveDate = expectedEffectiveDate,
                IncludeFuturePrices = false,
                StoreScanCodeCollection = new List<StoreScanCode>
                {
                    new StoreScanCode { BusinessUnitID = expectedStoreBuId, ScanCode = expectedItemScanCode }
                }
            };

            // When
            var prices = this.queryHandler.Search(this.query);

            // Then
            // Expect current and future prices
            Assert.AreEqual(2, prices.Count(), "Price count is not correct.");
            Assert.AreEqual(expectedRegularStartDate, prices.First(p => p.PriceType == "REG").StartDate);
            Assert.AreEqual(expectedTprStartDate, prices.First(p => p.PriceType != "REG").StartDate);
            Assert.IsNull(prices.FirstOrDefault(p => p.StartDate > DateTime.Today));
        }

        [TestMethod]
        public void GetPricesGpmQuery_NoPricesExists_ReturnsEmptyCollection()
        {
            // Given
            this.query = new GetItemPriceAttributesByStoreAndScanCodeQuery
            {
                Region = expectedRegion,
                EffectiveDate = expectedEffectiveDate,
                IncludeFuturePrices = true,
                StoreScanCodeCollection = new List<StoreScanCode>
                {
                    new StoreScanCode { BusinessUnitID = expectedStoreBuId, ScanCode = expectedItemScanCode }
                }
            };

            // When
            var prices = this.queryHandler.Search(this.query);

            // Then
            // Expect current and future prices
            Assert.AreEqual(0, prices.Count());
        }

        [TestMethod]
        public void GetPricesGpmQuery_EffectiveDateInTheFuture_ReturnsFutureRegularPriceInsteadOfCurrentRegularPrice()
        {
            // Given
            // Given
            DateTime expectedRegularStartDate = DateTime.UtcNow.Date.AddDays(-3);
            DateTime expectedFutureRegStartDate = DateTime.UtcNow.Date.AddDays(5);

            this.db.Connection.Execute(
                insertPriceSql,
                objectFactory.Build<PricesGpm>()
                    .With(p => p.Region, expectedRegion)
                    .With(p => p.ItemID, expectedItemId)
                    .With(p => p.BusinessUnitID, expectedStoreBuId)
                    .With(p => p.Price, 3.50m)
                    .With(p => p.PriceType, "REG")
                    .With(p => p.StartDate, expectedRegularStartDate)
                    .With(p => p.CurrencyCode, "USD")
                    .With(p => p.GpmID, Guid.NewGuid())
                    .With(p => p.SellableUOM, "EA")
                    .With(p => p.InsertDateUtc, DateTime.UtcNow)
                    .With(p => p.PriceTypeAttribute, "REG").CreatedObject,
                this.db.Transaction);

            this.db.Connection.Execute(
                insertPriceSql,
                objectFactory.Build<PricesGpm>()
                    .With(p => p.Region, expectedRegion)
                    .With(p => p.ItemID, expectedItemId)
                    .With(p => p.BusinessUnitID, expectedStoreBuId)
                    .With(p => p.Price, 2.99m)
                    .With(p => p.PriceType, "REG")
                    .With(p => p.StartDate, expectedFutureRegStartDate)
                    .With(p => p.CurrencyCode, "USD")
                    .With(p => p.GpmID, Guid.NewGuid())
                    .With(p => p.SellableUOM, "EA")
                    .With(p => p.InsertDateUtc, DateTime.UtcNow)
                    .With(p => p.PriceTypeAttribute, "REG").CreatedObject,
                this.db.Transaction);

            this.query = new GetItemPriceAttributesByStoreAndScanCodeQuery
            {
                Region = expectedRegion,
                EffectiveDate = expectedFutureRegStartDate,
                IncludeFuturePrices = false,
                StoreScanCodeCollection = new List<StoreScanCode>
                {
                    new StoreScanCode { BusinessUnitID = expectedStoreBuId, ScanCode = expectedItemScanCode }
                }
            };

            // When
            var prices = this.queryHandler.Search(this.query);

            // Then
            // Expect current and future prices
            Assert.AreEqual(1, prices.Count());
            Assert.AreEqual(expectedFutureRegStartDate, prices.Single().StartDate);
        }

        private void SetupInsertSqlStatements()
        {
            this.insertStoreSql = dapperFactory.BuildInsertSql<Locales>(true);
            this.insertBrandSql = @"INSERT INTO dbo.HierarchyClass (HierarchyClassID, HierarchyID, HierarchyClassName, AddedDate) VALUES (@HierarchyClassID, @HierarchyID, @HierarchyClassName, @AddedDate)";
            this.insertSubTeamSql = @"INSERT INTO dbo.Financial_SubTeam (Name, PSNumber, AddedDate) VALUES (@Name, @PSNumber, @AddedDate)";
            this.insertItemLocaleSql = @"INSERT INTO [dbo].[ItemAttributes_Locale_FL]
                                           ([Region]
                                           ,[ItemID]
                                           ,[BusinessUnitID]
                                           ,[Discount_Case]
                                           ,[Discount_TM]
                                           ,[Restriction_Age]
                                           ,[Restriction_Hours]
                                           ,[Authorized]
                                           ,[Discontinued]
                                           ,[LocalItem]
                                           ,[LabelTypeDesc]
                                           ,[Product_Code]
                                           ,[RetailUnit]
                                           ,[Sign_Desc]
                                           ,[Locality]
                                           ,[Sign_RomanceText_Long]
                                           ,[Sign_RomanceText_Short]
                                           ,[MSRP]
                                           ,[AddedDate])
                                     VALUES
                                           (@Region,
                                           @ItemID,
                                           @BusinessUnitID,
                                           @Discount_Case,
                                           @Discount_TM,
                                           @Restriction_Age,
                                           @Restriction_Hours,
                                           @Authorized,
                                           @Discontinued,
                                           @LocalItem,
                                           @LabelTypeDesc,
                                           @Product_Code,
                                           @RetailUnit,
                                           @Sign_Desc,
                                           @Locality,
                                           @Sign_RomanceText_Long,
                                           @Sign_RomanceText_Short,
                                           @MSRP,
                                           @AddedDate)";
            this.insertItemSql = @"INSERT INTO dbo.Items (ItemID, ScanCode, BrandHCID, PSNumber, Desc_Product, AddedDate, RetailSize, RetailUOM, FoodStampEligible, PackageUnit) VALUES (@ItemID, @ScanCode, @BrandHCID, @PSNumber, @Desc_Product, @AddedDate, @RetailSize, @RetailUOM, @FoodStampEligible, @PackageUnit) SELECT SCOPE_IDENTITY()";
            this.insertPriceSql = @"INSERT INTO gpm.Price_FL (Region, GpmID, ItemID, BusinessUnitID, StartDate, EndDate, Price, PriceType, PriceTypeAttribute, SellableUOM, CurrencyCode, Multiple, NewTagExpiration, InsertDateUtc, ModifiedDateUtc)
                                    VALUES(@Region, @GpmID, @ItemID, @BusinessUnitID, @StartDate, @EndDate, @Price, @PriceType, @PriceTypeAttribute, @SellableUOM, @CurrencyCode, @Multiple, @NewTagExpiration, @InsertDateUtc, @ModifiedDateUtc)";
        }

        private void InsertItemAttributeData()
        {
            this.db.Connection.Execute(
                insertStoreSql,
                objectFactory.Build<Locales>()
                    .With(l => l.BusinessUnitID, expectedStoreBuId)
                    .With(l => l.Region, "FL")
                    .With(l => l.StoreName, "My Test Store 1")
                    .With(l => l.StoreAbbrev, "MTS1").CreatedObject,
                this.db.Transaction);

            this.db.Connection.Execute(
                insertBrandSql,
                objectFactory.Build<HierarchyClass>()
                    .With(b => b.HierarchyClassID, expectedBrandId)
                    .With(b => b.HierarchyClassName, expectedBrandName)
                    .With(b => b.HierarchyID, 1)
                    .With(b => b.AddedDate, DateTime.UtcNow).CreatedObject,
                this.db.Transaction);

            this.db.Connection.Execute(
                insertSubTeamSql,
                objectFactory.Build<Financial_SubTeam>()
                    .With(f => f.Name, expectedSubTeamName)
                    .With(f => f.PSNumber, expectedPsNumber)
                    .With(f => f.AddedDate, DateTime.UtcNow).CreatedObject,
                this.db.Transaction);

            this.db.Connection.Execute(
                insertItemSql,
                objectFactory.Build<Item>()
                    .With(i => i.ItemID, expectedItemId)
                    .With(i => i.ScanCode, expectedItemScanCode)
                    .With(i => i.BrandHCID, expectedBrandId)
                    .With(i => i.PSNumber, expectedPsNumber)
                    .With(i => i.Desc_Product, expectedItemDescription)
                    .With(i => i.FoodStampEligible, expectedFoodStamp)
                    .With(i => i.Desc_POS, expectedPosDescription)
                    .With(i => i.PackageUnit, expectedPackageUnit)
                    .With(i => i.RetailSize, expectedRetailSize)
                    .With(i => i.RetailUOM, expectedRetailUom)
                    .CreatedObject,
                this.db.Transaction);

            this.db.Connection.Execute(
                insertItemLocaleSql,
                objectFactory.Build<ItemAttributes_Locale>()
                    .With(il => il.ItemID, expectedItemId)
                    .With(il => il.BusinessUnitID, expectedStoreBuId)
                    .With(il => il.Region, expectedRegion)
                    .With(il => il.Authorized, expectedAuthorized)
                    .With(il => il.Discount_Case, expectedDiscountCase)
                    .With(il => il.Discount_TM, expectedDiscountTm)
                    .With(il => il.Restriction_Hours, expectedRestrictedHours)
                    .With(il => il.Discontinued, expectedDiscontinued)
                    .With(il => il.LocalItem, expectedLocalItem)
                    .With(il => il.Msrp, expectedMsrp)
                    .With(il => il.AddedDate, DateTime.UtcNow)
                    .With(il => il.Sign_Desc, expectedSignDescription)
                    .CreatedObject,
                this.db.Transaction);
        }
    }
}
