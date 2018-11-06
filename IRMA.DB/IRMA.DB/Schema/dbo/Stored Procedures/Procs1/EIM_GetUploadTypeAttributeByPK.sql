﻿CREATE PROCEDURE dbo.EIM_GetUploadTypeAttributeByPK
	@UploadTypeAttribute_ID int
	
AS

	-- This stored procedure can be found in
	-- \EIM_CRUD_Procedures\EIM_UploadTypeAttribute_CRUD_Procs.sql file in the ItemCatalogDB project.
	-- Created By:	David Marine
	-- Created   :	Mar 02, 2007
	-- 20100422 - Dave Stacey - Split into seperate files

	SELECT
		UploadTypeAttribute.[UploadTypeAttribute_ID],
		UploadTypeAttribute.[UploadType_Code],
		UploadTypeAttribute.[UploadAttribute_ID],
		UploadTypeAttribute.[IsRequiredForUploadTypeForExistingItems],
		UploadTypeAttribute.[IsRequiredForUploadTypeForNewItems],
		UploadTypeAttribute.[IsReadOnlyForExistingItems],
		UploadTypeAttribute.[IsReadOnlyForNewItems],
		UploadTypeAttribute.[IsHidden],
		UploadTypeAttribute.[GridPosition],
		UploadTypeAttribute.[GroupName]
	FROM UploadTypeAttributeView UploadTypeAttribute (NOLOCK)
		JOIN UploadAttributeView UploadAttribute (NOLOCK)
			ON UploadAttribute.UploadAttribute_ID = UploadTypeAttribute.UploadAttribute_ID
	WHERE UploadTypeAttribute.UploadTypeAttribute_ID = @UploadTypeAttribute_ID
		AND UploadAttribute.IsActive = 1
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EIM_GetUploadTypeAttributeByPK] TO [IRMAClientRole]
    AS [dbo];

