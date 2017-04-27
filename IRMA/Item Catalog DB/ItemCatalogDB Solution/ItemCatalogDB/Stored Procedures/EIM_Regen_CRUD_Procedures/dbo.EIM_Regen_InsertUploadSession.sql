

--=====================================================================
--*********      dbo.EIM_Regen_InsertUploadSession                         
--=====================================================================

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[EIM_Regen_InsertUploadSession]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[EIM_Regen_InsertUploadSession]
GO
CREATE PROCEDURE dbo.EIM_Regen_InsertUploadSession
		@Name varchar(100),
			@IsUploaded bit,
			@ItemsProcessedCount int,
			@ItemsLoadedCount int,
			@ErrorsCount int,
			@EmailToAddress varchar(255),
			@CreatedByUserID int,
			@CreatedDateTime datetime,
			@ModifiedByUserID int,
			@ModifiedDateTime datetime,
			@IsNewItemSessionFlag bit,
			@IsFromSLIM bit,
			@IsDeleteItemSessionFlag bit
		,
		@UploadSession_ID int OUTPUT
	
AS 

	-- This stored procedure can be found with all the
	-- Generated CRUD procedures for the UploadSession table in the
	-- \EIM_Regen_CRUD_Procedures\EIM_Regen_UploadSession_CRUD_Procs.sql file in the ItemCatalogDB project.
	-- Created By:	David Marine
	-- Created   :	Jul 29, 2007
	-- 20100422 - Dave Stacey - Split into seperate files

	INSERT INTO UploadSession
	(
		[Name],
		[IsUploaded],
		[ItemsProcessedCount],
		[ItemsLoadedCount],
		[ErrorsCount],
		[EmailToAddress],
		[CreatedByUserID],
		[CreatedDateTime],
		[ModifiedByUserID],
		[ModifiedDateTime],
		[IsNewItemSessionFlag],
		[IsFromSLIM],
		[IsDeleteItemSessionFlag]
	)
	VALUES (
		@Name,
		@IsUploaded,
		@ItemsProcessedCount,
		@ItemsLoadedCount,
		@ErrorsCount,
		@EmailToAddress,
		@CreatedByUserID,
		@CreatedDateTime,
		@ModifiedByUserID,
		@ModifiedDateTime,
		@IsNewItemSessionFlag,
		@IsFromSLIM,
		@IsDeleteItemSessionFlag
	)
	
		SELECT @UploadSession_ID  = SCOPE_IDENTITY()
	
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO