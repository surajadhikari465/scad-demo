IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InsertOrUpdateItemExtraText]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[InsertOrUpdateItemExtraText]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[InsertOrUpdateItemExtraText]
	@ExtraTextID		INT,
	@ItemKey			INT,
	@Description		VARCHAR(50),
	@Scale_LabelType_ID	INT,
	@ExtraText			VARCHAR(4200)
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
		MERGE ItemNutrition WITH (UPDLOCK, ROWLOCK) INF
		USING (SELECT @ItemKey AS ItemKey, @New_ID AS ExtraTextID) Input
		ON INF.ItemKey = Input.ItemKey
		WHEN MATCHED THEN
			UPDATE SET
				INF.Item_ExtraText_ID = Input.ExtraTextID
		WHEN NOT MATCHED THEN
			INSERT (ItemKey, Item_ExtraText_ID) VALUES (@ItemKey, @New_ID);

		-- Queue event for mammoth to refresh its data.
		EXEC [mammoth].[InsertItemLocaleChangeQueue] @ItemKey, NULL, 'ItemLocaleAddOrUpdate', NULL, NULL

	END TRY

	BEGIN CATCH
		IF @@TRANCOUNT > 0
			ROLLBACK TRANSACTION;
	END CATCH;
	
	IF @@TRANCOUNT > 0
		COMMIT TRANSACTION;

END