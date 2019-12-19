using Icon.Web.DataAccess.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Icon.Shared.DataAccess.Dapper.DbProviders;
using System.Data.SqlClient;
using System.Configuration;
using Icon.Web.DataAccess.Models;
using Dapper;
using Icon.Common.Models;
using System.Collections.Generic;

namespace Icon.Web.Tests.Integration.Commands
{
    [TestClass]
    public class AddAttributeCommandHandlerTests
    {
        private AddAttributeCommandHandler commandHandler;
        private AddUpdatePickListDataCommandHandler commandPickListHandler;
        private IDbProvider db;

        [TestInitialize]
        public void Initialize()
        {
            this.db = new SqlDbProvider();
            string connectionString = ConfigurationManager.ConnectionStrings["Icon"].ConnectionString;
            this.db.Connection = new SqlConnection(connectionString);
            this.db.Connection.Open();
            this.db.Transaction = this.db.Connection.BeginTransaction();
            this.commandHandler = new AddAttributeCommandHandler(this.db);
            this.commandPickListHandler = new AddUpdatePickListDataCommandHandler(this.db);
            InsertDataType();
        }

        [TestCleanup]
        public void Cleanup()
        {
            this.db.Transaction.Rollback();
            this.db.Transaction.Dispose();
        }

        [TestMethod]
        public void AddAttribute_SuccessfulExecution_AttributeShouldBeAddedToTheDatabase()
        {
            // Given.
            AttributeModel attributeModel = BuildAttributeModel();

            var command = new AddAttributeCommand
            {
                AttributeModel = attributeModel
            };

            // When.
            commandHandler.Execute(command);

            // Then.
            var newAttribute = GetAttributeByName("test1234");

            Assert.AreEqual(newAttribute.TraitCode, attributeModel.TraitCode);

            var webConfiguration = GetAttributesWebConfiguration(newAttribute);
            Assert.AreEqual(200, webConfiguration.GridColumnWidth);
            Assert.IsFalse(webConfiguration.ReadOnly);
        }

        [TestMethod]
        [ExpectedException(typeof(SqlException))]
        public void AddAttribute_DuplicateName_ExceptionShouldBeThrown()
        {
             AttributeModel attributeModel = BuildAttributeModel();

            var command = new AddAttributeCommand
            {
                AttributeModel = attributeModel
            };

            var duplicateCommand = new AddAttributeCommand
            {
                AttributeModel = attributeModel
            };

            // When.
            commandHandler.Execute(command);
            commandHandler.Execute(duplicateCommand);

        }

        private AttributeModel GetAttributeByName(string name)
        {
            string sql = @"SELECT * from attributes where attributeName = @name";

            AttributeModel attributeModel = this.db.Connection.Query<AttributeModel>(sql, new { name = name }, transaction: this.db.Transaction).First();

            return attributeModel;
        }

        private List<PickListModel> GetAttributePickListByAttributeId(int attributeId)
        {
            string sql = $"SELECT * from PickListData where AttributeId = {attributeId}";
            return this.db.Connection.Query<PickListModel>(sql, transaction: this.db.Transaction).ToList();
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

        private AttributesWebConfigurationModel GetAttributesWebConfiguration(AttributeModel attribute)
        {
            string sql = "SELECT * FROM dbo.AttributesWebConfiguration WHERE AttributeId = @attributeId";

            return this.db.Connection.QuerySingle<AttributesWebConfigurationModel>(sql, attribute, transaction: this.db.Transaction);
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
        public void AddAttribute_SuccessfulExecution_AttributeWithPickListShouldBeAddedToTheDatabase()
        {
            // Given.
            var attributeModel = CreateAttributeWithPickList();

            var pickListCommand = new AddUpdatePickListDataCommand
            {
                AttributeId = attributeModel.AttributeId,
                PickListModel = attributeModel.PickListData.Select(x => new PickListModel() { PickListValue = x.PickListValue }).ToList()
            };

            // When.
            commandPickListHandler.Execute(pickListCommand);

            // Then.
            var pickList = GetAttributePickListByAttributeId(attributeModel.AttributeId);

            Assert.IsTrue(pickList.Count() == attributeModel.PickListData.Count());
            foreach(var val in attributeModel.PickListData)
            {
                Assert.IsTrue(pickList.Any(x => x.PickListValue == val.PickListValue));
            }
        }

        [TestMethod]
        public void AddAttribute_DeleteFromPickList_ShouldDeleteOneValueFromPickList()
        {
            // Given.
            var attributeModel = CreateAttributeWithPickList();

            var pickListCommand = new AddUpdatePickListDataCommand
            {
                AttributeId = attributeModel.AttributeId,
                PickListModel = attributeModel.PickListData.Select(x => new PickListModel() { PickListValue = x.PickListValue }).Take(1).ToList()
            };

            // When.
            commandPickListHandler.Execute(pickListCommand);

            // Then.
            var pickList = GetAttributePickListByAttributeId(attributeModel.AttributeId);

            Assert.IsTrue(pickList.Count() == 1);
            Assert.IsTrue(pickList.Any(x => x.PickListValue == pickListCommand.PickListModel.First().PickListValue));
        }

        [TestMethod]
        public void AddAttribute_AddToPickList_ShouldAddOneValueToPickList()
        {
            // Given.
            var attributeModel = CreateAttributeWithPickList();
            var originalPickList = attributeModel.PickListData.Select(x => new PickListModel() { PickListValue = x.PickListValue }).ToList();
            var newPickList = attributeModel.PickListData.Select(x => new PickListModel() { PickListValue = x.PickListValue }).ToList();
            newPickList.Add(new PickListModel{ PickListValue = "C" });

            var pickListCommand = new AddUpdatePickListDataCommand
            {
                AttributeId = attributeModel.AttributeId,
                PickListModel = originalPickList 
            };

            // When.
            commandPickListHandler.Execute(pickListCommand);
            pickListCommand.PickListModel = newPickList;
            commandPickListHandler.Execute(pickListCommand);

            // Then.
            var pickList = GetAttributePickListByAttributeId(attributeModel.AttributeId);

            Assert.IsTrue(newPickList.Count() - originalPickList.Count() == 1);

            foreach(var val in originalPickList)
            {
                Assert.IsTrue(newPickList.Any(x => x.PickListValue == val.PickListValue));
            }

            Assert.IsTrue(pickList.Any(x => x.PickListValue == "C"));
        }

        AttributeModel CreateAttributeWithPickList()
        {
            var model = BuildAttributeModel();
            model.IsPickList = true;
            model.PickListData = new List<PickListModel>()
                {
                    new PickListModel{ PickListValue = "A" },
                    new PickListModel{ PickListValue = "B" }
                };

            var command = new AddAttributeCommand
            {
                AttributeModel = model
            };

            commandHandler.Execute(command);
            model.AttributeId = GetAttributeByName("test1234").AttributeId;
            return model;
        }
    }
}
