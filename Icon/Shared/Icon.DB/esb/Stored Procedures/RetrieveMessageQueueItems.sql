CREATE PROCEDURE [esb].[RetrieveMessageQueueItems] 
	@itemIds esb.MessageQueueItemIdsType readonly
AS

BEGIN
	set nocount on;
	
  IF(object_id('tempdb..#itemIds') is not null) DROP TABLE #itemIds;

  SELECT DISTINCT itemId INTO #itemIds
  FROM @itemIds;
 

 select 
     [i].[itemId]
	,[i].[itemTypeId]
    ,[i].[itemAttributesJson]
	,[it].[ItemTypeDesc] as ItemTypeDescription
	,[it].[ItemTypeCode]
    ,[sc].[scanCode]
	,[sc].[scanCodeId]
	,[sct].[scanCodeTypeId]
    ,[sct].[scanCodeTypeDesc]
FROM dbo.Item i
JOIN dbo.ScanCode sc on sc.itemId = i.itemId
JOIN dbo.ScanCodeType sct on sct.scanCodeTypeId = sc. scanCodeTypeId
JOIN dbo.ItemType it on it.ItemTypeId = i.ItemTypeId
JOIN #itemIds on #itemIds.ItemId = i.itemID;

--if there is no nutrition record in main table but there is one in history table
--then SysEndTimeUtc will be in past and we know nutrition has been deleted (isdeleted will be true)
WITH NutritionDataWithHistory AS
(
   SELECT 
     ROW_NUMBER() OVER (PARTITION BY [Plu] ORDER BY SysStartTimeUtc DESC) AS rowNumber,
     [RecipeId]
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
    ,[InsertDate]
    ,[ModifiedDate]
    ,[AddedSugarsWeight]
    ,[AddedSugarsPercent]
    ,[CalciumWeight]
    ,[IronWeight]
    ,[VitaminDWeight]
	,CASE WHEN SysEndTimeUtc > SYSUTCDATETIME()  THEN 0 ELSE 1 END AS IsDeleted
FROM nutrition.ItemNutrition FOR SYSTEM_TIME all as inu
JOIN dbo.ScanCode sc on sc.scanCode = inu.Plu
JOIN #itemIds on #itemIds.ItemId = sc.itemID
)

SELECT 
     [RecipeId]
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
    ,[InsertDate]
    ,[ModifiedDate]
    ,[AddedSugarsWeight]
    ,[AddedSugarsPercent]
    ,[CalciumWeight]
    ,[IronWeight]
    ,[VitaminDWeight]
	,[IsDeleted]
FROM NutritionDataWithHistory
WHERE rowNumber = 1

select 
    ihc.hierarchyClassId,
    h.hierarchyId,
    h.hierarchyName,
    hc.hierarchyClassName,
	hc.hierarchyLevel,
    ihc.itemId,
	parenthc.hierarchyClassId HierarchyClassParentId,
	parenthc.hierarchyClassId HierarchyClassParentName

FROM dbo.ItemHierarchyClass ihc
JOIN dbo.HierarchyClass hc on hc.hierarchyClassId = ihc.hierarchyClassId
JOIN dbo.Hierarchy h on h.hierarchyId = hc.hierarchyId
JOIN #itemIds on #itemIds.ItemId = ihc.itemID
LEFT JOIN dbo.HierarchyClass parenthc on parenthc.hierarchyClassId = hc.hierarchyParentClassId

END