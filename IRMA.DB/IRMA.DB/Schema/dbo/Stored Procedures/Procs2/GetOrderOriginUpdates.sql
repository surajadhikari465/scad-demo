CREATE PROCEDURE dbo.GetOrderOriginUpdates
@OrderHeader_ID int
AS 

SELECT Item_Key, MAX(Origin_ID) AS Origin_ID
FROM OrderItem
WHERE OrderHeader_ID = @OrderHeader_ID AND (Origin_ID IS NOT NULL) AND QuantityReceived > 0
GROUP BY Item_Key
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderOriginUpdates] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderOriginUpdates] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderOriginUpdates] TO [IRMAReportsRole]
    AS [dbo];

