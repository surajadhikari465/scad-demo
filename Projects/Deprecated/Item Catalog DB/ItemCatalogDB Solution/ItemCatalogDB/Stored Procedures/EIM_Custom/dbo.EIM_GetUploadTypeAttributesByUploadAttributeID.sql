
--=====================================================================
--*********      dbo.EIM_GetUploadTypeAttributesByUploadAttributeID                             
--=====================================================================

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[EIM_GetUploadTypeAttributesByUploadAttributeID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[EIM_GetUploadTypeAttributesByUploadAttributeID]
GO

CREATE PROCEDURE dbo.EIM_GetUploadTypeAttributesByUploadAttributeID
	@UploadAttribute_ID int
	
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
	WHERE UploadTypeAttribute.UploadAttribute_ID = @UploadAttribute_ID
		AND UploadAttribute.IsActive = 1

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO