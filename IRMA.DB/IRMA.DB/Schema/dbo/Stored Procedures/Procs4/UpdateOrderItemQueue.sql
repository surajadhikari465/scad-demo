CREATE PROCEDURE dbo.UpdateOrderItemQueue 
    @OrderItemQueue_ID int,
    @Quantity decimal,
    @CreditReason_ID int
AS
BEGIN
    SET NOCOUNT ON
    
    UPDATE OrderItemQueue
    SET Quantity = @Quantity, CreditReason_ID = @CreditReason_ID, Insert_Date = GETDATE()
    WHERE OrderItemQueue_ID = @OrderItemQueue_ID 
        
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOrderItemQueue] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOrderItemQueue] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOrderItemQueue] TO [IRMAReportsRole]
    AS [dbo];

