

--=====================================================================
--*********      dbo.EIM_Regen_DeletePriceChgType                
--=====================================================================

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[EIM_Regen_DeletePriceChgType]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[EIM_Regen_DeletePriceChgType]
GO
CREATE PROCEDURE dbo.EIM_Regen_DeletePriceChgType
		@PriceChgTypeID tinyint
		,
		@DeleteCount int OUTPUT
AS 

	-- This stored procedure can be found with all the
	-- Generated CRUD procedures for the PriceChgType table in the
	-- \EIM_Regen_CRUD_Procedures\EIM_Regen_PriceChgType_CRUD_Procs.sql file in the ItemCatalogDB project.
	-- Created By:	David Marine
	-- Created   :	Mar 21, 2007
	-- 20100422 - Dave Stacey - Split into seperate files
	
	DELETE FROM PriceChgType
	WHERE
		PriceChgTypeID = @PriceChgTypeID
	
			
	SELECT @DeleteCount = @@ROWCOUNT

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO