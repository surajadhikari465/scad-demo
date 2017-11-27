using Mammoth.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using WebSupport.DataAccess.Queries;

namespace WebSupport.DataAccess.Test.Queries
{
    [TestClass]
    public class GetPriceResetPricesQueryTests
    {
        private GetPriceResetPricesQuery getPriceResetPricesQuery;
        private GetPriceResetPricesParameters parameters;
        private MammothContext context;
        private TransactionScope transaction;

        [TestInitialize]
        public void Initialize()
        {
            transaction = new TransactionScope();
            context = new MammothContext();
            parameters = new GetPriceResetPricesParameters();
            getPriceResetPricesQuery = new GetPriceResetPricesQuery(context);
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Dispose();
            context.Dispose();
        }

        [TestMethod]
        public void GetPriceResetPrices_PriceExists_ReturnsPrice()
        {
            //Given
            var businessUnitId = "6789";
            var storeName = "UNIT TEST STORE";
            var priceType = "TPR";
            var priceTypeAttribute = "REG";
            var sellableUom = "EA";
            var currencyCode = "USD";
            decimal price = 4.00m;
            byte multiple = 1;
            DateTime today = DateTime.Now;
            DateTime startDate = new DateTime(today.Year, today.Month, today.Day, 0, 0, 0, 0);
            DateTime endDate = startDate.AddDays(10);

            var itemType = context.ItemTypes.Add(new ItemType { itemTypeCode = "TST", itemTypeDesc = "TEST TYPE" });
            context.SaveChanges();
            var item = context.Items.Add(new Item { ItemID = 88997788, ScanCode = "123456789", ItemTypeID = itemType.itemTypeID });
            context.SaveChanges();
            InsertStoreIntoDatabase(businessUnitId, storeName);
            InsertPriceIntoDatabase(businessUnitId, priceType, priceTypeAttribute, sellableUom, currencyCode, price, multiple, startDate, endDate, item);

            parameters.BusinessUnitIds = new List<string> { businessUnitId };
            parameters.Region = "FL";
            parameters.ScanCodes = new List<string> { item.ScanCode };

            //When
            var result = getPriceResetPricesQuery.Search(parameters);

            //Then
            var priceResetPrice = result.Single();
            Assert.AreEqual(item.ItemID, priceResetPrice.ItemId);
            Assert.AreEqual(item.ScanCode, priceResetPrice.ScanCode);
            Assert.AreEqual(itemType.itemTypeCode, priceResetPrice.ItemTypeCode);
            Assert.AreEqual(itemType.itemTypeDesc, priceResetPrice.ItemTypeDesc);
            Assert.AreEqual(int.Parse(businessUnitId), priceResetPrice.BusinessUnitId);
            Assert.AreEqual(storeName, priceResetPrice.StoreName);
            Assert.AreEqual(priceType, priceResetPrice.PriceType);
            Assert.AreEqual(priceTypeAttribute, priceResetPrice.PriceTypeAttribute);
            Assert.AreEqual(sellableUom, priceResetPrice.SellableUom);
            Assert.AreEqual(currencyCode, priceResetPrice.CurrencyCode);
            Assert.AreEqual(price, priceResetPrice.Price);
            Assert.AreEqual(multiple, priceResetPrice.Multiple);
            Assert.AreEqual(startDate, priceResetPrice.StartDate);
            Assert.AreEqual(endDate, priceResetPrice.EndDate);
        }

        [TestMethod]
        public void GetPriceResetPrices_MultiplePricesExists_ReturnsAllPrices()
        {
            //Given
            var businessUnitId = "6789";
            var storeName = "UNIT TEST STORE";
            var regPriceType = "REG";
            var salePriceType = "TPR";
            var priceTypeAttribute = "REG";
            var sellableUom = "EA";
            var currencyCode = "USD";
            decimal regPrice = 4.00m;
            decimal salePrice = 3.00m;
            byte multiple = 1;
            DateTime today = DateTime.Now;
            DateTime startDate = new DateTime(today.Year, today.Month, today.Day, 0, 0, 0, 0);
            DateTime endDate = startDate.AddDays(10);

            var itemType = context.ItemTypes.Add(new ItemType { itemTypeCode = "TST", itemTypeDesc = "TEST TYPE" });
            context.SaveChanges();
            var item = context.Items.Add(new Item { ItemID = 88997788, ScanCode = "123456789", ItemTypeID = itemType.itemTypeID });
            context.SaveChanges();
            InsertStoreIntoDatabase(businessUnitId, storeName);
            InsertPriceIntoDatabase(businessUnitId, regPriceType, priceTypeAttribute, sellableUom, currencyCode, regPrice, multiple, startDate, endDate, item);
            InsertPriceIntoDatabase(businessUnitId, salePriceType, priceTypeAttribute, sellableUom, currencyCode, salePrice, multiple, startDate, endDate, item);

            parameters.BusinessUnitIds = new List<string> { businessUnitId };
            parameters.Region = "FL";
            parameters.ScanCodes = new List<string> { item.ScanCode };

            //When
            var result = getPriceResetPricesQuery.Search(parameters);

            //Then
            var regPriceResetPrice = result.Single(p => p.PriceType == regPriceType);
            Assert.AreEqual(item.ItemID, regPriceResetPrice.ItemId);
            Assert.AreEqual(item.ScanCode, regPriceResetPrice.ScanCode);
            Assert.AreEqual(itemType.itemTypeCode, regPriceResetPrice.ItemTypeCode);
            Assert.AreEqual(itemType.itemTypeDesc, regPriceResetPrice.ItemTypeDesc);
            Assert.AreEqual(int.Parse(businessUnitId), regPriceResetPrice.BusinessUnitId);
            Assert.AreEqual(storeName, regPriceResetPrice.StoreName);
            Assert.AreEqual(regPriceType, regPriceResetPrice.PriceType);
            Assert.AreEqual(priceTypeAttribute, regPriceResetPrice.PriceTypeAttribute);
            Assert.AreEqual(sellableUom, regPriceResetPrice.SellableUom);
            Assert.AreEqual(currencyCode, regPriceResetPrice.CurrencyCode);
            Assert.AreEqual(regPrice, regPriceResetPrice.Price);
            Assert.AreEqual(multiple, regPriceResetPrice.Multiple);
            Assert.AreEqual(startDate, regPriceResetPrice.StartDate);
            Assert.AreEqual(endDate, regPriceResetPrice.EndDate);

            var salePriceResetPrice = result.Single(p => p.PriceType == salePriceType);
            Assert.AreEqual(item.ItemID, salePriceResetPrice.ItemId);
            Assert.AreEqual(item.ScanCode, salePriceResetPrice.ScanCode);
            Assert.AreEqual(itemType.itemTypeCode, salePriceResetPrice.ItemTypeCode);
            Assert.AreEqual(itemType.itemTypeDesc, salePriceResetPrice.ItemTypeDesc);
            Assert.AreEqual(int.Parse(businessUnitId), salePriceResetPrice.BusinessUnitId);
            Assert.AreEqual(storeName, salePriceResetPrice.StoreName);
            Assert.AreEqual(salePriceType, salePriceResetPrice.PriceType);
            Assert.AreEqual(priceTypeAttribute, salePriceResetPrice.PriceTypeAttribute);
            Assert.AreEqual(sellableUom, salePriceResetPrice.SellableUom);
            Assert.AreEqual(currencyCode, salePriceResetPrice.CurrencyCode);
            Assert.AreEqual(salePrice, salePriceResetPrice.Price);
            Assert.AreEqual(multiple, salePriceResetPrice.Multiple);
            Assert.AreEqual(startDate, salePriceResetPrice.StartDate);
            Assert.AreEqual(endDate, salePriceResetPrice.EndDate);
        }

