USE Staging
GO

SET NOCOUNT ON
GO

-- MAMMOTH . DEV . 02A . ETL . Staging - Icon
-- Populates Icon staging tables from Icon

DECLARE @ProjectName NVARCHAR(255)
DECLARE @Environment NCHAR(3)
DECLARE @ScriptName NVARCHAR(255)
DECLARE @Msg NVARCHAR(255)
DECLARE @MsgWidth INT
DECLARE @TableName NVARCHAR(128)
DECLARE @Operator sysname
DECLARE @DataSource sysname

SET @ProjectName = 'MAMMOTH';
SET @Environment = 'QA';
SET @ScriptName = '02A. ETL. Extract - Icon.sql'
SELECT @Operator = SUSER_NAME()
SET @DataSource = '[QA-ICON]'
SET @MsgWidth = 80

PRINT 'Project: ' + @ProjectName
PRINT 'Environment: ' + @Environment
PRINT 'Script: ' + @ScriptName
PRINT REPLICATE('-', @MsgWidth)
PRINT 'Data sources: [QA-ICON] = IDQ-ICON\SQLSHARED3Q.Icon'

-- 01.  Clean Staging Tables

SET @Msg = 'Truncating staging tables'
SET @Msg += REPLICATE('.', @MsgWidth - LEN(@Msg))
PRINT @Msg

TRUNCATE TABLE Staging.icon.Hierarchy
TRUNCATE TABLE Staging.icon.HierarchyClass
TRUNCATE TABLE Staging.icon.HierarchyClassTrait
TRUNCATE TABLE Staging.icon.Item
TRUNCATE TABLE Staging.icon.ItemHierarchyClass
TRUNCATE TABLE Staging.icon.ItemTrait
TRUNCATE TABLE Staging.icon.ItemType
TRUNCATE TABLE Staging.icon.Locale
TRUNCATE TABLE Staging.icon.LocaleTrait
TRUNCATE TABLE Staging.icon.ScanCode
TRUNCATE TABLE Staging.icon.Trait
TRUNCATE TABLE Staging.icon.TraitGroup
TRUNCATE TABLE Staging.icon.AnimalWelfareRating
TRUNCATE TABLE Staging.icon.ItemSignAttribute
TRUNCATE TABLE Staging.icon.MilkType
TRUNCATE TABLE Staging.icon.EcoScaleRating
TRUNCATE TABLE Staging.icon.SeafoodFreshOrFrozen
TRUNCATE TABLE Staging.icon.SeafoodCatchType
TRUNCATE TABLE Staging.icon.HealthyEatingRating
TRUNCATE TABLE Staging.icon.ItemNutrition
TRUNCATE TABLE Staging.etl.Traits

-- 02.  Populate configuration tables
SET @Msg = 'Populating trait list'
SET @Msg += REPLICATE('.', @MsgWidth - LEN(@Msg))
PRINT @Msg

INSERT INTO etl.Traits (TraitGroupID, TraitDesc) VALUES
	  (1, 'Product Description')
	, (1, 'POS Description')
	, (1, 'Retail UOM')
	, (1, 'Retail Size')
	, (1, 'Package Unit')
	, (1, 'Validation Date')
	, (1, 'Food Stamp Eligible')
	, (5, 'PS Business Unit ID')
	, (5, 'Store Abbreviation')
	, (6, 'Merch Fin Mapping')
	, (7, 'Financial Hierarchy Code')
	, (7, 'POS Department Number')

SET @Msg = 'Populating staging tables'
SET @Msg += REPLICATE('.', @MsgWidth - LEN(@Msg))
PRINT @Msg

-- 03.  Populate the staging tables
SET @TableName = 'dbo.Hierarchy'
SET @Msg = '...Table: ' + @TableName + ' DataSource: ' + @DataSource
SET @Msg += REPLICATE('.', @MsgWidth - LEN(@Msg))
PRINT @Msg

INSERT INTO Staging.icon.Hierarchy (hierarchyID, hierarchyName)
SELECT hierarchyID, hierarchyName
FROM [QA - ICON].[Icon02].[dbo].Hierarchy h

SET @TableName = 'dbo.HierarchyClass'
SET @Msg = '...Table: ' + @TableName + ' DataSource: ' + @DataSource
SET @Msg += REPLICATE('.', @MsgWidth - LEN(@Msg))
PRINT @Msg

