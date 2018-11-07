CREATE PROCEDURE dbo.InsertCustomerReturn 
	@CustomerID int,
    @Store_No int,
    @User_ID int,
    @ReturnDate smalldatetime,
    @Approver_ID int,
    @ReturnID int OUTPUT
AS
BEGIN
    SET NOCOUNT ON

    INSERT INTO CustomerReturn (CustomerID, Store_No, User_ID, ReturnDate, Approver_ID)
    VALUES (@CustomerID, @Store_No, @User_ID, @ReturnDate, @Approver_ID)

    SELECT @ReturnID = SCOPE_IDENTITY()

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertCustomerReturn] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertCustomerReturn] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertCustomerReturn] TO [IRMAReportsRole]
    AS [dbo];

