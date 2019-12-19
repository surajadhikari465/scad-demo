using AttributePublisher.DataAccess.Queries;
using Dapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using System.Data.SqlClient;
using System.Transactions;

namespace AttributePublisher.DataAccess.Tests.Integration.Queries
{
    [TestClass]
    public class GetAttributesQueryHandlerTests
    {
        private GetAttributesQueryHandler queryHandler;
        private GetAttributesParameters parameters;
        private SqlConnection sqlConnection;
        private TransactionScope transaction;
        private int attributeId;

        [TestInitialize]
        public void Initialize()
        {
            transaction = new TransactionScope();
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Icon"].ConnectionString);
            queryHandler = new GetAttributesQueryHandler(sqlConnection);
            parameters = new GetAttributesParameters { RecordsPerQuery = 1 };
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Dispose();
            sqlConnection.Dispose();
        }

        [TestMethod]
        public void GetAttributes_MessageQueueAttributeExists_ReturnsAttribute()
        {
            //Given
            InsertTestData();

            //When
            var results = queryHandler.Search(parameters);

            //Then
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(attributeId, results[0].AttributeId);
            Assert.AreEqual("TestGroup", results[0].AttributeGroupName);
            Assert.AreEqual("TestAttributeName", results[0].AttributeName);
            Assert.AreEqual("TestDataType", results[0].DataType);
            Assert.AreEqual("TST", results[0].TraitCode);
            Assert.AreEqual("Test Attribute Description", results[0].XmlTraitDescription);
        }

        private void InsertTestData()
        {
            attributeId = sqlConnection.QueryFirst<int>(@"
                DECLARE @groupId INT
                DECLARE @dataTypeId INT
                DECLARE @attributeId INT

                INSERT dbo.AttributeGroup (AttributeGroupName)
                VALUES ('TestGroup')

                SET @groupId = SCOPE_IDENTITY()

                INSERT dbo.DataType (DataType)
                VALUES ('TestDataType')

                SET @dataTypeId = SCOPE_IDENTITY()

                INSERT dbo.Attributes (
	                AttributeName,
	                TraitCode,
	                XmlTraitDescription,
	                DataTypeId,
	                AttributeGroupId
	                )
                VALUES (
	                'TestAttributeName',
	                'TST',
	                'Test Attribute Description',
                    @dataTypeId,
	                @groupId	                
	                )

                SET @attributeId = SCOPE_IDENTITY()
                
                INSERT INTO esb.MessageQueueAttribute(AttributeId) VALUES (@attributeId)
                
                SELECT @attributeId");
        }
    }
}
