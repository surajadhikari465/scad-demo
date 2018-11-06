CREATE PROCEDURE dbo.LockVendor 
@Vendor_ID int, 
@User_ID int  
AS 

UPDATE Vendor 
SET User_ID = @User_ID 
WHERE Vendor_ID = @Vendor_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[LockVendor] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[LockVendor] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[LockVendor] TO [IRMAReportsRole]
    AS [dbo];

