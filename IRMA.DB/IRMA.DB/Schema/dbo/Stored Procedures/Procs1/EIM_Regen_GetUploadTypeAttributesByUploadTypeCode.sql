﻿CREATE PROCEDURE dbo.EIM_Regen_GetUploadTypeAttributesByUploadTypeCode
	@UploadType_Code varchar(50)
	
AS

	-- This stored procedure can be found with all the
	-- Generated CRUD procedures for the UploadTypeAttribute table in the
	-- \EIM_Regen_CRUD_Procedures\EIM_Regen_UploadTypeAttribute_CRUD_Procs.sql file in the ItemCatalogDB project.
	-- Created By:	David Marine
	-- Created   :	Jul 29, 2007
	-- 20100422 - Dave Stacey - Split into seperate files

	SELECT
		[UploadTypeAttribute_ID],
		[UploadType_Code],
		[UploadAttribute_ID],
		[IsRequiredForUploadTypeForExistingItems],
		[IsReadOnlyForExistingItems],
		[IsHidden],
		[GridPosition],
		[IsRequiredForUploadTypeForNewItems],
		[IsReadOnlyForNewItems],
		[GroupName]
	FROM UploadTypeAttribute (NOLOCK) 
	WHERE UploadType_Code = @UploadType_Code
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EIM_Regen_GetUploadTypeAttributesByUploadTypeCode] TO [IRMAClientRole]
    AS [dbo];

