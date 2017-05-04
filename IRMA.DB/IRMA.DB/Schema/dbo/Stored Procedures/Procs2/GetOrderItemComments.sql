CREATE PROCEDURE dbo.GetOrderItemComments
@OrderItem_ID int 
AS 

SELECT Comments 
FROM OrderItem 
WHERE OrderItem_ID = @OrderItem_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderItemComments] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderItemComments] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderItemComments] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderItemComments] TO [IRMAReportsRole]
    AS [dbo];

