CREATE PROCEDURE dbo.EIM_Regen_GetUploadTypeAttributesByUploadAttributeID
	@UploadAttribute_ID int
	
AS

	-- This stored procedure can be found with all the
	-- Generated CRUD procedures for the UploadTypeAttribute table in the
	-- \EIM_Regen_CRUD_Procedures\EIM_Regen_UploadTypeAttribute_CRUD_Procs.sql file in the ItemCatalogDB project.
	-- Created By:	David Marine
	-- Created   :	Jul 29, 2007
	-- 20100422 - Dave Stacey - Split into seperate files

	SELECT
		[UploadTypeAttribute_ID],
		[UploadType_Code],
		[UploadAttribute_ID],
		[IsRequiredForUploadTypeForExistingItems],
		[IsReadOnlyForExistingItems],
		[IsHidden],
		[GridPosition],
		[IsRequiredForUploadTypeForNewItems],
		[IsReadOnlyForNewItems],
		[GroupName]
	FROM UploadTypeAttribute (NOLOCK) 
	WHERE UploadAttribute_ID = @UploadAttribute_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EIM_Regen_GetUploadTypeAttributesByUploadAttributeID] TO [IRMAClientRole]
    AS [dbo];

