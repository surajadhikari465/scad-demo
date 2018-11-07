CREATE PROCEDURE dbo.EIM_Regen_UpdateUploadTypeAttribute
		@UploadTypeAttribute_ID int,
		@UploadType_Code varchar(50),
		@UploadAttribute_ID int,
		@IsRequiredForUploadTypeForExistingItems bit,
		@IsReadOnlyForExistingItems bit,
		@IsHidden bit,
		@GridPosition int,
		@IsRequiredForUploadTypeForNewItems bit,
		@IsReadOnlyForNewItems bit,
		@GroupName varchar(100)
		,
		@UpdateCount int OUTPUT
AS 

	-- This stored procedure can be found with all the
	-- Generated CRUD procedures for the UploadTypeAttribute table in the
	-- \EIM_Regen_CRUD_Procedures\EIM_Regen_UploadTypeAttribute_CRUD_Procs.sql file in the ItemCatalogDB project.
	-- Created By:	David Marine
	-- Created   :	Jul 29, 2007
	-- 20100422 - Dave Stacey - Split into seperate files

	UPDATE UploadTypeAttribute
	SET
		[UploadType_Code] = @UploadType_Code,
		[UploadAttribute_ID] = @UploadAttribute_ID,
		[IsRequiredForUploadTypeForExistingItems] = @IsRequiredForUploadTypeForExistingItems,
		[IsReadOnlyForExistingItems] = @IsReadOnlyForExistingItems,
		[IsHidden] = @IsHidden,
		[GridPosition] = @GridPosition,
		[IsRequiredForUploadTypeForNewItems] = @IsRequiredForUploadTypeForNewItems,
		[IsReadOnlyForNewItems] = @IsReadOnlyForNewItems,
		[GroupName] = @GroupName
	WHERE
		UploadTypeAttribute_ID = @UploadTypeAttribute_ID
		
	SELECT @UpdateCount = @@ROWCOUNT
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EIM_Regen_UpdateUploadTypeAttribute] TO [IRMAClientRole]
    AS [dbo];

