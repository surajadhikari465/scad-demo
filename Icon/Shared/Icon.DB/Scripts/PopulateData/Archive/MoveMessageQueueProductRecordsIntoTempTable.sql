IF OBJECT_ID('tempdb..##TempMessageQueueProduct') IS NOT NULL 
	DROP TABLE ##TempMessageQueueProduct
GO
CREATE TABLE ##TempMessageQueueProduct
(
	[MessageQueueId] [int] NOT NULL,
	[MessageTypeId] [int] NOT NULL,
	[MessageStatusId] [int] NOT NULL,
	[MessageHistoryId] [int] NULL,
	[InsertDate] [datetime2](7) NOT NULL,
	[ItemId] [int] NOT NULL,
	[LocaleId] [int] NOT NULL,
	[ItemTypeCode] [nvarchar](3) NOT NULL,
	[ItemTypeDesc] [nvarchar](255) NOT NULL,
	[ScanCodeId] [int] NOT NULL,
	[ScanCode] [nvarchar](13) NOT NULL,
	[ScanCodeTypeId] [int] NOT NULL,
	[ScanCodeTypeDesc] [nvarchar](255) NOT NULL,
	[ProductDescription] [nvarchar](255) NOT NULL,
	[PosDescription] [nvarchar](255) NOT NULL,
	[PackageUnit] [nvarchar](255) NOT NULL,
	[RetailSize] [nvarchar](255) NULL,
	[RetailUom] [nvarchar](255) NULL,
	[FoodStampEligible] [nvarchar](255) NOT NULL,
	[ProhibitDiscount] [bit] NOT NULL,
	[DepartmentSale] [nvarchar](255) NOT NULL,
	[BrandId] [int] NOT NULL,
	[BrandName] [nvarchar](255) NOT NULL,
	[BrandLevel] [int] NOT NULL,
	[BrandParentId] [int] NULL,
	[BrowsingClassId] [int] NULL,
	[BrowsingClassName] [nvarchar](255) NULL,
	[BrowsingLevel] [int] NULL,
	[BrowsingParentId] [int] NULL,
	[MerchandiseClassId] [int] NOT NULL,
	[MerchandiseClassName] [nvarchar](255) NOT NULL,
	[MerchandiseLevel] [int] NOT NULL,
	[MerchandiseParentId] [int] NULL,
	[TaxClassId] [int] NOT NULL,
	[TaxClassName] [nvarchar](255) NOT NULL,
	[TaxLevel] [int] NOT NULL,
	[TaxParentId] [int] NULL,
	[FinancialClassId] [nvarchar](32) NOT NULL,
	[FinancialClassName] [nvarchar](255) NOT NULL,
	[FinancialLevel] [int] NOT NULL,
	[FinancialParentId] [int] NULL,
	[InProcessBy] [int] NULL,
	[ProcessedDate] [datetime2](7) NULL,
	[AnimalWelfareRating] [nvarchar](50) NULL,
	[Biodynamic] [nvarchar](1) NULL,
	[CheeseMilkType] [nvarchar](50) NULL,
	[CheeseRaw] [nvarchar](1) NULL,
	[EcoScaleRating] [nvarchar](50) NULL,
	[GlutenFreeAgency] [nvarchar](255) NULL,
	[HealthyEatingRating] [nvarchar](50) NULL,
	[KosherAgency] [nvarchar](50) NULL,
	[Msc] [nvarchar](1) NULL,
	[NonGmoAgency] [nvarchar](255) NULL,
	[OrganicAgency] [nvarchar](255) NULL,
	[PremiumBodyCare] [nvarchar](1) NULL,
	[SeafoodFreshOrFrozen] [nvarchar](50) NULL,
	[SeafoodCatchType] [nvarchar](50) NULL,
	[VeganAgency] [nvarchar](255) NULL,
	[Vegetarian] [nvarchar](1) NULL,
	[WholeTrade] [nvarchar](1) NULL,
	[GrassFed] [nvarchar](1) NULL,
	[PastureRaised] [nvarchar](1) NULL,
	[FreeRange] [nvarchar](1) NULL,
	[DryAged] [nvarchar](1) NULL,
	[AirChilled] [nvarchar](1) NULL,
	[MadeInHouse] [nvarchar](1) NULL,
	[CustomerFriendlyDescription] [nvarchar](60) NULL,
	[NutritionRequired] [nvarchar](255) NULL,
	[GlobalPricingProgram] [nvarchar](255) NULL
)

GO

INSERT INTO ##TempMessageQueueProduct
           ([MessageQueueId]
		   ,[MessageTypeId]
           ,[MessageStatusId]
           ,[MessageHistoryId]
           ,[InsertDate]
           ,[ItemId]
           ,[LocaleId]
           ,[ItemTypeCode]
           ,[ItemTypeDesc]
           ,[ScanCodeId]
           ,[ScanCode]
           ,[ScanCodeTypeId]
           ,[ScanCodeTypeDesc]
           ,[ProductDescription]
           ,[PosDescription]
           ,[PackageUnit]
           ,[RetailSize]
           ,[RetailUom]
           ,[FoodStampEligible]
           ,[ProhibitDiscount]
           ,[DepartmentSale]
           ,[BrandId]
           ,[BrandName]
           ,[BrandLevel]
           ,[BrandParentId]
           ,[BrowsingClassId]
           ,[BrowsingClassName]
           ,[BrowsingLevel]
           ,[BrowsingParentId]
           ,[MerchandiseClassId]
           ,[MerchandiseClassName]
           ,[MerchandiseLevel]
           ,[MerchandiseParentId]
           ,[TaxClassId]
           ,[TaxClassName]
           ,[TaxLevel]
           ,[TaxParentId]
           ,[FinancialClassId]
           ,[FinancialClassName]
           ,[FinancialLevel]
           ,[FinancialParentId]
           ,[InProcessBy]
           ,[ProcessedDate]
           ,[AnimalWelfareRating]
           ,[Biodynamic]
           ,[CheeseMilkType]
           ,[CheeseRaw]
           ,[EcoScaleRating]
           ,[GlutenFreeAgency]
           ,[HealthyEatingRating]
           ,[KosherAgency]
           ,[Msc]
           ,[NonGmoAgency]
           ,[OrganicAgency]
           ,[PremiumBodyCare]
           ,[SeafoodFreshOrFrozen]
           ,[SeafoodCatchType]
           ,[VeganAgency]
           ,[Vegetarian]
           ,[WholeTrade]
           ,[GrassFed]
           ,[PastureRaised]
           ,[FreeRange]
           ,[DryAged]
           ,[AirChilled]
           ,[MadeInHouse]
           ,[CustomerFriendlyDescription]
           ,[NutritionRequired]
           ,[GlobalPricingProgram])
SELECT [MessageQueueId]
      ,[MessageTypeId]
      ,[MessageStatusId]
      ,[MessageHistoryId]
      ,[InsertDate]
      ,[ItemId]
      ,[LocaleId]
      ,[ItemTypeCode]
      ,[ItemTypeDesc]
      ,[ScanCodeId]
      ,[ScanCode]
      ,[ScanCodeTypeId]
      ,[ScanCodeTypeDesc]
      ,[ProductDescription]
      ,[PosDescription]
      ,[PackageUnit]
      ,[RetailSize]
      ,[RetailUom]
      ,[FoodStampEligible]
      ,[ProhibitDiscount]
      ,[DepartmentSale]
      ,[BrandId]
      ,[BrandName]
      ,[BrandLevel]
      ,[BrandParentId]
      ,[BrowsingClassId]
      ,[BrowsingClassName]
      ,[BrowsingLevel]
      ,[BrowsingParentId]
      ,[MerchandiseClassId]
      ,[MerchandiseClassName]
      ,[MerchandiseLevel]
      ,[MerchandiseParentId]
      ,[TaxClassId]
      ,[TaxClassName]
      ,[TaxLevel]
      ,[TaxParentId]
      ,[FinancialClassId]
      ,[FinancialClassName]
      ,[FinancialLevel]
      ,[FinancialParentId]
      ,[InProcessBy]
      ,[ProcessedDate]
      ,[AnimalWelfareRating]
      ,[Biodynamic]
      ,[CheeseMilkType]
      ,[CheeseRaw]
      ,[EcoScaleRating]
      ,[GlutenFreeAgency]
      ,[HealthyEatingRating]
      ,[KosherAgency]
      ,[Msc]
      ,[NonGmoAgency]
      ,[OrganicAgency]
      ,[PremiumBodyCare]
      ,[SeafoodFreshOrFrozen]
      ,[SeafoodCatchType]
      ,[VeganAgency]
      ,[Vegetarian]
      ,[WholeTrade]
      ,[GrassFed]
      ,[PastureRaised]
      ,[FreeRange]
      ,[DryAged]
      ,[AirChilled]
      ,[MadeInHouse]
      ,[CustomerFriendlyDescription]
      ,[NutritionRequired]
      ,[GlobalPricingProgram]
  FROM [app].[MessageQueueProduct]
GO

IF OBJECT_ID('tempdb..##TempMessageQueueNutrition') IS NOT NULL 
	DROP TABLE ##TempMessageQueueNutrition
GO

CREATE TABLE ##TempMessageQueueNutrition
(
	[MessageQueueNutritionId] [int] NOT NULL,
	[MessageQueueId] [int] NOT NULL,
	[Plu] [varchar](50) NULL,
	[RecipeName] [nvarchar](100) NULL,
	[Allergens] [nvarchar](510) NULL,
	[Ingredients] [nvarchar](4000) NULL,
	[ServingsPerPortion] [float] NULL,
	[ServingSizeDesc] [nvarchar](50) NULL,
	[ServingPerContainer] [nvarchar](50) NULL,
	[HshRating] [int] NULL,
	[ServingUnits] [tinyint] NULL,
	[SizeWeight] [int] NULL,
	[Calories] [int] NULL,
	[CaloriesFat] [int] NULL,
	[CaloriesSaturatedFat] [int] NULL,
	[TotalFatWeight] [decimal](10, 1) NULL,
	[TotalFatPercentage] [smallint] NULL,
	[SaturatedFatWeight] [decimal](10, 1) NULL,
	[SaturatedFatPercent] [smallint] NULL,
	[PolyunsaturatedFat] [decimal](10, 1) NULL,
	[MonounsaturatedFat] [decimal](10, 1) NULL,
	[CholesterolWeight] [decimal](10, 1) NULL,
	[CholesterolPercent] [smallint] NULL,
	[SodiumWeight] [decimal](10, 1) NULL,
	[SodiumPercent] [smallint] NULL,
	[PotassiumWeight] [decimal](10, 1) NULL,
	[PotassiumPercent] [smallint] NULL,
	[TotalCarbohydrateWeight] [decimal](10, 1) NULL,
	[TotalCarbohydratePercent] [smallint] NULL,
	[DietaryFiberWeight] [decimal](10, 1) NULL,
	[DietaryFiberPercent] [smallint] NULL,
	[SolubleFiber] [decimal](10, 1) NULL,
	[InsolubleFiber] [decimal](10, 1) NULL,
	[Sugar] [decimal](10, 1) NULL,
	[SugarAlcohol] [decimal](10, 1) NULL,
	[OtherCarbohydrates] [decimal](10, 1) NULL,
	[ProteinWeight] [decimal](10, 1) NULL,
	[ProteinPercent] [smallint] NULL,
	[VitaminA] [smallint] NULL,
	[Betacarotene] [smallint] NULL,
	[VitaminC] [smallint] NULL,
	[Calcium] [smallint] NULL,
	[Iron] [smallint] NULL,
	[VitaminD] [smallint] NULL,
	[VitaminE] [smallint] NULL,
	[Thiamin] [smallint] NULL,
	[Riboflavin] [smallint] NULL,
	[Niacin] [smallint] NULL,
	[VitaminB6] [smallint] NULL,
	[Folate] [smallint] NULL,
	[VitaminB12] [smallint] NULL,
	[Biotin] [smallint] NULL,
	[PantothenicAcid] [smallint] NULL,
	[Phosphorous] [smallint] NULL,
	[Iodine] [smallint] NULL,
	[Magnesium] [smallint] NULL,
	[Zinc] [smallint] NULL,
	[Copper] [smallint] NULL,
	[Transfat] [decimal](10, 1) NULL,
	[CaloriesFromTransfat] [int] NULL,
	[Om6Fatty] [decimal](10, 1) NULL,
	[Om3Fatty] [decimal](10, 1) NULL,
	[Starch] [decimal](10, 1) NULL,
	[Chloride] [smallint] NULL,
	[Chromium] [smallint] NULL,
	[VitaminK] [smallint] NULL,
	[Manganese] [smallint] NULL,
	[Molybdenum] [smallint] NULL,
	[Selenium] [smallint] NULL,
	[TransfatWeight] [decimal](10, 1) NULL,
	[HazardousMaterialFlag] [int] NULL,
	[HazardousMaterialTypeCode] [varchar](4) NULL,
	[InsertDate] [datetime2](7) NOT NULL
)

GO

INSERT INTO ##TempMessageQueueNutrition
           ([MessageQueueNutritionId]
		   ,[MessageQueueId]
           ,[Plu]
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
           ,[HazardousMaterialFlag]
           ,[HazardousMaterialTypeCode]
           ,[InsertDate])
SELECT [MessageQueueNutritionId]
      ,[MessageQueueId]
      ,[Plu]
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
      ,[HazardousMaterialFlag]
      ,[HazardousMaterialTypeCode]
      ,[InsertDate]
  FROM [app].[MessageQueueNutrition]
GO

DELETE FROM app.MessageQueueNutrition

DELETE FROM app.MessageQueueProduct

GO


