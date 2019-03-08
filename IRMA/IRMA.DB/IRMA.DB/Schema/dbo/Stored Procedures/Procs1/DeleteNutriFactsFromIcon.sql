--Formatted by PoorSQL
CREATE PROCEDURE dbo.DeleteNutriFactsFromIcon @Identifiers dbo.IdentifiersType readonly
AS
BEGIN
	SET NOCOUNT ON;

	IF (object_id('tempdb..#scanCodes') IS NOT NULL)
		DROP TABLE #scanCodes;

	SELECT DISTINCT Identifier
	INTO #ScanCodes
	FROM @Identifiers;

	BEGIN TRY
		BEGIN TRANSACTION;

		DELETE
		FROM dbo.ItemNutrition
		WHERE ItemKey IN (
				SELECT DISTINCT A.Item_Key
				FROM ItemIdentifier A
				INNER JOIN #ScanCodes B ON B.Identifier = A.Identifier
				);

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

		COMMIT TRANSACTION;
	END TRY

	BEGIN CATCH
		DECLARE @errorMessage NVARCHAR(4000) = 'dbo.DeleteNutriFactsFromIcon failed: ' + ERROR_MESSAGE();

		ROLLBACK TRANSACTION;

		RAISERROR (
				@errorMessage
				,16
				,1
				);
	END CATCH

	IF (object_id('tempdb..#scanCodes') IS NOT NULL)
		DROP TABLE #scanCodes;

	SET NOCOUNT OFF;
END
GO

GRANT EXECUTE ON OBJECT::[dbo].[DeleteNutriFactsFromIcon] TO [IConInterface];
GO