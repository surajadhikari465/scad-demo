CREATE PROCEDURE dbo.GetVendorAndID 
AS 

SELECT Vendor_ID, CompanyName 
FROM Vendor 
ORDER BY CompanyName