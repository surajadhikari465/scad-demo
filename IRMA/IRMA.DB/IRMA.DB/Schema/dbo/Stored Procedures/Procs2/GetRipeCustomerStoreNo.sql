CREATE PROCEDURE dbo.GetRipeCustomerStoreNo 
@CustomerID int

AS 
BEGIN
    SET NOCOUNT ON

    SELECT DISTINCT Recipe.dbo.Customer.CompanyName, Recipe.dbo.Customer.CustomerID, Recipe.dbo.Customer.StoreNo
    FROM Recipe.dbo.Customer 
    WHERE Recipe.dbo.Customer.CustomerID = @CustomerID
    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetRipeCustomerStoreNo] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetRipeCustomerStoreNo] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetRipeCustomerStoreNo] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetRipeCustomerStoreNo] TO [IRMAReportsRole]
    AS [dbo];

