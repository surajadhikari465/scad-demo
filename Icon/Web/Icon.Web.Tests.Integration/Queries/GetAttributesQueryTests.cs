using System.Collections.Generic;
using Icon.Web.DataAccess.Queries;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Data.SqlClient;
using System.Transactions;
using Dapper;
using Icon.Common.DataAccess;
using Icon.Common.Models;
using Icon.Web.Tests.Integration.TestHelpers;

namespace Icon.Web.Tests.Integration.Queries
{
    [TestClass]
    public class GetAttributesQueryTests
    {
        private SqlConnection connection;
        private GetAttributesQueryHandler query;
        private TransactionScope transaction;
        private EmptyQueryParameters<IEnumerable<AttributeModel>> parameters;
        private int pickListId;
        private int dataTypeId;
        private int itemAttributeGroupId;
        private int skuAttributeGroupId;
        private int priceLineAttributeGroupId;
        private int nutritionAttributeGroupId;
        private int attributeId;
        private int pickListSecondId;
        private int attributesWebConfigurationId;
        private int characterSetId;
        private int attributeCharacterSetId;
        private GetItemCountOnAttributeQueryHandler itemCountQry;
        private EmptyAttributesParameters param;

        [TestInitialize]
        public void InitializeData()
        {
            transaction = new TransactionScope();
            connection = SqlConnectionBuilder.CreateIconConnection();
            query = new GetAttributesQueryHandler(connection);
            parameters = new EmptyQueryParameters<IEnumerable<AttributeModel>>();
            itemCountQry = new GetItemCountOnAttributeQueryHandler(connection);
            param = new EmptyAttributesParameters();
            setupData();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Dispose();
        }

        [TestMethod]
        public void GetAttributesQuery_AttributeExists_ReturnsAttributes()
        {
            //When
            var result = query.Search(parameters);

            //Then
            var testAttribute = result.SingleOrDefault(a => a.AttributeId == attributeId);
            Assert.IsNotNull(testAttribute);
            Assert.AreEqual("AttributeDisplayItem", testAttribute.DisplayName);
            Assert.AreEqual("AttributeNameItem", testAttribute.AttributeName);
            Assert.AreEqual(itemAttributeGroupId, testAttribute.AttributeGroupId);
            Assert.AreEqual(false, testAttribute.HasUniqueValues);
            Assert.AreEqual("Test", testAttribute.Description);
            Assert.AreEqual(dataTypeId, testAttribute.DataTypeId);
            Assert.AreEqual("Test", testAttribute.DataTypeName);
            Assert.AreEqual("TestDefaultValue", testAttribute.DefaultValue);
            Assert.IsTrue(testAttribute.IsRequired);
            Assert.IsNull(testAttribute.SpecialCharactersAllowed);
            Assert.AreEqual("TestCode", testAttribute.TraitCode);
            Assert.AreEqual(1000000, testAttribute.DisplayOrder);
            Assert.AreEqual(100001, testAttribute.InitialValue);
            Assert.AreEqual(20, testAttribute.IncrementBy);
            Assert.AreEqual(100, testAttribute.InitialMax);
            Assert.AreEqual("Number", testAttribute.DisplayType);
            Assert.AreEqual(500, testAttribute.MaxLengthAllowed);
            Assert.AreEqual(1m, decimal.Parse(testAttribute.MinimumNumber));
            Assert.AreEqual(2m, decimal.Parse(testAttribute.MaximumNumber));
            Assert.AreEqual(3, int.Parse(testAttribute.NumberOfDecimals));
            Assert.IsTrue(testAttribute.IsPickList);
            Assert.IsTrue(testAttribute.IsReadOnly);

            Assert.AreEqual(2, testAttribute.PickListData.Count());

            var firstPickListData = testAttribute.PickListData.Where(s=>s.PickListId== pickListId).FirstOrDefault();
            Assert.AreEqual(attributeId, firstPickListData.AttributeId);
            Assert.AreEqual("Yes", firstPickListData.PickListValue);

            var lastPickListData = testAttribute.PickListData.Where(s => s.PickListId == pickListSecondId).FirstOrDefault();
            Assert.AreEqual(attributeId, lastPickListData.AttributeId);
            Assert.AreEqual("No", lastPickListData.PickListValue);

            var characterSet = testAttribute.CharacterSets.Single();
            Assert.AreEqual(attributeId, characterSet.AttributeId);
            Assert.AreEqual(attributeCharacterSetId, characterSet.AttributeCharacterSetId);
            Assert.AreEqual(characterSetId, characterSet.CharacterSetModel.CharacterSetId);
            Assert.AreEqual("TestCharacterSet", characterSet.CharacterSetModel.Name);
            Assert.AreEqual("Test", characterSet.CharacterSetModel.RegEx);
            Assert.IsFalse(result.Any(x => x.AttributeGroupId == nutritionAttributeGroupId));
            Assert.IsFalse(result.Any(x => x.AttributeGroupId == skuAttributeGroupId));
            Assert.IsFalse(result.Any(x => x.AttributeGroupId ==  priceLineAttributeGroupId));
        }

