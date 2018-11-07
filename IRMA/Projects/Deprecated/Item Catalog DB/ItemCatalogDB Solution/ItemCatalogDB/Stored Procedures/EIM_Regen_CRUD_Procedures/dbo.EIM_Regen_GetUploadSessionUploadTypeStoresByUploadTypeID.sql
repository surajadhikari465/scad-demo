

--=====================================================================
--*********      dbo.EIM_Regen_GetUploadSessionUploadTypeStoresByUploadSessionUploadTypeID                             
--=====================================================================

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[EIM_Regen_GetUploadSessionUploadTypeStoresByUploadSessionUploadTypeID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[EIM_Regen_GetUploadSessionUploadTypeStoresByUploadSessionUploadTypeID]
GO

CREATE PROCEDURE dbo.EIM_Regen_GetUploadSessionUploadTypeStoresByUploadSessionUploadTypeID
	@UploadSessionUploadType_ID int
	
AS

	-- This stored procedure can be found with all the
	-- Generated CRUD procedures for the UploadSessionUploadTypeStore table in the
	-- \EIM_Regen_CRUD_Procedures\EIM_Regen_UploadSessionUploadTypeStore_CRUD_Procs.sql file in the ItemCatalogDB project.
	-- Created By:	David Marine
	-- Created   :	Jul 29, 2007
	-- 20100422 - Dave Stacey - Split into seperate files

	SELECT
		[UploadSessionUploadTypeStore_ID],
		[UploadSessionUploadType_ID],
		[Store_No]
	FROM UploadSessionUploadTypeStore (NOLOCK) 
	WHERE UploadSessionUploadType_ID = @UploadSessionUploadType_ID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO