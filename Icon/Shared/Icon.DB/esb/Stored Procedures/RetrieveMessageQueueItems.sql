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
JOIN #itemIds on #itemIds.ItemId = i.itemID

select 
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
FROM nutrition.ItemNutrition inu
JOIN dbo.ScanCode sc on sc.scanCode = inu.Plu
JOIN #itemIds on #itemIds.ItemId = sc.itemID

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
