﻿CREATE PROCEDURE dbo.EIM_Regen_GetAllUploadTypeTemplates
	
AS

	-- This stored procedure can be found with all the
	-- Generated CRUD procedures for the UploadTypeTemplate table in the
	-- \EIM_Regen_CRUD_Procedures\EIM_Regen_UploadTypeTemplate_CRUD_Procs.sql file in the ItemCatalogDB project.
	-- Created By:	David Marine
	-- Created   :	Jul 29, 2007

	SELECT
		[UploadTypeTemplate_ID],
		[UploadType_Code],
		[Name],
		[CreatedByUserID],
		[CreatedDateTime],
		[ModifiedByUserID],
		[ModifiedDateTime]
	FROM UploadTypeTemplate (NOLOCK)
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EIM_Regen_GetAllUploadTypeTemplates] TO [IRMAClientRole]
    AS [dbo];

