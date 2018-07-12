using Dapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;
using WebSupport.DataAccess.Queries;

namespace WebSupport.DataAccess.Test.Queries
{
    [TestClass]
    public class GetItemPsgDataQueryForGpmRegionsTests
    {
        private GetItemPsgDataForGpmRegionQuery query;
        private SqlConnection connection;
        private TransactionScope transaction;
        private GetItemPsgDataForGpmRegionParameters parameters;
        private int testItemId = 900000000;
        private string testScanCode = "999999999999";
        private string testItemTypeCode = "TES";
        private int testItemTypeId;
        private int testBusinessUnitId = 99999;
        private string testStoreName = "TestStoreName";
        private int testPsNumber = 1000;
      
        [TestInitialize]
        public void Initialize()
        {
            transaction = new TransactionScope();
            connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MammothContext"].ConnectionString);
            query = new GetItemPsgDataForGpmRegionQuery(connection);
            parameters = new GetItemPsgDataForGpmRegionParameters();
            parameters.Region = "FL";

            testItemTypeId = connection.Query<int>(
                $@" insert into dbo.ItemTypes(ItemTypeCode) 
                    values('{testItemTypeCode}')
                    SELECT SCOPE_IDENTITY()").Single();
            connection.Execute(
                $@" insert into dbo.Items(
                        ItemID, 
                        ScanCode, 
                        ItemTypeID,
                        PSNumber) 
                    values ({testItemId}, 
                        '{testScanCode}', 
                        {testItemTypeId},
                        {testPsNumber})");
            connection.Execute(
                $@" INSERT INTO dbo.Locales_FL (
                        Region, 
                        BusinessUnitID, 
                        StoreName, 
                        StoreAbbrev)
                    VALUES ('FL', 
                        {testBusinessUnitId}, 
                        '{testStoreName}', 
                        'Test')");

            parameters.BusinessUnitIds = new List<int>
            {
                testBusinessUnitId
            };
            parameters.ScanCodes = new List<string>
            {
                testScanCode
            };
            parameters.ExcludedPsNumbers = new List<int>();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Dispose();
        }

        [TestMethod]
        public void GetItemPsgData_NoSaleExists_ReturnsDelete()
        {
            //Given
            InsertPrice(1, PriceTypes.Codes.Reg, DateTime.Today, null, "Reg");

            //When
            var result = query.Search(parameters);

            //Then
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("Delete", result.Single().MessageAction);
        }

        [TestMethod]
        public void GetItemPsgData_SaleExistsAndIsActiveAndIsPrimeEligible_ReturnsAdd()
        {
            //Given
            InsertPrice(1, PriceTypes.Codes.Reg, DateTime.Today, null,"Reg");
            InsertPrice(1, "TPR", DateTime.Today, DateTime.Today.AddDays(10), "TPR");

            //When
            var result = query.Search(parameters);

            //Then
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("AddOrUpdate", result.Single().MessageAction);
        }

        [TestMethod]
        public void GetItemPsgData_SaleExistsAndIsInTheFutureAndIsPrimeEligible_ReturnsDelete()
        {
            //Given
            InsertPrice(1, PriceTypes.Codes.Reg, DateTime.Today, null,"Reg");
            InsertPrice(1, "TPR", DateTime.Today.AddDays(1), DateTime.Today.AddDays(10),"TPR");

            //When
            var result = query.Search(parameters);

            //Then
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("Delete", result.Single().MessageAction);
        }

        [TestMethod]
        public void GetItemPsgData_SaleExistsAndIsInThePastAndIsPrimeEligible_ReturnsDelete()
        {
            //Given
            InsertPrice(1, PriceTypes.Codes.Reg, DateTime.Today, null, "Reg");
            InsertPrice(1, "TPR", DateTime.Today.AddDays(-10), DateTime.Today.AddDays(-1), "TPR");

            //When
            var result = query.Search(parameters);

            //Then
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("Delete", result.Single().MessageAction);
        }

        [TestMethod]
        public void GetItemPsgData_SaleJustCanceledAndIsPrimeEligible_ReturnsDelete()
        {
            //Given
            InsertPrice(1, PriceTypes.Codes.Reg, DateTime.Today, null, "Reg");
            InsertPrice(1, "TPR", DateTime.Today.AddDays(-10), DateTime.Today,"TPR");

            //When
            var result = query.Search(parameters);

            //Then
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("Delete", result.Single().MessageAction);
        }

        [TestMethod]
        public void GetItemPsgData_RegExistsAndIsInTheFuture_ReturnsDelete()
        {
            //Given
            InsertPrice(1, PriceTypes.Codes.Reg, DateTime.Today.AddDays(1), null,"Reg");

            //When
            var result = query.Search(parameters);

            //Then
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("Delete", result.Single().MessageAction);
        }

        private int InsertPrice(decimal price, string priceType, DateTime startDate, DateTime? endDate, string priceTypeAttribute)
        {
            return connection.Execute(
                $@"INSERT INTO gpm.Price_FL
                       ([Region]
                       ,[GpmID]
                       ,[ItemID]
                       ,[BusinessUnitID]
                       ,[StartDate]
                       ,[EndDate]
                       ,[Price]
                       ,[PriceType]
                       ,[PriceTypeAttribute]
                       ,[SellableUOM]
                       ,[CurrencyCode]
                       ,[Multiple]
                       ,[TagExpirationDate])
                 VALUES
                       ('FL'
                        ,NEWID()
                       ,{testItemId}
                       ,{testBusinessUnitId}
                       ,@startDate
                       ,@endDate
                       ,{price}
                       ,'{priceType}'
                       ,'{priceTypeAttribute}'
                       ,'EA'
                       ,1
                       ,1
                       ,NULL)",
                 new
                 {
                     StartDate = startDate,
                     EndDate = endDate
                 });
    
        }
    }
}