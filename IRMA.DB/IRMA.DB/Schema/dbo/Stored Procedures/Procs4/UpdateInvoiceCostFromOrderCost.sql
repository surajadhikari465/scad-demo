CREATE PROCEDURE dbo.UpdateInvoiceCostFromOrderCost 
@OrderHeader_ID int,
@SubTeam_No int
AS
BEGIN
	-- Update the vendor invoice data for an order to match the PO cost data.
	
	-- Calculate the cost for the order.
	DECLARE @OrderCost money, @DiscountType int, @QuantityDiscount money
    SELECT @OrderCost = ISNULL(SUM(ReceivedItemCost), 0) 
    FROM dbo.OrderItem 
    WHERE OrderHeader_ID = @OrderHeader_ID

    -- Apply overall order discount to the OrderCost
    --IF (@DiscountType IN (2,4)) SELECT @OrderCost = @OrderCost - (@OrderCost * (100 - @QuantityDiscount))

    IF (@DiscountType IN (2,4)) SELECT @OrderCost = @OrderCost - (@OrderCost * (@QuantityDiscount / 100))
    IF (@DiscountType = 1) SELECT @OrderCost = @OrderCost - @QuantityDiscount
    
    -- Update the invoice cost to match the order cost
    UPDATE dbo.OrderInvoice
	SET InvoiceCost = @OrderCost
	WHERE OrderHeader_ID = @OrderHeader_ID
		AND SubTeam_No = @SubTeam_No
 
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateInvoiceCostFromOrderCost] TO [IRMAClientRole]
    AS [dbo];

