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
using System.Transactions;

namespace MammothWebApi.Tests.DataAccess.Queries
{
    [TestClass]
    public class GetActivePricesByScanCodeAndStoreQueryHandlerTests
    {
        private const string TestStoreName = "Test";
        private const string TestItemTypeCode = "TST";
        private const string PriceTypeReg = "REG";
        private const string PriceTypeSal = "SAL";

        private GetActivePricesByScanCodeAndStoreQueryHandler queryHandler;
        private GetActivePricesByScanCodeAndStoreQuery query;
        private SqlDbProvider dbProvider;
        private TransactionScope transaction;
        private string testRegion = "FL";
        private List<string> testPriceTypes;
        private int testValidPSNumber = 8888;
        private List<int> testItemIds;
        private List<int> testBusinessUnitIds;

        [TestInitialize]
        public void Initialize()
        {
            transaction = new TransactionScope();
            dbProvider = new SqlDbProvider { Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Mammoth"].ConnectionString) };
            queryHandler = new GetActivePricesByScanCodeAndStoreQueryHandler(dbProvider);
            query = new GetActivePricesByScanCodeAndStoreQuery();

            testItemIds = new List<int> { 99999990, 99999991, 99999992 };
            testBusinessUnitIds = new List<int> { 77777770, 77777771, 77777772 };
            testPriceTypes = new List<string>
            {
                "TST",
                "TS2"
            };
            InsertTestData();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Dispose();
        }

        [TestMethod]
        public void GetActivePricesByScanCodeAndStore_ActiveRegsExist_ReturnsActiveRegs()
        {
            //Given
            var price1 = InsertTestPrices(testItemIds[0], testBusinessUnitIds[0], DateTime.Today, null, PriceTypeReg);
            var price2 = InsertTestPrices(testItemIds[0], testBusinessUnitIds[0], DateTime.Today.AddDays(-1), null, PriceTypeReg);
            var price3 = InsertTestPrices(testItemIds[0], testBusinessUnitIds[0], DateTime.Today.AddDays(-5), null, PriceTypeReg);
            var price4 = InsertTestPrices(testItemIds[0], testBusinessUnitIds[0], DateTime.Today.AddDays(-10), null, PriceTypeReg);
            var price5 = InsertTestPrices(testItemIds[1], testBusinessUnitIds[0], DateTime.Today, null, PriceTypeReg);
            var price6 = InsertTestPrices(testItemIds[1], testBusinessUnitIds[0], DateTime.Today.AddDays(-1), null, PriceTypeReg);
            var price7 = InsertTestPrices(testItemIds[1], testBusinessUnitIds[0], DateTime.Today.AddDays(-5), null, PriceTypeReg);
            var price8 = InsertTestPrices(testItemIds[1], testBusinessUnitIds[0], DateTime.Today.AddDays(-10), null, PriceTypeReg);

            //When
            var results = queryHandler.Search(new GetActivePricesByScanCodeAndStoreQuery
            {
                Region = testRegion,
                StoreScanCodes = new List<StoreScanCode>
                {
                    new StoreScanCode { BusinessUnitID = testBusinessUnitIds[0], ScanCode = "sc" + testItemIds[0]},
                    new StoreScanCode { BusinessUnitID = testBusinessUnitIds[0], ScanCode = "sc" + testItemIds[1]}
                }
            });

            //Then
            Assert.AreEqual(8, results.Count);
            Assert.AreEqual(4, results.Count(p => p.ScanCode == "sc" + testItemIds[0]));
            Assert.AreEqual(4, results.Count(p => p.ScanCode == "sc" + testItemIds[1]));
            Assert.IsTrue(results.All(p => p.BusinessUnitId == testBusinessUnitIds[0]
                && p.PriceType == PriceTypeReg
                && p.EndDate == null));
            Assert.AreEqual(2, results.Count(p => p.StartDate == DateTime.Today));
            Assert.AreEqual(2, results.Count(p => p.StartDate == DateTime.Today.AddDays(-1)));
            Assert.AreEqual(2, results.Count(p => p.StartDate == DateTime.Today.AddDays(-5)));
            Assert.AreEqual(2, results.Count(p => p.StartDate == DateTime.Today.AddDays(-10)));

        }

        [TestMethod]
        public void GetActivePricesByScanCodeAndStore_ActiveTprsExist_ReturnsActiveTprs()
        {
            //Given
            var price1 = InsertTestPrices(testItemIds[0], testBusinessUnitIds[0], DateTime.Today, DateTime.Today.AddDays(1), PriceTypeSal);
            var price2 = InsertTestPrices(testItemIds[0], testBusinessUnitIds[0], DateTime.Today.AddDays(-1), DateTime.Today.AddDays(2), PriceTypeSal);
            var price3 = InsertTestPrices(testItemIds[0], testBusinessUnitIds[0], DateTime.Today.AddDays(-5), DateTime.Today.AddDays(5), PriceTypeSal);
            var price4 = InsertTestPrices(testItemIds[0], testBusinessUnitIds[0], DateTime.Today.AddDays(-10), DateTime.Today.AddDays(10), PriceTypeSal);
            var price5 = InsertTestPrices(testItemIds[1], testBusinessUnitIds[0], DateTime.Today, DateTime.Today.AddDays(1), PriceTypeSal);
            var price6 = InsertTestPrices(testItemIds[1], testBusinessUnitIds[0], DateTime.Today.AddDays(-1), DateTime.Today.AddDays(2), PriceTypeSal);
            var price7 = InsertTestPrices(testItemIds[1], testBusinessUnitIds[0], DateTime.Today.AddDays(-5), DateTime.Today.AddDays(5), PriceTypeSal);
            var price8 = InsertTestPrices(testItemIds[1], testBusinessUnitIds[0], DateTime.Today.AddDays(-10), DateTime.Today.AddDays(10), PriceTypeSal);

            //When
            var results = queryHandler.Search(new GetActivePricesByScanCodeAndStoreQuery
            {
                Region = testRegion,
                StoreScanCodes = new List<StoreScanCode>
                {
                    new StoreScanCode { BusinessUnitID = testBusinessUnitIds[0], ScanCode = "sc" + testItemIds[0]},
                    new StoreScanCode { BusinessUnitID = testBusinessUnitIds[0], ScanCode = "sc" + testItemIds[1]}
                }
            });

            //Then
            Assert.AreEqual(8, results.Count);
            Assert.AreEqual(4, results.Count(p => p.ScanCode == "sc" + testItemIds[0]));
            Assert.AreEqual(4, results.Count(p => p.ScanCode == "sc" + testItemIds[1]));
            Assert.IsTrue(results.All(p => p.BusinessUnitId == testBusinessUnitIds[0]
                && p.PriceType == PriceTypeSal
                && p.EndDate != null));
            Assert.AreEqual(2, results.Count(p => p.StartDate == DateTime.Today && p.EndDate == DateTime.Today.AddDays(1)));
            Assert.AreEqual(2, results.Count(p => p.StartDate == DateTime.Today.AddDays(-1) && p.EndDate == DateTime.Today.AddDays(2)));
            Assert.AreEqual(2, results.Count(p => p.StartDate == DateTime.Today.AddDays(-5) && p.EndDate == DateTime.Today.AddDays(5)));
            Assert.AreEqual(2, results.Count(p => p.StartDate == DateTime.Today.AddDays(-10) && p.EndDate == DateTime.Today.AddDays(10)));
        }

        [TestMethod]
        public void GetActivePricesByScanCodeAndStore_ActiveRegsAndTprsExist_ReturnsActiveRegsAndTprs()
        {
            //Given
            var price1 = InsertTestPrices(testItemIds[0], testBusinessUnitIds[0], DateTime.Today, DateTime.Today.AddDays(1), PriceTypeSal);
            var price2 = InsertTestPrices(testItemIds[0], testBusinessUnitIds[0], DateTime.Today.AddDays(-1), DateTime.Today.AddDays(2), PriceTypeSal);
            var price3 = InsertTestPrices(testItemIds[0], testBusinessUnitIds[0], DateTime.Today.AddDays(-5), DateTime.Today.AddDays(5), PriceTypeSal);
            var price4 = InsertTestPrices(testItemIds[0], testBusinessUnitIds[0], DateTime.Today.AddDays(-10), DateTime.Today.AddDays(10), PriceTypeSal);
            var price5 = InsertTestPrices(testItemIds[1], testBusinessUnitIds[0], DateTime.Today, DateTime.Today.AddDays(1), PriceTypeSal);
            var price6 = InsertTestPrices(testItemIds[1], testBusinessUnitIds[0], DateTime.Today.AddDays(-1), DateTime.Today.AddDays(2), PriceTypeSal);
            var price7 = InsertTestPrices(testItemIds[1], testBusinessUnitIds[0], DateTime.Today.AddDays(-5), DateTime.Today.AddDays(5), PriceTypeSal);
            var price8 = InsertTestPrices(testItemIds[1], testBusinessUnitIds[0], DateTime.Today.AddDays(-10), DateTime.Today.AddDays(10), PriceTypeSal);
            var price9 = InsertTestPrices(testItemIds[0], testBusinessUnitIds[0], DateTime.Today, null, PriceTypeReg);
            var price10 = InsertTestPrices(testItemIds[0], testBusinessUnitIds[0], DateTime.Today.AddDays(-1), null, PriceTypeReg);
            var price11 = InsertTestPrices(testItemIds[0], testBusinessUnitIds[0], DateTime.Today.AddDays(-5), null, PriceTypeReg);
            var price12 = InsertTestPrices(testItemIds[0], testBusinessUnitIds[0], DateTime.Today.AddDays(-10), null, PriceTypeReg);

            //When
            var results = queryHandler.Search(new GetActivePricesByScanCodeAndStoreQuery
            {
                Region = testRegion,
                StoreScanCodes = new List<StoreScanCode>
                {
                    new StoreScanCode { BusinessUnitID = testBusinessUnitIds[0], ScanCode = "sc" + testItemIds[0]},
                    new StoreScanCode { BusinessUnitID = testBusinessUnitIds[0], ScanCode = "sc" + testItemIds[1]}
                }
            });

            //Then
            Assert.AreEqual(12, results.Count);
            Assert.AreEqual(4, results.Count(p => p.ScanCode == "sc" + testItemIds[0] && p.PriceType == PriceTypeReg));
            Assert.AreEqual(4, results.Count(p => p.ScanCode == "sc" + testItemIds[0] && p.PriceType == PriceTypeSal));
            Assert.AreEqual(4, results.Count(p => p.ScanCode == "sc" + testItemIds[1]));
            Assert.IsTrue(results.Where(p => p.PriceType == PriceTypeSal)
                .All(p => p.BusinessUnitId == testBusinessUnitIds[0]
                    && p.EndDate != null));
            Assert.AreEqual(2, results.Count(p => p.StartDate == DateTime.Today && p.EndDate == DateTime.Today.AddDays(1)));
            Assert.AreEqual(2, results.Count(p => p.StartDate == DateTime.Today.AddDays(-1) && p.EndDate == DateTime.Today.AddDays(2)));
            Assert.AreEqual(2, results.Count(p => p.StartDate == DateTime.Today.AddDays(-5) && p.EndDate == DateTime.Today.AddDays(5)));
            Assert.AreEqual(2, results.Count(p => p.StartDate == DateTime.Today.AddDays(-10) && p.EndDate == DateTime.Today.AddDays(10)));

            Assert.IsTrue(results.Where(p => p.PriceType == PriceTypeReg)
                .All(p => p.BusinessUnitId == testBusinessUnitIds[0]
                    && p.PriceType == PriceTypeReg
                    && p.EndDate == null));
            Assert.AreEqual(1, results.Count(p => p.StartDate == DateTime.Today && p.EndDate == null));
            Assert.AreEqual(1, results.Count(p => p.StartDate == DateTime.Today.AddDays(-1) && p.EndDate == null));
            Assert.AreEqual(1, results.Count(p => p.StartDate == DateTime.Today.AddDays(-5) && p.EndDate == null));
            Assert.AreEqual(1, results.Count(p => p.StartDate == DateTime.Today.AddDays(-10) && p.EndDate == null));
        }

        [TestMethod]
        public void GetActivePricesByScanCodeAndStore_NoPricesExist_ReturnsNoPrices()
        {
            //Given
            var price1 = InsertTestPrices(testItemIds[0], testBusinessUnitIds[0], DateTime.Today, DateTime.Today.AddDays(1), PriceTypeSal);
            var price2 = InsertTestPrices(testItemIds[0], testBusinessUnitIds[0], DateTime.Today.AddDays(-1), DateTime.Today.AddDays(2), PriceTypeSal);
            var price3 = InsertTestPrices(testItemIds[0], testBusinessUnitIds[0], DateTime.Today.AddDays(-5), DateTime.Today.AddDays(5), PriceTypeSal);
            var price4 = InsertTestPrices(testItemIds[0], testBusinessUnitIds[0], DateTime.Today.AddDays(-10), DateTime.Today.AddDays(10), PriceTypeSal);
            

            //When
            var results = queryHandler.Search(new GetActivePricesByScanCodeAndStoreQuery
            {
                Region = testRegion,
                StoreScanCodes = new List<StoreScanCode>
                {
                    new StoreScanCode { BusinessUnitID = testBusinessUnitIds[0], ScanCode = "sc" + testItemIds[1]}
                }
            });

            //Then
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void GetActivePricesByScanCodeAndStore_PricesAreInThePast_ReturnsNoPrices()
        {
            //Given
            var price1 = InsertTestPrices(testItemIds[0], testBusinessUnitIds[0], DateTime.Today.AddDays(-10), DateTime.Today.AddDays(-1), PriceTypeSal);

            //When
            var results = queryHandler.Search(new GetActivePricesByScanCodeAndStoreQuery
            {
                Region = testRegion,
                StoreScanCodes = new List<StoreScanCode>
                {
                    new StoreScanCode { BusinessUnitID = testBusinessUnitIds[0], ScanCode = "sc" + testItemIds[0]}
                }
            });

            //Then
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void GetActivePricesByScanCodeAndStore_PricesAreInTheFuture_ReturnsNoPrices()
        {
            //Given
            var price1 = InsertTestPrices(testItemIds[0], testBusinessUnitIds[0], DateTime.Today.AddDays(1), DateTime.Today.AddDays(20), PriceTypeSal);

            //When
            var results = queryHandler.Search(new GetActivePricesByScanCodeAndStoreQuery
            {
                Region = testRegion,
                StoreScanCodes = new List<StoreScanCode>
                {
                    new StoreScanCode { BusinessUnitID = testBusinessUnitIds[0], ScanCode = "sc" + testItemIds[0]}
                }
            });

            //Then
            Assert.AreEqual(0, results.Count);
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
            dbProvider.Connection.Execute(
                @"  INSERT INTO dbo.ItemTypes(itemTypeCode) 
                    VALUES (@ItemTypeCode)",
                new { ItemTypeCode = TestItemTypeCode });
        }

        private void InsertTestItem(int itemId, int pSNumber)
        {
            dbProvider.Connection.Execute(
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
                dbProvider.Connection.Execute(
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
            var priceId = dbProvider.Connection.QueryFirst<int>(
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

            return dbProvider.Connection.QueryFirst(
                $@" SELECT * 
                    FROM dbo.Price_{testRegion} 
                    WHERE PriceID = @PriceId",
                new { PriceId = priceId });
        }
    }
}
