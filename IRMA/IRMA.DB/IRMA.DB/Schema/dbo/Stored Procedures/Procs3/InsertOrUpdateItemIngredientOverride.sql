-- =============================================
-- Author:		Ed McNab
-- Create date: 2019-08-22
-- Description: Adds or updates an item-to-ingredient nutrition override mapping
--    for the provided alternate jurisdiction
-- Date			Modified By		TFS		Description
-- 
-- =============================================

CREATE PROCEDURE [dbo].[InsertOrUpdateItemIngredientOverride]
	@Scale_Ingredient_ID	INT,
	@ItemKey				INT,
	@Description			VARCHAR(50),
	@Ingredients			VARCHAR(4200),
	@Jurisdiction           INT
AS 
BEGIN
	DECLARE @New_ID		INT
	DECLARE @ErrorCode	INT

	BEGIN TRANSACTION;

	BEGIN TRY
		;
		MERGE Scale_Ingredient WITH (UPDLOCK, ROWLOCK) SIG
		USING	(SELECT @Scale_Ingredient_ID AS Scale_Ingredient_ID, 
				@Description AS [Description],
				@Ingredients AS Ingredients) Input
		ON SIG.Scale_Ingredient_ID = Input.Scale_Ingredient_ID
		WHEN MATCHED THEN
			UPDATE SET 
				SIG.[Description] = @Description,
				SIG.Ingredients = @Ingredients
		WHEN NOT MATCHED THEN
			INSERT ([Description], Ingredients) VALUES (@Description, @Ingredients);

		IF (@Scale_Ingredient_ID = 0)
			SELECT @NEW_ID = SCOPE_IDENTITY()
		ELSE
			SELECT @NEW_ID = @Scale_Ingredient_ID;

		;
		MERGE ItemNutritionOverride WITH (UPDLOCK, ROWLOCK) INO
		USING (SELECT @ItemKey AS ItemKey, @New_ID AS Scale_Ingredient_ID, @Jurisdiction as JurisdictionId) Input
		ON INO.ItemKey = Input.ItemKey AND INO.StoreJurisdictionID = Input.JurisdictionId
		WHEN MATCHED THEN
			UPDATE SET
				INO.Scale_Ingredient_ID = Input.Scale_Ingredient_ID
		WHEN NOT MATCHED THEN
			INSERT (ItemKey, Scale_Ingredient_ID, StoreJurisdictionID) VALUES (@ItemKey, @New_ID, @Jurisdiction);

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
    ON OBJECT::[dbo].[InsertOrUpdateItemIngredientOverride] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertOrUpdateItemIngredientOverride] TO [IRSUser]
    AS [dbo];

