
--=====================================================================
--*********      dbo.EIM_Regen_UpdateUploadType                    
--=====================================================================

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[EIM_Regen_UpdateUploadType]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[EIM_Regen_UpdateUploadType]
GO
CREATE PROCEDURE dbo.EIM_Regen_UpdateUploadType
		@UploadType_Code varchar(50),
		@Name varchar(50),
		@Description varchar(255),
		@IsActive bit
		,
		@UpdateCount int OUTPUT
AS 

	-- This stored procedure can be found with all the
	-- Generated CRUD procedures for the UploadType table in the
	-- \EIM_Regen_CRUD_Procedures\EIM_Regen_UploadType_CRUD_Procs.sql file in the ItemCatalogDB project.
	-- Created By:	David Marine
	-- Created   :	Jul 29, 2007
	-- 20100422 - Dave Stacey - Split into seperate files

	UPDATE UploadType
	SET
		[Name] = @Name,
		[Description] = @Description,
		[IsActive] = @IsActive
	WHERE
		UploadType_Code = @UploadType_Code
		
	SELECT @UpdateCount = @@ROWCOUNT
	
	
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO