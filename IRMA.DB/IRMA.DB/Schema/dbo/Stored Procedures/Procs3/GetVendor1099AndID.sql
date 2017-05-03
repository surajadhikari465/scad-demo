CREATE PROCEDURE dbo.GetVendor1099AndID 
AS 

SELECT Vendor1099_ID, Vendor1099_Desc 
FROM Vendor1099
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVendor1099AndID] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVendor1099AndID] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVendor1099AndID] TO [IRMAReportsRole]
    AS [dbo];

