CREATE PROCEDURE dbo.EIM_Regen_GetUploadSessionByPK
	@UploadSession_ID int
	
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
	WHERE UploadSession_ID = @UploadSession_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EIM_Regen_GetUploadSessionByPK] TO [IRMAClientRole]
    AS [dbo];

