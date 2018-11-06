IF EXISTS (SELECT name 
	   FROM   sysobjects 
	   WHERE  name = N'GetCustomerReturn' 
	   AND 	  type = 'P')
    DROP PROCEDURE GetCustomerReturn
GO

CREATE PROCEDURE dbo.GetCustomerReturn 
	@ReturnID int,
    @Store_No int OUTPUT,
    @User_ID int OUTPUT,
    @ReturnDate datetime OUTPUT,
    @Approver_ID int OUTPUT
AS
BEGIN
    SET NOCOUNT ON
    
    SELECT @Store_No = Store_No, @User_ID = User_ID, @ReturnDate = ReturnDate, @Approver_ID = Approver_ID
    FROM CustomerReturn 
    WHERE ReturnID = @ReturnID

    SET NOCOUNT OFF
END

GO


