
--=====================================================================
--*********      dbo.EIM_Regen_DeleteUploadType                
--=====================================================================

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[EIM_Regen_DeleteUploadType]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[EIM_Regen_DeleteUploadType]
GO
CREATE PROCEDURE dbo.EIM_Regen_DeleteUploadType
		@UploadType_Code varchar(50)
		,
		@DeleteCount int OUTPUT
AS 

	-- This stored procedure can be found with all the
	-- Generated CRUD procedures for the UploadType table in the
	-- \EIM_Regen_CRUD_Procedures\EIM_Regen_UploadType_CRUD_Procs.sql file in the ItemCatalogDB project.
	-- Created By:	David Marine
	-- Created   :	Jul 29, 2007
	-- 20100422 - Dave Stacey - Split into seperate files
	
	DELETE FROM UploadType
	WHERE
		UploadType_Code = @UploadType_Code
	
			
	SELECT @DeleteCount = @@ROWCOUNT

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO