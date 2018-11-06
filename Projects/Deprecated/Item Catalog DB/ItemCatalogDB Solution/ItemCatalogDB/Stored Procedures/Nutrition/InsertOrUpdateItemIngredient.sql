IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InsertOrUpdateItemIngredient]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[InsertOrUpdateItemIngredient]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[InsertOrUpdateItemIngredient]
	@Scale_Ingredient_ID	INT,
	@ItemKey				INT,
	@Description			VARCHAR(50),
	@Ingredients			VARCHAR(4200)
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
		MERGE ItemNutrition WITH (UPDLOCK, ROWLOCK) INF
		USING (SELECT @ItemKey AS ItemKey, @New_ID AS Scale_Ingredient_ID) Input
		ON INF.ItemKey = Input.ItemKey
		WHEN MATCHED THEN
			UPDATE SET
				INF.Scale_Ingredient_ID = Input.Scale_Ingredient_ID
		WHEN NOT MATCHED THEN
			INSERT (ItemKey, Scale_Ingredient_ID) VALUES (@ItemKey, @New_ID);

	END TRY

	BEGIN CATCH
		IF @@TRANCOUNT > 0
			ROLLBACK TRANSACTION;
	END CATCH;
	
	IF @@TRANCOUNT > 0
		COMMIT TRANSACTION;

END
GO
