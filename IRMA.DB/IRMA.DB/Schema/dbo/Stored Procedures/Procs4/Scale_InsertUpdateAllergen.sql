CREATE PROCEDURE dbo.Scale_InsertUpdateAllergen 
	@ID INT
	,@Description VARCHAR(50)
	,@Scale_LabelType_ID INT
	,@Allergens VARCHAR(4200)
	,@NEW_ID INT OUTPUT
AS
BEGIN
	DECLARE @ItemKey INT

	IF @ID > 0
	BEGIN
		UPDATE Scale_Allergen
		SET Description = @Description
			,Scale_LabelType_ID = @Scale_LabelType_ID
			,Allergens = @Allergens
		WHERE Scale_Allergen_ID = @ID
	END
	ELSE
	BEGIN
		INSERT INTO Scale_Allergen (
			Description
			,Scale_LabelType_ID
			,Allergens
			)
		VALUES (
			@Description
			,@Scale_LabelType_ID
			,@Allergens
			)

		SELECT @NEW_ID = SCOPE_IDENTITY()
	END
END
GO

GRANT EXECUTE
	ON OBJECT::[dbo].[Scale_InsertUpdateAllergen]
	TO [IRMAClientRole] AS [dbo];
GO

GRANT EXECUTE
	ON OBJECT::[dbo].[Scale_InsertUpdateAllergen]
	TO [IRMASLIMRole] AS [dbo];