CREATE PROCEDURE dbo.BRC_GetFacilities
AS

BEGIN
    
    SELECT V.Vendor_ID, V.CompanyName
    FROM Vendor V inner join Store S on V.Store_No = S.Store_No
    WHERE 
	V.Customer = 1 and V.InternalCustomer = 1 and S.Manufacturer = 1
    ORDER BY V.CompanyName

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[BRC_GetFacilities] TO [IRMAClientRole]
    AS [dbo];

