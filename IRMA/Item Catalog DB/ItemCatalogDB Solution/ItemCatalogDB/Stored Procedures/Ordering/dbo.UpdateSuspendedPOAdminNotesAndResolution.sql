IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[UpdateSuspendedPOAdminNotesAndResolution]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[UpdateSuspendedPOAdminNotesAndResolution]
GO
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
