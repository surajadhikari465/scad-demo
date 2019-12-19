using Icon.Web.DataAccess.Queries;
using Icon.Web.Tests.Integration.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.SqlClient;
using System.Transactions;

namespace Icon.Web.Tests.Integration.Queries
{
    [TestClass]
    public class GetItemQueryHandlerTests
    {
        private GetItemQueryHandler queryHandler;
        private GetItemParameters parameters;
        private SqlConnection sqlConnection;
        private TransactionScope transaction;
        private ItemTestHelper itemTestHelper;

        [TestInitialize]
        public void Initialize()
        {
            transaction = new TransactionScope();
            sqlConnection = SqlConnectionBuilder.CreateIconConnection();
            queryHandler = new GetItemQueryHandler(sqlConnection);
            parameters = new GetItemParameters();
            itemTestHelper = new ItemTestHelper();
            itemTestHelper.Initialize(sqlConnection);
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Dispose();
            sqlConnection.Dispose();
        }

        [TestMethod]
        public void GetItem_ItemExists_ReturnsItem()
        {
            //Given
            parameters.ScanCode = itemTestHelper.TestItem.ScanCode;

            //When
            var result = queryHandler.Search(parameters);

            //Then
            Assert.AreEqual(itemTestHelper.TestItem.BrandsHierarchyClassId, result.BrandsHierarchyClassId);
            Assert.AreEqual(itemTestHelper.TestItem.FinancialHierarchyClassId, result.FinancialHierarchyClassId);
            Assert.AreEqual(itemTestHelper.TestItem.ItemAttributesJson, result.ItemAttributesJson);
            Assert.AreEqual(itemTestHelper.TestItem.ItemId, result.ItemId);
            Assert.AreEqual(itemTestHelper.TestItem.ItemTypeId, result.ItemTypeId);
            Assert.AreEqual(itemTestHelper.TestItem.MerchandiseHierarchyClassId, result.MerchandiseHierarchyClassId);
            Assert.AreEqual(itemTestHelper.TestItem.NationalHierarchyClassId, result.NationalHierarchyClassId);
            Assert.AreEqual(itemTestHelper.TestItem.ScanCode, result.ScanCode);
            Assert.AreEqual(itemTestHelper.TestItem.BarcodeTypeId, result.BarcodeTypeId);
            Assert.AreEqual(itemTestHelper.TestItem.TaxHierarchyClassId, result.TaxHierarchyClassId);
            Assert.AreEqual(itemTestHelper.TestItem.ManufacturerHierarchyClassId, result.ManufacturerHierarchyClassId);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetItem_ItemDoesNotExist_ThrowsException()
        {
            //Given
            parameters.ScanCode = "DoesNotExist";

            //When
            var result = queryHandler.Search(parameters);
        }

        [TestMethod]
        public void GetItem_ItemExists_ReturnsItemWithNutrition()
        {
            //Given
            itemTestHelper.CreateItemNutrition(itemTestHelper.TestItem);
            parameters.ScanCode = itemTestHelper.TestItem.ScanCode;

            //When
            var result = queryHandler.Search(parameters);

            //Then
            Assert.AreEqual(result.Nutritions["Calories"], "1");
        }
    }
}
