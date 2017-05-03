CREATE PROCEDURE dbo.GetBackOrderItems
@OrderHeader_ID int
AS

SELECT OrderItem.Item_key, (OrderItem.QuantityOrdered - OrderItem.QuantityReceived) AS Quantity, OrderItem.QuantityUnit
FROM Item INNER JOIN OrderItem ON (Item.Item_Key = OrderItem.Item_Key) 
WHERE (OrderItem.QuantityOrdered > OrderItem.QuantityReceived) AND OrderItem.OrderHeader_ID = @OrderHeader_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetBackOrderItems] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetBackOrderItems] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetBackOrderItems] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetBackOrderItems] TO [IRMAReportsRole]
    AS [dbo];

