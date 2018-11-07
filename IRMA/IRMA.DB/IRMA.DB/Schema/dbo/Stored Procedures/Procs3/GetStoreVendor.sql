CREATE PROCEDURE dbo.GetStoreVendor
@Store_No int
AS 

SELECT MAX(Vendor_ID) AS Vendor_ID
FROM Vendor (NOLOCK) 
WHERE Store_No = @Store_No
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetStoreVendor] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetStoreVendor] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetStoreVendor] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetStoreVendor] TO [IRMAReportsRole]
    AS [dbo];

