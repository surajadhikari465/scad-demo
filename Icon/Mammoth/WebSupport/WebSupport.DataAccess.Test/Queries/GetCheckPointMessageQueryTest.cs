using Mammoth.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Transactions;
using WebSupport.DataAccess.Queries;

namespace WebSupport.DataAccess.Test.Queries
{
    [TestClass]
    public class GetCheckPointMessageQueryTest
    {
        private GetCheckPointMessageQuery getCheckPointMessageQuery;
        private GetCheckPointMessageParameters parameters;
        private MammothContext context;
        private TransactionScope transaction;

        [TestInitialize]
        public void Initialize()
        {
            transaction = new TransactionScope();
            context = new MammothContext();
            parameters = new GetCheckPointMessageParameters();
            getCheckPointMessageQuery = new GetCheckPointMessageQuery(context);
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Dispose();
            context.Dispose();
        }

        [TestMethod]
        public void GetCheckPointMessageQuery_SearchForDataThatExistsInDatabase_ReturnsData()
        {
            //Given
            var businessUnitId = "6789";
            var storeName = "UNIT TEST STORE";
            var priceType = "TPR";
            var priceTypeAttribute = "FAKE";
            var sellableUom = "EA";
            var currencyCode = "USD";
            decimal price = 4.00m;
            byte multiple = 1;
            var messageID = "1233";
            var patchFamilySequenceId = "1";
            var PatchFamilyId = "88997798-6789";
            var ItemID = 88997798;
            var scanCode = "123456779";

            DateTime startDate = new DateTime(2017, 8, 28, 10, 10, 10, 0);
            DateTime endDate = startDate.AddDays(10);

            var itemType = context.ItemTypes.Add(new ItemType { itemTypeCode = "TST", itemTypeDesc = "TEST TYPE" });
            context.SaveChanges();
            var item = context.Items.Add(new Item { ItemID = ItemID, ScanCode = scanCode, ItemTypeID = itemType.itemTypeID });
            context.SaveChanges();
            InsertStoreIntoDatabase(businessUnitId, storeName);
            InsertPriceIntoDatabase(businessUnitId, priceType, priceTypeAttribute, sellableUom, currencyCode, price, multiple, startDate, endDate, item);
            InsertIntoMessageSequence(ItemID, businessUnitId, PatchFamilyId, patchFamilySequenceId, messageID, DateTime.Now, DateTime.Now);

            parameters.BusinessUnitId = businessUnitId;
            parameters.Region = "FL";
            parameters.ScanCode = item.ScanCode;

            //When
            var result = getCheckPointMessageQuery.Search(parameters);

            //Then        
            Assert.AreEqual(item.ItemID, result.ItemId);
            Assert.AreEqual(int.Parse(businessUnitId), result.BusinessUnitID);
            Assert.AreEqual(PatchFamilyId, result.PatchFamilyId);
            Assert.AreEqual(int.Parse(patchFamilySequenceId), result.SequenceId);
        }

        [TestMethod]
        public void GetCheckPointMessageQuery_SearchForDataThatDoesNotExistInDatabase_ReturnsNull()
        {
            //Given
            var businessUnitId = "6789";
            var scanCode = "123456779";

            DateTime startDate = new DateTime(2017, 8, 28, 10, 10, 10, 0);
            DateTime endDate = startDate.AddDays(10);

            parameters.BusinessUnitId = businessUnitId;
            parameters.Region = "FL";
            parameters.ScanCode = scanCode;

            //When
            var result = getCheckPointMessageQuery.Search(parameters);

            //Then        
            Assert.AreEqual(null, result);
        }
        private void InsertIntoMessageSequence( object itemID, string businessUnitId, 
                                                string patchFamilyId, string patchFamilySequenceId, 
                                                string messageID, DateTime insertDateUtc, 
                                                DateTime modifiedDateUtc
                                              )
        {
            context.Database.ExecuteSqlCommand($@"
                INSERT INTO [gpm].[MessageSequence]
                       ([ItemID]
                       ,[BusinessUnitID]
                       ,[PatchFamilyID]
                       ,[PatchFamilySequenceID]
                       ,[MessageID]
                       ,[InsertDateUtc]
                       ,[ModifiedDateUtc])
                 VALUES
                       ({itemID}
                        ,{businessUnitId}
                       ,'{patchFamilyId}'
                       ,'{patchFamilySequenceId}'
                       ,'{messageID}'
                       ,'{insertDateUtc.ToString("u")}'
                       ,'{modifiedDateUtc.ToString("u")}')");
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

        private void InsertPriceIntoDatabase(string businessUnitId, string priceType,
                                             string priceTypeAttribute, string sellableUom,
                                             string currencyCode, decimal price,
                                             byte multiple, DateTime startDate,
                                             DateTime endDate, Item item
                                            )
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