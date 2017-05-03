CREATE PROCEDURE dbo.GetVendorLockStatus 
@Vendor_ID int 
AS 

SELECT User_ID 

FROM Vendor 

WHERE Vendor_ID = @Vendor_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVendorLockStatus] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVendorLockStatus] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVendorLockStatus] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVendorLockStatus] TO [IRMAReportsRole]
    AS [dbo];