        [TestMethod]
        public void GetPriceResetPrices_ExpiredRegAndTprPricesExist_ReturnsOnlyActiveAndFuturePrices()
        {
            //Given
            var businessUnitId = "6789";
            var storeName = "UNIT TEST STORE";
            var regPriceType = "REG";
            var salePriceType = "TPR";
            var priceTypeAttribute = "REG";
            var sellableUom = "EA";
            var currencyCode = "USD";
            decimal regPrice = 4.00m;
            decimal salePrice = 3.00m;
            byte multiple = 1;
            DateTime today = DateTime.Now;
            DateTime todayWithoutMilliseconds = new DateTime(today.Year, today.Month, today.Day, today.Hour, today.Minute, today.Second, 0);
            DateTime regStartDate = todayWithoutMilliseconds.AddDays(-5);
            DateTime regExpiredStartDate = regStartDate.AddDays(-5);
            DateTime regFutureStartDate = todayWithoutMilliseconds.AddDays(10);

            DateTime tprStartDate = todayWithoutMilliseconds.AddDays(-20);
            DateTime tprExpiredStartDate = tprStartDate.AddDays(-10);
            DateTime tprExpiredStartDate2 = tprStartDate.AddDays(-11);
            DateTime tprFutureStartDate = todayWithoutMilliseconds.AddDays(11);
            DateTime tprFutureStartDate2 = todayWithoutMilliseconds.AddDays(12);
            DateTime tprFutureStartDate3 = todayWithoutMilliseconds.AddDays(13);

            DateTime endDate = todayWithoutMilliseconds.AddDays(100);
            DateTime endDateExpired = today.AddDays(-1);

            var itemType = context.ItemTypes.Add(new ItemType { itemTypeCode = "TST", itemTypeDesc = "TEST TYPE" });
            context.SaveChanges();
            var item = context.Items.Add(new Item { ItemID = 88997788, ScanCode = "123456789", ItemTypeID = itemType.itemTypeID });
            context.SaveChanges();
            InsertStoreIntoDatabase(businessUnitId, storeName);
            //Insert regs
            InsertPriceIntoDatabase(businessUnitId, regPriceType, priceTypeAttribute, sellableUom, currencyCode, regPrice, multiple, regStartDate, endDate, item);
            InsertPriceIntoDatabase(businessUnitId, regPriceType, priceTypeAttribute, sellableUom, currencyCode, regPrice, multiple, regExpiredStartDate, endDate, item);
            InsertPriceIntoDatabase(businessUnitId, regPriceType, priceTypeAttribute, sellableUom, currencyCode, regPrice, multiple, regFutureStartDate, endDate, item);
            //Insert tprs
            InsertPriceIntoDatabase(businessUnitId, salePriceType, priceTypeAttribute, sellableUom, currencyCode, salePrice, multiple, tprStartDate, endDate, item);
            InsertPriceIntoDatabase(businessUnitId, salePriceType, priceTypeAttribute, sellableUom, currencyCode, salePrice, multiple, tprExpiredStartDate, endDateExpired, item);
            InsertPriceIntoDatabase(businessUnitId, salePriceType, priceTypeAttribute, sellableUom, currencyCode, salePrice, multiple, tprExpiredStartDate2, endDateExpired, item);
            InsertPriceIntoDatabase(businessUnitId, salePriceType, priceTypeAttribute, sellableUom, currencyCode, salePrice, multiple, tprFutureStartDate, endDate, item);
            InsertPriceIntoDatabase(businessUnitId, salePriceType, priceTypeAttribute, sellableUom, currencyCode, salePrice, multiple, tprFutureStartDate2, endDate, item);
            InsertPriceIntoDatabase(businessUnitId, salePriceType, priceTypeAttribute, sellableUom, currencyCode, salePrice, multiple, tprFutureStartDate3, endDate, item);

            parameters.BusinessUnitIds = new List<string> { businessUnitId };
            parameters.Region = "FL";
            parameters.ScanCodes = new List<string> { item.ScanCode };

            //When
            var result = getPriceResetPricesQuery.Search(parameters);

            //Then
            var regPriceResetPrices = result.Where(p => p.PriceType == regPriceType);
            Assert.AreEqual(2, regPriceResetPrices.Count());
            Assert.IsNotNull(regPriceResetPrices.SingleOrDefault(r => r.StartDate == regStartDate));
            Assert.IsNotNull(regPriceResetPrices.SingleOrDefault(r => r.StartDate == regFutureStartDate));

            var salePriceResetPrice = result.Where(p => p.PriceType == salePriceType);
            Assert.AreEqual(4, salePriceResetPrice.Count());
            Assert.IsNotNull(salePriceResetPrice.SingleOrDefault(r => r.StartDate == tprStartDate));
            Assert.IsNotNull(salePriceResetPrice.SingleOrDefault(r => r.StartDate == tprFutureStartDate));
            Assert.IsNotNull(salePriceResetPrice.SingleOrDefault(r => r.StartDate == tprFutureStartDate2));
            Assert.IsNotNull(salePriceResetPrice.SingleOrDefault(r => r.StartDate == tprFutureStartDate3));
        }

