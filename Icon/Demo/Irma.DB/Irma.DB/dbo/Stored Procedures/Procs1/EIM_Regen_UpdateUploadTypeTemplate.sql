CREATE PROCEDURE dbo.EIM_Regen_UpdateUploadTypeTemplate
		@UploadTypeTemplate_ID int,
		@UploadType_Code varchar(50),
		@Name varchar(50),
		@CreatedByUserID int,
		@CreatedDateTime datetime,
		@ModifiedByUserID int,
		@ModifiedDateTime datetime
		,
		@UpdateCount int OUTPUT
AS 

	-- This stored procedure can be found with all the
	-- Generated CRUD procedures for the UploadTypeTemplate table in the
	-- \EIM_Regen_CRUD_Procedures\EIM_Regen_UploadTypeTemplate_CRUD_Procs.sql file in the ItemCatalogDB project.
	-- Created By:	David Marine
	-- Created   :	Jul 29, 2007
	-- 20100422 - Dave Stacey - Split into seperate files

	UPDATE UploadTypeTemplate
	SET
		[UploadType_Code] = @UploadType_Code,
		[Name] = @Name,
		[CreatedByUserID] = @CreatedByUserID,
		[CreatedDateTime] = @CreatedDateTime,
		[ModifiedByUserID] = @ModifiedByUserID,
		[ModifiedDateTime] = @ModifiedDateTime
	WHERE
		UploadTypeTemplate_ID = @UploadTypeTemplate_ID
		
	SELECT @UpdateCount = @@ROWCOUNT
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EIM_Regen_UpdateUploadTypeTemplate] TO [IRMAClientRole]
    AS [dbo];

