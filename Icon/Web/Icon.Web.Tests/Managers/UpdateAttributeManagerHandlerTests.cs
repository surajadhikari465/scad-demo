using Dapper;
using Icon.Common.DataAccess;
using Icon.Shared.DataAccess.Dapper.DbProviders;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Managers;
using Icon.Web.Mvc.App_Start;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using Icon.Common.Models;

namespace Icon.Web.Tests.Unit.Managers
{
    [TestClass]
    public class UpdateAttributeManagerHandlerTests
    {
        private UpdateAttributeManagerHandler managerHandler;
        private Mock<ICommandHandler<UpdateAttributeCommand>> mockUpdateAttributeCommand;
        private Mock<ICommandHandler<AddUpdateCharacterSetCommand>> mockAddUpdateCharacterSetCommand;
        private Mock<ICommandHandler<AddUpdatePickListDataCommand>> mockAddUpdatePickListDataCommand;
        private Mock<ICommandHandler<AddMissingColumnsToItemColumnDisplayTableCommand>> mockAddMissingColumnsToItemColumnDisplayTableCommand;
        private IDbProvider db;
        private AttributeModel attribute;
        private List<CharacterSetModel> characterSetModelList;
        private List<PickListModel> pickListModel;
        private List<int> characterSetIds;

        [TestInitialize]
        public void Initialize()
        {
            this.db = new SqlDbProvider();
            string connectionString = ConfigurationManager.ConnectionStrings["Icon"].ConnectionString;
            this.db.Connection = new SqlConnection(connectionString);
            this.db.Connection.Open();
            this.db.Transaction = this.db.Connection.BeginTransaction();

            mockUpdateAttributeCommand = new Mock<ICommandHandler<UpdateAttributeCommand>>();
            mockAddUpdateCharacterSetCommand = new Mock<ICommandHandler<AddUpdateCharacterSetCommand>>();
            mockAddUpdatePickListDataCommand = new Mock<ICommandHandler<AddUpdatePickListDataCommand>>();
            mockAddMissingColumnsToItemColumnDisplayTableCommand = new Mock<ICommandHandler<AddMissingColumnsToItemColumnDisplayTableCommand>>();

            AutoMapperWebConfiguration.Configure();
            BuildModel();
        }

        private void BuildAttributeManagerHandler()
        {
            managerHandler = new UpdateAttributeManagerHandler(mockUpdateAttributeCommand.Object, 
                mockAddUpdateCharacterSetCommand.Object, 
                mockAddUpdatePickListDataCommand.Object, 
                mockAddMissingColumnsToItemColumnDisplayTableCommand.Object);
        }

        [TestMethod]
        public void UpdateAttribute_SuccessfulExecution_AllCommandsShouldBeCalled()
        {
            // Given.
            BuildAttributeManagerHandler();

            var manager = GetUpdateAttributeManager(attribute, characterSetModelList, pickListModel);

            // When.
            managerHandler.Execute(manager);

            // Then.
            mockUpdateAttributeCommand.Verify(c => c.Execute(It.IsAny<UpdateAttributeCommand>()), Times.Once);
            mockAddUpdateCharacterSetCommand.Verify(c => c.Execute(It.IsAny<AddUpdateCharacterSetCommand>()), Times.Once);
            mockAddUpdatePickListDataCommand.Verify(c => c.Execute(It.IsAny<AddUpdatePickListDataCommand>()), Times.Once);
            mockAddMissingColumnsToItemColumnDisplayTableCommand.Verify(c => c.Execute(It.IsAny<AddMissingColumnsToItemColumnDisplayTableCommand>()), Times.Once);
        }

        private UpdateAttributeManager GetUpdateAttributeManager(AttributeModel attribute, List<CharacterSetModel> characterSetModelList, List<PickListModel> pickListModel)
        {
            return new UpdateAttributeManager()
            {
                Attribute = attribute,
                CharacterSetModelList = characterSetModelList,
                PickListModel = pickListModel
            };
        }

        private void BuildModel()
        {
            InsertDataType();
            BuildAttributeModel();
            UpdateAttribute();
            InsertCharacterSets("test1", "test1");
            InsertCharacterSets("test2", "test2");
            characterSetIds = new List<int>();
            characterSetIds.Add(GetCharacterSetId("test1"));
            characterSetIds.Add(GetCharacterSetId("test1"));
            characterSetModelList = BuildCharacterSetModelList(characterSetIds);
            attribute = GetAttributeByName("test1234");
            pickListModel = BuildPickListModel(attribute.AttributeId);
        }

        private int GetCharacterSetIdByAttributeId(int attributeId)
        {
            string sql = @"SELECT count(*) from AttributeCharacterSets where attributeId = @attributeId";

            int count = this.db.Connection.Query<int>(sql, new { attributeId = attributeId }, transaction: this.db.Transaction).First();

            return count;
        }

        private List<PickListModel> BuildPickListModel(int attributeId)
        {
            List<PickListModel> pickListModels = new List<PickListModel>
            {
                new PickListModel{AttributeId=attributeId, PickListValue="PickListValue1"},
                new PickListModel{AttributeId=attributeId, PickListValue="PickListValue1" }
            };

            return pickListModels;
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

        private void UpdateAttribute()
        {
            string sql = @"
             UPDATE [dbo].[Attributes]
		     SET [DisplayName] = 'test1234'
			,[Description] ='test1234'
		    ,[AttributeName] = 'test1234'			
			,[MaxLengthAllowed] = 1
			,[IsPickList] = 1
			,[SpecialCharactersAllowed] = 'All'
            ,[IsRequired] = 0
            ,[IsActive] = 0
             WHERE [AttributeId]=@AttributeId";
            int affectedRows = this.db.Connection.Execute(sql, new { AttributeId = 123 }, transaction: this.db.Transaction);
        }

        private void InsertCharacterSets(string name, string regex)
        {
            string sql = @"
                        INSERT INTO [dbo].[CharacterSets]
                                   (    
                                        [Name]
                                       ,[RegEx]
                                   )
                                  VALUES
                                  (
	                                    @name
                                       ,@regex
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
                IsPickList = true,
                DataTypeId = GetNewDataTypeId()
            };
        }
    }
}