

--=====================================================================
--*********      dbo.EIM_Regen_GetUploadValueByPK                             
--=====================================================================

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[EIM_Regen_GetUploadValueByPK]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[EIM_Regen_GetUploadValueByPK]
GO

CREATE PROCEDURE dbo.EIM_Regen_GetUploadValueByPK
	@UploadValue_ID int
	
AS

	-- This stored procedure can be found with all the
	-- Generated CRUD procedures for the UploadValue table in the
	-- \EIM_Regen_CRUD_Procedures\EIM_Regen_UploadValue_CRUD_Procs.sql file in the ItemCatalogDB project.
	-- Created By:	David Marine
	-- Created   :	Jul 29, 2007
	-- 20100422 - Dave Stacey - Split into seperate files

	SELECT
		[UploadValue_ID],
		[UploadAttribute_ID],
		[UploadRow_ID],
		[Value]
	FROM UploadValue (NOLOCK) 
	WHERE UploadValue_ID = @UploadValue_ID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO