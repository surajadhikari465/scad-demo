IF EXISTS (SELECT name 
	   FROM   sysobjects 
	   WHERE  name = N'InsertCustomerReturn' 
	   AND 	  type = 'P')
    DROP PROCEDURE InsertCustomerReturn
GO

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

