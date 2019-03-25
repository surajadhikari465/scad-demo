--Formatted by PoorSQL
CREATE PROCEDURE dbo.DeleteNutriFactsFromIcon @Identifiers dbo.IdentifiersType readonly
AS
BEGIN
	SET NOCOUNT ON;

	IF (object_id('tempdb..#itemKeys') IS NOT NULL)
		DROP TABLE #itemKeys;

  IF (object_id('tempdb..#scanCodes') IS NOT NULL)
		DROP TABLE #scanCodes;

	SELECT DISTINCT Identifier
	INTO #ScanCodes
	FROM @Identifiers;

  SELECT DISTINCT A.Item_Key
  INTO #itemKeys
	FROM ItemIdentifier A
	INNER JOIN #ScanCodes B ON B.Identifier = A.Identifier

	BEGIN TRY
		BEGIN TRANSACTION;

    UPDATE A SET Scale_Allergen_ID = NULL, Scale_Ingredient_ID = NULL
    FROM dbo.ItemScale A
    INNER JOIN #itemKeys B on B.Item_Key = A.Item_Key;

		DELETE A
		FROM dbo.ItemNutrition A
    INNER JOIN #itemKeys B ON B.Item_Key = A.ItemKey

		DELETE A
		FROM dbo.NutriFactsChgQueue A
		INNER JOIN dbo.NutriFacts B ON B.NutriFactsID = A.NutriFactsID
		INNER JOIN #ScanCodes C ON C.Identifier = B.Description;

		DELETE A
		FROM dbo.NutriFacts A
		INNER JOIN #ScanCodes B ON B.Identifier = A.Description;
    
		DELETE A
		FROM dbo.Scale_Allergen A
		INNER JOIN #ScanCodes B ON B.Identifier = A.Description;

		DELETE A
		FROM dbo.Scale_Ingredient A
		INNER JOIN #ScanCodes B ON B.Identifier = A.Description;

    INSERT INTO PLUMCorpChgQueue(Item_Key, ActionCode, Store_No)
      SELECT DISTINCT C.Item_Key, 'C', D.Store_No
      FROM #ScanCodes A 
      INNER JOIN dbo.ItemIdentifier B ON B.Identifier = A.Identifier
      INNER JOIN dbo.Item C ON C.Item_Key = B.Item_Key
      INNER JOIN dbo.StoreItem D ON D.Item_Key = C.Item_Key
      LEFT JOIN PLUMCorpChgQueue E ON E.Item_Key = C.Item_Key AND E.Store_No = D.Store_No AND E.ActionCode = 'C'
      WHERE E.Item_Key IS NULL;

		COMMIT TRANSACTION;
	END TRY

	BEGIN CATCH
		DECLARE @errorMessage NVARCHAR(4000) = 'dbo.DeleteNutriFactsFromIcon failed: ' + ERROR_MESSAGE();

		ROLLBACK TRANSACTION;

		RAISERROR(@errorMessage, 16, 1);
	END CATCH

	IF (object_id('tempdb..#itemKeys') IS NOT NULL)
		DROP TABLE #itemKeys;

  IF (object_id('tempdb..#scanCodes') IS NOT NULL)
		DROP TABLE #scanCodes;

	SET NOCOUNT OFF;
END
GO

GRANT EXECUTE ON OBJECT::[dbo].[DeleteNutriFactsFromIcon] TO [IConInterface];
GO