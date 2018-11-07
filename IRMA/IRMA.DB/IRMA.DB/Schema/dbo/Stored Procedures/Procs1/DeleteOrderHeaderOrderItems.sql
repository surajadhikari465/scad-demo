CREATE PROCEDURE dbo.DeleteOrderHeaderOrderItems 
@OrderHeader_ID int 
AS 

DELETE 
FROM OrderItem
WHERE OrderHeader_ID = @OrderHeader_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteOrderHeaderOrderItems] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteOrderHeaderOrderItems] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteOrderHeaderOrderItems] TO [IRMAReportsRole]
    AS [dbo];

