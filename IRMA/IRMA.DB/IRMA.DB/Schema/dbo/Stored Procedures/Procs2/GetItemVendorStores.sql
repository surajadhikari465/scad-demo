CREATE PROCEDURE dbo.GetItemVendorStores 
@Item_Key int,
@Vendor_ID int 
AS 

SELECT Store.Store_Name
	,SIV.Store_No
	,Store.Zone_ID
	,SIV.PrimaryVendor
    ,Store.WFM_Store
    ,Store.Mega_Store
    ,Vendor.State
    ,dbo.fn_getCustomerType(Store.Store_No, Store.Internal, Store.BusinessUnit_ID) as CustomerType -- (3 = Regional)
FROM 
	StoreItemVendor SIV (NOLOCK)
INNER JOIN 
    Store (NOLOCK) ON Store.Store_No = SIV.Store_No
LEFT JOIN 
	Vendor (nolock) ON Store.Store_No = Vendor.Store_No

WHERE 
	SIV.Vendor_ID = @Vendor_ID 
    AND SIV.Item_Key = @Item_Key
    AND (SIV.DeleteDate is null or SIV.DeleteDate > CONVERT(datetime, CONVERT(varchar(255), GETDATE(), 101)))
order by Store.Store_Name
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemVendorStores] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemVendorStores] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemVendorStores] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemVendorStores] TO [IRMAReportsRole]
    AS [dbo];

