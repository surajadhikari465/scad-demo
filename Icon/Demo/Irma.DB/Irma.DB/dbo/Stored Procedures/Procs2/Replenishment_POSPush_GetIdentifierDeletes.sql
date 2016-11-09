CREATE PROCEDURE [dbo].[Replenishment_POSPush_GetIdentifierDeletes]
    @Date				datetime,
    @IsScaleZoneData	bit  -- USED TO LIMIT OUTPUT TO SCALE ITEMS 
AS 
BEGIN
	-- *************************************************************************************
	-- Procedure: Replenishment_POSPush_GetIdentifierDeletes()
	--    Author: n/a
	--      Date: n/a
	--
	-- Description:
	-- Gets Identifier Delete info as part of POS Push routine
	--
	-- Modification History:
	-- Date      Init	TFS		Comment
	-- 20110824	 BBB	2866	Minor format tweak; removed select * from fn call;
	-- 20110824	 DBS	2866	Took out loop and table var for tax values for performance
	-- *************************************************************************************
	
	--**************************************************************************
	--Declare internal variables
	--**************************************************************************
	DECLARE @PluDigitsSentToScale varchar(20)
	DECLARE @UseRegionalScaleFile bit
	DECLARE @UseStoreJurisdictions int
	DECLARE @itemStoreTable table (Item_Key int, Store_No int, UNIQUE CLUSTERED (Item_Key, Store_No))
	DECLARE @ItemKey int
	DECLARE @StoreNo int

	--**************************************************************************
	--Populate internal variables
	--**************************************************************************
	--Determine how region wants to send down data to scales
	SELECT @PluDigitsSentToScale = PluDigitsSentToScale FROM InstanceData

	--Using the regional scale file?
	SELECT @UseRegionalScaleFile = (SELECT FlagValue FROM InstanceDataFlags (NOLOCK) WHERE FlagKey='UseRegionalScaleFile')

	-- Check the Store Jurisdiction Flag
	SELECT @UseStoreJurisdictions = FlagValue FROM InstanceDataFlags WHERE FlagKey = 'UseStoreJurisdictions'

	-- Read the tax details for each item that will be returned in the second result set 
	INSERT INTO @itemStoreTable(Item_Key, Store_No)
		SELECT 
				i.Item_Key, 
				p.Store_No 
		FROM 
				dbo.Price							(nolock) p
				INNER JOIN  dbo.Item				(nolock) i		ON i.Item_Key		= p.Item_Key
				INNER JOIN  dbo.ItemIdentifier		(nolock) ii		ON ii.Item_Key		= i.Item_Key
				INNER JOIN	dbo.Store				(nolock) s		ON s.Store_No		= p.Store_No
		WHERE 
				ii.Add_Identifier			= 0 
				AND ii.Remove_Identifier	= 1 
				AND (
					s.Mega_Store			= 1 
					OR 
					s.WFM_Store				= 1
					)
		GROUP BY 
				i.Item_Key, 
				p.Store_No					
		ORDER BY 
				p.Store_No, 
				i.Item_Key 

	--**************************************************************************
	-- First resultset - tax hosting details for each item contained in the second resultset
	--**************************************************************************
	SELECT 
		tf.Store_No, 
		tf.Item_Key, 
		tf.TaxFlagKey, 
		tf.TaxFlagValue, 
		tf.TaxPercent, 
		tf.POSID  
	FROM @itemStoreTable ist
		CROSS APPLY (
					SELECT 
						Store_No, 
						Item_Key, 
						TaxFlagKey, 
						TaxFlagValue, 
						TaxPercent, 
						POSID  
					FROM 
						dbo.fn_TaxFlagData(ist.Item_Key, ist.Store_No)
					) as tf
	ORDER BY 
		tf.Store_No, 
		tf.Item_Key, 
		tf.TaxFlagKey
	
	--**************************************************************************
	-- Second resultset  - list of items associated with the added identifiers and their details
	--**************************************************************************
	EXEC dbo.Dynamic_POSSearchForNonBatchedChanges
		@NewItemVal				= 0,
		@ItemChangeVal			= 0,
		@RemoveItemVal			= 1,
		@PIRUSHeaderActionVal	= 'D ',
		@Deletes				= 0,
		@IsPOSPush				= 0,
		@IsScaleZoneData		= @IsScaleZoneData,
		@POSDeAuthData			= 0,	
		@ScaleDeAuthData		= 0, 
		@ScaleAuthData			= 0,
		@IdentifierAdds			= 0,
		@IdentifierDeletes		= 1,
		@Date					= @Date,
		@AuditReport			= 0,
		@Store_No				= NULL
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_GetIdentifierDeletes] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_GetIdentifierDeletes] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_GetIdentifierDeletes] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_GetIdentifierDeletes] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_GetIdentifierDeletes] TO [IRMAReportsRole]
    AS [dbo];

