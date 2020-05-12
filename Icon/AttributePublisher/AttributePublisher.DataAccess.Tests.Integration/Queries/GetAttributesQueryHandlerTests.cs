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
            InsertTestData(true);

            //When
            var results = queryHandler.Search(parameters);

            //Then
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(attributeId, results[0].AttributeId);
            Assert.AreEqual("TestDescription", results[0].Description);
            Assert.AreEqual("TST", results[0].TraitCode);
            Assert.AreEqual(10, results[0].MaxLengthAllowed);
            Assert.AreEqual("1", results[0].MinimumNumber);
            Assert.AreEqual("5", results[0].MaximumNumber);
            Assert.AreEqual("Test Attribute Description", results[0].XmlTraitDescription);
            Assert.AreEqual("TestPattern", results[0].CharacterSetRegexPattern);
            Assert.AreEqual("TestGroup", results[0].AttributeGroupName);
            Assert.AreEqual("TestDataType", results[0].DataType);
            Assert.AreEqual(true, results[0].IsPickList);
            Assert.AreEqual(2, results[0].PickListValues.Count);
            Assert.AreEqual("TestPickList1", results[0].PickListValues[0]);
            Assert.AreEqual("TestPickList2", results[0].PickListValues[1]);
        }

        [TestMethod]
        public void GetAttributes_MessageQueueAttributeExistsButIsNotPickList_ReturnsAttributeWithoutPickListValues()
        {
            //Given
            InsertTestData(false);

            //When
            var results = queryHandler.Search(parameters);

            //Then
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(attributeId, results[0].AttributeId);
            Assert.AreEqual("TestDescription", results[0].Description);
            Assert.AreEqual("TST", results[0].TraitCode);
            Assert.AreEqual(10, results[0].MaxLengthAllowed);
            Assert.AreEqual("1", results[0].MinimumNumber);
            Assert.AreEqual("5", results[0].MaximumNumber);
            Assert.AreEqual("Test Attribute Description", results[0].XmlTraitDescription);
            Assert.AreEqual("TestPattern", results[0].CharacterSetRegexPattern);
            Assert.AreEqual("TestGroup", results[0].AttributeGroupName);
            Assert.AreEqual("TestDataType", results[0].DataType);
            Assert.AreEqual(false, results[0].IsPickList);
            Assert.IsNull(results[0].PickListValues);
        }

        private void InsertTestData(bool isPickList)
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
                    Description,
	                TraitCode,
                    MaxLengthAllowed,
                    MinimumNumber,
                    MaximumNumber,
                    IsPickList,
	                XmlTraitDescription,
                    isSpecialTransform,
	                DataTypeId,
	                AttributeGroupId
	                )
                VALUES (
	                'TestAttributeName',
                    'TestDescription',
	                'TST',
                    10,
                    1,
                    5,
                    @isPickList,
	                'Test Attribute Description',
                    0,
                    @dataTypeId,
	                @groupId
                    )
                
                SET @attributeId = SCOPE_IDENTITY()
                INSERT INTO dbo.AttributesWebConfiguration(Attributeid,GridColumnWidth,CharacterSetRegexPattern)values(@attributeId,200,'TestPattern')
                INSERT INTO esb.MessageQueueAttribute(AttributeId) VALUES (@attributeId)

                INSERT INTO dbo.PickListData(AttributeId, PickListValue)
                VALUES (@attributeId, 'TestPickList1'), (@attributeId, 'TestPickList2')
                
                SELECT @attributeId",
                new { isPickList });
        }
    }
}