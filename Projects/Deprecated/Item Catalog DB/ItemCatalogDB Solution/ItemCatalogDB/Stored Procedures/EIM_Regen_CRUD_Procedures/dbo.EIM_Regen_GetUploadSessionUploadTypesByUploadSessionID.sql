
--=====================================================================
--*********      dbo.EIM_Regen_GetUploadSessionUploadTypesByUploadSessionID                             
--=====================================================================

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[EIM_Regen_GetUploadSessionUploadTypesByUploadSessionID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[EIM_Regen_GetUploadSessionUploadTypesByUploadSessionID]
GO

CREATE PROCEDURE dbo.EIM_Regen_GetUploadSessionUploadTypesByUploadSessionID
	@UploadSession_ID int
	
AS

	-- This stored procedure can be found with all the
	-- Generated CRUD procedures for the UploadSessionUploadType table in the
	-- \EIM_Regen_CRUD_Procedures\EIM_Regen_UploadSessionUploadType_CRUD_Procs.sql file in the ItemCatalogDB project.
	-- Created By:	David Marine
	-- Created   :	Jul 29, 2007
	-- 20100422 - Dave Stacey - Split into seperate files

	SELECT
		[UploadSessionUploadType_ID],
		[UploadSession_ID],
		[UploadType_Code],
		[UploadTypeTemplate_ID],
		[StoreSelectionType],
		[Zone_ID],
		[State]
	FROM UploadSessionUploadType (NOLOCK) 
	WHERE UploadSession_ID = @UploadSession_ID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO