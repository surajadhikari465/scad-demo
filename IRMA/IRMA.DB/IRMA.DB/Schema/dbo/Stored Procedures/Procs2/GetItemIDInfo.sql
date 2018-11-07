CREATE PROCEDURE dbo.GetItemIDInfo 
	@Item_Key int,
	@Vendor_ID int
AS 

SELECT 
	ItemVendor.Item_ID, 
	Vendor.CompanyName, 
	Item_Description, 
	Identifier,
    EDLP = ISNULL((SELECT TOP 1 PCT.MSRP_Required
				FROM Price P, PriceChgType PCT 
				WHERE P.Item_Key = @Item_Key
				  AND P.PriceChgTypeId = PCT.PriceChgTypeID
				ORDER BY PCT.MSRP_Required DESC), 0),
	CostedByWeight ,CatchweightRequired
	, ItemVendor.IgnoreCasePack
	, ItemVendor.RetailCasePack
	, ItemVendor.VendorItemDescription
	, ItemVendor.VendorItemStatus
	
FROM 
	ItemIdentifier (nolock) 
	INNER JOIN (Item (nolock) 
		INNER JOIN (Vendor (nolock) 
			INNER JOIN ItemVendor (rowlock) 
				ON (ItemVendor.Vendor_ID = Vendor.Vendor_ID)
       	) ON (Item.Item_Key = ItemVendor.Item_Key)
     ) ON (ItemIdentifier.Item_Key = Item.Item_Key AND ItemIdentifier.Default_Identifier = 1)
WHERE 
	ItemVendor.Item_Key = @Item_Key 
	AND ItemVendor.Vendor_ID = @Vendor_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemIDInfo] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemIDInfo] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemIDInfo] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemIDInfo] TO [IRMAReportsRole]
    AS [dbo];

