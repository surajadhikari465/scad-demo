CREATE PROCEDURE dbo.UpdateOrderSentDate 
	@OrderHeader_ID int
AS
/*
	[ Modification History ]
	--------------------------------------------
	Date		Developer		TFS		Comment
	--------------------------------------------
	01/24/2011	Tom Lux			759		Added set of POCostDate (related to lead-time), since it's based on Sent Date.
										Created var for Sent Date and removed @CentralTimeZoneOffset.

	--------------------------------------------
*/

	DECLARE @SentDate datetime

	SELECT @SentDate = DATEADD(hour, CentralTimeZoneOffset, GETDATE())
	FROM Region

	UPDATE
		OrderHeader
	SET
		SentDate = @SentDate
		,User_ID = NULL
		,POCostDate = DATEADD(DAY, dbo.fn_GetLeadTimeDays(Vendor_ID), @SentDate)
	FROM
		OrderHeader (rowlock)
	WHERE
		OrderHeader_ID = @OrderHeader_ID
		AND Sent = 1
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOrderSentDate] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOrderSentDate] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOrderSentDate] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOrderSentDate] TO [IRMAReportsRole]
    AS [dbo];