		[TestMethod]
		public void GetAttributesQuery_AttributeExists_ReturnsAttributesWithoutNutritionAttributes()
        {
            //When
            var result = query.Search(parameters);

			var groupId = connection.ExecuteScalar("SELECT AttributeGroupID FROM AttributeGroup WHERE AttributeGroupName = 'Nutrition';");

			//Then
			Assert.IsFalse(result.Any(x => x.AttributeGroupId == (int)groupId));
			Assert.IsTrue(result.Count() > 0);
		}

        [TestMethod]
        public void GetAttributesQuery_AttributeExists_ReturnsAttributesWithoutSkuLineAttributes()
        {
            //When
            var result = query.Search(parameters);

            var groupId = connection.ExecuteScalar("SELECT AttributeGroupID FROM AttributeGroup WHERE AttributeGroupName = 'Sku';");

            //Then
            Assert.IsFalse(result.Any(x => x.AttributeGroupId == (int)groupId));
            Assert.IsTrue(result.Count() > 0);
        }

        [TestMethod]
        public void GetAttributesQuery_AttributeExists_ReturnsAttributesWithoutPriceLineLineAttributes()
        {
            //When
            var result = query.Search(parameters);

            var groupId = connection.ExecuteScalar("SELECT AttributeGroupID FROM AttributeGroup WHERE AttributeGroupName = 'PriceLine';");

            //Then
            Assert.IsFalse(result.Any(x => x.AttributeGroupId == (int)groupId));
            Assert.IsTrue(result.Count() > 0);
        }

        [TestMethod]
        public void GetAttributesQuery_AttributeExists_ReturnsAttributesWithItemCount()
        {
            //When
            var result = itemCountQry.Search(param).Where(a => a.AttributeName.Equals("ABF")).FirstOrDefault().ItemCount;            
            
            var itemCount = connection.ExecuteScalar("SELECT COUNT(*) FROM Item i with (nolock) CROSS APPLY OPENJSON(ItemAttributesJson) j WHERE j.[key] = 'ABF' GROUP BY j.[Key]; ");

            //Then         
            Assert.IsNotNull(result);
            Assert.IsNotNull(itemCount);
            Assert.AreEqual(itemCount, result);            
        }

        private void setupData()
        {
            StageNutritionData();
            StagePriceLineAttributeData();
            StageSkuAttributeData();
            StageItemAttributeData();
        }

        private void StageNutritionData()
        {
            dataTypeId = connection.QuerySingle<int>(@"
                INSERT dbo.DataType VALUES ('Test');
                SELECT SCOPE_IDENTITY();");
            nutritionAttributeGroupId = connection.QuerySingle<int>($"INSERT dbo.attributeGroup(attributeGroupName) VALUES ('Test'); SELECT SCOPE_IDENTITY()");
            attributeId = connection.QuerySingle<int>(
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
	                'AttributeDisplay1',    --DisplayName,
	                'AttributeName1',       --AttributeName,
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
                    AttributeGroupId = nutritionAttributeGroupId,
                    DataTypeId = dataTypeId,
                    MinimumNumber = 1,
                    MaximumNumber = 2,
                    NumberOfDecimals = 3
                });
            pickListId = connection.QuerySingle<int>($"insert into dbo.picklistdata(attributeId,PickListValue) values ({attributeId},'Yes') SELECT SCOPE_IDENTITY()", new { attributeId = attributeId });
            pickListSecondId = connection.QuerySingle<int>($"insert into dbo.picklistdata(attributeId,PickListValue) values ({attributeId},'No')   SELECT SCOPE_IDENTITY()", new { attributeId = attributeId });
            attributesWebConfigurationId = connection.QuerySingle<int>("INSERT INTO dbo.AttributesWebConfiguration(AttributeId, GridColumnWidth, IsReadOnly) VALUES (@AttributeId, 200, 1); SELECT SCOPE_IDENTITY()", new { AttributeId = attributeId });
            characterSetId = connection.QuerySingle<int>("INSERT INTO dbo.CharacterSets(Name, RegEx) VALUES ('TestCharacterSet', 'Test'); SELECT SCOPE_IDENTITY()");
            attributeCharacterSetId = connection.QuerySingle<int>("INSERT INTO dbo.AttributeCharacterSets(AttributeId, CharacterSetId) VALUES (@AttributeId, @CharacterSetId); SELECT SCOPE_IDENTITY()", new { AttributeId = attributeId, CharacterSetId = characterSetId });
        }

        private void StageSkuAttributeData()
        {
            connection.Execute($"IF NOT EXISTS( SELECT 1 FROM attributeGroup Where AttributeGroupName = 'Sku')BEGIN INSERT dbo.attributeGroup(attributeGroupName) VALUES ('Sku') END");

            dataTypeId = connection.QuerySingle<int>(@"
                INSERT dbo.DataType VALUES ('Test');
                SELECT SCOPE_IDENTITY();");
            skuAttributeGroupId = (int)connection.ExecuteScalar("SELECT AttributeGroupID FROM AttributeGroup WHERE AttributeGroupName = 'Sku';");
            attributeId = connection.QuerySingle<int>(
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
	                'AttributeDisplaySku',    --DisplayName,
	                'AttributeNameSku',       --AttributeName,
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
                    AttributeGroupId = skuAttributeGroupId,
                    DataTypeId = dataTypeId,
                    MinimumNumber = 1,
                    MaximumNumber = 2,
                    NumberOfDecimals = 3
                });
            pickListId = connection.QuerySingle<int>($"insert into dbo.picklistdata(attributeId,PickListValue) values ({attributeId},'Yes') SELECT SCOPE_IDENTITY()", new { attributeId = attributeId });
            pickListSecondId = connection.QuerySingle<int>($"insert into dbo.picklistdata(attributeId,PickListValue) values ({attributeId},'No')   SELECT SCOPE_IDENTITY()", new { attributeId = attributeId });
            attributesWebConfigurationId = connection.QuerySingle<int>("INSERT INTO dbo.AttributesWebConfiguration(AttributeId, GridColumnWidth, IsReadOnly) VALUES (@AttributeId, 200, 1); SELECT SCOPE_IDENTITY()", new { AttributeId = attributeId });
            characterSetId = connection.QuerySingle<int>("INSERT INTO dbo.CharacterSets(Name, RegEx) VALUES ('TestCharacterSet', 'Test'); SELECT SCOPE_IDENTITY()");
            attributeCharacterSetId = connection.QuerySingle<int>("INSERT INTO dbo.AttributeCharacterSets(AttributeId, CharacterSetId) VALUES (@AttributeId, @CharacterSetId); SELECT SCOPE_IDENTITY()", new { AttributeId = attributeId, CharacterSetId = characterSetId });
        }

