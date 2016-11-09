CREATE PROCEDURE dbo.Replenishment_POSPush_GetIdentifierAdds
    @Date datetime,
    @AuditReport bit,
    @Store_No int
AS 

-- Read the tax details for each item that will be returned in the second result set
DECLARE @TaxFlagValues TABLE (Store_No int, Item_Key int, TaxFlagKey char(1), TaxFlagValue bit, TaxPercent decimal(9,4), POSID int)
DECLARE @itemKey int
DECLARE @storeNo int

-- The AuditReport logic is making this query crawl.  
IF @AuditReport = 1
BEGIN
	EXEC Replenishment_GetIdentifierAddsForAudits @Date, @Store_No
END
ELSE
BEGIN
	--Exclude SKUs from the POS/Scale Push?  (TFS 3632)
	DECLARE @ExcludeSKUIdentifiers bit
	SELECT @ExcludeSKUIdentifiers = ISNULL([dbo].[fn_InstanceDataValue] ('POSPush_ExcludeSKUIdentifiers', NULL), 0)

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
		WHERE (Mega_Store = 1 OR WFM_Store = 1)
			AND (Deleted_Item = 0 AND Remove_Item = 0 AND Add_Identifier = 1)
			AND (@ExcludeSKUIdentifiers = 0 
				OR (@ExcludeSKUIdentifiers = 1 AND ItemIdentifier.IdentifierType <> 'S'))
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
		@IdentifierAdds = 1,
		@IdentifierDeletes = 0,
		@Date = @Date,
		@AuditReport = @AuditReport,
		@Store_No = @Store_No
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_GetIdentifierAdds] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_GetIdentifierAdds] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_GetIdentifierAdds] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_GetIdentifierAdds] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_GetIdentifierAdds] TO [IRMAReportsRole]
    AS [dbo];

