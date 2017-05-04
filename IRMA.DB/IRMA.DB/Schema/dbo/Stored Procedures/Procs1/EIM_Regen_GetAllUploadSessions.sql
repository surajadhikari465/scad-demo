﻿CREATE PROCEDURE dbo.EIM_Regen_GetAllUploadSessions
	
AS

	-- This stored procedure can be found with all the
	-- Generated CRUD procedures for the UploadSession table in the
	-- \EIM_Regen_CRUD_Procedures\EIM_Regen_UploadSession_CRUD_Procs.sql file in the ItemCatalogDB project.
	-- Created By:	David Marine
	-- Created   :	Jul 29, 2007
	-- 20100422 - Dave Stacey - Split into seperate files

	SELECT
		[UploadSession_ID],
		[Name],
		[IsUploaded],
		[ItemsProcessedCount],
		[ItemsLoadedCount],
		[ErrorsCount],
		[EmailToAddress],
		[CreatedByUserID],
		[CreatedDateTime],
		[ModifiedByUserID],
		[ModifiedDateTime],
		[IsNewItemSessionFlag],
		[IsFromSLIM],
		[IsDeleteItemSessionFlag]
	FROM UploadSession (NOLOCK)
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EIM_Regen_GetAllUploadSessions] TO [IRMAClientRole]
    AS [dbo];

