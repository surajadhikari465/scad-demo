using Icon.Common;
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
        public void GetCheckPointMessageQuery_SearchForSingleExistingStoreItem_ReturnsChkPtMsgStoreItem()
        {
            //Given
            var businessUnitId = 6789;
            var scanCode = "123456779";
            var storeName = "UNIT TEST STORE";
            var itemID = 88997798;
            var messageID = "1233";
            var patchFamilySequenceId = "1";
            var patchFamilyId = $"{businessUnitId}-{itemID}";

            var item = InsertTestItemIntoDatabase(itemID, scanCode);
            InsertStoreIntoDatabase(businessUnitId, storeName);
            InsertIntoMessageSequence(itemID, businessUnitId.ToString(), patchFamilyId, patchFamilySequenceId, messageID, DateTime.Now, DateTime.Now);

            parameters.Region = "FL";
            parameters.BusinessUnitIds = new List<int> { businessUnitId };
            parameters.ScanCodes = new List<string> { scanCode };

            //When
            var result = getCheckPointMessageQuery.Search(parameters);

            //Then        
            var firsCheckpointMsg = result.FirstOrDefault();
            Assert.IsNotNull(firsCheckpointMsg);
            Assert.AreEqual(item.ItemID, firsCheckpointMsg.ItemId);
            Assert.AreEqual(businessUnitId, firsCheckpointMsg.BusinessUnitID);
            Assert.AreEqual(patchFamilyId, firsCheckpointMsg.PatchFamilyId);
            Assert.AreEqual(int.Parse(patchFamilySequenceId), firsCheckpointMsg.SequenceId);
        }

        [TestMethod]
        public void GetCheckPointMessageQuery_SearchForSingleNonExistingStoreItem_ReturnsEmptyList()
        {
            //Given
            var businessUnitId = 6789;
            var scanCode = "123456779";

            parameters.Region = "FL";
            parameters.BusinessUnitIds = new List<int> { businessUnitId };
            parameters.ScanCodes = new List<string> { scanCode };

            //When
            var result = getCheckPointMessageQuery.Search(parameters);

            //Then        
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void GetCheckPointMessageQuery_SearchForMultipleExistingStoreItems_ReturnsChkPtMsgForRequestedStoreItems()
        {
            //Given
            var businessUnitId_A = 6789;
            var scanCode_A = "123456779";
            var storeName_A = "TEST STORE 1";
            var itemID_A = 88997798;
            var messageID_A = "1233";
            var patchFamilySequenceId_A = "1";
            var patchFamilyId_A = $"{businessUnitId_A}-{itemID_A}";

            var businessUnitId_B = 8877;
            var scanCode_B = "123456888";
            var storeName_B = "TEST STORE 2";
            var itemID_B = 88997799;
            var messageID_B = "1234";
            var patchFamilySequenceId_B = "2";
            var patchFamilyId_B = $"{businessUnitId_B}-{itemID_B}";

            var item_A = InsertTestItemIntoDatabase(itemID_A, scanCode_A);
            InsertStoreIntoDatabase(businessUnitId_A, storeName_A);
            InsertIntoMessageSequence(itemID_A, businessUnitId_A.ToString(), patchFamilyId_A, patchFamilySequenceId_A, messageID_A, DateTime.Now, DateTime.Now);

            var item_B = InsertTestItemIntoDatabase(itemID_B, scanCode_B);
            InsertStoreIntoDatabase(businessUnitId_B, storeName_B);
            InsertIntoMessageSequence(itemID_B, businessUnitId_B.ToString(), patchFamilyId_B, patchFamilySequenceId_B, messageID_B, DateTime.Now, DateTime.Now);

            parameters.Region = "FL";
            parameters.BusinessUnitIds = new List<int> { businessUnitId_A, businessUnitId_B };
            parameters.ScanCodes = new List<string> { scanCode_A, scanCode_B };

            //When
            var result = getCheckPointMessageQuery.Search(parameters);

            //Then      
            int expectedCount = 2;
            Assert.AreEqual(expectedCount, result.Count(), $"Only {expectedCount} message sequence records were inserted for the test, so only {expectedCount} checkpoint messages should have been returned.");
            var chkPtMsg_A = result.FirstOrDefault(r=>r.BusinessUnitID==businessUnitId_A && r.ScanCode==scanCode_A);
            Assert.IsNotNull(chkPtMsg_A);
            Assert.AreEqual(item_A.ItemID, chkPtMsg_A.ItemId);
            Assert.AreEqual(businessUnitId_A, chkPtMsg_A.BusinessUnitID);
            Assert.AreEqual(patchFamilyId_A, chkPtMsg_A.PatchFamilyId);
            Assert.AreEqual(int.Parse(patchFamilySequenceId_A), chkPtMsg_A.SequenceId);

            var chkPtMsg_B = result.FirstOrDefault(r => r.BusinessUnitID == businessUnitId_B && r.ScanCode == scanCode_B);
            Assert.IsNotNull(chkPtMsg_B);
            Assert.AreEqual(item_B.ItemID, chkPtMsg_B.ItemId);
            Assert.AreEqual(businessUnitId_B, chkPtMsg_B.BusinessUnitID);
            Assert.AreEqual(patchFamilyId_B, chkPtMsg_B.PatchFamilyId);
            Assert.AreEqual(int.Parse(patchFamilySequenceId_B), chkPtMsg_B.SequenceId);
        }

        [TestMethod]
        public void GetCheckPointMessageQuery_SearchForMultipleNonExistingStoreItems_ReturnsEmptyList()
        {
            //Given
            var businessUnitId_A = 6789;
            var scanCode_A = "123456779";
            var businessUnitId_B = 8877;
            var scanCode_B = "123456888";

            parameters.Region = "FL";
            parameters.BusinessUnitIds = new List<int> { businessUnitId_A, businessUnitId_B };
            parameters.ScanCodes = new List<string> { scanCode_A, scanCode_B };

            //When
            var result = getCheckPointMessageQuery.Search(parameters);

            //Then        
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void GetCheckPointMessageQuery_SearchForMultipleStoreItemsWhenSomeExistAndSomeDoNot_ReturnsChkPtMsgForExistingOnly()
        {
            //Given
            var businessUnitId_A = 6789;
            var scanCode_A = "123456779";

            var businessUnitId_B = 8877;
            var scanCode_B = "123456888";
            var storeName_B = "TEST STORE 2";
            var itemID_B = 88997799;
            var messageID_B = "1234";
            var patchFamilySequenceId_B = "2";
            var patchFamilyId_B = $"{businessUnitId_B}-{itemID_B}";

            var item_B = InsertTestItemIntoDatabase(itemID_B, scanCode_B);
            InsertStoreIntoDatabase(businessUnitId_B, storeName_B);
            InsertIntoMessageSequence(itemID_B, businessUnitId_B.ToString(), patchFamilyId_B, patchFamilySequenceId_B, messageID_B, DateTime.Now, DateTime.Now);
            
            parameters.Region = "FL";
            parameters.BusinessUnitIds = new List<int> { businessUnitId_A, businessUnitId_B };
            parameters.ScanCodes = new List<string> { scanCode_A, scanCode_B };

            //When
            var result = getCheckPointMessageQuery.Search(parameters);

            //Then  
            Assert.AreEqual(1, result.Count());
            var chkPtMsg_B = result.FirstOrDefault(r => r.BusinessUnitID == businessUnitId_B && r.ScanCode == scanCode_B);
            Assert.IsNotNull(chkPtMsg_B);
            Assert.AreEqual(item_B.ItemID, chkPtMsg_B.ItemId);
            Assert.AreEqual(businessUnitId_B, chkPtMsg_B.BusinessUnitID);
            Assert.AreEqual(patchFamilyId_B, chkPtMsg_B.PatchFamilyId);
            Assert.AreEqual(int.Parse(patchFamilySequenceId_B), chkPtMsg_B.SequenceId);
        }

        [TestMethod]
        public void GetCheckPointMessageQuery_SearchForMultipleExistingStoreItems_ReturnsChkPtMsgForAllReferencedStoreItems()
        {
            //Given
            var businessUnitId_A = 6789;
            var scanCode_A = "123456779";
            var storeName_A = "TEST STORE 1";
            var itemID_A = 88997798;

            var businessUnitId_B = 8877;
            var scanCode_B = "123456888";
            var storeName_B = "TEST STORE 2";
            var itemID_B = 88997799;

            var item_A = InsertTestItemIntoDatabase(itemID_A, scanCode_A);
            InsertStoreIntoDatabase(businessUnitId_A, storeName_A);
            var item_B = InsertTestItemIntoDatabase(itemID_B, scanCode_B);
            InsertStoreIntoDatabase(businessUnitId_B, storeName_B);

            var messageID_A1 = "1987654321001";
            var patchFamilySequenceId_A1 = "1";
            var patchFamilyId_A1 = $"{businessUnitId_A}-{itemID_A}";

            var messageID_A2 = "1987654321002";
            var patchFamilySequenceId_A2 = "1";
            var patchFamilyId_A2 = $"{businessUnitId_A}-{itemID_B}";

            var messageID_B1 = "1987654321003";
            var patchFamilySequenceId_B1 = "1";
            var patchFamilyId_B1 = $"{businessUnitId_B}-{itemID_B}";

            var messageID_B2 = "1987654321004";
            var patchFamilySequenceId_B2 = "1";
            var patchFamilyId_B2 = $"{businessUnitId_B}-{itemID_A}";

            InsertIntoMessageSequence(itemID_A, businessUnitId_A.ToString(), patchFamilyId_A1, patchFamilySequenceId_A1, messageID_A1, DateTime.Now, DateTime.Now);
            InsertIntoMessageSequence(itemID_B, businessUnitId_A.ToString(), patchFamilyId_A2, patchFamilySequenceId_A2, messageID_A2, DateTime.Now, DateTime.Now);
            InsertIntoMessageSequence(itemID_B, businessUnitId_B.ToString(), patchFamilyId_B1, patchFamilySequenceId_B1, messageID_B1, DateTime.Now, DateTime.Now);
            InsertIntoMessageSequence(itemID_A, businessUnitId_B.ToString(), patchFamilyId_B2, patchFamilySequenceId_B2, messageID_B2, DateTime.Now, DateTime.Now);
            
            parameters.Region = "FL";
            parameters.BusinessUnitIds = new List<int> { businessUnitId_A, businessUnitId_B };
            parameters.ScanCodes = new List<string> { scanCode_A, scanCode_B };

            //When
            var result = getCheckPointMessageQuery.Search(parameters);

            //Then    
            int expectedCount = 4;
            Assert.AreEqual(expectedCount, result.Count(), $"{expectedCount} message sequence records were inserted for the test (number of items * number of stores), so {expectedCount} checkpint messages should have been returned.");

            var chkPtMsg_A1 = result.FirstOrDefault(r => r.BusinessUnitID == businessUnitId_A && r.ScanCode == scanCode_A);
            Assert.IsNotNull(chkPtMsg_A1);
            Assert.AreEqual(item_A.ItemID, chkPtMsg_A1.ItemId);
            Assert.AreEqual(businessUnitId_A, chkPtMsg_A1.BusinessUnitID);
            Assert.AreEqual(patchFamilyId_A1, chkPtMsg_A1.PatchFamilyId);
            Assert.AreEqual(int.Parse(patchFamilySequenceId_A1), chkPtMsg_A1.SequenceId);

            var chkPtMsg_A2 = result.FirstOrDefault(r => r.BusinessUnitID == businessUnitId_A && r.ScanCode == scanCode_B);
            Assert.IsNotNull(chkPtMsg_A2);
            Assert.AreEqual(item_B.ItemID, chkPtMsg_A2.ItemId);
            Assert.AreEqual(businessUnitId_A, chkPtMsg_A2.BusinessUnitID);
            Assert.AreEqual(patchFamilyId_A2, chkPtMsg_A2.PatchFamilyId);
            Assert.AreEqual(int.Parse(patchFamilySequenceId_A2), chkPtMsg_A2.SequenceId);

            var chkPtMsg_B1 = result.FirstOrDefault(r => r.BusinessUnitID == businessUnitId_B && r.ScanCode == scanCode_B);
            Assert.IsNotNull(chkPtMsg_B1);
            Assert.AreEqual(item_B.ItemID, chkPtMsg_B1.ItemId);
            Assert.AreEqual(businessUnitId_B, chkPtMsg_B1.BusinessUnitID);
            Assert.AreEqual(patchFamilyId_B1, chkPtMsg_B1.PatchFamilyId);
            Assert.AreEqual(int.Parse(patchFamilySequenceId_B1), chkPtMsg_B1.SequenceId);

            var chkPtMsg_B2 = result.FirstOrDefault(r => r.BusinessUnitID == businessUnitId_B && r.ScanCode == scanCode_A);
            Assert.IsNotNull(chkPtMsg_B2);
            Assert.AreEqual(item_A.ItemID, chkPtMsg_B2.ItemId);
            Assert.AreEqual(businessUnitId_B, chkPtMsg_B2.BusinessUnitID);
            Assert.AreEqual(patchFamilyId_B2, chkPtMsg_B2.PatchFamilyId);
            Assert.AreEqual(int.Parse(patchFamilySequenceId_B2), chkPtMsg_B2.SequenceId);
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

        private void InsertStoreIntoDatabase(int businessUnitId, string storeName)
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
        protected Item InsertTestItemIntoDatabase(int itemID, string scanCode, string itemTypeCode = "TST", string itemTypeDesc = "TEST TYPE")
        {
            var itemType = context.ItemTypes.Add(new ItemType { itemTypeCode = itemTypeCode, itemTypeDesc = itemTypeDesc });
            context.SaveChanges();
            var item = context.Items.Add(new Item { ItemID = itemID, ScanCode = scanCode, ItemTypeID = itemType.itemTypeID });
            context.SaveChanges();
            return item;
        }
    }
}