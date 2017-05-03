CREATE PROCEDURE dbo.GetItemVendors
	@Item_Key int 

AS 

-- **************************************************************************
-- Procedure: GetItemVendors()
--
-- Description:
-- This procedure returns list of vendors for an item.
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 12.13.13		FA   	9251	Added Currency column in the SELECT
-- 2013-01-31	KM		9251	Change Currency join to LEFT; Add ISNULLs around Location parameters 
--								to avoid breaking the string concat with a null (x + null = null!);
-- **************************************************************************

BEGIN
	SET NOCOUNT ON

	DECLARE @CurrDate datetime

	SELECT @CurrDate = CONVERT(datetime, CONVERT(varchar(255), GETDATE(), 101))
	
	SELECT 
		ItemVendor.Vendor_ID, 
		Vendor.CompanyName, 
		(ISNULL(Vendor.City + ',', ''))+(ISNULL(' ' + Vendor.State, ''))+(ISNULL(' ' + Vendor.Zip_Code, '')) AS Location, 
		Item_ID, 
		ItemVendor.VendorItemDescription, 
		CASE WHEN ISNULL(ItemVendor.IgnoreCasePack, 0) = 1 THEN ItemVendor.RetailCasePack ELSE NULL END AS RetailCasePack, 
		StatusName AS VendorItemStatusFull, 
		StatusCode AS VendorItemStatus, 
		Item.Item_Description,
		Currency.CurrencyCode,
		Currency.CurrencyID
		
	FROM 
		Vendor								(nolock) 
		INNER JOIN	ItemVendor				(nolock)	ON Vendor.Vendor_ID				= ItemVendor.Vendor_ID
		LEFT JOIN	VendorItemStatuses VIS	(nolock)	ON ItemVendor.VendorItemStatus	= VIS.StatusID
		INNER JOIN	Item					(nolock)	ON ItemVendor.Item_Key			= Item.Item_Key
		LEFT JOIN	Currency				(nolock)	ON Currency.CurrencyID			= Vendor.CurrencyID
	
	WHERE 
		ItemVendor.Item_Key	= @Item_Key 
		AND (ItemVendor.DeleteDate IS NULL OR ItemVendor.DeleteDate > @CurrDate)
		AND (PS_Vendor_Id IS NOT NULL 
			OR Vendor.Store_No IN 
				(SELECT Store_No FROM Store WHERE Mega_Store = 1 OR Distribution_Center = 1 OR Manufacturer = 1 OR WFM_Store = 1)
			OR WFM = 1 OR InStoreManufacturedProducts = 1)
	
	ORDER BY 
		CompanyName
	
	SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemVendors] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemVendors] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemVendors] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemVendors] TO [IRMAReportsRole]
    AS [dbo];

