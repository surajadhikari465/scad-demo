CREATE PROCEDURE dbo.DeleteCustReturnItem 
	@ReturnItemID int
AS
BEGIN
    SET NOCOUNT ON
    
    DELETE CustomerReturnItem
    WHERE ReturnItemID = @ReturnItemID

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteCustReturnItem] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteCustReturnItem] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteCustReturnItem] TO [IRMAReportsRole]
    AS [dbo];

