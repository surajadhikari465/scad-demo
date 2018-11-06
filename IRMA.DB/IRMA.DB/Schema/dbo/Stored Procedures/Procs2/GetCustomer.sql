CREATE PROCEDURE dbo.GetCustomer 
	@CustomerID int
AS
BEGIN
    SET NOCOUNT ON

	SELECT FirstName, LastName, Phone, ISNULL(Address1, '') As Address1, ISNULL(Address2, '') As Address2, ISNULL(City, '') As City, ISNULL(State, '') As State, ISNULL(ZipCode, '') As ZipCode
    FROM Customer (NOLOCK)
    WHERE CustomerID = @CustomerID

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCustomer] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCustomer] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCustomer] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCustomer] TO [IRMAReportsRole]
    AS [dbo];

