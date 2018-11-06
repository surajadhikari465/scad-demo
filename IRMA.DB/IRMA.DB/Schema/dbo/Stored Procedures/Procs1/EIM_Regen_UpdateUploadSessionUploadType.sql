﻿CREATE PROCEDURE dbo.EIM_Regen_UpdateUploadSessionUploadType
		@UploadSessionUploadType_ID int,
		@UploadSession_ID int,
		@UploadType_Code varchar(50),
		@UploadTypeTemplate_ID int,
		@StoreSelectionType varchar(50),
		@Zone_ID int,
		@State varchar(50)
		,
		@UpdateCount int OUTPUT
AS 

	-- This stored procedure can be found with all the
	-- Generated CRUD procedures for the UploadSessionUploadType table in the
	-- \EIM_Regen_CRUD_Procedures\EIM_Regen_UploadSessionUploadType_CRUD_Procs.sql file in the ItemCatalogDB project.
	-- Created By:	David Marine
	-- Created   :	Jul 29, 2007
	-- 20100422 - Dave Stacey - Split into seperate files

	UPDATE UploadSessionUploadType
	SET
		[UploadSession_ID] = @UploadSession_ID,
		[UploadType_Code] = @UploadType_Code,
		[UploadTypeTemplate_ID] = @UploadTypeTemplate_ID,
		[StoreSelectionType] = @StoreSelectionType,
		[Zone_ID] = @Zone_ID,
		[State] = @State
	WHERE
		UploadSessionUploadType_ID = @UploadSessionUploadType_ID
		
	SELECT @UpdateCount = @@ROWCOUNT
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EIM_Regen_UpdateUploadSessionUploadType] TO [IRMAClientRole]
    AS [dbo];

