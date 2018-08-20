CREATE PROCEDURE [dbo].[GetItemNutritionAttributes]
	@ItemIds AS [dbo].[IntListType] READONLY
AS
BEGIN

	SELECT  DISTINCT Value AS ItemId
	INTO #ItemIds
	FROM @ItemIds;

	CREATE NONCLUSTERED INDEX #ix_ItemIds ON #ItemIds (ItemId)

	SELECT 
		ian.ItemID, 
		ian.Calories, 
		ian.ServingUnits, 
		ian.ServingsPerPortion, 
		ian.ServingSizeDesc, 
		ian.ServingPerContainer
	FROM dbo.ItemAttributes_Nutrition ian
		INNER JOIN	#ItemIds i ON ian.ItemID = i.ItemId	

END
