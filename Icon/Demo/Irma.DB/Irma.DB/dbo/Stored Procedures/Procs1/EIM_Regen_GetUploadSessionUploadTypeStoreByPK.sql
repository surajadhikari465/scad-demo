CREATE PROCEDURE dbo.EIM_Regen_GetUploadSessionUploadTypeStoreByPK
	@UploadSessionUploadTypeStore_ID int
	
AS

	-- This stored procedure can be found with all the
	-- Generated CRUD procedures for the UploadSessionUploadTypeStore table in the
	-- \EIM_Regen_CRUD_Procedures\EIM_Regen_UploadSessionUploadTypeStore_CRUD_Procs.sql file in the ItemCatalogDB project.
	-- Created By:	David Marine
	-- Created   :	Jul 29, 2007
	-- 20100422 - Dave Stacey - Split into seperate files

	SELECT
		[UploadSessionUploadTypeStore_ID],
		[UploadSessionUploadType_ID],
		[Store_No]
	FROM UploadSessionUploadTypeStore (NOLOCK) 
	WHERE UploadSessionUploadTypeStore_ID = @UploadSessionUploadTypeStore_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EIM_Regen_GetUploadSessionUploadTypeStoreByPK] TO [IRMAClientRole]
    AS [dbo];

