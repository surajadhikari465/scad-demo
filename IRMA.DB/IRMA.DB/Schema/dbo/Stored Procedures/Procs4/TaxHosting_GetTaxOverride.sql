CREATE    PROCEDURE dbo.TaxHosting_GetTaxOverride(@ItemKey int, @StoreNo int) AS	

	SELECT TaxOverride.Store_No, TaxOverride.Item_Key, TaxOverride.TaxFlagKey, TaxOverride.TaxFlagValue
	FROM TaxOverride, Store, Item
	WHERE TaxOverride.Store_No = Store.Store_No
		AND TaxOverride.Item_Key = Item.Item_Key
		AND Store.Store_No = @StoreNo
		AND Item.Item_Key = @ItemKey
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TaxHosting_GetTaxOverride] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TaxHosting_GetTaxOverride] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TaxHosting_GetTaxOverride] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TaxHosting_GetTaxOverride] TO [IRMAReportsRole]
    AS [dbo];

