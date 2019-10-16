using Dapper;
using Mammoth.Common.DataAccess.Models;
using Mammoth.Esb.ProductListener.Queries;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;

namespace Mammoth.Esb.ProductListener.Tests.Queries
{
    [TestClass]
    public class GetItemsQueryHandlerTests
    {
        private GetItemsQueryHandler query;
        private GetItemsParameters parameters;
        private SqlConnection sqlConnection;
        private TransactionScope transaction;
        private int testItemTypeId;
        private const string TestItemTypeCode = "TST";

        [TestInitialize]
        public void Initialize()
        {
            transaction = new TransactionScope();
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Mammoth"].ConnectionString);
            query = new GetItemsQueryHandler(sqlConnection);
            parameters = new GetItemsParameters();

            testItemTypeId = InsertTestItemType();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Dispose();
        }

        [TestMethod]
        public void GetItems_ItemsExist_ReturnsItems()
        {
            //Given
            List<int> itemIds = new List<int> { 99999999, 88888888, 77777777 };
            string expectedRetailSize = "8";
            string expectedPackageUnit = "9";
            CreateItems(itemIds, expectedRetailSize);
            parameters.ItemIds = itemIds;

            //When
            var results = query.Search(parameters);

            //Then
            Assert.AreEqual(3, results.Count());
            foreach (var itemId in itemIds)
            {
                var item = results.Single(i => i.ItemID == itemId);
                Assert.IsNotNull(item.AddedDate);
                Assert.AreEqual(12, item.BrandHCID);
                Assert.AreEqual("Desc_CustomerFriendly" + item.ScanCode, item.Desc_CustomerFriendly);
                Assert.AreEqual("Desc_POS" + item.ScanCode, item.Desc_POS);
                Assert.AreEqual("Desc_Product" + item.ScanCode, item.Desc_Product);
                Assert.AreEqual(true, item.FoodStampEligible);
                Assert.AreEqual(12345, item.HierarchyMerchandiseID);
                Assert.AreEqual(6789, item.HierarchyNationalClassID);
                Assert.AreEqual(TestItemTypeCode, item.ItemTypeCode);
                Assert.AreEqual(testItemTypeId, item.ItemTypeID);
                Assert.IsNotNull(item.ModifiedDate);
                Assert.AreEqual(expectedPackageUnit, item.PackageUnit);
                Assert.AreEqual(expectedRetailSize, item.RetailSize);
                Assert.AreEqual("EA", item.RetailUOM);
                Assert.AreEqual("sc" + itemId, item.ScanCode);
                Assert.AreEqual(13, item.TaxClassHCID);
                Assert.IsNotNull(item.AddedDate);
            }
        }

        [TestMethod]
        public void GetItems_ItemsExistRetailSizeHasDecimal_ReturnsItems()
        {
            //Given
            List<int> itemIds = new List<int> { 99999999, 88888888, 77777777 };
            string expectedRetailSize = "4.8";
            string expectedPackageUnit = "9";
            CreateItems(itemIds, expectedRetailSize);
            parameters.ItemIds = itemIds;

            //When
            var results = query.Search(parameters);

            //Then
            Assert.AreEqual(3, results.Count());
            foreach (var itemId in itemIds)
            {
                var item = results.Single(i => i.ItemID == itemId);
                Assert.IsNotNull(item.AddedDate);
                Assert.AreEqual(12, item.BrandHCID);
                Assert.AreEqual("Desc_CustomerFriendly" + item.ScanCode, item.Desc_CustomerFriendly);
                Assert.AreEqual("Desc_POS" + item.ScanCode, item.Desc_POS);
                Assert.AreEqual("Desc_Product" + item.ScanCode, item.Desc_Product);
                Assert.AreEqual(true, item.FoodStampEligible);
                Assert.AreEqual(12345, item.HierarchyMerchandiseID);
                Assert.AreEqual(6789, item.HierarchyNationalClassID);
                Assert.AreEqual(TestItemTypeCode, item.ItemTypeCode);
                Assert.AreEqual(testItemTypeId, item.ItemTypeID);
                Assert.IsNotNull(item.ModifiedDate);
                Assert.AreEqual(expectedPackageUnit, item.PackageUnit);
                Assert.AreEqual(expectedRetailSize, item.RetailSize);
                Assert.AreEqual("EA", item.RetailUOM);
                Assert.AreEqual("sc" + itemId, item.ScanCode);
                Assert.AreEqual(13, item.TaxClassHCID);
                Assert.IsNotNull(item.AddedDate);
            }
        }

        [TestMethod]
        public void GetItems_ItemsDontExist_ReturnsEmptyList()
        {
            //Given
            parameters.ItemIds = new List<int>();

            //When
            var results = query.Search(parameters);

            //Then
            Assert.IsFalse(results.Any());
        }

        private void CreateItems(List<int> itemIds, string retailSize)
        {
            foreach (int itemId in itemIds)
            {

               

                sqlConnection.Execute(
                 $@" INSERT INTO dbo.Items(
                            [ItemID]
                           ,[ItemTypeID]
                           ,[ScanCode]
                           ,[HierarchyMerchandiseID]
                           ,[HierarchyNationalClassID]
                           ,[BrandHCID]
                           ,[TaxClassHCID]
                           ,[PSNumber]
                           ,[Desc_Product]
                           ,[Desc_POS]
                           ,[PackageUnit]
                           ,[RetailSize]
                           ,[RetailUOM]
                           ,[FoodStampEligible]
                           ,[Desc_CustomerFriendly]
                           ,[ModifiedDate])
                        SELECT TOP 1 @ItemId, 
                            itemTypeID,
                            @ScanCode, 
                            12345,
                            6789,
                            12,
                            13,
                            1111,
                            'Desc_Product' + @ScanCode,
                            'Desc_POS' + @ScanCode,
                            9,
                            @RetailSize,
                            'EA',
                            1,
                            'Desc_CustomerFriendly' + @ScanCode,
                            GETDATE()
                        FROM dbo.ItemTypes 
                        WHERE itemTypeCode = @ItemTypeCode",
                 new { ItemId = itemId, ScanCode = "sc" + itemId, ItemTypeCode = TestItemTypeCode, RetailSize = retailSize });
            }   
        }

        private int InsertTestItemType()
        {
            return sqlConnection.QueryFirst<int>(
                @"  INSERT INTO dbo.ItemTypes(itemTypeCode) 
                    VALUES (@ItemTypeCode)
                    SELECT SCOPE_IDENTITY()",
                new { ItemTypeCode = TestItemTypeCode });
        }
    }
}