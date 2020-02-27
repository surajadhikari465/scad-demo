using Dapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;
using WebSupport.DataAccess.Queries;
using System.Collections.Generic;

namespace WebSupport.DataAccess.Test.Queries
{
    [TestClass]
    public class GetGpmPricesQueryTests
    {
        private GetGpmPricesQuery query;
        private SqlConnection connection;
        private TransactionScope transaction;
        private GetGpmPricesParameters parameters;
        private int testItemId = 900000000;
        private string testScanCode = "999999999999";
        private string testItemTypeCode = "TES";
        private int testItemTypeId;
        private int testBusinessUnitId = 99999;
        private string testStoreName = "TestStoreName";
        private string testRegion = "FL";

        [TestInitialize]
        public void Initialize()
        {
            transaction = new TransactionScope();
            connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MammothContext"].ConnectionString);
            query = new GetGpmPricesQuery(connection);
            parameters = new GetGpmPricesParameters();

            testItemTypeId = connection.Query<int>(
                $@" insert into dbo.ItemTypes(ItemTypeCode) 
                    values('{testItemTypeCode}')
                    SELECT SCOPE_IDENTITY()").Single();
            connection.Execute(
                $@" insert into dbo.Items(
                        ItemID, 
                        ScanCode, 
                        ItemTypeID) 
                    values ({testItemId}, 
                        '{testScanCode}', 
                        {testItemTypeId})");
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
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Dispose();
        }

        [TestMethod]
        public void GetGpmPrices_PriceExists_ReturnsPrice()
        {
            //Given
            parameters.BusinessUnitId = testBusinessUnitId.ToString();
            parameters.ScanCodes = new List<string>(){ testScanCode };
            parameters.Region = testRegion;

            decimal price = 100m;
            string priceType = PriceTypes.Codes.Reg;
            string priceTypeAttribute = PriceTypes.Codes.Reg;
            DateTime startDate = DateTime.Today;
            DateTime? endDate = null;

            var priceId = InsertPrice(price, priceType, priceTypeAttribute, startDate, endDate);
            InsertMessageSequence();

            //When
            var result = query.Search(parameters);

            //Then
            Assert.AreEqual(1, result.Count);
            var priceModel = result.Single();
            Assert.AreEqual(testBusinessUnitId, priceModel.BusinessUnitId);
            Assert.AreEqual("USD", priceModel.CurrencyCode);
            Assert.AreEqual(endDate, priceModel.EndDate);
            Assert.IsNotNull(priceModel.GpmId);
            Assert.IsNotNull(priceModel.InsertDateUtc);
            Assert.AreEqual(testItemId, priceModel.ItemId);
            Assert.AreEqual(testItemTypeCode, priceModel.ItemTypeCode);
            Assert.IsNull(priceModel.ModifiedDateUtc);
            Assert.AreEqual(1, priceModel.Multiple);
            Assert.IsNull(priceModel.TagExpirationDate);
            Assert.AreEqual(testItemId + "-" + testBusinessUnitId, priceModel.PatchFamilyId);
            Assert.AreEqual(price, priceModel.Price);
            Assert.AreEqual(priceId, priceModel.PriceId);
            Assert.AreEqual(priceType, priceModel.PriceType);
            Assert.AreEqual(priceTypeAttribute, priceModel.PriceTypeAttribute);
            Assert.AreEqual(testRegion, priceModel.Region);
            Assert.AreEqual(testScanCode, priceModel.ScanCode);
            Assert.AreEqual("EA", priceModel.SellableUOM);
            Assert.AreEqual("1", priceModel.SequenceId);
            Assert.AreEqual(startDate, priceModel.StartDate);
            Assert.AreEqual(testStoreName, priceModel.StoreName);
        }

        [TestMethod]
        public void GetGpmPrices_PricesDoNotExist_ReturnNoPrices()
        {
            //Given
            parameters.BusinessUnitId = testBusinessUnitId.ToString();
            parameters.ScanCodes =new List<string>(){ testScanCode };
            parameters.Region = testRegion;

            //When
            var result = query.Search(parameters);

            //Then
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void GetGpmPrices_RegAndTprExist_ReturnsRegAndTpr()
        {
            //Given
            parameters.BusinessUnitId = testBusinessUnitId.ToString();
            parameters.ScanCodes = new List<string>(){ testScanCode };
            parameters.Region = testRegion;

            decimal price = 100m;
            DateTime startDate = DateTime.Today;
            DateTime? endDate = DateTime.Today.AddDays(10);

            InsertPrice(price, PriceTypes.Codes.Reg, PriceTypes.Codes.Reg, startDate, null);
            InsertPrice(price, PriceTypes.Codes.Tpr, PriceTypes.Codes.Tpr, startDate, endDate);
            InsertMessageSequence();

            //When
            var result = query.Search(parameters);

            //Then
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public void GetGpmPrices_NonActiveOldAndFuturePricesExist_ReturnsActivePrices()
        {
            //Given
            parameters.BusinessUnitId = testBusinessUnitId.ToString();
            parameters.ScanCodes = new List<string>(){ testScanCode };
            parameters.Region = testRegion;

            decimal priceReg = 100m;
            decimal priceTpr = 101m;
            DateTime startDate = DateTime.Today;
            DateTime? endDate = DateTime.Today.AddDays(100);

            var activeRegPriceId = InsertPrice(priceReg, PriceTypes.Codes.Reg, PriceTypes.Codes.Reg, startDate, null);
            InsertPrice(priceReg, PriceTypes.Codes.Reg, PriceTypes.Codes.Reg, startDate.AddDays(1), null);
            InsertPrice(priceReg, PriceTypes.Codes.Reg, PriceTypes.Codes.Reg, startDate.AddDays(5), null);
            InsertPrice(priceReg, PriceTypes.Codes.Reg, PriceTypes.Codes.Reg, startDate.AddDays(10), null);
            InsertPrice(priceReg, PriceTypes.Codes.Reg, PriceTypes.Codes.Reg, startDate.AddDays(-1), null);
            InsertPrice(priceReg, PriceTypes.Codes.Reg, PriceTypes.Codes.Reg, startDate.AddDays(-5), null);
            InsertPrice(priceReg, PriceTypes.Codes.Reg, PriceTypes.Codes.Reg, startDate.AddDays(-10), null);

            var activeTprPriceId = InsertPrice(priceTpr, PriceTypes.Codes.Tpr, PriceTypes.Codes.Tpr, startDate, endDate);
            InsertPrice(priceTpr, PriceTypes.Codes.Tpr, PriceTypes.Codes.Tpr, startDate.AddDays(1), endDate);
            InsertPrice(priceTpr, PriceTypes.Codes.Tpr, PriceTypes.Codes.Tpr, startDate.AddDays(5), endDate);
            InsertPrice(priceTpr, PriceTypes.Codes.Tpr, PriceTypes.Codes.Tpr, startDate.AddDays(10), endDate);
            InsertPrice(priceTpr, PriceTypes.Codes.Tpr, PriceTypes.Codes.Tpr, startDate.AddDays(-1), endDate);
            InsertPrice(priceTpr, PriceTypes.Codes.Tpr, PriceTypes.Codes.Tpr, startDate.AddDays(-5), endDate);
            InsertPrice(priceTpr, PriceTypes.Codes.Tpr, PriceTypes.Codes.Tpr, startDate.AddDays(-10), endDate);
            InsertMessageSequence();

            //When
            var result = query.Search(parameters);

            //Then
            Assert.AreEqual(2, result.Count);

            var priceModelReg = result.Single(p => p.PriceType == PriceTypes.Codes.Reg);
            Assert.AreEqual(testBusinessUnitId, priceModelReg.BusinessUnitId);
            Assert.AreEqual("USD", priceModelReg.CurrencyCode);
            Assert.IsNull(priceModelReg.EndDate);
            Assert.IsNotNull(priceModelReg.GpmId);
            Assert.IsNotNull(priceModelReg.InsertDateUtc);
            Assert.AreEqual(testItemId, priceModelReg.ItemId);
            Assert.AreEqual(testItemTypeCode, priceModelReg.ItemTypeCode);
            Assert.IsNull(priceModelReg.ModifiedDateUtc);
            Assert.AreEqual(1, priceModelReg.Multiple);
            Assert.IsNull(priceModelReg.TagExpirationDate);
            Assert.AreEqual(testItemId + "-" + testBusinessUnitId, priceModelReg.PatchFamilyId);
            Assert.AreEqual(priceReg, priceModelReg.Price);
            Assert.AreEqual(activeRegPriceId, priceModelReg.PriceId);
            Assert.AreEqual(PriceTypes.Codes.Reg, priceModelReg.PriceType);
            Assert.AreEqual(PriceTypes.Codes.Reg, priceModelReg.PriceTypeAttribute);
            Assert.AreEqual(testRegion, priceModelReg.Region);
            Assert.AreEqual(testScanCode, priceModelReg.ScanCode);
            Assert.AreEqual("EA", priceModelReg.SellableUOM);
            Assert.AreEqual("1", priceModelReg.SequenceId);
            Assert.AreEqual(startDate, priceModelReg.StartDate);
            Assert.AreEqual(testStoreName, priceModelReg.StoreName);

            var priceModelTpr = result.Single(p => p.PriceType == PriceTypes.Codes.Tpr);
            Assert.AreEqual(testBusinessUnitId, priceModelTpr.BusinessUnitId);
            Assert.AreEqual("USD", priceModelTpr.CurrencyCode);
            Assert.AreEqual(endDate, priceModelTpr.EndDate);
            Assert.IsNotNull(priceModelTpr.GpmId);
            Assert.IsNotNull(priceModelTpr.InsertDateUtc);
            Assert.AreEqual(testItemId, priceModelTpr.ItemId);
            Assert.AreEqual(testItemTypeCode, priceModelTpr.ItemTypeCode);
            Assert.IsNull(priceModelTpr.ModifiedDateUtc);
            Assert.AreEqual(1, priceModelTpr.Multiple);
            Assert.IsNull(priceModelTpr.TagExpirationDate);
            Assert.AreEqual(testItemId + "-" + testBusinessUnitId, priceModelTpr.PatchFamilyId);
            Assert.AreEqual(priceTpr, priceModelTpr.Price);
            Assert.AreEqual(activeTprPriceId, priceModelTpr.PriceId);
            Assert.AreEqual(PriceTypes.Codes.Tpr, priceModelTpr.PriceType);
            Assert.AreEqual(PriceTypes.Codes.Tpr, priceModelTpr.PriceTypeAttribute);
            Assert.AreEqual(testRegion, priceModelTpr.Region);
            Assert.AreEqual(testScanCode, priceModelTpr.ScanCode);
            Assert.AreEqual("EA", priceModelTpr.SellableUOM);
            Assert.AreEqual("1", priceModelTpr.SequenceId);
            Assert.AreEqual(startDate, priceModelTpr.StartDate);
            Assert.AreEqual(testStoreName, priceModelTpr.StoreName);
        }

        [TestMethod]
        public void GetGpmPrices_NewPriceTypePriceExists_ReturnsNewPriceTypePrices()
        {
            //Given
            parameters.BusinessUnitId = testBusinessUnitId.ToString();
            parameters.ScanCodes = new List<string>(){ testScanCode };
            parameters.Region = testRegion;

            decimal price = 100m;
            DateTime startDate = DateTime.Today.AddDays(-5);
            DateTime? endDate = DateTime.Today.AddDays(100);

            InsertPrice(price, PriceTypes.Codes.Reg, PriceTypes.Codes.Reg, startDate, null);
            InsertPrice(price, PriceTypes.Codes.Tpr, PriceTypes.Codes.Tpr, startDate, endDate);
            InsertPrice(price, "TST", "TST", startDate, endDate);
            InsertPrice(price, "T2T", "T2T", startDate, endDate);
            InsertMessageSequence();

            //When
            var result = query.Search(parameters);

            //Then
            Assert.AreEqual(4, result.Count);
        }

        [TestMethod]
        public void GetGpmPrices_RegAndOnlyExpiredAndFutureTprPricesExists_ReturnsOnlyRegPrice()
        {
            //Given
            parameters.BusinessUnitId = testBusinessUnitId.ToString();
            parameters.ScanCodes = new List<string>(){ testScanCode };
            parameters.Region = testRegion;

            decimal priceReg = 100m;
            decimal priceTpr = 101m;
            DateTime startDate = DateTime.Today;
            DateTime? endDate = DateTime.Today.AddDays(100);
            DateTime expiredStartDate = DateTime.Today.AddDays(-200);
            DateTime? expiredEndDate = DateTime.Today.AddDays(-100);

            var activeRegPriceId = InsertPrice(priceReg, PriceTypes.Codes.Reg, PriceTypes.Codes.Reg, startDate, null);
            InsertPrice(priceReg, PriceTypes.Codes.Reg, PriceTypes.Codes.Reg, startDate.AddDays(1), null);
            InsertPrice(priceReg, PriceTypes.Codes.Reg, PriceTypes.Codes.Reg, startDate.AddDays(5), null);
            InsertPrice(priceReg, PriceTypes.Codes.Reg, PriceTypes.Codes.Reg, startDate.AddDays(10), null);
            InsertPrice(priceReg, PriceTypes.Codes.Reg, PriceTypes.Codes.Reg, startDate.AddDays(-1), null);
            InsertPrice(priceReg, PriceTypes.Codes.Reg, PriceTypes.Codes.Reg, startDate.AddDays(-5), null);
            InsertPrice(priceReg, PriceTypes.Codes.Reg, PriceTypes.Codes.Reg, startDate.AddDays(-10), null);

            InsertPrice(priceTpr, PriceTypes.Codes.Tpr, PriceTypes.Codes.Tpr, startDate.AddDays(5), endDate);
            InsertPrice(priceTpr, PriceTypes.Codes.Tpr, PriceTypes.Codes.Tpr, startDate.AddDays(10), endDate);
            InsertPrice(priceTpr, PriceTypes.Codes.Tpr, PriceTypes.Codes.Tpr, expiredStartDate.AddDays(-1), expiredEndDate);
            InsertPrice(priceTpr, PriceTypes.Codes.Tpr, PriceTypes.Codes.Tpr, expiredStartDate.AddDays(-5), expiredEndDate);
            InsertPrice(priceTpr, PriceTypes.Codes.Tpr, PriceTypes.Codes.Tpr, expiredStartDate.AddDays(-10), expiredEndDate);
            InsertMessageSequence();

            //When
            var result = query.Search(parameters);

            //Then
            Assert.AreEqual(1, result.Count);

            var priceModelReg = result.Single(p => p.PriceType == PriceTypes.Codes.Reg);
            Assert.AreEqual(testBusinessUnitId, priceModelReg.BusinessUnitId);
            Assert.AreEqual("USD", priceModelReg.CurrencyCode);
            Assert.IsNull(priceModelReg.EndDate);
            Assert.IsNotNull(priceModelReg.GpmId);
            Assert.IsNotNull(priceModelReg.InsertDateUtc);
            Assert.AreEqual(testItemId, priceModelReg.ItemId);
            Assert.AreEqual(testItemTypeCode, priceModelReg.ItemTypeCode);
            Assert.IsNull(priceModelReg.ModifiedDateUtc);
            Assert.AreEqual(1, priceModelReg.Multiple);
            Assert.IsNull(priceModelReg.TagExpirationDate);
            Assert.AreEqual(testItemId + "-" + testBusinessUnitId, priceModelReg.PatchFamilyId);
            Assert.AreEqual(priceReg, priceModelReg.Price);
            Assert.AreEqual(activeRegPriceId, priceModelReg.PriceId);
            Assert.AreEqual(PriceTypes.Codes.Reg, priceModelReg.PriceType);
            Assert.AreEqual(PriceTypes.Codes.Reg, priceModelReg.PriceTypeAttribute);
            Assert.AreEqual(testRegion, priceModelReg.Region);
            Assert.AreEqual(testScanCode, priceModelReg.ScanCode);
            Assert.AreEqual("EA", priceModelReg.SellableUOM);
            Assert.AreEqual("1", priceModelReg.SequenceId);
            Assert.AreEqual(startDate, priceModelReg.StartDate);
            Assert.AreEqual(testStoreName, priceModelReg.StoreName);
        }

        private int InsertPrice(decimal price, string priceType, string priceTypeAttribute, DateTime startDate, DateTime? endDate)
        {
            return connection.Query<int>(
                $@"INSERT INTO gpm.Price_FL
                       (Region
                       ,GpmID
                       ,ItemID
                       ,BusinessUnitID
                       ,StartDate
                       ,EndDate
                       ,Price
                       ,PriceType
                       ,PriceTypeAttribute
                       ,SellableUOM
                       ,CurrencyCode
                       ,Multiple
                       ,TagExpirationDate
                       ,InsertDateUtc
                       ,ModifiedDateUtc)
                 VALUES
                       ('FL'
                       ,NEWID()
                       ,{testItemId}
                       ,{testBusinessUnitId}
                       ,@StartDate
                       ,@EndDate
                       ,{price}
                       ,'{priceType}'
                       ,'{priceTypeAttribute}'
                       ,'EA'
                       ,'USD'
                       ,1
                       ,NULL
                       ,GETDATE()
                       ,NULL)

                    SELECT SCOPE_IDENTITY()",
                       new
                       {
                           StartDate = startDate,
                           EndDate = endDate
                       }).Single();
        }

        private void InsertMessageSequence()
        {
            connection.Execute(
                $@"INSERT INTO gpm.MessageSequence
                       (ItemID
                       ,BusinessUnitID
                       ,PatchFamilyID
                       ,PatchFamilySequenceID)
                 VALUES
                       ({testItemId}
                       ,{testBusinessUnitId}
                       ,'{testItemId}-{testBusinessUnitId}'
                       ,'1')");
        }
    }
}