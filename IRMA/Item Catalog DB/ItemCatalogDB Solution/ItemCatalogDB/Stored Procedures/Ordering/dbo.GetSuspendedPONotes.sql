IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[GetSuspendedPONotes]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[GetSuspendedPONotes]
GO

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