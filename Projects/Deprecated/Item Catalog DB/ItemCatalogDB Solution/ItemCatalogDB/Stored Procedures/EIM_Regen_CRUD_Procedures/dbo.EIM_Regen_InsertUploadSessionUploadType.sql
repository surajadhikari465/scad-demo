
--=====================================================================
--*********      dbo.EIM_Regen_InsertUploadSessionUploadType                         
--=====================================================================

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[EIM_Regen_InsertUploadSessionUploadType]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[EIM_Regen_InsertUploadSessionUploadType]
GO
CREATE PROCEDURE dbo.EIM_Regen_InsertUploadSessionUploadType
		@UploadSession_ID int,
			@UploadType_Code varchar(50),
			@UploadTypeTemplate_ID int,
			@StoreSelectionType varchar(50),
			@Zone_ID int,
			@State varchar(50)
		,
		@UploadSessionUploadType_ID int OUTPUT
	
AS 

	-- This stored procedure can be found with all the
	-- Generated CRUD procedures for the UploadSessionUploadType table in the
	-- \EIM_Regen_CRUD_Procedures\EIM_Regen_UploadSessionUploadType_CRUD_Procs.sql file in the ItemCatalogDB project.
	-- Created By:	David Marine
	-- Created   :	Jul 29, 2007
	-- 20100422 - Dave Stacey - Split into seperate files

	INSERT INTO UploadSessionUploadType
	(
		[UploadSession_ID],
		[UploadType_Code],
		[UploadTypeTemplate_ID],
		[StoreSelectionType],
		[Zone_ID],
		[State]
	)
	VALUES (
		@UploadSession_ID,
		@UploadType_Code,
		@UploadTypeTemplate_ID,
		@StoreSelectionType,
		@Zone_ID,
		@State
	)
	
		SELECT @UploadSessionUploadType_ID  = SCOPE_IDENTITY()
	
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO