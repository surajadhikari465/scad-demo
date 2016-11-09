CREATE PROCEDURE dbo.InsertCustomer 
	@FirstName varchar(50), 
	@LastName varchar(50),
    @Phone varchar(20),
    @Address1 varchar(50),
    @Address2 varchar(50),
    @City varchar(30),
    @State varchar(2),
    @ZipCode varchar(10),
    @CustomerID int OUTPUT
AS
BEGIN
    SET NOCOUNT ON
    
    INSERT INTO Customer (FirstName,LastName,Phone,Address1,Address2,City,State,ZipCode)
    VALUES (@FirstName,@LastName,@Phone,@Address1,@Address2,@City,@State,@ZipCode)

    SELECT @CustomerID = SCOPE_IDENTITY()

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertCustomer] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertCustomer] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertCustomer] TO [IRMAReportsRole]
    AS [dbo];

