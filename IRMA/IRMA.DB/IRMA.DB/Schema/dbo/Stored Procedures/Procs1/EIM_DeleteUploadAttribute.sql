CREATE PROCEDURE dbo.EIM_DeleteUploadAttribute
		@UploadAttribute_ID int
		,
		@DeleteCount int OUTPUT
AS 

	-- This stored procedure can be found with all the
	-- Generated CRUD procedures for the UploadAttribute table in the
	-- \EIM_CRUD_Procedures\EIM_UploadAttribute_CRUD_Procs.sql file in the ItemCatalogDB project.
	-- Created By:	David Marine
	-- Created   :	Jul 10, 2007
	-- 20100422 - Dave Stacey - Split into seperate files
	
	DELETE FROM UploadAttribute
	WHERE
		UploadAttribute_ID = @UploadAttribute_ID
	
			
	SELECT @DeleteCount = @@ROWCOUNT