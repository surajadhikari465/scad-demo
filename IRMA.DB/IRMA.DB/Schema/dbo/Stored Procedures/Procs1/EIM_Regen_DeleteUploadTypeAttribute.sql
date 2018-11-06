CREATE PROCEDURE dbo.EIM_Regen_DeleteUploadTypeAttribute
		@UploadTypeAttribute_ID int
		,
		@DeleteCount int OUTPUT
AS 

	-- This stored procedure can be found with all the
	-- Generated CRUD procedures for the UploadTypeAttribute table in the
	-- \EIM_Regen_CRUD_Procedures\EIM_Regen_UploadTypeAttribute_CRUD_Procs.sql file in the ItemCatalogDB project.
	-- Created By:	David Marine
	-- Created   :	Jul 29, 2007
	-- 20100422 - Dave Stacey - Split into seperate files
	
	DELETE FROM UploadTypeAttribute
	WHERE
		UploadTypeAttribute_ID = @UploadTypeAttribute_ID
	
			
	SELECT @DeleteCount = @@ROWCOUNT
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EIM_Regen_DeleteUploadTypeAttribute] TO [IRMAClientRole]
    AS [dbo];

