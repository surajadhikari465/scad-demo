CREATE PROCEDURE dbo.EIM_Regen_UpdateUploadTypeTemplateAttribute
		@UploadTypeTemplateAttribute_ID int,
		@UploadTypeTemplate_ID int,
		@UploadTypeAttribute_ID int
		,
		@UpdateCount int OUTPUT
AS 

	-- This stored procedure can be found with all the
	-- Generated CRUD procedures for the UploadTypeTemplateAttribute table in the
	-- \EIM_Regen_CRUD_Procedures\EIM_Regen_UploadTypeTemplateAttribute_CRUD_Procs.sql file in the ItemCatalogDB project.
	-- Created By:	David Marine
	-- Created   :	Jul 29, 2007
	-- 20100422 - Dave Stacey - Split into seperate files

	UPDATE UploadTypeTemplateAttribute
	SET
		[UploadTypeTemplate_ID] = @UploadTypeTemplate_ID,
		[UploadTypeAttribute_ID] = @UploadTypeAttribute_ID
	WHERE
		UploadTypeTemplateAttribute_ID = @UploadTypeTemplateAttribute_ID
		
	SELECT @UpdateCount = @@ROWCOUNT
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EIM_Regen_UpdateUploadTypeTemplateAttribute] TO [IRMAClientRole]
    AS [dbo];

