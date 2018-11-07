CREATE PROCEDURE dbo.[GetRetailStoresPOSPriceTax]
	@ItemKey as int
AS 
BEGIN
    SET NOCOUNT ON
    
SELECT Store_No
			,Store_Name
			,SUM(TaxRates.TaxRate) as TaxRate
			,POSPrice
			,POSSale_Price
FROM (
		--------------------------------------------
		--Select all stores and their tax rates 
		--------------------------------------------
		SELECT	Store.Store_No
			,Store_Name
			,CASE WHEN (TOV.TaxFlagValue IS NOT NULL)
				THEN CASE WHEN (TOV.TaxFlagValue = 1)
					THEN TD.TaxPercent
					ELSE 0 
					END
				ELSE CASE WHEN (TF.TaxFlagValue = 1)
					THEN TD.TaxPercent
					ELSE 0 
					END
 				END as TaxRate
			,Price.POSPrice
			,Price.POSSale_Price
		FROM
			Store (NOLOCK)
		CROSS JOIN  
			Item (NOLOCK) 
		LEFT JOIN 
			TaxFlag TF
			ON (TF.TaxClassID = Item.TaxClassID AND TF.TaxJurisdictionID = Store.TaxJurisdictionID)
		LEFT JOIN 
			TaxDefinition TD
			ON (TD.TaxJurisdictionID = TF.TaxJurisdictionID AND TD.TaxFlagKey = TF.TaxFlagKey)
		LEFT JOIN
			TaxOverride TOV
			ON TOV.Item_Key = Item.Item_Key AND TOV.Store_No = Store.Store_No AND TOV.TaxFlagKey = TF.TaxFlagKey
		left JOIN
			Price
			ON Price.Item_Key = Item.Item_Key and Price.Store_No = Store.Store_No
		WHERE
			(Mega_Store = 1 OR WFM_Store = 1) AND Item.Item_Key = @ItemKey

	UNION 
		
		-------------------------------------------------------------------------------
		-- Select any overrides that are not associated to current taxflags.
		-- ie. overrides that are not overriding existing tax flags.
		-------------------------------------------------------------------------------
		SELECT	Store.Store_No
			,Store_Name
			,TD.TaxPercent
			,Price.POSPrice
			,Price.POSSale_Price

		FROM
			Store
		CROSS JOIN  
			Item 
		RIGHT JOIN
			TaxOverride TOV
			ON (TOV.Item_Key = Item.Item_Key AND TOV.Store_No = Store.Store_No)
			AND TOV.TaxFlagKey NOT IN 
				(SELECT TaxFlagKey 
				 FROM TaxDefinition
				 WHERE TaxJurisdictionID = Store.TaxJurisdictionID)
		LEFT JOIN 
			TaxDefinition TD
			ON (TOV.TaxFlagKey = TD.TaxFlagKey AND
				TD.TaxJurisdictionID = Store.TaxJurisdictionID)
		right JOIN
			Price
			ON Price.Item_Key = Item.Item_Key and Price.Store_No = Store.Store_No

WHERE
	Item.Item_Key = @ItemKey) AS TaxRates

GROUP BY Store_Name
	 ,Store_No
	,POSPrice
	,POSSale_Price

ORDER BY Store_Name
    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetRetailStoresPOSPriceTax] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetRetailStoresPOSPriceTax] TO [IRMAClientRole]
    AS [dbo];