        [TestMethod]
        public void GetPriceResetPrices_OnlyFuturePricesExist_ReturnsFuturePrices()
        {
            //Given
            var businessUnitId = "6789";
            var storeName = "UNIT TEST STORE";
            var regPriceType = "REG";
            var salePriceType = "TPR";
            var priceTypeAttribute = "REG";
            var sellableUom = "EA";
            var currencyCode = "USD";
            decimal regPrice = 4.00m;
            decimal salePrice = 3.00m;
            byte multiple = 1;
            DateTime today = DateTime.Now;
            DateTime todayWithoutMilliseconds = new DateTime(today.Year, today.Month, today.Day, today.Hour, today.Minute, today.Second, 0);
            DateTime regFutureStartDate = todayWithoutMilliseconds.AddDays(10);
            DateTime tprFutureStartDate = todayWithoutMilliseconds.AddDays(11);

            DateTime endDate = todayWithoutMilliseconds.AddDays(100);

            var itemType = context.ItemTypes.Add(new ItemType { itemTypeCode = "TST", itemTypeDesc = "TEST TYPE" });
            context.SaveChanges();
            var item = context.Items.Add(new Item { ItemID = 88997788, ScanCode = "123456789", ItemTypeID = itemType.itemTypeID });
            context.SaveChanges();
            InsertStoreIntoDatabase(businessUnitId, storeName);
            InsertPriceIntoDatabase(businessUnitId, regPriceType, priceTypeAttribute, sellableUom, currencyCode, regPrice, multiple, regFutureStartDate, endDate, item);
            InsertPriceIntoDatabase(businessUnitId, salePriceType, priceTypeAttribute, sellableUom, currencyCode, salePrice, multiple, tprFutureStartDate, endDate, item);

            parameters.BusinessUnitIds = new List<string> { businessUnitId };
            parameters.Region = "FL";
            parameters.ScanCodes = new List<string> { item.ScanCode };

            //When
            var result = getPriceResetPricesQuery.Search(parameters);

            //Then
            Assert.AreEqual(2, result.Count);
            var regPriceResetPrices = result.Where(p => p.PriceType == regPriceType);
            Assert.IsNotNull(regPriceResetPrices.SingleOrDefault(r => r.StartDate == regFutureStartDate));

            var salePriceResetPrice = result.Where(p => p.PriceType == salePriceType);
            Assert.IsNotNull(salePriceResetPrice.SingleOrDefault(r => r.StartDate == tprFutureStartDate));
        }

        private void InsertStoreIntoDatabase(string businessUnitId, string storeName)
        {
            context.Database.ExecuteSqlCommand($@"
                INSERT INTO [dbo].[Locales_FL]
                       ([Region]
                       ,[BusinessUnitID]
                       ,[StoreName]
                       ,[StoreAbbrev])
                 VALUES
                       ('FL'
                       ,{businessUnitId}
                       ,'{storeName}'
                       ,'UTS')");
        }

        private void InsertPriceIntoDatabase(
            string businessUnitId, 
            string priceType, 
            string priceTypeAttribute, 
            string sellableUom, 
            string currencyCode, 
            decimal price, 
            byte multiple, 
            DateTime startDate, 
            DateTime endDate, 
            Item item)
        {
            context.Database.ExecuteSqlCommand($@"
                INSERT INTO [gpm].[Price_FL]
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
                            ,[NewTagExpiration])
                        VALUES
                            ('FL'
                            ,NEWID()
                            ,{item.ItemID}
                            ,{businessUnitId}
                            ,'{startDate.ToString("u")}'
                            ,'{endDate.ToString("u")}'
                            ,{price}
                            ,'{priceType}'
                            ,'{priceTypeAttribute}'
                            ,'{sellableUom}'
                            ,'{currencyCode}'
                            ,{multiple}
                            ,'{endDate.ToString("u")}')");
        }
    }
}
