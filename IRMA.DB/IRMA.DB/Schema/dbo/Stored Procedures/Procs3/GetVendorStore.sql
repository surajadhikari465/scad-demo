CREATE PROCEDURE dbo.GetVendorStore 
@Vendor_ID int 
AS 

SELECT Vendor.Store_No, Distribution_Center, Manufacturer, Mega_Store, WFM_Store
FROM Vendor (NOLOCK)
INNER JOIN 
    Store (NOLOCK) ON 
    Store.Store_No = Vendor.Store_No
WHERE Vendor_ID = @Vendor_ID 
AND ((Distribution_Center = 1) OR (Manufacturer = 1) OR (Mega_Store = 1) OR (WFM_Store = 1))
AND dbo.fn_VendorType(Vendor.PS_Vendor_ID, Vendor.WFM, Vendor.Store_No, Store.Internal) = 3 -- regional
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVendorStore] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVendorStore] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVendorStore] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVendorStore] TO [IRMAReportsRole]
    AS [dbo];

