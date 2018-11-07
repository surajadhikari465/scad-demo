
CREATE PROCEDURE dbo.LoadInventoryServiceImport
    @ImportFilename varchar(200),
	@UsePSSubTeamNoForImport bit

AS

-- ****************************************************************************************************************
-- Procedure: LoadInventoryServiceImport
--    Author: unknown
--      Date: unknown
--
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 2013/07/22	BL		11623	Add the @UsePSSubTeamNoForImport variable;
-- 2013/09/26	KM		11623	Modify the #tmpHARTImport table to match HART's table spec (also matches the SO
--								version of the InventoryServiceImportLoad table);
-- 2013/10/15	KM		11623	Remove unnecessary type conversions now that the table specs are aligned;
-- 2013/11/12	KM		14373	Performance improvements for non-avg-cost regions;
-- ****************************************************************************************************************

BEGIN
	-- Debug.
	--IF OBJECT_ID('tempdb..#tmpHARTImport') IS NOT NULL DROP TABLE #tmpHARTImport
	
	DECLARE 
		@SQL	varchar(400),
		@EOP	date,
		@Period int,
		@Year	int,
		@ICVID	int
    
    SELECT @ICVID = ICVID FROM CycleCountVendor WHERE ICVABBRV = 'HART'
    
	-- Passing NULL to this function is equivalent to passing GETDATE().
    SELECT @EOP = dbo.fn_LoadExternalCycleCountEOP(NULL)

    SELECT @Period = Period, @Year = [Year] FROM [Date] (nolock) WHERE Date_Key = @EOP
	    
    -- Import the file contents to a temporary location.
    CREATE TABLE #tmpHARTImport 
	(
		REGION varchar(5),
		STORE_NAME varchar(50),
		PS_BU int,
		PS_PROD_SUBTEAM int,
		PS_PROD_DESCRIPTION varchar(100),
		UPC varchar(14),
		COUNT numeric(9,4),
		EFF_PRICE money,
		EFF_PRICE_EXTENDED money,
		FLAG varchar(1),
		COST money,
		INV_LOC_INDEX varchar(4),
		AREA varchar(4),
		SECTION varchar(4),
		DESCRIPTION varchar(30),
		SKU int
	) 

	SET @SQL = 'BULK INSERT #tmpHARTImport FROM ''' + @ImportFilename  + ''' WITH(FIRSTROW = 2, FIELDTERMINATOR = ''\t'', ROWTERMINATOR = ''\n'')'
	EXECUTE(@SQL)
	
	-- Insert the records into the load table, adding in the Period, Year, and InsertDate fields.  UPC is casted from varchar to bigint back to varchar in order to remove
	-- any leading zeroes.
	INSERT INTO InventoryServiceImportLoad (REGION,	STORE_NAME,	PS_BU, PS_PROD_SUBTEAM,	PS_PROD_DESCRIPTION, UPC, COUNT, EFF_PRICE,
											EFF_PRICE_EXTENDED,	FLAG, COST,	INV_LOC_INDEX, AREA, SECTION, DESCRIPTION, SKU, 
											ICVID, Period, Year, InsertDate)
		SELECT
			hi.REGION, hi.STORE_NAME, hi.PS_BU, hi.PS_PROD_SUBTEAM, hi.PS_PROD_DESCRIPTION, CAST(CAST(hi.UPC AS bigint) AS varchar(14)), hi.COUNT, hi.EFF_PRICE,
			hi.EFF_PRICE_EXTENDED, hi.FLAG, hi.COST, hi.INV_LOC_INDEX, hi.AREA, hi.SECTION, hi.DESCRIPTION, hi.SKU, 
			@ICVID, @Period, @Year, GETDATE()
		FROM
			#tmpHARTImport hi
	
	-- The HART files for SO will already the SKU field populated.  This update is for non-SO regions and will map the UPC back to an item key.
	UPDATE InventoryServiceImportLoad 
		SET
			SKU = ii.Item_Key
		FROM 
			InventoryServiceImportLoad				il 
			INNER JOIN ItemIdentifier	(nolock)	ii	ON	il.UPC					= ii.Identifier
														AND ii.Deleted_Identifier	= 0
														AND ii.Remove_Identifier	= 0
			INNER JOIN Item				(nolock)	i	ON	i.Item_Key		= ii.Item_Key
														AND i.Deleted_Item	= 0
														AND i.Remove_Item	= 0
		WHERE 
			IL.SKU = 0
	
	-- Non-SO regions do not map their subteams directly to the PS_PROD_SUBTEAM value, so we have to take the PS_PROD_SUBTEAM value and map it back to 
	-- the item's regional subteam number.
	DELETE FROM CycleCountExternalLoad
	INSERT INTO CycleCountExternalLoad (Store_No, Item_Key, Quantity, SubTeam_No) 
		SELECT
			s.Store_No,
			il.SKU,
			il.COUNT,
			CASE 
				WHEN @UsePSSubTeamNoForImport = 1
					THEN IL.PS_PROD_SUBTEAM
					ELSE i.SubTeam_No 
				END
		FROM 
			InventoryServiceImportLoad					il
			INNER JOIN	Store				(nolock)	s	ON s.BusinessUnit_ID	= IL.PS_BU
			LEFT JOIN	Item				(nolock)	i	ON il.SKU				= i.Item_Key
			
		WHERE 
			il.SKU		> 0 AND
			[PS_BU]		= (SELECT TOP 1 [PS_BU] FROM InventoryServiceImportLoad ORDER BY InsertDate DESC) AND
			[Period]	= (SELECT TOP 1 [Period] FROM InventoryServiceImportLoad ORDER BY InsertDate DESC) AND
			[Year]		= (SELECT TOP 1 [Year] FROM InventoryServiceImportLoad ORDER BY InsertDate DESC)			
		
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[LoadInventoryServiceImport] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[LoadInventoryServiceImport] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[LoadInventoryServiceImport] TO [IRMASchedJobsRole]
    AS [dbo];

