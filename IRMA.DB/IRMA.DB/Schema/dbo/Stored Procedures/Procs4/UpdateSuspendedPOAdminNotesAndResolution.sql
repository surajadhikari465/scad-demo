CREATE PROCEDURE [dbo].[UpdateSuspendedPOAdminNotesAndResolution]
	@OrderHeaderID		int,
	@OrderItemID		int,
	@Notes				varchar(5000),
	@ResolutionCodeId	int
AS 
BEGIN
	SET NOCOUNT ON

	IF @OrderHeaderID IS NOT NULL
		BEGIN
			UPDATE	OrderHeader
			SET		AdminNotes       = @Notes,
					ResolutionCodeID = @ResolutionCodeId
			WHERE	OrderHeader_ID   = @OrderHeaderID
		END
	ELSE
		BEGIN
			UPDATE	OrderItem
			SET		AdminNotes       = @Notes,
					ResolutionCodeID = @ResolutionCodeId
			WHERE	OrderItem_ID	 = @OrderItemID
		END
	
		SET NOCOUNT OFF 
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateSuspendedPOAdminNotesAndResolution] TO [IRMAClientRole]
    AS [dbo];

