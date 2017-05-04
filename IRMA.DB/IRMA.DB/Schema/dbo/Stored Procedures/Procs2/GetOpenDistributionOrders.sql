CREATE PROCEDURE dbo.GetOpenDistributionOrders 
@Item_Key int,
@Store_No int
AS 
SELECT OrderDate, 
       QuantityOrdered, 
       QuantityUnit
FROM Orderitem INNER JOIN OrderHeader ON (OrderItem.OrderHeader_ID = OrderHeader.OrderHeader_ID)
WHERE Item_Key = @Item_Key AND 
      CloseDate IS NULL AND 
      DateReceived IS NULL AND
      (Transfer_SubTeam IS NOT NULL) AND
      OrderHeader.ReceiveLocation_ID IN (SELECT Vendor_ID FROM Vendor WHERE Store_No = @Store_No)
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOpenDistributionOrders] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOpenDistributionOrders] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOpenDistributionOrders] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOpenDistributionOrders] TO [IRMAReportsRole]
    AS [dbo];

