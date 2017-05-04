CREATE PROCEDURE dbo.UpdateCustomer 
	@CustomerID int,
    @FirstName varchar(50), 
	@LastName varchar(50),
    @Phone varchar(20),
    @Address1 varchar(50),
    @Address2 varchar(50),
    @City varchar(30),
    @State varchar(2),
    @ZipCode varchar(10)
AS
BEGIN
    SET NOCOUNT ON

	UPDATE Customer
    SET FirstName = @FirstName,
        LastName = @LastName,
        Phone = @Phone,
        Address1 = @Address1,
        Address2 = @Address2,
        City = @City,
        State = @State,
        ZipCode = @ZipCode
    WHERE CustomerID = @CustomerID

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateCustomer] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateCustomer] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateCustomer] TO [IRMAReportsRole]
    AS [dbo];

