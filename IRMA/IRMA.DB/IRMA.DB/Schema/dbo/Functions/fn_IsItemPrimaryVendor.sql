-- This function checks to see if a a vendor is primary for a specified item and store
Create  FUNCTION [dbo].[fn_IsItemPrimaryVendor] (
	@Item_Key int,
	@Store_No int,
	@Vendor_ID int
)
RETURNS bit
AS

BEGIN  
	RETURN ISNULL((SELECT PrimaryVendor FROM StoreItemVendor WHERE Item_Key = @Item_Key AND Store_No = @Store_No AND Vendor_ID = @Vendor_ID), 0)
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_IsItemPrimaryVendor] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_IsItemPrimaryVendor] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_IsItemPrimaryVendor] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_IsItemPrimaryVendor] TO [IRMAReportsRole]
    AS [dbo];

