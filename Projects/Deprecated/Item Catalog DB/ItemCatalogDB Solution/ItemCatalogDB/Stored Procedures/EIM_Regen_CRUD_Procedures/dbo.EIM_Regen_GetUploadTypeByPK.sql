

--=====================================================================
--*********      dbo.EIM_Regen_GetUploadTypeByPK                             
--=====================================================================

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[EIM_Regen_GetUploadTypeByPK]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[EIM_Regen_GetUploadTypeByPK]
GO

CREATE PROCEDURE dbo.EIM_Regen_GetUploadTypeByPK
	@UploadType_Code varchar(50)
	
AS

	-- This stored procedure can be found with all the
	-- Generated CRUD procedures for the UploadType table in the
	-- \EIM_Regen_CRUD_Procedures\EIM_Regen_UploadType_CRUD_Procs.sql file in the ItemCatalogDB project.
	-- Created By:	David Marine
	-- Created   :	Jul 29, 2007
	-- 20100422 - Dave Stacey - Split into seperate files

	SELECT
		[UploadType_Code],
		[Name],
		[Description],
		[IsActive]
	FROM UploadType (NOLOCK) 
	WHERE UploadType_Code = @UploadType_Code
		AND IsActive = 1

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
