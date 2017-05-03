
CREATE PROCEDURE dbo.EIM_Regen_GetUploadRowsByItemKey
	@Item_Key int
	
AS

	-- This stored procedure can be found with all the
	-- Generated CRUD procedures for the UploadRow table in the
	-- \EIM_Regen_CRUD_Procedures\EIM_Regen_UploadRow_CRUD_Procs.sql file in the ItemCatalogDB project.
	-- Created By:	David Marine
	-- Created   :	Mar 21, 2007

	SELECT
		[UploadRow_ID],
		[Item_Key],
		[UploadSession_ID],
		[Identifier],
		[ValidationLevel]
	FROM UploadRow (NOLOCK) 
	WHERE Item_Key = @Item_Key


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EIM_Regen_GetUploadRowsByItemKey] TO [IRMAClientRole]
    AS [dbo];