        private void StagePriceLineAttributeData()
        {
            connection.Execute($"IF NOT EXISTS( SELECT 1 FROM attributeGroup Where AttributeGroupName = 'PriceLine')BEGIN INSERT dbo.attributeGroup(attributeGroupName) VALUES ('PriceLine') END");

            dataTypeId = connection.QuerySingle<int>(@"
                INSERT dbo.DataType VALUES ('Test');
                SELECT SCOPE_IDENTITY();");
            priceLineAttributeGroupId = (int)connection.ExecuteScalar("SELECT AttributeGroupID FROM AttributeGroup WHERE AttributeGroupName = 'PriceLine';");
            attributeId = connection.QuerySingle<int>(
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
	                'AttributeDisplaySku',    --DisplayName,
	                'AttributeNameSku',       --AttributeName,
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
                    AttributeGroupId = priceLineAttributeGroupId,
                    DataTypeId = dataTypeId,
                    MinimumNumber = 1,
                    MaximumNumber = 2,
                    NumberOfDecimals = 3
                });
            pickListId = connection.QuerySingle<int>($"insert into dbo.picklistdata(attributeId,PickListValue) values ({attributeId},'Yes') SELECT SCOPE_IDENTITY()", new { attributeId = attributeId });
            pickListSecondId = connection.QuerySingle<int>($"insert into dbo.picklistdata(attributeId,PickListValue) values ({attributeId},'No')   SELECT SCOPE_IDENTITY()", new { attributeId = attributeId });
            attributesWebConfigurationId = connection.QuerySingle<int>("INSERT INTO dbo.AttributesWebConfiguration(AttributeId, GridColumnWidth, IsReadOnly) VALUES (@AttributeId, 200, 1); SELECT SCOPE_IDENTITY()", new { AttributeId = attributeId });
            characterSetId = connection.QuerySingle<int>("INSERT INTO dbo.CharacterSets(Name, RegEx) VALUES ('TestCharacterSet', 'Test'); SELECT SCOPE_IDENTITY()");
            attributeCharacterSetId = connection.QuerySingle<int>("INSERT INTO dbo.AttributeCharacterSets(AttributeId, CharacterSetId) VALUES (@AttributeId, @CharacterSetId); SELECT SCOPE_IDENTITY()", new { AttributeId = attributeId, CharacterSetId = characterSetId });
        }

        private void StageItemAttributeData()
        {
             connection.Execute($"IF NOT EXISTS( SELECT 1 FROM attributeGroup Where AttributeGroupName = 'Global Item')BEGIN INSERT dbo.attributeGroup(attributeGroupName) VALUES ('Global Item') END");

            dataTypeId = connection.QuerySingle<int>(@"
                INSERT dbo.DataType VALUES ('Test');
                SELECT SCOPE_IDENTITY();");
            itemAttributeGroupId = (int)connection.ExecuteScalar("SELECT AttributeGroupID FROM AttributeGroup WHERE AttributeGroupName = 'Global Item';");
            attributeId = connection.QuerySingle<int>(
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
            attributesWebConfigurationId = connection.QuerySingle<int>("INSERT INTO dbo.AttributesWebConfiguration(AttributeId, GridColumnWidth, IsReadOnly) VALUES (@AttributeId, 200, 1); SELECT SCOPE_IDENTITY()", new { AttributeId = attributeId });
            characterSetId = connection.QuerySingle<int>("INSERT INTO dbo.CharacterSets(Name, RegEx) VALUES ('TestCharacterSet', 'Test'); SELECT SCOPE_IDENTITY()");
            attributeCharacterSetId = connection.QuerySingle<int>("INSERT INTO dbo.AttributeCharacterSets(AttributeId, CharacterSetId) VALUES (@AttributeId, @CharacterSetId); SELECT SCOPE_IDENTITY()", new { AttributeId = attributeId, CharacterSetId = characterSetId });
        }
    }
}