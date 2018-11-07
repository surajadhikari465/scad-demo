CREATE PROCEDURE dbo.CheckOrderReceived
@OrderHeader_ID int
AS 

SELECT SUM(QuantityReceived) AS Received
FROM OrderItem
WHERE OrderHeader_ID = @OrderHeader_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckOrderReceived] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckOrderReceived] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckOrderReceived] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckOrderReceived] TO [IRMAReportsRole]
    AS [dbo];

