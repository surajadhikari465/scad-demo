CREATE PROCEDURE dbo.EIM_Regen_InsertUploadTypeTemplateAttribute
		@UploadTypeTemplate_ID int,
			@UploadTypeAttribute_ID int
		,
		@UploadTypeTemplateAttribute_ID int OUTPUT
	
AS 

	-- This stored procedure can be found with all the
	-- Generated CRUD procedures for the UploadTypeTemplateAttribute table in the
	-- \EIM_Regen_CRUD_Procedures\EIM_Regen_UploadTypeTemplateAttribute_CRUD_Procs.sql file in the ItemCatalogDB project.
	-- Created By:	David Marine
	-- Created   :	Jul 29, 2007
	-- 20100422 - Dave Stacey - Split into seperate files

	INSERT INTO UploadTypeTemplateAttribute
	(
		[UploadTypeTemplate_ID],
		[UploadTypeAttribute_ID]
	)
	VALUES (
		@UploadTypeTemplate_ID,
		@UploadTypeAttribute_ID
	)
	
		SELECT @UploadTypeTemplateAttribute_ID  = SCOPE_IDENTITY()
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EIM_Regen_InsertUploadTypeTemplateAttribute] TO [IRMAClientRole]
    AS [dbo];

