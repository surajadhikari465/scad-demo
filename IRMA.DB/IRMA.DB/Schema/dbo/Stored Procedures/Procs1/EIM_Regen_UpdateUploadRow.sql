CREATE PROCEDURE dbo.EIM_Regen_UpdateUploadRow
		@UploadRow_ID int,
		@Item_Key int,
		@UploadSession_ID int,
		@Identifier varchar(13),
		@ValidationLevel int,
		@ItemRequest_ID int
		,
		@UpdateCount int OUTPUT
AS 

	-- This stored procedure can be found with all the
	-- Generated CRUD procedures for the UploadRow table in the
	-- \EIM_Regen_CRUD_Procedures\EIM_Regen_UploadRow_CRUD_Procs.sql file in the ItemCatalogDB project.
	-- Created By:	David Marine
	-- Created   :	Jul 29, 2007
	-- 20100422 - Dave Stacey - Split into seperate files

	UPDATE UploadRow
	SET
		[Item_Key] = @Item_Key,
		[UploadSession_ID] = @UploadSession_ID,
		[Identifier] = @Identifier,
		[ValidationLevel] = @ValidationLevel,
		[ItemRequest_ID] = @ItemRequest_ID
	WHERE
		UploadRow_ID = @UploadRow_ID
		
	SELECT @UpdateCount = @@ROWCOUNT
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EIM_Regen_UpdateUploadRow] TO [IRMAClientRole]
    AS [dbo];

