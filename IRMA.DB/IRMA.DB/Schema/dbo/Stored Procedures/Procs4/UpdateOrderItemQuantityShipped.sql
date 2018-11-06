﻿CREATE Procedure dbo.UpdateOrderItemQuantityShipped
	(
		@OrderItem_ID int,
		@QuantityShipped decimal(18,4)
	)
AS
    UPDATE OrderItem
    SET QuantityShipped =  @QuantityShipped
    WHERE OrderItem_ID = @OrderItem_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOrderItemQuantityShipped] TO [IRMAClientRole]
    AS [dbo];

