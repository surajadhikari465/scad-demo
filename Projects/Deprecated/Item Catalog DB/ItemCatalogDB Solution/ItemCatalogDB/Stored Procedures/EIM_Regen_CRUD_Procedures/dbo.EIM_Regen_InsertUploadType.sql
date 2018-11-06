

--=====================================================================
--*********      dbo.EIM_Regen_InsertUploadType                         
--=====================================================================

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[EIM_Regen_InsertUploadType]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[EIM_Regen_InsertUploadType]
GO
CREATE PROCEDURE dbo.EIM_Regen_InsertUploadType
		@Name varchar(50),
			@Description varchar(255),
			@IsActive bit
		,
		@UploadType_Code varchar(50) OUTPUT
	
AS 

	-- This stored procedure can be found with all the
	-- Generated CRUD procedures for the UploadType table in the
	-- \EIM_Regen_CRUD_Procedures\EIM_Regen_UploadType_CRUD_Procs.sql file in the ItemCatalogDB project.
	-- Created By:	David Marine
	-- Created   :	Jul 29, 2007
	-- 20100422 - Dave Stacey - Split into seperate files

	INSERT INTO UploadType
	(
		[Name],
		[Description],
		[IsActive]
	)
	VALUES (
		@Name,
		@Description,
		@IsActive
	)
	
		SELECT @UploadType_Code  = SCOPE_IDENTITY()
	
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO