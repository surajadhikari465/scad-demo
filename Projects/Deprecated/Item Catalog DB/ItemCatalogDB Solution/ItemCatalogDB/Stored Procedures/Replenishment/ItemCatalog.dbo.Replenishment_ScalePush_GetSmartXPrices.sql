/****** Object:  StoredProcedure [dbo].[Replenishment_ScalePush_GetSmartXPrices]    Script Date: 05/30/2007 ******/
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Replenishment_ScalePush_GetSmartXPrices]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Replenishment_ScalePush_GetSmartXPrices]
GO

/****** Object:  StoredProcedure [dbo].[Replenishment_ScalePush_GetSmartXPrices]    Script Date: 05/30/2007 ******/
CREATE PROCEDURE [dbo].[Replenishment_ScalePush_GetSmartXPrices]
    @Date datetime,
    @Deletes bit,
    @MaxBatchItems int,
    @IsScaleZoneData bit	-- USED TO LIMIT OUTPUT TO SCALE ITEMS 
AS
BEGIN
/*********************************************************************************************
CHANGE LOG
DEV		DATE		TASK	Description
----------------------------------------------------------------------------------------------
DBS		20110819	2672	Replace inside-out fn_GetScalePLU code w/fixed func call
BJL		20130514	11959	Modified the fn_GetScalePLU function to take in the Identifier. This is to ensure
							alternate scale identifiers are handled.

***********************************************************************************************/
	--Determine how region wants to send down data to scales
	DECLARE @PluDigitsSentToScale varchar(20)
	SELECT @PluDigitsSentToScale = PluDigitsSentToScale FROM InstanceData

	----------------------------------------------------------------------------------
	-- POPULATE THE @AuthDeAuthRecords TABLE WITH THE RECORDS BEING PROCESSED DURING THIS JOB
	----------------------------------------------------------------------------------
	-- Get the Authorizations and De-Authorizations that will be processed during this run of scale push.
	-- A separate table is needed so we can make sure the price change result set matches the same values that
	-- are returned in the result set of StoreItemAuthorizationID records.  We don't want these to get out of
	-- sync if the user is making updates in IRMA while we are running.
	DECLARE @AuthDeAuthRecords TABLE (
		[StoreItemAuthorizationID] [int],
		[Store_No] [int],
		[Item_Key] [int],
		[ScaleAuth] [bit],
		[ScaleDeAuth] [bit]
		)
		
	INSERT @AuthDeAuthRecords (
		[StoreItemAuthorizationID],
		[Store_No],
		[Item_Key],
		[ScaleAuth],
		[ScaleDeAuth]
		)
	SELECT 
		[StoreItemAuthorizationID],
		[Store_No],
		[Item_Key],
		[ScaleAuth],
		[ScaleDeAuth]
	FROM StoreItem WHERE 
		ISNULL(ScaleAuth,0) = 1 OR 
		ISNULL(ScaleDeAuth,0) = 1

	----------------------------------------------------------------------------------
	-- POPULATE THE TABLE @IRMAPrices WITH THE BATCHED PRICE CHANGES, AUTO DE-AUTH CHANGES
	-- AND AUTO AUTH CHANGES.
	----------------------------------------------------------------------------------
	DECLARE @IRMAPrices TABLE (
		[Item_Key] [int],
		[Identifier] [varchar](13),
		[Store_No] [int],
		[New_Item] [tinyint],
		[Price_Change] [tinyint],
		[Item_Change] [tinyint],
		[Remove_Item] [tinyint],
		[On_Sale] [bit],
		[Multiple] [tinyint],
		[Sale_Multiple] [tinyint],
		[CurrMultiple] [tinyint],
		[Price] [money],
		[Sale_Start_Date] [smalldatetime],
		[Sale_End_Date] [smalldatetime],
		[Sale_Price] [money],
		[CurrPrice] [money],
		[POSPrice] [money],
		[POSSale_Price] [money],
		[POSCurrPrice] [money],
		[MultipleWithPOSPrice] [varchar](15),
		[SaleMultipleWithPOSSalePrice] [varchar](15),
		[PricingMethod_ID] [int],
		[ScaleDept] [int],
		[ScalePLU] [varchar](5),
		[ScaleUPC] [varchar](13),
		[PLUMStoreNo] [int]
		)
		
	-- Populate the @IRMAPrices with the scale items that are currently part of the price change push files.
	-- This will be returned with a price record per item per store.
	-- This data will then be reformated as needed by SmartX, which is a single record for each item that contains
	-- the prices for all of the stores.
	INSERT @IRMAPrices (
			[Item_Key],
			[Identifier],
			[Store_No],
			[New_Item],
			[Price_Change],
			[Item_Change],
			[Remove_Item],
			[On_Sale],
			[Multiple],
			[Sale_Multiple],
			[CurrMultiple],
			[Price],
			[Sale_Start_Date],
			[Sale_End_Date],
			[Sale_Price],
			[CurrPrice],
			[POSPrice],
			[POSSale_Price],
			[POSCurrPrice],
			[MultipleWithPOSPrice],
			[SaleMultipleWithPOSSalePrice],
			[PricingMethod_ID],
			[ScaleDept],
			[ScalePLU],
			[ScaleUPC],
			[PLUMStoreNo]
			)
		SELECT 
			[Item_Key],
			[Identifier],
			[Store_No],
			[New_Item],
			[Price_Change],
			[Item_Change],
			[Remove_Item],
			[On_Sale],
			[Multiple],
			[Sale_Multiple],
			[CurrMultiple],
			[Price],
			[Sale_Start_Date],
			[Sale_End_Date],
			[Sale_Price],
			[CurrPrice],
			[POSPrice],
			[POSSale_Price],
			[POSCurrPrice],
			[MultipleWithPOSPrice],
			[SaleMultipleWithPOSSalePrice],
			[PricingMethod_ID],
			[ScaleDept],
			[ScalePLU],
			[ScaleUPC],
			[PLUMStoreNo]
		FROM [dbo].[fn_GetPriceBatchDetailPrices](@Date, @Deletes, @MaxBatchItems, @IsScaleZoneData, @PluDigitsSentToScale)

	-- Add to @IRMAPrices the scale items that have been de-authorized so they can be automatically communicated.
	INSERT @IRMAPrices (
			[Item_Key],
			[Identifier],
			[Store_No],
			[New_Item],
			[Price_Change],
			[Item_Change],
			[Remove_Item],
			[On_Sale],
			[Multiple],
			[Sale_Multiple],
			[CurrMultiple],
			[Price],
			[CurrPrice],
			[POSPrice],
			[POSSale_Price],
			[POSCurrPrice],
			[ScaleDept],
			[ScalePLU],
			[ScaleUPC],
			[PLUMStoreNo]
			)
		SELECT 
			ADR.[Item_Key],
			II.[Identifier],
			ADR.[Store_No],
			0 AS [New_Item],
			1 AS [Price_Change],
			0 AS [Item_Change],
			0 AS [Remove_Item],
			0 AS [On_Sale],
			1 AS [Multiple],		-- hard coding the multiple and setting the price to zero for de-auths
			1 AS [Sale_Multiple],
			1 AS [CurrMultiple],
			0 AS [Price],
			0 AS [CurrPrice],
			0 AS [POSPrice],
			0 AS [POSSale_Price],
			0 AS [POSCurrPrice],
			ST_Scale.ScaleDept,
			dbo.fn_GetScalePLU(II.Identifier, II.NumPluDigitsSentToScale, @PluDigitsSentToScale, 0) as ScalePLU, 
			CASE
				WHEN SUBSTRING(II.Identifier, 1, 1) = '2' -- TYPE-2 ITEM
					THEN SUBSTRING(II.Identifier, 2, 5) 
				WHEN SUBSTRING(II.Identifier,1,1) != '2' -- NON TYPE-2 ITEM
					THEN II.Identifier
				ELSE SUBSTRING(II.Identifier, 2, 5) 
			END	AS ScaleUPC,
			S.PLUMStoreNo
		FROM @AuthDeAuthRecords ADR
		INNER JOIN
			Item I (nolock)
			ON I.Item_Key = ADR.Item_Key
		INNER JOIN
			Store S (nolock)
			ON S.Store_No = ADR.Store_No
		INNER JOIN
			ItemIdentifier II (nolock)
			ON  II.Item_Key = ADR.Item_Key 
				AND II.Scale_Identifier = 1 --ONLY INSERT SCALE IDENTIFIERS THAT ARE MEANT TO BE SENT TO SCALES	
		LEFT JOIN
			SubTeam ST_Scale (nolock)
			ON ST_Scale.SubTeam_No = I.SubTeam_No  -- Scale values do not use the exception subteams  
		WHERE ADR.ScaleDeAuth = 1

	-- Add to @IRMAPrices the scale items that have been authorized so they can be automatically communicated.
	INSERT @IRMAPrices (
			[Item_Key],
			[Identifier],
			[Store_No],
			[New_Item],
			[Price_Change],
			[Item_Change],
			[Remove_Item],
			[On_Sale],
			[Multiple],
			[Sale_Multiple],
			[CurrMultiple],
			[Price],
			[Sale_Start_Date],
			[Sale_End_Date],
			[Sale_Price],
			[CurrPrice],
			[POSPrice],
			[POSSale_Price],
			[POSCurrPrice],
			[MultipleWithPOSPrice],
			[SaleMultipleWithPOSSalePrice],
			[PricingMethod_ID],
			[ScaleDept],
			[ScalePLU],
			[ScaleUPC],
			[PLUMStoreNo]
			)
		SELECT 
			ADR.[Item_Key],
			II.[Identifier],
			ADR.[Store_No],
			0 AS [New_Item],
			1 AS [Price_Change],
			0 AS [Item_Change],
			0 AS [Remove_Item],
			dbo.fn_OnSale(Price.PriceChgTypeID) As On_Sale,
			Price.Multiple,
			Price.Sale_Multiple,  
			dbo.fn_PricingMethodInt(Price.PriceChgTypeID, Price.PricingMethod_ID, Price.Multiple, Price.Sale_Multiple) AS CurrMultiple,
			ROUND(Price.Price, 2) AS Price,  -- this value will be POSPrice unless item is on sale it will be POSSale_Price
			Price.Sale_Start_Date,
			Price.Sale_End_Date, 
			ROUND(Price.Sale_Price, 2) AS Sale_Price, 
			ROUND(dbo.fn_PricingMethodMoney(Price.PriceChgTypeID, Price.PricingMethod_ID, Price.Price, Price.Sale_Price)
				,2) AS CurrPrice,
			ROUND(Price.POSPrice, 2) AS POSPrice,
			ROUND(Price.POSSale_Price, 2) AS POSSale_Price,
			ROUND(dbo.fn_PricingMethodMoney(Price.PriceChgTypeID, Price.PricingMethod_ID, Price.POSPrice, Price.POSSale_Price)
				,2) AS POSCurrPrice,		 
			CONVERT(varchar(3),Price.Multiple) + '/' + CONVERT(varchar(10),Price.POSPrice) AS  MultipleWithPOSPrice, -- always the Base Price for the Item
			CONVERT(varchar(3), dbo.fn_PricingMethodInt(Price.PriceChgTypeID, Price.PricingMethod_ID, Price.Multiple, Price.Sale_Multiple))
			+ '/' + 
			CONVERT(varchar(10), dbo.fn_PricingMethodMoney(Price.PriceChgTypeID, Price.PricingMethod_ID, Price.POSPrice, Price.POSSale_Price))
					As SaleMultipleWithPOSSalePrice,  -- the current Price for the Item 

			Price.PricingMethod_ID, 
			ST_Scale.ScaleDept,
			dbo.fn_GetScalePLU(II.Identifier, II.NumPluDigitsSentToScale, @PluDigitsSentToScale, 0) as ScalePLU, 
			CASE
				WHEN SUBSTRING(II.Identifier, 1, 1) = '2' -- TYPE-2 ITEM
					THEN SUBSTRING(II.Identifier, 2, 5) 
				WHEN SUBSTRING(II.Identifier,1,1) != '2' -- NON TYPE-2 ITEM
					THEN II.Identifier
				ELSE SUBSTRING(II.Identifier, 2, 5) 
			END	AS ScaleUPC,
			S.PLUMStoreNo
		FROM @AuthDeAuthRecords ADR
		INNER JOIN
			Item I (nolock)
			ON I.Item_Key = ADR.Item_Key
		INNER JOIN
			Store S (nolock)
			ON S.Store_No = ADR.Store_No
		INNER JOIN
			Price (nolock)
			ON Price.Item_Key = ADR.Item_Key 
			AND Price.Store_No = ADR.Store_No
		INNER JOIN
			ItemIdentifier II (nolock)
			ON  II.Item_Key = ADR.Item_Key 
				AND II.Scale_Identifier = 1 --ONLY INSERT SCALE IDENTIFIERS THAT ARE MEANT TO BE SENT TO SCALES	
		LEFT JOIN
			SubTeam ST_Scale (nolock)
			ON ST_Scale.SubTeam_No = I.SubTeam_No  -- Scale values do not use the exception subteams  
		WHERE ADR.ScaleAuth = 1

	----------------------------------------------------------------------------------
	-- TRANSLATE THE PRICES TO A RECORD PER ITEM, INSTEAD OF A RECORD PER ITEM PER STORE.
	----------------------------------------------------------------------------------
	-- Create a table of all possible Toledo Store Numbers to JOIN with IRMA Prices
	DECLARE @ToledoStores TABLE (
		[PLUMStoreNo] [int], [Item_Key] [int]
	)
	DECLARE @ToledoStoreNo int, @MinToledoStoreNo int, @MaxToledoStoreNo int 	
    SELECT @MinToledoStoreNo=1
    SELECT @MaxToledoStoreNo=99
	
    
	-- MA Needs scale price changes to trigger corressponding item changes and vice versa.
	-- The fn_GetPLUMCorpOrScalePriceChangeKeys function returns queued item change item keys 
	-- that are not already queued as price changes.  We union the returned keys with
	-- the item keys in @IRMAPrices.
	
    DECLARE populateStores CURSOR
    READ_ONLY
    FOR SELECT DISTINCT Item_Key FROM @IRMAPrices IP UNION SELECT Item_Key FROM dbo.fn_GetPLUMCorpOrScalePriceChangeKeys(@Date, 'PLUM')
	DECLARE @Item_Key int
	OPEN populateStores
    FETCH NEXT FROM populateStores INTO @Item_Key
    WHILE (@@fetch_status <> -1)
    BEGIN
    	IF (@@fetch_status <> -2)
		BEGIN
			-- Add a store mapping for each toledo store no & this item key
			SET @ToledoStoreNo=@MinToledoStoreNo
			WHILE (@ToledoStoreNo <= @MaxToledoStoreNo) 
			BEGIN
				INSERT INTO @ToledoStores (PLUMStoreNo, Item_Key) VALUES (@ToledoStoreNo, @Item_Key)
				SELECT @ToledoStoreNo = @ToledoStoreNo + 1
			END
		END
		FETCH NEXT FROM populateStores INTO @Item_Key
	END
    CLOSE populateStores
    DEALLOCATE populateStores

	-- Populate a new table that is formatted with a record per item, instead of per item per store. This
	-- record will contain a price for all possible Toledo stores, even if the store is not mapped in IRMA.
 	DECLARE @SmartXPrices TABLE (
		[Item_Key] [int],
		[Identifier] [varchar](13),
		[New_Item] [tinyint],
		[Price_Change] [tinyint],
		[Item_Change] [tinyint],
		[Remove_Item] [tinyint],
		[ZoneMultiples] [varchar](5000),
		[ZonePrices] [varchar](5000),
		[ScalePLU] [varchar](5),
		[ScaleUPC] [varchar](13)
		)

    DECLARE prices CURSOR
    READ_ONLY
    FOR     
        SELECT 			
			TS.Item_Key, 
			--ISNULL(IP.Identifier, dbo.fn_GetIdentifier(TS.Item_Key, @IsScaleZoneData)), 
			scaleIdentifiers.Identifier, 
			IP.New_Item, 
			IP.Price_Change, 
			IP.Item_Change, 
			IP.Remove_Item,
			-- FKN  - ScalePLU and ScaleUPC functions are coded based on assumption that there is one scale identifier for the 
			--	item. Pertinent logic has been taken from those functions and inserted into this query.
			ISNULL(IP.ScalePLU, dbo.fn_GetScalePLU(scaleIdentifiers.Identifier, scaleIdentifiers.NumPluDigitsSentToScale, @PluDigitsSentToScale, 0)) as ScalePLU, 
			CASE
				WHEN SUBSTRING(scaleIdentifiers.Identifier, 1, 1) = '2' -- TYPE-2 ITEM
					THEN SUBSTRING(scaleIdentifiers.Identifier, 2, 5) 
				WHEN SUBSTRING(scaleIdentifiers.Identifier,1,1) != '2' -- NON TYPE-2 ITEM
					THEN scaleIdentifiers.Identifier
				ELSE SUBSTRING(scaleIdentifiers.Identifier, 2, 5) 
			END	AS ScaleUPC,
			CASE WHEN ISNULL(SI.Authorized,0) = 1 
				 THEN ISNULL(IP.CurrMultiple, 1) 
				 ELSE 1 END
			AS CurrMultiple, 
			CASE WHEN ISNULL(SI.Authorized,0) = 1 
				THEN ISNULL(IP.POSCurrPrice, 0) 
				ELSE 0 END
			AS POSCurrPrice, 
			TS.PLUMStoreNo
        FROM @ToledoStores TS
		-- We must have an entry for all scale identifiers for all stores if an item has a change - joining this subquery
		-- makes sure there is an entry for all non-deleted Scale Identifiers for all items in the scale push.
		INNER JOIN (SELECT Item_Key, Identifier, NumPluDigitsSentToScale
					FROM ItemIdentifier ii
					WHERE Scale_Identifier = 1
					AND Deleted_Identifier = 0) scaleIdentifiers
		ON scaleIdentifiers.Item_Key = TS.Item_Key
        LEFT JOIN @IRMAPrices IP ON
			IP.PLUMStoreNo = TS.PLUMStoreNo 
			AND IP.Identifier = scaleIdentifiers.Identifier
		LEFT JOIN StoreItem SI (nolock) ON	-- Only set the prices for items that are authorized in IRMA
			SI.Item_Key = IP.Item_Key AND
			SI.Store_No = IP.Store_No 
        ORDER BY TS.Item_Key, TS.PLUMStoreNo
        
    DECLARE @Identifier varchar(13), @New_Item tinyint, @Price_Change tinyint, @Item_Change tinyint, @Remove_Item tinyint, 
			@ScalePLU varchar(5), @ScaleUPC varchar(13), @CurrMultiple tinyint, @POSCurrPrice money, @PLUMStoreNo int   
	DECLARE @IRMAStoreNo int, @IRMAPriceCount int
	OPEN prices
    FETCH NEXT FROM prices INTO @Item_Key, @Identifier, @New_Item, @Price_Change, @Item_Change, @Remove_Item, 
			@ScalePLU, @ScaleUPC, @CurrMultiple, @POSCurrPrice, @PLUMStoreNo 
		
    WHILE (@@fetch_status <> -1)
    BEGIN
    	IF (@@fetch_status <> -2)
		BEGIN
			-- Check to see if a SmartXPrices record already exists for this Item_Key.  If not,
			-- enter a new record to store the values for this item.
			IF (SELECT COUNT(0) FROM @SmartXPrices WHERE Identifier=@Identifier) = 0 
			BEGIN
				-- Initialize a SmartXPrices record for the item
				INSERT INTO @SmartXPrices
					(Item_Key, Identifier, New_Item, Price_Change, Item_Change, Remove_Item,
					 ScalePLU, ScaleUPC)
				VALUES
					(@Item_Key, @Identifier, @New_Item, @Price_Change, @Item_Change, @Remove_Item, 
					 @ScalePLU, @ScaleUPC) 
			END 
			
			-- If the Price for this item is zero, see if we have a price in IRMA we can use.
			IF @POSCurrPrice = 0 
			BEGIN
				-- Get the IRMA Store No
				SELECT @IRMAStoreNo = Store_No FROM Store WHERE PLUMStoreNo = @PLUMStoreNo
				SELECT @IRMAStoreNo = ISNULL(@IRMAStoreNo, -1)
				-- See if there is an IRMA Price for the store that is authorized.
				SELECT @IRMAPriceCount = COUNT(0) 
						FROM Price P
						INNER JOIN StoreItem SI (nolock) ON	-- Only set the prices for items that are authorized in IRMA
							SI.Item_Key = P.Item_Key AND
							SI.Store_No = P.Store_No AND
							SI.Authorized = 1
						WHERE P.Item_Key=@Item_Key AND P.Store_No=@IRMAStoreNo AND P.Price > 0
				
				-- If a Price exists for the IRMA store and the store is authorized, use it.  
				-- Otherwise, the price will be set to zero.
				IF @IRMAPriceCount > 0 
				BEGIN
					SELECT
					@CurrMultiple = CASE WHEN dbo.fn_OnSale(PriceChgTypeID) = 1 
						THEN CASE PricingMethod_ID WHEN 0 THEN Sale_Multiple
												   WHEN 1 THEN Sale_Multiple
												   WHEN 2 THEN Multiple 
												   WHEN 4 THEN Multiple END
						ELSE Multiple END,
					@POSCurrPrice = ROUND(CASE WHEN dbo.fn_OnSale(PriceChgTypeID) = 1 
							  THEN CASE PricingMethod_ID WHEN 0 THEN Sale_Price 
														 WHEN 1 THEN Sale_Price
														 WHEN 2 THEN Price
														 WHEN 4 THEN Price END
							  ELSE Price END, 2) 
					FROM Price (nolock) 
					WHERE Item_Key = @Item_Key AND Store_No = @IRMAStoreNo
				END
			END
				
			-- Update the item record with the price and multiple for this store
			UPDATE @SmartXPrices SET
				ZoneMultiples = 
						CASE WHEN ZoneMultiples IS NOT NULL THEN 
							ZoneMultiples + ',' + CONVERT(varchar, @CurrMultiple)
						ELSE
							CONVERT(varchar, @CurrMultiple) END,
				ZonePrices = 
						CASE WHEN ZonePrices IS NOT NULL THEN 
							ZonePrices + ',' + CONVERT(varchar, @POSCurrPrice)
							-- ZonePrices + ',' + RIGHT('0000000' + REPLACE(@POSCurrPrice, '.', ''), 7)
						ELSE
							CONVERT(varchar, @POSCurrPrice) END,
							-- RIGHT('0000000' + REPLACE(@POSCurrPrice, '.', ''), 7) END,
				Identifier = ISNULL(@Identifier,Identifier), 
				New_Item = ISNULL(@New_Item,New_Item), 
				Price_Change = ISNULL(@Price_Change,Price_Change), 
				Item_Change = ISNULL(@Item_Change,Item_Change), 
				Remove_Item = ISNULL(@Remove_Item,Remove_Item), 
				ScalePLU = ISNULL(@ScalePLU,ScalePLU), 
				ScaleUPC = ISNULL(@ScaleUPC,ScaleUPC) 
			WHERE Identifier = @Identifier
		END

		SELECT @IRMAStoreNo = -1
		SELECT @IRMAPriceCount = 0
    	FETCH NEXT FROM prices INTO @Item_Key, @Identifier, @New_Item, @Price_Change, @Item_Change, @Remove_Item, 
			@ScalePLU, @ScaleUPC, @CurrMultiple, @POSCurrPrice, @PLUMStoreNo
    END
    
    CLOSE prices
    DEALLOCATE prices
		
 	----------------------------------------------------------------------------------
 	-- RETURN THE SMARTX PRICE RECORDS (RESULT SET #1)
	----------------------------------------------------------------------------------
   -- SmartX Maintenance Date and Time is based on the sysdate
    DECLARE @CurrDay DateTime
    SELECT @CurrDay = GetDate()
    
    DECLARE @SmartX_PendingName AS CHAR(14)
    SELECT @SmartX_PendingName = 'PEND: ' + CONVERT(CHAR(8),@CurrDay,10)
    
    DECLARE @SmartX_MaintenanceDateTime AS CHAR(16)
    SELECT @SmartX_MaintenanceDateTime = CONVERT(CHAR(8),@CurrDay,10) + CONVERT(CHAR(8),@CurrDay,8)
	
    SELECT 
		CASE WHEN Remove_Item = 1 THEN 'Y'	-- Code for Delete 
			 WHEN New_Item = 1 THEN 'Z'		-- Code for New Item
			 ELSE 'W'						-- Code for Change; not returning price change only code 'X'
			 END AS SmartX_RecordType,
		@SmartX_MaintenanceDateTime AS SmartX_MaintenanceDateTime,
		@SmartX_PendingName As SmartX_PendingName,
		Identifier, 
		ScalePLU,
		ScaleUPC,
		ZoneMultiples,
		ZonePrices
	FROM @SmartXPrices
	
	----------------------------------------------------------------------------------
	-- RETURN THE LIST OF AUTH/DE-AUTH CHANGES PROCESSED BY THIS JOB (RESULT SET #2)
	----------------------------------------------------------------------------------
	SELECT 
		StoreItemAuthorizationID, Store_No, Item_Key, ScaleAuth, ScaleDeAuth
	FROM @AuthDeAuthRecords

END

GO
