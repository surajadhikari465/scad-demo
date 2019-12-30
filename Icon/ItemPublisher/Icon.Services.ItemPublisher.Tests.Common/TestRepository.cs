using Dapper;
using Icon.Services.ItemPublisher.Repositories.Entities;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Icon.Services.Newitem.Test.Common
{
    /// <summary>
    /// This class contains all of the helper methods needed to verify changes or to put mock data in the database
    /// </summary>
    public class TestRepository
    {
        private ConnectionHelper connectionHelper;
        private TestDataFactory testDataFactory;

        public TestRepository(ConnectionHelper connectionHelper)
        {
            this.connectionHelper = connectionHelper;
            this.testDataFactory = new TestDataFactory();
        }

        public async Task<int> InsertAttribute(string traitCode, string dataTypeName)
        {
            string query = $@"
                DECLARE @dataTypeId INT = (SELECT DataTypeId FROM dbo.DataType WHERE DataType = @DataTypeName)

                INSERT INTO [dbo].[Attributes]
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
                    ,[IsPickList]
                    ,[XmlTraitDescription]
                    ,[IsSpecialTransform])
                VALUES
                    (@DisplayName,
                    @AttributeName,
                    @AttributeGroupId,
                    @HasUniqueValues,
                    @Description,
                    @DefaultValue,
                    @IsRequired,
                    @SpecialCharactersAllowed,
                    @TraitCode,
                    @dataTypeId,
                    @DisplayOrder,
                    @InitialValue,
                    @IncrementBy,
                    @InitialMax,
                    @DisplayType,
                    @MaxLengthAllowed,
                    @IsPickList,
                    @XmlTraitDescription,
                    @IsSpecialTransform);
                    SELECT CAST(SCOPE_IDENTITY() AS INT);";

            int id = (await this.connectionHelper.ProviderFactory.Provider.Connection.QueryAsync<int>(query,
                new
                {
                    DisplayName = "AttributeDisplayName",
                    AttributeName = "AttributeName",
                    AttributeGroupId = 2,
                    HasUniqueValues = false,
                    Description = "Description",
                    DefaultValue = "DefaultValue",
                    IsRequired = false,
                    SpecialCharactersAllowed = false,
                    TraitCode = traitCode,
                    DisplayOrder = 1,
                    InitialValue = 1,
                    IncrementBy = 1,
                    InitialMax = 1,
                    DisplayType = "DisplayType",
                    MaxLengthAllowed = "",
                    NumberValidationRule = "",
                    IsPickList = false,
                    XmlTraitDescription = "XmlTraitDescription",
                    DataTypeName = dataTypeName,
                    IsSpecialTransform = 1
                }, this.connectionHelper.ProviderFactory.Transaction)).First();

            return id;
        }

        public async Task<int> InsertHierarchy(string name)
        {
            string query = $@"INSERT INTO [dbo].[Hierarchy]
            ([hierarchyName])
            VALUES
            (@hierarchyName);
            SELECT CAST(SCOPE_IDENTITY() AS INT);";

            int id = (await this.connectionHelper.ProviderFactory.Provider.Connection.QueryAsync<int>(query,
               new
               {
                   hierarchyName = name,
               },
               connectionHelper.ProviderFactory.Transaction)).First();
            return id;
        }

        public async Task<int> InsertTrait(string traitCode)
        {
            string query = $@"INSERT INTO [dbo].[Trait]
           ([traitCode]
           ,[traitPattern]
           ,[traitGroupId])
            VALUES
           (@traitcode,
            @traitPattern,
            @traitGroupId);
           SELECT CAST(SCOPE_IDENTITY() AS INT);";

            int id = (await this.connectionHelper.ProviderFactory.Provider.Connection.QueryAsync<int>(query,
               new
               {
                   traitCode = traitCode,
                   traitPattern = "",
                   traitGroupId = 1,
               },
               connectionHelper.ProviderFactory.Transaction)).First();
            return id;
        }

        public async Task<int> InsertScanCode(int itemId)
        {
            string query = $@"INSERT INTO [dbo].[ScanCode]
           ([itemId]
          ,[scanCode]
          ,[scanCodeTypeId]
          ,[localeId])
            VALUES
           (@itemId,
            @scanCode,
            @scanCodeTypeId,
            @localeId);
           SELECT CAST(SCOPE_IDENTITY() AS INT);";

            int id = (await this.connectionHelper.ProviderFactory.Provider.Connection.QueryAsync<int>(query,
               new
               {
                   itemId = itemId,
                   scanCode = "test",
                   scanCodeTypeId = 1,
                   localeId = 1
               },
               connectionHelper.ProviderFactory.Transaction)).First();
            return id;
        }

        public async Task<int> InsertItem(int itemTypeId)
        {
            string query = $@"INSERT INTO [dbo].[Item]
           ([ItemTypeId]
           ,[ItemAttributesJson])
            VALUES
           (@ItemTypeId
           ,@ItemAttributesJson);
           SELECT CAST(SCOPE_IDENTITY() AS INT)";

            int id = (await this.connectionHelper.ProviderFactory.Provider.Connection.QueryAsync<int>(query,
            new
            {
                ItemTypeId = itemTypeId,
                ItemAttributesJson = @"{""ProductDescription"":""ProductDescription"",
                    ""POSDescription"":""POSDescription"",
                    ""CustomerFriendlyDescription"":""CustomerFriendlyDescription"",
                    ""PackageUnit"":""99"",
                    ""RetailSize"":""1.0"",
                    ""RetailUOM"":""Reta"",
                    ""IsActive"":""1"",
                    ""FoodStampEligible"":""1"",
                    ""POSScaleTare"":""4.0"",
                    ""ProhibitDiscount"":""1"",
                    ""Notes"":""Notes"",
                    ""CreatedBy"":""CreatedBy"",
                    ""CreatedDateTimeUtc"":""2019-01-01"",
                    ""ModifiedBy"":""ModifiedBy"",
                    ""ModifiedDateTimeUtc"":""2019-01-01""
                }"
            },
            connectionHelper.ProviderFactory.Transaction)).First();

            return id;
        }

        public async Task<int> InsertItemType()
        {
            string query = $@"INSERT INTO [dbo].[ItemType]
            ([itemTypeCode]
            ,[itemTypeDesc])
            VALUES
            (@itemTypeCode,
            @itemTypeDesc);
            SELECT CAST(SCOPE_IDENTITY() AS INT);";

            int id = (await this.connectionHelper.ProviderFactory.Provider.Connection.QueryAsync<int>(query,
               new
               {
                   itemTypeId = 9999999,
                   itemTypeCode = "TES",
                   itemTypeDesc = "desc"
               },
               connectionHelper.ProviderFactory.Transaction)).First();
            return id;
        }

        public async Task<int> InsertNutrition(string plu)
        {
            string query = $@"INSERT INTO [nutrition].[ItemNutrition]
           ([Plu]
           ,[RecipeName]
           ,[Allergens]
           ,[Ingredients]
           ,[ServingsPerPortion]
           ,[ServingSizeDesc]
           ,[ServingPerContainer]
           ,[HshRating]
           ,[ServingUnits]
           ,[SizeWeight]
           ,[Calories]
           ,[CaloriesFat]
           ,[CaloriesSaturatedFat]
           ,[TotalFatWeight]
           ,[TotalFatPercentage]
           ,[SaturatedFatWeight]
           ,[SaturatedFatPercent]
           ,[PolyunsaturatedFat]
           ,[MonounsaturatedFat]
           ,[CholesterolWeight]
           ,[CholesterolPercent]
           ,[SodiumWeight]
           ,[SodiumPercent]
           ,[PotassiumWeight]
           ,[PotassiumPercent]
           ,[TotalCarbohydrateWeight]
           ,[TotalCarbohydratePercent]
           ,[DietaryFiberWeight]
           ,[DietaryFiberPercent]
           ,[SolubleFiber]
           ,[InsolubleFiber]
           ,[Sugar]
           ,[SugarAlcohol]
           ,[OtherCarbohydrates]
           ,[ProteinWeight]
           ,[ProteinPercent]
           ,[VitaminA]
           ,[Betacarotene]
           ,[VitaminC]
           ,[Calcium]
           ,[Iron]
           ,[VitaminD]
           ,[VitaminE]
           ,[Thiamin]
           ,[Riboflavin]
           ,[Niacin]
           ,[VitaminB6]
           ,[Folate]
           ,[VitaminB12]
           ,[Biotin]
           ,[PantothenicAcid]
           ,[Phosphorous]
           ,[Iodine]
           ,[Magnesium]
           ,[Zinc]
           ,[Copper]
           ,[Transfat]
           ,[CaloriesFromTransfat]
           ,[Om6Fatty]
           ,[Om3Fatty]
           ,[Starch]
           ,[Chloride]
           ,[Chromium]
           ,[VitaminK]
           ,[Manganese]
           ,[Molybdenum]
           ,[Selenium]
           ,[TransfatWeight]
           ,[InsertDate]
           ,[ModifiedDate])
           VALUES
           (@Plu,
           @RecipeName,
           @Allergens,
           @Ingredients,
           @ServingsPerPortion,
           @ServingSizeDesc,
           @ServingPerContainer,
           @HshRating,
           @ServingUnits,
           @SizeWeight,
           @Calories,
           @CaloriesFat,
           @CaloriesSaturatedFat,
           @TotalFatWeight,
           @TotalFatPercentage,
           @SaturatedFatWeight,
           @SaturatedFatPercent,
           @PolyunsaturatedFat,
           @MonounsaturatedFat,
           @CholesterolWeight,
           @CholesterolPercent,
           @SodiumWeight,
           @SodiumPercent,
           @PotassiumWeight,
           @PotassiumPercent,
           @TotalCarbohydrateWeight,
           @TotalCarbohydratePercent,
           @DietaryFiberWeight,
           @DietaryFiberPercent,
           @SolubleFiber,
           @InsolubleFiber,
           @Sugar,
           @SugarAlcohol,
           @OtherCarbohydrates,
           @ProteinWeight,
           @ProteinPercent,
           @VitaminA,
           @Betacarotene,
           @VitaminC,
           @Calcium,
           @Iron,
           @VitaminD,
           @VitaminE,
           @Thiamin,
           @Riboflavin,
           @Niacin,
           @VitaminB6,
           @Folate,
           @VitaminB12,
           @Biotin,
           @PantothenicAcid,
           @Phosphorous,
           @Iodine,
           @Magnesium,
           @Zinc,
           @Copper,
           @Transfat,
           @CaloriesFromTransfat,
           @Om6Fatty,
           @Om3Fatty,
           @Starch,
           @Chloride,
           @Chromium,
           @VitaminK,
           @Manganese,
           @Molybdenum,
           @Selenium,
           @TransfatWeight,
           @InsertDate,
           @ModifiedDate);
           SELECT CAST(SCOPE_IDENTITY() AS INT);";

            int id = (await this.connectionHelper.ProviderFactory.Provider.Connection.QueryAsync<int>(query,
            new
            {
                Plu = plu,
                RecipeName = "RecipeName",
                Allergens = "Allergens",
                Ingredients = "Ingredients",
                ServingsPerPortion = 1.0,
                ServingSizeDesc = "ServingSizeDesc",
                ServingPerContainer = "ServingPerContainer",
                HshRating = 1.0,
                ServingUnits = 1.0,
                SizeWeight = 1.0,
                Calories = 1.0,
                CaloriesFat = 1.0,
                CaloriesSaturatedFat = 1.0,
                TotalFatWeight = 1.0,
                TotalFatPercentage = 1.0,
                SaturatedFatWeight = 1.0,
                SaturatedFatPercent = 1.0,
                PolyunsaturatedFat = 1.0,
                MonounsaturatedFat = 1.0,
                CholesterolWeight = 1.0,
                CholesterolPercent = 1.0,
                SodiumWeight = 1.0,
                SodiumPercent = 1.0,
                PotassiumWeight = 1.0,
                PotassiumPercent = 1.0,
                TotalCarbohydrateWeight = 1.0,
                TotalCarbohydratePercent = 1.0,
                DietaryFiberWeight = 1.0,
                DietaryFiberPercent = 1.0,
                SolubleFiber = 1.0,
                InsolubleFiber = 1.0,
                Sugar = 1.0,
                SugarAlcohol = 1.0,
                OtherCarbohydrates = 1.0,
                ProteinWeight = 1.0,
                ProteinPercent = 1.0,
                VitaminA = 1.0,
                Betacarotene = 1.0,
                VitaminC = 1.0,
                Calcium = 1.0,
                Iron = 1.0,
                VitaminD = 1.0,
                VitaminE = 1.0,
                Thiamin = 1.0,
                Riboflavin = 1.0,
                Niacin = 1.0,
                VitaminB6 = 1.0,
                Folate = 1.0,
                VitaminB12 = 1.0,
                Biotin = 1.0,
                PantothenicAcid = 1.0,
                Phosphorous = 1.0,
                Iodine = 1.0,
                Magnesium = 1.0,
                Zinc = 1.0,
                Copper = 1.0,
                Transfat = 1.0,
                CaloriesFromTransfat = 1.0,
                Om6Fatty = 1.0,
                Om3Fatty = 1.0,
                Starch = 1.0,
                Chloride = 1.0,
                Chromium = 1.0,
                VitaminK = 1.0,
                Manganese = 1.0,
                Molybdenum = 1.0,
                Selenium = 1.0,
                TransfatWeight = 1.0,
                InsertDate = DateTime.Parse("2019-01-01"),
                ModifiedDate = DateTime.Parse("2019-01-01")
            },
            connectionHelper.ProviderFactory.Transaction)).First();

            return id;
        }

        public async Task<int> InsertProductSelectionGroup()
        {
            string query = $@"INSERT INTO [app].[ProductSelectionGroup]
           ([ProductSelectionGroupName]
           ,[ProductSelectionGroupTypeId]
           ,[MerchandiseHierarchyClassId])
           VALUES
           (@ProductSelectionGroupName,
           @ProductSelectionGroupTypeId,
           @MerchandiseHierarchyClassId);
           SELECT CAST(SCOPE_IDENTITY() AS INT);";

            int id = await this.connectionHelper.ProviderFactory.Provider.Connection.ExecuteAsync(query,
            new
            {
                ProductSelectionGroupName = "test",
                ProductSelectionGroupTypeId = 1,
                MerchandiseHierarchyClassId = 1
            },
            connectionHelper.ProviderFactory.Transaction);

            return id;
        }

        public async Task<int> InsertMessageQueueItem(int itemId)
        {
            int id = (await this.connectionHelper.ProviderFactory.Provider.Connection.QueryAsync<int>($@"INSERT INTO
                esb.MessageQueueItem
                (EsbReadyDateTimeUtc,
                ItemId,
                InsertDateUtc)
                VALUES
                (@esbReadyDateTimeUtc,
                @itemId,
                @insertDateUtc);
                SELECT CAST(SCOPE_IDENTITY() AS INT);",
            new
            {
                esbReadyDateTimeUtc = DateTime.Parse("2019-01-01"),
                itemId = itemId,
                insertDateUtc = DateTime.Parse("2019-01-01"),
            },
            connectionHelper.ProviderFactory.Transaction)).First();

            return id;
        }

        public async Task<MessageQueueItemArchive> SelectLatestInsertedRecord()
        {
            return await this.connectionHelper.ProviderFactory.Provider.Connection.QueryFirstAsync<MessageQueueItemArchive>($@"SELECT
                    TOP(1)
                    [MessageQueueItemJson]
                   ,[ErrorOccurred]
                   ,[ErrorMessage]
                   ,[Message]
                   ,[MessageHeader]
                   ,[WarningMessage]
                   ,[InsertDateUtc]
                   ,[Machine]
                   FROM [esb].[MessageQueueItemArchive]
                   ORDER BY MessageQueueItemArchiveId DESC",
                   null,
                   this.connectionHelper.ProviderFactory.Provider.Transaction);
        }
        public async Task<MessageDeadLetterQueue> SelectLatestInsertedDeadLeatterRecord()
        {
            string json = await this.connectionHelper.ProviderFactory.Provider.Connection.QueryFirstAsync<string>($@"SELECT
                    TOP(1)
                   [JsonObject]
                   FROM [esb].[MessageDeadLetterQueue]
                   ORDER BY MessageDeadLetterQueueId DESC",
                   null,
                   this.connectionHelper.ProviderFactory.Provider.Transaction);

            return JsonConvert.DeserializeObject<MessageDeadLetterQueue>(json);
        }


        public string SelectAttributeSql()
        {
            return $@"SELECT
           [DisplayName]
           ,[AttributeName]
           ,[AttributeGroupId]
           ,[HasUniqueValues]
           ,[Description]
           ,[DefaultValue]
           ,[RequiredForPublishing]
           ,[SpecialCharactersAllowed]
           ,[TraitCode]
           ,[DataTypeId]
           ,[DisplayOrder]
           ,[InitialValue]
           ,[IncrementBy]
           ,[InitialMax]
           ,[DisplayType]
           ,[MaxLengthAllowed]
           ,[NumberValidationRule]
           ,[IsPickList]
            FROM [dbo].[Attributes] )";
        }

        public string SelectNutritionSql()
        {
            return $@"SELECT
            [Plu]
           ,[RecipeName]
           ,[Allergens]
           ,[Ingredients]
           ,[ServingsPerPortion]
           ,[ServingSizeDesc]
           ,[ServingPerContainer]
           ,[HshRating]
           ,[ServingUnits]
           ,[SizeWeight]
           ,[Calories]
           ,[CaloriesFat]
           ,[CaloriesSaturatedFat]
           ,[TotalFatWeight]
           ,[TotalFatPercentage]
           ,[SaturatedFatWeight]
           ,[SaturatedFatPercent]
           ,[PolyunsaturatedFat]
           ,[MonounsaturatedFat]
           ,[CholesterolWeight]
           ,[CholesterolPercent]
           ,[SodiumWeight]
           ,[SodiumPercent]
           ,[PotassiumWeight]
           ,[PotassiumPercent]
           ,[TotalCarbohydrateWeight]
           ,[TotalCarbohydratePercent]
           ,[DietaryFiberWeight]
           ,[DietaryFiberPercent]
           ,[SolubleFiber]
           ,[InsolubleFiber]
           ,[Sugar]
           ,[SugarAlcohol]
           ,[OtherCarbohydrates]
           ,[ProteinWeight]
           ,[ProteinPercent]
           ,[VitaminA]
           ,[Betacarotene]
           ,[VitaminC]
           ,[Calcium]
           ,[Iron]
           ,[VitaminD]
           ,[VitaminE]
           ,[Thiamin]
           ,[Riboflavin]
           ,[Niacin]
           ,[VitaminB6]
           ,[Folate]
           ,[VitaminB12]
           ,[Biotin]
           ,[PantothenicAcid]
           ,[Phosphorous]
           ,[Iodine]
           ,[Magnesium]
           ,[Zinc]
           ,[Copper]
           ,[Transfat]
           ,[CaloriesFromTransfat]
           ,[Om6Fatty]
           ,[Om3Fatty]
           ,[Starch]
           ,[Chloride]
           ,[Chromium]
           ,[VitaminK]
           ,[Manganese]
           ,[Molybdenum]
           ,[Selenium]
           ,[TransfatWeight]
           ,[InsertDate]
           ,[ModifiedDate]
            FROM [nutrition].[ItemNutrition]";
        }

        public string SelectProductSelectionGroupSql()
        {
            return $@"SELECT
           [ProductSelectionGroupName]
           ,[ProductSelectionGroupTypeId]
           ,[TraitId]
           ,[TraitValue]
           ,[MerchandiseHierarchyClassId]
            FROM [app].[ProductSelectionGroup]";
        }

        public async Task<int> InsertDataType(string dataTypeName)
        {
            string query = $@"
                INSERT INTO dbo.DataType(DataType)
                VALUES (@dataTypeName)
                SELECT CAST(SCOPE_IDENTITY() AS INT)";

            return await this.connectionHelper.ProviderFactory.Provider.Connection.QueryFirstAsync<int>(
                query,
                new
                {
                    dataTypeName = dataTypeName
                },
                connectionHelper.ProviderFactory.Transaction);
        }
    }
}