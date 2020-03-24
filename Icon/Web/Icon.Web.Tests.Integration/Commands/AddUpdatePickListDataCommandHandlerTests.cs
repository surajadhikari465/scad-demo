using Icon.Web.DataAccess.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Icon.Shared.DataAccess.Dapper.DbProviders;
using System.Data.SqlClient;
using System.Configuration;
using Dapper;
using System.Collections.Generic;
using Icon.Common.Models;


namespace Icon.Web.Tests.Integration.Commands
{
    [TestClass]
    public class AddUpdatePickListDataCommandHandlerTests
    {
        private AddUpdatePickListDataCommandHandler commandHandler;
        private IDbProvider db;
        AttributeModel attributeModel;

        [TestInitialize]
        public void Initialize()
        {
            this.db = new SqlDbProvider();
            string connectionString = ConfigurationManager.ConnectionStrings["Icon"].ConnectionString;
            this.db.Connection = new SqlConnection(connectionString);
            this.db.Connection.Open();

            this.db.Transaction = this.db.Connection.BeginTransaction();
            commandHandler = new AddUpdatePickListDataCommandHandler(this.db);
            InsertDataType();
            BuildAttributeModel();
            InsertAttribute();
        }


        [TestCleanup]
        public void Cleanup()
        {
            this.db.Transaction.Rollback();
            this.db.Transaction.Dispose();
        }

        [TestMethod]
        public void AddPickLists_SuccessfulExecution_PickListShouldBeAddedToTheDatabase()
        {
            // Given.
            List<PickListModel> pickListModel;
            attributeModel = GetAttributeByName("test1234");
            pickListModel = BuildPickListModel(attributeModel.AttributeId);

            var command = new AddUpdatePickListDataCommand
            {
                AttributeId = attributeModel.AttributeId,
                PickListModel = pickListModel
            };

            // When.
            commandHandler.Execute(command);
            int count = GetPickListCountByAttributeId(attributeModel.AttributeId);

            // Then.
            Assert.AreEqual(count, pickListModel.Count);
        }

        private int GetPickListCountByAttributeId(int attributeId)
        {
            string sql = @"SELECT Count(*) from PickListData where attributeId = @attributeId";

            int countOfPickList = this.db.Connection.Query<int>(sql, new { attributeId = attributeId }, transaction: this.db.Transaction).First();

            return countOfPickList;
        }

        private List<PickListModel> BuildPickListModel(int attributeId)
        {
            List<PickListModel> pickListModels = new List<PickListModel>
            {
                new PickListModel{AttributeId=attributeId, PickListValue="PickListValue1"},
                new PickListModel{AttributeId=attributeId, PickListValue="PickListValue2" }
            };

            return pickListModels;
        }

        private AttributeModel GetAttributeByName(string name)
        {
            string sql = @"SELECT * from attributes where attributeName = @name";
            AttributeModel attributeModel = this.db.Connection.Query<AttributeModel>(sql, new { name = name }, transaction: this.db.Transaction).First();

            return attributeModel;
        }

        private void InsertDataType()
        {
            string sql = @"INSERT INTO DataType
                            (
	                            DataType
                            )
                            VALUES
                            (
	                            'test'
                            )";
            int affectedRows = this.db.Connection.Execute(sql, transaction: this.db.Transaction);
        }

        private int GetNewDataTypeId()
        {
            string sql = @"SELECT TOP 1 DataTypeId from DataType where DataType = 'test'";
            int newDataTypeId = this.db.Connection.Query<int>(sql, transaction: this.db.Transaction).First();

            return newDataTypeId;
        }

        private void InsertAttribute()
        {
            string sql = @"
                        INSERT INTO [dbo].[Attributes]
                                   ([DisplayName]
                                   ,[AttributeName]
                                   ,[Description]             
                                   ,[TraitCode]
                                   ,[DataTypeId]
                                  
                                   ,[IsPickList])
                            VALUES
                            (
	                            'test1234',
                                'test1234',
                                'test 1234', 
                                'test1234',
                                 @DataTypeId,
                                0
                            )";
            int affectedRows = this.db.Connection.Execute(sql, new { DataTypeId = GetNewDataTypeId() }, transaction: this.db.Transaction);
        }

        private void InsertPickList(int attributeId, string pickListValue)
        {
            string sql = @"
                        INSERT INTO [dbo].[PickListData]
                                   ([AttributeId]
                                   ,[PickListValue])
                            VALUES
                            (
	                            @attributeId,
                                @pickListValue
                            )";
            int affectedRows = this.db.Connection.Execute(sql, new { attributeId = attributeId, pickListValue = pickListValue }, transaction: this.db.Transaction);
        }

        private AttributeModel BuildAttributeModel()
        {
            return new AttributeModel
            {
                AttributeName = "test1234",
                DisplayName = "test1234",
                TraitCode = "test1234",
                DataTypeId = GetNewDataTypeId()
            };
        }

        [TestMethod]
        public void UpdatePickLists_SuccessfulExecution_PickListShouldBeUpdatedToTheDatabase()
        {
            List<PickListModel> pickListModel;
            attributeModel = GetAttributeByName("test1234");
            pickListModel = BuildPickListModel(attributeModel.AttributeId);

            var command = new AddUpdatePickListDataCommand
            {
                AttributeId = attributeModel.AttributeId,
                PickListModel = pickListModel
            };

            // When.
            commandHandler.Execute(command);
            int updatedCount = GetPickListCountByAttributeId(attributeModel.AttributeId);
            
                // Then.
                Assert.AreEqual(updatedCount, pickListModel.Count);
        }
    }
}