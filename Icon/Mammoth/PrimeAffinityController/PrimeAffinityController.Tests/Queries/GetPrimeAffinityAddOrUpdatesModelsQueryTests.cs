using Dapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PrimeAffinityController.Queries;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;

namespace PrimeAffinityController.Tests.Queries
{
    [TestClass]
    public class GetPrimeAffinityAddOrUpdatesModelsQueryTests
    {
        private const string AddOrUpdate = "AddOrUpdate";
        private const string TestStoreName = "Test";
        private const string TestItemTypeCode = "TST";

        private GetPrimeAffinityAddPsgsFromPricesQuery query;
        private GetPrimeAffinityAddPsgsFromPricesParameters parameters;
        private SqlConnection sqlConnection;
        private TransactionScope transaction;
        private string testRegion = "FL";
        private List<string> testPriceTypes;
        private List<int> testExcludedPSNumbers;
        private int testValidPSNumber = 8888;
        private List<int> testItemIds;
        private List<int> testBusinessUnitIds;

        [TestInitialize]
        public void Initialize()
        {
            transaction = new TransactionScope();
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Mammoth"].ConnectionString);
            query = new GetPrimeAffinityAddPsgsFromPricesQuery(sqlConnection);

            testItemIds = new List<int> { 99999990, 99999991, 99999992 };
            testBusinessUnitIds = new List<int> { 77777770, 77777771, 77777772 };
            testPriceTypes = new List<string>
            {
                "TST",
                "TS2"
            };
            testExcludedPSNumbers = new List<int>
            {
                1234,
                5678
            };
            parameters = new GetPrimeAffinityAddPsgsFromPricesParameters
            {
                Region = testRegion,
                PriceTypes = testPriceTypes,
                ExcludedPSNumbers = testExcludedPSNumbers
            };

            InsertTestData();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Dispose();
        }

        [TestMethod]
        public void GetPrimeAffinityAddPsgsFromPrices_SalesAreStartingToday_ShouldReturnSales()
        {
            //Given
            List<dynamic> prices = new List<dynamic>();
            foreach (var itemId in testItemIds)
            {
                foreach (var businessUnitId in testBusinessUnitIds)
                {
                    prices.Add(InsertTestPrices(itemId, businessUnitId, DateTime.Today, DateTime.Today.AddDays(10), testPriceTypes[0]));
                }
            }

            //When
            var results = query.Search(parameters);

            //Then
            Assert.AreEqual(9, results.Count());
            foreach (var result in results)
            {
                var price = prices.First(p => p.PriceID == result.PriceID);
                Assert.AreEqual(AddOrUpdate, result.MessageAction);
                Assert.AreEqual(price.BusinessUnitID, result.BusinessUnitID);
                Assert.AreEqual(price.ItemID, result.ItemID);
                Assert.AreEqual("sc" + result.ItemID, result.ScanCode);
                Assert.AreEqual(TestStoreName, result.StoreName);
                Assert.AreEqual(TestItemTypeCode, result.ItemTypeCode);
            }
        }

        [TestMethod]
        public void GetPrimeAffinityAddPsgsFromPrices_NoSalesStartingToday_ShouldReturnNoSales()
        {
            //Given
            List<dynamic> prices = new List<dynamic>();
            foreach (var itemId in testItemIds)
            {
                foreach (var businessUnitId in testBusinessUnitIds)
                {
                    prices.Add(InsertTestPrices(itemId, businessUnitId, DateTime.Today.AddDays(5), DateTime.Today.AddDays(10), testPriceTypes[0]));
                }
            }

            //When
            var results = query.Search(parameters);

            //Then
            Assert.AreEqual(0, results.Count());
        }

        [TestMethod]
        public void GetPrimeAffinityAddPsgsFromPrices_SalesHaveDifferentPriceTypeThanParameters_ShouldOnlyReturnSalesWithParametersPriceTypes()
        {
            //Given
            List<dynamic> prices = new List<dynamic>();
            prices.Add(InsertTestPrices(testItemIds[0], testBusinessUnitIds[0], DateTime.Today, DateTime.Today.AddDays(10), testPriceTypes[0]));
            prices.Add(InsertTestPrices(testItemIds[0], testBusinessUnitIds[0], DateTime.Today, DateTime.Today.AddDays(10), testPriceTypes[1]));
            prices.Add(InsertTestPrices(testItemIds[0], testBusinessUnitIds[0], DateTime.Today, DateTime.Today.AddDays(10), "TS3"));
            prices.Add(InsertTestPrices(testItemIds[0], testBusinessUnitIds[0], DateTime.Today, DateTime.Today.AddDays(10), "TS4"));

            //When
            var results = query.Search(parameters);

            //Then
            Assert.AreEqual(2, results.Count());
            foreach (var result in results)
            {
                var price = prices.First(p => p.PriceID == result.PriceID);
                Assert.AreEqual(AddOrUpdate, result.MessageAction);
                Assert.AreEqual(price.BusinessUnitID, result.BusinessUnitID);
                Assert.AreEqual(price.ItemID, result.ItemID);
                Assert.AreEqual("sc" + result.ItemID, result.ScanCode);
                Assert.AreEqual(TestStoreName, result.StoreName);
                Assert.AreEqual(TestItemTypeCode, result.ItemTypeCode);
            }
        }

