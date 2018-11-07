CREATE PROCEDURE dbo.UnlockVendor 
@Vendor_ID int 
AS 

UPDATE Vendor 
SET User_ID = NULL 
WHERE Vendor_ID = @Vendor_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UnlockVendor] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UnlockVendor] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UnlockVendor] TO [IRMAReportsRole]
    AS [dbo];

