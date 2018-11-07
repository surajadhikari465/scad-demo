CREATE PROCEDURE dbo.EIM_Regen_GetUploadValuesByUploadAttributeID
	@UploadAttribute_ID int
	
AS

	-- This stored procedure can be found with all the
	-- Generated CRUD procedures for the UploadValue table in the
	-- \EIM_Regen_CRUD_Procedures\EIM_Regen_UploadValue_CRUD_Procs.sql file in the ItemCatalogDB project.
	-- Created By:	David Marine
	-- Created   :	Jul 29, 2007
	-- 20100422 - Dave Stacey - Split into seperate files

	SELECT
		[UploadValue_ID],
		[UploadAttribute_ID],
		[UploadRow_ID],
		[Value]
	FROM UploadValue (NOLOCK) 
	WHERE UploadAttribute_ID = @UploadAttribute_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EIM_Regen_GetUploadValuesByUploadAttributeID] TO [IRMAClientRole]
    AS [dbo];

