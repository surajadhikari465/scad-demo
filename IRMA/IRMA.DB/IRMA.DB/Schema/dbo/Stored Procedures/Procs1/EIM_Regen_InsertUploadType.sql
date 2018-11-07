CREATE PROCEDURE dbo.EIM_Regen_InsertUploadType
		@Name varchar(50),
			@Description varchar(255),
			@IsActive bit
		,
		@UploadType_Code varchar(50) OUTPUT
	
AS 

	-- This stored procedure can be found with all the
	-- Generated CRUD procedures for the UploadType table in the
	-- \EIM_Regen_CRUD_Procedures\EIM_Regen_UploadType_CRUD_Procs.sql file in the ItemCatalogDB project.
	-- Created By:	David Marine
	-- Created   :	Jul 29, 2007
	-- 20100422 - Dave Stacey - Split into seperate files

	INSERT INTO UploadType
	(
		[Name],
		[Description],
		[IsActive]
	)
	VALUES (
		@Name,
		@Description,
		@IsActive
	)
	
		SELECT @UploadType_Code  = SCOPE_IDENTITY()
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EIM_Regen_InsertUploadType] TO [IRMAClientRole]
    AS [dbo];

