CREATE PROCEDURE dbo.EIM_Regen_InsertUploadTypeAttribute
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
		@UploadTypeAttribute_ID int OUTPUT
	
AS 

	-- This stored procedure can be found with all the
	-- Generated CRUD procedures for the UploadTypeAttribute table in the
	-- \EIM_Regen_CRUD_Procedures\EIM_Regen_UploadTypeAttribute_CRUD_Procs.sql file in the ItemCatalogDB project.
	-- Created By:	David Marine
	-- Created   :	Jul 29, 2007
	-- 20100422 - Dave Stacey - Split into seperate files

	INSERT INTO UploadTypeAttribute
	(
		[UploadType_Code],
		[UploadAttribute_ID],
		[IsRequiredForUploadTypeForExistingItems],
		[IsReadOnlyForExistingItems],
		[IsHidden],
		[GridPosition],
		[IsRequiredForUploadTypeForNewItems],
		[IsReadOnlyForNewItems],
		[GroupName]
	)
	VALUES (
		@UploadType_Code,
		@UploadAttribute_ID,
		@IsRequiredForUploadTypeForExistingItems,
		@IsReadOnlyForExistingItems,
		@IsHidden,
		@GridPosition,
		@IsRequiredForUploadTypeForNewItems,
		@IsReadOnlyForNewItems,
		@GroupName
	)
	
		SELECT @UploadTypeAttribute_ID  = SCOPE_IDENTITY()
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EIM_Regen_InsertUploadTypeAttribute] TO [IRMAClientRole]
    AS [dbo];

