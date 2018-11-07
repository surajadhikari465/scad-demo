CREATE PROCEDURE dbo.UpdateOrderItemComments
@OrderItem_ID int, 
@Comments varchar(255)
AS

UPDATE OrderItem 
SET Comments = @Comments
WHERE OrderItem_ID = @OrderItem_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOrderItemComments] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOrderItemComments] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOrderItemComments] TO [IRMAReportsRole]
    AS [dbo];

