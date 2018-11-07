CREATE PROCEDURE dbo.UpdateUnreceivedOrders 
@OrderHeader_ID int
AS 

UPDATE OrderItem 
SET LineItemCost = 0,
    LineItemHandling = 0,
    LineItemFreight = 0

WHERE OrderHeader_ID = @OrderHeader_ID AND QuantityReceived = 0
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateUnreceivedOrders] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateUnreceivedOrders] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateUnreceivedOrders] TO [IRMAReportsRole]
    AS [dbo];

