CREATE PROCEDURE dbo.GetOrderReceivedCost
@OrderHeader_ID int 
AS 
BEGIN
    DECLARE @LineItemCost1 MONEY
    DECLARE @LineItemCost2 MONEY
    DECLARE @LineItemCost3 MONEY

    SET @LineItemCost1 = (SELECT ISNULL(SUM(OrderItem.ReceivedItemCost),0)
                         FROM OrderItem 
                         WHERE ((OrderItem.OrderHeader_ID = @OrderHeader_ID) AND
                                (OrderItem.DiscountType = 0) AND
                                (OrderItem.NetVendorItemDiscount = 0)))

    SET @LineItemCost2 = (SELECT (ISNULL(SUM(OrderItem.ReceivedItemCost),0))
                         FROM OrderItem 
                         WHERE ((OrderItem.OrderHeader_ID = @OrderHeader_ID) AND
                               ((OrderItem.DiscountType <> 0) OR
                                (OrderItem.NetVendorItemDiscount <> 0))))
    
    SET @LineItemCost3 = @LineItemCost1 + @LineItemCost2 

    SELECT @LineItemCost3                                AS LineItemCost,
           ISNULL(SUM(OrderItem.ReceivedItemFreight),0)  AS LineItemFreight, 
           ISNULL(SUM(OrderItem.ReceivedItemHandling),0) AS LineItemHandling
    FROM   OrderItem 
    WHERE  OrderItem.OrderHeader_ID = @OrderHeader_ID
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderReceivedCost] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderReceivedCost] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderReceivedCost] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderReceivedCost] TO [IRMAReportsRole]
    AS [dbo];

