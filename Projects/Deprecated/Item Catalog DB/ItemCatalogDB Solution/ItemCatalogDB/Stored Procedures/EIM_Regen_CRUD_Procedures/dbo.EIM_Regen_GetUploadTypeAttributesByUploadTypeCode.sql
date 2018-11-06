
--=====================================================================
--*********      dbo.EIM_Regen_GetUploadTypeAttributesByUploadTypeCode                             
--=====================================================================

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[EIM_Regen_GetUploadTypeAttributesByUploadTypeCode]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[EIM_Regen_GetUploadTypeAttributesByUploadTypeCode]
GO

CREATE PROCEDURE dbo.EIM_Regen_GetUploadTypeAttributesByUploadTypeCode
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
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO