CREATE PROCEDURE dbo.[Replenishment_POSPull_InsertTempPriceAudit]
	@Store_No int 
AS
BEGIN
    DECLARE @Error_No int,
			@POSSystemId as int
    BEGIN TRAN

	SELECT @POSSystemId = (SELECT POSSystemId FROM Store 
						WHERE Store_No = @Store_No)

    DELETE FROM dbo.Temp_PriceAudit WHERE BusinessUnit_ID = (SELECT BusinessUnit_ID FROM Store WHERE Store_No = @Store_No)

    SELECT @Error_No = @@ERROR

	-- Check the Store Jurisdiction Flag
	DECLARE @UseStoreJurisdictions int
	SELECT @UseStoreJurisdictions = FlagValue FROM InstanceDataFlags WHERE FlagKey = 'UseStoreJurisdictions'
 
	-- Get the items on the POS that have different prices than IRMA

    IF @Error_No = 0
	BEGIN
	 IF @POSSystemId = 2	-- IBM
	  BEGIN
		INSERT INTO dbo.Temp_PriceAudit
		SELECT  
			ISNULL(Store.BusinessUnit_ID, POSItem.BusinessUnit_ID) AS BusinessUnit_ID, 
			Store.Store_Name, 
			Item.SubTeam_No, 
			ISNULL(ItemIdentifier.Identifier, POSItem.Identifier), 
			ISNULL(ItemOverride.Item_Description, Item.Item_Description), 
			0 AS CostUnitAmount, 
			'' AS CostUnit, 
			'' AS RetailUnit, 
			0 AS Cost, 
			dbo.fn_Price(Price.PriceChgTypeId, Price.Multiple, Price.POSPrice, Price.PricingMethod_ID, Price.Sale_Multiple, Price.POSSale_Price)
				AS Price,
			0 AS Margin,
			0 AS Cycle_Cnt_Dist, 
			0 AS Cycle_Cnt_Store,
			CASE WHEN dbo.fn_OnSale(Price.PriceChgTypeId) = 1
				 THEN CASE WHEN Price.PricingMethod_ID = 2
						   THEN Deal_Price
						   ELSE Unit_Price END
				 ELSE Unit_Price END / CASE WHEN Deal_Quantity > 0 THEN Deal_Quantity ELSE 1 
			END As POS_Unit_Price, 
			POSItem.PricingMethod_ID, 
			Price.POSSale_Price AS Sale_Price,
			dbo.fn_OnSale(Price.PriceChgTypeId),
			Price.Multiple,
			Price.Sale_Multiple 
		FROM Price (nolock)
		INNER JOIN
			Item (nolock) ON Price.Item_Key = Item.Item_Key 
				AND Item.Deleted_Item = 0 
				AND Item.Retail_Sale = 1
		INNER JOIN
			SubTeam (nolock) ON Item.SubTeam_No = Subteam.SubTeam_No
			and SubTeam.SubTeam_Name not like '%INGREDIENTS'
		INNER JOIN
			ItemIdentifier (nolock) ON Item.Item_Key = ItemIdentifier.Item_Key 
				AND ItemIdentifier.Default_Identifier = 1 
				AND ItemIdentifier.Deleted_Identifier = 0 
				AND Item.Shipper_Item = 0
				AND ItemIdentifier.Identifier <> '888888888888' -- User Report Card
		INNER JOIN
			Store (nolock) ON 
			Price.Store_No = Store.Store_No 
				AND ISNULL(@Store_No, Price.Store_No) = Price.Store_No
				AND (Store.Mega_Store = 1 OR 
						(
							(Store.WFM_Store = 1 AND Store.Store_No IN 
								(SELECT Store_No FROM StoreFTPConfig WHERE ISNULL(StoreFTPConfig.IP_Address, 'NONE') <> 'NONE')
							) 
							OR Store.Distribution_Center = 1
						) 
						AND dbo.fn_getCustomerType(Store.Store_No, Store.Internal, Store.BusinessUnit_ID) = 3
					)
		LEFT JOIN 
			ItemOverride (nolock) ON 
			Item.Item_Key = ItemOverride.Item_Key  
				AND  ItemOverride.StoreJurisdictionID = Store.StoreJurisdictionID
				AND  @UseStoreJurisdictions = 1
		FULL OUTER JOIN
			(SELECT BusinessUnit_ID, POSItem.Store_No, POSItem.Item_Key, POSItem.Identifier, 
					Unit_Price, Deal_Price, Deal_Quantity, PricingMethod_ID
			 FROM POSItem
			 INNER JOIN 
				 Store ON Store.Store_No = POSItem.Store_No
					AND ISNULL(@Store_No, POSItem.Store_No) = POSItem.Store_No
			 INNER JOIN
				 ItemIdentifier ON ItemIdentifier.Identifier = POSItem.Identifier 
					AND Default_Identifier = 1 
					AND Deleted_Identifier = 0 
			 INNER JOIN
				Item ON Item.Item_Key = POSItem.Item_Key 
					AND Item.Retail_Sale = 1
			 WHERE 
				POSItem.Identifier <> '888888888888') AS POSItem
		ON 
			POSItem.Item_Key = Price.Item_Key 
			AND POSItem.Store_No = Price.Store_No
		WHERE 
			((ABS(dbo.fn_Price(Price.PriceChgTypeId, Price.Multiple, Price.POSPrice, Price.PricingMethod_ID, Price.Sale_Multiple, Price.POSSale_Price)
				) 
			<> ABS(CASE WHEN dbo.fn_OnSale(Price.PriceChgTypeId) = 1
						   THEN CASE WHEN Price.PricingMethod_ID = 2
									 THEN Deal_Price
									 ELSE Unit_Price END
						   ELSE Unit_Price END / CASE WHEN Deal_Quantity > 0 THEN Deal_Quantity ELSE 1 END))
			)
		ORDER BY Store.BusinessUnit_ID, Item.SubTeam_No, ItemIdentifier.Identifier
	    	
		SELECT @Error_No = @@ERROR
	 END
	ELSE IF @POSSystemId = 1	-- NCR
	 BEGIN
		INSERT INTO dbo.Temp_PriceAudit
		SELECT  
			ISNULL(Store.BusinessUnit_ID, POSItem.BusinessUnit_ID) AS BusinessUnit_ID, 
			Store.Store_Name, 
			Item.SubTeam_No, 
			ISNULL(ItemIdentifier.Identifier, POSItem.Identifier), 
			ISNULL(ItemOverride.Item_Description, Item.Item_Description), 
			0 AS CostUnitAmount, 
			'' AS CostUnit, 
			'' AS RetailUnit, 
			0 AS Cost, 
			dbo.fn_Price(PriceHistoryYesterday.PriceChgTypeId, PriceHistoryYesterday.Multiple, PriceHistoryYesterday.POSPrice, PriceHistoryYesterday.PricingMethod_ID, PriceHistoryYesterday.Sale_Multiple, PriceHistoryYesterday.POSSale_Price)
				AS Price,
			0 AS Margin,
			0 AS Cycle_Cnt_Dist, 
			0 AS Cycle_Cnt_Store,
			Unit_Price As POS_Unit_Price, 
			POSItem.PricingMethod_ID, 
			PriceHistoryYesterday.POSSale_Price AS Sale_Price,
			dbo.fn_OnSale(PriceHistoryYesterday.PriceChgTypeId) as On_Sale,
			PriceHistoryYesterday.Multiple,
			PriceHistoryYesterday.Sale_Multiple 
		FROM Price (nolock) 
		INNER JOIN
			Item (nolock) ON Price.Item_Key = Item.Item_Key 
				AND Item.Deleted_Item = 0 
				AND Item.Retail_Sale = 1
		INNER JOIN
			SubTeam (nolock) ON Item.SubTeam_No = Subteam.SubTeam_No
			and SubTeam.SubTeam_Name not like '%INGREDIENTS'
		INNER JOIN
			ItemIdentifier (nolock) ON Item.Item_Key = ItemIdentifier.Item_Key 
				AND ItemIdentifier.Default_Identifier = 1 
				AND ItemIdentifier.Deleted_Identifier = 0 
				AND Item.Shipper_Item = 0
				AND ItemIdentifier.Identifier <> '888888888888' -- User Report Card
		INNER JOIN
			Store (nolock) ON 
			Price.Store_No = Store.Store_No 
				AND ISNULL(@Store_No, Price.Store_No) = Price.Store_No
				AND (Store.Mega_Store = 1 OR 
						(
							(Store.WFM_Store = 1 AND Store.Store_No IN 
								(SELECT Store_No FROM StoreFTPConfig (nolock) WHERE ISNULL(StoreFTPConfig.IP_Address, 'NONE') <> 'NONE')
							) 
							OR Store.Distribution_Center = 1
						) 
						AND dbo.fn_getCustomerType(Store.Store_No, Store.Internal, Store.BusinessUnit_ID) = 3
					)
		LEFT JOIN 
			ItemOverride (nolock) ON Item.Item_Key = ItemOverride.Item_Key  
				AND  ItemOverride.StoreJurisdictionID = Store.StoreJurisdictionID
				AND  @UseStoreJurisdictions = 1
		OUTER APPLY 
			(SELECT TOP 1 POSPrice, POSSale_Price, PricingMethod_ID, Sale_Multiple, Multiple, PriceChgTypeId
				FROM PriceHistory (nolock) PH 
				WHERE PH.Store_No = Price.Store_No AND PH.Store_No = @Store_No
					AND PH.Item_Key = Price.Item_Key 
					AND Effective_Date < CONVERT(smalldatetime, CONVERT(varchar(255), GETDATE(), 101))
				ORDER BY Effective_Date DESC) as PriceHistoryYesterday
		FULL OUTER JOIN
			(SELECT BusinessUnit_ID, POSItem.Store_No, POSItem.Item_Key, POSItem.Identifier, 
					Unit_Price, Deal_Price, Deal_Quantity, PricingMethod_ID  
			 FROM POSItem (nolock)
			 INNER JOIN 
				 Store (nolock) ON Store.Store_No = POSItem.Store_No
					AND ISNULL(@Store_No, POSItem.Store_No) = POSItem.Store_No
			 INNER JOIN
				 ItemIdentifier (nolock) ON ItemIdentifier.Identifier = POSItem.Identifier 
					AND Default_Identifier = 1 
					AND Deleted_Identifier = 0 
			 INNER JOIN
				Item (nolock) ON Item.Item_Key = POSItem.Item_Key 
					AND Item.Retail_Sale = 1
			 WHERE 
				POSItem.Identifier <> '888888888888') AS POSItem
		ON 
			POSItem.Item_Key = Price.Item_Key 
			AND POSItem.Store_No = Price.Store_No
		WHERE 
			(ABS(dbo.fn_Price(PriceHistoryYesterday.PriceChgTypeId, PriceHistoryYesterday.Multiple, PriceHistoryYesterday.POSPrice, PriceHistoryYesterday.PricingMethod_ID, PriceHistoryYesterday.Sale_Multiple, PriceHistoryYesterday.POSSale_Price)
			)
			<> 
			ABS(Unit_Price))
		ORDER BY Store.BusinessUnit_ID, Item.SubTeam_No, ItemIdentifier.Identifier

		SELECT @Error_No = @Error_No + @@ERROR
	 END

	-- Get the items in on the POS but missing in IRMA

	 INSERT INTO dbo.Temp_PriceAudit
	  SELECT 
		Store.BusinessUnit_ID, 
		Store.Store_Name, 
		NULL AS SubTeam_No, 
		POSItem.Identifier as Identifier, 
		NULL AS Item_Description, 
		0 AS CostUnitAmount, 
		'' AS CostUnit, 
		'' AS RetailUnit, 
		0 AS Cost, 
		NULL AS Price,
		0 AS Margin,
		0 AS Cycle_Cnt_Dist, 
		0 AS Cycle_Cnt_Store,
		ISNULL(Unit_Price, Deal_Price) As POS_Unit_Price,
		PricingMethod_ID, 
		NULL AS Sale_Price,
		NULL AS On_Sale,
		NULL AS Multiple,
		NULL AS Sale_Multiple 
		FROM POSItem
		 INNER JOIN 
			 Store ON Store.Store_No = POSItem.Store_No
			AND POSItem.Store_No = ISNULL(@Store_No, POSItem.Store_No)
		 WHERE POSItem.Identifier <> '888888888888'
		AND POSItem.Item_Key = -1
	 ORDER BY Store.BusinessUnit_ID, POSItem.Identifier
	
	 SELECT @Error_No = @Error_No + @@ERROR
	
	END

    IF @Error_No = 0
        COMMIT TRAN
    ELSE
    BEGIN
        ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('InsertTempPriceAudit failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPull_InsertTempPriceAudit] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPull_InsertTempPriceAudit] TO [IRMAClientRole]
    AS [dbo];

