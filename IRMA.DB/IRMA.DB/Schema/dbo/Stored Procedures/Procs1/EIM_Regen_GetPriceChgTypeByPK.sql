CREATE PROCEDURE dbo.EIM_Regen_GetPriceChgTypeByPK
	@PriceChgTypeID tinyint
	
AS

	-- This stored procedure can be found with all the
	-- Generated CRUD procedures for the PriceChgType table in the
	-- \EIM_Regen_CRUD_Procedures\EIM_Regen_PriceChgType_CRUD_Procs.sql file in the ItemCatalogDB project.
	-- Created By:	David Marine
	-- Created   :	Mar 21, 2007
	-- 20100422 - Dave Stacey - Split into seperate files

	SELECT
		[PriceChgTypeID],
		[PriceChgTypeDesc],
		[Priority],
		[On_Sale],
		[MSRP_Required],
		[LineDrive]
	FROM PriceChgType (NOLOCK) 
	WHERE PriceChgTypeID = @PriceChgTypeID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EIM_Regen_GetPriceChgTypeByPK] TO [IRMAClientRole]
    AS [dbo];

