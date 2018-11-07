CREATE PROCEDURE dbo.GetVendor_ByCompanyNameExact
	@CompanyName varchar(50)
AS 

SELECT 
	Vendor_ID,
	Vendor_Key,
	CompanyName
FROM 
	Vendor
WHERE 
	CompanyName = @CompanyName
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVendor_ByCompanyNameExact] TO [IRMASLIMRole]
    AS [dbo];

