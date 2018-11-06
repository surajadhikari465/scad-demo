CREATE PROCEDURE dbo.EIM_OptimizedInsertUploadRow
		@Item_Key int,
		@UploadSession_ID int,
		@Identifier varchar(13),
		@ValidationLevel int,
		@ItemRequest_ID int,
		@ConcatonatedUploadValuesString VARCHAR(MAX)
		,
		@UploadValueIdsString VARCHAR(MAX) OUTPUT,
		@UploadRow_ID int OUTPUT
	
AS 

	-- This stored procedure can be found with all the
	-- Generated CRUD procedures for the UploadRow table in the
	-- \EIM_Regen_CRUD_Procedures\EIM_Regen_UploadRow_CRUD_Procs.sql file in the ItemCatalogDB project.
	-- Created By:	David Marine
	-- Created   :	Mar 08, 2007
	-- 20100422 - Dave Stacey - Split into seperate files

	BEGIN TRY

		INSERT INTO UploadRow
		(
			[Item_Key],
			[UploadSession_ID],
			[Identifier],
			[ValidationLevel],
			[ItemRequest_ID]
		)
		VALUES (
			@Item_Key,
			@UploadSession_ID,
			@Identifier,
			@ValidationLevel,
			@ItemRequest_ID
		)
		
		SELECT @UploadRow_ID  = SCOPE_IDENTITY()
				

		EXEC dbo.EIM_OptimizedSaveUploadValues
			'INSERT',
			@UploadRow_ID,
			@ConcatonatedUploadValuesString,
			@UploadValueIdsString OUTPUT
					
	END TRY
	BEGIN CATCH
		
		-- clean up the partially inserted row and values
		DELETE FROM UploadValue WHERE UploadRow_ID = @UploadRow_ID;
		DELETE FROM UploadRow WHERE UploadRow_ID = @UploadRow_ID;

		-- now throw an error
		RAISERROR ('An error occured during an optimized UploadRow insert.',11,1)
	
	END CATCH
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EIM_OptimizedInsertUploadRow] TO [IRMAClientRole]
    AS [dbo];

