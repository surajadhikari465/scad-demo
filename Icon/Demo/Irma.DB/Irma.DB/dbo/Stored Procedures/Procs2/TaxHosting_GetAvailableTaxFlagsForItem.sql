CREATE   PROCEDURE dbo.TaxHosting_GetAvailableTaxFlagsForItem(@Item_Key int, @Store_No int) AS	
BEGIN
	-- THIS PROCEDURE RETURNS THE AVAILABLE TAX FLAG VALUES FOR A GIVEN ITEM/STORE COMBINATION
	-- THE RESULTS ARE THE ONLY VALID TAX FLAGS THAT CAN BE OVERRIDDEN IN THE TAX OVERRIDE SCREEN
	SELECT DISTINCT TaxFlag.TaxFlagKey
	FROM TaxFlag, TaxClass, TaxJurisdiction, Item, Store
	WHERE TaxFlag.TaxClassID = TaxClass.TaxClassID
		AND TaxFlag.TaxJurisdictionID = TaxJurisdiction.TaxJurisdictionID
		AND TaxFlag.TaxClassID = Item.TaxClassID
		AND TaxFlag.TaxJurisdictionID = Store.TaxJurisdictionID
		AND Item_Key = @Item_Key
		AND Store_No = @Store_No
	ORDER BY TaxFlag.TaxFlagKey
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TaxHosting_GetAvailableTaxFlagsForItem] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TaxHosting_GetAvailableTaxFlagsForItem] TO [IRMAClientRole]
    AS [dbo];

