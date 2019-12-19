using Dapper;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Tests.Integration.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;

namespace Icon.Web.Tests.Integration.Queries
{
    [TestClass]
    public class GetCharacterSetsByAttributeTests
    {
        private SqlConnection connection;
        private GetCharacterSetsByAttributeQuery query;
        private TransactionScope transaction;
        private GetCharacterSetsByAttributeParameters parameters;
        private int pickListId;
        private int dataTypeId;
        private int attributeGroupId;
        private int attributeId;
        private int pickListSecondId;
        private int characterSetIdOne;
        private int characterSetIdTwo;
        private int attributeCharacterSetIdOne;
        private int attributeCharacterSetIdTwo;

        [TestInitialize]
        public void InitializeData()
        {
            transaction = new TransactionScope();
            connection = SqlConnectionBuilder.CreateIconConnection();
            StageData();
            query = new GetCharacterSetsByAttributeQuery(connection);
            parameters = new GetCharacterSetsByAttributeParameters();
            parameters.AttributeId = attributeId;
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Dispose();
        }

        [TestMethod]
        public void GetCharacterSetsByAttributeQuery_CharacterSetExists_ReturnsCharacterSets()
        {
            //When
            var result = query.Search(parameters);

            //Then
            Assert.AreEqual(2, result.Where(r => r.AttributeId == attributeId).Count());
        }

        private void StageData()
        {
            dataTypeId = connection.Query<int>($"insert into dbo.datatype(datatype) values ('Test')   SELECT SCOPE_IDENTITY()").Single();
            attributeGroupId = connection.Query<int>($"insert into dbo.attributeGroup(attributeGroupName) values ('Test')   SELECT SCOPE_IDENTITY()").Single();
            attributeId = connection.Query<int>($@"
                                                INSERT INTO[dbo].[Attributes]
                                               ([DisplayName]
                                               ,[AttributeName]
                                               ,[AttributeGroupId]
                                               ,[HasUniqueValues]
                                               ,[Description]
                                               ,[DefaultValue]
                                               ,[IsRequired]
                                               ,[SpecialCharactersAllowed]
                                               ,[TraitCode]
                                               ,[DataTypeId]
                                               ,[DisplayOrder]
                                               ,[InitialValue]
                                               ,[IncrementBy]
                                               ,[InitialMax]
                                               ,[DisplayType]
                                               ,[MaxLengthAllowed]
                                               ,[IsPickList]) values ('Attribute1','Attribute1',{attributeGroupId},'0','Test',Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,'1')  
                                                SELECT SCOPE_IDENTITY()", new
            {
                attributeGroupId = attributeGroupId
            }).Single();

            pickListId = connection.Query<int>($"insert into dbo.picklistdata(attributeId,PickListValue) values ({attributeId},'Yes') SELECT SCOPE_IDENTITY()", new { attributeId = attributeId }).Single();

            pickListSecondId = connection.Query<int>($"insert into dbo.picklistdata(attributeId,PickListValue) values ({attributeId},'No')   SELECT SCOPE_IDENTITY()", new { attributeId = attributeId }).Single();

            characterSetIdOne = connection.Query<int>($"insert into dbo.CharacterSets(Name,RegEx) values ('CharacterSet1','') SELECT SCOPE_IDENTITY()", new { attributeId = attributeId }).Single();

            characterSetIdTwo = connection.Query<int>($"insert into dbo.CharacterSets(Name,RegEx) values ('CharacterSet2','') SELECT SCOPE_IDENTITY()", new { attributeId = attributeId }).Single();

            attributeCharacterSetIdOne = connection.Query<int>($"insert into dbo.[AttributeCharacterSets]([CharacterSetId],[AttributeId]) values ({characterSetIdOne},{attributeId}) SELECT SCOPE_IDENTITY()", new { attributeId = attributeId, characterSetIdOne = characterSetIdOne }).Single();
            attributeCharacterSetIdTwo = connection.Query<int>($"insert into dbo.[AttributeCharacterSets]([CharacterSetId],[AttributeId]) values ({characterSetIdTwo},{attributeId}) SELECT SCOPE_IDENTITY()", new { attributeId = attributeId, characterSetIdTwo = characterSetIdTwo }).Single();
        }
    }
}