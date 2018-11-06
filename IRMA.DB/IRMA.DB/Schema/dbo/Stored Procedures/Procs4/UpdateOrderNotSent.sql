CREATE PROCEDURE dbo.UpdateOrderNotSent  
	@OrderHeader_ID int
AS
/*
	[ Modification History ]
	--------------------------------------------
	Date		Developer		TFS		Comment
	--------------------------------------------
	01/24/2011	Tom Lux			759		Added reset of POCostDate (related to lead-time), since it's based on Sent Date.
*/
BEGIN
    SET NOCOUNT ON
    
    UPDATE
		OrderHeader
    SET
		SentDate = NULL
		,POCostDate = NULL
    WHERE
		OrderHeader_ID = @OrderHeader_ID
    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOrderNotSent] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOrderNotSent] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOrderNotSent] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOrderNotSent] TO [IRMAReportsRole]
    AS [dbo];

