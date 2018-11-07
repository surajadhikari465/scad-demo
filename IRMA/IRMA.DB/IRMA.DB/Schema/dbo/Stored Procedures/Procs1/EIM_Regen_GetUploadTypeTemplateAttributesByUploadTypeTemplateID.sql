CREATE PROCEDURE dbo.EIM_Regen_GetUploadTypeTemplateAttributesByUploadTypeTemplateID
	@UploadTypeTemplate_ID int
	
AS

	-- This stored procedure can be found with all the
	-- Generated CRUD procedures for the UploadTypeTemplateAttribute table in the
	-- \EIM_Regen_CRUD_Procedures\EIM_Regen_UploadTypeTemplateAttribute_CRUD_Procs.sql file in the ItemCatalogDB project.
	-- Created By:	David Marine
	-- Created   :	Jul 29, 2007
	-- 20100422 - Dave Stacey - Split into seperate files

	SELECT
		[UploadTypeTemplateAttribute_ID],
		[UploadTypeTemplate_ID],
		[UploadTypeAttribute_ID]
	FROM UploadTypeTemplateAttribute (NOLOCK) 
	WHERE UploadTypeTemplate_ID = @UploadTypeTemplate_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EIM_Regen_GetUploadTypeTemplateAttributesByUploadTypeTemplateID] TO [IRMAClientRole]
    AS [dbo];

