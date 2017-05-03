CREATE PROCEDURE dbo.GetVendorName 
@Vendor_ID int 
AS 

SELECT CompanyName 
FROM Vendor 
WHERE Vendor_ID = @Vendor_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVendorName] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVendorName] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVendorName] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVendorName] TO [IRMAReportsRole]
    AS [dbo];

