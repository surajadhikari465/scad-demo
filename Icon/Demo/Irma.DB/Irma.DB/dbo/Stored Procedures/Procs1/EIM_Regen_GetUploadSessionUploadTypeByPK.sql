﻿CREATE PROCEDURE dbo.EIM_Regen_GetUploadSessionUploadTypeByPK
	@UploadSessionUploadType_ID int
	
AS

	-- This stored procedure can be found with all the
	-- Generated CRUD procedures for the UploadSessionUploadType table in the
	-- \EIM_Regen_CRUD_Procedures\EIM_Regen_UploadSessionUploadType_CRUD_Procs.sql file in the ItemCatalogDB project.
	-- Created By:	David Marine
	-- Created   :	Jul 29, 2007
	-- 20100422 - Dave Stacey - Split into seperate files

	SELECT
		[UploadSessionUploadType_ID],
		[UploadSession_ID],
		[UploadType_Code],
		[UploadTypeTemplate_ID],
		[StoreSelectionType],
		[Zone_ID],
		[State]
	FROM UploadSessionUploadType (NOLOCK) 
	WHERE UploadSessionUploadType_ID = @UploadSessionUploadType_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EIM_Regen_GetUploadSessionUploadTypeByPK] TO [IRMAClientRole]
    AS [dbo];

