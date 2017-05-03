CREATE PROCEDURE dbo.GetFirstVendor
AS

SELECT MIN(Vendor_ID) AS FirstVendor 

FROM Vendor
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetFirstVendor] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetFirstVendor] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetFirstVendor] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetFirstVendor] TO [IRMAReportsRole]
    AS [dbo];

