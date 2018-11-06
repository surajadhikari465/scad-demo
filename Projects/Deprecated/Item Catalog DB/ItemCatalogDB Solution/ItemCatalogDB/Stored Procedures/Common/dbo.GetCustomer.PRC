IF EXISTS (SELECT name 
	   FROM   sysobjects 
	   WHERE  name = N'GetCustomer' 
	   AND 	  type = 'P')
    DROP PROCEDURE GetCustomer
GO

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

