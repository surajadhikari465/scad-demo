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
    public class AddUpdateCharacterSetCommandHandlerTests
    {
        private AddUpdateCharacterSetCommandHandler commandHandler;
        private IDbProvider db;
        private List<int> characterSetIds;

        [TestInitialize]
        public void Initialize()
        {
            this.db = new SqlDbProvider();
            string connectionString = ConfigurationManager.ConnectionStrings["Icon"].ConnectionString;
            this.db.Connection = new SqlConnection(connectionString);
            this.db.Connection.Open();

            this.db.Transaction = this.db.Connection.BeginTransaction();
            commandHandler = new AddUpdateCharacterSetCommandHandler(this.db);
            InsertDataType();
            BuildAttributeModel();
            InsertAttribute();
            InsertCharacterSets("test1", "test1");
            InsertCharacterSets("test2", "test2");
            characterSetIds = new List<int>();
            characterSetIds.Add(GetCharacterSetId("test1"));
            characterSetIds.Add(GetCharacterSetId("test1"));
        }


        [TestCleanup]
        public void Cleanup()
        {
            this.db.Transaction.Rollback();
            this.db.Transaction.Dispose();
        }

        [TestMethod]
        public void AddCharacterSets_SuccessfulExecution_CharacterSetsShouldBeAddedToTheDatabase()
        {
            // Given.
            List<CharacterSetModel> characterSetModelList;
            var newAttribute = GetAttributeByName("test1234");

            characterSetModelList = BuildCharacterSetModelList(characterSetIds);

            var command = new AddUpdateCharacterSetCommand
            {
                AttributeId = newAttribute.AttributeId,
                CharacterSetModelList = characterSetModelList
            };

            // When.
            commandHandler.Execute(command);
            int count = GetCharacterSetIdByAttributeId(newAttribute.AttributeId);

            // Then.
            Assert.AreEqual(count, characterSetIds.Count);
        }

        private int GetCharacterSetIdByAttributeId(int attributeId)
        {
            string sql = @"SELECT count(*) from AttributeCharacterSets where attributeId = @attributeId";

            int count = this.db.Connection.Query<int>(sql, new { attributeId = attributeId }, transaction: this.db.Transaction).First();

            return count;
        }

        private int GetCharacterSetId(string name)
        {
            string sql = @"SELECT top 1 characterSetId from characterSets where name = @name";

            int characterSetId = this.db.Connection.Query<int>(sql, new { name = name }, transaction: this.db.Transaction).First();

            return characterSetId;
        }

        private List<CharacterSetModel> BuildCharacterSetModelList(List<int> characterSetIds)
        {
            List<CharacterSetModel> characterSetModels = new List<CharacterSetModel>
            {
                new CharacterSetModel{Name="Char1", RegEx="Char1",CharacterSetId= characterSetIds[0]},
                new CharacterSetModel{Name="Char2", RegEx="Char2",CharacterSetId =characterSetIds[1] }
            };

            return characterSetModels;
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

        private void InsertCharacterSets(string name, string regex)
        {
            string sql = @"
                        INSERT INTO [dbo].[CharacterSets]
                                   ([Name]
                                   ,[RegEx])
                            VALUES
                            (
	                            @name,
                                @regex
                            )";
            int affectedRows = this.db.Connection.Execute(sql, new { name = name, regex = regex }, transaction: this.db.Transaction);
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
    }
}