INSERT INTO Staging.icon.HierarchyClass (hierarchyClassID, hierarchyLevel, hierarchyID, hierarchyParentClassID, hierarchyClassName)
SELECT hierarchyClassID, hierarchyLevel, hc.hierarchyID, hierarchyParentClassID, hierarchyClassName
FROM [QA - ICON].[Icon02].[dbo].HierarchyClass hc
JOIN [QA - ICON].[Icon02].[dbo].Hierarchy h on hc.hierarchyID = h.hierarchyID
WHERE h.hierarchyName IN ('Merchandise', 'Tax', 'Brands', 'Financial', 'National') -- Merchandise, TaxClass, Brand, Financial, National Class

SET @TableName = 'dbo.HierarchyClassTrait'
SET @Msg = '...Table: ' + @TableName + ' DataSource: ' + @DataSource
SET @Msg += REPLICATE('.', @MsgWidth - LEN(@Msg))
PRINT @Msg

INSERT INTO Staging.icon.HierarchyClassTrait ( traitID , hierarchyClassID , uomID , traitValue)
SELECT hct01.traitID, hct01.HierarchyClassID, hct01.uomID, hct01.TraitValue
FROM [QA - ICON].[Icon02].[dbo].HierarchyClassTrait hct01
INNER JOIN [QA - ICON].[Icon02].[dbo].Trait t01 ON (hct01.TraitID = t01.TraitID)
INNER JOIN Staging.etl.Traits t02 ON (t01.traitDesc= t02.traitDesc)

SET @TableName = 'dbo.Trait'
SET @Msg = '...Table: ' + @TableName + ' DataSource: ' + @DataSource
SET @Msg += REPLICATE('.', @MsgWidth - LEN(@Msg))
PRINT @Msg

INSERT INTO Staging.icon.Trait (traitID, traitCode, traitPattern, traitDesc, traitGroupID)
SELECT t1.traitID, t1.traitCode, t1.traitPattern, t1.traitDesc, t1.traitGroupID
FROM [QA - ICON].[Icon02].[dbo].Trait t1
INNER JOIN Staging.etl.Traits t2 ON (t1.TraitDesc = t2.TraitDesc)

SET @TableName = 'dbo.ItemTrait'
SET @Msg = '...Table: ' + @TableName + ' DataSource: ' + @DataSource
SET @Msg += REPLICATE('.', @MsgWidth - LEN(@Msg))
PRINT @Msg

INSERT INTO Staging.icon.ItemTrait( traitID , itemID , uomID , traitValue ,localeID)
SELECT it.traitID, it.itemID, it.uomID, it.traitValue, it.localeID
FROM [QA - ICON].[Icon02].[dbo].ItemTrait it
INNER JOIN Staging.icon.Trait tt ON (it.traitID = tt.traitID)

SET @TableName = 'dbo.TraitGroup'
SET @Msg = '...Table: ' + @TableName + ' DataSource: ' + @DataSource
SET @Msg += REPLICATE('.', @MsgWidth - LEN(@Msg))
PRINT @Msg

INSERT INTO Staging.icon.TraitGroup (traitGroupID, traitGroupCode, traitGroupDesc)
SELECT TraitGroupID, TraitGroupCode, TraitGroupDesc
FROM [QA - ICON].[Icon02].[dbo].TraitGroup

SET @TableName = 'dbo.ItemType'
SET @Msg = '...Table: ' + @TableName + ' DataSource: ' + @DataSource
SET @Msg += REPLICATE('.', @MsgWidth - LEN(@Msg))
PRINT @Msg

INSERT INTO Staging.icon.ItemType (itemTypeID, itemTypeCode, itemTypeDesc)
SELECT itemTypeID, itemTypeCode, itemTypeDesc
FROM [QA - ICON].[Icon02].[dbo].ItemType

SET @TableName = 'dbo.Item'
SET @Msg = '...Table: ' + @TableName + ' DataSource: ' + @DataSource
SET @Msg += REPLICATE('.', @MsgWidth - LEN(@Msg))
PRINT @Msg

INSERT INTO Staging.icon.Item (ItemID, ItemTypeID)
SELECT ItemID, ItemTypeID
FROM [QA - ICON].[Icon02].[dbo].Item

SET @TableName = 'dbo.ScanCode'
SET @Msg = '...Table: ' + @TableName + ' DataSource: ' + @DataSource
SET @Msg += REPLICATE('.', @MsgWidth - LEN(@Msg))
PRINT @Msg

INSERT INTO Staging.icon.ScanCode( scanCodeID ,itemID ,scanCode ,scanCodeTypeID ,localeID)
SELECT scanCodeID, itemID, scanCode, scanCodeTypeID, localeID
FROM [QA - ICON].[Icon02].[dbo].scancode

