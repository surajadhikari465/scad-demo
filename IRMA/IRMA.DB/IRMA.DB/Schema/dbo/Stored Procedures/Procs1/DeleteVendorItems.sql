CREATE PROCEDURE dbo.DeleteVendorItems 
@Vendor_ID int,
@DeleteDate smalldatetime
AS 
    
update ItemVendor
set DeleteDate = @DeleteDate
WHERE Vendor_ID = @Vendor_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteVendorItems] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteVendorItems] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteVendorItems] TO [IRMAReportsRole]
    AS [dbo];

