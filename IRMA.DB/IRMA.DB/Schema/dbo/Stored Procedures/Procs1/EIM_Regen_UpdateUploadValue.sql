﻿CREATE PROCEDURE dbo.EIM_Regen_UpdateUploadValue
		@UploadValue_ID int,
		@UploadAttribute_ID int,
		@UploadRow_ID int,
		@Value varchar(4500),
		@UpdateCount int OUTPUT
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

	
	UPDATE UploadValue
	SET
		[UploadAttribute_ID] = @UploadAttribute_ID,
		[UploadRow_ID] = @UploadRow_ID,
		[Value] =  CASE WHEN (UA.DisplayFormatString = 'dd/MM/yyyy' and LEN(@Value) > 5)
			then  substring(@Value, 4, 3) + substring(@Value, 1, 2)+ Right(@Value, Len(@Value) - 5)
			else @Value 
			end
	FROM UploadValue UV
	JOIN UploadAttributeView UA ON UA.UploadAttribute_ID = UV.UploadAttribute_ID
	WHERE
		UV.UploadValue_ID = @UploadValue_ID
		
	SELECT @UpdateCount = @@ROWCOUNT
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EIM_Regen_UpdateUploadValue] TO [IRMAClientRole]
    AS [dbo];

