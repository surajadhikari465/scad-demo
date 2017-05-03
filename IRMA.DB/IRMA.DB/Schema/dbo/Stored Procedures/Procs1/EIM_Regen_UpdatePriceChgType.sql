CREATE PROCEDURE [dbo].[EIM_Regen_UpdatePriceChgType]
		@PriceChgTypeID tinyint,
		@PriceChgTypeDesc varchar(20),
		@Priority smallint,
		@On_Sale bit,
		@MSRP_Required bit,
		@LineDrive bit,
		@Competetive bit,
		@UpdateCount int OUTPUT
AS 

	-- This stored procedure can be found with all the
	-- Generated CRUD procedures for the PriceChgType table in the
	-- \EIM_Regen_CRUD_Procedures\EIM_Regen_PriceChgType_CRUD_Procs.sql file in the ItemCatalogDB project.
	-- Created By:	David Marine
	-- Created   :	Mar 21, 2007
	-- 20100422 - Dave Stacey - Split into seperate files

	UPDATE PriceChgType
	SET
		[PriceChgTypeDesc] = @PriceChgTypeDesc,
		[Priority] = @Priority,
		[On_Sale] = @On_Sale,
		[MSRP_Required] = @MSRP_Required,
		[LineDrive] = @LineDrive,
		[Competitive] = @Competetive
	WHERE
		PriceChgTypeID = @PriceChgTypeID
		
	SELECT @UpdateCount = @@ROWCOUNT
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EIM_Regen_UpdatePriceChgType] TO [IRMAClientRole]
    AS [dbo];

