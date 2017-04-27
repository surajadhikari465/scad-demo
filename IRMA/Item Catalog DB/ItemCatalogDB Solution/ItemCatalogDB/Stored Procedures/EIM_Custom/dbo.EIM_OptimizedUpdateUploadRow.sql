
--=====================================================================
--*********      dbo.EIM_OptimizedUpdateUploadRow                    
--=====================================================================

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[EIM_OptimizedUpdateUploadRow]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[EIM_OptimizedUpdateUploadRow]
GO
CREATE PROCEDURE dbo.EIM_OptimizedUpdateUploadRow
		@UploadRow_ID int,
		@Item_Key int,
		@UploadSession_ID int,
		@Identifier varchar(13),
		@ValidationLevel int,
		@ItemRequest_ID int,
		@ConcatonatedUploadValuesString VARCHAR(MAX),
		@UpdateCount int OUTPUT
AS 

	-- This stored procedure can be found with all the
	-- Generated CRUD procedures for the UploadRow table in the
	-- \EIM_Regen_CRUD_Procedures\EIM_Regen_UploadRow_CRUD_Procs.sql file in the ItemCatalogDB project.
	-- Created By:	David Marine
	-- Created   :	Mar 08, 2007
	-- 20100422 - Dave Stacey - Split into seperate files

DECLARE
	@IgnoreUploadValueIdsString VARCHAR(MAX)

	UPDATE UploadRow
	SET
		[Item_Key] = @Item_Key,
		[UploadSession_ID] = @UploadSession_ID,
		[Identifier] = @Identifier,
		[ValidationLevel] = @ValidationLevel,
		[ItemRequest_ID] = @ItemRequest_ID
	WHERE
		UploadRow_ID = @UploadRow_ID
		
	SELECT @UpdateCount = @@ROWCOUNT

	EXEC dbo.EIM_OptimizedSaveUploadValues
		'UPDATE',
		@UploadRow_ID,
		@ConcatonatedUploadValuesString,
		@IgnoreUploadValueIdsString OUTPUT
	
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