SET @TableName = 'dbo.Locale'
SET @Msg = '...Table: ' + @TableName + ' DataSource: ' + @DataSource
SET @Msg += REPLICATE('.', @MsgWidth - LEN(@Msg))
PRINT @Msg

INSERT INTO Staging.icon.Locale (localeID, ownerOrgPartyID, localeName, localeOpenDate, localeCloseDate, localeTypeID, parentLocaleID)
SELECT localeID, ownerOrgPartyID, localeName, localeOpenDate, localeCloseDate, localeTypeID, parentLocaleID
FROM [QA - ICON].[Icon02].[dbo].Locale

SET @TableName = 'dbo.LocaleTrait'
SET @Msg = '...Table: ' + @TableName + ' DataSource: ' + @DataSource
SET @Msg += REPLICATE('.', @MsgWidth - LEN(@Msg))
PRINT @Msg

INSERT INTO Staging.icon.LocaleTrait (traitID, localeID, uomID, traitValue)
SELECT traitID, localeID, uomID, traitValue
FROM [QA - ICON].[Icon02].[dbo].LocaleTrait

SET @TableName = 'dbo.ItemHierarchyClass'
SET @Msg = '...Table: ' + @TableName + ' DataSource: ' + @DataSource
SET @Msg += REPLICATE('.', @MsgWidth - LEN(@Msg))
PRINT @Msg

INSERT INTO Staging.icon.ItemHierarchyClass ( itemID, hierarchyClassID, localeID)
SELECT ItemID, hierarchyClassID, localeID
FROM [QA - ICON].[Icon02].[dbo].ItemHierarchyClass

SET @TableName = 'icon.AnimalWelfareRating'
SET @Msg = '...Table: ' + @TableName + ' DataSource: ' + @DataSource
SET @Msg += REPLICATE('.', @MsgWidth - LEN(@Msg))
PRINT @Msg

INSERT INTO [Staging].[icon].[AnimalWelfareRating] (AnimalWelfareRatingID, [Description])
SELECT AnimalWelfareRatingID, [Description]
FROM [QA - ICON].[Icon02].[dbo].AnimalWelfareRating

SET @TableName = 'icon.EcoScaleRating'
SET @Msg = '...Table: ' + @TableName + ' DataSource: ' + @DataSource
SET @Msg += REPLICATE('.', @MsgWidth - LEN(@Msg))
PRINT @Msg

INSERT INTO [Staging].[icon].[EcoScaleRating] (EcoScaleRatingId, [Description])
SELECT EcoScaleRatingID, [Description]
FROM [QA - ICON].[Icon02].[dbo].EcoScaleRating


SET @TableName = 'icon.MilkType'
SET @Msg = '...Table: ' + @TableName + ' DataSource: ' + @DataSource
SET @Msg += REPLICATE('.', @MsgWidth - LEN(@Msg))
PRINT @Msg

INSERT INTO [Staging].[icon].[MilkType] (MilkTypeId, [Description])
SELECT MilkTypeID, [Description]
FROM [QA - ICON].[Icon02].[dbo].MilkType

SET @TableName = 'icon.HealthyEatingRating'
SET @Msg = '...Table: ' + @TableName + ' DataSource: ' + @DataSource
SET @Msg += REPLICATE('.', @MsgWidth - LEN(@Msg))
PRINT @Msg

INSERT INTO [Staging].[icon].[HealthyEatingRating] (HealthyEatingRatingId, [Description])
SELECT HealthyEatingRatingId, [Description]
FROM [QA - ICON].[Icon02].[dbo].HealthyEatingRating

SET @TableName = 'icon.SeafoodCatchType'
SET @Msg = '...Table: ' + @TableName + ' DataSource: ' + @DataSource
SET @Msg += REPLICATE('.', @MsgWidth - LEN(@Msg))
PRINT @Msg

INSERT INTO [Staging].[icon].[SeafoodCatchType] (SeafoodCatchTypeId, [Description])
SELECT SeafoodCatchTypeId, [Description]
FROM [QA - ICON].[Icon02].[dbo].SeafoodCatchType

SET @TableName = 'icon.SeafoodFreshOrFrozen'
SET @Msg = '...Table: ' + @TableName + ' DataSource: ' + @DataSource
SET @Msg += REPLICATE('.', @MsgWidth - LEN(@Msg))
PRINT @Msg

INSERT INTO [Staging].[icon].[SeafoodFreshOrFrozen] (SeafoodFreshOrFrozenId, [Description])
SELECT SeafoodFreshOrFrozenId, [Description]
FROM [QA - ICON].[Icon02].[dbo].SeafoodFreshOrFrozen

