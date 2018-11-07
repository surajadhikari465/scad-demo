CREATE PROCEDURE dbo.EIM_Regen_UpdateUploadSessionUploadTypeStore
		@UploadSessionUploadTypeStore_ID int,
		@UploadSessionUploadType_ID int,
		@Store_No int
		,
		@UpdateCount int OUTPUT
AS 

	-- This stored procedure can be found with all the
	-- Generated CRUD procedures for the UploadSessionUploadTypeStore table in the
	-- \EIM_Regen_CRUD_Procedures\EIM_Regen_UploadSessionUploadTypeStore_CRUD_Procs.sql file in the ItemCatalogDB project.
	-- Created By:	David Marine
	-- Created   :	Jul 29, 2007
	-- 20100422 - Dave Stacey - Split into seperate files

	UPDATE UploadSessionUploadTypeStore
	SET
		[UploadSessionUploadType_ID] = @UploadSessionUploadType_ID,
		[Store_No] = @Store_No
	WHERE
		UploadSessionUploadTypeStore_ID = @UploadSessionUploadTypeStore_ID
		
	SELECT @UpdateCount = @@ROWCOUNT
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EIM_Regen_UpdateUploadSessionUploadTypeStore] TO [IRMAClientRole]
    AS [dbo];

