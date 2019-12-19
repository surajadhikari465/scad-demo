CREATE PROCEDURE [nutrition].[DeleteNutritionItems] @Plu nutrition.Plu readonly
AS
BEGIN
	DECLARE @itemIDs app.UpdatedItemIDsType
	DECLARE @nutritionPlu TABLE (Plu VARCHAR(50));
	DECLARE @eventTypeID INT = (
			SELECT EventId
			FROM app.EventType
				WHERE EventName = 'Nutrition Delete'
			);

	IF (object_id('tempdb..#plu') IS NOT NULL)
		DROP TABLE #plu;

	SELECT DISTINCT Plu
	INTO #plu
	FROM @Plu;

	DELETE A
	OUTPUT deleted.Plu
	INTO @nutritionPlu
	FROM nutrition.ItemNutrition A
	INNER JOIN #plu B ON B.Plu = A.Plu;

	INSERT INTO app.EventQueue (
		EventID
		,EventMessage
		,InsertDate
		,RegionCode
		)
	SELECT @eventTypeID
		,Plu
		,SysDateTime()
		,regioncode
	FROM app.IRMAItemSubscription i 
	INNER JOIN @nutritionPlu np ON np.Plu = i.identifier

	--Generate messages
	INSERT INTO @itemIDs (itemID)
	SELECT DISTINCT A.itemID
	FROM ScanCode A
	INNER JOIN @nutritionPlu B ON B.Plu = A.scanCode;

	EXEC infor.GenerateItemUpdateMessages @updatedItemIDs = @itemIDs
		,@isDeleteNutrition = 1;

	SELECT Count(*) DeletedRecordCount
	FROM @nutritionPlu;

	IF (object_id('tempdb..#plu') IS NOT NULL)
		DROP TABLE #plu;
END
GO