SET @TableName = 'icon.ItemNutrition'
SET @Msg = '...Table: ' + @TableName + ' DataSource: ' + @DataSource
SET @Msg += REPLICATE('.', @MsgWidth - LEN(@Msg))
PRINT @Msg

INSERT INTO [Staging].[icon].[ItemNutrition] (RecipeID, PLU, RecipeName, Allergens, Ingredients, ServingsPerPortion, ServingSizeDesc, ServingPerContainer, HshRating, ServingUnits, SizeWeight, Calories, CaloriesFat, CaloriesSaturatedFat, TotalFatWeight, TotalFatPercentage, SaturatedFatWeight, SaturatedFatPercent, PolyunsaturatedFat, MonounsaturatedFat, CholesterolWeight, CholesterolPercent, SodiumWeight, SodiumPercent, PotassiumWeight, PotassiumPercent, TotalCarbohydrateWeight, TotalCarbohydratePercent, DietaryFiberWeight, DietaryFiberPercent, SolubleFiber, InsolubleFiber, Sugar, SugarAlcohol, OtherCarbohydrates, ProteinWeight, ProteinPercent, VitaminA, Betacarotene, VitaminC, Calcium, Iron, VitaminD, VitaminE, Thiamin, Riboflavin, Niacin, VitaminB6, Folate, VitaminB12, Biotin, PantothenicAcid, Phosphorous, Iodine, Magnesium, Zinc, Copper, Transfat, CaloriesFromTransfat, Om6Fatty, Om3Fatty, Starch, Chloride, Chromium, VitaminK, Manganese, Molybdenum, Selenium, TransfatWeight, InsertDate, ModifiedDate)
SELECT RecipeID, PLU, RecipeName, Allergens, Ingredients, ServingsPerPortion, ServingSizeDesc, ServingPerContainer, HshRating, ServingUnits, SizeWeight, Calories, CaloriesFat, CaloriesSaturatedFat, TotalFatWeight, TotalFatPercentage, SaturatedFatWeight, SaturatedFatPercent, PolyunsaturatedFat, MonounsaturatedFat, CholesterolWeight, CholesterolPercent, SodiumWeight, SodiumPercent, PotassiumWeight, PotassiumPercent, TotalCarbohydrateWeight, TotalCarbohydratePercent, DietaryFiberWeight, DietaryFiberPercent, SolubleFiber, InsolubleFiber, Sugar, SugarAlcohol, OtherCarbohydrates, ProteinWeight, ProteinPercent, VitaminA, Betacarotene, VitaminC, Calcium, Iron, VitaminD, VitaminE, Thiamin, Riboflavin, Niacin, VitaminB6, Folate, VitaminB12, Biotin, PantothenicAcid, Phosphorous, Iodine, Magnesium, Zinc, Copper, Transfat, CaloriesFromTransfat, Om6Fatty, Om3Fatty, Starch, Chloride, Chromium, VitaminK, Manganese, Molybdenum, Selenium, TransfatWeight, InsertDate, ModifiedDate
FROM [QA - ICON].[Icon02].nutrition.ItemNutrition

SET @TableName = 'icon.ItemSignAttribute'
SET @Msg = '...Table: ' + @TableName + ' DataSource: ' + @DataSource
SET @Msg += REPLICATE('.', @MsgWidth - LEN(@Msg))
PRINT @Msg

INSERT INTO [Staging].[icon].[ItemSignAttribute] (ItemSignAttributeID, ItemID, AnimalWelfareRatingId, Biodynamic, CheeseMilkTypeId, CheeseRaw, EcoScaleRatingId, GlutenFreeAgencyId, HealthyEatingRatingId, KosherAgencyId, Msc, NonGmoAgencyId, OrganicAgencyId, PremiumBodyCare, SeafoodFreshOrFrozenId, SeafoodCatchTypeId, VeganAgencyId, Vegetarian, WholeTrade, GrassFed, PastureRaised, FreeRange, DryAged, AirChilled, MadeInHouse)
SELECT ItemSignAttributeID, ItemID, AnimalWelfareRatingId, Biodynamic, CheeseMilkTypeId, CheeseRaw, EcoScaleRatingId, GlutenFreeAgencyId, HealthyEatingRatingId, KosherAgencyId, Msc, NonGmoAgencyId, OrganicAgencyId, PremiumBodyCare, SeafoodFreshOrFrozenId, SeafoodCatchTypeId, VeganAgencyId, Vegetarian, WholeTrade, GrassFed, PastureRaised, FreeRange, DryAged, AirChilled, MadeInHouse
FROM [QA - ICON].[Icon02].[dbo].ItemSignAttribute
