using Icon.Web.DataAccess.Queries;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Data.SqlClient;
using System.Transactions;
using Dapper;
using Icon.Web.Tests.Integration.TestHelpers;

namespace Icon.Web.Tests.Integration.Queries
{
    [TestClass]
    public class GetAttributeByAttributeIdQueryTests
    {
        private SqlConnection connection;
        private GetAttributeByAttributeIdQuery query;
        private TransactionScope transaction;
        private GetAttributeByAttributeIdParameters parameters;
        private int pickListId;
        private int dataTypeId;
        private int attributeGroupId;
        private int attributeId;
        private int pickListSecondId;

        [TestInitialize]
        public void InitializeData()
        {
            transaction = new TransactionScope();
            connection = SqlConnectionBuilder.CreateIconConnection();
            query = new GetAttributeByAttributeIdQuery(connection);
            parameters = new GetAttributeByAttributeIdParameters();
            StageData();
            parameters.AttributeId = attributeId;
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Dispose();
            connection.Dispose();
        }

        [TestMethod]
        public void GetAttributeByAttributeIdQuery_AttributeExists_ReturnsAttribute()
        {
            //When
            var result = query.Search(parameters);

            //Then
            Assert.AreEqual(attributeId, result.AttributeId);
            Assert.AreEqual("1", result.MinimumNumber);
            Assert.AreEqual("2", result.MaximumNumber);
            Assert.AreEqual("3", result.NumberOfDecimals);
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
                                               ,[MinimumNumber]
                                               ,[MaximumNumber]
                                               ,[NumberOfDecimals]
                                               ,[IsPickList]) values ('Attribute1','Attribute1',{attributeGroupId},'0','Test',Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,'1','2','3','1')  
                                                SELECT SCOPE_IDENTITY()", new
            {
                attributeGroupId = attributeGroupId
            }).Single();

            pickListId = connection.Query<int>($"insert into dbo.picklistdata(attributeId,PickListValue) values ({attributeId},'Yes') SELECT SCOPE_IDENTITY()", new { attributeId = attributeId }).Single();

            pickListSecondId = connection.Query<int>($"insert into dbo.picklistdata(attributeId,PickListValue) values ({attributeId},'No')   SELECT SCOPE_IDENTITY()", new { attributeId = attributeId }).Single();
        }
    }
}