/****** Object:  StoredProcedure [dbo].[Replenishment_GetIdentifierAddsForAudits]    Script Date: 05/19/2006 16:32:57 ******/
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Replenishment_GetIdentifierAddsForAudits]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Replenishment_GetIdentifierAddsForAudits]
GO

/****** Object:  StoredProcedure [dbo].[Replenishment_GetIdentifierAddsForAudits]    Script Date: 05/19/2006 16:32:57 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE dbo.Replenishment_GetIdentifierAddsForAudits
    @Date datetime,
    @Store_No int
AS 

SET NOCOUNT ON

--Using the regional scale file?
DECLARE @UseRegionalScaleFile bit
SELECT @UseRegionalScaleFile = (SELECT FlagValue FROM InstanceDataFlags (NOLOCK) WHERE FlagKey='UseRegionalScaleFile')
	
-- Check the Store Jurisdiction Flag
DECLARE @UseStoreJurisdictions int
SELECT @UseStoreJurisdictions = FlagValue FROM InstanceDataFlags WHERE FlagKey = 'UseStoreJurisdictions'

-- Read the tax details for each item that will be returned in the second result set
DECLARE @TaxFlagValues TABLE (Store_No int, Item_Key int, TaxFlagKey char(1), TaxFlagValue bit, TaxPercent decimal(9,4), POSID int)
DECLARE @itemKey int
DECLARE @storeNo int

	DECLARE itemStoreCursor CURSOR
	READ_ONLY
	FOR     
		SELECT DISTINCT Item.Item_Key, Price.Store_No 
		FROM Price (nolock)
		INNER JOIN
			Item (nolock)
			ON (Item.Item_Key = Price.Item_Key)
		INNER JOIN
			ItemIdentifier (nolock)
			ON (ItemIdentifier.Item_Key = Item.Item_Key)
		INNER JOIN
			Store (nolock)
			ON (Store.Store_No = Price.Store_No)
		INNER JOIN dbo.StoreItem SI (NOLOCK) ON SI.Store_No = Price.Store_No AND SI.Item_Key = Price.Item_Key
		WHERE (Mega_Store = 1 OR WFM_Store = 1)
			AND (Store.Store_No = @Store_No) AND (Price.Price > 0 OR (Price.Price = 0 and Item.Price_Required = 1))
			AND SI.Authorized = 1
		ORDER BY Price.Store_No, Item.Item_Key 

	OPEN itemStoreCursor
	FETCH NEXT FROM itemStoreCursor INTO @itemKey, @storeNo
	WHILE (@@fetch_status <> -1)
	BEGIN
		IF (@@fetch_status <> -2)
		BEGIN
			-- Read the tax information for the current item/store combination, making sure not to include any
			-- items with a TaxOverride value set for the store..
			-- Populate the @TaxFlagValues table with the results.
			INSERT INTO @TaxFlagValues SELECT * FROM dbo.fn_TaxFlagData(@itemKey, @storeNo)
		END
		FETCH NEXT FROM itemStoreCursor INTO @itemKey, @storeNo
	END 
	CLOSE itemStoreCursor
	DEALLOCATE itemStoreCursor

 -- First resultset - tax hosting details for each item contained in the second resultset
SELECT Store_No, Item_Key, TaxFlagKey, TaxFlagValue, TaxPercent, POSID FROM @TaxFlagValues ORDER BY Store_No, Item_Key, TaxFlagKey

 -- Second  resultset  - list of items associated with the added identifiers and their details
 EXEC dbo.Dynamic_POSSearchForNonBatchedChanges
		@NewItemVal = 1,
		@ItemChangeVal = 0,
		@RemoveItemVal = 0,
		@PIRUSHeaderActionVal = 'A ',
		@Deletes = 0,
		@IsPOSPush = 0,
		@IsScaleZoneData = 0,
		@POSDeAuthData = 0,	
		@ScaleDeAuthData = 0,
		@ScaleAuthData = 0, 
		@IdentifierAdds = 0,
		@IdentifierDeletes = 0,
		@Date = @Date,
		@AuditReport = 1,
		@Store_No = @Store_No

GO
