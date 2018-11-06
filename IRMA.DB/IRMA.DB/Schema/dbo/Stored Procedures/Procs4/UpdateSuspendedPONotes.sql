CREATE PROCEDURE dbo.UpdateSuspendedPONotes
	@OrderItemID	int,
	@OrderHeaderID	int,
	@Notes			varchar(max)
AS

BEGIN
	IF @OrderHeaderID IS NOT NULL
		UPDATE OrderHeader SET AdminNotes = @Notes WHERE OrderHeader_ID = @OrderHeaderID
	ELSE
		UPDATE OrderItem SET AdminNotes = @Notes WHERE OrderItem_ID = @OrderItemID
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateSuspendedPONotes] TO [IRMAClientRole]
    AS [dbo];

