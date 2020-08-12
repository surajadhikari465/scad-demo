using Icon.Web.DataAccess.Queries;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlClient;
using System.Transactions;
using Dapper;
using Icon.Web.Tests.Integration.TestHelpers;

namespace Icon.Web.Tests.Integration.Queries
{
    [Ignore("TODO: PBI-39840 - Fix Icon Unit and Integration tests in Icon Web")]
    [TestClass]
    public class GetAttributeByNameGroupQueryTests
    {
        private SqlConnection connection;
        private GetAttributeByNameGroupQueryHandler query;
        private TransactionScope transaction;
        private GetAttributeByNameGroupParameters parameters;
        private int pickListId;
        private int dataTypeId;
        private string attributeName;
        private int pickListSecondId;
        private int itemAttributeGroupId;

        [TestInitialize]
        public void InitializeData()
        {
            transaction = new TransactionScope();
            connection = SqlConnectionBuilder.CreateIconConnection();
            query = new GetAttributeByNameGroupQueryHandler(connection);
            parameters = new GetAttributeByNameGroupParameters();
            StageItemAttributeData();
            parameters.AttributeName = attributeName;
            parameters.AttributeGroupName = "SKU";
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Dispose();
            connection.Dispose();
        }

        [TestMethod]
        public void GetAttributeByNameGroupQuery_AttributeExists_ReturnsAttribute()
        {
            //When
            var result = query.Search(parameters);

            //Then
            Assert.AreEqual(attributeName, result.AttributeName);
            Assert.AreEqual("1", result.MinimumNumber);
            Assert.AreEqual("2", result.MaximumNumber);
            Assert.AreEqual("3", result.NumberOfDecimals);
        }

        private void StageItemAttributeData()
        {
            attributeName = "AttributeNameItem";
            connection.Execute($"IF NOT EXISTS( SELECT 1 FROM attributeGroup Where AttributeGroupName = 'Global Item')BEGIN INSERT dbo.attributeGroup(attributeGroupName) VALUES ('Global Item') END");

            dataTypeId = connection.QuerySingle<int>(@"
                INSERT dbo.DataType VALUES ('Test');
                SELECT SCOPE_IDENTITY();");
            itemAttributeGroupId = (int)connection.ExecuteScalar("SELECT AttributeGroupID FROM AttributeGroup WHERE AttributeGroupName = 'Global Item';");
            int attributeId = connection.QuerySingle<int>(
                 $@"
                    INSERT dbo.Attributes (
	                DisplayName,
	                AttributeName,
	                AttributeGroupId,
	                HasUniqueValues,
	                Description,
	                DefaultValue,
	                IsRequired,
	                SpecialCharactersAllowed,
	                TraitCode,
	                DataTypeId,
	                DisplayOrder,
	                InitialValue,
	                IncrementBy,
	                InitialMax,
	                DisplayType,
	                MaxLengthAllowed,
                    MinimumNumber,
                    MaximumNumber,
                    NumberOfDecimals,
	                IsPickList
	                )
                VALUES (
	                'AttributeDisplayItem',    --DisplayName,
	                'AttributeNameItem',       --AttributeName,
	                @AttributeGroupId,      --AttributeGroupId,
	                '0',                    --HasUniqueValues,
	                'Test',                 --Description,
	                'TestDefaultValue',     --DefaultValue,
	                1,                      --IsRequired,
	                NULL,                   --SpecialCharactersAllowed,
	                'TestCode',             --TraitCode,
	                @DataTypeId,            --DataTypeId,
	                1000000,                --DisplayOrder,
	                100001,                 --InitialValue,
	                20,                     --IncrementBy,
	                100,                    --InitialMax,
	                'Number',               --DisplayType,
	                500,                    --MaxLengthAllowed,
	                @MinimumNumber,         --MinimumNumber,
	                @MaximumNumber,         --MaximumNumber,
	                @NumberOfDecimals,      --NumberOfDecimals,
	                1                       --IsPickList
	                )

                SELECT SCOPE_IDENTITY()
                ",
                 new
                 {
                     AttributeGroupId = itemAttributeGroupId,
                     DataTypeId = dataTypeId,
                     MinimumNumber = 1,
                     MaximumNumber = 2,
                     NumberOfDecimals = 3
                 });
            pickListId = connection.QuerySingle<int>($"insert into dbo.picklistdata(attributeId,PickListValue) values ({attributeId},'Yes') SELECT SCOPE_IDENTITY()", new { attributeId = attributeId });
            pickListSecondId = connection.QuerySingle<int>($"insert into dbo.picklistdata(attributeId,PickListValue) values ({attributeId},'No')   SELECT SCOPE_IDENTITY()", new { attributeId = attributeId });
            int attributesWebConfigurationId = connection.QuerySingle<int>("INSERT INTO dbo.AttributesWebConfiguration(AttributeId, GridColumnWidth, IsReadOnly) VALUES (@AttributeId, 200, 1); SELECT SCOPE_IDENTITY()", new { AttributeId = attributeId });
            int characterSetId = connection.QuerySingle<int>("INSERT INTO dbo.CharacterSets(Name, RegEx) VALUES ('TestCharacterSet', 'Test'); SELECT SCOPE_IDENTITY()");
            int attributeCharacterSetId = connection.QuerySingle<int>("INSERT INTO dbo.AttributeCharacterSets(AttributeId, CharacterSetId) VALUES (@AttributeId, @CharacterSetId); SELECT SCOPE_IDENTITY()", new { AttributeId = attributeId, CharacterSetId = characterSetId });
        }

    }
}