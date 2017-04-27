
--=====================================================================
--*********      EIM_CascadeDeleteUploadSession                
--=====================================================================

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[EIM_CascadeDeleteUploadSession]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[EIM_CascadeDeleteUploadSession]
GO
CREATE PROCEDURE dbo.EIM_CascadeDeleteUploadSession
		@UploadSession_ID int
		,
		@DeleteCount int OUTPUT
AS 

	-- This stored procedure can be found with all the
	-- Generated CRUD procedures for the UploadSession table in the
	-- \EIM_Regen_CRUD_Procedures\EIM_Regen_UploadSession_CRUD_Procs.sql file in the ItemCatalogDB project.
	-- Created By:	David Marine
	-- Created   :	Feb 22, 2007
	
	BEGIN TRANSACTION
	
	-- delete all row values
	DELETE FROM UploadValue
	WHERE
		UploadRow_ID IN
			(SELECT UploadRow_ID FROM UploadRow WHERE UploadRow.UploadSession_ID = @UploadSession_ID)
	
	-- delete all rows
	DELETE FROM UploadROW
	WHERE
		UploadSession_ID = @UploadSession_ID

	-- delete all UploadSessionUploadTypeStores	
	DELETE FROM UploadSessionUploadTypeStore
	WHERE
		UploadSessionUploadTypeStore_ID IN
			(SELECT UploadSessionUploadTypeStore_ID FROM UploadSessionUploadType WHERE UploadSessionUploadType.UploadSession_ID = @UploadSession_ID)

	-- delete all UploadSessionUploadTypes	
	DELETE FROM UploadSessionUploadType
	WHERE
		UploadSession_ID = @UploadSession_ID

	-- delete the session
	DELETE FROM UploadSession
	WHERE
		UploadSession_ID = @UploadSession_ID
		
	COMMIT TRANSACTION
	
			
	SELECT @DeleteCount = @@ROWCOUNT

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO