using System;
using System.Collections.Generic;
using Icon.Web.DataAccess.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Icon.Shared.DataAccess.Dapper.DbProviders;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using Dapper;
using Icon.Common.DataAccess;
using Icon.Common.Models;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;

namespace Icon.Web.Tests.Integration.Commands
{

    [TestClass]
    public class UpdateItemColumnOrderCommandHandlerTests
    {
        private IDbConnection dbConnection;
        private IDbTransaction transaction;  
        private UpdateItemColumnOrderCommandHandler updateItemColumnOrderCommandHandler;


        [TestInitialize]
        public void Initialize()    
        {

            dbConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Icon"].ConnectionString);
            dbConnection.Open();
            transaction = dbConnection.BeginTransaction();
            updateItemColumnOrderCommandHandler = new UpdateItemColumnOrderCommandHandler(new SqlDbProvider {Connection = dbConnection, Transaction = transaction} );
        }
        [TestMethod]
        public void UpdateItemColumnOrderCommandHandler_Update1OrderValue_DBValueIncremented()
        {
            var displayOrderData =
                GetItemColumnOrderData(dbConnection, transaction);

            var randomItem = displayOrderData.OrderBy(x => Guid.NewGuid()).Take(1).Single();
            var originalDisplayOrder = randomItem.DisplayOrder;
            var newDisplayOrder = originalDisplayOrder + 1;

            randomItem.DisplayOrder = newDisplayOrder;

            var command = new UpdateItemColumnOrderCommand
            {
                DisplayData = new List<ItemColumnOrderModel>() {randomItem}
            };
            updateItemColumnOrderCommandHandler.Execute(command);


            var updatedOrderData = GetItemColumnOrderData(dbConnection, transaction);
            var updatedItem = updatedOrderData.Single(s =>
                s.ReferenceId == randomItem.ReferenceId && s.ColumnType == randomItem.ColumnType);

            Assert.AreEqual(newDisplayOrder, updatedItem.DisplayOrder);
        }

        private List<ItemColumnOrderModel> GetItemColumnOrderData(IDbConnection db, IDbTransaction tran)
        {
            var sql = @"
               select  ColumnType, 
                        ReferenceId, 
                        h.HierarchyName ReferenceName,
                        d.DisplayOrder 
                from dbo.ItemDisplayOrder d
                inner join dbo.Hierarchy h on d.ReferenceId = h.HierarchyId
				where d.ColumnType = 'Hierarchy'

                union all 

                select  ColumnType, 
                        ReferenceId, 
                        a.AttributeName ReferenceName,
                        d.DisplayOrder 
                from dbo.ItemDisplayOrder d
                inner join dbo.Attributes a on d.ReferenceId = a.AttributeId
				where d.ColumnType = 'Attribute'
                order by DisplayOrder
            ";

           return db.Query<ItemColumnOrderModel>(sql, transaction: tran).ToList();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
            transaction.Dispose();
            dbConnection.Close();
            dbConnection.Dispose();

        }
    }

    [TestClass]
    public class UpdateAttributeCommandHandlerTests
    {
        private UpdateAttributeCommandHandler commandHandler;
        private IDbProvider db;

        [TestInitialize]
        public void Initialize()
        {
            this.db = new SqlDbProvider();
            string connectionString = ConfigurationManager.ConnectionStrings["Icon"].ConnectionString;
            this.db.Connection = new SqlConnection(connectionString);
            this.db.Connection.Open();
            this.db.Transaction = this.db.Connection.BeginTransaction();
            this.commandHandler = new UpdateAttributeCommandHandler(this.db);
            InsertDataType();
        }

        [TestCleanup]
        public void Cleanup()
        {
            this.db.Transaction.Rollback();
            this.db.Transaction.Dispose();
        }

        [TestMethod]
        public void EditAttribute_SuccessfulExecution_AttributeShouldBeUpdatedToTheDatabase()
        {
            // Given.
            int dataTypeId = GetNewDataTypeId();
            InsertAttribute(dataTypeId);
            int newAttributeId = GetNewAttributeId();

            AttributeModel attributeModel = BuildAttributeModel(newAttributeId, dataTypeId);

            var command = new UpdateAttributeCommand
            {
                AttributeModel = attributeModel
            };

            // When.
            commandHandler.Execute(command);

            // Then.
            var updateAttribute = GetAttributeById(attributeModel.AttributeId);
            if (updateAttribute != null)
            {
                Assert.AreEqual(updateAttribute.DisplayName, attributeModel.DisplayName);
                Assert.IsNotNull(updateAttribute.LastModifiedDate);
                Assert.AreEqual(updateAttribute.LastModifiedBy, attributeModel.LastModifiedBy);
            }
            else
            {
                Assert.Fail("Record Not Found");
            }

         

        }

        private AttributeModel GetAttributeById(int attributeId)
        {
            //string sql = @"SELECT * from attributes where attributeId = @attributeId";
            string sql = @"SELECT *, a.SysStartTimeUtc LastModifiedDate 
                            FROM dbo.Attributes a 
                        where a.attributeId = @attributeId";

            AttributeModel attributeModel = this.db.Connection.Query<AttributeModel>(sql, new { attributeId = attributeId }, transaction: this.db.Transaction).FirstOrDefault();

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

        private void InsertAttribute(int dataTypeId)
        {
            string sql = @"
                        INSERT INTO [dbo].[Attributes]
                                   ([DisplayName]
                                   ,[AttributeName]
                                   ,[Description]             
                                   ,[TraitCode]
                                   ,[DataTypeId]
                                  
                                   ,[IsPickList]
                                   ,[LastModifiedBy])
                            VALUES
                            (
	                            'Today',
                                'Today',
                                'Today', 
                                'Today',
                                 @DataTypeId,
                                0,
                                'TestUser'
                            )";

            int affectedRows = this.db.Connection.Execute(sql, new { DataTypeId = dataTypeId }, transaction: this.db.Transaction);
        }

        private int GetNewAttributeId()
        {
            string sql = @"SELECT TOP 1 AttributeId from Attributes where DisplayName = 'Today'";
            int newAttributeId = this.db.Connection.Query<int>(sql, transaction: this.db.Transaction).First();

            return newAttributeId;
        }

        private AttributeModel BuildAttributeModel(int newAttributeId, int dataTypeId)
        {
            return new AttributeModel
            {
                AttributeId = newAttributeId,
                AttributeName = "today",
                DisplayName = "today",
                TraitCode = "today",
                DataTypeId = dataTypeId,
                LastModifiedBy = "NewTestUser"
            };
        }
    }
}