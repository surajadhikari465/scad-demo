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
    /// <summary>
    /// Summary description for GetPricesByScanCodeAndBusinessUnitIDQueryHandlerTests
    /// </summary>
    [TestClass]
    public class GetPricesByScanCodeAndBusinessUnitIDQueryHandlerTests
    {
        private IDbProvider db;
        private GetPricesByScanCodeAndStoreQueryHandler queryHandler;
        private GetPricesByScanCodeAndStoreQuery query;

        #region Additional test attributes
    
        // Use TestInitialize to run code before running each test 
        [TestInitialize()]
        public void MyTestInitialize()
        {
            db = new SqlDbProvider();
            db.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Mammoth"].ConnectionString);
            db.Connection.Open();
            db.Transaction = db.Connection.BeginTransaction();

            this.query = new GetPricesByScanCodeAndStoreQuery();
            this.queryHandler = new GetPricesByScanCodeAndStoreQueryHandler(this.db);
        }

        // Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void MyTestCleanup()
        {
            this.queryHandler = null;
            if (this.db.Transaction != null)
            {
                this.db.Transaction.Rollback();
                this.db.Transaction.Dispose();
            }
            this.db.Connection.Dispose();
        }

        #endregion

        [TestMethod]
        public void GetPricesByScanCodeAndBusinessUnitIDQueryHandler_ShouldReturnPricesWithSpecifiedScanCodeAndBusinessUnitID()
        {
            // Given
            var expectedStore1BusID = 70001;
            var expectedStore2BusID = 70002;
            var expectedItem1ID = 900001;
            var expectedItem1ScanCode = "7000000000001";
            var expectedItem2ID = 900002;
            var expectedItem2ScanCode = "7000000000002";

            var objectFactory = new ObjectBuilderFactory(this.GetType().Assembly);
            var dapperFactory = new DapperSqlFactory(this.GetType().Assembly);
            var insertStoreSql = dapperFactory.BuildInsertSql<Locales>(true);
            var insertItemSql = @"INSERT INTO dbo.Items (ItemID, ScanCode, Desc_Product, AddedDate) VALUES (@ItemID, @ScanCode, @Desc_Product, @AddedDate) SELECT SCOPE_IDENTITY()";
            var insertPriceSql = dapperFactory.BuildInsertSql<Prices>(true);

            int currencyId = this.db.Connection.Query<int>(
                dapperFactory.BuildInsertSql<Currency>(true),
                objectFactory.Build<Currency>()
                    .With(c => c.CurrencyCode, "USD")
                    .With(c => c.CurrencyDesc, "US Dollar").CreatedObject,
                this.db.Transaction).First();

            this.db.Connection.Execute(
                insertStoreSql,
                objectFactory.Build<Locales>()
                    .With(l => l.BusinessUnitID, expectedStore1BusID)
                    .With(l => l.Region, "FL")
                    .With(l => l.StoreName, "My Test Store 1")
                    .With(l => l.StoreAbbrev, "MTS1").CreatedObject, 
                this.db.Transaction);

            this.db.Connection.Execute(
                insertStoreSql, 
                objectFactory.Build<Locales>()
                    .With(l => l.BusinessUnitID, expectedStore2BusID)
                    .With(l => l.Region, "FL")
                    .With(l => l.StoreName, "My Test Store 2")
                    .With(l => l.StoreAbbrev, "MTS2").CreatedObject, 
                this.db.Transaction);

            this.db.Connection.Execute(
                insertItemSql,
                objectFactory.Build<Item>()
                    .With(i => i.ItemID, expectedItem1ID)
                    .With(i => i.ScanCode, expectedItem1ScanCode)
                    .With(i => i.Desc_Product, "My Test Product 1").CreatedObject,
                this.db.Transaction);

            this.db.Connection.Execute(
                insertItemSql,
                objectFactory.Build<Item>()
                    .With(i => i.ItemID, expectedItem2ID)
                    .With(i => i.ScanCode, expectedItem2ScanCode)
                    .With(i => i.Desc_Product, "My Test Product 2").CreatedObject,
                this.db.Transaction);

            this.db.Connection.Execute(
                insertPriceSql,
                objectFactory.Build<Prices>()
                    .With(p => p.ItemID, expectedItem1ID)
                    .With(p => p.BusinessUnitID, expectedStore1BusID)
                    .With(p => p.Price, 3.50m)
                    .With(p => p.PriceType, "SAL")
                    .With(p => p.StartDate, DateTime.Now.Date.AddDays(-3))
                    .With(p => p.EndDate, DateTime.Now.Date.AddDays(7))
                    .With(p => p.CurrencyID, currencyId).CreatedObject,
                this.db.Transaction);

            this.db.Connection.Execute(
                insertPriceSql,
                objectFactory.Build<Prices>()
                    .With(p => p.ItemID, expectedItem2ID)
                    .With(p => p.BusinessUnitID, expectedStore2BusID)
                    .With(p => p.Price, 9.99m)
                    .With(p => p.PriceType, "SAL")
                    .With(p => p.StartDate, DateTime.Now.Date.AddDays(-3))
                    .With(p => p.EndDate, DateTime.Now.Date.AddDays(10))
                    .With(p => p.CurrencyID, currencyId).CreatedObject,
                this.db.Transaction);

            this.query = new GetPricesByScanCodeAndStoreQuery
            {
                Region = "FL",
                BusinessUnitIds = new List<int> { expectedStore1BusID, expectedStore2BusID },
                ScanCodes = new List<string> { expectedItem1ScanCode, expectedItem2ScanCode }               
            };

            // When
            var prices = this.queryHandler.Search(this.query);

            // Then
            var price1 = prices.Single(p => p.ScanCode == expectedItem1ScanCode && p.BusinessUnitId == expectedStore1BusID);
            Assert.AreEqual(3.50m, price1.Price);
            Assert.AreEqual(DateTime.Now.Date.AddDays(-3), price1.StartDate);
            Assert.AreEqual(DateTime.Now.Date.AddDays(7), price1.EndDate);

            var price2 = prices.Single(p => p.ScanCode == expectedItem2ScanCode && p.BusinessUnitId == expectedStore2BusID);
            Assert.AreEqual(9.99m, price2.Price);
            Assert.AreEqual(DateTime.Now.Date.AddDays(-3), price2.StartDate);
            Assert.AreEqual(DateTime.Now.Date.AddDays(10), price2.EndDate);
        }
    }
}
