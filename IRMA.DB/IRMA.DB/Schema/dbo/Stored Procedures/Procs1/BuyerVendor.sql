CREATE PROCEDURE dbo.BuyerVendor
AS

SELECT Vendor_ID, Vendor_Key, CompanyName 
FROM Vendor
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[BuyerVendor] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[BuyerVendor] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[BuyerVendor] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[BuyerVendor] TO [IRMAReportsRole]
    AS [dbo];

