CREATE PROCEDURE dbo.VendorSearch 
@CompanyName varchar(52)

AS

BEGIN

SELECT @CompanyName = '%' + @CompanyName + '%'

select CompanyName, Vendor_ID, City, State
from Vendor 
where CompanyName LIKE @CompanyName
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[VendorSearch] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[VendorSearch] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[VendorSearch] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[VendorSearch] TO [IRMAReportsRole]
    AS [dbo];

