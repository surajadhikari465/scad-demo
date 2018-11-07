CREATE PROCEDURE dbo.UpdateOrderSentToFaxDate 
    @OrderHeader_ID int
AS

	DECLARE @CentralTimeZoneOffset int

	SELECT @CentralTimeZoneOffset = CentralTimeZoneOffset FROM Region

    UPDATE OrderHeader
    SET SentToFaxDate = DATEADD(hour, @CentralTimeZoneOffset, GETDATE())
    FROM OrderHeader (rowlock)
    WHERE OrderHeader_ID = @OrderHeader_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOrderSentToFaxDate] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOrderSentToFaxDate] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOrderSentToFaxDate] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOrderSentToFaxDate] TO [IRMAReportsRole]
    AS [dbo];

