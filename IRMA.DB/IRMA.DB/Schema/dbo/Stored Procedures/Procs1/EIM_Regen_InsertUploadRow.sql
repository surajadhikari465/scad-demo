CREATE PROCEDURE dbo.EIM_Regen_InsertUploadRow
		@Item_Key int,
			@UploadSession_ID int,
			@Identifier varchar(13),
			@ValidationLevel int,
			@ItemRequest_ID int
		,
		@UploadRow_ID int OUTPUT
	
AS 

	-- This stored procedure can be found with all the
	-- Generated CRUD procedures for the UploadRow table in the
	-- \EIM_Regen_CRUD_Procedures\EIM_Regen_UploadRow_CRUD_Procs.sql file in the ItemCatalogDB project.
	-- Created By:	David Marine
	-- Created   :	Jul 29, 2007
	-- 20100422 - Dave Stacey - Split into seperate files

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
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EIM_Regen_InsertUploadRow] TO [IRMAClientRole]
    AS [dbo];

