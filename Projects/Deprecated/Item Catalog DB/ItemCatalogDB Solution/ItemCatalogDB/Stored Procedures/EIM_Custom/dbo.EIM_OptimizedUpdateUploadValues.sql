
--=====================================================================
--*********      dbo.EIM_OptimizedUpdateUploadValues                    
--=====================================================================

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[EIM_OptimizedSaveUploadValues]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[EIM_OptimizedSaveUploadValues]
GO
CREATE PROCEDURE dbo.EIM_OptimizedSaveUploadValues
		@InsertOrUpdate VARCHAR(6),
		@UploadRow_ID int,
		@ConcatonatedUploadValuesString VARCHAR(MAX),
		@UploadValueIdsString VARCHAR(MAX) OUTPUT

AS
	
DECLARE
	@ConcatonatedUploadValueProperties VARCHAR(8000),
	@PropertyValue VARCHAR(4500),
	@UploadValue_ID INT,
	@UploadAttribute_ID INT,
	@Value VARCHAR(4500),
	@UploadValueID INT,
	@PropertyIndex INT

SELECT @UploadValueIdsString = ''

DECLARE uploadValues_cursor CURSOR FOR
	SELECT Key_Value
	FROM
	dbo.fn_ParseStringList(@ConcatonatedUploadValuesString, '|')

OPEN uploadValues_cursor
FETCH NEXT FROM uploadValues_cursor INTO @ConcatonatedUploadValueProperties

WHILE @@FETCH_STATUS = 0
BEGIN
	
	DECLARE uploadValueProperties_cursor CURSOR FOR
		SELECT Key_Value
		FROM
		dbo.fn_ParseStringList(@ConcatonatedUploadValueProperties, '^')

	OPEN uploadValueProperties_cursor
	FETCH NEXT FROM uploadValueProperties_cursor INTO @PropertyValue

	SET @PropertyIndex = 0

	WHILE @@FETCH_STATUS = 0
	BEGIN

		IF @PropertyIndex = 0
		BEGIN
			SET @UploadValue_ID = CAST(@PropertyValue AS INT)
		END
		ELSE IF @PropertyIndex = 1
		BEGIN
			SET @UploadAttribute_ID = CAST(@PropertyValue AS INT)
		END
		ELSE IF @PropertyIndex = 2
		BEGIN
			IF LEN(@PropertyValue) > 0
				SET @Value = @PropertyValue
			ELSE
				SET @Value = NULL
		END
				
		SET @PropertyIndex = @PropertyIndex + 1
		
		FETCH NEXT FROM uploadValueProperties_cursor INTO @PropertyValue

	END

	CLOSE uploadValueProperties_cursor
	DEALLOCATE uploadValueProperties_cursor

	If @InsertOrUpdate = 'INSERT'
		BEGIN

		EXEC dbo.EIM_Regen_InsertUploadValue
				@UploadAttribute_ID,
				@UploadRow_ID,
				@Value,
				@UploadValueID OUTPUT
		
		SELECT @UploadValueIdsString = @UploadValueIdsString + CAST(@UploadValueID AS VARCHAR(38)) + '|' 
		END
	ELSE
		BEGIN

		EXEC dbo.EIM_Regen_UpdateUploadValue
				@UploadValue_ID,
				@UploadAttribute_ID,
				@UploadRow_ID,
				@Value,
				@UploadValueID OUTPUT
		END

	FETCH NEXT FROM uploadValues_cursor INTO @ConcatonatedUploadValueProperties
END

CLOSE uploadValues_cursor
DEALLOCATE uploadValues_cursor

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
