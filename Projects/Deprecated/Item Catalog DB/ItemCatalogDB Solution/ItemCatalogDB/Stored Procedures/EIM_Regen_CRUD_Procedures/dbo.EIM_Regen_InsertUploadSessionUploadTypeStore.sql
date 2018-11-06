

--=====================================================================
--*********      dbo.EIM_Regen_InsertUploadSessionUploadTypeStore                         
--=====================================================================

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[EIM_Regen_InsertUploadSessionUploadTypeStore]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[EIM_Regen_InsertUploadSessionUploadTypeStore]
GO
CREATE PROCEDURE dbo.EIM_Regen_InsertUploadSessionUploadTypeStore
		@UploadSessionUploadType_ID int,
			@Store_No int
		,
		@UploadSessionUploadTypeStore_ID int OUTPUT
	
AS 

	-- This stored procedure can be found with all the
	-- Generated CRUD procedures for the UploadSessionUploadTypeStore table in the
	-- \EIM_Regen_CRUD_Procedures\EIM_Regen_UploadSessionUploadTypeStore_CRUD_Procs.sql file in the ItemCatalogDB project.
	-- Created By:	David Marine
	-- Created   :	Jul 29, 2007
	-- 20100422 - Dave Stacey - Split into seperate files

	INSERT INTO UploadSessionUploadTypeStore
	(
		[UploadSessionUploadType_ID],
		[Store_No]
	)
	VALUES (
		@UploadSessionUploadType_ID,
		@Store_No
	)
	
		SELECT @UploadSessionUploadTypeStore_ID  = SCOPE_IDENTITY()
	
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO