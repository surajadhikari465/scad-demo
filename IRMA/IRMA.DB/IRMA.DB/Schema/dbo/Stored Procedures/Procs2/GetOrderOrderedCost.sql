CREATE PROCEDURE dbo.GetOrderOrderedCost
@OrderHeader_ID int 
AS 
BEGIN
    DECLARE @LineItemCost1 MONEY
    DECLARE @LineItemCost2 MONEY
    DECLARE @LineItemCost3 MONEY
    DECLARE @OrderType INT

    SET @LineItemCost1 = (SELECT ISNULL(SUM(OrderItem.LineItemCost),0)
                         FROM OrderItem 
                         WHERE ((OrderItem.OrderHeader_ID = @OrderHeader_ID) AND
                                (OrderItem.DiscountType = 0) AND
                                (OrderItem.NetVendorItemDiscount = 0)))

    SET @LineItemCost2 = (SELECT (ISNULL(SUM(OrderItem.LineItemCost),0))
                         FROM OrderItem 
                         WHERE ((OrderItem.OrderHeader_ID = @OrderHeader_ID) AND
                               ((OrderItem.DiscountType <> 0) OR
                                (OrderItem.NetVendorItemDiscount <> 0))))
                                
    SET @OrderType = (SELECT OrderType_ID FROM OrderHeader WHERE OrderHeader_ID = @OrderHeader_ID)    
    
    SET @LineItemCost3 = @LineItemCost1 + @LineItemCost2
    
    SELECT @LineItemCost3 + CASE @OrderType 
								WHEN 2 THEN (ISNULL(SUM(OrderItem.HandlingCharge * OrderItem.QuantityOrdered), 0))
							ELSE
								0
							END AS LineItemCost,
           ISNULL(SUM(OrderItem.LineItemFreight),0)  AS LineItemFreight, 
           ISNULL(SUM(OrderItem.LineItemHandling),0) AS LineItemHandling
    FROM   OrderItem 
    WHERE  OrderItem.OrderHeader_ID = @OrderHeader_ID
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderOrderedCost] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderOrderedCost] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderOrderedCost] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderOrderedCost] TO [IRMAReportsRole]
    AS [dbo];

