IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'UpdateOrderItemQuantityShipped')
	BEGIN
		DROP  Procedure  UpdateOrderItemQuantityShipped
	END

GO

CREATE Procedure dbo.UpdateOrderItemQuantityShipped
	(
		@OrderItem_ID int,
		@QuantityShipped decimal(18,4)
	)
AS
    UPDATE OrderItem
    SET QuantityShipped =  @QuantityShipped
    WHERE OrderItem_ID = @OrderItem_ID 

GO
-- grant exec on dbo.UpdateOrderItemQuantityShipped to IRMASchedJobsRole
