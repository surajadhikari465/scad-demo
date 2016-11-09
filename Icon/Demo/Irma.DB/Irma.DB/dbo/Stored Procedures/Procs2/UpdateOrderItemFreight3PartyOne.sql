CREATE Procedure dbo.UpdateOrderItemFreight3PartyOne
    @OrderItem_ID int,
    @LineItemFreight3Party money,
    @UseQtyReceived bit	
	/*

	Written by Rick Kelleher, Dec 2007
	Used for updating 3rd Party Freight values 

	EXEC dbo.UpdateOrderItemFreight3PartyOne 67, 6.0, True

	grant exec on dbo.UpdateOrderItemFreight3PartyOne to IRMAClientRole
	grant exec on dbo.UpdateOrderItemFreight3PartyOne to IRMAReportsRole

	*/
AS
BEGIN
    SET NOCOUNT ON

    if @UseQtyReceived = 1
	    UPDATE OrderItem 
	    SET LineItemFreight3Party = ROUND(@LineItemFreight3Party, 2),
		    Freight3Party = 
			    CASE QuantityReceived WHEN 0
			    THEN
				    0
			    ELSE
				    ROUND(@LineItemFreight3Party / QuantityReceived, 2)
			    END
	    WHERE OrderItem_ID = @OrderItem_ID
    ELSE
	    UPDATE OrderItem 
	    SET LineItemFreight3Party = ROUND(@LineItemFreight3Party, 2),
		    Freight3Party = 
			    CASE QuantityOrdered WHEN 0
			    THEN
			        0
			    ELSE
				    ROUND(@LineItemFreight3Party / QuantityOrdered, 2)
			    END
	    WHERE OrderItem_ID = @OrderItem_ID


    SET NOCOUNT OFF
END

SET QUOTED_IDENTIFIER ON
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOrderItemFreight3PartyOne] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOrderItemFreight3PartyOne] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOrderItemFreight3PartyOne] TO [IRMAReportsRole]
    AS [dbo];

