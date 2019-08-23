-- =============================================
-- Author:		Ed McNab
-- Create date: 2019-08-22
-- Description: Adds or updates an item-to-allergen nutrition override mapping
--    for the provided alternate jurisdiction
-- Date			Modified By		TFS		Description
-- 
-- =============================================

CREATE PROCEDURE [dbo].[InsertOrUpdateItemAllergenOverride]
	@Scale_Allergen_ID		INT,
	@ItemKey				INT,
	@Description			VARCHAR(50),
	@Allergens				VARCHAR(4200),
	@Jurisdiction           INT
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
		MERGE ItemNutritionOverride WITH (UPDLOCK, ROWLOCK) INO
		USING (SELECT @ItemKey AS ItemKey, @New_ID AS Scale_Allergen_ID, @Jurisdiction as JurisdictionId) Input
		ON INO.ItemKey = Input.ItemKey AND INO.StoreJurisdictionID = Input.JurisdictionId
		WHEN MATCHED THEN
			UPDATE SET
				INO.Scale_Allergen_ID = Input.Scale_Allergen_ID
		WHEN NOT MATCHED THEN
			INSERT (ItemKey, Scale_Allergen_ID, StoreJurisdictionID) VALUES (@ItemKey, @New_ID, @Jurisdiction);

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
    ON OBJECT::[dbo].[InsertOrUpdateItemAllergenOverride] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertOrUpdateItemAllergenOverride] TO [IRSUser]
    AS [dbo];

