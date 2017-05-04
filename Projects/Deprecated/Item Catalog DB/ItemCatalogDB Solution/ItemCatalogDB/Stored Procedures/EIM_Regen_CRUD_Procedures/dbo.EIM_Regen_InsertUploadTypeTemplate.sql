

--=====================================================================
--*********      dbo.EIM_Regen_InsertUploadTypeTemplate                         
--=====================================================================

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[EIM_Regen_InsertUploadTypeTemplate]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[EIM_Regen_InsertUploadTypeTemplate]
GO
CREATE PROCEDURE dbo.EIM_Regen_InsertUploadTypeTemplate
		@UploadType_Code varchar(50),
			@Name varchar(50),
			@CreatedByUserID int,
			@CreatedDateTime datetime,
			@ModifiedByUserID int,
			@ModifiedDateTime datetime
		,
		@UploadTypeTemplate_ID int OUTPUT
	
AS 

	-- This stored procedure can be found with all the
	-- Generated CRUD procedures for the UploadTypeTemplate table in the
	-- \EIM_Regen_CRUD_Procedures\EIM_Regen_UploadTypeTemplate_CRUD_Procs.sql file in the ItemCatalogDB project.
	-- Created By:	David Marine
	-- Created   :	Jul 29, 2007
	-- 20100422 - Dave Stacey - Split into seperate files

	INSERT INTO UploadTypeTemplate
	(
		[UploadType_Code],
		[Name],
		[CreatedByUserID],
		[CreatedDateTime],
		[ModifiedByUserID],
		[ModifiedDateTime]
	)
	VALUES (
		@UploadType_Code,
		@Name,
		@CreatedByUserID,
		@CreatedDateTime,
		@ModifiedByUserID,
		@ModifiedDateTime
	)
	
		SELECT @UploadTypeTemplate_ID  = SCOPE_IDENTITY()
	
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO