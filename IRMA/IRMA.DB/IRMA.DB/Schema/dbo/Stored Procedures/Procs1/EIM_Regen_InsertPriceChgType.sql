create PROCEDURE [dbo].[EIM_Regen_InsertPriceChgType]
		@PriceChgTypeDesc varchar(20),
			@Priority smallint,
			@On_Sale bit,
			@MSRP_Required bit,
			@LineDrive bit,
			@Competetive bit,
		@PriceChgTypeID tinyint OUTPUT
	
AS 

    select  @PriceChgTypeID = ISNULL( MAX(PriceChgTypeID) + 1, 1 )
    from    PriceChgType

	-- This stored procedure can be found with all the
	-- Generated CRUD procedures for the PriceChgType table in the
	-- \EIM_Regen_CRUD_Procedures\EIM_Regen_PriceChgType_CRUD_Procs.sql file in the ItemCatalogDB project.
	-- Created By:	David Marine
	-- Created   :	Mar 21, 2007
	-- 20100422 - Dave Stacey - Split into seperate files
    -- 20120403 - Generate PriceChgTypeID because table is not defined as Identity.

	INSERT INTO PriceChgType
	(
	    [PriceChgTypeID],
		[PriceChgTypeDesc],
		[Priority],
		[On_Sale],
		[MSRP_Required],
		[LineDrive],
		[Competitive]
	)
	VALUES (
	    @PriceChgTypeID,
		@PriceChgTypeDesc,
		@Priority,
		@On_Sale,
		@MSRP_Required,
		@LineDrive,
		@Competetive
	)
	
	--PriceChgTypeID was not defined as an Identity
    --SELECT @PriceChgTypeID  = SCOPE_IDENTITY()
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EIM_Regen_InsertPriceChgType] TO [IRMAClientRole]
    AS [dbo];

