CREATE PROCEDURE dbo.GetSuspendedPONotes
	@OrderItemID int,
	@OrderHeaderID int
AS

BEGIN
	IF @OrderHeaderID IS NOT NULL
		SELECT AdminNotes FROM OrderHeader WHERE OrderHeader_ID = @OrderHeaderID
	ELSE
		SELECT AdminNotes FROM OrderItem WHERE OrderItem_ID = @OrderItemID
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSuspendedPONotes] TO [IRMAClientRole]
    AS [dbo];

