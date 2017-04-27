
--=====================================================================
--*********      dbo.EIM_Regen_InsertUploadValue                         
--=====================================================================

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[EIM_Regen_InsertUploadValue]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[EIM_Regen_InsertUploadValue]
GO
CREATE PROCEDURE dbo.EIM_Regen_InsertUploadValue
		@UploadAttribute_ID int,
			@UploadRow_ID int,
			@Value varchar(4500)
		,
		@UploadValue_ID int OUTPUT
	
AS 

	-- This stored procedure can be found with all the
	-- Generated CRUD procedures for the UploadValue table in the
	-- \EIM_Regen_CRUD_Procedures\EIM_Regen_UploadValue_CRUD_Procs.sql file in the ItemCatalogDB project.
	-- Created By:	David Marine
	-- Created   :	Jul 29, 2007
	--20100215 - Dave Stacey - Add Handling for UK grid Date Conversion 
	--The issue of handling this in a more elegant fashion should be addressed in the future, for now it's out of scope 
	--20100311  - Alex Z  -  Added join on the view instead of the Uploadattribute table. Flex Attributes were causing 
	-- this procedure to fail.
	-- 20100422 - Dave Stacey - Split into seperate files
	

	INSERT INTO UploadValue
	(
		[UploadAttribute_ID],
		[UploadRow_ID],
		[Value]
	)
	SELECT
		[UploadAttribute_ID] = @UploadAttribute_ID,
		[UploadRow_ID] = @UploadRow_ID,
		[Value] =  CASE WHEN (UA.DisplayFormatString = 'dd/MM/yyyy' AND LEN(@Value) > 0)
			then  substring(@Value, 4, 3) + substring(@Value, 1, 2)+ Right(@Value, Len(@Value) - 5)
			else @Value 
			end
	FROM UploadAttributeView UA 
	WHERE
		UA.UploadAttribute_ID = @UploadAttribute_ID
	
		SELECT @UploadValue_ID  = SCOPE_IDENTITY()
	
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO