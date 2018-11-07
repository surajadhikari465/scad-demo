CREATE PROCEDURE [dbo].[InsertOrUpdateItemAllergen]
	@Scale_Allergen_ID		INT,
	@ItemKey				INT,
	@Description			VARCHAR(50),
	@Allergens				VARCHAR(4200)
AS 
BEGIN
	DECLARE @New_ID		INT
	DECLARE @ErrorCode	INT

	BEGIN TRANSACTION;

	BEGIN TRY
		;
		MERGE Scale_Allergen WITH (UPDLOCK, ROWLOCK) SAL
		USING	(SELECT @Scale_Allergen_ID AS Scale_Allergen_ID, 
				@Description AS [Description],
				@Allergens AS Allergens) Input
		ON SAL.Scale_Allergen_ID = Input.Scale_Allergen_ID
		WHEN MATCHED THEN
			UPDATE SET 
				SAL.[Description] = @Description,
				SAL.Allergens = @Allergens
		WHEN NOT MATCHED THEN
			INSERT ([Description], Allergens) VALUES (@Description, @Allergens);

		IF (@Scale_Allergen_ID = 0)
			SELECT @NEW_ID = SCOPE_IDENTITY()
		ELSE
			SELECT @NEW_ID = @Scale_Allergen_ID;

		;
		MERGE ItemNutrition WITH (UPDLOCK, ROWLOCK) INF
		USING (SELECT @ItemKey AS ItemKey, @New_ID AS Scale_Allergen_ID) Input
		ON INF.ItemKey = Input.ItemKey
		WHEN MATCHED THEN
			UPDATE SET
				INF.Scale_Allergen_ID = Input.Scale_Allergen_ID
		WHEN NOT MATCHED THEN
			INSERT (ItemKey, Scale_Allergen_ID) VALUES (@ItemKey, @New_ID);

	END TRY

	BEGIN CATCH
		IF @@TRANCOUNT > 0
			ROLLBACK TRANSACTION;
	END CATCH;
	
	IF @@TRANCOUNT > 0
		COMMIT TRANSACTION;

END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertOrUpdateItemAllergen] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertOrUpdateItemAllergen] TO [IRSUser]
    AS [dbo];

