using Icon.Web.DataAccess.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Icon.Shared.DataAccess.Dapper.DbProviders;
using System.Data.SqlClient;
using System.Configuration;
using Dapper;
using Icon.Common.Models;

namespace Icon.Web.Tests.Integration.Commands
{
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
            AttributeModel attributeModel = BuildAttributeModel();

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
            }
            else
            {
                Assert.Fail("Record Not Found");
            }
            

        }

        private AttributeModel GetAttributeById(int attributeId)
        {
            string sql = @"SELECT * from attributes where attributeId = @attributeId";

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

        private AttributeModel BuildAttributeModel()
        {
            return new AttributeModel
            {
                AttributeId = 518,
                AttributeName = "today",
                DisplayName = "today",
                TraitCode = "today",
                DataTypeId = GetNewDataTypeId()
            };
        }
    }
}