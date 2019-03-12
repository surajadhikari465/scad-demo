using Dapper;
using Mammoth.Common.DataAccess.DbProviders;
using MammothWebApi.DataAccess.Models;
using MammothWebApi.DataAccess.Queries;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
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
        public void GetItemPriceAttributesByStoreAndScanCodeQuery_ValidStoreAndItem_ReturnsPrices()
        {
            // Given
            DateTime expectedStartDate = DateTime.UtcNow.Date.AddDays(-3);
            InsertGpmPriceObject(expectedRegion, expectedItemId, expectedStoreBuId,
                3.50m, "REG", "REG", expectedStartDate, null);

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
            Assert.IsNull(price.PercentOff);
        }

        [TestMethod]
        public void GetItemPriceAttributesByStoreAndScanCodeQuery_ExpiredRegularPriceExists_ReturnsOnlyActiveRegularPrice()
        {
            // Given
            InsertGpmPriceObject(expectedRegion, expectedItemId, expectedStoreBuId,
                3.50m, "REG", "REG", DateTime.UtcNow.Date.AddDays(-3), null);
            // another REG starting earlier than the first price
            InsertGpmPriceObject(expectedRegion, expectedItemId, expectedStoreBuId,
                3.50m, "REG", "REG", DateTime.UtcNow.Date.AddDays(-20), null);

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
        public void GetItemPriceAttributesByStoreAndScanCodeQuery_ExpiredSalePriceExists_ReturnsOnlyActiveSalePrice()
        {
            // Given
            DateTime expectedRegularStartDate = DateTime.UtcNow.Date.AddDays(-3);
            DateTime expectedTprStartDate = DateTime.UtcNow.Date.AddDays(-2);
            DateTime expectedTprEndDate = DateTime.UtcNow.Date.AddDays(15);

            InsertGpmPriceObject(expectedRegion, expectedItemId, expectedStoreBuId,
                3.50m, "REG", "REG", expectedRegularStartDate, null);
            InsertGpmPriceObject(expectedRegion, expectedItemId, expectedStoreBuId,
                1.50m, "TPR", "MSAL", expectedTprStartDate, expectedTprEndDate);
            // EndDate is before today
            InsertGpmPriceObject(expectedRegion, expectedItemId, expectedStoreBuId,
                2.50m, "TPR", "MSAL", DateTime.UtcNow.Date.AddDays(-8), DateTime.UtcNow.Date.AddDays(-4));

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
        public void GetItemPriceAttributesByStoreAndScanCodeQuery_SaleAndRewardPriceActive_ReturnsRegularAndSaleAndRewardPrice()
        {
            // Given
            DateTime expectedRegularStartDate = DateTime.UtcNow.Date.AddDays(-3);
            DateTime expectedTprStartDate = DateTime.UtcNow.Date.AddDays(-2);
            DateTime expectedTprEndDate = DateTime.UtcNow.Date.AddDays(15);
            DateTime expectedRewardStartDate = DateTime.UtcNow.Date.AddDays(-2);
            DateTime expectedRewardEndDate = DateTime.UtcNow.Date.AddDays(15);

            InsertGpmPriceObject(expectedRegion, expectedItemId, expectedStoreBuId,
                3.50m, "REG", "REG", expectedRegularStartDate);
            InsertGpmPriceObject(expectedRegion, expectedItemId, expectedStoreBuId,
                1.50m, "TPR", "MSAL", expectedTprStartDate, expectedTprEndDate);
            // EndDate is before today
            InsertGpmPriceObject(expectedRegion, expectedItemId, expectedStoreBuId,
                2.50m, "TPR", "MSAL", DateTime.UtcNow.Date.AddDays(-8), DateTime.UtcNow.Date.AddDays(-4));
            InsertGpmPriceObject(expectedRegion, expectedItemId, expectedStoreBuId,
                2.50m, "RWD", "PRM", expectedRewardStartDate, expectedRewardEndDate);

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
        public void GetItemPriceAttributesByStoreAndScanCodeQuery_RewardsPriceHasPercentOffPopulated_ReturnsPercentOffAsPartOfRewardsPrices()
        {
            // Given
            decimal expectedPercentOff = 5.55m;
            DateTime expectedRegularStartDate = DateTime.UtcNow.Date.AddDays(-2);
            DateTime expectedRewardStartDate = DateTime.UtcNow.Date.AddDays(-2);
            DateTime expectedRewardEndDate = DateTime.UtcNow.Date.AddDays(15);

            InsertGpmPriceObject(expectedRegion, expectedItemId, expectedStoreBuId,
                3.50m, "REG", "REG", expectedRegularStartDate);
            InsertGpmPriceObject(expectedRegion, expectedItemId, expectedStoreBuId,
                2.50m, "RWD", "PRM", expectedRewardStartDate, expectedRewardEndDate, expectedPercentOff);

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
            // Expect only one REG and one RWD price
            Assert.AreEqual(2, prices.Count());
            var rewardPrice = prices.Single(p => p.PriceType == "RWD");
            Assert.AreEqual("RWD", rewardPrice.PriceType);
            Assert.AreEqual(expectedRewardStartDate, rewardPrice.StartDate);
            Assert.AreEqual(expectedRewardEndDate, rewardPrice.EndDate);
            Assert.AreEqual(expectedPercentOff, rewardPrice.PercentOff);
        }

        [TestMethod]
        public void GetItemPriceAttributesByStoreAndScanCodeQuery_FuturePricesExistsAndIncludeFuturePricesIsTrue_ReturnsCurrentAndFuturePrice()
        {
            // Given
            DateTime expectedRegularStartDate = DateTime.UtcNow.Date.AddDays(-3);
            DateTime expectedTprStartDate = DateTime.UtcNow.Date.AddDays(-2);
            DateTime expectedTprEndDate = DateTime.UtcNow.Date.AddDays(15);
            DateTime expectedFutureRegStartDate = DateTime.UtcNow.Date.AddDays(30);
            DateTime expectedFutureTprStartDate = DateTime.UtcNow.Date.AddDays(20);
            DateTime expectedFutureTprEndDate = DateTime.UtcNow.Date.AddDays(40);

            InsertGpmPriceObject(expectedRegion, expectedItemId, expectedStoreBuId,
                3.50m, "REG", "REG", expectedRegularStartDate, null);
            InsertGpmPriceObject(expectedRegion, expectedItemId, expectedStoreBuId,
                1.50m, "TPR", "MSAL", expectedTprStartDate, expectedTprEndDate);
            // EndDate is before today
            InsertGpmPriceObject(expectedRegion, expectedItemId, expectedStoreBuId,
                2.50m, "TPR", "MSAL", DateTime.UtcNow.Date.AddDays(-8), DateTime.UtcNow.Date.AddDays(-4));
            // future REG
            InsertGpmPriceObject(expectedRegion, expectedItemId, expectedStoreBuId,
                4.49m, "REG", "REG", expectedFutureRegStartDate, null);
            // future TPR
            InsertGpmPriceObject(expectedRegion, expectedItemId, expectedStoreBuId,
                4.49m, "TPR", "SSAL", expectedFutureTprStartDate, expectedFutureTprEndDate);

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
        public void GetItemPriceAttributesByStoreAndScanCodeQuery_FuturePricesExistsAndIncludeFuturePricesIsFalse_ReturnsOnlyCurrentPrices()
        {
            // Given
            DateTime expectedRegularStartDate = DateTime.UtcNow.Date.AddDays(-3);
            DateTime expectedTprStartDate = DateTime.UtcNow.Date.AddDays(-2);
            DateTime expectedTprEndDate = DateTime.UtcNow.Date.AddDays(15);
            DateTime expectedFutureRegStartDate = DateTime.UtcNow.Date.AddDays(30);
            DateTime expectedFutureTprStartDate = DateTime.UtcNow.Date.AddDays(20);
            DateTime expectedFutureTprEndDate = DateTime.UtcNow.Date.AddDays(40);

            InsertGpmPriceObject(expectedRegion, expectedItemId, expectedStoreBuId,
                4.50m, "REG", "REG", expectedRegularStartDate, null);
            InsertGpmPriceObject(expectedRegion, expectedItemId, expectedStoreBuId,
                1.50m, "TPR", "MSAL", expectedTprStartDate, expectedTprEndDate);
            // EndDate is before today
            InsertGpmPriceObject(expectedRegion, expectedItemId, expectedStoreBuId,
                2.50m, "TPR", "MSAL", DateTime.UtcNow.Date.AddDays(-8), DateTime.UtcNow.Date.AddDays(-4));
            // future REG
            InsertGpmPriceObject(expectedRegion, expectedItemId, expectedStoreBuId,
                4.49m, "REG", "REG", expectedFutureRegStartDate, null);
            // future TPR
            InsertGpmPriceObject(expectedRegion, expectedItemId, expectedStoreBuId,
                4.49m, "TPR", "SSAL", expectedFutureTprStartDate, expectedFutureTprEndDate);

            this.query = new GetItemPriceAttributesByStoreAndScanCodeQuery
            {
                Region = expectedRegion,
                EffectiveDate = expectedEffectiveDate,
                IncludeFuturePrices = false,
                StoreScanCodeCollection = new List<StoreScanCode>
                {
                    new StoreScanCode {BusinessUnitID = expectedStoreBuId, ScanCode = expectedItemScanCode}
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
        public void GetItemDifferentPriceAttributesByStoreAndScanCodeQuery_FutureExpectedPricesAndIncludeFuturePricesIsFalse_ReturnsCurrentPricesOnly()
        {
            // Given
            DateTime expectedFutureRegStartDate = DateTime.UtcNow.Date.AddDays(30);
            DateTime expectedFutureTprStartDate = DateTime.UtcNow.Date.AddDays(20);
            DateTime expectedFutureTprEndDate = DateTime.UtcNow.Date.AddDays(40);
            DateTime expectedRegularStartDate = DateTime.UtcNow.Date.AddDays(-5);
            DateTime expectedTprStartDate = DateTime.UtcNow.Date.AddDays(-1);
            DateTime expectedTprEndDate = DateTime.UtcNow.Date.AddDays(10);

            // future REG
            InsertGpmPriceObject(expectedRegion, expectedItemId, expectedStoreBuId,
                7.50m, "REG", "REG", expectedFutureRegStartDate, null);
            // future TPR
            InsertGpmPriceObject(expectedRegion, expectedItemId, expectedStoreBuId,
                4.49m, "TPR", "SSAL", expectedFutureTprStartDate, expectedFutureTprEndDate);
            // expected REG
            InsertGpmPriceObject(expectedRegion, expectedItemId, expectedStoreBuId,
                2.50m, "REG", "REG", expectedRegularStartDate, null);
            InsertGpmPriceObject(expectedRegion, expectedItemId, expectedStoreBuId,
                4.50m, "REG", "REG", expectedRegularStartDate.AddDays(-2), null);
            // expected TRP
            InsertGpmPriceObject(expectedRegion, expectedItemId, expectedStoreBuId,
                2.50m, "TPR", "MSAL", expectedTprStartDate, expectedTprEndDate);
            InsertGpmPriceObject(expectedRegion, expectedItemId, expectedStoreBuId,
                2.50m, "TPR", "MSAL", DateTime.UtcNow.Date.AddDays(-8), DateTime.UtcNow.Date.AddDays(-4));

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
        public void GetItemPriceAttributesByStoreAndScanCodeQuery_NoPricesExists_ReturnsEmptyCollection()
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
        public void GetItemPriceAttributesByStoreAndScanCodeQuery_EffectiveDateIsInTheFuture_ReturnsPricesActiveDuringEffectiveDate()
        {
            // Given
            DateTime effectiveDate = DateTime.Today.AddDays(10);
            DateTime activeRegularStartDate = DateTime.Today.Date.AddDays(-3);
            DateTime nonActiveTprStartDate = DateTime.Today.Date.AddDays(-2);
            DateTime nonActiveTprEndDate = DateTime.Today.Date.AddDays(2); // still active as of today but not according to effective date
            DateTime expiredRegStartDate = activeRegularStartDate.AddDays(-10);
            DateTime effectiveDateTprStartDate = effectiveDate.AddDays(-2); // active according to effective date
            DateTime effectiveDateTprEndDate = effectiveDate.AddDays(5); // active according to effective date

            InsertGpmPriceObject(expectedRegion, expectedItemId, expectedStoreBuId,
                3.50M, "REG", "REG", activeRegularStartDate);
            // non active tpr according to effective date - is not expected to be returned
            InsertGpmPriceObject(expectedRegion, expectedItemId, expectedStoreBuId,
                1.50M, "TPR", "SSAL", nonActiveTprStartDate, nonActiveTprEndDate);
            // effective date tpr
            InsertGpmPriceObject(expectedRegion, expectedItemId, expectedStoreBuId,
                2.50M, "TPR", "MSAL", effectiveDateTprStartDate, effectiveDateTprEndDate);
            InsertGpmPriceObject(expectedRegion, expectedItemId, expectedStoreBuId,
                4.49m, "REG", "REG", expiredRegStartDate, null);

            this.query = new GetItemPriceAttributesByStoreAndScanCodeQuery
            {
                Region = expectedRegion,
                EffectiveDate = effectiveDate,
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
            Assert.AreEqual(activeRegularStartDate, prices.First(p => p.PriceType == "REG").StartDate);
            Assert.AreEqual(effectiveDateTprStartDate, prices.First(p => p.PriceType != "REG").StartDate);
            Assert.AreEqual(effectiveDateTprEndDate, prices.First(p => p.PriceType != "REG").EndDate);
        }

        [TestMethod]
        public void GetItemPriceAttributesByStoreAndScanCodeQuery_EffectiveDateInTheFuture_ReturnsFutureRegularPriceInsteadOfCurrentRegularPrice()
        {
            // Given
            DateTime expectedRegularStartDate = DateTime.UtcNow.Date.AddDays(-3);
            DateTime expectedFutureRegStartDate = DateTime.UtcNow.Date.AddDays(5);

            InsertGpmPriceObject(expectedRegion, expectedItemId, expectedStoreBuId,
                3.50m, "REG", "REG", expectedRegularStartDate);
            InsertGpmPriceObject(expectedRegion, expectedItemId, expectedStoreBuId,
                2.99m, "REG", "REG", expectedFutureRegStartDate);

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

        [TestMethod]
        public void GetItemPriceAttributesByStoreAndScanCodeQuery_PriceTypeParameterReg_OnlyReturnsRegPrice()
        {
            // Given 
            DateTime expectedTprStartDate = DateTime.UtcNow.Date.AddDays(-2);
            DateTime expectedTprEndDate = DateTime.UtcNow.Date.AddDays(15);
            DateTime expectedRegStartDate = DateTime.UtcNow.Date.AddDays(-3);

            InsertGpmPriceObject(expectedRegion, expectedItemId, expectedStoreBuId,
                3.50m, "REG", "REG", expectedRegStartDate, null);
            InsertGpmPriceObject(expectedRegion, expectedItemId, expectedStoreBuId,
                1.50m, "TPR", "MSAL", expectedTprStartDate, expectedTprEndDate);

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
                },
                PriceType = "REG"
            };

            // When
            var prices = this.queryHandler.Search(this.query);

            // Then
            var price = prices.Single(p => p.ScanCode == expectedItemScanCode && p.BusinessUnitID == expectedStoreBuId);
            Assert.AreEqual("REG", price.PriceType);
            Assert.AreEqual(true, price.Authorized);
        }

        [TestMethod]
        public void GetItemPriceAttributesByStoreAndScanCodeQuery_PriceTypeParameterTpr_OnlyReturnsTprPrice()
        {
            // Given 
            DateTime expectedTprStartDate = DateTime.UtcNow.Date.AddDays(-2);
            DateTime expectedTprEndDate = DateTime.UtcNow.Date.AddDays(15);
            DateTime expectedRegStartDate = DateTime.UtcNow.Date.AddDays(-3);

            InsertGpmPriceObject(expectedRegion, expectedItemId, expectedStoreBuId,
                3.50m, "REG", "REG", expectedRegStartDate, null);
            InsertGpmPriceObject(expectedRegion, expectedItemId, expectedStoreBuId,
                1.50m, "TPR", "MSAL", expectedTprStartDate, expectedTprEndDate);

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
                },
                PriceType = "TPR"
            };

            // When
            var prices = this.queryHandler.Search(this.query);

            // Then
            var price = prices.Single(p => p.ScanCode == expectedItemScanCode && p.BusinessUnitID == expectedStoreBuId);
            Assert.AreEqual("TPR", price.PriceType);
            Assert.AreEqual(true, price.Authorized);
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
            this.insertPriceSql = @"INSERT INTO gpm.Price_FL (Region, GpmID, ItemID, BusinessUnitID, StartDate, EndDate, Price, PriceType, PriceTypeAttribute, SellableUOM, CurrencyCode, Multiple, TagExpirationDate, PercentOff, InsertDateUtc, ModifiedDateUtc)
                                    VALUES(@Region, @GpmID, @ItemID, @BusinessUnitID, @StartDate, @EndDate, @Price, @PriceType, @PriceTypeAttribute, @SellableUOM, @CurrencyCode, @Multiple, @TagExpirationDate, @PercentOff, @InsertDateUtc, @ModifiedDateUtc)";
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

        private void InsertGpmPriceObject(IDbConnection dbConnection, string insertSql, IDbTransaction transaction,
            string region, int itemID, int businessUnit,
            decimal price, string priceType, string priceTypeAttribute, DateTime startDate, DateTime? endDate = null,
            decimal? percentOff = null, string currencyCode = "USD", string sellableUOM = "EA")
        {
            dbConnection.Execute(
                insertSql,
                BuildGpmPriceObject(region, itemID, businessUnit, price, priceType, priceTypeAttribute,
                    startDate, endDate, percentOff, currencyCode, sellableUOM),
                transaction);
        }

        private void InsertGpmPriceObject(string region, int itemID, int businessUnit,
            decimal price, string priceType, string priceTypeAttribute, DateTime startDate, DateTime? endDate = null,
            decimal? percentOff = null, string currencyCode = "USD", string sellableUOM = "EA")
        {
            InsertGpmPriceObject(this.db.Connection, this.insertPriceSql, this.db.Transaction,
                region, itemID, businessUnit, price, priceType, priceTypeAttribute, startDate, endDate,
                percentOff, currencyCode, sellableUOM);
        }

        private PricesGpm BuildGpmPriceObject(string region, int itemID, int businessUnit,
            decimal price, string priceType, string priceTypeAttributem, DateTime startDate, DateTime? endDate = null,
            decimal? percentOff = null, string currencyCode = "USD", string sellableUOM = "EA")
        {
            var gpmID = Guid.NewGuid();
            var insertDateUtc = DateTime.UtcNow;

            return objectFactory.Build<PricesGpm>()
                     .With(p => p.Region, region)
                     .With(p => p.ItemID, itemID)
                     .With(p => p.BusinessUnitID, businessUnit)
                     .With(p => p.Price, price)
                     .With(p => p.PriceType, priceType)
                     .With(p => p.StartDate, startDate)
                     .With(p => p.EndDate, endDate)
                     .With(p => p.CurrencyCode, currencyCode)
                     .With(p => p.GpmID, gpmID)
                     .With(p => p.SellableUOM, sellableUOM)
                     .With(p => p.InsertDateUtc, insertDateUtc)
                     .With(p => p.PriceTypeAttribute, priceTypeAttributem)
                     .With(p => p.PercentOff, percentOff)
                .CreatedObject;
        }
    }
}
