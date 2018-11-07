CREATE PROCEDURE dbo.DeleteCustomerReturn 
	@ReturnID int
AS
BEGIN
    SET NOCOUNT ON
    
    DELETE CustomerReturn
    WHERE ReturnID = @ReturnID

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteCustomerReturn] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteCustomerReturn] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteCustomerReturn] TO [IRMAReportsRole]
    AS [dbo];

