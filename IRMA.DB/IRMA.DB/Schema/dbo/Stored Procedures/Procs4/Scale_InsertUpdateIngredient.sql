CREATE PROCEDURE dbo.Scale_InsertUpdateIngredient 
	@ID INT
	,@Description VARCHAR(50)
	,@Scale_LabelType_ID INT
	,@Ingredients VARCHAR(4200)
	,@NEW_ID INT OUTPUT
AS
BEGIN
	DECLARE @ItemKey INT

	IF @ID > 0
	BEGIN
		UPDATE Scale_Ingredient
		SET Description = @Description
			,Scale_LabelType_ID = @Scale_LabelType_ID
			,Ingredients = @Ingredients
		WHERE Scale_Ingredient_ID = @ID
	END
	ELSE
	BEGIN
		INSERT INTO Scale_Ingredient (
			Description
			,Scale_LabelType_ID
			,Ingredients
			)
		VALUES (
			@Description
			,@Scale_LabelType_ID
			,@Ingredients
			)

		SELECT @NEW_ID = SCOPE_IDENTITY()
	END
END
GO

GRANT EXECUTE
	ON OBJECT::[dbo].[Scale_InsertUpdateIngredient]
	TO [IRMAClientRole] AS [dbo];
GO

GRANT EXECUTE
	ON OBJECT::[dbo].[Scale_InsertUpdateIngredient]
	TO [IRMASLIMRole] AS [dbo];