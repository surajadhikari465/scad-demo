CREATE PROCEDURE dbo.DeleteOrderItemQueue 
@OrderItemQueue_ID int  
AS 

DELETE 
FROM OrderItemQueue 
WHERE OrderItemQueue_ID = @OrderItemQueue_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteOrderItemQueue] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteOrderItemQueue] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteOrderItemQueue] TO [IRMAReportsRole]
    AS [dbo];

