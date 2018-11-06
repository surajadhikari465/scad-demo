﻿CREATE PROCEDURE dbo.EIM_Regen_GetUploadRowByPK
	@UploadRow_ID int
	
AS

	-- This stored procedure can be found with all the
	-- Generated CRUD procedures for the UploadRow table in the
	-- \EIM_Regen_CRUD_Procedures\EIM_Regen_UploadRow_CRUD_Procs.sql file in the ItemCatalogDB project.
	-- Created By:	David Marine
	-- Created   :	Jul 29, 2007
	-- 20100422 - Dave Stacey - Split into seperate files

	SELECT
		[UploadRow_ID],
		[Item_Key],
		[UploadSession_ID],
		[Identifier],
		[ValidationLevel],
		[ItemRequest_ID]
	FROM UploadRow (NOLOCK) 
	WHERE UploadRow_ID = @UploadRow_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EIM_Regen_GetUploadRowByPK] TO [IRMAClientRole]
    AS [dbo];

