CREATE PROCEDURE dbo.EIM_Regen_DeleteUploadTypeTemplate
		@UploadTypeTemplate_ID int
		,
		@DeleteCount int OUTPUT
AS 

	-- This stored procedure can be found with all the
	-- Generated CRUD procedures for the UploadTypeTemplate table in the
	-- \EIM_Regen_CRUD_Procedures\EIM_Regen_UploadTypeTemplate_CRUD_Procs.sql file in the ItemCatalogDB project.
	-- Created By:	David Marine
	-- Created   :	Jul 29, 2007
	-- 20100422 - Dave Stacey - Split into seperate files
	
	DELETE FROM UploadTypeTemplate
	WHERE
		UploadTypeTemplate_ID = @UploadTypeTemplate_ID
	
			
	SELECT @DeleteCount = @@ROWCOUNT
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EIM_Regen_DeleteUploadTypeTemplate] TO [IRMAClientRole]
    AS [dbo];

