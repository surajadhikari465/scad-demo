CREATE    PROCEDURE dbo.TaxHosting_GetTaxOverrideForItem(@ItemKey int) AS	

	SELECT Store.Store_Name, TaxOverride.TaxFlagKey, TaxOverride.TaxFlagValue
	FROM TaxOverride, Store, Item
	WHERE TaxOverride.Store_No = Store.Store_No
		AND TaxOverride.Item_Key = Item.Item_Key		
		AND Item.Item_Key = @ItemKey
	ORDER BY Store.Store_Name, TaxOverride.TaxFlagKey
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TaxHosting_GetTaxOverrideForItem] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TaxHosting_GetTaxOverrideForItem] TO [IRMAClientRole]
    AS [dbo];

