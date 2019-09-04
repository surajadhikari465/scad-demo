-- =============================================
-- Author:		Ed McNab
-- Create date: 2019-08-22
-- Description: Adds or updates an item-to-extra text nutrition override mapping
--    for the provided alternate jurisdiction
-- Date			Modified By		TFS		Description
-- 
-- =============================================

CREATE PROCEDURE [dbo].[InsertOrUpdateItemExtraTextOverride]
	@ExtraTextID		INT,
	@ItemKey			INT,
	@Description		VARCHAR(50),
	@Scale_LabelType_ID	INT,
	@ExtraText			VARCHAR(4200),
	@Jurisdiction       INT
AS 
BEGIN
	DECLARE @New_ID		INT
	DECLARE @ErrorCode	INT

	BEGIN TRANSACTION;

	BEGIN TRY
		;
		MERGE Item_ExtraText WITH (UPDLOCK, ROWLOCK) IET
		USING	(SELECT @ExtraTextID AS Item_ExtraText_ID, 
				@Scale_LabelType_ID AS Scale_LabelType_ID,
				@Description AS [Description],
				@ExtraText AS ExtraText) Input
		ON IET.Item_ExtraText_ID = Input.Item_ExtraText_ID
		WHEN MATCHED THEN
			UPDATE SET 
				IET.Scale_LabelType_ID = @Scale_LabelType_ID,
				IET.[Description] = @Description,
				IET.ExtraText = @ExtraText
		WHEN NOT MATCHED THEN
			INSERT (Scale_LabelType_ID, [Description], ExtraText) VALUES (@Scale_LabelType_ID, @Description, @ExtraText);

		IF (@ExtraTextID = 0)
			SELECT @NEW_ID = SCOPE_IDENTITY()
		ELSE
			SELECT @NEW_ID = @ExtraTextID;

		;
		MERGE ItemNutritionOverride WITH (UPDLOCK, ROWLOCK) INO
		USING (SELECT @ItemKey AS ItemKey, @New_ID AS ExtraTextID, @Jurisdiction as JurisdictionId) Input
		ON INO.ItemKey = Input.ItemKey
		WHEN MATCHED THEN
			UPDATE SET
				INO.Item_ExtraText_ID = Input.ExtraTextID
		WHEN NOT MATCHED THEN
			INSERT (ItemKey, Item_ExtraText_ID, StoreJurisdictionID) VALUES (@ItemKey, @New_ID, @Jurisdiction);

		-- Queue event for mammoth to refresh its data.
		EXEC [mammoth].[InsertItemLocaleChangeQueue] @ItemKey, NULL, 'ItemLocaleAddOrUpdate', NULL, @Jurisdiction

	END TRY

	BEGIN CATCH
		IF @@TRANCOUNT > 0
			ROLLBACK TRANSACTION;
	END CATCH;
	
	IF @@TRANCOUNT > 0
		COMMIT TRANSACTION;

END

print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Finish: [InsertOrUpdateItemExtraTextOverride.sql]'

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertOrUpdateItemExtraTextOverride] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertOrUpdateItemExtraTextOverride] TO [IRSUser]
    AS [dbo];

