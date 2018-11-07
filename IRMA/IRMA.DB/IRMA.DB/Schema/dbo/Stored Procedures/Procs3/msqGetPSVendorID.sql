/****** Object:  Stored Procedure dbo.msqGetPSVendorID    Script Date: 1/27/2005 9:38:22 AM ******/
CREATE PROCEDURE dbo.msqGetPSVendorID

AS

SELECT CompanyName, PS_Vendor_ID 
FROM Vendor
WHERE PS_Vendor_ID IS NOT NULL
ORDER BY CompanyName
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[msqGetPSVendorID] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[msqGetPSVendorID] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[msqGetPSVendorID] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[msqGetPSVendorID] TO [IRMAReportsRole]
    AS [dbo];

