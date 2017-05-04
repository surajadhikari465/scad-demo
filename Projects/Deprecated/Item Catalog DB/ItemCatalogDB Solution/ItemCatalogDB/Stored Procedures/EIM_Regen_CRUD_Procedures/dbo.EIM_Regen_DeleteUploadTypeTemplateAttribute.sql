
--=====================================================================
--*********      dbo.EIM_Regen_DeleteUploadTypeTemplateAttribute                
--=====================================================================

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[EIM_Regen_DeleteUploadTypeTemplateAttribute]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[EIM_Regen_DeleteUploadTypeTemplateAttribute]
GO
CREATE PROCEDURE dbo.EIM_Regen_DeleteUploadTypeTemplateAttribute
		@UploadTypeTemplateAttribute_ID int
		,
		@DeleteCount int OUTPUT
AS 

	-- This stored procedure can be found with all the
	-- Generated CRUD procedures for the UploadTypeTemplateAttribute table in the
	-- \EIM_Regen_CRUD_Procedures\EIM_Regen_UploadTypeTemplateAttribute_CRUD_Procs.sql file in the ItemCatalogDB project.
	-- Created By:	David Marine
	-- Created   :	Jul 29, 2007
	-- 20100422 - Dave Stacey - Split into seperate files
	
	DELETE FROM UploadTypeTemplateAttribute
	WHERE
		UploadTypeTemplateAttribute_ID = @UploadTypeTemplateAttribute_ID
	
			
	SELECT @DeleteCount = @@ROWCOUNT

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO