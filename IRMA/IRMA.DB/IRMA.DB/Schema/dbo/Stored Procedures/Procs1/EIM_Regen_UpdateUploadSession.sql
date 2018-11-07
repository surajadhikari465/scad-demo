CREATE PROCEDURE dbo.EIM_Regen_UpdateUploadSession
		@UploadSession_ID int,
		@Name varchar(100),
		@IsUploaded bit,
		@ItemsProcessedCount int,
		@ItemsLoadedCount int,
		@ErrorsCount int,
		@EmailToAddress varchar(255),
		@CreatedByUserID int,
		@CreatedDateTime datetime,
		@ModifiedByUserID int,
		@ModifiedDateTime datetime,
		@IsNewItemSessionFlag bit,
		@IsFromSLIM bit,
		@IsDeleteItemSessionFlag bit
		,
		@UpdateCount int OUTPUT
AS 

	-- This stored procedure can be found with all the
	-- Generated CRUD procedures for the UploadSession table in the
	-- \EIM_Regen_CRUD_Procedures\EIM_Regen_UploadSession_CRUD_Procs.sql file in the ItemCatalogDB project.
	-- Created By:	David Marine
	-- Created   :	Jul 29, 2007
	-- 20100422 - Dave Stacey - Split into seperate files

	UPDATE UploadSession
	SET
		[Name] = @Name,
		[IsUploaded] = @IsUploaded,
		[ItemsProcessedCount] = @ItemsProcessedCount,
		[ItemsLoadedCount] = @ItemsLoadedCount,
		[ErrorsCount] = @ErrorsCount,
		[EmailToAddress] = @EmailToAddress,
		[CreatedByUserID] = @CreatedByUserID,
		[CreatedDateTime] = @CreatedDateTime,
		[ModifiedByUserID] = @ModifiedByUserID,
		[ModifiedDateTime] = @ModifiedDateTime,
		[IsNewItemSessionFlag] = @IsNewItemSessionFlag,
		[IsFromSLIM] = @IsFromSLIM,
		[IsDeleteItemSessionFlag] = @IsDeleteItemSessionFlag
	WHERE
		UploadSession_ID = @UploadSession_ID
		
	SELECT @UpdateCount = @@ROWCOUNT
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EIM_Regen_UpdateUploadSession] TO [IRMAClientRole]
    AS [dbo];

