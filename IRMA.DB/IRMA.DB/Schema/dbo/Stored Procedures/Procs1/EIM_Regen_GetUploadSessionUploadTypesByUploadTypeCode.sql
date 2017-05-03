CREATE PROCEDURE dbo.EIM_Regen_GetUploadSessionUploadTypesByUploadTypeCode
	@UploadType_Code varchar(50)
	
AS

	-- This stored procedure can be found with all the
	-- Generated CRUD procedures for the UploadSessionUploadType table in the
	-- \EIM_Regen_CRUD_Procedures\EIM_Regen_UploadSessionUploadType_CRUD_Procs.sql file in the ItemCatalogDB project.
	-- Created By:	David Marine
	-- Created   :	Jul 29, 2007
	-- 20100422 - Dave Stacey - Split into seperate files

	SELECT
		[UploadSessionUploadType_ID],
		[UploadSession_ID],
		[UploadType_Code],
		[UploadTypeTemplate_ID],
		[StoreSelectionType],
		[Zone_ID],
		[State]
	FROM UploadSessionUploadType (NOLOCK) 
	WHERE UploadType_Code = @UploadType_Code
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EIM_Regen_GetUploadSessionUploadTypesByUploadTypeCode] TO [IRMAClientRole]
    AS [dbo];

