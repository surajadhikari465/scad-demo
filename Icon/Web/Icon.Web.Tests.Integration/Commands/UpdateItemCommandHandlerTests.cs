using Dapper;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Models;
using Icon.Web.Tests.Integration.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Transactions;

namespace Icon.Web.Tests.Integration.Commands
{
    [TestClass]
    public class UpdateItemCommandHandlerTests
    {
        private UpdateItemCommandHandler commandHandler;
        private UpdateItemCommand command;
        private SqlConnection dbConnection;
        private TransactionScope transaction;
        private ItemTestHelper itemTestHelper;

        [TestInitialize]
        public void Initialize()
        {
            transaction = new TransactionScope();
            dbConnection = SqlConnectionBuilder.CreateIconConnection();
            commandHandler = new UpdateItemCommandHandler(dbConnection);
            command = new UpdateItemCommand();
            itemTestHelper = new ItemTestHelper();
            itemTestHelper.Initialize(dbConnection);

            command.ItemId = itemTestHelper.TestItem.ItemId;
            command.ItemTypeCode = "RTL";
            command.BrandsHierarchyClassId = itemTestHelper.TestItem.BrandsHierarchyClassId;
            command.FinancialHierarchyClassId = itemTestHelper.TestItem.FinancialHierarchyClassId;
            command.ItemAttributes = JsonConvert.DeserializeObject<Dictionary<string, string>>(itemTestHelper.TestItem.ItemAttributesJson);
            command.MerchandiseHierarchyClassId = itemTestHelper.TestItem.MerchandiseHierarchyClassId;
            command.NationalHierarchyClassId = itemTestHelper.TestItem.NationalHierarchyClassId;
            command.TaxHierarchyClassId = itemTestHelper.TestItem.TaxHierarchyClassId;
            command.ManufacturerHierarchyClassId = itemTestHelper.TestItem.ManufacturerHierarchyClassId;
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Dispose();
        }

        [TestMethod]
        public void UpdateItem_DifferentMerchandiseHierarchy_UpdatesMerchandiseHierarchy()
        {
            //Given
            command.MerchandiseHierarchyClassId = itemTestHelper.TestHierarchyClasses["Merchandise"][1].HierarchyClassId;

            //When
            commandHandler.Execute(command);

            //Then
            var result = dbConnection.QueryFirst<ItemDbModel>("SELECT * FROM ItemView WHERE ItemId = @ItemId", command);
            Assert.AreEqual(result.MerchandiseHierarchyClassId, command.MerchandiseHierarchyClassId);
        }

        [TestMethod]
        public void UpdateItem_DifferentBrandsHierarchy_UpdatesBrandsHierarchy()
        {
            //Given
            command.BrandsHierarchyClassId = itemTestHelper.TestHierarchyClasses["Brands"][1].HierarchyClassId;

            //When
            commandHandler.Execute(command);

            //Then
            var result = dbConnection.QueryFirst<ItemDbModel>("SELECT * FROM ItemView WHERE ItemId = @ItemId", command);
            Assert.AreEqual(result.BrandsHierarchyClassId, command.BrandsHierarchyClassId);
        }

        [TestMethod]
        public void UpdateItem_DifferentTaxHierarchy_UpdatesTaxHierarchy()
        {
            //Given
            command.TaxHierarchyClassId = itemTestHelper.TestHierarchyClasses["Tax"][1].HierarchyClassId;

            //When
            commandHandler.Execute(command);

            //Then
            var result = dbConnection.QueryFirst<ItemDbModel>("SELECT * FROM ItemView WHERE ItemId = @ItemId", command);
            Assert.AreEqual(result.TaxHierarchyClassId, command.TaxHierarchyClassId);
        }

        [TestMethod]
        public void UpdateItem_DifferentFinancialHierarchy_UpdatesFinancialHierarchy()
        {
            //Given
            command.FinancialHierarchyClassId = itemTestHelper.TestHierarchyClasses["Financial"][1].HierarchyClassId;

            //When
            commandHandler.Execute(command);

            //Then
            var result = dbConnection.QueryFirst<ItemDbModel>("SELECT * FROM ItemView WHERE ItemId = @ItemId", command);
            Assert.AreEqual(result.FinancialHierarchyClassId, command.FinancialHierarchyClassId);
        }
        [TestMethod]
        public void UpdateItem_DifferentManufacturerHierarchy_UpdatesManufacturerHierarchy()
        {
            //Given
            command.ManufacturerHierarchyClassId = itemTestHelper.TestHierarchyClasses["Manufacturer"][1].HierarchyClassId;

            //When
            commandHandler.Execute(command);

            //Then
            var result = dbConnection.QueryFirst<ItemDbModel>("SELECT * FROM ItemView WHERE ItemId = @ItemId", command);
            Assert.AreEqual(result.ManufacturerHierarchyClassId, command.ManufacturerHierarchyClassId);

            // verify none of the other hierarchy values changed
            Assert.AreEqual(itemTestHelper.TestItem.MerchandiseHierarchyClassId, result.MerchandiseHierarchyClassId);
            Assert.AreEqual(itemTestHelper.TestItem.BrandsHierarchyClassId, result.BrandsHierarchyClassId);
            Assert.AreEqual(itemTestHelper.TestItem.TaxHierarchyClassId, result.TaxHierarchyClassId);
            Assert.AreEqual(itemTestHelper.TestItem.NationalHierarchyClassId, result.NationalHierarchyClassId);
            Assert.AreEqual(itemTestHelper.TestItem.FinancialHierarchyClassId, result.FinancialHierarchyClassId);
        }

        [TestMethod]
        public void UpdateItem_ManufacturerHierarchyIsRemoved_UpdatesManufacturerHierarchyToNotExist()
        {
            //Given
            command.ManufacturerHierarchyClassId = 0;

            //When
            commandHandler.Execute(command);

            //Then
            var result = dbConnection.QueryFirst<ItemDbModel>("SELECT * FROM ItemView WHERE ItemId = @ItemId", command);
            Assert.AreEqual(0, command.ManufacturerHierarchyClassId);
            var manufacturerHierarchyClassIdExists = dbConnection.QueryFirstOrDefault<bool>("SELECT 1 WHERE EXISTS(SELECT * FROM dbo.ItemHierarchyClass ihc WHERE ihc.HierarchyClassId=@hierarchyClassId)", new { hierarchyClassId = result.ManufacturerHierarchyClassId });
            Assert.IsFalse(manufacturerHierarchyClassIdExists);
        }


        [TestMethod]
        public void UpdateItem_DifferentNationalHierarchy_UpdatesNationalHierarchy()
        {
            //Given
            command.NationalHierarchyClassId = itemTestHelper.TestHierarchyClasses["National"][1].HierarchyClassId;

            //When
            commandHandler.Execute(command);

            //Then
            var result = dbConnection.QueryFirst<ItemDbModel>("SELECT * FROM ItemView WHERE ItemId = @ItemId", command);
            Assert.AreEqual(result.NationalHierarchyClassId, command.NationalHierarchyClassId);
        }

        [TestMethod]
        public void UpdateItem_DifferentItemAttributesJsonHierarchy_UpdatesItemAttributesJsonHierarchy()
        {
            //Given
            command.ItemAttributes = new Dictionary<string, string>
            {
                {"Test1Prop", "Test1Value" },
                {"Test2Prop", "Test2Value" }
            };

            //When
            commandHandler.Execute(command);

            //Then
            var result = dbConnection.QueryFirst<ItemDbModel>("SELECT * FROM ItemView WHERE ItemId = @ItemId", command);
            Assert.AreEqual(result.ItemAttributesJson, @"{""Test1Prop"":""Test1Value"",""Test2Prop"":""Test2Value""}");
        }
    }
}
