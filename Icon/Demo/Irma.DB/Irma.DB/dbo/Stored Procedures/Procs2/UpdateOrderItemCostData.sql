CREATE PROCEDURE dbo.UpdateOrderItemCostData
    @OrderItem_ID          INT,
    @LandingCost           MONEY,
    @Markupcost            MONEY,
    @LineItemCost          MONEY,
    @LineItemHandling      MONEY,
    @LineItemFreight       MONEY,
    @ReceivedItemCost      MONEY,
    @ReceivedItemHandling  MONEY,
    @ReceivedItemFreight   MONEY,
    @Freight3Party		   MONEY,
    @LineItemFreight3Party MONEY
AS 
BEGIN
    UPDATE OrderItem
    SET 
            LandedCost            = @LandingCost,
            Markupcost            = @Markupcost,
            LineItemCost          = @LineItemCost,
            LineItemHandling      = @LineItemHandling,
            LineItemFreight       = @LineItemFreight,
            ReceivedItemCost      = @ReceivedItemCost,
            ReceivedItemHandling  = @ReceivedItemHandling,
            ReceivedItemFreight   = @ReceivedItemFreight,
			Freight3Party		  = @Freight3Party,
			LineItemFreight3Party = @LineItemFreight3Party
    WHERE OrderItem_ID = @OrderItem_ID
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOrderItemCostData] TO [IRMAClientRole]
    AS [dbo];