        [TestMethod]
        public void GetPrimeAffinityAddPsgsFromPrices_SalesAssociatedToItemsThatHaveExcludedPSNumbers_ShouldOnlyReturnSalesNotInExcludedPSNumbers()
        {
            //Given
            var invalidItemId = 1234567;
            var invalidItemId2 = 12345678;
            InsertTestItem(invalidItemId, testExcludedPSNumbers[0]);
            InsertTestItem(invalidItemId2, testExcludedPSNumbers[1]);

            List<dynamic> prices = new List<dynamic>();
            prices.Add(InsertTestPrices(testItemIds[0], testBusinessUnitIds[0], DateTime.Today, DateTime.Today.AddDays(10), testPriceTypes[0]));
            prices.Add(InsertTestPrices(testItemIds[0], testBusinessUnitIds[0], DateTime.Today, DateTime.Today.AddDays(10), testPriceTypes[1]));
            prices.Add(InsertTestPrices(invalidItemId, testBusinessUnitIds[0], DateTime.Today, DateTime.Today.AddDays(10), testPriceTypes[0]));
            prices.Add(InsertTestPrices(invalidItemId2, testBusinessUnitIds[0], DateTime.Today, DateTime.Today.AddDays(10), testPriceTypes[0]));

            //When
            var results = query.Search(parameters);

            //Then
            Assert.AreEqual(2, results.Count());
            Assert.IsFalse(results.Any(p => p.ItemID == invalidItemId && p.ItemID == invalidItemId2));
            foreach (var result in results)
            {
                var price = prices.First(p => p.PriceID == result.PriceID);
                Assert.AreEqual(AddOrUpdate, result.MessageAction);
                Assert.AreEqual(price.BusinessUnitID, result.BusinessUnitID);
                Assert.AreEqual(price.ItemID, result.ItemID);
                Assert.AreEqual("sc" + result.ItemID, result.ScanCode);
                Assert.AreEqual(TestStoreName, result.StoreName);
                Assert.AreEqual(TestItemTypeCode, result.ItemTypeCode);
            }
        }

        private void InsertTestData()
        {
            InsertTestItemType();
            foreach (var itemId in testItemIds)
            {
                InsertTestItem(itemId, testValidPSNumber);
            }
            InsertTestLocale();
        }

        private void InsertTestItemType()
        {
            sqlConnection.Execute(
                @"  INSERT INTO dbo.ItemTypes(itemTypeCode) 
                    VALUES (@ItemTypeCode)",
                new { ItemTypeCode = TestItemTypeCode });
        }

        private void InsertTestItem(int itemId, int pSNumber)
        {
                sqlConnection.Execute(
                    $@" INSERT INTO dbo.Items(
                            ItemID, 
                            ScanCode, 
                            PSNumber, 
                            ItemTypeID)
                        SELECT @ItemId, 
                            @ScanCode, 
                            @PSNumber, 
                            ItemTypeID 
                            FROM dbo.ItemTypes 
                        WHERE itemTypeCode = @ItemTypeCode",
                    new { ItemId = itemId, ScanCode = "sc" + itemId, PSNumber = pSNumber, ItemTypeCode = TestItemTypeCode });
        }

        private void InsertTestLocale()
        {
            foreach (var businessUnitId in testBusinessUnitIds)
            {
                sqlConnection.Execute(
                $@" INSERT INTO [dbo].[Locales_{testRegion}](
	                    [Region],
	                    [BusinessUnitID],
	                    [StoreName],
	                    [StoreAbbrev])
                    VALUES (@Region, 
                            @BusinessUnitId, 
                            @StoreName, 
                            'TST')",
                new { Region = testRegion, BusinessUnitId = businessUnitId, StoreName = TestStoreName });
            }
        }

        private dynamic InsertTestPrices(int itemId, int businessUnitId, DateTime startDate, DateTime? endDate, string priceType)
        {
            var priceId = sqlConnection.QueryFirst<int>(
                $@" INSERT INTO [dbo].[Price_{testRegion}]
                            ([Region]
                            ,[ItemID]
                            ,[BusinessUnitID]
                            ,[StartDate]
                            ,[EndDate]
                            ,[Price]
                            ,[PriceType]
                            ,[PriceUOM]
                            ,[CurrencyID]
                            ,[Multiple]
                            ,[AddedDate])
                        VALUES
                            (@Region
                            ,@ItemId
                            ,@BusinessUnitId
                            ,@StartDate
                            ,@EndDate
                            ,1.99
                            ,@PriceType
                            ,'EA'
                            ,1
                            ,1
                            ,GETDATE())

                        SELECT SCOPE_IDENTITY()",
                new
                {
                    Region = testRegion,
                    ItemId = itemId,
                    BusinessUnitId = businessUnitId,
                    StartDate = startDate,
                    EndDate = endDate,
                    PriceType = priceType
                });

            return sqlConnection.QueryFirst(
                $@" SELECT * 
                    FROM dbo.Price_{testRegion} 
                    WHERE PriceID = @PriceId",
                new { PriceId = priceId });
        }
    }
}
