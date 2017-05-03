IF EXISTS (SELECT name 
	   FROM   sysobjects 
	   WHERE  name = N'GetCustomerSearch' 
	   AND 	  type = 'P')
    DROP PROCEDURE GetCustomerSearch
GO

CREATE PROCEDURE dbo.GetCustomerSearch 
	@FirstName varchar(255), 
	@LastName varchar(255),
    @Phone varchar(255),
    @City varchar(255),
    @State varchar(2),
    @ZipCode varchar(10)
AS
BEGIN
    SET NOCOUNT ON

	SELECT CustomerID, FirstName, LastName, ISNULL(Phone, '') As Phone, 
           ISNULL(Address1, '') As Address1, ISNULL(Address2, '')As Address2, 
           ISNULL(City, '') As City, ISNULL(State, '') As State, ISNULL(ZipCode, '') As ZipCode
    FROM Customer (NOLOCK)
    WHERE ISNULL(FirstName, '') LIKE ISNULL(@FirstName, ISNULL(FirstName, '')) + '%'
    AND ISNULL(LastName, '') LIKE ISNULL(@LastName, ISNULL(LastName, '')) + '%'
    AND ISNULL(Phone, '') = ISNULL(@Phone, ISNULL(Phone, ''))
    AND ISNULL(City, '') LIKE ISNULL(@City, ISNULL(City, '')) + '%'
    AND ISNULL(State, '') LIKE ISNULL(@State, ISNULL(State, '')) + '%'
    AND ISNULL(ZipCode, '') LIKE ISNULL(@ZipCode, ISNULL(ZipCode, '')) + '%'
    AND NOT ((@FirstName IS NULL) AND (@LastName IS NULL) AND (@Phone IS NULL) AND (@City IS NULL) AND (@State IS NULL) AND (@ZipCode IS NULL))
    

    SET NOCOUNT OFF
END
GO


